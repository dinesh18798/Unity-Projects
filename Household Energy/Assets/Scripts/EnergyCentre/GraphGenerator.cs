using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class GraphGenerator : MonoBehaviour
{
    private static Color DOT_CONNECTION_COLOR = new Color(0.98f, 0.7f, 0.5f, 0.3f);

    private RectTransform graphCRectTransform;
    private RectTransform labelXRectTransform;
    private RectTransform labelYRectTransform;
    private RectTransform dashXRectTransform;
    private RectTransform dashYRectTransform;
    private GameObject tooltipGameObject;

    private List<GameObject> graphGameObjectsList;
    private List<IGraphVisualObject> graphVisualObjectsList;
    private Sprite dotSprite;
    private float barWidthMultiplier = 0.8f;

    public GraphGenerator(GameObject graphContainer, Sprite dot)
    {
        graphCRectTransform = graphContainer.GetComponent<RectTransform>();
        labelXRectTransform = graphCRectTransform.Find("GraphLabelTemplateX").GetComponent<RectTransform>();
        labelYRectTransform = graphCRectTransform.Find("GraphLabelTemplateY").GetComponent<RectTransform>();
        dashXRectTransform = graphCRectTransform.Find("GraphDashTemplateX").GetComponent<RectTransform>();
        dashYRectTransform = graphCRectTransform.Find("GraphDashTemplateY").GetComponent<RectTransform>();
        tooltipGameObject = graphCRectTransform.Find("GraphTooltip").gameObject;

        dotSprite = dot;
        graphGameObjectsList = new List<GameObject>();
        graphVisualObjectsList = new List<IGraphVisualObject>();
    }

    internal void GenerateGraph(Dictionary<string, float> valueList, bool isBarChart, int maxVisibleValueCount = -1)
    {
        ShowGraph(valueList, GetGraphVisualType(isBarChart), maxVisibleValueCount);
    }

    private IGraphVisual GetGraphVisualType(bool isBarChart)
    {
        IGraphVisual graphVisual = null;

        if (isBarChart)
            graphVisual = new BarGraphVisual(this, graphCRectTransform, barWidthMultiplier);
        else
            graphVisual = new LineGraphVisual(this, graphCRectTransform, dotSprite, DOT_CONNECTION_COLOR);

        return graphVisual;
    }

    private void ShowGraph(Dictionary<string, float> valueList, IGraphVisual graphVisual, int maxVisibleValueCount)
    {
        ClearGraphGameObjects();

        if (valueList.Count <= 0) return;

        maxVisibleValueCount = maxVisibleValueCount <= 0 || maxVisibleValueCount > valueList.Count ?
            valueList.Count : maxVisibleValueCount;
        int startIndex = Mathf.Max(valueList.Count - maxVisibleValueCount, 0);

        float graphWidth = graphCRectTransform.sizeDelta.x;
        float graphHeight = graphCRectTransform.sizeDelta.y;

        float minValueInList = valueList.Values.ToList().GetRange(startIndex, maxVisibleValueCount).Min();
        float maxValueInList = valueList.Values.ToList().GetRange(startIndex, maxVisibleValueCount).Max();

        float diffMinMax = maxValueInList - minValueInList;
        if (diffMinMax <= 0) { diffMinMax = 5f; }

        float yMinimum = 0f;
        float yMaximum = maxValueInList + (diffMinMax * 0.5f);
        float diffInY = yMaximum - yMinimum;

        float gapBetweenData = graphWidth / (maxVisibleValueCount + 1);
        int xIndex = 0;

        for (int i = startIndex; i < valueList.Count; i++, xIndex++)
        {
            float xPosition = gapBetweenData + xIndex * gapBetweenData;
            float yPosition = ((valueList.ElementAt(i).Value - yMinimum) / diffInY) * graphHeight;

            string tooltipText = valueList.ElementAt(i).Value.ToString("0.000");
            graphVisualObjectsList.Add(graphVisual.CreateGraphVisualObject(new Vector2(xPosition, yPosition), gapBetweenData, tooltipText));

            RectTransform labelX = Instantiate(labelXRectTransform);
            labelX.SetParent(graphCRectTransform, false);
            labelX.gameObject.SetActive(true);
            labelX.anchoredPosition = new Vector2(xPosition, -10f);
            labelX.GetComponent<TextMeshProUGUI>().text = valueList.ElementAt(i).Key;
            graphGameObjectsList.Add(labelX.gameObject);

            RectTransform dashY = Instantiate(dashYRectTransform);
            dashY.SetParent(graphCRectTransform, false);
            dashY.gameObject.SetActive(true);
            dashY.anchoredPosition = new Vector2(xPosition, -5f);
            graphGameObjectsList.Add(dashY.gameObject);
        }

        float separatorCount = 10f;
        for (int j = 0; j <= separatorCount; j++)
        {
            RectTransform labelY = Instantiate(labelYRectTransform);
            labelY.SetParent(graphCRectTransform);
            labelY.gameObject.SetActive(true);
            float normalizedValue = j / separatorCount;
            labelY.anchoredPosition = new Vector2(-85f, normalizedValue * graphHeight);
            labelY.GetComponent<TextMeshProUGUI>().text = Math.Round(yMinimum + (normalizedValue * diffInY), 2).ToString();
            graphGameObjectsList.Add(labelY.gameObject);

            RectTransform dashX = Instantiate(dashXRectTransform);
            dashX.SetParent(graphCRectTransform, false);
            dashX.gameObject.SetActive(true);
            dashX.anchoredPosition = new Vector2(-5f, normalizedValue * graphHeight);
            graphGameObjectsList.Add(dashX.gameObject);
        }
    }

    internal void ShowTooltip(string tooltipText, Vector2 anchoredPosition)
    {
        tooltipGameObject.SetActive(true);
        tooltipGameObject.GetComponent<RectTransform>().anchoredPosition = anchoredPosition;
        TextMeshProUGUI toolTipTextObject = tooltipGameObject.transform.Find("GraphTooltipText").GetComponent<TextMeshProUGUI>();
        toolTipTextObject.text = tooltipText;

        float xPadding = 2f;
        float yPadding = 2f;

        Vector2 backgroundSize = new Vector2(toolTipTextObject.preferredWidth + (xPadding * 2f),
            toolTipTextObject.preferredHeight + (yPadding * 2f));

        tooltipGameObject.transform.Find("GraphTooltipText").GetComponent<RectTransform>().sizeDelta =
            new Vector2(toolTipTextObject.preferredWidth, toolTipTextObject.preferredHeight);

        tooltipGameObject.transform.Find("GraphTooltipBackground").GetComponent<RectTransform>().sizeDelta = backgroundSize;

        tooltipGameObject.transform.SetAsLastSibling();
    }

    internal void HideTooltip()
    {
        tooltipGameObject.SetActive(false);
    }

    internal void ClearGraphGameObjects()
    {
        foreach (GameObject gameObject in graphGameObjectsList)
        {
            Destroy(gameObject);
        }
        graphGameObjectsList.Clear();

        foreach (IGraphVisualObject graphVisualObject in graphVisualObjectsList)
        {
            graphVisualObject.DestroyGraphVisualObject();
        }
        graphVisualObjectsList.Clear();
    }
}
