using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SaveManager
{
    public static void Save<T>(T data)
    {
        PlayerPrefs.SetString(typeof(T).ToString(), JsonUtility.ToJson(data));
    }

    public static T Load<T>() where T : class, new()
    {
        var res = JsonUtility.FromJson<T>(PlayerPrefs.GetString(typeof(T).ToString()));
        if (res == null)
            res = new T();
        return res;
    }
}
