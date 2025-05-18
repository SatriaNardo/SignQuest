using MoreMountains.InventoryEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BattleDialogueBox : MonoBehaviour
{
    [SerializeField] int lettersPerSecond;
    [SerializeField] Color highlightedColor;
    [SerializeField] TextMeshProUGUI dialogueText;
    [SerializeField] BattleSystem battleSystem;
    [SerializeField] List<TextMeshProUGUI> actionSelect;
    [SerializeField] GameObject moveList;
    [SerializeField] GameObject magicList;
    [SerializeField] GameObject itemList;
    [SerializeField] GameObject consumableList;
    [SerializeField] TextMeshProUGUI moveListText;
    [SerializeField] public List<TextMeshProUGUI> moveSelect = new List<TextMeshProUGUI>();
    [SerializeField] public List<TextMeshProUGUI> itemSelect = new List<TextMeshProUGUI>();
    public bool cr_running;
    public bool isHovering;
    public void SetDialog(string dialog)
    {
        dialogueText.text = dialog; 
    }
    public IEnumerator TypeDialog(string dialog)
    {
        cr_running = true;
        dialogueText.text = "";
        foreach (var letter in dialog.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(1f / lettersPerSecond);
            
        }
        yield return new WaitForSeconds(0.5f);
        cr_running =false;
        battleSystem.clicking = false;
    }
    public void UpdateActionSelection(int selectedAction)
    {
        for (int i=0; i < actionSelect.Count ; i++)
        {
            if(isHovering == false)
            {
                if(battleSystem.selecting == true && battleSystem.clicking == false)
                {
                    if (i == selectedAction)
                        actionSelect[i].color = highlightedColor;
                    else
                        actionSelect[i].color = Color.black;

                    switch (selectedAction)
                    {
                        case 0:
                            dialogueText.text = "Deal a considerable amount of damange to your enemies";
                            break;
                        case 1:
                            dialogueText.text = "Use the active character skill and magic to obliterate your enemies";
                            break;
                        case 2:
                            dialogueText.text = "Use your item to strategize yourself on the battle";
                            break;
                        case 3:
                            dialogueText.text = "Retreating is not a defeat but delaying your victory";
                            break;
                    }
                }
            }
            
        }
    }
    public void UpdateMoveSelection(int selectedAction, Moves moves)
    {
        for (int i = 0; i < moveSelect.Count; i++)
        {
            if(isHovering == false)
            {
                if(battleSystem.selecting == true)
                {
                    if (i == selectedAction)
                        moveSelect[i].color = highlightedColor;
                    else
                        moveSelect[i].color = Color.black;
                 
                    dialogueText.text = moves.Base.Description;
                }
            }
            
        }
    }
    public void UpdateItemSelection(int selectedAction, InventoryItem items)
    {
        for (int i = 0; i < itemSelect.Count; i++)
        {
            if (isHovering == false)
            {
                if (battleSystem.selecting == true)
                {
                    if (i == selectedAction)
                        itemSelect[i].color = highlightedColor;
                    else
                        itemSelect[i].color = Color.black;

                    dialogueText.text = items.Description;
                }
            }

        }
    }
    public void SetMoves(List<Moves> moves)
    {
        for(int i = 1 ; i<moves.Count ;i++)
        {
            ButtonHover test = moveList.GetComponent<ButtonHover>();
            test.comment = moves[i].Base.Description;
            moveListText.text = moves[i].Base.Name;
            GameObject Skills = Instantiate(moveList, magicList.transform);
            moveSelect.Add(Skills.GetComponent<TextMeshProUGUI>());
        }
    }
    public void SetUsableItems()
    {
        Inventory targetInventory = Inventory.FindInventory("KoalaMainInventory", "Player1");
        if (targetInventory == null)
        {
            Debug.LogWarning("No inventory found!");
            return;
        }

        if (itemList == null || consumableList == null || moveListText == null)
        {
            Debug.LogError("UI references not assigned! (itemList, consumableList, or moveListText)");
            return;
        }

        InventoryItem[] allItems = targetInventory.Content;

        foreach (InventoryItem item in allItems)
        {
            if (item == null || string.IsNullOrEmpty(item.ItemName) || item.Quantity <= 0)
                continue;

            if (!item.Usable)
                continue;

            GameObject itemButton = Instantiate(itemList, consumableList.transform);

            // Set text and description
            TextMeshProUGUI buttonText = itemButton.GetComponentInChildren<TextMeshProUGUI>();
            if (buttonText != null)
            {
                buttonText.text = item.ItemName;
            }

            ButtonHover test = itemButton.GetComponent<ButtonHover>();
            if (test != null)
            {
                test.comment = item.Description;
            }

            itemSelect.Add(buttonText);
        }
    }

    public void ClearMoves()
    {
        foreach (Transform child in magicList.transform)
        {
            Destroy(child.gameObject);
        }
        moveSelect.Clear();
    }
    public void ClearItem()
    {
        foreach (Transform child in consumableList.transform)
        {
            Destroy(child.gameObject);
        }
        itemSelect.Clear();
    }
}