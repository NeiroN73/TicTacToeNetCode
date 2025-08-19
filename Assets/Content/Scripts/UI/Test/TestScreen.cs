using Content.Scripts.UI.Base;
using Content.Scripts.UI.Binders;
using Cysharp.Threading.Tasks;
using R3;
using UnityEngine;

namespace Content.Scripts.UI.Test
{
    public class TestScreen : Screen<TestViewModel>
    {
        [SerializeField] private TextViewBinder testTextView = new("text");
        [SerializeField] private ButtonViewBinder buttonView = new("button");

        public override void Initialize()
        {
            Bind(testTextView, buttonView);
        }
    }

    public class TestViewModel : ViewModel
    {
        public ViewModelBinder<string> TestString = new("text");
        public ViewModelBinder<ReactiveCommand> Button = new("button");

        public override async void Initialize()
        {
            Bind(TestString, Button);
            

            TestString.Value = "Fsdf";

            Button.Value = new();
            Button.Value.Subscribe(_ => Debug.Log("Button clicked!")).AddTo(Disposable);

            await UniTask.Delay(500);
            
            TestString.Value = "AAAAAAAAAAAAAAAAAAAAAAAAAAAA";
        }
    }
}