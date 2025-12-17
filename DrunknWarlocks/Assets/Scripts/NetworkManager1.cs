using Mirror;
using UnityEngine;

public class NetworkManager1 : NetworkManager
{
    [Header("Spawn Points")]
    public Transform[] spawnPoints;

    public override Transform GetStartPosition()
    {
        // Mirror will call this when adding a player
        if (spawnPoints != null && spawnPoints.Length > 0)
        {
            // Pick a random spawn for now (later: smarter)
            return spawnPoints[Random.Range(0, spawnPoints.Length)];
        }
        return base.GetStartPosition();
    }
}