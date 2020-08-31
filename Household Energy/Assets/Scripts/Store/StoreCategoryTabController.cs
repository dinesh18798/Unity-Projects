using System.Collections.Generic;
using UnityEngine;

public class StoreCategoryTabController : MonoBehaviour
{
    public Transform categoriesPanel;
    public Transform storesPanel;

    internal readonly Color normalColor = Color.white;
    internal readonly Color mouseEnterColor = new Color(0.98f, 1f, 0.52f, 1f);
    internal readonly Color mouseClickedColor = new Color(0.62f, 0.94f, 0.54f, 1f);

    private int selectedIndex;
    private CategoryTab selectedCategory;

    private List<CategoryTab> categoryList = new List<CategoryTab>();
    private List<Transform> storeList = new List<Transform>();

    private void Start()
    {
        if (categoriesPanel != null)
        {
            for (int i = 0; i < categoriesPanel.transform.childCount; i++)
            {
                GameObject tempGameObject = categoriesPanel.transform.GetChild(i).gameObject;
                CategoryTab tempCategoryTab = tempGameObject.GetComponent<CategoryTab>();
                tempCategoryTab.SetIndex(i);
                categoryList.Add(tempCategoryTab);
            }
        }

        foreach (Transform item in storesPanel.transform)
        {
            storeList.Add(item);
        }

        TabMouseClick(0);
    }

    internal void TabMouseClick(int tabIndex)
    {
        if(selectedCategory != null)
        {
            selectedCategory.ToggleActive();
        }

        selectedIndex = tabIndex;
        selectedCategory = categoryList[selectedIndex];
        selectedCategory.ToggleActive();
        HideAllPanel();
    }

    private void HideAllPanel()
    {
        for (int i = 0; i < storeList.Count; i++)
        {
            bool flag = i == selectedIndex;
            storeList[i].gameObject.SetActive(flag);
        }
    }
}
