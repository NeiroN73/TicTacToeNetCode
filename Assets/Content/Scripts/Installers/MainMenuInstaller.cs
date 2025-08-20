using Content.Scripts.Services;
using Content.Scripts.UI.MainMenu;
using Cysharp.Threading.Tasks;
using UnityEngine;
using VContainer;

namespace Content.Scripts.Installers
{
    public class MainMenuInstaller : MonoBehaviour
    {
        [Inject] private ScreensService _screensService;
        
        private void Awake()
        {
            _screensService.OpenAsync<MainMenuScreen>().Forget();
        }
    }
}