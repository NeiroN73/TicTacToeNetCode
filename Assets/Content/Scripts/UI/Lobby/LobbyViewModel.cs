using Content.Scripts.Services;
using Unity.Services.Relay.Models;
using VContainer;

namespace Content.Scripts.UI.Lobby
{
    public class LobbyViewModel : ViewModel
    {
        [Inject] private LobbiesService _lobbiesService;
        
        private readonly ViewModelBinder<string> _joinCodeString = new("joinCode");
        
        public override void Initialize()
        {
            Bind(_joinCodeString);
        }
        
        public async void Initialize(Allocation allocation)
        {
            _joinCodeString.Value = await _lobbiesService.GetJoinCodeAsync(allocation.AllocationId);
        }
    }
}