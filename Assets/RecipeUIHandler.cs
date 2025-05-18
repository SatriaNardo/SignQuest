using UnityEngine;
using UnityEngine.UI;
using TMPro;
using MoreMountains.InventoryEngine;

public class RecipeUIHandler : MonoBehaviour
{
    public Image resultIcon;
    public TMP_Text resultName;
    public Button craftButton;

    public Transform ingredientListContainer; // container for ingredient UI prefabs
    public GameObject ingredientUIPrefab;     // prefab for individual ingredient

    private CraftingRecipe currentRecipe;
    private CraftingManager craftingManager;

    public void Setup(CraftingRecipe recipe, CraftingManager manager)
    {
        currentRecipe = recipe;
        craftingManager = manager;

        // Set result icon and name
        if (recipe.result.Icon != null)
        {
            resultIcon.sprite = recipe.result.Icon;
        }
        resultName.text = recipe.result.ItemName;

        // Clear previous ingredient UIs if any
        foreach (Transform child in ingredientListContainer)
        {
            Destroy(child.gameObject);
        }

        // Create IngredientUI for each ingredient
        foreach (var ingredient in recipe.ingredients)
        {
            GameObject ingredientUIObj = Instantiate(ingredientUIPrefab, ingredientListContainer);
            RecipeUI handler = ingredientUIObj.GetComponent<RecipeUI>();
            handler.Setup(ingredient.Item, ingredient.Quantity);
        }

        // Setup Craft Button
        craftButton.onClick.RemoveAllListeners();
        craftButton.onClick.AddListener(OnCraftButtonPressed);
    }

    void OnCraftButtonPressed()
    {
        if (craftingManager != null)
        {
            bool success = craftingManager.Craft(currentRecipe);
            Debug.Log(success ? "Crafted successfully!" : "Crafting failed!");
        }
    }
}
