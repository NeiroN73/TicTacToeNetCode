using Content.Scripts.Factories;
using UnityEngine;
using VContainer;

namespace Content.Scripts.UI.Base
{
    public abstract class View<TViewModel> : View
        where TViewModel : ViewModel, new()
    {
        [Inject] private ViewModelFactory _viewModelFactory;
        
        protected TViewModel ViewModel;
        
        protected void Bind(params ViewBinder[] viewBinders)
        {
            ViewModel = _viewModelFactory.Create<TViewModel>(viewBinders);
            foreach (var viewBinder in viewBinders)
            {
                viewBinder.Initialize();
            }
        }
    }

    public abstract class View : MonoBehaviour
    {
        public abstract void Initialize();
        
        public virtual void Open()
        {
            gameObject.SetActive(true);
        }

        public virtual void Close()
        {
            gameObject.SetActive(false);
        }
    }
}