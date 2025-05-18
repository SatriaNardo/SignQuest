using UnityEngine;
using UnityEngine.UI;
using TMPro;
using MoreMountains.InventoryEngine;

public class RecipeUI : MonoBehaviour
{
    public Image icon;
    public TMP_Text nameText;
    public TMP_Text quantityText;

    public void Setup(InventoryItem item, int quantity)
    {
        if (item.Icon != null)
        {
            icon.sprite = item.Icon;
        }

        nameText.text = item.ItemName;
        quantityText.text = "x" + quantity;
    }
}
