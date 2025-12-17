using Mirror;
using UnityEngine;

public class BeerThrower : NetworkBehaviour
{
    public BeerProjectile beerProjectilePrefab;
    public Transform firePoint;
    public float cooldown = 0.4f;

    double nextFireTime;

    void Start()
    {
        if (firePoint == null)
        {
            // Create a firepoint in front of player if not set
            var fp = new GameObject("FirePoint");
            fp.transform.SetParent(transform);
            fp.transform.localPosition = new Vector3(0, 1f, 0.8f);
            firePoint = fp.transform;
        }
    }

    void Update()
    {
        if (!isLocalPlayer) return;

        if (Input.GetMouseButton(0) && NetworkTime.time >= nextFireTime)
        {
            nextFireTime = NetworkTime.time + cooldown;

            // Aim direction: forward for now (Day 16 improves aiming)
            Vector3 dir = transform.forward;

            CmdFire(dir);
        }
    }

    [Command]
    void CmdFire(Vector3 direction)
    {
        if (beerProjectilePrefab == null) return;

        var proj = Instantiate(
            beerProjectilePrefab.gameObject,
            firePoint.position,
            Quaternion.LookRotation(direction)
        ).GetComponent<BeerProjectile>();

        proj.ServerLaunch(direction, netId);

        NetworkServer.Spawn(proj.gameObject);
    }
}