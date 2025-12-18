using Mirror;
using UnityEngine;

public class ShopSystem : NetworkBehaviour
{
    public enum Item { Vodka, Wine, Whiskey, DamageUp, CooldownUp }

    [SyncVar] public bool vodkaUnlocked;
    [SyncVar] public bool wineUnlocked;
    [SyncVar] public bool whiskeyUnlocked;

    [SyncVar] public int damageLevel;
    [SyncVar] public int cooldownLevel;

    [Command]
    public void CmdBuy(Item item)
    {
        var stats = GetComponent<PlayerStats>();
        if (stats == null) return;

        int cost = GetCost(item);
        if (stats.points < cost) return;

        stats.points -= cost;

        switch (item)
        {
            case Item.Vodka: vodkaUnlocked = true; break;
            case Item.Wine: wineUnlocked = true; break;
            case Item.Whiskey: whiskeyUnlocked = true; break;
            case Item.DamageUp: damageLevel = Mathf.Min(damageLevel + 1, 3); break;
            case Item.CooldownUp: cooldownLevel = Mathf.Min(cooldownLevel + 1, 3); break;
        }
    }

    int GetCost(Item item)
    {
        return item switch
        {
            Item.Vodka => 15,
            Item.Wine => 20,
            Item.Whiskey => 25,
            Item.DamageUp => 10 * (damageLevel + 1),
            Item.CooldownUp => 10 * (cooldownLevel + 1),
            _ => 999
        };
    }
}