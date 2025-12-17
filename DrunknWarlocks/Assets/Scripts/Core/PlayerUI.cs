using UnityEngine;
using UnityEngine.UI;
using Mirror;

public class PlayerUI : NetworkBehaviour
{
    public Slider drunkSlider;

    private PlayerHealth playerHealth;

    void Start()
    {
        if (!isLocalPlayer)
        {
            // Only local player controls their own UI
            Destroy(drunkSlider.transform.parent.gameObject);
            return;
        }

        playerHealth = GetComponent<PlayerHealth>();
        drunkSlider.maxValue = playerHealth.maxDrunk;
        drunkSlider.value = playerHealth.drunkLevel;
    }

    void Update()
    {
        if (!isLocalPlayer || playerHealth == null) return;

        drunkSlider.value = playerHealth.drunkLevel;
    }
}