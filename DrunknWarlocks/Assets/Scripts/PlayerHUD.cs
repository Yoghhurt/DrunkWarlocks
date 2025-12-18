using Mirror;
using TMPro;
using UnityEngine;

public class PlayerHUD : MonoBehaviour
{
    public TextMeshProUGUI hpText;
    public TextMeshProUGUI pointsText;

    private Health localHealth;
    private PlayerStats localStats;

    void Update()
    {
        if (NetworkClient.localPlayer == null)
        {
            if (hpText != null) hpText.enabled = false;
            if (pointsText != null) pointsText.enabled = false;
            return;
        }

        if (hpText != null) hpText.enabled = true;
        if (pointsText != null) pointsText.enabled = true;

        if (localHealth == null || localStats == null)
        {
            localHealth = NetworkClient.localPlayer.GetComponent<Health>();
            localStats = NetworkClient.localPlayer.GetComponent<PlayerStats>();
        }

        if (hpText != null && localHealth != null)
            hpText.text = $"HP: {localHealth.currentHp}";

        if (pointsText != null && localStats != null)
            pointsText.text = $"Points: {localStats.points}";
    }
}