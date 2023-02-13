using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISaveSlot : MonoBehaviour
{
    public Text playerInfoText;
    public Image playerHeadPortraitImg;

    public Button button;

    public int saveIndex = 0; // 从0开始

    public void Awake()
    {
        //InitListener(); // 这样会使得ScrollView拖拽不懂 判断是射线检测问题

        button.onClick.AddListener(
            () =>
            {
                Debug.Log($"ChooseSave");
                if (true)
                {
                    SceneSystem.GetInstance().SetScene(new MainScene());
                    GameManager.instance.CurrentPlayerInfo.SetInfoData(GameManager.instance.PlayerInfo.datas[saveIndex]);
                    PanelStack.Instance.PopAll();
                }
            });
    }

    public void Start()
    {
        Debug.Log($"UISaveSlot:Start  SaveIndex:{saveIndex}");
    }

    public void SetText(string name, int level, int goldCount)
    {
        playerInfoText.text = $"昵称:<color=#ffffff>{name}</color>\n等级:{level}\t\t金币:{goldCount}";
    }

    public void SetImage(string imgPathName)
    {
        var path = "Pic/HeadPortrait/" + imgPathName;
        Sprite Head = Resources.Load<Sprite>(path);
        if (Head)
        {
            playerHeadPortraitImg.sprite = Head;

        }
        else
        {
            Debug.Log($"图片路径:【{path}】不存在");
        }
    }

    public void ChooseSlot()
    {
        Debug.Log($"ChooseSave");
        if (true)
        {
            SceneSystem.GetInstance().SetScene(new MainScene());
            PanelStack.Instance.PopAll();
        }
    }

    public void InitListener()
    {
        UIEventTriggerListener.Get(button.gameObject).OnClick = ChooseSave;
    }

    /// <summary>
    /// 现在我们让选择好角色存档后就进游戏
    /// </summary>
    public void ChooseSave(GameObject go)
    {
        Debug.Log($"ChooseSave");
        if (true)
        {
            SceneSystem.GetInstance().SetScene(new MainScene());
            PanelStack.Instance.PopAll();
        }
    }
}
