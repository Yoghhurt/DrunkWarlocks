using Mirror;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement1 : NetworkBehaviour
{
    public float moveSpeed = 6f;
    public float gravity = -20f;

    private CharacterController controller;
    private Vector3 velocity;

    void Awake()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        if (!isLocalPlayer) return;

        float x = Input.GetAxisRaw("Horizontal"); // A/D
        float z = Input.GetAxisRaw("Vertical");   // W/S

        Vector3 move = new Vector3(x, 0, z).normalized;
        controller.Move(move * moveSpeed * Time.deltaTime);

        // gravity
        if (controller.isGrounded && velocity.y < 0)
            velocity.y = -1f;

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
}