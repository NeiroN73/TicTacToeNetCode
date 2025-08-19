using System;
using R3;
using UnityEngine;
using UnityEngine.UI;

namespace Content.Scripts.UI.Binders
{
    [Serializable]
    public class ButtonViewBinder : ViewBinder<ReactiveCommand>
    {
        [SerializeField] private Button _button;
        
        public ButtonViewBinder(string id) : base(id)
        {
        }

        public override void Parse(ReactiveCommand to)
        {
            _button.onClick.AddListener(() => to.Execute(Unit.Default));
        }
    }
}