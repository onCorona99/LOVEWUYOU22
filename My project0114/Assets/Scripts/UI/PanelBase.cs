public class BasePanel
{
    public PanelType PanelType { get; private set; }

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

    public virtual void OnPause() { }

    public virtual void OnResume() { }

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