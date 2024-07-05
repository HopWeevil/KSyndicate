using System;
using System.Collections.Generic;
using CodeBase.Enemy;
using CodeBase.Infrastructure.ServiceLocator;
using CodeBase.Infrastructure.Services.PersistentProgress;
using CodeBase.StaticData;
using UnityEngine;

namespace CodeBase.Infrastructure.Factory
{
    public interface IGameFactory: IService
    {
        GameObject CreateHero(Vector3 at);
        GameObject CreateHud();
        List<ISavedProgressReader> ProgressReaders { get; }
        List<ISavedProgress> ProgressWriters { get; }
        void Cleanup();
        GameObject CreateMonster(MonsterTypeId monsterTypeId, Transform parent);
        LootPiece CreateLoot();
        void CreateSpawner(Vector3 position, string spawnerId, MonsterTypeId monsterTypeId);
        void CreateLevelTransfer(Vector3 at);
    }
}