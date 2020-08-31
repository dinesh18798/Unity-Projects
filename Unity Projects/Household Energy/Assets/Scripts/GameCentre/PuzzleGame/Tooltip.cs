using TMPro;
using UnityEngine;

public class Tooltip : MonoBehaviour
{
    private static Tooltip instance;
    private RectTransform backgroundRectTransform;
    private TextMeshProUGUI tooltipText;

    private void Awake()
    {
        instance = this;
        backgroundRectTransform = transform.Find("TooltipBackground").GetComponent<RectTransform>();
        tooltipText = transform.Find("TooltipText").GetComponent<TextMeshProUGUI>();
        HideTooltip();
    }

    private void Update()
    {
        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(transform.parent.GetComponent<RectTransform>(), Input.mousePosition, null, out localPoint);
        transform.localPosition = localPoint;       
    }

    private void ShowTooltip(string text)
    {
        gameObject.SetActive(true);
        tooltipText.text = text;

        float padding = 4f;
        Vector2 backgroundSize = new Vector2(tooltipText.preferredWidth + padding * 2f, tooltipText.preferredHeight + padding * 2f);
        backgroundRectTransform.sizeDelta = backgroundSize;
    }

    private void HideTooltip()
    {
        gameObject.SetActive(false);
    }

    public static void ShowTooltip_Static(string text)
    {
        instance.ShowTooltip(text);
    }


    public static void HideTooltip_Static()
    {
        instance.HideTooltip();
    }
}
