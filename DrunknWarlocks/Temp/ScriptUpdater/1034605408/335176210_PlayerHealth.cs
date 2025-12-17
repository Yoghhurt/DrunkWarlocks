using Mirror;
using UnityEngine;

public class PlayerHealth : NetworkBehaviour
{
    [SyncVar]
    public int drunkLevel;

    public int maxDrunk = 100;

    public override void OnStartServer()
    {
        drunkLevel = maxDrunk;
        GameManager.Instance.RegisterPlayer(netIdentity);
    }

    [Server]
    public void TakeDamage(int amount)
    {
        drunkLevel = Mathf.Max(drunkLevel - amount, 0);

        if (drunkLevel == 0)
        {
            RpcPassOut();
            GameManager.Instance.UnregisterPlayer(netIdentity);
        }
    }

    [ClientRpc]
    void RpcPassOut()
    {
        GetComponent<PlayerController>().enabled = false;
        GetComponent<Rigidbody>().linearVelocity = Vector3.zero;

        foreach (Renderer r in GetComponentsInChildren<Renderer>())
            r.enabled = false;
    }

    /* -------------------- RESET -------------------- */

    [Server]
    public void ResetPlayer()
    {
        drunkLevel = maxDrunk;
        RpcResetVisuals();
    }

    [ClientRpc]
    void RpcResetVisuals()
    {
        GetComponent<PlayerController>().enabled = true;

        foreach (Renderer r in GetComponentsInChildren<Renderer>())
            r.enabled = true;

        transform.position = GetSpawnPoint();
    }

    Vector3 GetSpawnPoint()
    {
        return new Vector3(
            Random.Range(-6f, 6f),
            0f,
            Random.Range(-6f, 6f)
        );
    }
}