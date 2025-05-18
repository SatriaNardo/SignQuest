using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LearningToggleController : MonoBehaviour
{
    public Toggle toggle;  // Reference to the Toggle UI element

    void Start()
    {
        // Check the current PlayerPrefs value and set the toggle state accordingly
        if (PlayerPrefs.GetInt("IsToggleOn", 0) == 1)
        {
            toggle.isOn = true;
        }
        else
        {
            toggle.isOn = false;
        }

        // Add a listener to detect changes in the toggle state
        toggle.onValueChanged.AddListener(OnToggleChanged);
    }

    // Method that gets called when the toggle is changed
    void OnToggleChanged(bool isOn)
    {
        // Save the state of the toggle in PlayerPrefs
        PlayerPrefs.SetInt("IsToggleOn", isOn ? 1 : 0);
        Debug.Log(isOn);
        PlayerPrefs.Save();  // Ensure the changes are saved
    }
}