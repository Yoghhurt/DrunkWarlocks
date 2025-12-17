using Mirror;
using UnityEngine;

public class PlayerStats : NetworkBehaviour
{
    [SyncVar] public int points;
    [SyncVar] public int kills;

    [Server] public void AddPoints(int amount) => points += amount;
    [Server] public void AddKill() => kills++;
}