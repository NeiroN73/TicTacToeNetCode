using System;

namespace Content.Scripts.UI
{
    public abstract class ViewBinder<TViewModelValue> : ViewBinder, IDisposable
    {
        private ViewModelBinder<TViewModelValue> _viewModelBinder;
        
        public ViewBinder(string id) : base(id)
        {
        }

        public override void Initialize()
        {
            if (ViewModelBinder is ViewModelBinder<TViewModelValue> viewModelBinder)
            {
                _viewModelBinder = viewModelBinder;
                _viewModelBinder.SubscribeParse(Parse);
            }
        }

        public abstract void Parse(TViewModelValue to);
        
        public void Dispose()
        {
            _viewModelBinder.DisposeParse();
        }
    }

    public abstract class ViewBinder
    {
        public ViewModelBinder ViewModelBinder;
        public string Id { get; private set; }

        public ViewBinder(string id)
        {
            Id = id;
        }

        public abstract void Initialize();
    }
}
