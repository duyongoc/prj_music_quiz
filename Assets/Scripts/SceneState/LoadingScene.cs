using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingScene : MonoBehaviour
{
    #region FIELDS
    [SerializeField]private Slider sliderLoading = default;

    [Header("Load scene with delay time")]
    public float timeDelay = 2f;
    public float timeProcess = 0.1f;

    private float timer = 0f;
    private float timeTmp = 0;
    #endregion

    #region UNTIY
    private void Awake()
    {
        
    }

    private void Start()
    {
        StoreManager.GetInstance().LoadData();

        timer = timeDelay;
        StartCoroutine(LoadSceneWithDelay(1, timer, timeProcess));
    }
    #endregion
    

    IEnumerator LoadScene(int index)
    {
        AsyncOperation async = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(index);
        while(!async.isDone)
        {
            Debug.Log("Loading progress: " + (async.progress * 100) + "%");
            sliderLoading.value = Mathf.Clamp01(async.progress / 0.9f);
            if(async.progress >= 0.9f)
            {
                async.allowSceneActivation = true;
            }
            yield return null;
        }
    }

    IEnumerator LoadSceneWithDelay(int index, float timer, float timeProcess)
    {
        while(timer >= 0f)
        {
            timeTmp += timeProcess;
            timer -= timeProcess;
            sliderLoading.value = timeTmp / timeDelay;
            
            // Debug.Log(timer);            
            if(timer <= 0f)
            {
                UnityEngine.SceneManagement.SceneManager.LoadScene(1);
            }

            yield return new WaitForSeconds(timeProcess);
        }
        
    }

}
