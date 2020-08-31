using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PurchasedItemsTab : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler, IPointerExitHandler
{
    internal PurchasedItemsTabController purchasedItemsTabController;
    internal Image image;

    private int tabIndex;
    private bool isActive = false;

    internal void SetIndex(int index)
    {
        tabIndex = index;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        purchasedItemsTabController.TabMouseClick(tabIndex);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!isActive)
        {
            image.color = purchasedItemsTabController.mouseEnterColor;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!isActive)
        {
            image.color = purchasedItemsTabController.normalColor;
        }
    }

    public void ToggleActive()
    {
        isActive = !isActive;
        image.color = isActive ? purchasedItemsTabController.mouseClickedColor : purchasedItemsTabController.normalColor;
    }
}
