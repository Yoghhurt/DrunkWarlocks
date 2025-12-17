using Mirror;
using UnityEngine;

public class MouseAim : NetworkBehaviour
{
    public LayerMask aimLayerMask;
    public Transform aimIndicator; // optional visual

    void Start()
    {
        if (aimLayerMask == 0)
        {
            // Default: everything
            aimLayerMask = ~0;
        }
    }

    void Update()
    {
        if (!isLocalPlayer) return;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, 200f, aimLayerMask))
        {
            Vector3 target = hit.point;
            Vector3 flat = target - transform.position;
            flat.y = 0;

            if (flat.sqrMagnitude > 0.001f)
                transform.rotation = Quaternion.LookRotation(flat);

            if (aimIndicator != null)
                aimIndicator.position = hit.point + Vector3.up * 0.05f;
        }
    }
}