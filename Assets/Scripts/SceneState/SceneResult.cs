using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SceneResult : StateScene
{
    #region FIELDS
    //
    // public 
    //
    [Header("Quiz Game")]
    public QuizGame quizGame;

    [Header("Load text in Result Question Choise")]
    public Transform allResult;
    public List<GameObject> listResult;
    public List<questions> listResultQuiz;

    [Header("Show result score of user")]
    public Text txtScore;

    //
    //  private
    //
    private ScoreManager scoreMgr;
    private StoreManager storeMgr;
    #endregion

    #region STATE
    public override void StartState()
    {
        base.EndState();
        Owner.SetActivePanelScene(this.name);

        // GetComponent what we need in state scene
        CacheComponent();

        // show all question and answer result in scene
        LoadResultQuestion();
        LoadTextScoreResult();
    }

    public override void UpdateState()
    {
        base.UpdateState();


    }

    public override void EndState()
    {
        base.EndState();

    }
    #endregion

    //===

    #region PUBLIC FUNCTION
    public void LoadTextScoreResult()
    {
        txtScore.text = scoreMgr.score.ToString() + "/" + scoreMgr.totalQuestion.ToString();
    }

    public void LoadResultQuestion()
    {
        listResultQuiz = StoreManager.GetInstance().GetListQuestionQuiz();

        // load the result of question 
        for (int i = 0; i < listResultQuiz.Count; i++)
        {
            var ques = listResultQuiz[i];
            LoadTheChoiceResult(listResultQuiz[i].choices, i,
                quizGame.listIndexAnswered[i].ToString(), listResultQuiz[i].answerIndex);
        }

    }

    public void LoadTheChoiceAnswered(choices[] arrChoice, int index, string answerIndex)
    {
        var arrQuestion = listResult[index].GetComponentsInChildren<ComChoice>();
        for (int i = 0; i < arrChoice.Length; i++)
        {
            if (int.Parse(answerIndex) == i)
            {
                var img = arrQuestion[i].GetComponent<Image>();
                img.color = Color.red;
            }
        }
    }

    public void LoadTheChoiceResult(choices[] arrChoice, int index, string answerIndex, string resultIndex)
    {
        var arrQuestion = listResult[index].GetComponentsInChildren<ComChoice>();
        listResult[index].GetComponent<Image>().sprite = storeMgr.currentListSprite[index];

        for (int i = 0; i < arrChoice.Length; i++)
        {
            arrQuestion[i].GetComponentInChildren<Text>().text = arrChoice[i].title;

            if (i == int.Parse(answerIndex))
            {
                var bor = arrQuestion[i].gameObject.GetComponentInChildren<ComBorder>();
                bor.gameObject.GetComponent<Image>().enabled = true;
            }

            // result the question
            if (i == int.Parse(resultIndex))
            {
                var img = arrQuestion[i].gameObject.GetComponent<Image>();
                img.color = Color.green;
            }
            // the question what user answered 
            else if (i == int.Parse(answerIndex))
            {
                var img = arrQuestion[i].gameObject.GetComponent<Image>();
                img.color = Color.red;
            }
            else
            {
                SetFalseTheAnswer(arrQuestion[i].gameObject, false);
            }
        }
    }

    public void SetFalseTheAnswer(GameObject go, bool value)
    {
        if (!value)
        {
            var img = go.GetComponent<Image>();
            img.color = new Color(img.color.r, img.color.g, img.color.b, 0.2f);
        }
    }
    #endregion

    #region PRIVATE FUNCTION
    private void CacheComponent()
    {
        // cache instance
        scoreMgr = ScoreManager.GetInstance();
        storeMgr = StoreManager.GetInstance();

        //cache component 
        var gridArray = allResult.GetComponentsInChildren<ComQuestion>();
        foreach (ComQuestion gird in gridArray)
            listResult.Add(gird.gameObject);
    }

    public void ResetColor()
    {
        for (int i = 0; i < listResult.Count; i++)
        {
            var arrQuestion = listResult[i].GetComponentsInChildren<ComChoice>();
            for (int j = 0; j < arrQuestion.Length; j++)
            {
                var bor = arrQuestion[j].gameObject.GetComponentInChildren<ComBorder>();
                bor.gameObject.GetComponent<Image>().enabled = false;

                var img = arrQuestion[j].GetComponent<Image>();
                img.color = Color.white;
            }
        }
    }
    #endregion

    #region PUBLIC FUNCTION
    public void OnPressButtonNext()
    {
        Reset();
    }

    public void Reset()
    {
        //
        scoreMgr.Reset();
        quizGame.Reset();
        ResetColor();

        // load scene welcome the quiz
        var scene = SceneManager.GetInstance();
        scene.ChangeState(scene.sceneWelcome);
    }
    #endregion
}
