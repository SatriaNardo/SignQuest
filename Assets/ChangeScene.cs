using MoreMountains.Tools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeScene : MonoBehaviour
{
    public void changeScene()
    {
        MMSceneLoadingManager.LoadScene("Village");
    }
}