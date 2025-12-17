using Mirror;
using UnityEngine;

public class Health : NetworkBehaviour
{
    [SyncVar(hook = nameof(OnHpChanged))]
    public int currentHp;

    public int maxHp = 100;

    [SyncVar] public bool isDead;

    public override void OnStartServer()
    {
        currentHp = maxHp;
        isDead = false;

        if (RoundManager.Instance != null)
            RoundManager.Instance.RegisterPlayer(this);
    }


    [Server]
    public void TakeDamage(int amount, uint attackerNetId)
    {
        if (isDead) return;

        currentHp -= amount;
        if (currentHp <= 0)
        {
            currentHp = 0;
            Die(attackerNetId);
        }
    }

    [Server]
    void Die(uint attackerNetId)
    {
        isDead = true;

        // For demo: just disable movement collider
        RpcOnDeath();

        // Later: RoundManager will handle respawn/round end
    }

    [ClientRpc]
    void RpcOnDeath()
    {
        // Disable visuals or movement on all clients
        var mover = GetComponent<PlayerMovement1>();
        if (mover != null) mover.enabled = false;

        var cc = GetComponent<CharacterController>();
        if (cc != null) cc.enabled = false;

        // Optional: hide mesh / play animation
        // gameObject.GetComponentInChildren<Renderer>().enabled = false;
    }

    void OnHpChanged(int oldValue, int newValue)
    {
        // Later: update UI
    }
}