using System.Collections;
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
    public float verticalScale = 1f;
    int gain = 0;
    int power = 0;
    [HideInInspector]
    public float heartRate = 1;
    bool inverseFunction = false;
    bool inverseGraph = false;
    public bool sleep = false;
    public int pathology = 0;
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
    float deltaTime;
    public LabelIndicators labelIndicators;
    public Transform speedBar;
    // Use this for initialization
    void Start () {
        texture = new Texture2D(sizeHorizontal, sizeVertical);
        GetComponent<Renderer>().material.mainTexture = texture;
        transform.localScale = new Vector3(sizeHorizontal, transform.localScale.y, transform.localScale.z);
        zero = sizeVertical / 2;
        PaintItBlack();
        deltaTime = Time.deltaTime;
		Time.fixedDeltaTime = 0.8f * deltaTime - Time.timeScale * 0.00025f;
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
        //focusController.invertido = true;
        //focusController.changeImages();
    }

    public void functionInverse()
    {
        inverseFunction = true;
        //focusController.invertido = false;
        //focusController.changeImages();
    }

    public void functionInverseGraph()
    {
        inverseGraph = !inverseGraph;
        focusController.invertido = inverseGraph;
        focusController.changeImages();
        speedBar.localScale = new Vector3(1f, -1f * speedBar.localScale.y, 1f);

    }

    public void makeSleep()
    {
        sleep = !sleep;
    }

    void drawFunction()
    {
        float currentValue = 0f;
        bool positiveFunction;
        for (int i = 0; i < 5 + (int)((gain + power) / 2); i++)
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
                //currentValue = Mathf.Cos((indexScan * heartRate / 4 % 65) * horizontalFactor * Mathf.PI / 180f - 19f) * 20f + 40;
                //currentValue = Mathf.Cos((indexScan * heartRate / 4 % 65) * horizontalFactor * Mathf.PI / 180f - 45f) * 20f + 40;
                horizontalFactor = 3.5f;
                //Funcion!!
                //currentValue = Mathf.Cos((indexScan * heartRate / 4 % 65) * horizontalFactor * Mathf.PI / 180f - 19f) * 30f + 50;
                currentValue = Mathf.Cos((indexScan * heartRate / (4 * incrIndexScan / 7f) % 65) * horizontalFactor * Mathf.PI / 180f - ((indexScan * heartRate / (4 * incrIndexScan / 7f) % 65 < 8) ? 15f : 45f) + pathology / 30f) * ((indexScan * heartRate / (4 * incrIndexScan / 7f) % 65 > 65 || indexScan * heartRate / (4 * incrIndexScan / 7f) % 65 < 10) ? 10f - pathology * -2f : 30f + pathology * 0.5f) + ((indexScan * heartRate / (4 * incrIndexScan / 7f) % 65 > 65 || indexScan * heartRate / (4 * incrIndexScan / 7f) % 65 < 10) ? -25f : 0f) + 35f + pathology * -0.5f;

            }
            if (SceneManager.GetActiveScene().name == "EcografiaDuctus")
            {
                horizontalFactor = 1.0f;
                //Funcion!!
                //currentValue = (Mathf.Cos((indexScan * heartRate / 4 % 65) * horizontalFactor * Mathf.PI / 180f * 8) * ((indexScan * heartRate / 4 % 65 < 30)?15f:30f)) + 60f;// + (Mathf.Sin((indexScan % 30) * Mathf.PI / 180f * 3f) * 60f) - (Mathf.Sin((indexScan % 60) * Mathf.PI / 180f * 3f) * 30f);
                if (cursorController.umbilicalDraw)
                {
                    horizontalFactor = 3.5f;
                    /*
                    currentValue = Mathf.Cos((indexScan * heartRate / (4f * incrIndexScan / 7f) % 65) * horizontalFactor * Mathf.PI / 180f - ((indexScan * heartRate / (4f * incrIndexScan / 7f) % 65 < 8) ? 15f : 45f));
                    currentValue *= ((indexScan * heartRate / (4f * incrIndexScan / 7f) % 65 > 65 || indexScan * heartRate / (4f * incrIndexScan / 7f) % 65 < 10) ? 0f : 30f);
                    currentValue += ((indexScan * heartRate / (4f * incrIndexScan / 7f) % 65 > 65 || indexScan * heartRate / (4f * incrIndexScan / 7f) % 65 < 10) ? -25f : 0f) + 45;*/
                    currentValue = Mathf.Cos((indexScan * heartRate / (4 * incrIndexScan / 7f) % 65) * horizontalFactor * Mathf.PI / 180f - ((indexScan * heartRate / (4 * incrIndexScan / 7f) % 65 < 8) ? 15f : 45f) + pathology / 30f) * ((indexScan * heartRate / (4 * incrIndexScan / 7f) % 65 > 65 || indexScan * heartRate / (4 * incrIndexScan / 7f) % 65 < 10) ? 10f - pathology * -2f : 30f + pathology * 0.5f) + ((indexScan * heartRate / (4 * incrIndexScan / 7f) % 65 > 65 || indexScan * heartRate / (4 * incrIndexScan / 7f) % 65 < 10) ? -25f : 0f) + 35f + pathology * -0.5f;

                }
                else
                {
                    currentValue = (Mathf.Cos((indexScan * heartRate / (4f * incrIndexScan / 7f) % 65) * horizontalFactor * Mathf.PI / 180f * 8) * ((indexScan * heartRate / (4f * incrIndexScan / 7f) % 65 < 0) ? 15f : 35f) * ((indexScan * heartRate / (4f * incrIndexScan / 7f) % 65 > 25f && indexScan * heartRate / (4f * incrIndexScan / 7f) % 65 < 40f) ? 0f : 1f));
                    currentValue += ((indexScan * heartRate / (4f * incrIndexScan / 7f) % 65 > 25f && indexScan * heartRate / (4f * incrIndexScan / 7f) % 65 < 40f) ? -35f : 0f);
                    currentValue += ((indexScan * heartRate / 4f % 65 < 60) ? 5f : 10f) + 45;// + (Mathf.Sin((indexScan % 30) * Mathf.PI / 180f * 3f) * 60f) - (Mathf.Sin((indexScan % 60) * Mathf.PI / 180f * 3f) * 30f);
                }
            }
            if (SceneManager.GetActiveScene().name == "EcografiaCerebral")
            {
                horizontalFactor = 3.5f;
                //Funcion!!
                //currentValue = Mathf.Cos((indexScan * heartRate / 4 % 65) * horizontalFactor * Mathf.PI / 180f - 19f) * 30f + 50;
                currentValue = Mathf.Cos((indexScan * (heartRate) / (4 * incrIndexScan / 7f) % 65) * horizontalFactor * Mathf.PI / 180f - ((indexScan * heartRate / (4 * incrIndexScan / 7f) % 65 < 8) ? 15f : 45f)) * ((indexScan * heartRate / (4 * incrIndexScan / 7f) % 65 > 65 || indexScan * heartRate / (4 * incrIndexScan / 7f) % 65 < 10) ? 0f : 50f - pathology * 0.75f) + ((indexScan * heartRate / (4 * incrIndexScan / 7f) % 65 > 65 || indexScan * heartRate / (4 * incrIndexScan / 7f) % 65 < 10) ? -50f + pathology * 0.75f : 0f) + 65 + pathology * 0.75f;
                //currentValue = Mathf.Cos((indexScan * heartRate / 4 % 65) * horizontalFactor * Mathf.PI / 180f - ((indexScan * heartRate / 4 % 65 < 8) ? 15f : 45f)) * 50 + 65;
                //print(indexScan * heartRate / 4 % 65);
            }
            if (SceneManager.GetActiveScene().name == "EcografiaUtero")
            {
                horizontalFactor = 3.5f;
                //Funcion!!
                //currentValue = Mathf.Cos((indexScan * heartRate / (4 * incrIndexScan / 7f) % 65) * horizontalFactor * Mathf.PI / 180f - 19f) * ((indexScan * heartRate / (4 * incrIndexScan / 7f) % 65 > 25) ? 0f : 40f) + ((indexScan * heartRate / (4 * incrIndexScan / 7f) % 65 > 25) ? Mathf.Cos((indexScan * heartRate / (4 * incrIndexScan / 7f) % 65) * 8f * Mathf.PI / 180f - 55f) * (15f - pathology * 1.5f) : 40f) + 35f;
                currentValue = Mathf.Cos((indexScan * heartRate / (4 * incrIndexScan / 7f) % 65) * horizontalFactor * Mathf.PI / 180f - ((indexScan * heartRate / (4 * incrIndexScan / 7f) % 65 < 8) ? 15f : 45f) + pathology / 30f) * ((indexScan * heartRate / (4 * incrIndexScan / 7f) % 65 > 65 || indexScan * heartRate / (4 * incrIndexScan / 7f) % 65 < 10) ? 10f - pathology * -2f : 30f + pathology * 0.5f) + ((indexScan * heartRate / (4 * incrIndexScan / 7f) % 65 > 65 || indexScan * heartRate / (4 * incrIndexScan / 7f) % 65 < 10) ? -25f : 0f) + 55f + pathology;

                //horizontalFactor = 17.0f - heartRate * 2f;;
                // - heartRate * 2f;//currentValue = Mathf.Cos((indexScan * heartRate / 5f % 150) * horizontalFactor * Mathf.PI / 180f - 0f) * ((indexScan * heartRate / 5 % 50 > 30) ? 0f : 40f) + 55f; // ((indexScan * heartRate / 4 % 65 > 25) ? Mathf.Cos((indexScan * heartRate / 4 % 65) * 8f * Mathf.PI / 180f - 55f) * 10f : 40f) + 55f;
            }
            //currentValue = Mathf.Cos((indexScan % 60) * 4.5f * Mathf.PI / 180f - 19f) * 20f + 40; FUNCIONA!
            //currentValue = (Mathf.Cos(horizontalFactor * ((indexScan * heartRate / 4) % 67) * Mathf.PI / 180f) * 60f) + 20;
            /*float period = 200f;
            float amplitude = 50f;
            currentValue = (2 * amplitude / Mathf.PI) * Mathf.Atan(1f / Mathf.Tan(indexScan * heartRate * horizontalFactor * Mathf.PI / period)) + 50 + (Mathf.Cos(horizontalFactor * (indexScan % 67) * heartRate * Mathf.PI / 180f) * 30f);
            */
            if (SceneManager.GetActiveScene().name != "EcografiaCerebral")
                currentValue -= pathology * 1f;

            currentValue *= (inverseFunction) ? -1 : 1;
            currentValue *= ((inverseGraph)) ? -1 : 1;
            //revisa si la funcion esta arriba o abajo del cero
            positiveFunction = currentValue > 0;
            if (i != 0)
            {
                if (positiveFunction)
                {
                    currentValue += Random.Range(-currentValue, 0);
                }
                else
                {
                    currentValue -= Random.Range(0, currentValue);
                }
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
            g.transform.localPosition = new Vector2(g.transform.localPosition.x + i * 20f - 65f, -140f);
            horizontalNumbers.Add(g);
            //g.GetComponent<UILabel>().text = "" + (int)(i * 10f * 1.5f) / 100f;
            if (SceneManager.GetActiveScene().name == "EcografiaUtero")
            {
                if (sliderheart.value <= 0.5f)
                    g.GetComponent<UILabel>().text = "" + (int)(i * 10f * 1.5f * ((sliderheart.value + 0.5f) / 0.45f) / ((incrIndexScan + 0.5f) / 3.1f)) / 100f;
                else
                    g.GetComponent<UILabel>().text = "" + (int)(i * 10f * 1.5f * ((sliderheart.value + 0.5f) / 1.7f) / ((incrIndexScan + 0.5f) / 8.2f)) / 100f;
            }
            else
            {
                if (SceneManager.GetActiveScene().name == "EcografiaUmbilical")
                {
                    if (sliderheart.value <= 0.5f)
                        if (sliderheart.value == 0.125f)
                        {
                            g.GetComponent<UILabel>().text = "" + (int)(i * 10f * 1.5f * ((sliderheart.value + 0.5f) / 1.8f) / ((incrIndexScan + 0.5f) / 9f)) / 100f;
                        }
                        else
                            g.GetComponent<UILabel>().text = "" + (int)(i * 10f * 1.5f * ((sliderheart.value + 0.5f) / 1.1f) / ((incrIndexScan + 0.5f) / 8.2f)) / 100f;
                    else
                        g.GetComponent<UILabel>().text = "" + (int)(i * 10f * 1.5f * ((sliderheart.value + 0.5f) / 1.2f) / ((incrIndexScan + 0.5f) / 8.2f)) / 100f;
                }
                else
                {
                    if (SceneManager.GetActiveScene().name == "EcografiaCerebral")
                    {
                        if (sliderheart.value <= 0.5f)
                            if (sliderheart.value == 0.125f)
                            {
                                g.GetComponent<UILabel>().text = "" + (int)(i * 10f * 1.5f * ((sliderheart.value + 0.5f) / 1.8f) / ((incrIndexScan + 0.5f) / 9f)) / 100f;
                            }
                            else
                                g.GetComponent<UILabel>().text = "" + (int)(i * 10f * 1.5f * ((sliderheart.value + 0.5f) / 1.2f) / ((incrIndexScan + 0.5f) / 8.2f)) / 100f;
                        else
                            g.GetComponent<UILabel>().text = "" + (int)(i * 10f * 1.5f * ((sliderheart.value + 0.5f) / 1.3f) / ((incrIndexScan + 0.5f) / 8.2f)) / 100f;
                    }
                    else
                    {
                        if (sliderheart.value <= 0.5f)
                            if (sliderheart.value == 0.125f)
                            {
                                g.GetComponent<UILabel>().text = "" + (int)(i * 10f * 1.5f * ((sliderheart.value + 0.5f) / 1.8f) / ((incrIndexScan + 0.5f) / 9f)) / 100f;
                            }
                            else
                                g.GetComponent<UILabel>().text = "" + (int)(i * 10f * 1.5f * ((sliderheart.value + 0.5f) / 1.2f) / ((incrIndexScan + 0.5f) / 9f)) / 100f;
                        
                        else
                            g.GetComponent<UILabel>().text = "" + (int)(i * 10f * 1.5f * ((sliderheart.value + 0.5f) / 1.5f) / ((incrIndexScan + 0.5f) / 7.7f)) / 100f;
                    }
                }
            }
        }
    }

    public void adjustTimeFixedScale()
    {
        StartCoroutine(adjustTimeFixedScaleCoroutine());
    }
    IEnumerator adjustTimeFixedScaleCoroutine() {
        yield return new WaitForFixedUpdate();
        float value = 0f;
        if (SceneManager.GetActiveScene().name == "EcografiaUtero")
        {
            if (sliderheart.value >= 0.5f)
            {
                value = 0.02f;
                if (sliderheart.value >= 0.625f)
                {
                    value = 0.017f;
                    if (sliderheart.value >= 0.75f)
                    {
                        value = 0.014f;
                        if (sliderheart.value >= 0.875f)
                        {
                            value = 0.011f;
                            if (sliderheart.value >= 1f)
                            {
                                value = 0.009f;
                                print(sliderheart.value);
                            }
                        }
                    }
                }

            }
            else
            {
                if (sliderheart.value >= 0.375f)
                    value = 0.03f;
                else
                    if (sliderheart.value >= 0.25f)
                    value = 0.038f;
                else
                    if (sliderheart.value >= 0.25f)
                    value = 0.045f;
                else
                    value = 0.067f;
            }
        }
        else
        {
            if (sliderheart.value >= 0.5f)
            {
                value = 0.00025f;
            }
            else
            {
                if (sliderheart.value >= 0.375f)
                    value = 0.003f;
                else
                    if (sliderheart.value >= 0.25f)
                    value = 0.018f;
                else
                    if (sliderheart.value >= 0.125f)
                    value = 0.003f;
                else
                    value = 0.0485f;
                
            }
        }
        Time.fixedDeltaTime = 0.8f * deltaTime - Time.timeScale * value;
        print(Time.fixedDeltaTime);
    }

    public void zeroAdjust()
    {
        labelIndicators.resetZero();
        labelIndicators.adjustZero(Mathf.RoundToInt((sliderZero.value - 0.5f) * 10));
    }

    // Update is called once per frame
    void FixedUpdate() {

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene("Login");
        }
        if (sleep) return;
        //slider speed
        incrIndexScan = (int)((sliderSpeed.value / 0.5f * 2.5f) + 1) * 2;
        //slider scale
        verticalScale = (Mathf.Pow(sliderScale.value, 1.5f) / (0.35355339059f));
        //slider zero
        zero = Mathf.CeilToInt((sliderZero.value - 0.5f) * 140) + sizeVertical / 2;
        //slider gain
        gain = (int)(sliderGain.value * 10);
        //slider power
        power = (int)(sliderPower.value * 10);
        //slider heartrate
        heartRate = (int)((sliderheart.value + 0.5f) * 6f - 2f);
        beatsPerMinute.text = "" + (((sliderheart.value) * 100) + 70);
        /*if (sliderheart.value >= 0.5f)
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
        }*/
        if (sliderheart.value >= 0.5f)
        {
            Time.timeScale = sliderheart.value * 1.5f + 0.25f;
        }
        else
        {
            if (sliderheart.value >= 0.375f)
                Time.timeScale = sliderheart.value * 1.4f + 0.2f;
            else
                if (sliderheart.value >= 0.25f)
                Time.timeScale = sliderheart.value * 1.2f + 0.2f;
            else
                if (sliderheart.value >= 0.125f)
                Time.timeScale = sliderheart.value * 8f + 0.2f;
            else
                Time.timeScale = 0.25f;
        }
        //print(0.8f * Time.deltaTime - Time.timeScale * 0.001f);
        //Time.fixedDeltaTime = 0.8f * Time.deltaTime;
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
        if (sliderScale.value >= 0.5f)
            if(sliderScale.value >= 1f)
                labelIndicators.verticalScale = 1 + (21f - sliderScale.value * 20f) * 0.9f;
            else
                labelIndicators.verticalScale = 1 + (20f - sliderScale.value * 20f) * 0.9f;
        else
        {
            labelIndicators.verticalScale = 0 + (0f + (2f - sliderScale.value) * (2f - sliderScale.value) * (2f - sliderScale.value) * (1.3f - sliderScale.value)) * 1f;
            labelIndicators.verticalScale *= labelIndicators.verticalScale;
        }
        //print(labelIndicators.verticalScale);
        indexScan += 2; // (int)(incrIndexScan);
        //reinicia el contador para reiniciar la funcion desde cero
        if (indexScan >= sizeHorizontal)
            indexScan = 0;

    }
}
