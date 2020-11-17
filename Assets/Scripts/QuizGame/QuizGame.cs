using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class QuizGame : MonoBehaviour
{
    #region FIELDS
    //
    //  public variable
    //
    public float timeDelayPerQuestion = 1.5f;

    [Header("Loading icon of Song")]
    public Image iconOfSong;

    [Header("Sound answer")]
    public AudioClip audioTrueAnswer;
    public AudioClip audioFalseAnswer;

    [Header("Load text in buttons Welcomes Scene")]
    public Transform allPlayList;
    public List<Button> listButtonPlayList;

    [Header("Load text in Question Choise")]
    public Transform allQuestion;
    public List<GameObject> listQuestion;

    [Header("Current Choise in Question")]
    public List<choices> currentChoices;
    public string answerIndex;
    public ComChoice[] arrChoice;

    [Header("List index that user answered")]
    public List<int> listIndexAnswered;

    [Header("Current state question")]
    public EStateQuestion currentStateQuestion;
    public enum EStateQuestion { One, Two, Three, Four, Five, None }

    //
    // private variable
    //
    private SceneManager sceneMgr;
    private ScoreManager scoreMgr;
    private StoreManager storeMgr;
    private SoundManger soundMgr;

    //
    private bool hasAnswer = false;
    #endregion


    #region UNITY
    private void Start()
    {
        // Get component for Quiz Game
        CacheComponent();

        LoadTextButtonPlayList();
    }

    private void Update()
    {
        switch (currentStateQuestion)
        {
            case EStateQuestion.One:

                break;
            case EStateQuestion.Two:

                break;
            case EStateQuestion.Three:

                break;
            case EStateQuestion.Four:

                break;
            case EStateQuestion.Five:

                break;
        }
    }
    #endregion

//==

    #region EVENT BUTTONS
    // Handle event when user press button playlist
    public void OnPressButtonPlayList()
    {
        string nameBtn = EventSystem.current.currentSelectedGameObject.name;

        // load data from storage
        storeMgr.LoadPlayListQuestion(nameBtn);

        // loading question one of Quiz
        ChangeStateQuestion(EStateQuestion.One);
        sceneMgr.ChangeState(sceneMgr.sceneQuiz);
    }

    // Handle event when user press button answer the question
    public void OnPressButtonAnswer()
    {
        if(hasAnswer)
            return;

        string nameIndex = EventSystem.current.currentSelectedGameObject.name;
        listIndexAnswered.Add(int.Parse(nameIndex));

        CheckAnswerTheQuestion(nameIndex);
        StartCoroutine("ChangeStateQuestionWithDelay", timeDelayPerQuestion);
    }
    #endregion 

//==

    #region ANSWER THE QUESTION
    public void CheckAnswerTheQuestion(string nameIndex)
    {
        // correct the answer, plus core for user
        if (nameIndex == answerIndex)
        {
            scoreMgr.PlusScore();
            soundMgr.PlayOnlySoundOneShot(audioTrueAnswer);
        }
        else
        {
            soundMgr.PlayOnlySoundOneShot(audioFalseAnswer);
        }
          
        //
        for (int i = 0; i < arrChoice.Length; i++)
        { 
            var cmChoice = arrChoice[i].gameObject.GetComponent<ComChoice>();

            if(int.Parse(nameIndex) == i) // the answer of user 
            {
                //  set default color is red 
                cmChoice.SetColor(Color.red); 

                //  when user get correct the answer - trigger animation
                if(int.Parse(answerIndex) == int.Parse(nameIndex)) 
                {
                    cmChoice.SetCorrectChoice();
                }
                else
                {  
                    cmChoice.SetFalseChoice();
                }
            }
            else if (int.Parse(answerIndex) == i) // change color of the correct answer
            {
                cmChoice.SetColor(Color.green);
            }
            else
            {
                SetChoiceAfterAnswer(cmChoice, false); // set opacity for another question
            }
        }
        
        hasAnswer = true;
        scoreMgr.UpdateScoreAndQuestion();
    }

    public void SetChoiceAfterAnswer(ComChoice com, bool value)
    {
        if (!value)
        {
            com.SetOpacity(0.35f);
        }
    }

    public EStateQuestion LoadNextStateQuestion()
    {
        switch (currentStateQuestion)
        {
            case EStateQuestion.One:
                currentStateQuestion = EStateQuestion.Two;
                return currentStateQuestion;

            case EStateQuestion.Two:
                currentStateQuestion = EStateQuestion.Three;
                return currentStateQuestion;

            case EStateQuestion.Three:
                currentStateQuestion = EStateQuestion.Four;
                return currentStateQuestion;

            case EStateQuestion.Four:
                currentStateQuestion = EStateQuestion.Five;
                return currentStateQuestion;

            case EStateQuestion.Five:
                currentStateQuestion = EStateQuestion.None;
                return currentStateQuestion;
        }
        return EStateQuestion.None;
    }
    #endregion

//== 

    #region LOADING QUESTION
    public void ChangeStateQuestion(EStateQuestion newState)
    {
        // Debug.Log(currentStateQuestion);
        currentStateQuestion = newState;
        if (currentStateQuestion == EStateQuestion.None)
        {
            sceneMgr.ChangeState(sceneMgr.sceneResult);
            return;
        }

        sceneMgr.sceneQuiz.GetRandomBackroundColor();
        LoadQuestionData((int)currentStateQuestion);
    }

    IEnumerator ChangeStateQuestionWithDelay(float timer)
    {
        yield return new WaitForSeconds(timer);

        hasAnswer = false;
        ChangeStateQuestion(LoadNextStateQuestion());
    }

    public void LoadQuestionData(int indexName)
    {
        string strIndexName = indexName.ToString();

        // Get current list with index from the list of storeMGr
        currentChoices = storeMgr.GetCurrentChoicesQuestionIndex(indexName);
        answerIndex = storeMgr.GetCurrentAnswerQuestionIndex(indexName);

        // Get sound and texture of question from data download
        soundMgr.PlaySound(storeMgr.GetAudioClipFromList(storeMgr.currentListQuestion[indexName].song.title));
        iconOfSong.sprite = storeMgr.GetSpiteFromList(storeMgr.currentListQuestion[indexName].song.title);

        LoadTextChoicesQuestion(indexName);
        
        foreach (GameObject ob in listQuestion)
        {
            ob.SetActive(strIndexName.Contains(ob.name));
        }
    }

    private void LoadTextChoicesQuestion(int index)
    {
        arrChoice = listQuestion[index].GetComponentsInChildren<ComChoice>();

        for (int i = 0; i < currentChoices.Count; i++)
        {
            arrChoice[i].GetComponentInChildren<ComChoice>().SetTitleText(currentChoices[i].title);
        }
    }

    public void ResetColor()
    {
        for (int i = 0; i < listQuestion.Count; i++)
        {
            var arrQuestion = listQuestion[i].GetComponentsInChildren<ComChoice>();
            for (int j = 0; j < arrQuestion.Length; j++)
            {
                var img = arrQuestion[j].GetComponent<Image>();
                img.color = Color.white;
            }
        }
    }

    public void Reset()
    {
        //clear old list answer and restart color of it
        listIndexAnswered.Clear();
        ResetColor();

        // reset origin color of choice 
        foreach(GameObject ob in listQuestion)
        {        
            var choice = ob.GetComponentsInChildren<ComChoice>();
            foreach(ComChoice com in choice)
            {   
                com.Reset();
            }
        }
    }
    #endregion

    #region PRIVATE FUNCTION
    private void CacheComponent()
    {
        // cache instance
        sceneMgr = SceneManager.GetInstance();
        scoreMgr = ScoreManager.GetInstance();
        storeMgr = StoreManager.GetInstance();
        soundMgr = SoundManger.GetInstance();

        // cache component
        var arrBtn = allPlayList.GetComponentsInChildren<Button>();
        listButtonPlayList = new List<Button>(arrBtn);

        // cache component on quizgame
        var gridArray = allQuestion.GetComponentsInChildren<ComQuestion>();
        foreach (ComQuestion gird in gridArray)
            listQuestion.Add(gird.gameObject);

        foreach (ComQuestion cQuestion in gridArray)
        {
            var arrChoice = cQuestion.GetComponentsInChildren<ComChoice>();
            foreach(ComChoice ch in arrChoice)
                ch.CacheComponent();
        }
    }

    private void LoadTextButtonPlayList()
    {
        int i = 0;
        var arr = storeMgr.playLists;
        foreach (Playlist plist in arr)
        {
            listButtonPlayList[i++].GetComponentInChildren<Text>().text = plist.playlist;
        }

    }
    #endregion
}
