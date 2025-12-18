using Mirror;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class NetworkProjectile : NetworkBehaviour
{
    [SyncVar] public uint ownerNetId;

    Rigidbody rb;

    int damage;
    float lifeTime;

    public override void OnStartServer()
    {
        rb = GetComponent<Rigidbody>();
    }

    [Server]
    public void ServerInit(uint owner, Vector3 direction, int dmg, float speed, float lifetime)
    {
        ownerNetId = owner;
        damage = dmg;
        lifeTime = lifetime;

        rb = GetComponent<Rigidbody>();
        rb.linearVelocity = direction.normalized * speed;

        Invoke(nameof(ServerDespawn), lifeTime);
    }

    [ServerCallback]
    void OnTriggerEnter(Collider other)
    {
        var health = other.GetComponent<Health>();

        if (health != null && health.netId != ownerNetId)
        {
            health.TakeDamage(damage, ownerNetId);
            ServerDespawn();
        }
    }

    [Server]
    void ServerDespawn()
    {
        if (gameObject != null)
            NetworkServer.Destroy(gameObject);
    }
}