using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialChecker : MonoBehaviour
{
    public GameObject tutorialPanel;
    void Start()
    {
        if (PlayerPrefs.GetInt("ShowedTutorial", 0) == 0)
        {
            tutorialPanel.SetActive(true);
            PlayerPrefs.SetInt("ShowedTutorial", 1); // Save that it was shown
        }
    }
}
