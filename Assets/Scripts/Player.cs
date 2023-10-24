using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HedgehogTeam.EasyTouch;
using DG.Tweening;
using PathCreation;

public class Player : MonoBehaviour
{

    [SerializeField] float xMax;
    [SerializeField] float xMin;
    public bool isMoving = false;
    public float playerspeed = 3f;
    [SerializeField] bool FlipAnim;
    bool isRot = true, isFinish = false, isdrag = true, isTurn = false, isEnding_scene = false, isRun= true, isDie;
    public Transform child;
    [SerializeField] float speed = 0.5f, playerSensitvity;//..0.03 Mobile
    public float Xvalue;
    public Animator anim;
    [Header("Path ------------------------>")]
    public PathCreator pathCreator,pathCreator2;
    public EndOfPathInstruction endOfPathInstruction;
    float distanceTravelled;
    public ParticleSystem ps,ps2,sprayPs,collectBulletPs;
    public Gradient colorGradient;
    public float endDistanceThreshold = 1.0f;
    public Transform finishPos,paintPos;
    [SerializeField] PlayerController2 playerController2;
    [SerializeField]GameObject SprayBox;
    float textureValue;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (/*gameManager.IsStart &&*/ pathCreator != null && !isFinish &&! isDie)
        {
            anim.SetBool("RUN", true);
            distanceTravelled += playerspeed * Time.deltaTime;
            transform.position = pathCreator.path.GetPointAtDistance(distanceTravelled, endOfPathInstruction);
            if (isTurn)
            {
                Quaternion newRot = pathCreator.path.GetRotationAtDistance(distanceTravelled, endOfPathInstruction);
                transform.rotation = newRot * Quaternion.Euler(0, 0, -270);

                Debug.Log("Yes turn Here");

                //child.rotation = newRot * Quaternion.Euler(0, 0, -270);
            }
            if (isdrag)
            {
                //uIManager.PbImg.fillAmount = pathCreator.path.GetPercentage(distanceTravelled);
                child.localPosition = Vector3.Lerp(child.localPosition, new Vector3(Xvalue, 0f, 0f), 0.5f);
                //child.localRotation = Quaternion.Lerp(child.localRotation, Quaternion.Euler(0f, angle, -Zangle), Time.deltaTime * 3f);
            }
            if (distanceTravelled >= pathCreator.path.length - endDistanceThreshold)
            {
                // If the player is close to the end of the path, stop the animation
                anim.SetBool("RUN", false);
                anim.SetBool("DRUN", true);
                isRun = false;
                Rigidbody rb = GetComponent<Rigidbody>();
                pathCreator = null;
                transform.DOMove(finishPos.transform.position, 2f).SetEase(Ease.Linear).OnComplete(()=> {
                    anim.SetBool("DRUN", false);
                    anim.SetBool("RUN", true);
                    transform.DOMove(new Vector3(transform.position.x, transform.position.y,paintPos.transform.position.z-2f ), 2f).SetEase(Ease.Linear).OnComplete(() => { 
                    anim.SetBool("RUN", false);
                        xMax = 1f;
                        xMin = 1f;
                        isFinish = true;
                        ps.Stop();
                        ps2.Stop();
                        SprayBox.SetActive(true);
                        playerController2.enabled = true;
                    });
                });
                // Optionally, you can also do other things when the animation stops, such as triggering an event, playing a sound, etc.
            }

        }
        if (isFinish)
        {
            if (Input.GetMouseButton(0))
            {
                sprayPs.Play();
                anim.SetBool("RUN", true);
                anim.SetTrigger("SPRAY");
            }
            if (Input.GetMouseButtonUp(0))
            {
                anim.SetBool("RUN", false);
                sprayPs.Stop();
            }
        }
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
        if (isRun)
        {
            Xvalue = child.localPosition.x + (gesture.deltaPosition.x * speed);
            Xvalue = Mathf.Clamp(Xvalue, xMin, xMax);  
        }
       
    }
    public void On_DragEnd(Gesture gesture)
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "PAINT")
        {
            collectBulletPs.gameObject.SetActive(true);
           
            if (LevelController.Instance.isRed && other.gameObject.GetComponent<Boxes>().red)
            {
                RedColorParticles();
                PaintBox.Instance.fillColor += 0.1f;
                PaintBox.Instance.fillBox.GetComponent<MeshRenderer>().material.color = Color.red;
                PaintBox.Instance.fillBox.transform.DOScale(new Vector3(1f, PaintBox.Instance.fillColor,0.6f),0.2f);
                Debug.Log("Paint Collect Red");

            }
            else if (LevelController.Instance.isBlue && other.gameObject.GetComponent<Boxes>().blue)
            {
                BlueColorParticles();
                PaintBox.Instance.fillColor += 0.1f;
                Color newColor = Color.red;
                PaintBox.Instance.fillBox.GetComponent<MeshRenderer>().material.color = Color.blue;
                PaintBox.Instance.fillBox.transform.DOScale(new Vector3(1f, PaintBox.Instance.fillColor, 0.6f), 0.2f);
                Debug.Log("Paint Collect Blue");

            }
            else if (LevelController.Instance.isPurple && other.gameObject.GetComponent<Boxes>().purple)
            {
                PurpleColorParticles();
                PaintBox.Instance.fillColor += 0.1f;
                PaintBox.Instance.fillBox.GetComponent<MeshRenderer>().material.color = Color.blue;
                PaintBox.Instance.fillBox.transform.DOScale(new Vector3(1f, PaintBox.Instance.fillColor, 0.6f), 0.2f);
                Debug.Log("Paint Collect Purple");

            }
            else if (LevelController.Instance.isPink && other.gameObject.GetComponent<Boxes>().pink)
            {
                PinkColorParticles();
                PaintBox.Instance.fillColor += 0.1f;
                PaintBox.Instance.fillBox.GetComponent<MeshRenderer>().material.color = Color.magenta;
                PaintBox.Instance.fillBox.transform.DOScale(new Vector3(1f, PaintBox.Instance.fillColor, 0.6f), 0.2f);
                Debug.Log("Paint Collect Pink");

            }
            else if(LevelController.Instance.isGreen && other.gameObject.GetComponent<Boxes>().green)
            {
                GreenColorParticles();
                PaintBox.Instance.fillColor += 0.1f;
                PaintBox.Instance.fillBox.GetComponent<MeshRenderer>().material.color = Color.green;
                Debug.Log("Paint Collect Green");
                PaintBox.Instance.fillBox.transform.DOScale(new Vector3(1f, PaintBox.Instance.fillColor, 0.6f), 0.2f);
            }
            else
            {
                if(PaintBox.Instance.fillColor > 0)
                {
                    PaintBox.Instance.fillColor -= 0.1f;
                    collectBulletPs.Play();
                    PaintBox.Instance.fillBox.transform.DOScale(new Vector3(1f, PaintBox.Instance.fillColor, 0.6f), 0.2f);
                }
                else
                {
                    Debug.Log("Game Over");
                }
               
            }
            Destroy(other.gameObject);

        }
        if (other.gameObject.tag == "Die")
        {
            pathCreator = null;
            isDie = true;
            Debug.Log("Dieeeeeeeeee");
            //isFinish = true;
            isRun = false;
            anim.SetBool("FELL", true);
            anim.SetBool("RUN", false);
        }
        if (other.gameObject.tag == "OBS")
        {
            isRun = false;
            anim.SetBool("RUN", false);
            pathCreator = null;
            playerspeed = 0f;
            //anim.SetTrigger("HIT");
            anim.SetBool("FELL", true);
            anim.DOComplete(false);
            other.gameObject.tag = "Untagged";

        }
        if (other.gameObject.tag == "DIAMOND")
        {
            other.gameObject.transform.GetChild(1).gameObject.SetActive(true);
            Destroy(other.gameObject,2f);
        }
    }
    void BlueColorParticles()
    {
        ps2.Play();
        ps.Play();
        Gradient colorGradient = new Gradient();
        GradientColorKey[] colorKeys = new GradientColorKey[2];
        GradientAlphaKey[] alphaKeys = new GradientAlphaKey[2];
        // Define the color keys.
        colorKeys[0].color = Color.blue;
        colorKeys[0].time = 0f;
        colorKeys[1].color = Color.blue;
        colorKeys[1].time = 1f;

        // Define the alpha keys.
        alphaKeys[0].alpha = 1.0f;
        alphaKeys[0].time = 0f;
        alphaKeys[1].alpha = 1.0f;
        alphaKeys[1].time = 1f;

        colorGradient.SetKeys(colorKeys, alphaKeys);

        // Get the ColorOverLifetime module.
        var colorOverLifetime = ps.colorOverLifetime;
        var colorOverLifetime2 = ps2.colorOverLifetime;

        // Set the color gradient over the lifetime of particles.
        colorOverLifetime.color = colorGradient;
        colorOverLifetime2.color = colorGradient;
    }
    void RedColorParticles()
    {
        ps2.Play();
        ps.Play();
        Gradient colorGradient = new Gradient();
        GradientColorKey[] colorKeys = new GradientColorKey[2];
        GradientAlphaKey[] alphaKeys = new GradientAlphaKey[2];
        // Define the color keys.
        colorKeys[0].color = Color.red;
        colorKeys[0].time = 0f;
        colorKeys[1].color = Color.red;
        colorKeys[1].time = 1f;

        // Define the alpha keys.
        alphaKeys[0].alpha = 1.0f;
        alphaKeys[0].time = 0f;
        alphaKeys[1].alpha = 0.0f;
        alphaKeys[1].time = 1f;

        colorGradient.SetKeys(colorKeys, alphaKeys);

        // Get the ColorOverLifetime module.
        var colorOverLifetime = ps.colorOverLifetime;
        var colorOverLifetime2 = ps2.colorOverLifetime;

        // Set the color gradient over the lifetime of particles.
        colorOverLifetime.color = colorGradient;
        colorOverLifetime2.color = colorGradient;
    }
    void PurpleColorParticles()
    {
        ps2.Play();
        ps.Play();
        Gradient colorGradient = new Gradient();
        GradientColorKey[] colorKeys = new GradientColorKey[2];
        GradientAlphaKey[] alphaKeys = new GradientAlphaKey[2];
        // Define the color keys.
        colorKeys[0].color = Color.blue;
        colorKeys[0].time = 0f;
        colorKeys[1].color = Color.blue;
        colorKeys[1].time = 1f;

        // Define the alpha keys.
        alphaKeys[0].alpha = 1.0f;
        alphaKeys[0].time = 0f;
        alphaKeys[1].alpha = 0.0f;
        alphaKeys[1].time = 1f;

        colorGradient.SetKeys(colorKeys, alphaKeys);

        // Get the ColorOverLifetime module.
        var colorOverLifetime = ps.colorOverLifetime;
        var colorOverLifetime2 = ps2.colorOverLifetime;

        // Set the color gradient over the lifetime of particles.
        colorOverLifetime.color = colorGradient;
        colorOverLifetime2.color = colorGradient;
    }
    void PinkColorParticles()
    {
        ps2.Play();
        ps.Play();
        Gradient colorGradient = new Gradient();
        GradientColorKey[] colorKeys = new GradientColorKey[2];
        GradientAlphaKey[] alphaKeys = new GradientAlphaKey[2];
        // Define the color keys.
        colorKeys[0].color = Color.cyan;
        colorKeys[0].time = 0f;
        colorKeys[1].color = Color.magenta;
        colorKeys[1].time = 1f;

        // Define the alpha keys.
        alphaKeys[0].alpha = 1.0f;
        alphaKeys[0].time = 0f;
        alphaKeys[1].alpha = 0.0f;
        alphaKeys[1].time = 1f;

        colorGradient.SetKeys(colorKeys, alphaKeys);

        // Get the ColorOverLifetime module.
        var colorOverLifetime = ps.colorOverLifetime;
        var colorOverLifetime2 = ps2.colorOverLifetime;

        // Set the color gradient over the lifetime of particles.
        colorOverLifetime.color = colorGradient;
        colorOverLifetime2.color = colorGradient;
    }
    void GreenColorParticles()
    {
        ps2.Play();
        ps.Play();
        Gradient colorGradient = new Gradient();
        GradientColorKey[] colorKeys = new GradientColorKey[2];
        GradientAlphaKey[] alphaKeys = new GradientAlphaKey[2];
        // Define the color keys.
        colorKeys[0].color = Color.green;
        colorKeys[0].time = 0f;
        colorKeys[1].color = Color.green;
        colorKeys[1].time = 1f;

        // Define the alpha keys.
        alphaKeys[0].alpha = 1.0f;
        alphaKeys[0].time = 0f;
        alphaKeys[1].alpha = 0.0f;
        alphaKeys[1].time = 1f;

        colorGradient.SetKeys(colorKeys, alphaKeys);

        // Get the ColorOverLifetime module.
        var colorOverLifetime = ps.colorOverLifetime;
        var colorOverLifetime2 = ps2.colorOverLifetime;

        // Set the color gradient over the lifetime of particles.
        colorOverLifetime.color = colorGradient;
        colorOverLifetime2.color = colorGradient;
    }
}
