using UnityEngine;
using UnityEngine.UI;

internal class BarGraphVisual : IGraphVisual
{
    private GraphGenerator graphGenerator;
    private readonly RectTransform graphCRectTransform;
    private readonly float barWidthMultiplier;

    public BarGraphVisual(GraphGenerator graphGenerator, RectTransform graphCRectTransform, float barWidthMultiplier)
    {
        this.graphGenerator = graphGenerator;
        this.graphCRectTransform = graphCRectTransform;
        this.barWidthMultiplier = barWidthMultiplier;
    }

    public IGraphVisualObject CreateGraphVisualObject(Vector2 graphPosition, float graphPositionWidth, string tooltipText)
    {
        GameObject barGameObject = CreateBar(graphPosition, graphPositionWidth);

        BarGraphVisualObject barChartVisualObject = new BarGraphVisualObject(graphGenerator, 
            barGameObject, barWidthMultiplier);
        barChartVisualObject.SetGraphVisualObjectInfo(graphPosition, graphPositionWidth, tooltipText);

        return barChartVisualObject;
    }

    private GameObject CreateBar(Vector2 graphPosition, float barWidth)
    {
        GameObject gameObject = new GameObject("bar", typeof(Image));
        gameObject.transform.SetParent(graphCRectTransform, false);
        gameObject.GetComponent<Image>().color = Random.ColorHSV(0f, 1f, 0f, 0.5f, 0.5f, 1f);

        RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = new Vector2(graphPosition.x, 0f);
        rectTransform.sizeDelta = new Vector2(barWidth * barWidthMultiplier, graphPosition.y);
        rectTransform.anchorMin = new Vector2(0, 0);
        rectTransform.anchorMax = new Vector2(0, 0);
        rectTransform.pivot = new Vector2(0.5f, 0f);
        
        gameObject.AddComponent<MouseEventHandler>();

        return gameObject;
    }
}