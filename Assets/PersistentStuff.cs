using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersistentStuff : MonoBehaviour
{
    public static PersistentStuff Instance;
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
