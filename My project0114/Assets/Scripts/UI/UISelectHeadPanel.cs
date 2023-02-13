using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UISelectHeadPanel : BasePanel
{
    public UISelectHeadPanel() : base(new PanelType(PathManager.UISelectHeadPanel)) { }

    public class Controls
    {
        public Transform HeadImgList;
    }

    public Controls m_ctl;


    public override void OnEnter()
    {
        base.OnEnter();
        var blocker = PanelManager.Instance.panelDict[PanelStack.Instance.stack.Peek().PanelType].GetComponent<Blocker>();
        blocker.RegisterEvent(() => PanelStack.Instance.Pop());

        InitHeadImgGrid();
    }

    private void InitHeadImgGrid()
    {
        var heads = Resources.LoadAll<Sprite>("Pic/HeadPortrait");
        GameObject UIHeadImgGridItem = Resources.Load<GameObject>("Prefabs/UI/UIHeadImgGridItem");
        foreach (var item in heads)
        {
            var go = UnityEngine.GameObject.Instantiate(UIHeadImgGridItem, m_ctl.HeadImgList);
            var Icon = GameUtility.FindChild(go, "Icon");
            Icon.GetComponent<Image>().sprite = item;
            GameUtility.GetOrAddComponent<Button>(Icon).onClick.AddListener(
                () =>
                {
                    PanelStack.Instance.Pop();
                    foreach (var panel in PanelStack.Instance.stack)
                    {
                        if (panel.PanelType.Path.Equals(PathManager.UICreateSavePanel))
                        {
                            (panel as UICreateSavePanel).m_ctl.PlayerHead.image.sprite = item;
                        }
                        continue;
                    }
                });
            Debug.Log(item.name);
        }
    }

    public override void Serializable()
    {
        GameUtility.FindControls(PanelManager.Instance.panelDict[PanelStack.Instance.stack.Peek().PanelType], ref m_ctl);
    }
    public override void InitListener()
    {
        
    }


    public override void OnExit()
    {
        PanelManager.Instance.DestroyPanel(this.PanelType);
    }
}