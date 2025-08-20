using Content.Scripts.Services;
using Content.Scripts.UI.Loading;
using Content.Scripts.UI.Lobby;
using Content.Scripts.Utils;
using Cysharp.Threading.Tasks;
using R3;
using VContainer;

namespace Content.Scripts.UI.MainMenu
{
    public class MainMenuViewModel : ViewModel
    {
        [Inject] private AuthenticationsService _authenticationsService;
        [Inject] private LobbiesService _lobbiesService;
        [Inject] private ScreensService _screensService;
        [Inject] private ScenesService _scenesService;
        
        private readonly RefTypeViewModelBinder<ReactiveCommand<string>> _joinCodeInputField = new("joinCode");
        private readonly RefTypeViewModelBinder<ReactiveCommand> _hostButton = new("hostButton");
        private readonly RefTypeViewModelBinder<ReactiveCommand> _joinButton = new("joinButton");

        private string _joinCode;
        
        public override void Initialize()
        {
            Bind(_joinCodeInputField, _hostButton, _joinButton);

            _joinCodeInputField.Value.Subscribe(OnJoinCodeChanged).AddTo(Disposable);
            _hostButton.Value.Subscribe(OnHostedClicked).AddTo(Disposable);
            _joinButton.Value.Subscribe(OnJoinClicked).AddTo(Disposable);
        }

        private void OnJoinCodeChanged(string code)
        {
            _joinCode = code;
        }
        
        private async void OnHostedClicked()
        {
            _screensService.OpenLoading<LoadingScreen>();
            await _authenticationsService.TrySignInAnonymously();
            var allocation = await _lobbiesService.CreateLobbyAsync();
            _screensService.Close();
            var lobbyScreen = await _screensService.OpenAsync<LobbyScreen>();
            lobbyScreen.ViewModel.Initialize(allocation);
            await _scenesService.LoadSceneAsync(SceneNames.Gameplay);
            _screensService.CloseLoading();
        }
        
        private async void OnJoinClicked()
        {
            _screensService.OpenLoading<LoadingScreen>();
            await _authenticationsService.TrySignInAnonymously();
            await _lobbiesService.JoinLobbyAsync(_joinCode);
            _screensService.CloseLoading();
            _screensService.Close();
        }
    }
}