using Unity.Netcode;

public class PlayerScore : NetworkBehaviour
{
    public NetworkVariable<int> score = new NetworkVariable<int>(
        0,
        NetworkVariableReadPermission.Everyone,
        NetworkVariableWritePermission.Server
    );

    public void AddScore(int amount)
    {
        if (!IsServer) return;
        score.Value += amount;
    }
}