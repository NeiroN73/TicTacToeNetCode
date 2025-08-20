using System.Collections.Generic;
using System.Linq;
using Content.Scripts.Services;
using UnityEngine;
using VContainer;

namespace Content.Scripts.Installers
{
    public class AppController : MonoBehaviour
    {
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
            DontDestroyOnLoad(this);

            foreach (var initializable in _initializables)
            {
                initializable.Initialize();
            }
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