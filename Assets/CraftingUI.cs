using UnityEngine;

public class CraftingUI : MonoBehaviour
{
    public CraftingManager craftingManager;
    public GameObject recipeUIPrefab;
    public Transform recipeListContainer;

    void Start()
    {
        foreach (CraftingRecipe recipe in craftingManager.availableRecipes)
        {
            GameObject newUI = Instantiate(recipeUIPrefab, recipeListContainer);
            RecipeUIHandler handler = newUI.GetComponent<RecipeUIHandler>();
            handler.Setup(recipe, craftingManager);
        }
    }
}