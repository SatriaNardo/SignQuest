using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialChecker2 : MonoBehaviour
{
    public GameObject tutorialPanel;
    void Start()
    {
        if (PlayerPrefs.GetInt("ShowedTutorial2", 0) == 0)
        {
            tutorialPanel.SetActive(true);
            PlayerPrefs.SetInt("ShowedTutorial2", 1); // Save that it was shown
        }
    }
}
