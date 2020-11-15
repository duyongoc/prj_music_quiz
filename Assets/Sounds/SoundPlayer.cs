using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

public class SoundPlayer : MonoBehaviour
{
    //Directory of folder to be searched anywhere on the computer
    public string FileDirectory;

    //Audio source
    public AudioSource Source;
    //Current sound playing
    public AudioClip Clip;

    //List of all valid directories
    List<string> Files = new List<string>();
    //List of all AudioClips
    public List<AudioClip> Clips = new List<AudioClip>();

    [System.Obsolete]
    private void Start()
    {
        Source = GetComponent<AudioSource>();

        //Grabs all files from FileDirectory
        string[] files;
        files = Directory.GetFiles(FileDirectory);

        //Checks all files and stores all WAV files into the Files list.
        for (int i = 0; i < files.Length; i++)
        {
            if (files[i].EndsWith(".wav"))
            {
                Files.Add(files[i]);
                Clips.Add(new WWW(files[i]).GetAudioClip(false, true, AudioType.WAV));
            }
        }
        //Calls the below method
        PlaySong(0);
    }
    public void PlaySong(int _listIndex)
    {
        Clip = Clips[_listIndex];
        Source.clip = Clip;
        Source.Play();
    }
}
