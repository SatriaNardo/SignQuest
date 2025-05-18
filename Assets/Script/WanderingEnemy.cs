using MoreMountains.TopDownEngine;
using System.Collections.Generic;
using UnityEngine;

public class WanderingEnemy : MonoBehaviour
{
    public BattleData test;
    public int level1;
    public int level2;
    public int level3;
    public SpawnData spawnData;
    public BattleDataHeroes heroesList;

    private List<int> levelList = new List<int>();
    private EnemyManagers enemyManager;

    // Unique ID tracking to avoid duplicates
    private static HashSet<string> spawnedEnemies = new HashSet<string>();
    private string enemyID;

    private void Awake()
    {
        // Create a unique ID based on the enemy's position and BattleData name
        enemyID = $"{test.name}_{transform.position.x}_{transform.position.y}";

        // If the battle was already won or this enemy is already spawned, destroy it
        if (test.battleWon || spawnedEnemies.Contains(enemyID))
        {
            Destroy(gameObject);
            return;
        }

        // Otherwise, mark this enemy as spawned
        spawnedEnemies.Add(enemyID);
        DontDestroyOnLoad(gameObject);

        // Setup level list
        levelList.Add(level1);
        levelList.Add(level2);
        levelList.Add(level3);

        // Cache the manager
        enemyManager = EnemyManagers.Instance;
    }

    private void Update()
    {
        // If the battle was won after returning to this scene, clean up
        if (test.battleWon)
        {
            if (enemyManager != null)
            {
                enemyManager.DeregisterEnemy();
            }

            spawnedEnemies.Remove(enemyID); // Optional: allow re-spawning if needed
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (enemyManager == null)
            {
                enemyManager = EnemyManagers.Instance;
            }

            Debug.Log("Enemies Added");

            enemyManager.RegisterEnemy(test, levelList);
            spawnData.spawnPointPosition = new Vector3(transform.position.x, transform.position.y - 3);
        }
    }
}
