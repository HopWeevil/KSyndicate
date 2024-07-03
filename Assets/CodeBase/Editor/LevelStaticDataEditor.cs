using CodeBase.Logic;
using CodeBase.Logic.EnemySpawners;
using CodeBase.Services.StaticData;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CodeBase.Editor
{
    [CustomEditor(typeof(LevelStaticData))]
    public class LevelStaticDataEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            LevelStaticData levelData = (LevelStaticData)target;

            if (GUILayout.Button("Collect"))
            {
                SpawnMarker[] spawnMarkers = FindObjectsOfType<SpawnMarker>();

                List<EnemySpawnerData> spawnersList = spawnMarkers.Select(x =>new EnemySpawnerData(x.GetComponent<UniqueId>().Id, x.MonsterTypeId, x.transform.position)).ToList();
                levelData.EnemySpawners = spawnersList;

                levelData.LevelKey = SceneManager.GetActiveScene().name;

            }


            EditorUtility.SetDirty(target);
        }
    }
}