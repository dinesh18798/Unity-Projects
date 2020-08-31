using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PurchasedItemsTabController : MonoBehaviour
{
    public Transform tabsPanel;
    public Transform viewsPanel;
    public Transform descriptionPanel;

    internal readonly Color normalColor = Color.white;
    internal readonly Color mouseEnterColor = new Color(0.98f, 1f, 0.52f, 1f);
    internal readonly Color mouseClickedColor = new Color(0.62f, 0.94f, 0.54f, 1f);

    private int selectedIndex;
    private PurchasedItemsTab selectedCategory;

    private List<PurchasedItemsTab> tabList = new List<PurchasedItemsTab>();
    private List<Transform> viewList = new List<Transform>();


    private void Start()
    {
        if (tabsPanel != null)
        {
            for (int i = 0; i < tabsPanel.transform.childCount; i++)
            {
                GameObject tempGameObject = tabsPanel.transform.GetChild(i).gameObject;
                PurchasedItemsTab purchasedItemsTab = tempGameObject.AddComponent<PurchasedItemsTab>();
                purchasedItemsTab.purchasedItemsTabController = this;
                purchasedItemsTab.image = tempGameObject.GetComponent<Image>();
                purchasedItemsTab.SetIndex(i);
                tabList.Add(purchasedItemsTab);
            }
        }

        foreach (Transform item in viewsPanel.transform)
        {
            viewList.Add(item);
        }

        TabMouseClick(0);
    }

    internal void TabMouseClick(int tabIndex)
    {
        if (selectedCategory != null)
        {
            selectedCategory.ToggleActive();
        }

        selectedIndex = tabIndex;
        selectedCategory = tabList[selectedIndex];
        selectedCategory.ToggleActive();
        HideAllPanel();
    }

    private void HideAllPanel()
    {
        for (int i = 0; i < viewList.Count; i++)
        {
            bool flag = i == selectedIndex;
            viewList[i].gameObject.SetActive(flag);
        }
        descriptionPanel.gameObject.SetActive(false);
    }
}
