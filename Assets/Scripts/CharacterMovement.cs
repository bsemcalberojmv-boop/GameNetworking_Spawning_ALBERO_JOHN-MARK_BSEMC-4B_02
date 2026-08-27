using Unity.Netcode;
using UnityEngine;

public class NetworkedPlayerMovement : NetworkBehaviour
{
    public float moveSpeed = 5f;
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (!IsOwner) return;

        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(horizontalInput, 0f, verticalInput) * moveSpeed * Time.deltaTime;
        transform.Translate(movement);

        bool isMoving = movement.magnitude > 0.001f;
        animator.SetBool("IsRunning", isMoving);
    }
}