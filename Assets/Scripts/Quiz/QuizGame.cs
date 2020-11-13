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
    private List<AudioClip> listAudioQuiz;

    //
    private bool hasAnswer = false;

    public ComChoice[] choice;
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

    #region ANSWER THE QUESTION
    public void OnPressButtonAnswer()
    {
        if(hasAnswer)
            return;

        string nameIndex = EventSystem.current.currentSelectedGameObject.name;
        listIndexAnswered.Add(int.Parse(nameIndex));
    
        CheckAnswerTheQuestion(nameIndex);
        StartCoroutine("ChangeStateQuestionWithDelay", timeDelayPerQuestion);
    }

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
            if(int.Parse(nameIndex) == i) // the answer of user
            {
                //set animation the anser       
                var ani = arrChoice[i].gameObject.GetComponent<Animator>();

                //set color the answer
                var img = arrChoice[i].gameObject.GetComponent<Image>();
                img.color = Color.red;

                if(int.Parse(answerIndex) == int.Parse(nameIndex))
                {
                    img.color = Color.green;
                    ani.SetTrigger("Correct");
                }
                else
                {  
                    ani.SetTrigger("Wrong");
                }
                    
            }
            else if (int.Parse(answerIndex) == i) // the correct answer
            {
                //set animation the anser 
                // var ani = arrChoice[i].gameObject.GetComponent<Animator>();
                // ani.SetTrigger("Correct");

                //set color the answer
                var img = arrChoice[i].gameObject.GetComponent<Image>();
                img.color = Color.green;
            }
            else
            {
                SetChoiceAfterAnswer(arrChoice[i].gameObject, false);
            }
            
                
        }

        hasAnswer = true;
        scoreMgr.UpdateScoreAndQuestion();
    }

    public void SetChoiceAfterAnswer(GameObject gameObject, bool value)
    {
        if (!value)
        {
            var img = gameObject.GetComponent<Image>();
            img.color = new Color(img.color.r, img.color.g, img.color.b, 0.1f);
        }
    }

    IEnumerator ChangeStateQuestionWithDelay(float timer)
    {
        yield return new WaitForSeconds(timer);

        hasAnswer = false;
        ChangeStateQuestion(LoadNextQuestion());
    }

    public EStateQuestion LoadNextQuestion()
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

    #region EVENT BUTTONS
    public void OnPressButtonPlayList()
    {
        string nameBtn = EventSystem.current.currentSelectedGameObject.name;

        // load data from storage
        storeMgr.LoadPlayListQuestion(nameBtn);
        listAudioQuiz = storeMgr.GetCurrentListAudioQuiz();

        // loading question one of Quiz
        ChangeStateQuestion(EStateQuestion.One);
        sceneMgr.ChangeState(sceneMgr.sceneQuiz);
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

        sceneMgr.sceneQuiz.SetRandomBG();
        LoadQuestionData((int)currentStateQuestion);
    }

    public void LoadQuestionData(int indexName)
    {
        string strIndexName = indexName.ToString();

        currentChoices = storeMgr.GetCurrentChoicesQuestionIndex(indexName);
        answerIndex = storeMgr.GetCurrentAnswerQuestionIndex(indexName);
        soundMgr.PlaySound(listAudioQuiz[indexName]);

        LoadTextChoicesQuestion(indexName);
        
        foreach (GameObject ob in listQuestion)
        {
            ob.SetActive(strIndexName.Contains(ob.name));
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

        //
        listIndexAnswered.Clear();
        ResetColor();

        // reset origin color of choice 
        foreach(GameObject ob in listQuestion)
        {        
            var choice = ob.GetComponentsInChildren<ComChoice>();
            foreach(ComChoice com in choice)
            {   
                var img = com.gameObject.GetComponent<Image>();
                img.color = new Color(img.color.r, img.color.g, img.color.b, 1f);
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

        var gridArray = allQuestion.GetComponentsInChildren<ComQuestion>();
        foreach (ComQuestion gird in gridArray)
            listQuestion.Add(gird.gameObject);
    }

    private void LoadTextButtonPlayList()
    {
        int i = 0;
        var arr = storeMgr.playLists;
        foreach (Playlist li in arr)
        {
            listButtonPlayList[i++].GetComponentInChildren<Text>().text = li.playlist;
        }

    }

    private void LoadTextChoicesQuestion(int index)
    {
        arrChoice = listQuestion[index].GetComponentsInChildren<ComChoice>();

        for (int i = 0; i < currentChoices.Count; i++)
        {
            //Debug.Log(currentChoices[i].title);
            arrChoice[i].GetComponentInChildren<Text>().text = currentChoices[i].title;
        }
    }
    #endregion
}
