using Unity.Netcode;
using UnityEngine;

public class NetworkButton : MonoBehaviour
{
    private enum NetworkType
    {
        Host,
        Client,
        Server
    }
    [SerializeField] private NetworkType networkType;
    private void Start()
    {
        GetComponent<UnityEngine.UI.Button>().onClick.AddListener(OnClick);
    }
    public void OnClick()
    {
        switch (networkType)
        {
            case NetworkType.Host:
                NetworkManager.Singleton.StartHost();
                break;
            case NetworkType.Client:
                NetworkManager.Singleton.StartClient();
                break;
            case NetworkType.Server:
                NetworkManager.Singleton.StartServer();
                break;
        }
    }
}
