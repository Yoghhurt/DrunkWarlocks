using Mirror;
using UnityEngine;

using Mirror;
using UnityEngine;

public class PlayerController : NetworkBehaviour
{
    public float moveSpeed = 5f;
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (!isLocalPlayer)
        {
            GetComponentInChildren<Camera>().gameObject.SetActive(false);
        }
    }

    void FixedUpdate()
    {
        if (!isLocalPlayer || !enabled) return;

        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        Vector3 move = new Vector3(h, 0, v) * moveSpeed;
        rb.linearVelocity = new Vector3(move.x, 0, move.z);
    }
}


