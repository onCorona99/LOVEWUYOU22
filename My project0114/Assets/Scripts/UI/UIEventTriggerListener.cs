using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// 这个EventTrigger 继承自Unity的EventTrigger 会使得如果只给gameObject注册了点击的话
/// 这个go是不可以被拖拽的
/// </summary>
public class UIEventTriggerListener : EventTrigger
{
    public delegate void VoidDelegate(GameObject go);

    public delegate void PointerDelegate(PointerEventData eventData);

    public delegate void DragDelegate(PointerEventData eventData);

    public VoidDelegate OnClick;

    public PointerDelegate PointerClick;

    public DragDelegate OnDragEvent;

    public DragDelegate OnEndDragEvent;

    public DragDelegate OnBeginDragEvent;

    public VoidDelegate OnDown;

    public PointerDelegate PointerDown;

    public PointerDelegate PointerUp;

    public VoidDelegate OnEnter;

    public PointerDelegate PointerEnter;

    public VoidDelegate OnExit;

    public PointerDelegate PointerExit;

    public VoidDelegate OnUp;

    public VoidDelegate OnSelectd;

    public VoidDelegate OnUpdateSelect;

    public static UIEventTriggerListener Get(GameObject go)
    {
        return GameUtility.GetOrAddComponent<UIEventTriggerListener>(go);
    }

    public override void OnPointerClick(PointerEventData eventData)
    {
        if (OnClick != null)
        {
            OnClick(base.gameObject);
        }

        if (PointerClick != null)
        {
            PointerClick(eventData);
        }
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        if (OnDown != null)
        {
            OnDown(base.gameObject);
        }

        if (PointerDown != null)
        {
            PointerDown(eventData);
        }
    }

    public override void OnPointerEnter(PointerEventData eventData)
    {
        if (OnEnter != null)
        {
            OnEnter(base.gameObject);
        }

        if (PointerEnter != null)
        {
            PointerEnter(eventData);
        }
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        if (OnExit != null)
        {
            OnExit(base.gameObject);
        }

        if (PointerExit != null)
        {
            PointerExit(eventData);
        }
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        if (OnUp != null)
        {
            OnUp(base.gameObject);
        }

        if (PointerUp != null)
        {
            PointerUp(eventData);
        }
    }

    public override void OnSelect(BaseEventData eventData)
    {
        if (OnSelectd != null)
        {
            OnSelectd(base.gameObject);
        }
    }

    public override void OnUpdateSelected(BaseEventData eventData)
    {
        if (OnUpdateSelect != null)
        {
            OnUpdateSelect(base.gameObject);
        }
    }

    public override void OnDrag(PointerEventData data)
    {
        if (OnDragEvent != null)
        {
            OnDragEvent(data);
        }
    }

    public override void OnEndDrag(PointerEventData data)
    {
        if (OnEndDragEvent != null)
        {
            OnEndDragEvent(data);
        }
    }

    public override void OnBeginDrag(PointerEventData data)
    {
        if (OnBeginDragEvent != null)
        {
            OnBeginDragEvent(data);
        }
    }
}

