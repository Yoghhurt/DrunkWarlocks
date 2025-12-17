using Mirror;
using UnityEngine;
using System.Collections.Generic;

public class GameManager : NetworkBehaviour
{
    public static GameManager Instance;

    private readonly SyncList<uint> alivePlayers = new SyncList<uint>();

    public override void OnStartServer()
    {
        Instance = this;
        InvokeRepeating(nameof(CheckWinCondition), 1f, 1f);
    }

    public override void OnStopServer()
    {
        alivePlayers.Clear();
    }

    /* -------------------- PLAYER REGISTRATION -------------------- */

    [Server]
    public void RegisterPlayer(NetworkIdentity player)
    {
        if (!alivePlayers.Contains(player.netId))
            alivePlayers.Add(player.netId);
    }

    [Server]
    public void UnregisterPlayer(NetworkIdentity player)
    {
        if (alivePlayers.Contains(player.netId))
            alivePlayers.Remove(player.netId);
    }

    /* -------------------- WIN CHECK -------------------- */

    [Server]
    void CheckWinCondition()
    {
        if (alivePlayers.Count <= 1 && alivePlayers.Count > 0)
        {
            uint winnerNetId = alivePlayers[0];
            RpcRoundWon(winnerNetId);
            CancelInvoke(nameof(CheckWinCondition));
            Invoke(nameof(RestartRound), 3f);
        }
    }

    /* -------------------- ROUND FLOW -------------------- */

    [ClientRpc]
    void RpcRoundWon(uint winnerNetId)
    {
        if (NetworkClient.spawned.TryGetValue(winnerNetId, out var identity))
        {
            string winnerName = identity.gameObject.name;

            // Show winner in UI
            GameUIManager uiManager = FindObjectOfType<GameUIManager>();
            if (uiManager != null)
                uiManager.ShowWinner(winnerName);

            // Debug log
            Debug.Log($"{winnerName} wins the round!");
        }
    }


    [Server]
    void RestartRound()
    {
        foreach (var kvp in NetworkServer.spawned)
        {
            if (kvp.Value.TryGetComponent(out PlayerHealth health))
            {
                health.ResetPlayer();
            }
        }

        alivePlayers.Clear();

        foreach (var kvp in NetworkServer.spawned)
        {
            RegisterPlayer(kvp.Value);
        }

        player.transform.position = new Vector3(spawnX, 0.5f, spawnZ);

        InvokeRepeating(nameof(CheckWinCondition), 1f, 1f);
    }
}
