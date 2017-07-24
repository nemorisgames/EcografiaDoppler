﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GraphController : MonoBehaviour {
    int sizeHorizontal = 384;
    int sizeVertical = 256;
    Texture2D texture;
    int zero = 0;
    int indexScan = 0;
    int incrIndexScan = 1;
    float verticalScale = 1f;
    int gain = 0;
    int power = 0;
    float heartRate = 1;
    bool inverseFunction = false;
    bool sleep = false;
    int pathology = 0;
    public UISlider sliderSpeed;
    public UISlider sliderScale;
    public UISlider sliderZero;
    public UISlider sliderGain;
    public UISlider sliderPower;
    public UISlider sliderheart;
    public UISlider sliderPathology;
    public UILabel beatsPerMinute;
    public GameObject horizontalNumberPrefab;
    ArrayList horizontalNumbers = new ArrayList();
    public CursorController cursorController;
    public FocusController focusController;
    // Use this for initialization
    void Start () {
        texture = new Texture2D(sizeHorizontal, sizeVertical);
        GetComponent<Renderer>().material.mainTexture = texture;
        transform.localScale = new Vector3(sizeHorizontal, transform.localScale.y, transform.localScale.z);
        zero = sizeVertical / 2;
        PaintItBlack();
    }

    public void PaintItBlack()
    {
        ///pinta el quad completamente de negro
        Color ini = new Color(0, 0, 0, 0);
        Color[] colArray = texture.GetPixels();
        for (int i = 0; i < colArray.Length; i++)
        {
            colArray[i] = ini;
        }
        texture.SetPixels(colArray);
        //pinta linea blanca horizontal en el centro del quad
        for (int i = 0; i < sizeHorizontal; i++)
        {
            texture.SetPixel(i, zero, Color.white);
        }
        texture.Apply();
    }

    void paintLineVerticalBlack(int indexHorizontal)
    {
        for(int i = 0; i < sizeVertical; i++)
        {
            for (int j = 0; j < incrIndexScan; j++)
                texture.SetPixel(indexHorizontal + j, i, Color.black);
        }
        //pinta linea blanca del zero
        for(int i = 0; i < incrIndexScan; i++)
            texture.SetPixel(indexHorizontal + i, zero, Color.white);
        texture.Apply();
    }

    public void functionInverseNormal()
    {
        inverseFunction = false;
    }

    public void functionInverse()
    {
        inverseFunction = true;
    }

    public void makeSleep()
    {
        sleep = !sleep;
    }

    void drawFunction()
    {
        float currentValue = 0f;
        bool positiveFunction;
        for (int i = 0; i < 10 + gain + power; i++)
        {
            paintLineVerticalBlack(indexScan % sizeHorizontal + incrIndexScan);
            if(indexScan % sizeHorizontal == 0)
            {
                focusController.resetCont();
            }
            //factor independiente para cada funcion que hace que calce con la medicion de latidos por segundos
            float horizontalFactor = 4.5f;//1.6f;//
            if (SceneManager.GetActiveScene().name == "EcografiaUmbilical")
            {
                //Funcion!!
                currentValue = Mathf.Cos((indexScan * heartRate / 4 % 65) * horizontalFactor * Mathf.PI / 180f - 19f) * 20f + 40;
                
            }
            if (SceneManager.GetActiveScene().name == "EcografiaDuctus")
            {
                horizontalFactor = 1.0f;
                //Funcion!!
                //currentValue = (Mathf.Cos((indexScan * heartRate / 4 % 60) * horizontalFactor * Mathf.PI / 180f * 8) * 17f) + 50f;// + (Mathf.Sin((indexScan % 30) * Mathf.PI / 180f * 3f) * 60f) - (Mathf.Sin((indexScan % 60) * Mathf.PI / 180f * 3f) * 30f);
                currentValue = (Mathf.Cos((indexScan * heartRate / 4 % 65) * horizontalFactor * Mathf.PI / 180f * 8) * ((indexScan * heartRate / 4 % 65 < 30)?15f:30f)) + 60f;// + (Mathf.Sin((indexScan % 30) * Mathf.PI / 180f * 3f) * 60f) - (Mathf.Sin((indexScan % 60) * Mathf.PI / 180f * 3f) * 30f);
            }
            if (SceneManager.GetActiveScene().name == "EcografiaCerebral")
            {
                //Funcion!!
                currentValue = Mathf.Cos((indexScan * heartRate / 4 % 65) * horizontalFactor * Mathf.PI / 180f - 19f) * 30f + 50;
            }
            if (SceneManager.GetActiveScene().name == "EcografiaUtero")
            {
                horizontalFactor = 3.5f;
                //Funcion!!
                currentValue = Mathf.Cos((indexScan * heartRate / 4 % 65) * horizontalFactor * Mathf.PI / 180f - 19f) * 20f + 50;
            }
            //currentValue = Mathf.Cos((indexScan % 60) * 4.5f * Mathf.PI / 180f - 19f) * 20f + 40; FUNCIONA!
            //currentValue = (Mathf.Cos(horizontalFactor * ((indexScan * heartRate / 4) % 67) * Mathf.PI / 180f) * 60f) + 20;
            /*float period = 200f;
            float amplitude = 50f;
            currentValue = (2 * amplitude / Mathf.PI) * Mathf.Atan(1f / Mathf.Tan(indexScan * heartRate * horizontalFactor * Mathf.PI / period)) + 50 + (Mathf.Cos(horizontalFactor * (indexScan % 67) * heartRate * Mathf.PI / 180f) * 30f);
            */
            currentValue -= pathology * 1f;
            currentValue *= inverseFunction ? -1 : 1;
            //revisa si la funcion esta arriba o abajo del cero
            positiveFunction = currentValue > 0;
            if (positiveFunction)
            {
                currentValue += Random.Range(-currentValue, 0);
            }
            else
            {
                currentValue -= Random.Range(0, currentValue);
            }
            //pinta la parte de abajo (al reves)
            if (!positiveFunction && currentValue > 0)
                currentValue = 0;
            if (positiveFunction && currentValue < 0)
                currentValue = 0;
            if (cursorController.inVein)
            {
                if(cursorController.positive)
                    currentValue = Random.Range(0, 30f);
                else
                    currentValue = Random.Range(-30, 0f);
                texture.SetPixel(indexScan % sizeHorizontal, (int)((currentValue * verticalScale + zero)), Color.white);
            }
            else
                texture.SetPixel(indexScan % sizeHorizontal, (int)((currentValue * verticalScale * cursorController.durezasAngle + zero)), Color.white);
        }
        texture.Apply();
    }

    void drawNumbersHorizontal()
    {
        for (int i = 0; i < horizontalNumbers.Count; i++)
        {
            Destroy((GameObject)horizontalNumbers[i]);
        }
        horizontalNumbers.Clear();

        for (int i = 0; i < 20; i++)
        {
            GameObject g = Instantiate(horizontalNumberPrefab);
            g.transform.parent = transform.parent;
            g.transform.localScale = new Vector3(1f, 1f, 1f);
            g.transform.localPosition = new Vector2(g.transform.localPosition.x + i * 20f - 190f - 85f, -140f);
            horizontalNumbers.Add(g);
            //g.GetComponent<UILabel>().text = "" + (int)(i * 10f * 1.5f) / 100f;
            g.GetComponent<UILabel>().text = "" + (int)(i * 10f * 1.5f * (sliderheart.value + 0.5f)) / 100f;
        }
    }

    // Update is called once per frame
    void Update() {
        if (sleep) return;
        //slider speed
        incrIndexScan = (int)((sliderSpeed.value / 0.5f * 2.5f) + 1) * 2;
        //slider scale
        verticalScale = (sliderScale.value / 0.5f);
        //slider zero
        zero = Mathf.CeilToInt((sliderZero.value - 0.5f) * 140) + sizeVertical / 2;
        //slider gain
        gain = (int)(sliderGain.value * 10);
        //slider power
        power = (int)(sliderPower.value * 10);
        //slider heartrate
        heartRate = (int)((sliderheart.value + 0.5f) * 6f - 2f);
        beatsPerMinute.text = "" + (((sliderheart.value) * 100) + 70);
        if (sliderheart.value >= 0.5f)
        {
            Time.timeScale = sliderheart.value * 1.5f + 0.25f;
        }
        else
        {
            if(sliderheart.value >= 0.375f)
                Time.timeScale = sliderheart.value * 1.4f + 0.2f;
            else
                if (sliderheart.value >= 0.25f)
                    Time.timeScale = sliderheart.value * 1.2f + 0.2f;
                else
                    Time.timeScale = 0.25f;
        }

        if (SceneManager.GetActiveScene().name == "EcografiaUtero")
        {
            //heartRate = (int)((sliderheart.value * 2f + 1f) * 1f - 0f);
            //heartRate = (sliderheart.value * 1.3f - 1.6f) * 1.3f + 3f;
            heartRate = (sliderheart.value * 1.3f - 1.6f) * 1.3f + 3f;
            beatsPerMinute.text = "" + (((sliderheart.value) * 10) + 60);
            if (sliderheart.value >= 0.5f)
            {
                Time.timeScale = sliderheart.value * 0.35f + 0.25f;
            }
            else
            {
                if (sliderheart.value >= 0.375f)
                    Time.timeScale = sliderheart.value * 0.35f + 0.2f;
                else
                    if (sliderheart.value >= 0.25f)
                        Time.timeScale = sliderheart.value * 0.33f + 0.2f;
                else
                    if (sliderheart.value >= 0.125f)
                        Time.timeScale = 0.25f;
                    else
                        Time.timeScale = 0.18f;
            }
        }
        //Time.timeScale = sliderheart.value * 1.5f + 0.3f;
        //incrIndexScan *= heartRate;
        //slider pathology
        pathology = (int)(sliderPathology.value * 30);

        drawFunction();
        drawNumbersHorizontal();
        indexScan += (int)(incrIndexScan);
        //reinicia el contador para reiniciar la funcion desde cero
        if (indexScan >= sizeHorizontal)
            indexScan = 0;

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene("Login");
        }
    }
}
