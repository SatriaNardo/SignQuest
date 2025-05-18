using MoreMountains.TopDownEngine;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AfterBattleSpawn : MonoBehaviour
{
    public SpawnData spawnData;
    public static AfterBattleSpawn Instance { get; set; }
    private void Awake()
    {
        gameObject.transform.position = spawnData.spawnPointPosition;
    }
    
   
}
