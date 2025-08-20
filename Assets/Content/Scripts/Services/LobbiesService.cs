using System;
using System.Linq;
using Cysharp.Threading.Tasks;
using Game.Scripts.Utils;
using R3;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;

namespace Content.Scripts.Services
{
    public class LobbiesService
    {
        private readonly Subject _connected = new();
        public IObservable Connected => _connected;

        public async UniTask<Allocation> CreateLobbyAsync()
        {
            NetworkManager.Singleton.NetworkConfig.ConnectionApproval = true;
            NetworkManager.Singleton.ConnectionApprovalCallback = ConnectionApproval;
            var allocation = await RelayService.Instance.CreateAllocationAsync(4);
            var joinCode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);

            var endpoint = allocation.ServerEndpoints.First(c => c.ConnectionType == "dtls");

            NetworkManager.Singleton.GetComponent<UnityTransport>().SetHostRelayData(endpoint.Host,
                (ushort)endpoint.Port, allocation.AllocationIdBytes,
                allocation.Key, allocation.ConnectionData, true);

            NetworkManager.Singleton.StartHost();

            _connected.OnNext();

            return allocation;
        }

        public async UniTask JoinLobbyAsync(string joinCode)
        {
            var allocation = await RelayService.Instance.JoinAllocationAsync(joinCode);
            var endpoint = allocation.ServerEndpoints.First(c => c.ConnectionType == "dtls");

            NetworkManager.Singleton.GetComponent<UnityTransport>().SetClientRelayData(endpoint.Host,
                (ushort)endpoint.Port, allocation.AllocationIdBytes,
                allocation.Key, allocation.ConnectionData, allocation.HostConnectionData, true);

            NetworkManager.Singleton.StartClient();

            _connected.OnNext();
        }

        public async UniTask<string> GetJoinCodeAsync(Guid allocationGuid)
        {
            return await RelayService.Instance.GetJoinCodeAsync(allocationGuid);
        }

        public void LeaveLobby()
        {
            NetworkManager.Singleton.Shutdown();
        }

        private void ConnectionApproval(NetworkManager.ConnectionApprovalRequest request,
            NetworkManager.ConnectionApprovalResponse response)
        {
            response.Approved = true;
            response.CreatePlayerObject = true;
            response.Pending = false;
        }
    }
}