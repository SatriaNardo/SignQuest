using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using TMPro;
using Unity.VisualScripting;

public class CharacterSlot : MonoBehaviour
{
    [Header("UI Elements")]
    //public GameObject characterImage;           // To display the character portrait
    public TextMeshProUGUI characterLevel; // To display the character level

    [Header("Slot Data")]
    public HeroBase assignedHero; // The hero assigned to this slot (null if empty)
    public HeroBase Empty;
    

    [Header("Drag & Drop")]
    private CanvasGroup canvasGroup;
    private Transform originalParent;
    private Vector3 originalPosition;

    public static Action<CharacterSlot, CharacterSlot> OnSwapSlots; // Event for swapping slots
    public BattleDataHeroes heroTeams;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void InitializeSlot(HeroBase hero)
    {
        assignedHero = hero;

        if (hero != null)
        {
            // Find the correct "InsidePanel" for this slot
            Transform insidePanel = transform.Find("InsidePanel/HeroPlace");
            if (insidePanel == null)
            {
                Debug.LogWarning("InsidePanel not found for this slot. Make sure it's properly set up in the hierarchy.");
                return;
            }

            // Clear existing children to avoid duplicates
            foreach (Transform child in insidePanel)
            {
                Destroy(child.gameObject);
            }

            // Instantiate the hero's UI as a child of the correct InsidePanel
            if(hero.charSlot != null)
            {
                Instantiate(hero.charSlot,  insidePanel);
            }    
            characterLevel.text = $"Level. {hero.curLevel}"; // Assign the hero's level
        }
        else
        {
            ClearSlot();
        }
    }

    public void ClearSlot()
    {
        assignedHero = Empty;
        //characterImage.SetActive(false);
        characterLevel.text = "";
    }

    
    private void Update()
    {
        // Swap active character with "E" key
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (EventSystem.current.currentSelectedGameObject == gameObject)
            {
                var activeSlotManager = FindObjectOfType<ActiveSlotManager>();
                activeSlotManager?.SwapWithFirstAvailableSlot(this);
            }
        }
    }
}