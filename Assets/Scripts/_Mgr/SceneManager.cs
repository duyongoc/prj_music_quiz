using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SceneManager : MonoBehaviour
{
    
    #region FIELDS
    [Header("All of scene in game")]
    public SceneWelcome sceneWelcome;
    public SceneQuiz sceneQuiz;
    public SceneResult sceneResult;

    [Header("Current state scene")]
    private StateScene currentState; 
    public StateScene CurrentState { get => currentState; set => currentState = value; }
    #endregion

    #region Init
    public static SceneManager s_instance;
    private void Awake()
    {
        if(s_instance != null)
            return;
        s_instance = this;

    }
    #endregion

    private void Start()
    {
        sceneWelcome.gameObject.SetActive(true);
        sceneQuiz.gameObject.SetActive(true);
        sceneResult.gameObject.SetActive(true);

        ChangeState(sceneWelcome);
        
    }

    private void FixedUpdate()
    {
        if(CurrentState != null)
        {
            CurrentState.UpdateState();
        }
    }

    public void ChangeState(StateScene newState)
    {
        if(currentState != null)
        {
            currentState.EndState();
        }

        currentState = newState;

        if(currentState != null)
        {
            currentState.Owner = this;
            currentState.StartState();
        }
    }


    public void SetActivePanelScene(string panelName)
    {
        sceneQuiz.gameObject.SetActive(panelName.Contains(sceneQuiz.name));
        sceneResult.gameObject.SetActive(panelName.Contains(sceneResult.name));
        sceneWelcome.gameObject.SetActive(panelName.Contains(sceneWelcome.name));
    }

    public static SceneManager GetInstance()
    {
        return s_instance;
    }

    public bool IsStateInGame()
    {
        return true;
    }

}
