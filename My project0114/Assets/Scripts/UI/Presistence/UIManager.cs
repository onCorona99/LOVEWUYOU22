using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager instance { get; private set; }

    public UICommonPopUp UICommonPopUp;

    private void Awake()
    {
        // µ¥Àý
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }

    public void ShowUICommonPopUp(string str)
    {
       
        UICommonPopUp.SetTextShow(str);
    }
   
}
