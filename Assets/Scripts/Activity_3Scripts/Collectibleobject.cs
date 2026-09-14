using UnityEngine;
using Unity.Netcode;

public class Collectibleobject : NetworkBehaviour
{
    [SerializeField] private int pointValue = 10;
    [SerializeField] private string collectibleName = "Coin";

    private void OnTriggerEnter(Collider other)
    {
        
        PlayerScore score = other.GetComponent<PlayerScore>();
        if (score == null) return;

       
        RequestCollectServerRpc(other.GetComponent<NetworkObject>().OwnerClientId);
    }

    [ServerRpc(RequireOwnership = false)]
    private void RequestCollectServerRpc(ulong collectingClientId)
    {
        
        if (!IsSpawned) return;

        
        if (!NetworkManager.Singleton.ConnectedClients.TryGetValue(collectingClientId, out var client))
            return;

        NetworkObject playerObject = client.PlayerObject;
        if (playerObject == null) return;

        PlayerScore playerScore = playerObject.GetComponent<PlayerScore>();
        if (playerScore == null) return;

        
        playerScore.AddScore(pointValue);

        
        string playerLabel = "Player " + (collectingClientId + 1);
        AnnounceCollectionClientRpc(playerLabel, collectibleName, pointValue);

        
        GetComponent<NetworkObject>().Despawn();
    }

    [ClientRpc]
    private void AnnounceCollectionClientRpc(string playerLabel, string itemName, int points)
    {
        string message = $"{playerLabel} collected a {itemName}! +{points} Points";
        Debug.Log(message);

        
        if (AnnouncementUI.Instance != null)
            AnnouncementUI.Instance.ShowAnnouncement(message);
    }
}