using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorController : MonoBehaviour {
    Texture2D hitTexture;
    public GraphController graphController;
    public UITexture durezasUITexture;
    public Texture2D durezasTexture;
    public float durezasAngle = 0f;
    public bool inVein = false;
    public bool positive = false;
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
        if (Physics.Raycast(transform.position, Vector3.forward, out hit))
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

        if ((cDurezas.r > 0.5f && cDurezas.b < 0.5f) || (cDurezas.b > 0.5f && cDurezas.r < 0.5f))
        {
            //Venas
            durezasAngle = 0f;
            print("en vena");
            inVein = true;
            positive = (cDurezas.r > 0.5f);
        }
        else
        {
            durezasAngle = cDurezas.g;
            inVein = false;
            print(durezasAngle);
        }
    }
	// Update is called once per frame
	void Update () {
        

    }
}
