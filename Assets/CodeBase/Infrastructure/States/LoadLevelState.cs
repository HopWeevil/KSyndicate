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
            _sceneLoader.Load(sceneName, OnLoaded);
        }

        public void Exit()
        {
            _loadingCurtain.Hide();
        }

        private void OnLoaded()
        {
            InitUiRoot();
            InitGameWorld();
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
        private void InitUiRoot()
        {
            _uIFactory.CreateUIRoot();
        }

        private void InitGameWorld()
        {
            LevelStaticData levelData = GetLevelStaticData();

            InitSpawners(levelData);
            InitLootPieces();
            GameObject hero = InitHero(levelData);
            InitLevelTransfer(levelData);
            InitHud(hero);

            CameraFollow(hero);
        }

        private LevelStaticData GetLevelStaticData()
        {
            string sceneKey = SceneManager.GetActiveScene().name;
            LevelStaticData levelData = _staticDataService.ForLevel(sceneKey);
            return levelData;
        }

        private GameObject InitHero(LevelStaticData levelData)
        {
            GameObject hero = _gameFactory.CreateHero(levelData.InitialHeroPosition);
            return hero;
        }

        private void InitLootPieces()
        {
            LootPieceDataDictionary lootPieceData = _progressService.Progress.WorldData.LootData.LootPiecesOnScene;

            foreach (string key in lootPieceData.Dictionary.Keys)
            {
                LootPiece lootPiece = _gameFactory.CreateLoot();
                lootPiece.GetComponent<UniqueId>().Id = key;
                lootPiece.Initialize(lootPieceData.Dictionary[key].Loot);
                lootPiece.transform.position = lootPieceData.Dictionary[key].Position.AsUnityVector();
            }
        }

        private void InitSpawners(LevelStaticData levelData)
        {
            foreach(EnemySpawnerData spawnerData in levelData.EnemySpawners)
            {
                _gameFactory.CreateSpawner(spawnerData.Position, spawnerData.Id, spawnerData.MonsterTypeId);
            }
        }

        private void InitHud(GameObject hero)
        {
            GameObject hud = _gameFactory.CreateHud();

            hud.GetComponentInChildren<ActorUI>().Construct(hero.GetComponent<HeroHealth>());
        }

        private void InitLevelTransfer(LevelStaticData levelData)
        {
            _gameFactory.CreateLevelTransfer(levelData.LevelTransfer.Position);
        }

        private void CameraFollow(GameObject hero)
        {
            Camera.main.GetComponent<CameraFollow>().Follow(hero);
        }
    }
}