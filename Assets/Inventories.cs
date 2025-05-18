using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventories : MonoBehaviour
{
    public static Inventories Instance { get; set; }
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
