using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeepItAlive : MonoBehaviour
{
    public static KeepItAlive Instance { get; set; }
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

}
