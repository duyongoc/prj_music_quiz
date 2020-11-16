using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class song
{
    public string id;
    public string title;
    public string artist;
    public string picture;
    public string sample;
}

[Serializable]
public class choices
{
    public string artist;
    public string title;
}

[Serializable]
public class questions
{
    public string id;
    public string answerIndex;
    public choices[] choices;
    public song song;
}

[Serializable]
public class Playlist
{
    public string id;
    public questions[] questions;
    //public string questions;
    public string playlist;
}

[Serializable]
public class AudioQuiz
{
    public List<AudioClip> listAudio;

}
