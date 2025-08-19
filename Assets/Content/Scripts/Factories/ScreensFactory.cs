using Content.Scripts.Configs;
using Content.Scripts.Services;
using Content.Scripts.UI.Base;
using Cysharp.Threading.Tasks;
using UnityEngine;
using VContainer;

namespace Content.Scripts.Factories
{
    public class ScreensFactory : Factory, IInitializable
    {
        [Inject] private ViewsFactory _viewsFactory;
        [Inject] private ScreensConfig _screensConfig;
        
        private Transform _parent;

        public void Initialize()
        {
            _parent = Object.Instantiate(_screensConfig.Root, null).transform;
        }

        public async UniTask<TView> Create<TView>()
            where TView : View
        {
            var prefab = await _screensConfig.Load<TView>();
            var screen = _viewsFactory.Create(prefab, _parent);
            return screen;
        }
    }
}
