using Mirror;
using System.Collections.Generic;
using UnityEngine;

public class RoundManager : NetworkBehaviour
{
    public static RoundManager Instance;

    public float roundTime = 60f;

    [SyncVar] public double roundEndTime;
    [SyncVar] public bool roundActive;

    private readonly List<Health> players = new List<Health>();

    void Awake()
    {
        Instance = this;
    }

    public override void OnStartServer()
    {
        InvokeRepeating(nameof(ServerTick), 1f, 1f);
        StartRound();
    }

    [Server]
    public void RegisterPlayer(Health h)
    {
        if (!players.Contains(h))
            players.Add(h);
    }

    [Server]
    void StartRound()
    {
        roundActive = true;
        roundEndTime = NetworkTime.time + roundTime;

        // Reset players
        foreach (var p in players)
        {
            if (p == null) continue;
            // For now: reload scene / or do respawn later
        }

        RpcAnnounce("ROUND START!");
    }

    [Server]
    void EndRound(string reason)
    {
        roundActive = false;
        RpcAnnounce("ROUND OVER: " + reason);

        // After short delay, start again
        Invoke(nameof(StartRound), 3f);
    }

    [Server]
    void ServerTick()
    {
        if (!roundActive) return;

        if (NetworkTime.time >= roundEndTime)
        {
            EndRound("Time up!");
            return;
        }

        int aliveCount = 0;
        Health lastAlive = null;

        foreach (var p in players)
        {
            if (p == null) continue;
            if (!p.isDead)
            {
                aliveCount++;
                lastAlive = p;
            }
        }

        if (aliveCount <= 1)
        {
            if (lastAlive != null)
                EndRound("Winner: " + lastAlive.name);
            else
                EndRound("Nobody survived");
        }
    }

    [ClientRpc]
    void RpcAnnounce(string msg)
    {
        Debug.Log(msg);
        // Later: show UI banner
    }
}
