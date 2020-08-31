using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemController : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler
{
    Color normalColor = new Color(0.19f, 0.28f, 0.33f);
    Color highlightColor = new Color(0.15f, 0.63f, 0.85f);
    Color clickedColor = new Color(0.087f, 0.63f, 0.52f);

    internal PurchasedItemsController PurchasedItemsController { get; set; } = null;
    internal CategoryID CtgID { get; set; } = 0;
    internal int ItemID { get; set; } = 0;

    internal Image background;
    private bool isActive = false;

    private void OnEnable()
    {
        background = transform.Find("ContainerBackground").GetComponent<Image>();
        background.color = normalColor;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        PurchasedItemsController.ResetAllColor(CtgID);
        if (PurchasedItemsController != null)
        {
            //Display the details
            PurchasedItemsController.DisplayItemDescription(CtgID, ItemID);
            isActive = true;
            background.color = clickedColor;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!isActive)
        {
            background.color = highlightColor;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!isActive)
        {
            background.color = normalColor;
        }
    }

    internal void ResetColor()
    {
        isActive = false;
        background.color = normalColor;
    }
}
