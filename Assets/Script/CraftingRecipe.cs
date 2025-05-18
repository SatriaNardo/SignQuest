using System.Collections.Generic;
using UnityEngine;
using MoreMountains.InventoryEngine;

[System.Serializable]
public class CraftingIngredient
{
    public InventoryItem Item;
    public int Quantity;
}

[CreateAssetMenu(fileName = "CraftingRecipe", menuName = "Inventory/Crafting Recipe")]
public class CraftingRecipe : ScriptableObject
{
    public string recipeName;
    public List<CraftingIngredient> ingredients;
    public InventoryItem result;
    public int resultQuantity = 1;
}