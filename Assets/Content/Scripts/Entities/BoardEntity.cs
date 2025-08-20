using System;
using System.Collections.Generic;
using Content.Scripts.Entities.Base;
using R3;
using UnityEngine;

namespace Content.Scripts.Entities
{
    public class BoardEntity : Entity, IDisposable
    {
        private const int BOARD_SIZE = 3;
        private const int CELLS_COUNT = BOARD_SIZE * BOARD_SIZE;
        
        [SerializeField] private CellEntity _cellPrefab;
        [SerializeField] private Transform _cellsContainer;
        [SerializeField] private float _cellSpacing = 0.33f;

        private readonly List<CellEntity> _cells = new();
        private readonly CompositeDisposable _disposable = new();
        
        private EntityPool<CellEntity> _cellsPool;
        
        public void Initialize()
        {
            _cellsPool = new EntityPool<CellEntity>(_cellPrefab, _cellsContainer, CELLS_COUNT, CELLS_COUNT);

            for (int y = BOARD_SIZE - 1; y >= 0; y--)
            {
                for (int x = 0; x < BOARD_SIZE; x++)
                {
                    var cell = _cellsPool.Get();
                    int index = (BOARD_SIZE - 1 - y) * BOARD_SIZE + x + 1;
                    cell.Initialize(index);
                    cell.Clicked.Subscribe(OnCellClicked).AddTo(_disposable);
    
                    Vector2 position = new Vector2(
                        x * _cellSpacing - (BOARD_SIZE - 1) * _cellSpacing * 0.5f,
                        y * _cellSpacing - (BOARD_SIZE - 1) * _cellSpacing * 0.5f
                    );
    
                    cell.transform.localPosition = position;
                    _cells.Add(cell);
                }
            }
        }

        public void ClearBoard()
        {
            foreach (var cell in _cells)
            {
                _cellsPool.Release(cell);
            }
            _cells.Clear();
        }
        
        public void OnCellClicked(int index)
        {
            Debug.Log($"Cell clicked: {index}");
            // Здесь обрабатывайте клик по клетке
        }

        public CellEntity GetCell(int index)
        {
            if (index >= 0 && index < _cells.Count)
                return _cells[index];
            return null;
        }

        public void Dispose()
        {
            _disposable.Dispose();
            ClearBoard();
        }
    }
}