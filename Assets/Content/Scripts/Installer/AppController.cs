using System.Collections.Generic;
using System.Linq;
using Content.Scripts.Services;
using Content.Scripts.UI.MainMenu;
using Cysharp.Threading.Tasks;
using UnityEngine;
using VContainer;

namespace Content.Scripts.Installer
{
    //правильней было бы стейт машину инициализации сделать, но для тестового задания пойдет и такой вариант
    public class AppController : MonoBehaviour
    {
        [SerializeField] private GameLifetimeScope _gameLifetimeScope;

        [Inject] private ScreensService _screensService;

        private IInitializable[] _initializables;
        private ITickable[] _tickables;

        [Inject]
        private void Construct(IObjectResolver resolver)
        {
            _initializables = resolver.Resolve<IEnumerable<IInitializable>>().ToArray();
            _tickables = resolver.Resolve<IEnumerable<ITickable>>().ToArray();
        }

        private void Awake()
        {
            _gameLifetimeScope.Build();

            foreach (var initializable in _initializables)
            {
                initializable.Initialize();
            }

            _screensService.OpenAsync<MainMenuScreen>().Forget();
        }

        private void Update()
        {
            foreach (var tickable in _tickables)
            {
                tickable.Tick();
            }
        }
    }
}