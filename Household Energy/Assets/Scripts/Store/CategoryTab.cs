using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CategoryTab : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler, IPointerExitHandler
{
    private StoreCategoryTabController storeCategoryTabController;
        
    private int tabIndex;
    private Image image;
    private bool isActive = false;

    private void Awake()
    {
        storeCategoryTabController = FindObjectOfType<StoreCategoryTabController>();
        image = GetComponent<Image>();
    }

    internal void SetIndex(int index)
    {
        tabIndex = index;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        storeCategoryTabController.TabMouseClick(tabIndex);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!isActive)
        {
            image.color = storeCategoryTabController.mouseEnterColor;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!isActive)
        {
            image.color = storeCategoryTabController.normalColor;
        }
    }

    public void ToggleActive()
    {
        isActive = !isActive;
        image.color = isActive ? storeCategoryTabController.mouseClickedColor : storeCategoryTabController.normalColor;
    }
}
