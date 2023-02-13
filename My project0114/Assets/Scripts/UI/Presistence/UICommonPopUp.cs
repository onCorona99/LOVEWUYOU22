using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UICommonPopUp : MonoBehaviour
{
    public class Controls
    {
        public Text TextTooltip;
    }

    public Controls m_ctl;

    public CanvasGroup canvasGroup;

    public Sequence seq;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        Serializable();
        InitListener();

        canvasGroup = GetComponent<CanvasGroup>();
    }

    void Start()
    {
        seq = DOTween.Sequence();
        Tween t1 = canvasGroup.DOFade(1f, 1f);
        Tween t2 = canvasGroup.DOFade(0f, 1f);
        // InsertCB�� ��һ�������������CB������seqʱ�����п�ʼ��ʱ�� 0f ��һ��ʼ���ž͵���
        seq.Append(t1).InsertCallback(0f, () => { Debug.Log("��ʼִ�лص�"); }).AppendInterval(2f).Append(t2);
        seq.Pause();
        seq.onComplete +=
          () =>
          {
              Debug.Log("�������SEQ");
              seq.Restart();
              seq.Pause();
          };
    }

    public void Serializable()
    {
        GameUtility.FindControls(gameObject, ref m_ctl);
    }
    public void InitListener()
    {

    }

    public void SetTextShow(string str)
    {
        if (seq.IsPlaying())
            return;

        m_ctl.TextTooltip.text = str;
        transform.SetAsLastSibling();

        Debug.Log("SetTextShow");

        seq.Play();
      
    }
}
