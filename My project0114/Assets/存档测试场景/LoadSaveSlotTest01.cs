using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadSaveSlotTest01 : MonoBehaviour
{
    PlayData PlayData;
    public PlayerInfoList InfoList;
    public string Name;
    public string ID;
    public int GoldCount;
    public string HeadImgPath;
    public int Level;

    void Start()
    {
        PlayData = new PlayData();
        Debug.Log($"<color=#00ff00>{1}</color>");
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            //读取数据
            PlayData PlayData = JsonManager.Instance.LoadFromJsonDate<PlayData>("/StreamingAssets/Json/PlayerInfo/Player01.json");
            if (PlayData!=null)
            {
                Debug.Log(PlayData.GoldCount);
                Debug.Log(PlayData.ID);
                Debug.Log(PlayData.Name);
                Debug.Log(PlayData.Level);
                Debug.Log(PlayData.HeadImgPath);
            }
        }
    }

    [ContextMenu("保存玩家信息")]
    public void SaveDatas()
    {
        InfoList = new PlayerInfoList();
        var datas = new List<PlayData>();
        var p1 = new PlayData() { ID = "114514", Name = "mike", GoldCount = 100 ,HeadImgPath = "21718947_m_Part1X1" };
        var p2 = new PlayData() { ID = "114515", Name = "nikoo", GoldCount = 200,HeadImgPath = "22059716_m_Part1X1" };
        var p3 = new PlayData() { ID = "114516", Name = "ekko", GoldCount = 300,HeadImgPath = "19011009_m_Part1X1" };
        datas.Add(p1);
        datas.Add(p2);
        datas.Add(p3);
        InfoList.datas = datas;
        JsonManager.Instance.SaveJsonDate(InfoList, "/StreamingAssets/Json/PlayerInfo/", "PlayerInfo.json");
    }

    [ContextMenu("读取玩家信息")]
    public void LoadDatas()
    {
        PlayerInfoList datas = JsonManager.Instance.LoadFromJsonDate<PlayerInfoList>("/StreamingAssets/Json/PlayerInfo/PlayerInfo.json");
        foreach (var item in datas.datas)
        {
            Debug.Log($"{item.ID}");
        }
        Debug.Log($"{datas.datas.Count}");
    }
}

[System.Serializable]
public class PlayData
{
    public string ID;
    public string Name;
    public int GoldCount;
    public int Level;
    public string HeadImgPath;

    public PlayData(string iD, string name, int goldCount, int level, string headImgPath)
    {
        ID = iD;
        Name = name;
        GoldCount = goldCount;
        Level = level;
        HeadImgPath = headImgPath;
    }

    public PlayData(string name, string headImgPath)
    {
        Name = name;
        HeadImgPath = headImgPath;
    }

    public PlayData()
    {

    }
}

[System.Serializable]
public class PlayerInfoList
{
    public List<PlayData> datas;
}
