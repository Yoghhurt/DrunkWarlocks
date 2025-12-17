using Mirror;
using UnityEngine;

public class BeerProjectile : NetworkBehaviour
{
    [Header("Stats")]
    public float speed = 18f;
    public int damage = 25;
    public float lifeTime = 3f;

    [SyncVar] public uint ownerNetId;

    Rigidbody rb;

    public override void OnStartServer()
    {
        rb = GetComponent<Rigidbody>();
        Invoke(nameof(ServerSelfDestruct), lifeTime);
    }

    [Server]
    public void ServerLaunch(Vector3 direction, uint owner)
    {
        ownerNetId = owner;
        rb = GetComponent<Rigidbody>();
        rb.linearVelocity = direction.normalized * speed;
    }

    [ServerCallback]
    void OnTriggerEnter(Collider other)
    {
        // If you use non-trigger collider, change to OnCollisionEnter
        var health = other.GetComponent<Health>();
        if (health != null && health.netId != ownerNetId)
        {
            health.TakeDamage(damage, ownerNetId);
            ServerSelfDestruct();
        }
        else
        {
            // Optionally collide with walls too
            // If other is environment layer -> destroy
        }
    }

    [Server]
    void ServerSelfDestruct()
    {
        NetworkServer.Destroy(gameObject);
    }
}