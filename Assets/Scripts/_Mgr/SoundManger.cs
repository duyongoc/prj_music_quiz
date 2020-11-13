using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManger : MonoBehaviour
{
    #region FIELDS
    //
    // public 
    //
    [Header("The use's score reach in per playlist")]
    public int totalScore;

    //
    // private
    //
    private AudioSource audioSource;
    #endregion

//==

    #region SINGLETON
    public static SoundManger s_instance;

    private void Awake()
    {
        if(s_instance != null)
            return;
        
        CacheComponent();
        s_instance = this;
    }

    public static SoundManger GetInstance()
    {
        return s_instance;
    }
    #endregion

//==

    #region UNITY
    private void Start()
    {
        CacheComponent();
    }

    private void Update()
    {

    }
    #endregion

//==

    #region PUBLIC FUNCTION
    public void PlaySoundOneShot(AudioClip audi)
    {
        audioSource.PlayOneShot(audi);
    }

    public void PlayOnlySoundOneShot(AudioClip audi)
    {
        StopSound();
        audioSource.PlayOneShot(audi);
    }
    
    public void PlaySound(AudioClip audi)
    {
        StopSound();
        audioSource.clip = audi;
        audioSource.Play();
        audioSource.loop = true;
    }

    public bool IsPlaying(AudioClip audi)
    {
        return (audioSource.clip == audi && audioSource.isPlaying);
    }

    public void StopSound()
    {
        audioSource.Stop();
    }
    #endregion

    #region PRIVATE FUNCTION
    private void CacheComponent()
    {
        // cache component
        audioSource = GetComponent<AudioSource>();
    }
    #endregion
}
