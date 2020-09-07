using UnityEngine;

public class DragDropPanelController : MonoBehaviour
{
    [SerializeField]
    private int thisLevel;

    [SerializeField]
    private GameObject dragDropPuzzlePanel;

    [SerializeField]
    private GameObject dragDropResultPanel;

    private PuzzleResultController resultController;
    private int answerCount;

    private void Start()
    {
        resultController = GetComponent<PuzzleResultController>();
    }

    internal void OnPuzzleCompleted(int count)
    {
        dragDropPuzzlePanel.SetActive(false);
        dragDropResultPanel.SetActive(true);
        answerCount = count;

        DisplayResult();
    }

    private void DisplayResult()
    {
        if (PlayerInfo.PuzzleCurrentLevel <= thisLevel)
            resultController.UpdateScore(answerCount);
        else
            resultController.RepeatedLevel();   
    }
}
