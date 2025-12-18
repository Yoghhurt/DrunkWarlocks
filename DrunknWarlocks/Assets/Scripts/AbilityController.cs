using Mirror;
using UnityEngine;

public class AbilityController : NetworkBehaviour
{
    public AbilityData[] allAbilities;
    public Transform firePoint;

    [SyncVar] public AbilityId equippedAbility = AbilityId.Beer;
    [SyncVar] double nextUseTime;

    void Start()
    {
        if (firePoint == null)
        {
            var fp = new GameObject("FirePoint");
            fp.transform.SetParent(transform);
            fp.transform.localPosition = new Vector3(0, 1f, 0.8f);
            firePoint = fp.transform;
        }
    }

    void Update()
    {
        if (!isLocalPlayer) return;

        if (Input.GetMouseButton(0))
            CmdTryUseAbility(transform.forward);

        // temporary testing keys
        if (Input.GetKeyDown(KeyCode.Alpha1)) CmdEquip(AbilityId.Beer);
        if (Input.GetKeyDown(KeyCode.Alpha2)) CmdEquip(AbilityId.Vodka);
        if (Input.GetKeyDown(KeyCode.Alpha3)) CmdEquip(AbilityId.Wine);
        if (Input.GetKeyDown(KeyCode.Alpha4)) CmdEquip(AbilityId.Whiskey);
    }

    AbilityData GetData(AbilityId id)
    {
        if (allAbilities == null) return null;
        foreach (var a in allAbilities)
            if (a != null && a.id == id) return a;
        return null;
    }

    [Command]
    void CmdEquip(AbilityId id)
    {
        equippedAbility = id;
    }

    [Command]
    void CmdTryUseAbility(Vector3 direction)
    {
        var data = GetData(equippedAbility);
        if (data == null) return;

        if (NetworkTime.time < nextUseTime) return;
        nextUseTime = NetworkTime.time + data.cooldown;

        if (data.isAOE)
            ServerDoAOE(data);
        else
            ServerSpawnProjectile(data, direction);
    }

    [Server]
    void ServerSpawnProjectile(AbilityData data, Vector3 direction)
    {
        if (data.projectilePrefab == null) return;

        var go = Instantiate(data.projectilePrefab, firePoint.position, Quaternion.LookRotation(direction));
        var proj = go.GetComponent<NetworkProjectile>();
        if (proj == null)
        {
            NetworkServer.Destroy(go);
            return;
        }

        proj.ServerInit(netId, direction, data.damage, data.projectileSpeed, data.projectileLifetime);
        NetworkServer.Spawn(go);
    }

    [Server]
    void ServerDoAOE(AbilityData data)
    {
        Vector3 center = firePoint.position + transform.forward * 1.5f;

        var hits = Physics.OverlapSphere(center, data.aoeRadius);
        foreach (var c in hits)
        {
            var h = c.GetComponent<Health>();
            if (h != null && h.netId != netId)
                h.TakeDamage(data.damage, netId);
        }
    }
}
