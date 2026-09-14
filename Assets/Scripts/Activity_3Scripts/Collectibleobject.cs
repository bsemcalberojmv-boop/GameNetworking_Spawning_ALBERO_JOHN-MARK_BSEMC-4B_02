using UnityEngine;
using Unity.Netcode;

public class Collectibleobject : NetworkBehaviour
{
    [SerializeField] private int pointValue = 10;
    [SerializeField] private string collectibleName = "Coin";

    private void OnTriggerEnter(Collider other)
    {
        // Only the client that touched it needs to *request* collection.
        // The actual score change and despawn must happen on the server.
        PlayerScore score = other.GetComponent<PlayerScore>();
        if (score == null) return;

        // Ask the server to process this collection.
        RequestCollectServerRpc(other.GetComponent<NetworkObject>().OwnerClientId);
    }

    [ServerRpc(RequireOwnership = false)]
    private void RequestCollectServerRpc(ulong collectingClientId)
    {
        // Server-side guard: if another player already triggered this
        // in the same frame, the NetworkObject may already be despawned.
        if (!IsSpawned) return;

        // Find the collecting player's NetworkObject on the server.
        if (!NetworkManager.Singleton.ConnectedClients.TryGetValue(collectingClientId, out var client))
            return;

        NetworkObject playerObject = client.PlayerObject;
        if (playerObject == null) return;

        PlayerScore playerScore = playerObject.GetComponent<PlayerScore>();
        if (playerScore == null) return;

        // 1. Update the score (server-authoritative NetworkVariable).
        playerScore.AddScore(pointValue);

        // 2. Announce the collection to everyone.
        string playerLabel = "Player " + (collectingClientId + 1);
        AnnounceCollectionClientRpc(playerLabel, collectibleName, pointValue);

        // 3. Remove the collectible so it can't be collected twice.
        GetComponent<NetworkObject>().Despawn();
    }

    [ClientRpc]
    private void AnnounceCollectionClientRpc(string playerLabel, string itemName, int points)
    {
        string message = $"{playerLabel} collected a {itemName}! +{points} Points";
        Debug.Log(message);

        // Route this to your UI announcer if you have one:
        //if (AnnouncementUI.Instance != null)
           // AnnouncementUI.Instance.ShowAnnouncement(message);
    }
}