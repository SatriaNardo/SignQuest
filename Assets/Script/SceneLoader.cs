using MoreMountains.TopDownEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{ 
    private static int debugSceneBuildIndex = 0;
    private static int battleSceneBuildIndex = 1;
    private static int savedSceneBuildIndex;
    private static Vector2 savedPlayerLocation;
    Character character;
    void Start()
    {
        character = GetComponent<Character>();
    }
    public static void LoadBattleScene()
    {
        
        savedSceneBuildIndex = SceneManager.GetActiveScene().buildIndex;
        //savedPlayerLocation = character.transform.position

    }
}
