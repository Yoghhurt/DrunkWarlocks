using Mirror;
using UnityEngine;

public class BeerProjectile : NetworkBehaviour
{
    public float speed = 15f;
    public int damage = 10;

    public override void OnStartServer()
    {
        Invoke(nameof(DestroySelf), 5f);
    }

    void FixedUpdate()
    {
        if (!isServer) return;

        transform.position += transform.forward * speed * Time.fixedDeltaTime;
    }

    [ServerCallback]
    void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out PlayerHealth health))
        {
            health.TakeDamage(damage);
            DestroySelf();
        }
    }

    [Server]
    void DestroySelf()
    {
        NetworkServer.Destroy(gameObject);
    }
}