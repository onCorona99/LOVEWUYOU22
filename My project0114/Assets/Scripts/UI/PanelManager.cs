using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 面板管理器：创建或销毁面板，并存储打开的面板的信息
/// </summary>
public class PanelManager
{
    protected static PanelManager instance = null;

    public static PanelManager Instance
    {
        get
        {
            if (null == instance)
            {
                instance = new PanelManager();
            }
            return instance;
        }
    }
    /// <summary>
    /// 用字典存储所有打开的面板，每一个面板类对应一个面板
    /// </summary>
    public Dictionary<PanelType, GameObject> panelDict;

    /// <summary>
    /// 构造函数：初始化字典
    /// </summary>
    public PanelManager()
    {
        panelDict = new Dictionary<PanelType, GameObject>();
    }

    /// <summary>
    /// Instantiate一个面板 并加入字典
    /// </summary>
    /// <param name="type">UI类型</param>
    /// <returns></returns>
    public GameObject ShowPanel(PanelType type)
    {
        //将画布指定为面板的父对象
        GameObject canvas = GameObject.Find("Canvas");

        if (!canvas)
        {
            UnityEngine.Debug.Log("Error:画布不存在");
            return null;
        }
        //如果字典中有指定的面板的信息，则返回这个面板的对象
        if (panelDict.ContainsKey(type))
            return panelDict[type];
        //将指定的面板克隆到画布上
        GameObject panel = GameObject.Instantiate(Resources.Load<GameObject>(type.Path), canvas.transform);
        //设定面板对象的名字
        panel.name = type.Name;
        //加入字典
        panelDict.Add(type, panel);
        return panel;
    }

    /// <summary>
    /// 关闭一个面板
    /// </summary>
    public void DestroyPanel(PanelType type)
    {
        if (panelDict.ContainsKey(type))
        {
            //销毁指定的面板
            Object.Destroy(panelDict[type]);
            //从字典移除该面板的键值对
            panelDict.Remove(type);
        }
    }
}


/// <summary>
/// 面板之栈：用于管理面板的响应顺序
/// </summary>
public class PanelStack
{
    protected static PanelStack instance = null;

    public static PanelStack Instance
    {
        get
        {
            if (null == instance)
            {
                instance = new PanelStack();
            }
            return instance;
        }
    }

    /// <summary>
    /// 存储面板的栈
    /// </summary>
    public Stack<BasePanel> stack;

    //面板基类引用
    private BasePanel panel;

    /// <summary>
    /// 初始化面板之栈
    /// </summary>
    public PanelStack()
    {
        stack = new Stack<BasePanel>();
    }

    /// <summary>
    /// 打开面板时将面板入栈
    /// </summary>
    /// <param name="nextPanel"></param>
    public void Push(BasePanel nextPanel)
    {
        //如果还有面板在显示
        if (stack.Count > 0)
        {
            //取栈顶
            panel = stack.Peek();
            //暂停上一个面板
            panel.OnPause();
        }
        //新面板入栈
        stack.Push(nextPanel);
        //获取一个面板
        GameObject panelToShow = PanelManager.Instance.ShowPanel(nextPanel.PanelType);

        //面板进入时要执行的任务
        nextPanel.OnEnter();
    }

    /// <summary>
    /// 关闭面板时将面板出栈：弹出栈顶的面板，再恢复新栈顶的面板
    /// </summary>
    public void Pop()
    {
        if (stack.Count > 0)
        {
            stack.Pop().OnExit();

            //栈顶的面板退出
            //stack.Peek().OnExit();
            //面板出栈
            //stack.Pop();
        }
        //恢复下层面板
        if (stack.Count > 0)
            stack.Peek().OnResume();
    }

    /// <summary>
    /// 关闭所有面板并执行其退出函数
    /// </summary>
    public void PopAll()
    {
        while (stack.Count > 0)
            stack.Pop().OnExit();
    }
}