using Content.Scripts.Entities.Base;
using R3;

namespace Content.Scripts.Entities
{
    public class CellEntity : PooledEntity
    {
        private int _index;

        private readonly Subject<int> _clicked = new();
        public Observable<int> Clicked => _clicked;

        public void Initialize(int index)
        {
            _index = index;
        }
        
        private void OnMouseDown()
        {
            _clicked.OnNext(_index);
        }
    }
}