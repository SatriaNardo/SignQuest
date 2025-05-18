using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyManagers : MonoBehaviour
{
    public static EnemyManagers Instance { get;  set; }

    public List<BattleData> ActiveEnemies = new List<BattleData>();
    public List<List<int>> ActiveEnemiesLevel = new List<List<int>>();
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void RegisterEnemy(BattleData enemies, List<int> enemiesLevel)
    {
        if (!ActiveEnemies.Contains(enemies))
        {
            ActiveEnemies.Add(enemies);
            ActiveEnemiesLevel.Add(enemiesLevel);
        }
    }

    public void DeregisterEnemy()
    {
        ActiveEnemies.Clear();
    }
}
