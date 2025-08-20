using Content.Scripts.Services;
using UnityEngine;
using VContainer;

namespace Content.Scripts.Installers
{
    public class EntryPointInstaller : MonoBehaviour
    {
        [Inject] private ScenesService _scenesService;
        
        private async void Awake()
        {
            await _scenesService.LoadSceneAsync(SceneNames.MainMenu);
        }
    }
}