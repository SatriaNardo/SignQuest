using MoreMountains.Tools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackToRealWorld : MonoBehaviour
{
    EnemyManagers enemyTeams = EnemyManagers.Instance;
    public void Retreating()
    {
        enemyTeams.DeregisterEnemy();
    }
}
