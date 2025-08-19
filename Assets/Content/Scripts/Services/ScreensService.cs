using System;
using System.Collections.Generic;
using Content.Scripts.Factories;
using Content.Scripts.UI.Base;
using VContainer;

namespace Content.Scripts.Services
{
    public class ScreensService : Service
    {
        [Inject] private readonly ScreensFactory _screensFactory;
        
        private readonly Dictionary<Type, View> _screensByType = new();
        private readonly Stack<View> _screensStack = new();
        
        public View CurrentScreen { get; private set; }

        public async void Open<TScreen>() where TScreen : View
        {
            if (_screensByType.TryGetValue(typeof(TScreen), out var screen))
            {
                screen.Open();
                _screensStack.Push(screen);
                CurrentScreen = screen;
            }
            else
            {
                var newScreen = await _screensFactory.Create<TScreen>();
                newScreen.Open();
                _screensByType.Add(typeof(TScreen), newScreen);
                _screensStack.Push(newScreen);
                CurrentScreen = newScreen;
            }
        }

        public void Close()
        {
            if (_screensStack.TryPop(out var screen))
            {
                screen.Close();
                screen.gameObject.SetActive(false);
                CurrentScreen = _screensStack.Peek();
            }
        }
    }
}
