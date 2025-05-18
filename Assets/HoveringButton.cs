using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using UnityEditor;

public class HoveringButton : MonoBehaviour
{
    [SerializeField] BattleSystem battleSystem;
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (battleSystem.state == BattleState.PickingEnemies)
        {
            if (battleSystem.picking == false)
            {
                
            }
        }
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        if (battleSystem.state == BattleState.PlayerAction || battleSystem.state == BattleState.PlayerSkills || battleSystem.state == BattleState.PlayerItems)
        {
        }
    }
}