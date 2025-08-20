using Content.Scripts.Entities;
using Content.Scripts.Factories;
using Content.Scripts.Services;
using UnityEngine;
using VContainer;

namespace Content.Scripts.Installers
{
    public class GameplayInstaller : MonoBehaviour
    {
        [Inject] private EntitiesFactory _entitiesFactory;
        [Inject] private ScreensService _screensService;

        [SerializeField] private BoardEntity _boardPrefab;
        
        private void Awake()
        {
            var board = _entitiesFactory.Create(_boardPrefab);
            board.Initialize();
        }
    }
}