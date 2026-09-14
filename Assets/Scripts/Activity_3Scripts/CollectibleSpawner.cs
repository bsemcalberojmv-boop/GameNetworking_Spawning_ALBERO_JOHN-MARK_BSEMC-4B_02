using UnityEngine;
using Unity.Netcode;

public class CollectibleSpawner : NetworkBehaviour
{
    [SerializeField] private GameObject collectiblePrefab;
    [SerializeField] private Transform[] spawnPoints;

    public override void OnNetworkSpawn()
    {
        if (!IsServer) return;

        foreach (Transform point in spawnPoints)
        {
            GameObject coin = Instantiate(collectiblePrefab, point.position, Quaternion.identity);
            coin.GetComponent<NetworkObject>().Spawn();
        }
    }
}