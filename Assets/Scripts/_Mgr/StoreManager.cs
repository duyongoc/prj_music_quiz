using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class StoreManager : MonoBehaviour
{
    #region FIELDS
    //
    //  public variable
    //
    [Header("List audio of game quiz")]
    public AudioQuiz[] listAllAudio;
    public List<AudioClip> currentListAudio;

    [Header("List Sprite of game quiz")]
    public SpriteQuiz[] listAllSprite;
    public List<Sprite> currentListSprite;

    [Header("Random BG in Quiz Game")]
    public Sprite[] arrSpriteBG;

    [Header("Loading json data")]
    public Text txtPlatform;
    public List<Playlist> playLists;


    [Header("Current questions in quiz")]
    public string idPlayListQuiz;
    public List<questions> currentListQuestion;

    //
    //  private variable
    //
    private string nameFile = "Data";
    #endregion

//---

    #region SINGLETON
    public static StoreManager s_instance;

    private void Awake()
    {
        if(s_instance != null)
            return;
#if UNITY_EDITOR
        LoadData();
#endif
        s_instance = this;
    }

    public static StoreManager GetInstance()
    {
        return s_instance;
    }
    #endregion


    #region UNiTY
    private void Start()
    {
        
    }

    private void Update()
    {

    }
    #endregion


    #region PUBLIC FUNCTION
    public void LoadPlayListQuestion(string index)
    {
        idPlayListQuiz = playLists[int.Parse(index)].id;
        currentListQuestion = new List<questions>(playLists[int.Parse(index)].questions);

        // load current audio and sprite in curent playlist
        currentListAudio = listAllAudio[int.Parse(index)].listAudio;
        currentListSprite = listAllSprite[int.Parse(index)].listSprite;
    }

    public string GetCurrentAnswerQuestionIndex(int index)
    {
        return currentListQuestion[index].answerIndex;
    }

    public List<AudioClip> GetCurrentListAudioQuiz()
    {
        return currentListAudio;
    }

    public List<choices> GetCurrentChoicesQuestionIndex(int index)
    {
        List<choices> listChoices;
        listChoices = new List<choices>(currentListQuestion[index].choices);
        return listChoices;
    }

    public List<questions> GetListQuestionQuiz()
    {
        return currentListQuestion;
    }
    #endregion

//---
    #region PUBLIC FUNCTION
    public Sprite GetRandomBackground()
    {
        int rand = Random.Range(0, arrSpriteBG.Length);
        return arrSpriteBG[rand];
    }
    #endregion

    #region PRIVATE FUNCTION
    public void LoadData()
    {
        txtPlatform.text = "Other";

#if UNITY_ANDROID && !UNITY_EDITOR
        txtPlatform.text = "Android";
        //LoadDataAndroid();
#elif UNITY_STANDALONE_WIN
        txtPlatform.text = "Windows";
        //LoadDataWindows();
#elif UNITY_IOS 
        txtPlatform.text = "IOS";
        //LoadDataIOS();
#endif

        // make gameobject immortal during game on.
        LoadDataResources();
        DontDestroyOnLoad(this);
    }

    private void LoadDataAndroid()
    {
        CreatePathFolder();
        LoadDataJson(nameFile);
    }

    private void LoadDataIOS()
    {
        LoadDataJson(nameFile);
    }

    private void LoadDataWindows()
    {
        LoadDataJson(nameFile);
    }

    private void LoadDataResources()
    {
        LoadDataJson(nameFile);
    }

    private void LoadDataJson(string pathName)
    {
        TextAsset assetJson = Resources.Load<TextAsset>(pathName);
        string jsonStr = "{\"List\":" + assetJson.text + "}";

        //Debug.Log(jsonStr);
        Playlist[] listData = JsonHelper.FromJson<Playlist>(jsonStr);

        // save data json to store manager
        playLists = new List<Playlist>(listData);
    }

    private void CreatePathFolder()
    {
        string str = CONFIG.pathFolder + "/" + nameFile;
        Debug.Log(str);
        if(!Directory.Exists(str))
        {    
            Directory.CreateDirectory(str);
        }
    }
    #endregion
}
