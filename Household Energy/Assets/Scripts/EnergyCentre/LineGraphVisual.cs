using UnityEngine;
using UnityEngine.UI;

internal class LineGraphVisual : IGraphVisual
{
    private GraphGenerator graphGenerator;
    private RectTransform graphCRectTransform;
    private Sprite dotSprite;
    private LineGraphVisualObject prevLineGraphVisualObject;
    private Color dotConnectionColor;

    public LineGraphVisual(GraphGenerator graphGenerator, RectTransform graphCRectTransform, Sprite dotSprite,
        Color dotConnectionColor)
    {
        this.graphGenerator = graphGenerator;
        this.graphCRectTransform = graphCRectTransform;
        this.dotSprite = dotSprite;
        this.dotConnectionColor = dotConnectionColor;

        prevLineGraphVisualObject = null;
    }

    public IGraphVisualObject CreateGraphVisualObject(Vector2 graphPosition, float graphPositionWidth, string tooltipText)
    {
        GameObject currentDotGameObject = CreateDot(graphPosition);

        GameObject dotConnectionGameObject = null;

        if (prevLineGraphVisualObject != null)
        {
            dotConnectionGameObject = CreateDotConnection(prevLineGraphVisualObject.GetGraphPosition(),
                currentDotGameObject.GetComponent<RectTransform>().anchoredPosition);
        }

        LineGraphVisualObject currentLineGraphVisualObject = new LineGraphVisualObject(graphGenerator, 
            currentDotGameObject, dotConnectionGameObject, prevLineGraphVisualObject);
        currentLineGraphVisualObject.SetGraphVisualObjectInfo(graphPosition, graphPositionWidth, tooltipText);

        prevLineGraphVisualObject = currentLineGraphVisualObject;

        return currentLineGraphVisualObject;
    }

    private GameObject CreateDot(Vector2 anchoredPosition)
    {
        GameObject gameObject = new GameObject("dot", typeof(Image));
        gameObject.transform.SetParent(graphCRectTransform, false);
        gameObject.GetComponent<Image>().sprite = dotSprite;
        gameObject.GetComponent<Image>().color = Random.ColorHSV(0f, 1f, 0f, 0.5f, 0.5f, 1f);

        RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = anchoredPosition;
        rectTransform.sizeDelta = new Vector2(20, 20);
        rectTransform.anchorMin = new Vector2(0, 0);
        rectTransform.anchorMax = new Vector2(0, 0);

        gameObject.AddComponent<MouseEventHandler>();

        return gameObject;
    }

    private GameObject CreateDotConnection(Vector2 startDotPosition, Vector2 endDotPosition)
    {
        GameObject gameObject = new GameObject("dotConnection", typeof(Image));
        gameObject.transform.SetParent(graphCRectTransform, false);
        gameObject.GetComponent<Image>().color = dotConnectionColor;

        RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
        Vector2 direction = (endDotPosition - startDotPosition).normalized;
        float distance = Vector2.Distance(startDotPosition, endDotPosition);
        rectTransform.sizeDelta = new Vector2(distance, 7.5f);
        rectTransform.anchorMin = new Vector2(0, 0);
        rectTransform.anchorMax = new Vector2(0, 0);
        rectTransform.localEulerAngles = new Vector3(0, 0, UtilClass.GetAngleFromVectorFloat(direction));
        rectTransform.anchoredPosition = startDotPosition + direction * distance * 0.5f;

        return gameObject;
    }
}