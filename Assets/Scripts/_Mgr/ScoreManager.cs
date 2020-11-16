using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    
    #region FIELDS
    [Header("The use's score reach in per playlist")]
    public int score = 0;

    [Header("The user's question")]
    public int totalQuestion = 5;
    public int question = 0;

    [Header("Show current score / total score")]
    public Text txtQuestion;
    public Text txtScore;
    #endregion

//==

    #region SINGLETON
    public static ScoreManager s_instance;

    private void Awake()
    {
        if(s_instance != null)
            return;

        s_instance = this;
    }

    public static ScoreManager GetInstance()
    {
        return s_instance;
    }
    #endregion

//==

    #region UNITY
    private void Start()
    {
        txtScore.text = "Correct: " + score.ToString();
        txtQuestion.text = "Question: " + question + "/" + totalQuestion + ".";
    }

    private void Update()
    {

    }
    #endregion

//==

    #region PUBLIC FUNCTION
    public void PlusScore()
    {
        score++;
    }

    public void UpdateScoreAndQuestion()
    {
        if(question < 5)
            question++;

        txtScore.text = "Correct: " + score.ToString();
        txtQuestion.text = "Question: " + question + "/" + totalQuestion + ".";
    }

    public int GetScore()
    {
        return score;
    }

    public void Reset()
    {
        score = 0;
        question = 0;

        txtScore.text = "Correct: " + score.ToString();
        txtQuestion.text = "Question: " + question + "/" + totalQuestion + ".";
    }
    #endregion

}
