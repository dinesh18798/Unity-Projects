using UnityEngine;

public class ToolMouseHandler : MonoBehaviour
{
    [SerializeField]
    private string tooltipText;

    private MouseEventHandler mouseEventHandler;

    private void Start()
    {
        mouseEventHandler = gameObject.AddComponent<MouseEventHandler>();

        mouseEventHandler.MouseOverOnceFunc = () =>
        {
            Tooltip.ShowTooltip_Static(tooltipText);
        };
        mouseEventHandler.MouseOutOnceFunc = () =>
        {
            Tooltip.HideTooltip_Static();
        };
    }
}
