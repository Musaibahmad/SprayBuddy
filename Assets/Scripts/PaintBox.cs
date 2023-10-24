using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaintBox : MonoBehaviour
{
    public static PaintBox Instance;
    public float fillColor;
    public GameObject fillBox;
    private void Awake()
    {
        if(Instance is null)
        {
            Instance = this;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        
    }
}
