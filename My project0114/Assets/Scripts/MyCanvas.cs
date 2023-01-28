using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyCanvas : MonoBehaviour
{
    public static MyCanvas instance { get; private set; }

    private void Awake()
    {
        // ����
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }
}
