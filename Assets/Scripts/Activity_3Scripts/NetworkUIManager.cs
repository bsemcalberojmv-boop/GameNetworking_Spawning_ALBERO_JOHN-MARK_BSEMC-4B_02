using UnityEngine;
using Unity.Netcode;

public class NetworkUIManager : MonoBehaviour
{
    public void OnHostButton()
    {
        NetworkManager.Singleton.StartHost();
    }

    public void OnClientButton()
    {
        NetworkManager.Singleton.StartClient();
    }
}