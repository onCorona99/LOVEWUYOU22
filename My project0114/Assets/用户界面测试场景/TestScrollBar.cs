using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestScrollBar : MonoBehaviour
{
    public Scrollbar sb;

    public void Awake()
    {
        sb = GetComponent<Scrollbar>();
    }
    // Start is called before the first frame update
    void Start()
    {
        sb.size = 0.2f;

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
