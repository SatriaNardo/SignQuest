using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BattleData", menuName = "ScriptableObjects/BattleData", order = 1)]
public class BattleData : ScriptableObject
{
    public List<EnemiesBase> currentEnemies;
    public bool battleWon = false;
}
