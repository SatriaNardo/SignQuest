using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DragDropHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler
{
    private RectTransform rectTransform;
    float dragSensitivity = 1.5f;
    private CanvasGroup canvasGroup;
    private Transform originalParent;
    private Vector3 originalPosition;
    public HeroBase Empty;
    private RectTransform scrollRectTransform;
    public bool isActiveSlot;
    private List<RectMask2D> rectMask2Ds = new List<RectMask2D>();


    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
        }
        Transform currentParent = transform.parent;
        if (currentParent != null)
        {
            var rectMask = currentParent.GetComponent<RectMask2D>();
            if (rectMask != null)
            {
                rectMask2Ds.Add(rectMask); // Store the found RectMask2D
            }
            else if (rectMask == null)
            {
                currentParent = currentParent.parent;
                if (currentParent != null)
                {
                    rectMask = currentParent.GetComponent<RectMask2D>();
                    if (rectMask != null)
                    {
                        rectMask2Ds.Add(rectMask); // Store the found RectMask2D
                    }
                }
            }
        }
        //parentCanvas = transform.parent.GetComponentInParent<Canvas>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        originalParent = transform.parent;
        originalPosition = transform.position;

        // Enable transparency and disable raycasting during drag
        canvasGroup.blocksRaycasts = false;
        canvasGroup.alpha = 0.8f; // Set to desired transparency while dragging
        foreach (var rectMask in rectMask2Ds)
        {
            rectMask.enabled = false;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        // Adjust drag movement using event delta but reduce sensitivity
        rectTransform.anchoredPosition += eventData.delta * dragSensitivity; // Adjust the drag sensitivity

        // Optionally adjust the dragging position by considering canvas scale
        if (transform.parent != null)
        {
            // If the canvas has a non-1 scale, adjust accordingly
            float scaleFactor = transform.parent.lossyScale.x;
            rectTransform.anchoredPosition += eventData.delta * scaleFactor * dragSensitivity;
        }
        Debug.Log(isActiveSlot);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = true;
        canvasGroup.alpha = 1f; // Restore to original visibility when not dragging
        foreach (var rectMask in rectMask2Ds)
        {
            rectMask.enabled = true;
        }
        // If the object is dropped back at its original parent, restore the original position
        if (transform.parent == originalParent)
        {
            transform.position = originalPosition;
        }
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag != null)
        {
            CharacterSlot draggedSlot = eventData.pointerDrag.GetComponent<CharacterSlot>();
            CharacterSlot targetSlot = GetComponent<CharacterSlot>();
            ActiveSlotManager activeSlotManager = FindObjectOfType<ActiveSlotManager>();
            if (draggedSlot != null && targetSlot != null)
            {
                // Assign the hero from draggedSlot to the active slot
                if (isActiveSlot == true)
                {
                    Debug.Log("WAAA");
                    HeroBase draggedHero = draggedSlot.assignedHero;
                    if (activeSlotManager != null)
                    {
                        int targetIndex = targetSlot.transform.GetSiblingIndex();
                        activeSlotManager.AssignHeroToActiveSlot(draggedHero, targetIndex);
                    }
                }
                else
                {
                    Debug.Log("Waaaa");
                    
                    if (activeSlotManager != null)
                    {
                        int targetIndex = draggedSlot.transform.GetSiblingIndex();
                        activeSlotManager.AssignHeroToActiveSlot(Empty, targetIndex);
                    }
                }
            }
        }
    }
}
