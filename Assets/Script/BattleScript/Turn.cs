using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class Turn : MonoBehaviour
{
    public GameObject turnPanelPrefab;       // Prefab for each turn panel
    //public GameObject charImagePrefab;      // Prefab for character image
    Vector3 originalScale;
    public Transform turnOrderPanel;        // Parent container for turn panels
    [SerializeField] public BattleSystem battleSystem;

    public Dictionary<object, GameObject> panelMap = new Dictionary<object, GameObject>(); // Map combatants to their UI panels

    public void InitializeTurnOrderUI(List<object> allCombatants)
    {
        // Clear existing panels
        foreach (Transform child in turnOrderPanel)
        {
            Destroy(child.gameObject);
        }

        panelMap.Clear();

        // Create a panel for each combatant
        foreach (var combatant in allCombatants)
        {
            GameObject panel = Instantiate(turnPanelPrefab, turnOrderPanel);
            originalScale = new Vector3(panel.transform.localScale.x, panel.transform.localScale.y, panel.transform.localScale.z); 
            var borders = panel.GetComponent<Image>();
            borders.enabled = true;
            panelMap[combatant] = panel;

            // Instantiate character image prefab inside the panel
            Transform charImageContainer = panel.transform.Find("Panel");
            var insidePanel = charImageContainer.GetComponent<Image>();
            insidePanel.enabled = true;
            GameObject charImage = Instantiate((combatant is Heroes hero) ? hero.Faces : ((Enemies)combatant).Faces, charImageContainer);

            UpdatePanel(combatant, panel, charImage);
        }
    }

    public void UpdateTurnOrderUI(List<object> sortedCombatants)
    {
        List<object> combatantsToRemove = new List<object>();

        foreach (var combatant in panelMap.Keys)
        {
            if (!sortedCombatants.Contains(combatant))
            {
                combatantsToRemove.Add(combatant);
            }
        }

        // Remove dead combatants from the UI
        foreach (var combatant in combatantsToRemove)
        {
            RemoveCombatantFromUI(combatant);
        }
        // Update the order and appearance of panels
        for (int i = 0; i < sortedCombatants.Count; i++)
        {
            var combatant = sortedCombatants[i];
            if (panelMap.TryGetValue(combatant, out GameObject panel))
            {

                // Update the panel's position in the hierarchy
                panel.transform.SetSiblingIndex(i);
                //StartCoroutine(SmoothVisualPosition(panel, i));
                // Highlight the combatant whose ActionValue is zero
                float actionValue = (combatant is Heroes hero) ? hero.ActionValue : ((Enemies)combatant).ActionValue;
                if (actionValue <= 0)
                {
                    EnlargePanel(panel);
                }
                else
                {
                    ResetPanelSize(panel);
                }
                //StartCoroutine(SmoothVisualPosition(panel, i));
                Transform charImageContainer = panel.transform.Find("Panel");
                GameObject charImage = charImageContainer.GetChild(0).gameObject; // Get the instantiated image
                UpdatePanel(combatant, panel, charImage);
            }
        }
    }

    private void UpdatePanel(object combatant, GameObject panel, GameObject charImage)
    {
        // Update ActionValue text
        TextMeshProUGUI actionValueText = panel.transform.Find("Panel/AVCounter/AVText").GetComponent<TextMeshProUGUI>();
        float actionValue = (combatant is Heroes hero) ? hero.ActionValue : ((Enemies)combatant).ActionValue;
        actionValueText.text = $"{Mathf.Floor(actionValue)}";

    }

    private void EnlargePanel(GameObject panel)
    {
        RectTransform rect = panel.GetComponent<RectTransform>();
        StopAllCoroutines(); // Stop any ongoing animation to avoid conflicts
        StartCoroutine(SmoothScale(rect, originalScale * 2f, 0.2f)); // Smooth enlarge over 0.2 seconds
    }

    private void ResetPanelSize(GameObject panel)
    {
        RectTransform rect = panel.GetComponent<RectTransform>();
        StopAllCoroutines(); // Stop any ongoing animation to avoid conflicts
        StartCoroutine(SmoothScale(rect, originalScale, 0.2f));
    }
    private IEnumerator SmoothScale(RectTransform rect, Vector3 targetScale, float duration)
    {
        Vector3 initialScale = rect.localScale;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            rect.localScale = Vector3.Lerp(initialScale, targetScale, elapsed / duration);
            yield return null;
        }

        rect.localScale = targetScale; // Ensure it reaches the exact target scale at the end
    }
    public void RemoveCombatantFromUI(object combatant)
    {
        if (panelMap.TryGetValue(combatant, out GameObject panel))
        {
            Destroy(panel); // Destroy the panel GameObject
            panelMap.Remove(combatant); // Remove the combatant from the dictionary
        }
    }
}