using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CharacterPopulator : MonoBehaviour
{
    [Header("Prefabs and Parent")]
    public GameObject characterSlotPrefab; // Prefab for the character slot
    public Transform availableSlotsParent; // Parent UI element for available slots

    [Header("Hero Data")]
    public List<HeroBase> allHeroes; // List of all available heroes

    private void Start()
    {
        PopulateCharacterSlots();
    }

    public void PopulateCharacterSlots()
    {
        // Clear existing slots
        foreach (Transform child in availableSlotsParent)
        {
            Destroy(child.gameObject);
        }

        // Instantiate a slot for each HeroBase
        foreach (HeroBase hero in allHeroes)
        {
            GameObject newSlot = Instantiate(characterSlotPrefab, availableSlotsParent);
            GameObject charImage = Instantiate(hero.charSlot, newSlot.transform);
            TextMeshProUGUI Level = newSlot.transform.Find("HeroLevel").GetComponent<TextMeshProUGUI>();
            Level.text = $"Level. {hero.curLevel}";
            // Set the slot's name and image based on HeroBase data
            CharacterSlot slotComponent = newSlot.GetComponent<CharacterSlot>();
            if (slotComponent != null)
            {
                slotComponent.InitializeSlot(hero);
            }
        }
    }
}
