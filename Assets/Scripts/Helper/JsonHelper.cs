using System;
using UnityEngine;

public static class JsonHelper 
{
    
    public static T[] FromJson<T>(string json)
    {
        Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(json);
        return wrapper.List;
    }

    public static string ToJson<T>(T[] array)
    {
        Wrapper<T> wrapper = new Wrapper<T>();
        wrapper.List = array;
        return JsonUtility.ToJson(wrapper);
    }

    public static string ToJson<T>( T[] array, bool print)
    {
        Wrapper<T> wrapper = new Wrapper<T>();
        wrapper.List = array;
        return JsonUtility.ToJson(wrapper, print);
    }


    [Serializable]
    private class Wrapper<T>
    {
        public T[] List;
    }
}
