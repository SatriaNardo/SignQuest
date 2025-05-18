using System.Collections.Generic;
using UnityEngine;

public class ActiveSlotManager : MonoBehaviour
{
    [Header("Prefabs and Parent")]
    public Transform activeSlotsParent; // Parent UI for active slots
    public List<CharacterSlot> activeSlots;
    public List<HeroBase> activeHeroes = new List<HeroBase>(3); // List of active heroes (3 max)
    public HeroBase Empty;

    [Header("Persistent Data")]
    public BattleDataHeroes activeTeamData; // ScriptableObject to store the active team
    private int selectedAvailableHeroIndex = -1;

    private void Start()
    {
        if (activeTeamData != null && activeTeamData.currentHero != null)
        {
            activeHeroes = new List<HeroBase>(activeTeamData.currentHero);
        }
        PopulateActiveSlots();
        SynchronizeSlotsWithHierarchy();
    }
    private void SynchronizeSlotsWithHierarchy()
    {
        activeSlots.Clear();

        // Iterate over the children of activeSlotsParent and populate the activeSlots list
        for (int i = 0; i < activeSlotsParent.childCount; i++)
        {
            CharacterSlot slot = activeSlotsParent.GetChild(i).GetComponent<CharacterSlot>();
            if (slot != null)
            {
                activeSlots.Add(slot);
            }
        }

        // Ensure activeHeroes matches the slot count
        while (activeHeroes.Count < activeSlots.Count)
        {
            activeHeroes.Add(Empty); // Fill with placeholders if there are more slots than heroes
        }

        while (activeHeroes.Count > activeSlots.Count)
        {
            activeHeroes.RemoveAt(activeHeroes.Count - 1); // Trim the excess
        }
    }
    public void PopulateActiveSlots()
    {
        // Ensure activeHeroes has exactly 3 slots
        while (activeHeroes.Count < 3)
        {
            activeHeroes.Add(Empty); // Fill with nulls if fewer than 3
        }

        for (int i = 0; i < activeSlotsParent.childCount; i++)
        {
            Transform slot = activeSlotsParent.GetChild(i);
            HeroBase hero = (i < activeHeroes.Count) ? activeHeroes[i] : null;

            CharacterSlot slotComponent = slot.GetComponent<CharacterSlot>();
            if (slotComponent != null)
            {
                slotComponent.InitializeSlot(hero);
            }
        }
        UpdateActiveTeamData();
    }

    public void AssignHeroToActiveSlot(HeroBase hero, int slotIndex)
    {
        if (slotIndex < 0 || slotIndex >= activeHeroes.Count)
            return;

        activeHeroes[slotIndex] = hero;

        // Update the corresponding slot UI
        Transform slot = activeSlotsParent.GetChild(slotIndex);
        CharacterSlot slotComponent = slot.GetComponent<CharacterSlot>();
        if (slotComponent != null)
        {
            slotComponent.InitializeSlot(hero);
        }

        // Update persistent data
        UpdateActiveTeamData();
    }

    public void UpdateActiveTeamData()
    {
        if (activeTeamData != null)
        {
            activeTeamData.currentHero = new List<HeroBase>(activeHeroes);
        }
    }

    public void SwapHeroWithActiveSlot(HeroBase availableHero, int activeSlotIndex)
    {
        if (activeSlotIndex < 0 || activeSlotIndex >= activeHeroes.Count)
            return;

        // Swap the hero
        HeroBase tempHero = activeHeroes[activeSlotIndex];
        activeHeroes[activeSlotIndex] = availableHero;

        // Update the UI
        AssignHeroToActiveSlot(activeHeroes[activeSlotIndex], activeSlotIndex);

        // Optionally return the replaced hero to the available list
        if (tempHero != null)
        {
            // Add tempHero back to the available list (implement this logic as needed)
        }

        UpdateActiveTeamData();
    }
    public void SwapWithFirstAvailableSlot(CharacterSlot draggedSlot)
    {
        if (draggedSlot.assignedHero == null) return;

        // Find the first empty active slot
        foreach (CharacterSlot activeSlot in activeSlots)
        {
            if (activeSlot.assignedHero == null)
            {
                // Perform the swap
                activeSlot.InitializeSlot(draggedSlot.assignedHero);
                draggedSlot.ClearSlot();
                return;
            }
        }

        // If no empty slot is found, swap with the first slot
        if (activeSlots.Count > 0)
        {
            CharacterSlot firstSlot = activeSlots[0];
            HeroBase tempHero = firstSlot.assignedHero;

            // Swap heroes
            firstSlot.InitializeSlot(draggedSlot.assignedHero);
            draggedSlot.InitializeSlot(tempHero);
        }
    }
    private void OnTransformChildrenChanged()
    {
        // Resynchronize slots with the UI hierarchy if the hierarchy changes
        SynchronizeSlotsWithHierarchy();
        PopulateActiveSlots();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && selectedAvailableHeroIndex >= 0)
        {
            //HeroBase availableHero = /* Retrieve hero at selectedAvailableHeroIndex */;
            //int activeSlotIndex = /* Determine active slot index (e.g., based on UI focus) */;
            //SwapHeroWithActiveSlot(availableHero, activeSlotIndex);
        }
    }
}
