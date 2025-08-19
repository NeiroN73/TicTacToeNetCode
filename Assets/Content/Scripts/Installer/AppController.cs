using System.Collections.Generic;
using System.Linq;
using Content.Scripts.Services;
using Content.Scripts.UI.Test;
using UnityEngine;
using VContainer;

namespace Content.Scripts.Installer
{
    //правильней было бы стейт машину инициализации сделать, но для тестового задания пойдет и такой вариант
    public class AppController : MonoBehaviour
    {
        [SerializeField] private GameLifetimeScope _gameLifetimeScope;

        [Inject] private ScreensService _screensService;

        private IReadOnlyList<IInitializable> _initializables;
        private IReadOnlyList<ITickable> _tickables;

        [Inject]
        private void Construct(IObjectResolver resolver)
        {
            _initializables = resolver.Resolve<IEnumerable<IInitializable>>().ToList();
            _tickables = resolver.Resolve<IEnumerable<ITickable>>().ToList();
        }

        private void Awake()
        {
            _gameLifetimeScope.Build();

            foreach (var initializable in _initializables)
            {
                initializable.Initialize();
            }
            
            _screensService.Open<TestScreen>();
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