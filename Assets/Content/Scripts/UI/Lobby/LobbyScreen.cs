using Content.Scripts.UI.Base;
using Content.Scripts.UI.Binders;
using UnityEngine;

namespace Content.Scripts.UI.Lobby
{
    public class LobbyScreen : Screen<LobbyViewModel>
    {
        [SerializeField] private InputFieldSetTextViewBinder _joinCodeInputField = new("joinCode");
        
        public override void Initialize()
        {
            Bind(_joinCodeInputField);
        }
    }
}