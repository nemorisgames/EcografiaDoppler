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
    public Vector3 posicionInicialLinterna;
    // Use this for initialization
    void Start () {
        durezasTexture = (Texture2D)durezasUITexture.mainTexture;
        GetPixelColor();
        posicionInicialLinterna = linterna.position;
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
            if ((Mathf.Abs(cDurezas.b - cDurezas.r) <= 1f / 255f && Mathf.Abs(cDurezas.g - cDurezas.r) <= 1f / 255f && Mathf.Abs(cDurezas.g - cDurezas.b) <= 1f / 255f))
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
        /*linterna.LookAt(puntoFoco);
        linterna.Rotate(0f, -90f, 0f);
        if (Mathf.Abs(linterna.rotation.y + 0.5f) < 0.01f)
        {
            print("aqui");
            linterna.rotation = new Quaternion(linterna.rotation.x, 180f, linterna.rotation.z, linterna.rotation.w);
        }*/
        transform.LookAt(puntoFoco);
        transform.Rotate(0f, -90f, 0f);
        if (Mathf.Abs(transform.rotation.y + 0.5f) < 0.01f)
        {
            print("aqui");
            transform.rotation = new Quaternion(transform.rotation.x, 180f, transform.rotation.z, transform.rotation.w);
        }
        lineaPunteada.height = (int)(Vector3.Distance(transform.position, puntoFoco.position) * 320f);

        linterna.position = posicionInicialLinterna - 0f * transform.right;
        linterna.rotation = transform.rotation;
        linterna.Rotate(0f, 0f, -90f);
    }
}
