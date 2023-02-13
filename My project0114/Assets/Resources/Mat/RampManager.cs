using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class RampManager : MonoBehaviour
{
    public string path = "newTex.png";

    public int width = 64;
    public int height = 8;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GenerateRampTex()
    {
        Debug.Log("开始生成渐变纹理");
        Texture2D tex = new Texture2D(width, height);
        Debug.Log($"Width:{tex.width} Height:{tex.height}");

        for (int i = 0; i < tex.width; i++)
        {
            for (int j = 0; j < tex.height; j++)
            {
                var value = i / (float)tex.width;
                Debug.Log($"{value}");
                Color c = new Color(0,0,0,1);
                if (value <= 0.33f)
                {
                    c = new Color(1, 1, 1,1);
                }
                else if (value <= 0.67f)
                {
                    c = new Color(1-value,1- value, 1-value,1);
                }
                else if (value <= 1f)
                {
                    c = new Color(0, 0, 0,1);
                }
                tex.SetPixel(i,j, c);
                tex.Apply();

            }
        }
        SaveTextureToFile(tex);
    }

    public void SaveTextureToFile(Texture2D tex)
    {
        var bytes = tex.EncodeToPNG();
        //File.WriteAllBytes(Application.dataPath + "/" + fileName, bytes);

        string savePath = Application.dataPath + "/" + path ;
        FileStream fileStream = File.Open(savePath, FileMode.OpenOrCreate);
        fileStream.Write(bytes, 0, bytes.Length);
        fileStream.Close();
        //var binary = new BinaryWriter(file);
        //binary.Write(bytes);
        //file.Close();
    }
}
