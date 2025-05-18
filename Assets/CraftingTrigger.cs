using UnityEngine;
using UnityEngine.UI;

public class CraftingTrigger : MonoBehaviour
{
    public GameObject craftingUI;
    private bool playerIsNear = false;
    public bool openTeams = false;
    public Toggle openBook;
    public GameObject bookOpened;
    public TeamActive teamActive;

    void Update()
    {
        if (playerIsNear && Input.GetKeyDown(KeyCode.C) && openTeams == false && bookOpened.active == false)
        {
            ToggleCraftingUI();
            
        }
    }

    void ToggleCraftingUI()
    {
        bool isActive = craftingUI.activeSelf;
        craftingUI.SetActive(!isActive);
        teamActive.craftUp = !isActive;
        openBook.interactable = isActive;

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Waaaaaa");
            playerIsNear = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerIsNear = false;
            teamActive.craftUp = false;
            craftingUI.SetActive(false); // auto-close if leaving
        }
    }
}
