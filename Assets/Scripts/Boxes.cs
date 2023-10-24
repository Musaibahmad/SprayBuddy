using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boxes : MonoBehaviour
{
    public bool green, blue, purple, pink, red;
    public static Boxes Instace;
    private void Awake()
    {
        if (Instace is null)
            Instace = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("colllect");
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
