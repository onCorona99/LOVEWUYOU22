using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// ����ű����ڵ���հ�����  �ɵ������ش�����(������������  ���Լ�����ص�����)
/// </summary>
public class Blocker : MonoBehaviour
{
    //Canvasͨ�������ܵ���¼�  ��������   https://docs.unity3d.com/Manual/script-GraphicRaycaster.html
    public GraphicRaycaster RaycastInCanvas;
    //�����֮�»��ж��Ƕ�� ���� ���A�����ֽ���PanelA  PanelA����������ť B  C  D �ֱ��ӦPanelB  PanelC  PanelD  ������PanelҲͬ������˴˽ű������Լ���Canvas  ������������Panelʱ  Ϊ�˲���PanelA�����ػ�ɵ� ��Ҫ��������Panel�����List��
    public List<Blocker> blockers;
    EventSystem eventSystem;

    //Ϊ�˱����ж��¼�ʱ ��ִ��һ�λص�����
    bool state;

    void Start()
    {
        eventSystem = EventSystem.current;
        if (RaycastInCanvas == null)
            RaycastInCanvas = GetComponentInParent<GraphicRaycaster>();

        CB += CBNEW;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            state = true;
        }
        if (Input.GetMouseButtonUp(0))
        {
            if (state)
                CheckGuiRaycastObjects();
        }
    }

    public void CBNEW()
    {
        Debug.Log("ui aabb block");
    }

    public delegate void CallBack();
    public CallBack CB;
    /// <summary>
    /// ע�����հ�����ʱ���¼�������  ����  �ɵ�   ���Լ�����ִ�з�����
    /// </summary>
    /// <param name="callback"></param>
    public void RegisterEvent(CallBack callback)
    {
        this.CB = callback;
    }

    /// <summary>
    /// �Ƿ����ڿհ�����
    /// </summary>
    /// <returns></returns>
    public bool CheckGuiRaycastObjects()
    {
        //��ִ����һ��֮��  ����������Ͳ���ִ�д˷���
        state = false;

        //��ȡ������������߼�⵽��UI
        PointerEventData eventData = new PointerEventData(eventSystem);
        eventData.pressPosition = Input.mousePosition;
        eventData.position = Input.mousePosition;
        List<RaycastResult> list = new List<RaycastResult>();
        RaycastInCanvas.Raycast(eventData, list);
        if (list.Count > 0)
        {
            for (int i = 0; i < list.Count; i++)
            {
                GameObject go = list[i].gameObject;
                if (go.GetComponentInParent<Blocker>() != null)
                {
                    return false;
                }
            }
        }
        //�ݹ�����Ӽ� ���Ƿ����ڿհ�����
        if (blockers != null)
        {
            foreach (var item in blockers)
            {
                if (item.gameObject.activeSelf && !item.CheckGuiRaycastObjects())
                {
                    return false;
                }
            }
        }
        CB?.Invoke();
        return true;
    }

}