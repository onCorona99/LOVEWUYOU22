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
    /// OnEnter时 处于栈顶 执行Serializable和InitListener
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
/// 面板的类型信息
/// </summary>
public class PanelType
{
    /// <summary>
    /// 面板的名称
    /// </summary>
    public string Name;

    /// <summary>
    /// 面板的存储路径
    /// </summary>
    public string Path;

    public PanelType(string path)
    {
        Path = path;
        //截取斜杠后的全部字符
        Name = path.Substring(path.LastIndexOf('/') + 1);
    }
}