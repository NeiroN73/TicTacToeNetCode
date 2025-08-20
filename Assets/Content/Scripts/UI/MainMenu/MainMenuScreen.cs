using Content.Scripts.UI.Base;
using Content.Scripts.UI.Binders;
using UnityEngine;

namespace Content.Scripts.UI.MainMenu
{
    public class MainMenuScreen : Screen<MainMenuViewModel>
    {
        [SerializeField] private InputFieldTextChangedViewBinder _joinCode = new("joinCode");
        [SerializeField] private ButtonViewBinder _hostButton = new("hostButton");
        [SerializeField] private ButtonViewBinder _joinButton = new("joinButton");
        
        public override void Initialize()
        {
            Bind(_joinCode, _hostButton, _joinButton);
        }
    }
}