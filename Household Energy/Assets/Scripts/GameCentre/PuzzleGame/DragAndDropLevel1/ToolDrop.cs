using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[Serializable]
public class SpriteDescription
{
    public Sprite sprite;
    public string typeofEnergy;
    public string sourceOfEnergy;
}

public class ToolDrop : MonoBehaviour, IDropHandler
{
    [SerializeField]
    private GameObject levelPuzzleController;
    [SerializeField]
    private GameObject dragDropPanel;

    [SerializeField]
    private TextMeshProUGUI toolText;
 
    [SerializeField]
    private SpriteDescription[] toolSprites;

    private float timeInterval = 2.0f;
    private Image image;
    private Animator animator;
    private List<SpriteDescription> toolsToAsk;
    private SpriteDescription currentTool;
    private static int toolIndex;
    private int correctAnswer = 0;
    private DragDropPanelController panelController;
    private AudioController audioController;

    void Start()
    {
        GameObject gameControllerObject = GameObject.FindGameObjectWithTag("GameController");
        if (gameControllerObject != null)
            audioController = gameControllerObject.GetComponent<AudioController>();

        if (toolsToAsk == null || toolsToAsk.Count == 0)
            toolsToAsk = toolSprites.ToList<SpriteDescription>();

        animator = dragDropPanel.GetComponent<Animator>();
        panelController = levelPuzzleController.GetComponent<DragDropPanelController>();
        image = GetComponent<Image>();
        SetRandomTool();
    }

    private void SetRandomTool()
    {
        animator.SetTrigger("DefaultState");
        toolText.text = String.Empty;
        if (toolsToAsk.Count != 0)
        {
            toolIndex = UnityEngine.Random.Range(0, toolsToAsk.Count);
            currentTool = toolsToAsk[toolIndex];
            image.sprite = currentTool.sprite;
        }
        else
        {
            panelController.OnPuzzleCompleted(correctAnswer);
        }
    }

    private IEnumerator TransitionNextTool()
    {
        toolsToAsk.RemoveAt(toolIndex);
        yield return new WaitForSeconds(timeInterval);
        SetRandomTool();
    }

    public void OnDrop(PointerEventData eventData)
    {
        string energySource = eventData.pointerDrag.name.ToLower();

        if (energySource.Equals(currentTool.sourceOfEnergy.ToLower()))
        {
            audioController.PlaySoundEffects(AnswerType.CORRECT);
            toolText.text = currentTool.typeofEnergy;
            animator.SetTrigger("CorrectMatch");
            correctAnswer += 1;
            StartCoroutine(TransitionNextTool());
        }
        else
        {
            audioController.PlaySoundEffects(AnswerType.WRONG);
            toolText.text = "Wrong";
            animator.SetTrigger("WrongMatch");
        }
    }
}
