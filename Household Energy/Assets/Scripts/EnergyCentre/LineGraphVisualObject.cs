using System;
using UnityEngine;

public class LineGraphVisualObject : IGraphVisualObject
{
    public event EventHandler OnChangeGraphVisualObjectInfo;

    private GraphGenerator graphGenerator;
    private GameObject dotGameObject;
    private GameObject dotConnectionGameObject;
    private LineGraphVisualObject prevLineGraphVisualObject;

    public LineGraphVisualObject(GraphGenerator graphGenerator, GameObject dotGameObject,
        GameObject dotConnectionGameObject, LineGraphVisualObject prevLineGraphVisualObject)
    {
        this.graphGenerator = graphGenerator;
        this.dotGameObject = dotGameObject;
        this.dotConnectionGameObject = dotConnectionGameObject;
        this.prevLineGraphVisualObject = prevLineGraphVisualObject;

        if (prevLineGraphVisualObject != null)
        {
            prevLineGraphVisualObject.OnChangeGraphVisualObjectInfo += PrevLineGraphVisualObject_OnChangeGraphVisualObjectInfo;
        }
    }

    private void PrevLineGraphVisualObject_OnChangeGraphVisualObjectInfo(object sender, EventArgs e)
    {
        UpdateDotConnection();
    }

    public void SetGraphVisualObjectInfo(Vector2 graphPosition, float graphPositionWidth, string tooltipText)
    {
        RectTransform rectTransform = dotGameObject.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = graphPosition;

        UpdateDotConnection();

        if (OnChangeGraphVisualObjectInfo != null) OnChangeGraphVisualObjectInfo(this, EventArgs.Empty);

        MouseEventHandler mouseEventHandler = dotGameObject.GetComponent<MouseEventHandler>();
        mouseEventHandler.MouseOverOnceFunc = () =>
        {
            graphGenerator.ShowTooltip(tooltipText, graphPosition);
        };
        mouseEventHandler.MouseOutOnceFunc = () =>
        {
            graphGenerator.HideTooltip();
        };
    }

    private void UpdateDotConnection()
    {
        if (dotConnectionGameObject != null)
        {
            RectTransform dotConnectionRectTransform = dotConnectionGameObject.GetComponent<RectTransform>();
            Vector2 direction = (prevLineGraphVisualObject.GetGraphPosition() - GetGraphPosition()).normalized;
            float distance = Vector2.Distance(GetGraphPosition(), prevLineGraphVisualObject.GetGraphPosition());
            dotConnectionRectTransform.sizeDelta = new Vector2(distance, 7.5f);
            dotConnectionRectTransform.anchorMin = new Vector2(0, 0);
            dotConnectionRectTransform.anchorMax = new Vector2(0, 0);
            dotConnectionRectTransform.localEulerAngles = new Vector3(0, 0, UtilClass.GetAngleFromVectorFloat(direction));
            dotConnectionRectTransform.anchoredPosition = GetGraphPosition() + direction * distance * 0.5f;
        }
    }

    public void DestroyGraphVisualObject()
    {
        UnityEngine.Object.Destroy(dotGameObject);
        UnityEngine.Object.Destroy(dotConnectionGameObject);
    }

    public Vector2 GetGraphPosition()
    {
        RectTransform rectTransform = dotGameObject.GetComponent<RectTransform>();
        return rectTransform.anchoredPosition;
    }
}
