using System;
using System.Collections.Generic;
using Content.Scripts.Factories;
using Content.Scripts.UI.Base;
using Content.Scripts.UI.Loading;
using Cysharp.Threading.Tasks;
using VContainer;

namespace Content.Scripts.Services
{
    public class ScreensService : Service
    {
        [Inject] private readonly ScreensFactory _screensFactory;
        
        private readonly Dictionary<Type, View> _screensByType = new();
        private readonly Stack<View> _screensStack = new();
        
        public View CurrentScreen { get; private set; }
        private View _loadingScreen;

        public async UniTask<TScreen> OpenAsync<TScreen>() where TScreen : View
        {
            if (_screensByType.TryGetValue(typeof(TScreen), out var screen))
            {
                screen.Open();
                _screensStack.Push(screen);
                CurrentScreen = screen;
                return (TScreen)screen;
            }
            
            OpenLoading<LoadingScreen>();
            var newScreen = await _screensFactory.CreateAsync<TScreen>();
            CloseLoading();
            
            newScreen.Open();
            _screensByType.Add(typeof(TScreen), newScreen);
            _screensStack.Push(newScreen);
            CurrentScreen = newScreen;
    
            return newScreen;
        }
        
        public TScreen OpenSync<TScreen>() where TScreen : View
        {
            if (_screensByType.TryGetValue(typeof(TScreen), out var screen))
            {
                screen.Open();
                _screensStack.Push(screen);
                CurrentScreen = screen;
                return (TScreen)screen;
            }
    
            TScreen newScreen;

            newScreen = _screensFactory.CreateSync<TScreen>();

            newScreen.Open();
            _screensByType.Add(typeof(TScreen), newScreen);
            _screensStack.Push(newScreen);
            CurrentScreen = newScreen;

            return newScreen;
        }

        public void OpenLoading<TScreen>() where TScreen : View
        {
            if (_screensByType.TryGetValue(typeof(TScreen), out var screen))
            {
                _loadingScreen = screen;
            }
            else
            {
                _loadingScreen = _screensFactory.CreateSync<TScreen>();
                _screensByType.Add(typeof(TScreen), _loadingScreen);
            }
            
            _loadingScreen.Open();
        }

        public void CloseLoading()
        {
            _loadingScreen.Close();
        }

        public void Close()
        {
            if (_screensStack.TryPop(out var screen))
            {
                screen.Close();
                screen.gameObject.SetActive(false);
                if (_screensStack.Count > 0)
                {
                    CurrentScreen = _screensStack.Peek();
                }
            }
        }
        
        public void Close<TScreen>() where TScreen : View
        {
            if (_screensByType.TryGetValue(typeof(TScreen), out var screenToClose))
            {
                screenToClose.Close();
                screenToClose.gameObject.SetActive(false);
        
                var screensList = new List<View>(_screensStack);
                screensList.Remove(screenToClose);
        
                _screensStack.Clear();
                for (int i = screensList.Count - 1; i >= 0; i--)
                {
                    _screensStack.Push(screensList[i]);
                }
            }
        }
    }

    [Serializable]
    public enum ScreenType
    {
        Primary,
        Overlay,
        Persistent,
        Transient,
        Loading
    }
}
