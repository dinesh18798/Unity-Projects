using UnityEngine;

// Interface definition for showing visual fo a data point
public interface IGraphVisual
{
    IGraphVisualObject CreateGraphVisualObject(Vector2 graphPosition, float graphPositionWidth, string tooltipText);
}

// Represent single visual object in the graph
public interface IGraphVisualObject
{
    void SetGraphVisualObjectInfo(Vector2 graphPosition, float graphPositionWidth, string tooltipText);
    void DestroyGraphVisualObject();
}