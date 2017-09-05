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

        if ((cDurezas.r > 0.5f && cDurezas.b < 0.5f) || (cDurezas.b > 0.5f && cDurezas.r < 0.5f) && !(cDurezas.b == cDurezas.r && cDurezas.g == cDurezas.r))
        {
            //Venas
            durezasAngle = 0f;
            print("en vena");
            inVein = true;
            positive = (cDurezas.r > 0.5f);
        }
        else
        {
            if (((int)(Mathf.Abs(cDurezas.b - cDurezas.r) * 100) <= (int)(100f / 255f) && (int)(Mathf.Abs(cDurezas.g - cDurezas.r) * 100) <= (int)(100f / 255f) && (int)(Mathf.Abs(cDurezas.g - cDurezas.b) * 100) <= (int)(100f / 255f)))
            {
                umbilicalDraw = true;
                durezasAngle = cDurezas.g;
                print("en dibujo umbilical");
            }
            else
            {
                durezasAngle = cDurezas.g;
                umbilicalDraw = false;
                inVein = false;
                print(durezasAngle);
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
        if (Mathf.Abs(transform.rotation.y + 0.5f) < 0.1f)
        {
            //print("aqui");
            //transform.rotation = new Quaternion(transform.rotation.x, -180f, transform.rotation.z, transform.rotation.w);
        }
        //if(transform.rotation.y > 90f)
        //    transform.rotation = new Quaternion(transform.rotation.x, 180f, transform.rotation.z, transform.rotation.w);
        print(transform.eulerAngles);
        if ((transform.eulerAngles.y >= -180f && transform.eulerAngles.y <= -0f) || (transform.eulerAngles.y <= 360f && transform.eulerAngles.y > 200f))
        //if(transform.eulerAngles.y == 270f || transform.eulerAngles.y == -90f)
        {
            print("aqui");
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, 0f, transform.eulerAngles.z);
        }
        else
        {
            print("aqui2");
            //transform.eulerAngles = new Vector3(transform.eulerAngles.x, 180f, transform.eulerAngles.z);
        }

        lineaPunteada.height = (int)(Vector3.Distance(transform.position, puntoFoco.position) * 320f);
        posicionInicialLinterna = transductor.position;// + transform.parent.up * 0.35f - transform.parent.right * 0.02f;
        linterna.position = posicionInicialLinterna;
        linterna.rotation = transform.rotation;
        linterna.Rotate(0f, 0f, -90f);
        //Vector3 angulo = new Vector3(0f, 0f, linterna.rotation.eulerAngles.z);
        //linterna.rotation = new Quaternion(0f, (linterna.rotation.y <= 90f || linterna.rotation.y >= -90f) ? 0f: 180f, linterna.rotation.z, linterna.rotation.w);
        transductor.rotation = linterna.rotation;
    }
}
