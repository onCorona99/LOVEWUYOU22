using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// 技能按钮 按下抬起的color暗示
/// </summary>
public class SkillBtnPressHint : MonoBehaviour,IPointerDownHandler,IPointerUpHandler
{
    private Image image_bgCircle;
    private Image image_icon;

    private void Awake()
    {
        image_bgCircle = GetComponent<Image>();
        image_icon = GameUtility.FindChild(this.gameObject, "Icon").GetComponent<Image>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        TurnPressCol();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        TurnNormalCol();
    }

    public void TurnPressCol()
    {
        image_bgCircle.color = Color.red;
        image_icon.color = new Color(1, 1, 1, 0.5f);
    }

    public void TurnNormalCol()
    {
        image_bgCircle.color = Color.white;
        image_icon.color = new Color(1, 1, 1, 1f);
    }
}
