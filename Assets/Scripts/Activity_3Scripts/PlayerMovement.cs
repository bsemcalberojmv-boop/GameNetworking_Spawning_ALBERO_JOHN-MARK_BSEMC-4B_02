using UnityEngine;
using Unity.Netcode;

public class PlayerMovement : NetworkBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        
        if (!IsOwner) return;

        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        Vector3 move = new Vector3(h, 0f, v) * moveSpeed * Time.deltaTime;

        transform.Translate(move, Space.World);
    }
}