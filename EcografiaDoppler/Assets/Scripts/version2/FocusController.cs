using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FocusController : MonoBehaviour {
    public Texture2D[] texturesRedBlue;
    public Texture2D[] texturesRedBlue_;
    public Texture2D[] texturesRedBlueInv;
    public Texture2D[] texturesRedBlueInv_;
    public Texture2D[] texturesAngle;
    public Texture2D[] texturesDireccion;
    public Texture2D[] texturesDireccionInv;
    public int index = 0;
    int cont = 0;
    public float contadorTiempo = 0f;
    public float contadorTiempoIncremento = 0.25f;
    public UITexture textureRedBlue;
    public UITexture textureAngle;
    public UITexture textureDireccion;
    public Transform xMarker;
    public CursorController cursorController;
    public bool invertido = false;
    public GraphController graphController;
    public GameObject[] buttonsAngle;
    // Use this for initialization
    void Start () {
        buttonsAngle[0].SetActive(false);
	}

    //public void changeImages()
    //{
        /*string angle = "0";
        switch (index)
        {
            case 0: angle = "0"; break;
            case 1: angle = "45"; break;
            case 2: angle = "90"; break;
            case 3: angle = "-45"; break;
            case 4: angle = "-90"; break;
        }
        changeImages(angle);*/
    //}

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
        for(int j = 0; j < 5; j++)
            buttonsAngle[j].SetActive(j != index);
        if (invertido)
        {
            textureRedBlue.mainTexture = texturesRedBlueInv[index];
            textureAngle.mainTexture = texturesAngle[index];
            cursorController.durezasTexture = (Texture2D)textureAngle.mainTexture;
            textureDireccion.mainTexture = texturesDireccionInv[index];
            cursorController.GetPixelColor();
            xMarker.position = transform.Find(i).transform.position;
        }
        else
        {
            textureRedBlue.mainTexture = texturesRedBlue[index];
            textureAngle.mainTexture = texturesAngle[index];
            cursorController.durezasTexture = (Texture2D)textureAngle.mainTexture;
            textureDireccion.mainTexture = texturesDireccion[index];
            cursorController.GetPixelColor();
            xMarker.position = transform.Find(i).transform.position;
        }
        
        //textureRedBlue.mainTexture.Apply();
        //textureAngle.mainTexture.Apply();
    }

    public void changeImagesNoAngle()
    {
        for (int j = 0; j < 5; j++)
            buttonsAngle[j].SetActive(j != index);
        if (invertido)
        {
            textureRedBlue.mainTexture = texturesRedBlueInv[index];
            textureAngle.mainTexture = texturesAngle[index];
            cursorController.durezasTexture = (Texture2D)textureAngle.mainTexture;
            textureDireccion.mainTexture = texturesDireccionInv[index];
            cursorController.GetPixelColor();
            //xMarker.position = transform.FindChild(i).transform.position;
        }
        else
        {
            textureRedBlue.mainTexture = texturesRedBlue[index];
            textureAngle.mainTexture = texturesAngle[index];
            cursorController.durezasTexture = (Texture2D)textureAngle.mainTexture;
            textureDireccion.mainTexture = texturesDireccion[index];
            cursorController.GetPixelColor();
            //xMarker.position = transform.FindChild(i).transform.position;
        }

    }

    public void resetCont()
    {
        cont = 1;
        contadorTiempo = Time.time + contadorTiempoIncremento;
    }

    // Update is called once per frame
    void Update () {
        if (graphController.sleep) return;
        if (contadorTiempo < Time.time)
        {
            cont++;
            contadorTiempo = Time.time + contadorTiempoIncremento;
        }
        if (cont % 2 == 0)
        {
            if (invertido)
            {
                textureRedBlue.mainTexture = texturesRedBlueInv[index];
            }
            else
            {
                textureRedBlue.mainTexture = texturesRedBlue[index];
            }
        }
        else
        {
            if (invertido)
            {
                textureRedBlue.mainTexture = texturesRedBlueInv_[index];
            }
            else
            {
                textureRedBlue.mainTexture = texturesRedBlue_[index];
            }
        }
    }
}
