using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISaveSlot : MonoBehaviour
{
    public Text playerInfoText;
    public Image playerHeadPortraitImg;

    public Button button;

    public int saveIndex = 0; // ��0��ʼ

    public void Awake()
    {
        //InitListener(); // ������ʹ��ScrollView��ק���� �ж������߼������

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
        playerInfoText.text = $"�ǳ�:<color=#ffffff>{name}</color>\n�ȼ�:{level}\t\t���:{goldCount}";
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
            Debug.Log($"ͼƬ·��:��{path}��������");
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
    /// ����������ѡ��ý�ɫ�浵��ͽ���Ϸ
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
