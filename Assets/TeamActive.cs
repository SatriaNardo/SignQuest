using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TeamActive : MonoBehaviour
{
    public GameObject teamMenu;
    public CraftingTrigger craftingTrigger;
    public bool teamingUp;
    public bool craftUp = false;
    public Toggle openBook;
    public GameObject bookOpened;
    void Start()
    {
        teamingUp = false;
    }
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.T))
        {
            if(!teamingUp && craftUp == false && bookOpened.active == false)
            {
                teamMenu.SetActive(true);
                if (craftingTrigger != null)
                {
                    craftingTrigger.openTeams = true;
                }
                teamingUp = true;
                openBook.interactable = false;
                
            }
            else if (teamingUp)
            {
                teamMenu.SetActive(false);
                if(craftingTrigger != null)
                {
                    craftingTrigger.openTeams = false;
                }
                teamingUp = false;
                openBook.interactable = true;
            }
        }
    }
}
