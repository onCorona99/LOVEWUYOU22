using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestSD01 : MonoBehaviour
{
    public GameObject target;

    public Shader sd1, sd2;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShiftSD()
    {
        if (!sd1)
        {
            Debug.Log("SD1Îª¿Õ");
            return;
        }
        if (!sd2)
        {
            Debug.Log("SD2Îª¿Õ");
            return;
        }
        Debug.Log("ÇÐ»»SD!!!");

        MeshRenderer renderer = target.GetComponent<MeshRenderer>();
        if (renderer.sharedMaterial.shader == sd1)
        {
            renderer.sharedMaterial.shader = sd2;
        }
        else
        {
            renderer.sharedMaterial.shader = sd1;
        }
    }
}
