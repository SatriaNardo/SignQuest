using UnityEngine;
using MoreMountains.InventoryEngine;
using System.Collections.Generic;

public class CraftingManager : MonoBehaviour
{
    public List<CraftingRecipe> availableRecipes;

    public bool Craft(CraftingRecipe recipe)
    {
        Inventory targetInventory = Inventory.FindInventory("KoalaMainInventory", "Player1");
        if (targetInventory == null)
        {
            Debug.LogWarning("No inventory found!");
            return false;
        }

        // Check if all ingredients exist in required quantities
        foreach (var ingredient in recipe.ingredients)
        {
            List<int> slotIndexes = targetInventory.InventoryContains(ingredient.Item.ItemID);
            int totalQuantity = 0;

            foreach (int index in slotIndexes)
            {
                InventoryItem item = targetInventory.Content[index];
                totalQuantity += item.Quantity;
            }

            if (totalQuantity < ingredient.Quantity)
            {
                Debug.Log($"Missing or not enough: {ingredient.Item.ItemID}");
                return false;
            }
        }

        // Remove ingredients
        foreach (var ingredient in recipe.ingredients)
        {
            int quantityToRemove = ingredient.Quantity;
            List<int> slotIndexes = targetInventory.InventoryContains(ingredient.Item.ItemID);

            foreach (int index in slotIndexes)
            {
                InventoryItem item = targetInventory.Content[index];
                int removeAmount = Mathf.Min(item.Quantity, quantityToRemove);

                targetInventory.RemoveItem(index, removeAmount);
                quantityToRemove -= removeAmount;

                if (quantityToRemove <= 0)
                {
                    break;
                }
            }
        }

        // Add the crafted result
        InventoryItem resultItem = Instantiate(recipe.result);
        targetInventory.AddItem(resultItem, recipe.resultQuantity);

        Debug.Log($"Crafted: {resultItem.ItemID}");
        return true;
    }
}
