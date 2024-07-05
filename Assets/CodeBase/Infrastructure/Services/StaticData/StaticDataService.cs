using System.Collections.Generic;
using System.Linq;
using CodeBase.StaticData;
using CodeBase.StaticData.Windows;
using CodeBase.UI.Services.Windows;
using UnityEngine;

namespace CodeBase.Services.StaticData
{
    public class StaticDataService : IStaticDataService
    {
        private const string MonstersDataPath = "StaticData/Monsters";
        private const string LevelsDataPath = "StaticData/Levels";
        private const string StaticDataWindowPath = "StaticData/UI/WindowStaticData";

        private Dictionary<string, LevelStaticData> _levels;
        private Dictionary<MonsterTypeId, MonsterStaticData> _monsters;
        private Dictionary<WindowId, WindowConfig> _windowConfigs;

        public void LoadMonsters()
        {
            _monsters = Resources.LoadAll<MonsterStaticData>(MonstersDataPath).ToDictionary(x => x.MonsterTypeId, x => x);
            _levels = Resources.LoadAll<LevelStaticData>(LevelsDataPath).ToDictionary(x => x.LevelKey, x => x);
            _windowConfigs = Resources.Load<WindowStaticData>(StaticDataWindowPath).Configs.ToDictionary(x => x.WindowId, x => x);
        }

        public LevelStaticData ForLevel(string sceneKey)
        {
            if (_levels.TryGetValue(sceneKey, out LevelStaticData staticData))
            {
                return staticData;
            }
            else
            {
                return null;
            }
        }

        public MonsterStaticData ForMonster(MonsterTypeId typeId)
        {
            if (_monsters.TryGetValue(typeId, out MonsterStaticData staticData))
            {
                return staticData;
            }
            else
            {
                return null;
            }
        }

        public WindowConfig ForWindow(WindowId windowId)
        {
            if (_windowConfigs.TryGetValue(windowId, out WindowConfig windowConfig))
            {
                return windowConfig;
            }
            else
            {
                return null;
            }
        }
    }
}