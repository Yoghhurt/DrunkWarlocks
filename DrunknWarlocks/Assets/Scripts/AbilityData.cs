using UnityEngine;

[CreateAssetMenu(menuName = "DrunknWarlocks/Ability Data")]
public class AbilityData : ScriptableObject
{
    public AbilityId id;

    [Header("Gameplay")]
    public float cooldown = 0.4f;
    public int damage = 25;

    [Header("Projectile (if used)")]
    public GameObject projectilePrefab;
    public float projectileSpeed = 18f;
    public float projectileLifetime = 3f;

    [Header("AOE (if used)")]
    public bool isAOE;
    public float aoeRadius = 3f;
}