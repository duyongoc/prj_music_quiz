using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateScene : MonoBehaviour
{
    //reference to our state machine
    protected SceneManager owner;
    public SceneManager Owner { get => owner; set => owner = value; }

    //Method called to prepare state
    public virtual void StartState()
    {
    }

    //Method called to update state on every frame
    public virtual void UpdateState()
    {
    }

    //Method called to destroy state
    public virtual void EndState()
    {
    }
}
