using UnityEngine;
using System;
public class HeGizmosCircle : MonoBehaviour
{
    public Transform m_Transform;
    public float m_Radius = 1; // Բ���İ뾶
    public float m_Theta = 0.1f; // ֵԽ��Բ��Խƽ��
    public Color m_Color = Color.green; // �߿���ɫ

    void Start()
    {
        if (m_Transform == null)
        {
            throw new Exception("Transform is NULL.");
        }
    }
    void OnDrawGizmos()
    {
        if (m_Transform == null) return;
        if (m_Theta < 0.0001f) m_Theta = 0.0001f;
        // ���þ���
        Matrix4x4 defaultMatrix = Gizmos.matrix;
        Gizmos.matrix = m_Transform.localToWorldMatrix;
        // ������ɫ
        Color defaultColor = Gizmos.color;
        Gizmos.color = m_Color;
        // ����Բ��
        Vector3 beginPoint = Vector3.zero;
        Vector3 firstPoint = Vector3.zero;
        for (float theta = 0; theta < 2 * Mathf.PI; theta += m_Theta)
        {
            float x = m_Radius * Mathf.Cos(theta);
            float z = m_Radius * Mathf.Sin(theta);
            Vector3 endPoint = new Vector3(x, 0, z);
            if (theta == 0)
            {
                firstPoint = endPoint;
            }
            else
            {
                Gizmos.DrawLine(beginPoint, endPoint);
            }
            beginPoint = endPoint;
        }
        // �������һ���߶�
        Gizmos.DrawLine(firstPoint, beginPoint);
        // �ָ�Ĭ����ɫ
        Gizmos.color = defaultColor;
        // �ָ�Ĭ�Ͼ���
        Gizmos.matrix = defaultMatrix;
    }
}
