using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ����������������������壬���洢�򿪵�������Ϣ
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
    /// ���ֵ�洢���д򿪵���壬ÿһ��������Ӧһ�����
    /// </summary>
    public Dictionary<PanelType, GameObject> panelDict;

    /// <summary>
    /// ���캯������ʼ���ֵ�
    /// </summary>
    public PanelManager()
    {
        panelDict = new Dictionary<PanelType, GameObject>();
    }

    /// <summary>
    /// Instantiateһ����� �������ֵ�
    /// </summary>
    /// <param name="type">UI����</param>
    /// <returns></returns>
    public GameObject ShowPanel(PanelType type)
    {
        //������ָ��Ϊ���ĸ�����
        GameObject canvas = GameObject.Find("Canvas");

        if (!canvas)
        {
            UnityEngine.Debug.Log("Error:����������");
            return null;
        }
        //����ֵ�����ָ����������Ϣ���򷵻�������Ķ���
        if (panelDict.ContainsKey(type))
            return panelDict[type];
        //��ָ��������¡��������
        GameObject panel = GameObject.Instantiate(Resources.Load<GameObject>(type.Path), canvas.transform);
        //�趨�����������
        panel.name = type.Name;
        //�����ֵ�
        panelDict.Add(type, panel);
        return panel;
    }

    /// <summary>
    /// �ر�һ�����
    /// </summary>
    public void DestroyPanel(PanelType type)
    {
        if (panelDict.ContainsKey(type))
        {
            //����ָ�������
            Object.Destroy(panelDict[type]);
            //���ֵ��Ƴ������ļ�ֵ��
            panelDict.Remove(type);
        }
    }
}


/// <summary>
/// ���֮ջ�����ڹ���������Ӧ˳��
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
    /// �洢����ջ
    /// </summary>
    public Stack<BasePanel> stack;

    //����������
    private BasePanel panel;

    /// <summary>
    /// ��ʼ�����֮ջ
    /// </summary>
    public PanelStack()
    {
        stack = new Stack<BasePanel>();
    }

    /// <summary>
    /// �����ʱ�������ջ
    /// </summary>
    /// <param name="nextPanel"></param>
    public void Push(BasePanel nextPanel)
    {
        //��������������ʾ
        if (stack.Count > 0)
        {
            //ȡջ��
            panel = stack.Peek();
            //��ͣ��һ�����
            panel.OnPause();
        }
        //�������ջ
        stack.Push(nextPanel);
        //��ȡһ�����
        GameObject panelToShow = PanelManager.Instance.ShowPanel(nextPanel.PanelType);

        //������ʱҪִ�е�����
        nextPanel.OnEnter();
    }

    /// <summary>
    /// �ر����ʱ������ջ������ջ������壬�ٻָ���ջ�������
    /// </summary>
    public void Pop()
    {
        if (stack.Count > 0)
        {
            stack.Pop().OnExit();

            //ջ��������˳�
            //stack.Peek().OnExit();
            //����ջ
            //stack.Pop();
        }
        //�ָ��²����
        if (stack.Count > 0)
            stack.Peek().OnResume();
    }

    /// <summary>
    /// �ر�������岢ִ�����˳�����
    /// </summary>
    public void PopAll()
    {
        while (stack.Count > 0)
            stack.Pop().OnExit();
    }
}