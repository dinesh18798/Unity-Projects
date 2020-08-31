using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ToggleSwitch : MonoBehaviour, IPointerDownHandler
{
    public UnityEvent onValueChanged;

    [SerializeField]
    private RectTransform toggleIndicator;
    [SerializeField]
    private Image backgroundImage;

    [SerializeField]
    private Color onColor;
    [SerializeField]
    private Color offColor;

    private bool on = false;
    private float offXPosition;
    private float onXPosition;
    private float tweenTime = 0.25f;

    public bool IsON
    {
        get { return on; }
        set { Toggle(value); }
    }

    private void Awake()
    {
        offXPosition = toggleIndicator.anchoredPosition.x;
        onXPosition = backgroundImage.rectTransform.rect.x + toggleIndicator.rect.width;
    }

    private void Toggle(bool value)
    {
        on = value;
        ToggleColor(value);
        MoveIndicator(value);

        onValueChanged.Invoke();
    }

    private void MoveIndicator(bool value)
    {
        TweenerCore<Vector2, Vector2, VectorOptions> tweenerMove;
        if (value)
            tweenerMove = toggleIndicator.DOAnchorPosX(onXPosition, tweenTime);
        else
            tweenerMove = toggleIndicator.DOAnchorPosX(offXPosition, tweenTime);
        if (tweenerMove != null) tweenerMove.SetUpdate(true);
    }

    private void ToggleColor(bool value)
    {
        TweenerCore<Color, Color, ColorOptions> tweenerColor;
        if (value)
            tweenerColor = backgroundImage.DOColor(onColor, tweenTime);
        else
            tweenerColor = backgroundImage.DOColor(offColor, tweenTime);
        if (tweenerColor != null) tweenerColor.SetUpdate(true);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Toggle(!on);
    }
}