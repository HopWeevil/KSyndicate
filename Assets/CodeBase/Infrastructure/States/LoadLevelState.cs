using CodeBase.CameraLogic;
using CodeBase.Data;
using CodeBase.Enemy;
using CodeBase.Hero;
using CodeBase.Infrastructure.Factory;
using CodeBase.Infrastructure.Services.PersistentProgress;
using CodeBase.Logic;
using CodeBase.Services.StaticData;
using CodeBase.UI;
using CodeBase.UI.Services.Factory;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CodeBase.Infrastructure.States
{
    public class LoadLevelState : IPayloadedState<string>
    {
        private readonly GameStateMachine _stateMachine;
        private readonly SceneLoader _sceneLoader;
        private readonly LoadingCurtain _loadingCurtain;
        private readonly IGameFactory _gameFactory;
        private readonly IPersistentProgressService _progressService;
        private readonly IStaticDataService _staticDataService;
        private readonly IUIFactory _uIFactory;

        public LoadLevelState(GameStateMachine gameStateMachine, SceneLoader sceneLoader, LoadingCurtain loadingCurtain, IGameFactory gameFactory, IPersistentProgressService progressService, IStaticDataService staticDataService, IUIFactory uIFactory)
        {
            _stateMachine = gameStateMachine;
            _sceneLoader = sceneLoader;
            _loadingCurtain = loadingCurtain;
            _gameFactory = gameFactory;
            _progressService = progressService;
            _staticDataService = staticDataService;
            _uIFactory = uIFactory;
        }

        public void Enter(string sceneName)
        {
            _loadingCurtain.Show();
            _gameFactory.Cleanup();
            _gameFactory.WarmUp();
            _sceneLoader.Load(sceneName, OnLoaded);
        }

        public void Exit()
        {
            _loadingCurtain.Hide();
        }

        private async void OnLoaded()
        {
            await InitUiRoot();
            await InitGameWorld();
            InformProgressReaders();

            _stateMachine.Enter<GameLoopState>();
        }

        private void InformProgressReaders()
        {
            foreach (ISavedProgressReader progressReader in _gameFactory.ProgressReaders)
            {
                progressReader.LoadProgress(_progressService.Progress);
            }
        }
        private async Task InitUiRoot()
        {
            await _uIFactory.CreateUIRoot();
        }

        private async Task InitGameWorld()
        {
            LevelStaticData levelData = GetLevelStaticData();

            await InitSpawners(levelData);
            await InitLootPieces();
            GameObject hero = await InitHero(levelData);
            await InitLevelTransfer(levelData);
            await InitHud(hero);

            CameraFollow(hero);
        }

        private LevelStaticData GetLevelStaticData()
        {
            string sceneKey = SceneManager.GetActiveScene().name;
            LevelStaticData levelData = _staticDataService.ForLevel(sceneKey);
            return levelData;
        }

        private async Task<GameObject> InitHero(LevelStaticData levelData)
        {
            GameObject hero = await _gameFactory.CreateHero(levelData.InitialHeroPosition);
            return hero;
        }

        private async Task InitLootPieces()
        {
            LootPieceDataDictionary lootPieceData = _progressService.Progress.WorldData.LootData.LootPiecesOnScene;

            foreach (string key in lootPieceData.Dictionary.Keys)
            {
                LootPiece lootPiece = await _gameFactory.CreateLoot();
                lootPiece.GetComponent<UniqueId>().Id = key;
                lootPiece.Initialize(lootPieceData.Dictionary[key].Loot);
                lootPiece.transform.position = lootPieceData.Dictionary[key].Position.AsUnityVector();
            }
        }

        private async Task InitSpawners(LevelStaticData levelData)
        {
            foreach(EnemySpawnerData spawnerData in levelData.EnemySpawners)
            {
                await _gameFactory.CreateSpawner(spawnerData.Id, spawnerData.Position, spawnerData.MonsterTypeId);
            }
        }

        private async Task InitHud(GameObject hero)
        {
            GameObject hud = await _gameFactory.CreateHud();

            hud.GetComponentInChildren<ActorUI>().Construct(hero.GetComponent<HeroHealth>());
        }

        private async Task InitLevelTransfer(LevelStaticData levelData)
        {
           await _gameFactory.CreateLevelTransfer(levelData.LevelTransfer.Position);
        }

        private void CameraFollow(GameObject hero)
        {
            Camera.main.GetComponent<CameraFollow>().Follow(hero);
        }
    }
}