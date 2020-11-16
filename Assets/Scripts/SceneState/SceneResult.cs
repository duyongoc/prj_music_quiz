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
    public Transform allResultTransform;
    public List<GameObject> listResultTransform;
    public List<questions> listResultQuiz;

    [Header("Icon Show result")]
    public IconShow[] arrIconShow;

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

    public void LoadTheChoiceResult(choices[] arrChoice, int index, string answerIndex, string resultIndex)
    {
        var arrQuestion = listResultTransform[index].GetComponentsInChildren<ComChoice>();
        arrIconShow[index].GetComponent<Image>().sprite = storeMgr.GetSpiteFromList(listResultQuiz[index].song.title);

        for (int i = 0; i < arrChoice.Length; i++)
        {
            var cmChoice = arrQuestion[i].gameObject.GetComponent<ComChoice>();
            cmChoice.SetTitleText(arrChoice[i].title);

            if (i == int.Parse(answerIndex))
            {
                var bor = cmChoice.GetComponentInChildren<ComBorder>();
                bor.gameObject.GetComponent<Image>().enabled = true;
            }
            // result the question
            if (i == int.Parse(resultIndex))
            {
                cmChoice.SetColor(new Color(55/255f, 175/255f, 55/255f, 1));
            }
            // the question what user answered 
            else if (i == int.Parse(answerIndex))
            {
                cmChoice.SetColor(new Color(195/255f, 35/255f, 35/255f, 1));
            }
            else
            {
                SetFalseTheAnswer(cmChoice, false);
            }
        }
    }

    public void SetFalseTheAnswer(ComChoice com, bool value)
    {
        if (!value)
        {
            com.SetColor(Color.black);
            com.SetOpacity(0.65f);
        }
    }
    #endregion

    #region PRIVATE FUNCTION
    private void CacheComponent()
    {
        // cache instance
        scoreMgr = ScoreManager.GetInstance();
        storeMgr = StoreManager.GetInstance();

        //
        arrIconShow = allResultTransform.GetComponentsInChildren<IconShow>();

        //cache component on result
        var gridArray = allResultTransform.GetComponentsInChildren<ComQuestion>();
        foreach (ComQuestion gird in gridArray)
            listResultTransform.Add(gird.gameObject);

        foreach (ComQuestion cQuestion in gridArray)
        {
            var arrChoice = cQuestion.GetComponentsInChildren<ComChoice>();
            foreach(ComChoice ch in arrChoice)
                ch.CacheComponent();
        }
    }

    public void ResetColor()
    {
        for (int i = 0; i < listResultTransform.Count; i++)
        {
            var arrQuestion = listResultTransform[i].GetComponentsInChildren<ComChoice>();
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
