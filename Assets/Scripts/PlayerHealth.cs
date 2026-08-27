using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : NetworkBehaviour
{
    public NetworkVariable<int> health = new NetworkVariable<int>(
        100,
        NetworkVariableReadPermission.Everyone,
        NetworkVariableWritePermission.Server
    );

    public Slider healthBar; 
    private Animator animator;
    private NetworkedPlayerMovement movementScript;

    public override void OnNetworkSpawn()
    {
        animator = GetComponent<Animator>();
        movementScript = GetComponent<NetworkedPlayerMovement>();

        if (IsOwner)
        {
            GameObject barObject = GameObject.Find("HealthBarSlider");
            if (barObject != null) healthBar = barObject.GetComponent<Slider>();
        }

        health.OnValueChanged += OnHealthChanged;
        OnHealthChanged(0, health.Value);
    }

    private void OnHealthChanged(int oldValue, int newValue)
    {
        if (healthBar != null)
        {
            healthBar.value = newValue;
        }

        if (newValue <= 0 && IsOwner)
        {
            animator.SetTrigger("Die");
            if (movementScript != null) movementScript.enabled = false;
        }
    }

    public void TakeDamage(int amount, ulong shooterId)
    {
        if (!IsServer) return;
        if (health.Value <= 0) return;

        health.Value = Mathf.Max(0, health.Value - amount);

       
        if (NetworkManager.Singleton.ConnectedClients.TryGetValue(shooterId, out var shooterClient))
        {
            PlayerScore shooterScore = shooterClient.PlayerObject.GetComponent<PlayerScore>();
            shooterScore?.AddScore(10);
        }
    }
}