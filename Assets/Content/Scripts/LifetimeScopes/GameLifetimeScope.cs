using System.Linq;
using Content.Scripts.Factories;
using Content.Scripts.Services;
using UnityEngine;
using UnityEngine.AddressableAssets;
using VContainer;
using VContainer.Unity;

namespace Content.Scripts.Installer
{
    public class GameLifetimeScope : LifetimeScope
    {
        [SerializeField] private AssetLabelReference _configsAssetLabel;

        private IContainerBuilder _builder;
        
        protected override void Configure(IContainerBuilder builder)
        {
            _builder = builder;
            
            RegisterConfigs();
            RegisterFactories();
            RegisterServices();
        }

        private void RegisterConfigs()
        {
            var configs = Addressables.LoadAssetsAsync<Config>(_configsAssetLabel, null)
                .WaitForCompletion().ToList();
            foreach (var config in configs)
            {
                Register(config);
            }
        }
        
        private void RegisterFactories()
        {
            Register<ViewModelFactory>();
            Register<ViewsFactory>();
            Register<ScreensFactory>();
        }

        private void RegisterServices()
        {
            Register<ScreensService>();
            Register<ScenesService>();
            Register<AuthenticationsService>();
            Register<LobbiesService>();
        }
        
        private void Register<T>() where T : class
        {
            _builder.Register<T>(Lifetime.Singleton).AsImplementedInterfaces().AsSelf();
        }
        
        private void Register<T>(T instance) where T : class
        {
            _builder.RegisterInstance(instance).AsImplementedInterfaces().AsSelf();
        }
    }
}