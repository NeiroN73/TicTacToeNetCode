using System;
using R3;

namespace Content.Scripts.UI
{
    public class ViewModelBinder<TValue> : ViewModelBinder
    {
        private TValue _value;
        private ReactiveProperty<TValue> _onBinderTriggered = new();
        private CompositeDisposable _disposable = new();
        
        public ViewModelBinder(string id) : base(id)
        {
            _value = default;
        }

        public TValue Value
        {
            get => _value;
            set
            {
                _value = value;
                _onBinderTriggered.Value = value;
            }
        }
        
        public void SubscribeParse(Action<TValue> action)
        {
            _onBinderTriggered.Subscribe(value => action?.Invoke(value)).AddTo(_disposable);
        }

        public void DisposeParse()
        {
            _disposable.Dispose();
        }
    }
    
    public class ViewModelBinder
    {
        public string Id;
        public ViewModelBinder(string id)
        {
            Id = id;
        }
    }
}