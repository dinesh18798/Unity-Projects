using UnityEngine;

public class BarGraphVisualObject : IGraphVisualObject
{
    private GraphGenerator graphGenerator;
    private GameObject barGameObject;
    private float barWidthMultiplier;

    public BarGraphVisualObject(GraphGenerator graphGenerator, GameObject barGameObject, float barWidthMultiplier)
    {
        this.graphGenerator = graphGenerator;
        this.barGameObject = barGameObject;
        this.barWidthMultiplier = barWidthMultiplier;
    }

    public void SetGraphVisualObjectInfo(Vector2 graphPosition, float graphPositionWidth, string tooltipText)
    {
        RectTransform rectTransform = barGameObject.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = new Vector2(graphPosition.x, 0f);
        rectTransform.sizeDelta = new Vector2(graphPositionWidth * barWidthMultiplier, graphPosition.y);

        MouseEventHandler mouseEventHandler = barGameObject.GetComponent<MouseEventHandler>();
        mouseEventHandler.MouseOverOnceFunc = () =>
        {
            graphGenerator.ShowTooltip(tooltipText, graphPosition);
        };
        mouseEventHandler.MouseOutOnceFunc = () =>
        {
            graphGenerator.HideTooltip();
        };
    }

    public void DestroyGraphVisualObject()
    {
        Object.Destroy(barGameObject);
    }
}