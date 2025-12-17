using Mirror;
using UnityEngine;
using UnityEngine.InputSystem;

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
        
        if (playerCamera != null)
            playerCamera.gameObject.SetActive(isLocalPlayer);

    }

    void FixedUpdate()
    {
        if (!isLocalPlayer || !enabled) return;

        // Read keyboard input using new Input System
        Vector2 moveInput = Vector2.zero;

        if (Keyboard.current != null)
        {
            moveInput.x = (Keyboard.current.dKey.isPressed ? 1 : 0) +
                          (Keyboard.current.aKey.isPressed ? -1 : 0);
            moveInput.y = (Keyboard.current.wKey.isPressed ? 1 : 0) +
                          (Keyboard.current.sKey.isPressed ? -1 : 0);
        }

        Vector3 move = new Vector3(moveInput.x, 0f, moveInput.y);
        Vector3 newPos = rb.position + move * moveSpeed * Time.fixedDeltaTime;

        rb.MovePosition(newPos);
    }
}