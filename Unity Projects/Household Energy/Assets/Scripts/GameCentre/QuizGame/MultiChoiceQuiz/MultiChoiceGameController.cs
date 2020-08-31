using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class MultiChoiceGameController : MonoBehaviour
{
    [SerializeField]
    private int thisLevel;

    [SerializeField]
    private MultiChoiceQuestion[] questions;

    [SerializeField]
    private TextMeshProUGUI questionText;

    [SerializeField]
    private TextMeshProUGUI questionNumberIndicator;

    [SerializeField]
    private GameObject multiChoiceGamePanel;

    [SerializeField]
    private GameObject multiChoiceResultPanel;

    [SerializeField]
    private TextMeshProUGUI[] optionsText;

    private static List<MultiChoiceQuestion> questionsToAsk;
    private MultiChoiceQuestion currentQuestion;
    private int questionNumber = 0;
    private float timeInterval = 2.5f;
    private static MultiChoiceCorrectAnswer correctAnswer = new MultiChoiceCorrectAnswer();
    private Animator animator;
    private int correctAnswerCount = 0;
    private QuizResultController resultController;
    private AudioController audioController;

    private void Start()
    {
        GameObject gameControllerObject = GameObject.FindGameObjectWithTag("GameController");
        if (gameControllerObject != null)
            audioController = gameControllerObject.GetComponent<AudioController>();

        if (questionsToAsk == null || questionsToAsk.Count == 0)
            questionsToAsk = questions.ToList<MultiChoiceQuestion>();

        if (multiChoiceGamePanel != null)
            animator = multiChoiceGamePanel.GetComponent<Animator>();

        SetRandomQuestion();
    }

    private void SetRandomQuestion()
    {
        if (questionsToAsk.Count != 0)
        {
            int questionIndex = UnityEngine.Random.Range(0, questionsToAsk.Count);
            currentQuestion = questionsToAsk[questionIndex];
            questionNumber += 1;

            questionText.text = currentQuestion.question;
            questionNumberIndicator.text = string.Format("Question No. {0}", questionNumber);

            var rnd = new System.Random();
            List<MultiChoiceOption> result = currentQuestion.options.OrderBy(item => rnd.Next()).ToList<MultiChoiceOption>();

            for (int i = 0; i < result.Count; i++)
            {
                string option = result[i].option;
                optionsText[i].text = option;
                if (result[i].isCorrect)
                {
                    correctAnswer.answer = option;
                    correctAnswer.positionIndex = i;
                }
            }
            animator.SetTrigger("DefaultAnswer");
        }
        else
        {
            multiChoiceGamePanel.SetActive(false);
            multiChoiceResultPanel.SetActive(true);
            resultController = gameObject.GetComponent<QuizResultController>();
            if (PlayerInfo.QuizCurrentLevel <= thisLevel)
                resultController.UpdateScore(correctAnswerCount);
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

    public void UserSelection(int index)
    {
        if (index.Equals(correctAnswer.positionIndex))
        {
            audioController.PlaySoundEffects(AnswerType.CORRECT);
            correctAnswerCount += 1;
            CorrectAnimation(index);
        }
        else
        {
            audioController.PlaySoundEffects(AnswerType.WRONG);
            WrongAnimation(index);
        }
    }

    private void WrongAnimation(int optionIndex)
    {
        switch (optionIndex)
        {
            case 0:
                switch (correctAnswer.positionIndex)
                {
                    case 1:
                        animator.SetTrigger("Opt1WrongOpt2Correct");
                        break;
                    case 2:
                        animator.SetTrigger("Opt1WrongOpt3Correct");
                        break;
                    default:
                        break;
                }
                break;
            case 1:
                switch (correctAnswer.positionIndex)
                {
                    case 0:
                        animator.SetTrigger("Opt2WrongOpt1Correct");
                        break;
                    case 2:
                        animator.SetTrigger("Opt2WrongOpt3Correct");
                        break;
                    default:
                        break;
                }
                break;
            case 2:
                switch (correctAnswer.positionIndex)
                {
                    case 0:
                        animator.SetTrigger("Opt3WrongOpt1Correct");
                        break;
                    case 1:
                        animator.SetTrigger("Opt3WrongOpt2Correct");
                        break;
                    default:
                        break;
                }
                break;
            default:
                break;
        }
        StartCoroutine(TransitionNextQuestion());
    }

    private void CorrectAnimation(int optionIndex)
    {
        switch (optionIndex)
        {
            case 0:
                animator.SetTrigger("Opt1Corect");
                break;
            case 1:
                animator.SetTrigger("Opt2Corect");
                break;
            case 2:
                animator.SetTrigger("Opt3Corect");
                break;
            default:
                break;
        }
        StartCoroutine(TransitionNextQuestion());
    }
}
