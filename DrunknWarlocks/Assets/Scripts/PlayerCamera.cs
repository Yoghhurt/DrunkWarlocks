using Mirror;
using UnityEngine;

public class PlayerCamera : NetworkBehaviour
{
    [Header("Camera Setup")]
    public Camera playerCamera;
    public Vector3 offset = new Vector3(0, 10, -8);
    public float followSpeed = 10f;

    void Start()
    {
        if (!isLocalPlayer)
        {
            // Disable camera on non-local players
            if (playerCamera != null) playerCamera.gameObject.SetActive(false);
            return;
        }

        if (playerCamera == null)
        {
            // Create a camera if you didn't assign one in prefab
            var camObj = new GameObject("LocalPlayerCamera");
            playerCamera = camObj.AddComponent<Camera>();
            camObj.tag = "MainCamera";
        }
    }

    void LateUpdate()
    {
        if (!isLocalPlayer || playerCamera == null) return;

        Vector3 targetPos = transform.position + offset;
        playerCamera.transform.position = Vector3.Lerp(
            playerCamera.transform.position,
            targetPos,
            followSpeed * Time.deltaTime
        );

        playerCamera.transform.LookAt(transform.position);
    }
}