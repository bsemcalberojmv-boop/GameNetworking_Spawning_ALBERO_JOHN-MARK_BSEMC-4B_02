using Unity.Netcode;
using UnityEngine;

public class Bullet : NetworkBehaviour
{
    public float speed = 20f;
    public int damage = 10;
    public float lifetime = 3f;

    private float timer;

    void Update()
    {
        if (!IsServer) return; 

        transform.Translate(Vector3.forward * speed * Time.deltaTime);

        timer += Time.deltaTime;
        if (timer >= lifetime)
        {
            GetComponent<NetworkObject>().Despawn();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!IsServer) return;

        PlayerHealth targetHealth = other.GetComponent<PlayerHealth>();
        if (targetHealth != null && other.GetComponent<NetworkObject>().OwnerClientId != OwnerClientId)
        {
            targetHealth.TakeDamage(damage, OwnerClientId);
            GetComponent<NetworkObject>().Despawn();
        }
    }
}