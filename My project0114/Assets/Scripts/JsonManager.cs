using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using System.IO;
using System;
using System.Text;

public class JsonManager
{
    private static JsonManager instance;
    public static JsonManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new JsonManager();
            }
            return instance;
        }
    }

    /// <summary>
    /// ��ȡ
    /// </summary>
    public T LoadJsonDatas<T>(string path)
    {
        StreamReader sr = new StreamReader(path);
        string readContent = sr.ReadToEnd();
        T jsonDatas = JsonMapper.ToObject<T>(readContent);
        sr.Close();
        sr.Dispose();
        return jsonDatas;
    }
    //
    public T LoadFromJsonDate<T>(string UnityAssetSubPath)
    {
        string filePath = Application.dataPath + UnityAssetSubPath;
        try
        {
            T t = JsonMapper.ToObject<T>(File.ReadAllText(filePath));
            if (t == null)
            {
                Debug.Log("JsonDate=Null");
                return default(T);
            }
            return t;
        }
        catch (Exception E)
        {
            Debug.Log($"<color=#ff0000>Json�ļ�:{filePath}������</color>");
            Debug.Log(E);
            return default(T);
        }
    }

    /// <summary>
    /// ���� dirPath eg:/StreamingAssets/Json/PlayerInfo/
    /// </summary>
    public string SaveJsonDate<T>(T t, string dirPath,string fileName)
    {
        string fullDirPath = Application.dataPath + dirPath;

        if (!Directory.Exists(fullDirPath))
        {
            Directory.CreateDirectory(fullDirPath);
        }

        string filePath = fullDirPath + fileName;
        Debug.Log($"{filePath}");

        FileInfo file = new FileInfo(filePath);


        string jsonstr = JsonMapper.ToJson(t);
        using (System.IO.StreamWriter thefile = new StreamWriter(filePath, false, Encoding.UTF8))
        {
            thefile.WriteLine(jsonstr);
        }
        //�ڱ༭����
#if UNITY_EDITOR
        UnityEditor.AssetDatabase.Refresh();
#endif
        return jsonstr;
    }
}