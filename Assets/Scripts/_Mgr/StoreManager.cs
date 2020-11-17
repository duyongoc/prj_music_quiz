using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class StoreManager : MonoBehaviour
{
    #region FIELDS
    //
    //  public variable
    //
    [Header("List audio of game quiz")]
    public List<AudioClip> listAllAudio = new List<AudioClip>();

    [Header("List Sprite of game quiz")]
    public List<Sprite> listAllSprite = new List<Sprite>();

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
    private AudioClip targetAudioClip;

    //
    private Sprite targetSprite;
    #endregion

    //---

    #region SINGLETON
    public static StoreManager s_instance;

    private void Awake()
    {
        if (s_instance != null)
            return;
        // hardcode for test purpose
#if UNITY_EDITOR
        LoadData();

        DownloadAudioResoures();
        DownloadTextureResources();
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

    }

    public string GetCurrentAnswerQuestionIndex(int index)
    {
        return currentListQuestion[index].answerIndex;
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

    #region GET DOWNLOAD SOUNDS
    public AudioClip GetAudioClipFromList(string str)
    {
        //Debug.Log(str);
        AudioClip audi = listAllAudio.Find(x => x.name == str);
        if (audi == null)
        {
            //Trying redownload reosoures
            StartCoroutine(RedownloadAndPlayAudioClip(str));

            // When redownload audio still faied. So hardcode loading them from resoures folder
            // Currently we use this bad solution while find another way better
            audi = Resources.Load<AudioClip>("Sounds/" + str);
        }

        return audi;
    }

    public void DownloadAudioResoures()
    {
        for (int i = 0; i < playLists.Count; i++)
        {
            var question = playLists[i].questions;
            for (int j = 0; j < question.Length; j++)
            {
                string strUrl = question[j].song.sample;
                string strTitle = question[j].song.title;

                StartCoroutine(GetAudioClip(strUrl, strTitle, (response) =>
                {
                    targetAudioClip = response;
                    targetAudioClip.name = strTitle;
                    listAllAudio.Add(targetAudioClip);
                }));
            }
        }
    }

    IEnumerator GetAudioClip(string strUrl, string strTitle, System.Action<AudioClip> callback)
    {
        using (UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip(strUrl, AudioType.WAV))
        {
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
            }
            else
            {
                if (www.isDone)
                {
                    var audio = DownloadHandlerAudioClip.GetContent(www);
                    callback(audio);
                }
            }
        }
    }

    IEnumerator RedownloadAndPlayAudioClip(string strUrl)
    {
        using (UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip(strUrl, AudioType.WAV))
        {
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
            }
            else
            {
                targetAudioClip = DownloadHandlerAudioClip.GetContent(www);
                SoundManger.GetInstance().PlaySound(targetAudioClip);
            }
        }
    }
    #endregion

    #region GET DOWNLOAD IMAGES
    public void DownloadTextureResources()
    {
        for (int i = 0; i < playLists.Count; i++)
        {
            var question = playLists[i].questions;
            for (int j = 0; j < question.Length; j++)
            {
                string strUrl = question[j].song.picture;
                string strTitle = question[j].song.title;

                StartCoroutine(GetTextureRequest(strUrl, strTitle, (response) =>
                {
                    targetSprite = response;
                    targetSprite.name = strTitle;
                    listAllSprite.Add(targetSprite);
                }));
            }
        }
    }

    public Sprite GetSpiteFromList(string str)
    {
        // Debug.Log(str);
        Sprite sprite = listAllSprite.Find(x => x.name == str);
        if (sprite == null)
        {
            // When download Sprite failed. So hardcode loading them from resoures folder
            // Currently we use this bad solution while find another way better
            sprite = Resources.Load<Sprite>("MusicPic/" + str);
        }

        return sprite;
    }

    IEnumerator GetTextureRequest(string url, string strTitle, System.Action<Sprite> callback)
    {
        using (var www = UnityWebRequestTexture.GetTexture(url))
        {
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
            }
            else
            {
                if (www.isDone)
                {
                    var texture = DownloadHandlerTexture.GetContent(www);
                    var rect = new Rect(0, 0, texture.width, texture.height);
                    var sprite = Sprite.Create(texture, rect, new Vector2(0.5f, 0.5f));
                    callback(sprite);
                }
            }
        }
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
    #endregion
}
