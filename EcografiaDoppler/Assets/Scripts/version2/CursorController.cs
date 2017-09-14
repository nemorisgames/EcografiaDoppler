using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorController : MonoBehaviour {
    Texture2D hitTexture;
    public GraphController graphController;
    public UITexture durezasUITexture;
    public Texture2D durezasTexture;
    public Transform puntoFoco;
    public UISprite lineaPunteada;
    public float durezasAngle = 0f;
    public bool inVein = false;
    public bool positive = false;
    public bool umbilicalDraw = false;
    public Transform linterna;
    public Transform transductor;
    public Vector3 posicionInicialLinterna;
    // Use this for initialization
    void Start () {
        durezasTexture = (Texture2D)durezasUITexture.mainTexture;
        GetPixelColor();
        
    }

    void OnDragEnd()
    {
        GetPixelColor();
    }

    public void GetPixelColor()
    {
        RaycastHit hit;
        int layerMask = 1 << 8;
        if (Physics.Raycast(transform.position, Vector3.forward, out hit, 100f, ~layerMask))
        {
            hitTexture = hit.transform.gameObject.GetComponentInChildren<UITexture>().mainTexture as Texture2D;
        }
        else return;
        Vector2 hitPoint = hit.textureCoord;
        hitPoint.x *= hitTexture.width;
        hitPoint.y *= hitTexture.height;
        print(((int)hitPoint.x + " , " + (int)hitPoint.y));
        Color c = hitTexture.GetPixel((int)hitPoint.x, (int)hitPoint.y);
        Color cDurezas = durezasTexture.GetPixel((int)hitPoint.x, (int)hitPoint.y);
        //Debug.Log("RGB: " + c.r + "," + c.g + "," + c.b);
        //rangos de color para detectar rojo, azul o n/a
        if (c.r > 0.6f && c.b < 0.5f)
        {
            //Debug.Log("R");
            graphController.functionInverseNormal();
        }
        else if (c.b > 0.6f && c.r < 0.5f)
        {
            //Debug.Log("B");
            graphController.functionInverse();
        }
        else
        {
            //Debug.Log("N/A");
        }
        print(c);
        print(cDurezas);
        if ((cDurezas.r > 0.5f && cDurezas.b < 0.5f) || (cDurezas.b > 0.5f && cDurezas.r < 0.5f) && !(cDurezas.b == cDurezas.r && cDurezas.g == cDurezas.r))
        {
            //Venas
            durezasAngle = 0f;
            print("en vena");
            inVein = true;
            umbilicalDraw = false;
            positive = (cDurezas.r > 0.5f);
        }
        else
        {
            if (cDurezas != Color.black && ((int)(Mathf.Abs(cDurezas.b - cDurezas.r) * 100) <= (int)(100f / 255f) && (int)(Mathf.Abs(cDurezas.g - cDurezas.r) * 100) <= (int)(100f / 255f) && (int)(Mathf.Abs(cDurezas.g - cDurezas.b) * 100) <= (int)(100f / 255f)))
            {
                inVein = false;
                umbilicalDraw = true;
                durezasAngle = 1f;// cDurezas.g;
                print("en dibujo umbilical");
                positive = (cDurezas.r > 0.5f);
            }
            else
            {
                if (!(cDurezas.b == cDurezas.r && cDurezas.g == cDurezas.r))
                {

                    print("no en dibujo umbilical");
                    durezasAngle = 1f;// cDurezas.g;
                    umbilicalDraw = false;
                    inVein = false;
                    print(durezasAngle);
                    positive = (cDurezas.r > 0.5f);
                }
                else
                {

                    durezasAngle = 0f;// cDurezas.g;
                }
            }
        }
    }
    // Update is called once per frame
    void LateUpdate()
    {
        transform.LookAt(puntoFoco);
        transform.Rotate(0f, -90f, 0f);
        /*
        //find the vector pointing from our position to the target
        Vector3 _direction = (puntoFoco.position - transform.position).normalized;

        //create the rotation we need to be in to look at the target
        Quaternion _lookRotation = Quaternion.LookRotation(_direction);

        transform.rotation = _lookRotation;
        transform.Rotate(0f, -90f, 0f);
        */
        //if (Mathf.Abs(transform.rotation.y + 0.5f) < 0.1f)
        //{
            //print("aqui");
            //transform.rotation = new Quaternion(transform.rotation.x, -180f, transform.rotation.z, transform.rotation.w);
        //}
        //if(transform.rotation.y > 90f)
        //    transform.rotation = new Quaternion(transform.rotation.x, 180f, transform.rotation.z, transform.rotation.w);
        //print(transform.eulerAngles);
        /*if(transform.eulerAngles.y < 360f && transform.eulerAngles.y > 180f)
        {
            if(transform.eulerAngles.y >= 270f)
                transform.eulerAngles = new Vector3(transform.eulerAngles.x, 0f, transform.eulerAngles.z * 90f / 81f);
            if (transform.eulerAngles.y < 270f && transform.eulerAngles.y > 180f)
                transform.eulerAngles = new Vector3(transform.eulerAngles.x, 180f, transform.eulerAngles.z * 90f / 81f);
            if (transform.eulerAngles.z < 90f && transform.eulerAngles.z > 0f)
                transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z);
            else
                transform.eulerAngles = new Vector3(transform.eulerAngles.x, 0f, 90f);
        }*/
        /* if ((transform.eulerAngles.y >= 0f && transform.eulerAngles.y <= 90f) || (transform.eulerAngles.y >= 270f && transform.eulerAngles.y <= 360f))
         {
             //print("aqui1");
             transform.eulerAngles = new Vector3(transform.eulerAngles.x, 0f, transform.eulerAngles.z);
         }
         else
         {
             //print("aqui2");
             transform.eulerAngles = new Vector3(transform.eulerAngles.x, 180f, transform.eulerAngles.z);
         }*/
        //print(transform.eulerAngles);
        //if ((transform.eulerAngles.y > -180f && transform.eulerAngles.y <= -0f) || (transform.eulerAngles.y <= 360f && transform.eulerAngles.y > 180f))
        /*if((transform.eulerAngles.y >= 270f && transform.eulerAngles.y <= 90f) || (transform.eulerAngles.y >= -90f && transform.eulerAngles.y <= -270f))
        {
            if((transform.eulerAngles.y >= 175f && transform.eulerAngles.y <= -175f) || (transform.eulerAngles.y >= -5f && transform.eulerAngles.y <= 5f))
            {
                print("aqui");
                transform.eulerAngles = new Vector3(transform.eulerAngles.x, 0, transform.eulerAngles.z);
            }
            else
            {

                print("aqui1");
                transform.eulerAngles = new Vector3(transform.eulerAngles.x, 0f, 180f - transform.eulerAngles.z);
            }
        }
        else
        {

            if ((transform.eulerAngles.y >= 130f && transform.eulerAngles.y <= 180f) || (transform.eulerAngles.y <= -130f && transform.eulerAngles.y >= -180f))
            {
                print("aqui2");
                transform.eulerAngles = new Vector3(transform.eulerAngles.x, 0, transform.eulerAngles.z);
            }
            else
            {*/
                if (transform.position.x >= transductor.position.x)
                {
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, 0f, 180f - transform.eulerAngles.z);// * ((transform.eulerAngles.z > 85 && transform.eulerAngles.z < 95f)?0.9f:1f));
                    //print("aqui3");
                }
                else
                {
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, 0f, transform.eulerAngles.z);// * ((transform.eulerAngles.z > 85 && transform.eulerAngles.z < 95f) ? 0.9f : 1f));
                    //print("aqui4");
                }
        //transform.eulerAngles = new Vector3(transform.eulerAngles.x, 0f, -transform.eulerAngles.z);
        //if(transform.eulerAngles.y != 0f || transform.eulerAngles.y != 180f)
        //    transform.eulerAngles = new Vector3(transform.eulerAngles.x, 180f, transform.eulerAngles.z);
        //}

        lineaPunteada.height = (int)(Vector3.Distance(transform.position, puntoFoco.position) * 320f);
        posicionInicialLinterna = transductor.position;
        linterna.position = posicionInicialLinterna;
        linterna.rotation = transform.rotation;
        linterna.Rotate(0f, 0f, -90f);
        transductor.rotation = linterna.rotation;
        //print(transform.eulerAngles);
        //transform.Rotate(0f, 0f, 180f);
        /*
        print(transform.eulerAngles);
        //transform.Rotate(0f, 0f, 90f);
        if (transform.eulerAngles.y >= 269f && transform.eulerAngles.y <= 271f)
        {
            transform.eulerAngles = new Vector3(0f, 0f, transform.eulerAngles.x);
            print("aqui");
        }
        else
        {
            //if (transform.eulerAngles.z >= 0f && transform.eulerAngles.z <= 90f)
            transform.eulerAngles = new Vector3(0f, 0f, 180f - transform.eulerAngles.x);
            print("aqui2");
        }*/
    }
}
