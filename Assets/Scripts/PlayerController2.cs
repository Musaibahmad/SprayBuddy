using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HedgehogTeam.EasyTouch;


public class PlayerController2 : MonoBehaviour
{
    [SerializeField] float xMax;
    [SerializeField] float xMin;
    [SerializeField] Transform child;
    [SerializeField] float speed = 0.5f, playerSensitvity;//..0.03 Mobile
    public float Xvalue;
    [SerializeField] Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    void OnEnable()
    {
        EasyTouch.On_TouchDown += On_Drag;
        EasyTouch.On_TouchUp += On_DragEnd;
    }
    void OnDisable()
    {
        EasyTouch.On_TouchDown -= On_Drag;
        EasyTouch.On_TouchUp -= On_DragEnd;
    }
    void OnDestroy()
    {
        EasyTouch.On_TouchDown -= On_Drag;
        EasyTouch.On_TouchUp -= On_DragEnd;
    }
    public void On_Drag(Gesture gesture)
    {
            Xvalue = child.localPosition.x + (gesture.deltaPosition.x * speed);
            Xvalue = Mathf.Clamp(Xvalue, xMin, xMax);
        
    }
    public void On_DragEnd(Gesture gesture)
    {

    }
    // Update is called once per frame
    void Update()
    {
        child.localPosition = Vector3.Lerp(child.localPosition, new Vector3(Xvalue, 0f, 0f), 0.5f);
        if (Input.GetMouseButton(0))
        {
            if (Xvalue > 0)
            {

                anim.SetBool("RIGHT", true);
                anim.SetBool("LEFT", false);
            }
            else
            {
                anim.SetBool("RIGHT", false);
                anim.SetBool("LEFT", true);
            }
        }
       
        if (Input.GetMouseButtonUp(0))
        {
            anim.SetBool("RIGHT", false);
            anim.SetBool("LEFT", false);
        }    
    }
    
}
