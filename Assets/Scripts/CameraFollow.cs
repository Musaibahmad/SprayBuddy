using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public  Transform player;
    public Vector3 offset;
    [Range(0.01f,1f)]
    public float smoothSpeed;
    [Range(0.01f, 1f)]
    public float smooth_rotation;
    public bool LootAt = true;
    //public GameObject wind_Particles;
    Vector3 startpos, childstartpos,childRotaion;

    private void OnEnable()
    {
        if (player != null)
        {
        offset = transform.position - player.position;

        }
    }
    private void Awake()
    {
        startpos = transform.position;
        childstartpos = transform.GetChild(0).localPosition;
        childRotaion = transform.GetChild(0).localEulerAngles;


    }
    void LateUpdate()
    {
        if (player)
        {
            Vector3 newpos = player.position + offset;
            transform.position = Vector3.Slerp(transform.position, newpos, smoothSpeed);
            if (LootAt)
            {
            transform.rotation = Quaternion.Slerp(transform.rotation, player.rotation, smooth_rotation);
            transform.rotation = Quaternion.Euler(new Vector3(0, transform.rotation.eulerAngles.y, 0f));
            }
        }
        else
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;
            if (player != null)
            {
                offset = transform.position - player.position;

            }
        }
    }
    public void Reset_Pos()
    {
        LootAt = true;
        Camera.main.fieldOfView = 40f;
        transform.position= startpos;
        transform.GetChild(0).localPosition=childstartpos;
        transform.GetChild(0).localEulerAngles= childRotaion;
    }
}
