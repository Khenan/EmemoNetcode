using Unity.Netcode;
using UnityEngine;

public class Player : NetworkBehaviour {
    
    public NetworkVariable<Vector3> Position = new NetworkVariable<Vector3>();

    public override void OnNetworkSpawn()
    {
        if (IsOwner)
        {
            Move();
        }
    }

    public void Move()
    {
        if(NetworkManager.Singleton.IsServer) {
            Vector3 _randomPosition = new Vector3(Random.Range(-10, 10), 0, Random.Range(-10, 10));
            transform.position = _randomPosition;
            Position.Value = _randomPosition;
        }
        else {
            SubmitPositionServerRpc();
        }
    }

    [ServerRpc]
    private void SubmitPositionServerRpc(ServerRpcParams _rpcParams = default)
    {
        Position.Value = new Vector3(Random.Range(-10, 10), 0, Random.Range(-10, 10));
    }
}
