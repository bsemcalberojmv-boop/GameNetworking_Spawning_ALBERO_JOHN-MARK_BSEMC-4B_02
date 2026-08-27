using Unity.Mathematics;
using Unity.Netcode;
using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    [SerializeField] public GameObject Playerprefab;

    void Start()
    {
        NetworkManager.Singleton.OnClientConnectedCallback += SpawnPlayer;
    }

    void OnDestroy()
    {
        if (NetworkManager.Singleton != null)
        {
            NetworkManager.Singleton.OnClientConnectedCallback -= SpawnPlayer;
        }
    }

    private void SpawnPlayer(ulong clientId)
    {
        if (!NetworkManager.Singleton.IsServer) return;

        Vector3 spawnPosition = new Vector3(clientId * 2f, 0, 0);
        
        GameObject playerInstance = Instantiate(Playerprefab, spawnPosition, quaternion.identity);
       
        playerInstance.GetComponent<NetworkObject>().SpawnAsPlayerObject(clientId);
    }
}