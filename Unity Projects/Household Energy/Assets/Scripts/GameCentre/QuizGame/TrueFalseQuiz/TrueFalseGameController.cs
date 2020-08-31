using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class TrueFalseGameController : MonoBehaviour
{
    [SerializeField]
    private int thisLevel;

    [SerializeField]
    private TrueFalseQuestion[] questions;

    [SerializeField]
    private TextMeshProUGUI questionText;

    [SerializeField]
    private TextMeshProUGUI questionNumberIndicator;

    [SerializeField]
    private GameObject trueFalseGamePanel;

    [SerializeField]
    private GameObject trueFalseResultPanel;
   
    private static List<TrueFalseQuestion> questionsToAsk;
    private TrueFalseQuestion currentQuestion;
    private float timeInterval = 2.0f;
    private int questionNumber = 0;
    private Animator animator;
    private int correctAnswer = 0;
    private QuizResultController resultController;
    private AudioController audioController;

    private void Start()
    {
        GameObject gameControllerObject = GameObject.FindGameObjectWithTag("GameController");
        if (gameControllerObject != null)
            audioController = gameControllerObject.GetComponent<AudioController>();

        if (questionsToAsk == null || questionsToAsk.Count == 0)
            questionsToAsk = questions.ToList<TrueFalseQuestion>();

        if (trueFalseGamePanel != null)
            animator = trueFalseGamePanel.GetComponent<Animator>();
        SetRandomQuestion();
    }

    private void SetRandomQuestion()
    {
        if (questionsToAsk.Count != 0)
        {
            int questionIndex = Random.Range(0, questionsToAsk.Count);
            currentQuestion = questionsToAsk[questionIndex];
            questionNumber += 1;

            questionText.text = currentQuestion.question;
            questionNumberIndicator.text = string.Format("Question No. {0}", questionNumber);
            animator.SetTrigger("DefaultAnswer");
        }
        else
        {
            trueFalseGamePanel.SetActive(false);
            trueFalseResultPanel.SetActive(true);
            resultController = gameObject.GetComponent<QuizResultController>();

            if (PlayerInfo.QuizCurrentLevel <= thisLevel)
                resultController.UpdateScore(correctAnswer);
            else
                resultController.RepeatedLevel();
        }
    }

    private IEnumerator TransitionNextQuestion()
    {
        questionsToAsk.Remove(currentQuestion);
        yield return new WaitForSeconds(timeInterval);
        SetRandomQuestion();
    }

    public void UserSelection(bool answer)
    {
        if (currentQuestion.isTrue == answer)
        {
            correctAnswer += 1;
            audioController.PlaySoundEffects(AnswerType.CORRECT);
            if (answer)
            {
                animator.SetTrigger("TrueCorrect");
            }
            else
            {
                animator.SetTrigger("FalseCorrect");
            }
        }
        else
        {
            audioController.PlaySoundEffects(AnswerType.WRONG);
            if (answer)
            {
                animator.SetTrigger("TrueWrong");
            }
            else
            {
                animator.SetTrigger("FalseWrong");
            }
        }
        StartCoroutine(TransitionNextQuestion());
    }
}