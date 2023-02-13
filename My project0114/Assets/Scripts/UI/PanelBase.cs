using UnityEngine;

public class BasePanel
{
    public PanelType PanelType { get; private set; }

    public bool isVisibleWhenPause;
    public bool isActiableWhenPause;

    public BasePanel(PanelType panelType)
    {
        PanelType = panelType;
    }

    public virtual void Serializable()
    {

    }

    public virtual void InitListener() { }
    /// <summary>
    /// OnEnterʱ ����ջ�� ִ��Serializable��InitListener
    /// </summary>
    public virtual void OnEnter()
    {
        Serializable();
        InitListener();
    }

    public virtual void OnPause() 
    {
        var canvasGroup = PanelExtensionTool.GetOrAddComponent<CanvasGroup>();
        canvasGroup.blocksRaycasts = false;
        canvasGroup.alpha = isVisibleWhenPause ? 1 : 0;
    }

    public virtual void OnResume() 
    {
        var canvasGroup = PanelExtensionTool.GetOrAddComponent<CanvasGroup>();
        canvasGroup.blocksRaycasts = true;
        canvasGroup.alpha = 1;
    }

    public virtual void OnExit() { }
}

/// <summary>
/// ����������Ϣ
/// </summary>
public class PanelType
{
    /// <summary>
    /// ��������
    /// </summary>
    public string Name;

    /// <summary>
    /// ���Ĵ洢·��
    /// </summary>
    public string Path;

    public PanelType(string path)
    {
        Path = path;
        //��ȡб�ܺ��ȫ���ַ�
        Name = path.Substring(path.LastIndexOf('/') + 1);
    }
}