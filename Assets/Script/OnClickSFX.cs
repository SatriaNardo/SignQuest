using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class OnClickSFX : MonoBehaviour
{
    void Start()
    {
        var button = GetComponent<Button>();
        if (button != null)
        {
            button.onClick.AddListener(() => SoundManager.Instance.PlayClick());
            return;
        }

        var toggle = GetComponent<Toggle>();
        if (toggle != null)
        {
            toggle.onValueChanged.AddListener((_) => SoundManager.Instance.PlayClick());
        }
    }
}
