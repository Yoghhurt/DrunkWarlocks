using Mirror;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : NetworkBehaviour
{
    public float moveSpeed = 5f;

    private Rigidbody rb;
    private Camera playerCamera;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        playerCamera = GetComponentInChildren<Camera>();

        // Camera handling
        if (playerCamera != null)
        {
            if (isLocalPlayer)
                playerCamera.gameObject.SetActive(true);
            else
                playerCamera.gameObject.SetActive(false);
        }
    }

    void FixedUpdate()
    {
        if (!isLocalPlayer || !enabled)
            return;

        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(h, 0f, v) * moveSpeed;
        rb.linearVelocity = new Vector3(movement.x, 0f, movement.z);
    }
}