using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;



public class EncounterEnemies : MonoBehaviour
{
    public GameObject respawnAfterWin;
    public Vector2 location;
    // Start is called before the first frame update
    private void OnTriggerEnter2D(Collider2D collision)
    {
        location.y = this.transform.position.y - 3;
        location.x = this.transform.position.x;
        respawnAfterWin.transform.position = location;
        
    }
}
