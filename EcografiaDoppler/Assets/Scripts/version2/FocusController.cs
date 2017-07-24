using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FocusController : MonoBehaviour {
    public Texture2D[] texturesRedBlue;
    public Texture2D[] texturesRedBlue_;
    public Texture2D[] texturesAngle;
    public int index = 0;
    int cont = 0;
    float contadorTiempo = 0f;
    public float contadorTiempoIncremento = 0.25f;
    public UITexture textureRedBlue;
    public UITexture textureAngle;
    public Transform xMarker;
    public CursorController cursorController;

    // Use this for initialization
    void Start () {
	}

    public void changeImages(string i)
    {
        switch (i)
        {
            case "0": index = 0; break;
            case "45": index = 1; break;
            case "90": index = 2; break;
            case "-45": index = 3; break;
            case "-90": index = 4; break;
        }
        textureRedBlue.mainTexture = texturesRedBlue[index];
        textureAngle.mainTexture = texturesAngle[index];
        cursorController.durezasTexture = (Texture2D)textureAngle.mainTexture;
        cursorController.GetPixelColor();
        xMarker.position = transform.FindChild(i).transform.position;
        //textureRedBlue.mainTexture.Apply();
        //textureAngle.mainTexture.Apply();
    }

    public void resetCont()
    {
        cont = 1;
        contadorTiempo = Time.time + contadorTiempoIncremento;
    }

    // Update is called once per frame
    void Update () {
        if (contadorTiempo < Time.time)
        {
            cont++;
            contadorTiempo = Time.time + contadorTiempoIncremento;
        }
        if (cont % 2 == 0)
        {
            textureRedBlue.mainTexture = texturesRedBlue[index];
        }
        else
        {
            textureRedBlue.mainTexture = texturesRedBlue_[index];
        }
    }
}
