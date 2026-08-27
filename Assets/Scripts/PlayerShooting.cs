using Unity.Netcode;
using UnityEngine;

public class PlayerShooting : NetworkBehaviour
{
    public GameObject bulletPrefab;
    public Transform firePoint; 
    void Update()
    {
        if (!IsOwner) return;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            FireServerRpc();
        }
    }

    [ServerRpc]
    private void FireServerRpc()
    {
        GameObject bulletInstance = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        bulletInstance.GetComponent<NetworkObject>().SpawnWithOwnership(OwnerClientId);
    }
}