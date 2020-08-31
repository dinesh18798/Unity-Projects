using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GeneratorDropHandler : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField]
    private GameObject currentLevelController;

    private DragDropController dragDropController;
    private Image currentImage;
    private RectTransform rectTransform;
    private SimpleTooltip simpleTooltip;

    void Start()
    {
        dragDropController = currentLevelController.GetComponent<DragDropController>();
        currentImage = gameObject.GetComponent<Image>();
        rectTransform = gameObject.GetComponent<RectTransform>();
        simpleTooltip = gameObject.GetComponent<SimpleTooltip>();
    }

    public void OnDrop(PointerEventData eventData)
    {
        dragDropController.ReplaceGenerator(eventData, currentImage, rectTransform);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (simpleTooltip == null) return;

       // Debug.Log("Pointer enter: " + currentImage.sprite.name);
        foreach (SpriteGroup generator in dragDropController.generatorList)
        {
            if (generator.sourceOfEnergy.Equals(currentImage.sprite.name, StringComparison.OrdinalIgnoreCase))
            {
                string energySource = String.Format("{0} Energy", generator.sourceOfEnergy);
                string generatedEnergy = String.Format("Generating Energy: {0}", generator.energyGenerated);
                simpleTooltip.infoLeft = energySource + "\n" + generatedEnergy;
            }
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (simpleTooltip == null) return;
        simpleTooltip.infoLeft = "";
    }
}
