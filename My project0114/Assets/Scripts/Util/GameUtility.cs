using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public static class GameUtility
{
    public static T TryLoadResource<T>(string path) where T : UnityEngine.Object
    {
        return Resources.Load(path, typeof(T)) as T;
    }

    public static T LoadResource<T>(string path) where T : UnityEngine.Object
    {
        T val = TryLoadResource<T>(path);
        if ((UnityEngine.Object)val == (UnityEngine.Object)null)
        {
            Debug.LogWarning("Cannot load resource " + typeof(T).Name + " \"" + path + "\"");
        }

        return val;
    }

    public static T GetOrAddComponent<T>(GameObject o) where T : Component
    {
        T component = o.GetComponent<T>();
        if ((bool)(UnityEngine.Object)component)
        {
            return component;
        }

        return o.AddComponent<T>();
    }

    public static GameObject LoadGameObject(string path, bool objectUsingPrefabName = false)
    {
        GameObject gameObject = LoadResource<GameObject>(path);
        if (!gameObject)
        {
            return null;
        }

        GameObject gameObject2 = UnityEngine.Object.Instantiate(gameObject);
        if (objectUsingPrefabName && (bool)gameObject2)
        {
            gameObject2.name = gameObject.name;
        }

        return gameObject2;
    }

    public static GameObject TryLoadGameObject(string path)
    {
        GameObject gameObject = TryLoadResource<GameObject>(path);
        if (!gameObject)
        {
            return null;
        }

        GameObject gameObject2 = UnityEngine.Object.Instantiate(gameObject);
        gameObject2.name = gameObject2.name.Replace("(Clone)", "");
        return gameObject2;
    }

    public static GameObject FindChild(GameObject root, string name, bool ignoreCase = true, bool recursive = true)
    {
        if (!root)
        {
            return null;
        }

        foreach (Transform item in root.transform)
        {
            if (item.name.Equals(name, ignoreCase ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal))
            {
                return item.gameObject;
            }

            if (recursive)
            {
                GameObject gameObject = FindChild(item.gameObject, name, ignoreCase, recursive);
                if ((bool)gameObject)
                {
                    return gameObject;
                }
            }
        }

        return null;
    }

    public static void DestroyAllChildren(GameObject o)
    {
        DestroyAllChildren(o.transform);
    }

    public static void DestroyAllChildren(Transform t)
    {
        if (t.childCount == 0)
        {
            return;
        }

        Transform[] array = new Transform[t.childCount];
        int num = 0;
        foreach (Transform item in t)
        {
            Transform transform = array[num] = item;
            num++;
        }

        Transform[] array2 = array;
        foreach (Transform transform2 in array2)
        {
            Destroy(transform2.gameObject);
        }
    }

    public static void Destroy(UnityEngine.Object obj)
    {
        if (!obj)
        {
            return;
        }

        if (Application.isEditor && !Application.isPlaying)
        {
            UnityEngine.Object.DestroyImmediate(obj);
            return;
        }

        GameObject exists = obj as GameObject;
        if ((bool)exists)
        {
            UnityEngine.Object.Destroy(obj);
        }
    }

    public static void SetParent(GameObject t, GameObject parent, bool keepWorldPos)
    {
        if ((bool)t)
        {
            SetParent(t.transform, parent ? parent.transform : null, keepWorldPos);
        }
    }

    public static void SetParent(Transform t, Transform parent, bool keepWorldPos)
    {
        if ((bool)t)
        {
            if (keepWorldPos)
            {
                t.transform.parent = parent;
                return;
            }

            Vector3 localPosition = t.localPosition;
            Quaternion localRotation = t.localRotation;
            Vector3 localScale = t.localScale;
            t.transform.SetParent(parent);
            t.localPosition = localPosition;
            t.localRotation = localRotation;
            t.localScale = localScale;
        }
    }

    public static int LayerId(string name)
    {
        return LayerMask.NameToLayer(name);
    }

    public static void FindControls<T>(Component comp, ref T controls) where T : new()
    {
        FindControls(comp.gameObject, ref controls);
    }
    /// <summary>
    /// 通过反射 为UI面板Prefab 下的UI组件(如Button/InputField)赋值
    /// </summary>
    public static void FindControls<T>(GameObject obj, ref T controls) where T : new()
    {
        if (controls == null)
        {
            controls = new T();
        }

        Type type = controls.GetType();
        FieldInfo[] fields = type.GetFields();
        FieldInfo[] array = fields;
        foreach (FieldInfo fieldInfo in array)
        {
            object[] customAttributes = fieldInfo.GetCustomAttributes(typeof(NonSerializedAttribute), inherit: false);
            if (customAttributes.Length == 0)
            {
                fieldInfo.GetCustomAttributes(typeof(NonSerializedAttribute), inherit: false);
                GameObject gameObject = FindChild(obj, fieldInfo.Name);
                if (gameObject == null)
                {
                    Debug.LogError("Cannot find widget [" + fieldInfo.Name + "] in " + Fullname(obj));
                    fieldInfo.SetValue(controls, null);
                }
                else if ((object)fieldInfo.FieldType == typeof(GameObject))
                {
                    fieldInfo.SetValue(controls, gameObject);
                }
                else if (fieldInfo.FieldType.IsSubclassOf(typeof(Component)))
                {
                    Component component = gameObject.GetComponent(fieldInfo.FieldType);
                    fieldInfo.SetValue(controls, component);
                }
            }
        }
    }

    public static string Fullname(GameObject o)
    {
        if (o == null)
        {
            return "null";
        }

        if ((bool)o.transform.parent)
        {
            return Fullname(o.transform.parent.gameObject) + "/" + o.name;
        }

        return o.name;
    }

    public static string Fullname(Component o)
    {
        if (o == null)
        {
            return "null";
        }

        return Fullname(o.gameObject) + "." + o.GetType().Name;
    }

}

/// <summary>
/// 面板扩展工具：给当前活动面板或其子对象添加组件
/// </summary>
public static class PanelExtensionTool
{
    /// <summary>
    /// 给当前活动面板获取或者添加一个组件
    /// </summary>
    /// <typeparam name="T">组件类型</typeparam>
    /// <returns>组件</returns>
    public static T GetOrAddComponent<T>() where T : UnityEngine.Component
    {
        var go = PanelManager.Instance.panelDict[PanelStack.Instance.stack.Peek().PanelType];
        if (go.GetComponent<T>() == null)
            go.AddComponent<T>();
        return go.GetComponent<T>();
    }

    /// <summary>
    /// 根据名称，获取指定子对象上的组件，如果没有，则为其添加
    /// </summary>
    /// <param name="name">子对象的名称</param>
    /// <typeparam name="T">组件类型</typeparam>
    /// <returns>组件</returns>
    public static T GetOrAddComponentInChildren<T>(string name) where T : UnityEngine.Component
    {
        GameObject child = FindChildGameObject(name);
        if (child)
        {
            if (child.GetComponent<T>() == null)
                child.AddComponent<T>();
            return child.GetComponent<T>();
        }
        return null;
    }

    /// <summary>
    /// 根据名称查找顶层面板上的子对象：比如按钮
    /// </summary>
    /// <param name="name">子对象名称</param>
    public static GameObject FindChildGameObject(string name)
    {
        //存储顶层面板的子对象的全部组件的Transform属性
        Transform[] trans = PanelManager.Instance.panelDict[PanelStack.Instance.stack.Peek().PanelType].GetComponentsInChildren<Transform>();

        foreach (Transform item in trans)
        {
            //在transform属性数组中，找到指定名称的子对象
            if (item.name == name)
            {
                return item.gameObject;
            }
        }

        Debug.Log("找不到子对象");
        return null;
    }
}