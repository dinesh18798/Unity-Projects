using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DragDropController : MonoBehaviour
{
    static Color RED_COLOUR = new Color(0.92f, 0.2f, 0.2f);
    static Color YELLOW_COLOUR = new Color(0.92f, 0.85f, 0.2f);
    static Color GREEN_COLOUR = new Color(0.2f, 0.92f, 0.31f);

    [SerializeField]
    private int thisLevel;

    [SerializeField]
    private GameObject puzzlePanel;

    [SerializeField]
    private GameObject resultPanel;

    [SerializeField]
    private SpriteGroup[] generatorSprites;

    [SerializeField]
    private string[] suitableEnergy;

    [SerializeField]
    private Slider energyBar;

    [SerializeField]
    private GameObject energyBarFill;

    [SerializeField]
    private TextMeshProUGUI energyBarFillText;

    [SerializeField]
    private int maximumEnergyValue;

    internal List<SpriteGroup> generatorList;
    private List<string> suitableEnergyList;
    private SpriteGroup tempGenerator = null;
    private Image energyBarFillImage;
    private PuzzleResultController resultController;
    private AudioController audioController;

    void Start()
    {
        GameObject gameControllerObject = GameObject.FindGameObjectWithTag("GameController");
        if (gameControllerObject != null)
            audioController = gameControllerObject.GetComponent<AudioController>();

        generatorList = generatorSprites.ToList();
        suitableEnergyList = suitableEnergy.ToList();
        energyBarFillImage = energyBarFill.GetComponent<Image>();
        resultController = gameObject.GetComponent<PuzzleResultController>();

        foreach (SpriteGroup generator in generatorList)
        {
            UpdateCounterText(generator);
        }
    }

    public void ReplaceGenerator(PointerEventData eventData, Image currentImage, RectTransform rectTransform)
    {
        UpdateCounter(currentImage.sprite.name.ToLower());

        string name = eventData.pointerDrag.name.ToLower();
        foreach (SpriteGroup generator in generatorList)
        {
            if (name.Equals(generator.sourceOfEnergy.ToLower()))
            {
                tempGenerator = generator;
                break;
            }
        }

        if (!CheckCounter())
        {
            audioController.PlaySoundEffects(AnswerType.CORRECT);
            currentImage.sprite = tempGenerator.sprite;
            rectTransform.sizeDelta = tempGenerator.spriteSize;
            UpdateEnergyBar();
        }
        else
        {
            audioController.PlaySoundEffects(AnswerType.WRONG);
        }
    }

    private void UpdateCounter(string name)
    {
        foreach (SpriteGroup generator in generatorList)
        {
            if (generator.sourceOfEnergy.Equals(name, StringComparison.OrdinalIgnoreCase))
            {
                generator.currentCount -= 1;
                UpdateCounterText(generator);
            }
        }
    }

    private bool CheckCounter()
    {
        bool overLimit = true;
        if (tempGenerator.currentCount < tempGenerator.maxCount)
        {
            overLimit = false;
            tempGenerator.currentCount += 1;
            UpdateCounterText(tempGenerator);
            
        }
        return overLimit;
    }

    private void UpdateCounterText(SpriteGroup generator)
    {
        generator.generatorCountText.text = String.Format("Remaining: {0}", generator.maxCount - generator.currentCount);
    }

    private void UpdateEnergyBar()
    {
        int percentage = 0;
        foreach (SpriteGroup generator in generatorList)
        {
            if(suitableEnergyList.Contains(generator.sourceOfEnergy, StringComparer.OrdinalIgnoreCase))
            {
                percentage += generator.currentCount * generator.energyGenerated;
            }
        }
        energyBar.value = percentage;

        if (percentage <= 30)
            energyBarFillImage.color = RED_COLOUR;
        else if (percentage > 30 && percentage <= 60)
            energyBarFillImage.color = YELLOW_COLOUR;
        else
            energyBarFillImage.color = GREEN_COLOUR;

        energyBarFillText.text = String.Format("{0}%", percentage);

        if (percentage >= maximumEnergyValue)
        {
            PuzzleSolved();
        }
    }

    private void PuzzleSolved()
    {
        puzzlePanel.SetActive(false);
        resultPanel.SetActive(true);

        if (PlayerInfo.PuzzleCurrentLevel <= thisLevel)
            resultController.UpdateScore(5);
        else
            resultController.RepeatedLevel();
    }
}

[Serializable]
public class SpriteGroup
{
    public Sprite sprite;
    public string sourceOfEnergy;
    public int energyGenerated;
    public TextMeshProUGUI generatorCountText;
    public int currentCount = 0;
    public int maxCount;
    public Vector2 spriteSize;
}
