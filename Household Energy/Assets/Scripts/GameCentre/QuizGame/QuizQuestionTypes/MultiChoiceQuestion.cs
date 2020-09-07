using System;
using System.Collections.Generic;

[Serializable]
public class MultiChoiceQuestion
{
    public string question;
    public List<MultiChoiceOption> options;
}

[Serializable]
public class MultiChoiceOption
{
    public string option;
    public bool isCorrect = false;
}

public class MultiChoiceCorrectAnswer
{
    public string answer;
    public int positionIndex;
}