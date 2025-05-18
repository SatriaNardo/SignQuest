using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using UnityEditor;

public class ButtonHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] public string comment = "";
    [SerializeField] public BattleDialogueBox dialogueBox;
    [SerializeField] BattleSystem battleSystem;
    public void OnPointerEnter(PointerEventData eventData)
    {
        if(battleSystem.state == BattleState.PlayerAction || battleSystem.state == BattleState.PlayerSkills || battleSystem.state == BattleState.PlayerItems)
        {
            if(battleSystem.typing == false)
            {
                TextMeshProUGUI Test = this.gameObject.GetComponent<TextMeshProUGUI>();
                Test.color = Color.red;
                StartCoroutine(dialogueBox.TypeDialog(comment));
                dialogueBox.isHovering = true;
                battleSystem.selecting = false;
            }
        }
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        if (battleSystem.state == BattleState.PlayerAction || battleSystem.state == BattleState.PlayerSkills || battleSystem.state == BattleState.PlayerItems)
        {
            if(battleSystem.typing == false)
            {
                if (dialogueBox.cr_running == true)
                {
                    StopAllCoroutines();
                    dialogueBox.SetDialog(comment);
                }
                TextMeshProUGUI Test = this.gameObject.GetComponent<TextMeshProUGUI>();
                Test.color = Color.black;
                dialogueBox.isHovering = false;
            }
        }
    }
}
