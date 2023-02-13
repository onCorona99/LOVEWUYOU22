using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance { get; private set; }

    public PlayerInfoList PlayerInfo;

    public PlayerInfoData CurrentPlayerInfo;

    private void Awake()
    {
        // µ¥Àý
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);

        PlayerInfo = JsonManager.Instance.LoadFromJsonDate<PlayerInfoList>("/StreamingAssets/Json/PlayerInfo/PlayerInfo.json");
    }

    // Start is called before the first frame update
    void Start()
    {
        CurrentPlayerInfo = new PlayerInfoData();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
