using UnityEngine;
using Unity.Netcode;

public class PlayerScore : NetworkBehaviour
{
    
    public NetworkVariable<int> Score = new NetworkVariable<int>(
        0,
        NetworkVariableReadPermission.Everyone,
        NetworkVariableWritePermission.Server
    );

    public override void OnNetworkSpawn()
    {
       
        Score.OnValueChanged += HandleScoreChanged;
    }

    public override void OnNetworkDespawn()
    {
        Score.OnValueChanged -= HandleScoreChanged;
    }

    private void HandleScoreChanged(int oldValue, int newValue)
    {
        Debug.Log($"{gameObject.name} score changed from {oldValue} to {newValue}");
       
    }

    
    public void AddScore(int amount)
    {
        if (!IsServer) return;
        Score.Value += amount;
    }
}