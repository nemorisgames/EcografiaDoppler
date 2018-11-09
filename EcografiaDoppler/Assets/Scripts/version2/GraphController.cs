using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GraphController : MonoBehaviour {
    int sizeHorizontal = 384;
    int sizeVertical = 256;
    Texture2D texture;
    public int zero = 0;
    int indexScan = 0;
    float incrIndexScan = 1;
    public float verticalScale = 1f;
    int gain = 0;
    int power = 0;
    [HideInInspector]
    public float heartRate = 1;
    public bool inverseFunction = false;
    public bool inverseGraph = false;
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
	int ciclo = 0;
	int cicloDots = 0;
	ArrayList positionDots = new ArrayList();
    float function = 256;

    float WFMLevel = 0f;
    public UISlider testSlider;
    AudioSource audioSource;
    public bool useLineRenderer = false;
    // Use this for initialization
    void Start () {
        audioSource = gameObject.AddComponent<AudioSource>() as AudioSource;
        audioSource.clip = Resources.Load<AudioClip>("Ecography") as AudioClip;
        audioSource.loop = true;
        audioSource.Play();
        texture = new Texture2D(sizeHorizontal, sizeVertical);
        GetComponent<Renderer>().material.mainTexture = texture;
        transform.localScale = new Vector3(sizeHorizontal, transform.localScale.y, transform.localScale.z);
        zero = sizeVertical / 2;
        PaintItBlack();
        deltaTime = Time.deltaTime;
		Time.fixedDeltaTime = Time.timeScale * (0.0155f - (sliderSpeed.value - 0.5f) * 0.01f);
		//positionDots = new Vector3[sizeHorizontal * (5 + (int)((gain + power) / 2))];
		print (sizeHorizontal * (5 + (int)((gain + power) / 2)));
    }

    void Awake(){
        lrPoints = new List<Vector3>();
    }

    public void WFMAdd()
    {
        WFMLevel++;
		if (WFMLevel > 10)
			WFMLevel = 10;
    }

    public void WFMLess()
	{
		WFMLevel--;
		if (WFMLevel < 0)
			WFMLevel = 0;
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
        if(lineRenderer != null && useLineRenderer) lineRenderer.positionCount = 0;
        lrPoints.Clear();
    }

    void paintLineVerticalBlack(int indexHorizontal)
    {
        for (int i = 0; i < sizeVertical; i++)
        {
            for (int j = 0; j < incrIndexScan; j++) {
                
                //if (sliderGain.value > 0.5f){
                //    if (inverseFunction)
                //    {
                        //if(zero > i)
                            texture.SetPixel(indexHorizontal + j, i, new Color(sliderGain.value - 0.5f, sliderGain.value - 0.5f, sliderGain.value - 0.5f));
                        //else
                        //    texture.SetPixel(indexHorizontal + j, i, Color.black);
                /*    }
                    else
                    {
                        if (zero < i)
                            texture.SetPixel(indexHorizontal + j, i, new Color(sliderGain.value - 0.5f, sliderGain.value - 0.5f, sliderGain.value - 0.5f));
                        else
                            texture.SetPixel(indexHorizontal + j, i, Color.black);
                    }
                }
                else
                {
                    texture.SetPixel(indexHorizontal + j, i, Color.black);
                }
                    */
            }
        }
        //pinta linea blanca del zero
        for(int i = 0; i < incrIndexScan; i++)
            texture.SetPixel(indexHorizontal + i, zero, Color.white);
		//positionDots [indexHorizontal]
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
        focusController.changeImagesNoAngle();
        speedBar.localScale = new Vector3(1f, -1f * speedBar.localScale.y, 1f);
        StartCoroutine(inverseGraphCoroutine());
    }

    IEnumerator inverseGraphCoroutine()
    {
        yield return new WaitForSeconds(0.3f); 
        cursorController.GetPixelColor();
    }

    public void makeSleep()
    {
        sleep = !sleep;
        DrawLine.Instance.ActivarPanelLineas(sleep);
    }

    public void reset()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); 
    }

    float currentValue = 0f;
    float tiempoBarrido = 0f;

    void drawFunction()
    {
        currentValue = 0f;
        bool positiveFunction;
		if(indexScan % sizeHorizontal == 2)
		{
            float aux = Time.time - tiempoBarrido;
            //Debug.Log("speed: "+sliderSpeed.value+" - "+aux/Time.timeScale+" s, heart: "+sliderheart.value);
			focusController.resetCont();
            tiempoBarrido = Time.time;
            if(lineRenderer != null && useLineRenderer){
                lineRenderer.positionCount = 0;
                lineRenderer.positionCount = lrPoints.Count;
                lineRenderer.SetPositions(lrPoints.ToArray());
                lrPoints.Clear();
            }
		}
        for (int i = 0; i < 6 + Mathf.Pow(2f, (int)((gain + power)/2f)) + (sliderScale.value - 0.5f) * 25; i++)
        {
			
            //factor independiente para cada funcion que hace que calce con la medicion de latidos por segundos
            float horizontalFactor = 4.5f;//1.6f;//
            if (SceneManager.GetActiveScene().name == "EcografiaUmbilical")
            {
                //Funcion!!
				//horizontalFactor = 5f;
                int repeticion = (65);

                if (sliderheart.value > 0.5f)
                {
                    //original
                    if ((indexScan * heartRate / (4f * incrIndexScan / 7f) % repeticion) < heartRate * 7f / 3f - 1f / 3f)
                    {
                        horizontalFactor = 20f;
                        float B7 = sliderheart.value * 8f - 4f;
                        repeticion = Mathf.RoundToInt(0.75f * B7 * B7 - 9.65f * B7 + 63.75f);
                        currentValue = Mathf.Cos((indexScan * 4f / (4f * incrIndexScan / 7f) % repeticion) * horizontalFactor * Mathf.PI / 180f + ((indexScan * 4f / (4f * incrIndexScan / 7f) % repeticion < 8) ? 35f : 45f));// + pathology / 30f);
                        currentValue *= 1.1f * (3.5f / (heartRate + 1f));
                        currentValue += 0.45f + (heartRate - 4f) * 0.3f;
                    }
                    else
                    {
                        horizontalFactor = 6f;
                        //ORIGINAL
                        if ((indexScan * heartRate / (4f * incrIndexScan / 7f) % repeticion) < heartRate * 4.5f + 9f - (9f - heartRate * 7f / 3f - 1f / 3f))
                        {
                            float B7 = sliderheart.value * 8f - 4f;
                            repeticion = Mathf.RoundToInt(0.75f * B7 * B7 - 9.65f * B7 + 63.75f);
                            currentValue = Mathf.Cos((indexScan * 4f / (4f * incrIndexScan / 7f) % repeticion) * horizontalFactor * Mathf.PI / 180f - ((indexScan * 4f / (4f * incrIndexScan / 7f) % repeticion < 8) ? 5f : 45f));// + pathology / 30f);
                            currentValue += 0.65f;
                        }
                        else
                        {
                            //ORIGINAL
                            /*if ((indexScan * heartRate / (4f * incrIndexScan / 7f) % repeticion) < heartRate * 4.5f + 9f - (9f - heartRate * 7f / 3f - 1f / 3f))
                            {
                                currentValue = Mathf.Cos((indexScan * 4f / (4f * incrIndexScan / 7f) % repeticion) * horizontalFactor * Mathf.PI / 180f - ((indexScan * 4f / (4f * incrIndexScan / 7f) % repeticion < 8) ? 5f : 45f));// + pathology / 30f);
                                currentValue += 0.65f;
                            }
                            else
                            {*/
                                //ORIGINAL
                                horizontalFactor = 3f;
                                if ((indexScan * heartRate / (4f * incrIndexScan / 7f) % repeticion) <= 65f)
                                {
                                    horizontalFactor = 3.5f;
                                float B7 = sliderheart.value * 8f - 4f;
                                repeticion = Mathf.RoundToInt(0.75f * B7 * B7 - 9.65f * B7 + 63.75f);
                                currentValue = Mathf.Cos((indexScan * 4f / (4f * incrIndexScan / 7f) % repeticion) * horizontalFactor * Mathf.PI / 180f - 56.5f);// + pathology / 30f);
                                    currentValue += 0.4f + (heartRate - 4f) * 0.1f;
                                    currentValue *= 1f * (5f / (heartRate + 1f));
                                }
                            //}
                        }
                    }
                }
                else
                {
                    if ((indexScan * heartRate / (4f * incrIndexScan / 7f) % repeticion) < heartRate * 7f / 3f - 1f / 3f)
                    {
                        horizontalFactor = 20f;//20f ;
                        repeticion = (int)(1.542f * Mathf.Pow(heartRate * 4f / 3f - 1 / 3f, 4) - 23.42f * Mathf.Pow(heartRate * 4f / 3f - 1 / 3f, 3) + 136f * Mathf.Pow(heartRate * 4f / 3f - 1 / 3f, 2) + -379.1f * (heartRate * 4f / 3f - 1 / 3f) + 525f);
                        currentValue = Mathf.Cos((indexScan * 4f / (4f * incrIndexScan / 7f) % repeticion) * horizontalFactor * Mathf.PI / 180f + ((indexScan * 4f / (4f * incrIndexScan / 7f) % repeticion < 8) ? 35f : 45f));// + pathology / 30f);
                        currentValue *= 1.1f * (3.5f / (4f + 1f)) ;
                        currentValue += 0.45f + (4f - 4f) * 0.3f;
                    }
                    else
                    {
                        horizontalFactor = 6f;

                        if ((indexScan * heartRate / (4f * incrIndexScan / 7f) % repeticion) < heartRate * 4.5f + 9f - (9f - heartRate * 7f / 3f - 1f / 3f))
                        {
                            repeticion = (int)(1.542f * Mathf.Pow(heartRate * 4f / 3f - 1 / 3f, 4) - 23.42f * Mathf.Pow(heartRate * 4f / 3f - 1 / 3f, 3) + 136f * Mathf.Pow(heartRate * 4f / 3f - 1 / 3f, 2) + -379.1f * (heartRate * 4f / 3f - 1 / 3f) + 525f);
                            //print(heartRate + " " + repeticion);
                            horizontalFactor = 5.5f;//heartRate * -1.6666f + 12.6666f;
                            currentValue = Mathf.Cos((indexScan * 4f / (4f * incrIndexScan / 7f) % repeticion) * horizontalFactor * Mathf.PI / 180f - ((indexScan * 4f / (4f * incrIndexScan / 7f) % repeticion < 8) ? 5f : 45f));// + pathology / 30f);
                            currentValue += 0.65f;
                        }
                        else
                        {


                            if ((indexScan * heartRate / (4f * incrIndexScan / 7f) % repeticion) <= 65f)
                            {
                                horizontalFactor = 2.5f;
                                repeticion = 65;// (int)(1.542f * Mathf.Pow(heartRate * 4f / 3f - 1 / 3f, 4) - 23.42f * Mathf.Pow(heartRate * 4f / 3f - 1 / 3f, 3) + 136f * Mathf.Pow(heartRate * 4f / 3f - 1 / 3f, 2) + -379.1f * (heartRate * 4f / 3f - 1 / 3f) + 525f);
                                currentValue = Mathf.Cos((indexScan * heartRate / (4f * incrIndexScan / 7f) % repeticion) * horizontalFactor * Mathf.PI / 180f - 56.5f + 0.16666f * (4f - heartRate));// + pathology / 30f);
                                currentValue += 0.2f + (heartRate - 4f) * 0.1f;//0.4f + (heartRate - 4f) * 0.1f;
                                currentValue *= 0.5f;
                            }
                        }
                    }
                }
                currentValue *= 0.5f;
                //currentValue = Mathf.Cos ((indexScan * heartRate / (4f * incrIndexScan / 7f) % repeticion) * horizontalFactor * Mathf.PI / 180f - ((indexScan * heartRate / (4f * incrIndexScan / 7f) % repeticion < 8) ? 5f : 45f));// + pathology / 30f);
                //lo parada de la grafica
                currentValue *= 30f + pathology * 1.6f;
                //elevado del ultimo tramo
                currentValue += 45f + pathology * -1.5f;
            }
            if (SceneManager.GetActiveScene().name == "EcografiaDuctus")
            {
                horizontalFactor = 1.0f;
                //Funcion!!
                //currentValue = (Mathf.Cos((indexScan * heartRate / 4 % 65) * horizontalFactor * Mathf.PI / 180f * 8) * ((indexScan * heartRate / 4 % 65 < 30)?15f:30f)) + 60f;// + (Mathf.Sin((indexScan % 30) * Mathf.PI / 180f * 3f) * 60f) - (Mathf.Sin((indexScan % 60) * Mathf.PI / 180f * 3f) * 30f);
                if (sliderheart.value > 0.5f)
                {
                    if (cursorController.umbilicalDraw)
                    {
                        int repeticion = 65;
                        //ORIGINAL
                        if ((indexScan * heartRate / (4f * incrIndexScan / 7f) % repeticion) < heartRate * 7f / 3f - 1f / 3f)
                        {
                            horizontalFactor = 20f;
                            float B7 = sliderheart.value * 8f - 4f;
                            repeticion = Mathf.RoundToInt(0.75f * B7 * B7 - 9.65f * B7 + 63.75f);
                            currentValue = Mathf.Cos((indexScan * 4f / (4f * incrIndexScan / 7f) % repeticion) * horizontalFactor * Mathf.PI / 180f + ((indexScan * 4f / (4f * incrIndexScan / 7f) % repeticion < 8) ? 35f : 45f));// + pathology / 30f);
                            currentValue *= 1.1f * (3.5f / (heartRate + 1f));
                            currentValue += 0.45f + (heartRate - 4f) * 0.3f;
                        }
                        else
                        {
                            horizontalFactor = 6f;
                            //ORIGINAL
                            if ((indexScan * heartRate / (4f * incrIndexScan / 7f) % repeticion) < heartRate * 4.5f + 9f - (9f - heartRate * 7f / 3f - 1f / 3f))
                            {
                                float B7 = sliderheart.value * 8f - 4f;
                                repeticion = Mathf.RoundToInt(0.75f * B7 * B7 - 9.65f * B7 + 63.75f);
                                currentValue = Mathf.Cos((indexScan * 4f / (4f * incrIndexScan / 7f) % repeticion) * horizontalFactor * Mathf.PI / 180f - ((indexScan * 4f / (4f * incrIndexScan / 7f) % repeticion < 8) ? 5f : 45f));// + pathology / 30f);
                                currentValue += 0.65f;
                            }
                            else
                            {
                                horizontalFactor = 3f;
                                //ORIGINAL

                                if ((indexScan * heartRate / (4f * incrIndexScan / 7f) % repeticion) <= 65f)
                                {
                                    float B7 = sliderheart.value * 8f - 4f;
                                    repeticion = Mathf.RoundToInt(0.75f * B7 * B7 - 9.65f * B7 + 63.75f);
                                    currentValue = Mathf.Cos((indexScan * 4f / (4f * incrIndexScan / 7f) % repeticion) * horizontalFactor * Mathf.PI / 180f - 56.5f);// + pathology / 30f);
                                    currentValue += 0.4f + (heartRate - 4f) * 0.1f;
                                    currentValue *= 1f * (5f / (heartRate + 1f));
                                }
                            }
                        }
                        currentValue *= 0.5f;
                        //currentValue = Mathf.Cos ((indexScan * heartRate / (4f * incrIndexScan / 7f) % repeticion) * horizontalFactor * Mathf.PI / 180f - ((indexScan * heartRate / (4f * incrIndexScan / 7f) % repeticion < 8) ? 5f : 45f));// + pathology / 30f);
                        //lo parada de la grafica
                        currentValue *= 30f + pathology * 1.6f;
                        //elevado del ultimo tramo
                        currentValue += 45f + pathology * -1.5f;
                    }
                    else
                    {
                        int repeticion = 65;
                        //ORIGINAL
                        if ((indexScan * heartRate / (4f * incrIndexScan / 7f) % repeticion) < heartRate * 7f / 3f - 1f / 3f)
                        {
                            horizontalFactor = 20f;
                            float B7 = sliderheart.value * 8f - 4f;
                            repeticion = Mathf.RoundToInt(0.75f * B7 * B7 - 9.65f * B7 + 63.75f);// (int)(-3.667f * B7 * B7 * B7 + 24.5f * B7 * B7 - 45.8f * B7 + 91);
                            currentValue = Mathf.Cos((indexScan * 4f / (4f * incrIndexScan / 7f) % repeticion) * horizontalFactor * Mathf.PI / 180f + ((indexScan * 4f / (4f * incrIndexScan / 7f) % repeticion < 8) ? 35f : 45f));// + pathology / 30f);
                            currentValue *= 1.1f - pathology * 0.02f;
                            currentValue += 0.45f;
                        }
                        else
                        {
                            horizontalFactor = 9f;
                            //ORIGINAL
                            float valor = sliderheart.value * 8f - 4f;
                            if ((indexScan * heartRate / (4f * incrIndexScan / 7f) % repeticion) <  1.833f * valor * valor * valor - 16.5f * valor * valor + 42.67f * valor + 8)//heartRate * 6.5f + 9f - (9f - heartRate * 7f / 3f - 1f / 3f))
                            {
                                float B7 = sliderheart.value * 8f - 4f;
                                repeticion = Mathf.RoundToInt(-10.83f * valor * valor * valor + 74f * valor * valor - 127.2f * valor + 92f);// (int)(-3.667f * B7 * B7 * B7 + 24.5f * B7 * B7 - 45.8f * B7 + 91);
                                currentValue = Mathf.Cos((indexScan * 4f / (4f * incrIndexScan / 7f) % repeticion) * horizontalFactor * Mathf.PI / 180f - ((indexScan * 4f / (4f * incrIndexScan / 7f) % repeticion < 8) ? 5f : 46f));// + pathology / 30f);
                                //currentValue *= 1f - pathology / 60f;
                                //currentValue += 0.65f;
                                currentValue *= 1f - pathology / 60f + sliderPathology.value * 1.8f;
                                currentValue += 0.65f - sliderPathology.value * 0.7f;
                            }
                            else
                            {
                                //ORIGINAL
                                valor = sliderheart.value * 8f - 4f;
                                if ((indexScan * heartRate / (4f * incrIndexScan / 7f) % repeticion) < 0.6667f * valor * valor * valor - 7f * valor * valor + 14.33f * valor + 42f)// 12.3333f * heartRate + 0.666f)
                                {
                                    horizontalFactor = 17f;
                                    float B7 = sliderheart.value * 8f - 4f;
                                    repeticion = Mathf.RoundToInt(0.75f * B7 * B7 - 9.65f * B7 + 63.75f);// (int)(-3.667f * B7 * B7 * B7 + 24.5f * B7 * B7 - 45.8f * B7 + 91);

                                    currentValue = Mathf.Cos((indexScan * 4f / (4f * incrIndexScan / 7f) % repeticion) * horizontalFactor * Mathf.PI / 180f - ((indexScan * 4f / (4f * incrIndexScan / 7f) % repeticion < 8) ? 5f : 50f));// + pathology / 30f);
                                    currentValue *= 0.6f - pathology / 60f;
                                    currentValue -= 0.1f;
                                }
                                else
                                {
                                    horizontalFactor = 17f;
                                    //ORIGINAL
                                    if ((indexScan * heartRate / (4f * incrIndexScan / 7f) % repeticion) <= 65f)
                                    {
                                        horizontalFactor = 17f;
                                        float B7 = sliderheart.value * 8f - 4f;
                                        repeticion = Mathf.RoundToInt(-5.333f * B7 * B7 * B7 + 29f * B7 * B7 - 32.67f * B7 + 64f);
                                        currentValue = Mathf.Cos((indexScan * 4f / (4f * incrIndexScan / 7f) % repeticion) * horizontalFactor * Mathf.PI / 180f - (-0.9167f * valor * valor * valor + 7.5f * valor * valor - 20.58f * valor + 77));// + pathology / 30f);
                                        //TO DO
                                        currentValue *= 0.25f * valor * valor * valor - 1.75f * valor * valor + 4.5f * valor - 2f + pathology / 50f;
                                        valor = pathology / 30f;
                                        currentValue -= 0.1667f * valor * valor * valor - 1.25f * valor * valor + 3.583f * valor - 2f -0.5f * valor * valor * valor + 3.5f * valor * valor - 8f * valor + 8f;//1.0f + pathology / 10f;
                                    }
                                }
                            }
                        }

                        currentValue *= 0.5f;
                        //currentValue = Mathf.Cos ((indexScan * heartRate / (4f * incrIndexScan / 7f) % repeticion) * horizontalFactor * Mathf.PI / 180f - ((indexScan * heartRate / (4f * incrIndexScan / 7f) % repeticion < 8) ? 5f : 45f));// + pathology / 30f);
                        //lo parada de la grafica
                        currentValue *= 30f;
                        //elevado del ultimo tramo
                        currentValue += 65f + pathology * -0.3f;
                    }
                }
                else
                {
                    if (cursorController.umbilicalDraw)
                    {
                        int repeticion = 65;
                        if ((indexScan * heartRate / (4f * incrIndexScan / 7f) % repeticion) < heartRate * 7f / 3f - 1f / 3f)
                        {
                            horizontalFactor = 20f;//20f ;
                            repeticion = (int)(1.542f * Mathf.Pow(heartRate * 4f / 3f - 1 / 3f, 4) - 23.42f * Mathf.Pow(heartRate * 4f / 3f - 1 / 3f, 3) + 136f * Mathf.Pow(heartRate * 4f / 3f - 1 / 3f, 2) + -379.1f * (heartRate * 4f / 3f - 1 / 3f) + 525f);
                            currentValue = Mathf.Cos((indexScan * 4f / (4f * incrIndexScan / 7f) % repeticion) * horizontalFactor * Mathf.PI / 180f + ((indexScan * 4f / (4f * incrIndexScan / 7f) % repeticion < 8) ? 35f : 45f));// + pathology / 30f);
                            currentValue *= 1.1f * (3.5f / (4f + 1f));
                            currentValue += 0.45f + (4f - 4f) * 0.3f;
                        }
                        else
                        {
                            horizontalFactor = 6f;
                            if ((indexScan * heartRate / (4f * incrIndexScan / 7f) % repeticion) < heartRate * 4.5f + 9f - (9f - heartRate * 7f / 3f - 1f / 3f))
                            {
                                repeticion = (int)(1.542f * Mathf.Pow(heartRate * 4f / 3f - 1 / 3f, 4) - 23.42f * Mathf.Pow(heartRate * 4f / 3f - 1 / 3f, 3) + 136f * Mathf.Pow(heartRate * 4f / 3f - 1 / 3f, 2) + -379.1f * (heartRate * 4f / 3f - 1 / 3f) + 525f);
                                //print(heartRate + " " + repeticion);
                                horizontalFactor = 5.5f;//heartRate * -1.6666f + 12.6666f;
                                currentValue = Mathf.Cos((indexScan * 4f / (4f * incrIndexScan / 7f) % repeticion) * horizontalFactor * Mathf.PI / 180f - ((indexScan * 4f / (4f * incrIndexScan / 7f) % repeticion < 8) ? 5f : 45f));// + pathology / 30f);
                                currentValue += 0.65f;
                            }
                            else
                            {
                                horizontalFactor = 3f;
                                if ((indexScan * heartRate / (4f * incrIndexScan / 7f) % repeticion) <= 65f)
                                {
                                    horizontalFactor = 2.5f;
                                    repeticion = 65;// (int)(1.542f * Mathf.Pow(heartRate * 4f / 3f - 1 / 3f, 4) - 23.42f * Mathf.Pow(heartRate * 4f / 3f - 1 / 3f, 3) + 136f * Mathf.Pow(heartRate * 4f / 3f - 1 / 3f, 2) + -379.1f * (heartRate * 4f / 3f - 1 / 3f) + 525f);
                                    currentValue = Mathf.Cos((indexScan * heartRate / (4f * incrIndexScan / 7f) % repeticion) * horizontalFactor * Mathf.PI / 180f - 56.5f + 0.16666f * (4f - heartRate));// + pathology / 30f);
                                    currentValue += 0.2f + (heartRate - 4f) * 0.1f;//0.4f + (heartRate - 4f) * 0.1f;
                                    currentValue *= 1f * (5f / (4f + 1f));
                                }
                            }
                        }
                        currentValue *= 0.5f;
                        //currentValue = Mathf.Cos ((indexScan * heartRate / (4f * incrIndexScan / 7f) % repeticion) * horizontalFactor * Mathf.PI / 180f - ((indexScan * heartRate / (4f * incrIndexScan / 7f) % repeticion < 8) ? 5f : 45f));// + pathology / 30f);
                        //lo parada de la grafica
                        currentValue *= 30f + pathology * 1.6f;
                        //elevado del ultimo tramo
                        currentValue += 45f + pathology * -1.5f;

                    }
                    else
					{
                        int repeticion = 65;
                        if ((indexScan * heartRate / (4f * incrIndexScan / 7f) % repeticion) < heartRate * 7f / 3f - 1f / 3f)
                        {
                            horizontalFactor = 20f;//20f ;
                            repeticion = (int)(1.542f * Mathf.Pow(heartRate * 4f / 3f - 1 / 3f, 4) - 23.42f * Mathf.Pow(heartRate * 4f / 3f - 1 / 3f, 3) + 136f * Mathf.Pow(heartRate * 4f / 3f - 1 / 3f, 2) + -379.1f * (heartRate * 4f / 3f - 1 / 3f) + 525f);
                            currentValue = Mathf.Cos((indexScan * 4f / (4f * incrIndexScan / 7f) % repeticion) * horizontalFactor * Mathf.PI / 180f + ((indexScan * 4f / (4f * incrIndexScan / 7f) % repeticion < 8) ? 35f : 45f));// + pathology / 30f);
                            currentValue *= 1.1f - pathology * 0.02f;
                            currentValue += 0.45f;
                        }
                        else
                        {
                            horizontalFactor = 8.5f;
                            if ((indexScan * heartRate / (4f * incrIndexScan / 7f) % repeticion) < heartRate * 6.5f + 9f - (9f - heartRate * 7f / 3f - 1f / 3f))
                            {
                                repeticion = (int)(1.542f * Mathf.Pow(heartRate * 4f / 3f - 1 / 3f, 4) - 23.42f * Mathf.Pow(heartRate * 4f / 3f - 1 / 3f, 3) + 136f * Mathf.Pow(heartRate * 4f / 3f - 1 / 3f, 2) + -379.1f * (heartRate * 4f / 3f - 1 / 3f) + 525f);
                                currentValue = Mathf.Cos((indexScan * 4f / (4f * incrIndexScan / 7f) % repeticion) * horizontalFactor * Mathf.PI / 180f - ((indexScan * 4f / (4f * incrIndexScan / 7f) % repeticion < 8) ? 5f : 46f));// + pathology / 30f);
                                currentValue *= 1f - pathology / 60f + sliderPathology.value * 1.8f;
                                currentValue += 0.65f - sliderPathology.value * 0.7f;
                            }
                            else
                            {
                                float valor = heartRate / 0.75f - 1f / 3f;
                                if ((indexScan * heartRate / (4f * incrIndexScan / 7f) % repeticion) <= 10.48f * Mathf.Pow(valor, 0.8645f)) //12.3333f * heartRate + 0.666f)
                                {
                                    horizontalFactor = 17f;
                                    valor = heartRate / 0.75f - 1f / 3f;
                                    repeticion = (int)(-1.583f * valor * valor * valor * valor + 19.17f * valor * valor * valor - 73.42f * valor * valor + 59.83f * valor + 194f);
                                    currentValue = Mathf.Cos((indexScan * 4f / (4f * incrIndexScan / 7f) % repeticion) * horizontalFactor * Mathf.PI / 180f - ((indexScan * 4f / (4f * incrIndexScan / 7f) % repeticion < 8) ? 5f : 50f));// + pathology / 30f);
                                    currentValue *= 0.6f - pathology / 60f;
                                    currentValue -= 0.1f;
                                }
                                else
                                {
                                    valor = heartRate / 0.75f - 1f / 3f;
                                    horizontalFactor = 0.0625f * valor * valor * valor * valor - 0.5417f * valor * valor * valor + 1.938f * valor * valor - 2.458f * valor + 2.5f;// 10f * (testSlider.value * 10f);//1.25f * (heartRate / 0.75f - 1f / 3f) * (heartRate / 0.75f - 1f / 3f) - 4f * (heartRate / 0.75f - 1f / 3f) + 5.75f;
                                    //ORIGINAL
                                    if ((indexScan * heartRate / (4f * incrIndexScan / 7f) % repeticion) <= 65f)
                                    {
                                        
                                        currentValue = Mathf.Cos((indexScan * heartRate / (4f * incrIndexScan / 7f) % repeticion) * horizontalFactor * Mathf.PI / 180f - (-0.2083f * Mathf.Pow((heartRate / 0.75f - 1f / 3f), 4) + 2.167f * Mathf.Pow((heartRate / 0.75f - 1f / 3f), 3) - 7.542f * Mathf.Pow((heartRate / 0.75f - 1f / 3f), 2) + 11.08f * (heartRate / 0.75f - 1f / 3f) + 50.5f));// + pathology / 30f);
                                        currentValue *= 1.01f + 1.5f * pathology / 30f + valor * -0.5f + 2f;
                                        currentValue -= 0.35f + pathology * 0.08f;// + testSlider.value;//1.0f + pathology / 10f;
                                    }
                                    /*if ((indexScan * heartRate / (4f * incrIndexScan / 7f) % repeticion) <= 65f)
                                    {
                                        //horizontalFactor = 20f;//20f ;
                                        //repeticion = (int)(1.542f * Mathf.Pow(heartRate * 4f / 3f - 1 / 3f, 4) - 23.42f * Mathf.Pow(heartRate * 4f / 3f - 1 / 3f, 3) + 136f * Mathf.Pow(heartRate * 4f / 3f - 1 / 3f, 2) + -379.1f * (heartRate * 4f / 3f - 1 / 3f) + 525f);
                                        currentValue = Mathf.Cos((indexScan * 4f / (4f * incrIndexScan / 7f) % repeticion) * horizontalFactor * Mathf.PI / 180f - 58f);// + pathology / 30f);
                                        currentValue *= 0.5f + pathology / 15f;
                                        currentValue -= 1.0f + pathology / 10f;
                                    }*/
                                }
                            }
                        }
                        currentValue *= 0.5f;
                        //currentValue = Mathf.Cos ((indexScan * heartRate / (4f * incrIndexScan / 7f) % repeticion) * horizontalFactor * Mathf.PI / 180f - ((indexScan * heartRate / (4f * incrIndexScan / 7f) % repeticion < 8) ? 5f : 45f));// + pathology / 30f);
                        //lo parada de la grafica
                        currentValue *= 30f;
                        //elevado del ultimo tramo
                        currentValue += 65f + pathology * -0.3f;
                    }
                }
            }
            if (SceneManager.GetActiveScene().name == "EcografiaCerebral")
            {
                int repeticion = 65;
                if (sliderheart.value > 0.5f)
                {

                    //ORIGINAL
                    /*
                    if ((indexScan * heartRate / (4f * incrIndexScan / 7f) % repeticion) < 9f)
                    {
                        horizontalFactor = 20f;
                        currentValue = Mathf.Cos((indexScan * heartRate / (4f * incrIndexScan / 7f) % repeticion) * horizontalFactor * Mathf.PI / 180f + ((indexScan * heartRate / (4f * incrIndexScan / 7f) % repeticion < 8) ? 35f : 45f));// + pathology / 30f);
                        currentValue *= 1.1f;
                        currentValue += 0.45f;
                    }
                    else
                    {
                        //ORIGINAL
                        horizontalFactor = 6f;
                        if ((indexScan * heartRate / (4f * incrIndexScan / 7f) % repeticion) < 27f)
                        {
                            currentValue = Mathf.Cos((indexScan * heartRate / (4f * incrIndexScan / 7f) % repeticion) * horizontalFactor * Mathf.PI / 180f - ((indexScan * heartRate / (4f * incrIndexScan / 7f) % repeticion < 8) ? 5f : 45f));// + pathology / 30f);
                            currentValue += 0.65f;
                        }
                        else
                        {
                            //ORIGINAL
                            horizontalFactor = 3f;
                            if ((indexScan * heartRate / (4f * incrIndexScan / 7f) % repeticion) <= 65f)
                            {
                                currentValue = Mathf.Cos((indexScan * heartRate / (4f * incrIndexScan / 7f) % repeticion) * horizontalFactor * Mathf.PI / 180f - 56.5f);// + pathology / 30f);
                                currentValue += 0.4f;
                            }

                        }
                    }*/
                    if ((indexScan * heartRate / (4f * incrIndexScan / 7f) % repeticion) <  heartRate * 7f / 3f - 1f / 3f)
                    {
                        horizontalFactor = 20f;
                        float B7 = sliderheart.value * 8f - 4f;
                        repeticion =  (int)(-B7 * B7 * B7 + 8.5f * B7 * B7 - 27.5f * B7 + 75f);
                        currentValue = Mathf.Cos((indexScan * 4f / (4f * incrIndexScan / 7f) % repeticion) * horizontalFactor * Mathf.PI / 180f + ((indexScan * 4f / (4f * incrIndexScan / 7f) % repeticion < 8) ? 35f : 45f));// + pathology / 30f);
                        currentValue *= 1.1f;
                        currentValue += 0.45f;
                    }
                    else
                    {
                        //ORIGINAL
                        float B7 = sliderheart.value * 8f - 4f;
                        horizontalFactor = -0.05f * B7 * B7 * B7 + 0.55f * B7 * B7 - 2.3f * B7 + 6.8f;
                        if ((indexScan * heartRate / (4f * incrIndexScan / 7f) % repeticion) < heartRate * 4.5f + 9f - (9f - heartRate * 7f / 3f - 1f / 3f))
                        {
                            //repeticion = (int)(1.542f * Mathf.Pow(heartRate * 4f / 3f - 1 / 3f, 4) - 23.42f * Mathf.Pow(heartRate * 4f / 3f - 1 / 3f, 3) + 136f * Mathf.Pow(heartRate * 4f / 3f - 1 / 3f, 2) + -379.1f * (heartRate * 4f / 3f - 1 / 3f) + 525f);
                            repeticion = 65;// (int)(55 + testSlider.value * 10f);
                            currentValue = Mathf.Cos((indexScan * heartRate / (4f * incrIndexScan / 7f) % repeticion) * horizontalFactor * Mathf.PI / 180f - ((indexScan * heartRate / (4f * incrIndexScan / 7f) % repeticion < 8) ? 5f : 45f));// + pathology / 30f);
                            currentValue += 0.65f;
                        }
                        else
                        {
                            //ORIGINAL
                            B7 = sliderheart.value * 8f - 4f;
                            horizontalFactor = -0.1f * B7 * B7 * B7 + 0.75f * B7 * B7 - 1.85f * B7 + 3f;
                            if ((indexScan * heartRate / (4f * incrIndexScan / 7f) % repeticion) <= 65f)
                            {
                                repeticion = 65;// (int)(1.542f * Mathf.Pow(heartRate * 4f / 3f - 1 / 3f, 4) - 23.42f * Mathf.Pow(heartRate * 4f / 3f - 1 / 3f, 3) + 136f * Mathf.Pow(heartRate * 4f / 3f - 1 / 3f, 2) + -379.1f * (heartRate * 4f / 3f - 1 / 3f) + 525f);
                                currentValue = Mathf.Cos((indexScan * heartRate / (4f * incrIndexScan / 7f) % repeticion) * horizontalFactor * Mathf.PI / 180f - 56.5f);// + pathology / 30f);
                                currentValue += 0.4f;
                            }

                        }
                    }
                }
                else
                {
                    if ((indexScan * heartRate / (4f * incrIndexScan / 7f) % repeticion) < heartRate * 7f / 3f - 1f / 3f)
                    {
                        horizontalFactor = 20f;//20f ;
                        repeticion = (int)(1.542f * Mathf.Pow(heartRate * 4f / 3f - 1 / 3f, 4) - 23.42f * Mathf.Pow(heartRate * 4f / 3f - 1 / 3f, 3) + 136f * Mathf.Pow(heartRate * 4f / 3f - 1 / 3f, 2) + -379.1f * (heartRate * 4f / 3f - 1 / 3f) + 525f);
                        currentValue = Mathf.Cos((indexScan * 4f / (4f * incrIndexScan / 7f) % repeticion) * horizontalFactor * Mathf.PI / 180f + ((indexScan * 4f / (4f * incrIndexScan / 7f) % repeticion < 8) ? 35f : 45f));// + pathology / 30f);
                        currentValue *= 1.1f;
                        currentValue += 0.45f;
                    }
                    else
                    {
                        
                        if ((indexScan * heartRate / (4f * incrIndexScan / 7f) % repeticion) < heartRate * 4.5f + 9f - (9f - heartRate * 7f / 3f - 1f / 3f))
                        {
                            repeticion = (int)(1.542f * Mathf.Pow(heartRate * 4f / 3f - 1 / 3f, 4) - 23.42f * Mathf.Pow(heartRate * 4f / 3f - 1 / 3f, 3) + 136f * Mathf.Pow(heartRate * 4f / 3f - 1 / 3f, 2) + -379.1f * (heartRate * 4f / 3f - 1 / 3f) + 525f);
                            //print(heartRate + " " + repeticion);
                            horizontalFactor = 6f;//heartRate * -1.6666f + 12.6666f;
                            currentValue = Mathf.Cos((indexScan * 4f / (4f * incrIndexScan / 7f) % repeticion) * horizontalFactor * Mathf.PI / 180f - ((indexScan * 4f / (4f * incrIndexScan / 7f) % repeticion < 8) ? 5f : 45f));// + pathology / 30f);
                            currentValue += 0.65f;
                        }
                        else
                        {
                            if ((indexScan * heartRate / (4f * incrIndexScan / 7f) % repeticion) <= 65f)
                            {
                                horizontalFactor = 2.0f;
                                repeticion = 65;// (int)(1.542f * Mathf.Pow(heartRate * 4f / 3f - 1 / 3f, 4) - 23.42f * Mathf.Pow(heartRate * 4f / 3f - 1 / 3f, 3) + 136f * Mathf.Pow(heartRate * 4f / 3f - 1 / 3f, 2) + -379.1f * (heartRate * 4f / 3f - 1 / 3f) + 525f);
                                float valor = heartRate / 0.75f - 1f / 3f;
                                currentValue = Mathf.Cos((indexScan * heartRate / (4f * incrIndexScan / 7f) % repeticion) * horizontalFactor * Mathf.PI / 180f - 56.5f + (-0.025f * valor * valor * valor * valor + 0.3f * valor * valor * valor - 1.225f * valor * valor + 1.85f * valor + 6.5f));//0.35f * (4f - heartRate));// + pathology / 30f);
                                currentValue += 0.4f;
                            }
                        }
                    }
                }
                currentValue *= 0.5f;
                //currentValue = Mathf.Cos ((indexScan * heartRate / (4f * incrIndexScan / 7f) % repeticion) * horizontalFactor * Mathf.PI / 180f - ((indexScan * heartRate / (4f * incrIndexScan / 7f) % repeticion < 8) ? 5f : 45f));// + pathology / 30f);
                //lo parada de la grafica
                currentValue *= 50f - pathology * 0.6f;
                //elevado del ultimo tramo
                currentValue += 25f + pathology * 0.3f;
            }
            if (SceneManager.GetActiveScene().name == "EcografiaUtero")
            {
                if (sliderheart.value > 0.5f)
                {
                    if (cursorController.umbilicalDraw)
                    {
                        int repeticion = 65;
                        //ORIGINAL
                        if ((indexScan * heartRate / (4f * incrIndexScan / 7f) % repeticion) < 9f)
                        {
                            horizontalFactor = 20f + pathology / 9f;
                            currentValue = Mathf.Cos((indexScan * heartRate / (4f * incrIndexScan / 7f) % repeticion) * horizontalFactor * Mathf.PI / 180f + ((indexScan * heartRate / (4f * incrIndexScan / 7f) % repeticion < 8) ? 35f : 45f));// + pathology / 30f);
                            currentValue *= 1.1f - (0.3f) ;
                            currentValue += 0.45f + pathology / 100f;
                        }
                        else
                        {
                            //ORIGINAL
                            //repeticion = 65 - (int)(10f * testSlider.value) * 2;
                            //print(sliderheart.value);
                            repeticion = 130 + (int)(60f * (0.025f * (sliderheart.value * 8f - 4f) * (sliderheart.value * 8f - 4f) + 0.045f * (sliderheart.value * 8f - 4f) - 0.075f)) + (int)((4f * pathology) / 30f) - (int)(200f * (0.1f * (sliderheart.value * 8f - 4f) - 0.1f) + testSlider.value * 10f);//146;// (int)(1.542f * Mathf.Pow(heartRate / 1.765f * 4f * 4f / 3f - 1 / 3f, 4) - 23.42f * Mathf.Pow(heartRate / 1.765f * 4f * 4f / 3f - 1 / 3f, 3) + 136f * Mathf.Pow(heartRate / 1.765f * 4f * 4f / 3f - 1 / 3f, 2) + -379.1f * (heartRate / 1.765f * 4f * 4f / 3f - 1 / 3f) + 525f);
                            if (sliderheart.value >= 0.875f)
                            {
                                if (sliderheart.value >= 1f)
                                {
                                    repeticion -= (int)(sliderPathology.value * 5f);
                                }
                                else
                                {
                                    repeticion -= (int)(sliderPathology.value * 2.5f);
                                }
                            }
                            horizontalFactor = 6f - (3f * (0f + 2f * 0.5f)) + 2f;// + pathology / 7.5f - 3.5f * (0.1333f * (heartRate / 0.75f - 1f / 3f) - 0.1333f) - 0.6f * 10f;

                            if ((indexScan * heartRate / (4f * incrIndexScan / 7f) % 65) < 27f - 4f)
                            {
                                currentValue = Mathf.Cos((indexScan * 4f / (4f * incrIndexScan / 7f) % repeticion) * horizontalFactor * Mathf.PI / 180f - ((indexScan * 4f / (4f * incrIndexScan / 7f) % repeticion < 8) ? 5f : 45f));// + pathology / 30f);
                                currentValue += 0.65f;
                            }
                            else
                            {
                                float var1 = 0f;
                                float var2 = 0f;
                                float var3 = 0f;

                                float var4 = 0f;
                                float var5 = 0f;
                                switch (pathology)
                                {
                                    case 0: var1 = 0f; var2 = 1f; var3 = 0.5f; break;
                                    case 3: var1 = -5f; var2 = 1f; var3 = -0.6f; var4 = 0.1f; var5 = 5f; break;
                                    case 7: var1 = -15f; var2 = 1f; var3 = 0f; var4 = 0.3f; var5 = 0f; break;
                                    case 11: var1 = -15f; var2 = 1f; var3 = 0f; var4 = 0.4f; var5 = 0f; break;//testSlider.value * 10f; 
                                    case 15: var1 = -15f; var2 = 1f; var3 = 0f; var4 = 0.4f; var5 = 0f; break;
                                    case 18: var1 = -15f; var2 = 1f; var3 = 0f; var4 = 0.4f; var5 = 0f; break;
                                    case 22: var1 = -15f; var2 = 1f; var3 = 0f; var4 = 0.4f; var5 = 0f; break;
                                    case 26: var1 = -15f; var2 = 1f; var3 = 0f; var4 = 0.4f; var5 = 0f; break;
                                    case 30: var1 = -15f; var2 = 1f; var3 = 0f; var4 = 0.4f; var5 = 0f; break;
                                }
                                horizontalFactor = 3f - var1 / 1.87f - pathology * 0.1f - 0.3f - var5;
                                repeticion = 130 - (int)(80f + 15f + 5f * 0.4f - 30f - var2);
                                //ORIGINAL
                                //if ((indexScan * heartRate / (4f * incrIndexScan / 7f) % repeticion) <= 65f)
                                //{
                                //print(var1 * (sliderheart.value) + var2);
                                currentValue = Mathf.Cos((indexScan * heartRate / (4f * incrIndexScan / 7f) % repeticion) * horizontalFactor * Mathf.PI / 180f - 56.5f + (0.35f * (4f - heartRate)));// - 0.5f + pathology * 0.05f));// + pathology / 30f);
                                currentValue *= 1f - (var4 * 3f);// - var2 / 27f;
                                currentValue += 0.9f- (-0.8f * sliderheart.value + 1.73f) + var3;

                                //}

                            }
                        }
                        currentValue *= 0.4f;
                        //lo parada de la grafica
                        currentValue *= 50f + pathology * 2f;
                        //elevado del ultimo tramo
                        currentValue += 65f - pathology * 0.3f;
                    }
                    else
                    {
                        int repeticion = 65;
                        if ((indexScan * heartRate / (4f * incrIndexScan / 7f) % repeticion) < 9f)
                        {
                            horizontalFactor = 20f + 30f / 9f;
                            currentValue = Mathf.Cos((indexScan * heartRate / (4f * incrIndexScan / 7f) % repeticion) * horizontalFactor * Mathf.PI / 180f + ((indexScan * heartRate / (4f * incrIndexScan / 7f) % repeticion < 8) ? 35f : 45f));// + pathology / 30f);
                            currentValue *= 0.7f;
                            currentValue += 0.45f + 30f / 100f;
                        }
                        else
                        {
                            horizontalFactor = 6f + 30f / 7.5f;
                            if ((indexScan * heartRate / (4f * incrIndexScan / 7f) % repeticion) < 28f - 30f / 4f)
                            {
                                currentValue = Mathf.Cos((indexScan * heartRate / (4f * incrIndexScan / 7f) % repeticion) * horizontalFactor * Mathf.PI / 180f - ((indexScan * heartRate / (4f * incrIndexScan / 7f) % repeticion < 8) ? 5f : 45f));// + pathology / 30f);
                                currentValue += 0.65f;
                            }
                            else
                            {
                                horizontalFactor = 3f - 30f / 2.3f - 0.5f - 0.9f;
                                if ((indexScan * heartRate / (4f * incrIndexScan / 7f) % repeticion) <= 34f)
                                {
                                    currentValue = Mathf.Cos((indexScan * heartRate / (4f * incrIndexScan / 7f) % repeticion) * horizontalFactor * Mathf.PI / 180f - 56.5f  - 4f);// + pathology / 30f);
                                    currentValue *= 1f - 30f / 24f + 0.7f + 0.3f;
                                    currentValue += 0.4f - 30f / 42f;

                                }
                                else
                                {

                                    horizontalFactor = 4f - 30f / 2.3f -0.7f + 2.0f;
                                    if ((indexScan * heartRate / (4f * incrIndexScan / 7f) % repeticion) <= 65f)
                                    {
                                        currentValue = Mathf.Cos((indexScan * heartRate / (4f * incrIndexScan / 7f) % repeticion) * horizontalFactor * Mathf.PI / 180f - 56.5f - 1f - 2f);// + pathology / 30f);
                                        currentValue *= 1f - 30f / 24f;
                                        currentValue += 0.4f - 30f / 42f;

                                    }
                                }
                            }
                        }
                        currentValue *= 0.4f;
                        //lo parada de la grafica
                        currentValue *= 50f + 30f * 2f;
                        //elevado del ultimo tramo
                        currentValue += 25f - 30f * 0.3f;
                    }
                }
                else
                {
                    if (cursorController.umbilicalDraw)
                    {
                        int repeticion = 65;
                        if ((indexScan * heartRate / (4f * incrIndexScan / 7f) % repeticion) < heartRate / 1.765f * 4f * 7f / 3f - 1f / 3f)
                        {
                            horizontalFactor = 22f + pathology / 4f - sliderPathology.value * 0.8f * 20f;
                            repeticion = (int)(1.542f * Mathf.Pow(heartRate / 1.765f * 4f * 4f / 3f - 1 / 3f, 4) - 23.42f * Mathf.Pow(heartRate / 1.765f * 4f * 4f / 3f - 1 / 3f, 3) + 136f * Mathf.Pow(heartRate / 1.765f * 4f * 4f / 3f - 1 / 3f, 2) + -379.1f * (heartRate / 1.765f * 4f * 4f / 3f - 1 / 3f) + 525f);
                            currentValue = Mathf.Cos((indexScan * 1.765f / (4f * incrIndexScan / 7f) % repeticion) * horizontalFactor * Mathf.PI / 180f + ((indexScan * 1.765f / (4f * incrIndexScan / 7f) % repeticion < 8) ? 35f : 45f) - (1f - pathology / 20f));
                            currentValue *= 1.0f;
                            currentValue += 0.6f + pathology / 100f;
                        }
                        else
                        {
                            
                            if ((indexScan * heartRate / (4f * incrIndexScan / 7f) % repeticion) < heartRate / 1.765f * 4f * 7f + 9f - (9f - heartRate / 1.765f * 4f * 7f / 3f - 1f / 3f + 4f))// - pathology * 0.73333f)
                            {
                                float var1 = 0f;
                                float var2 = 0f;
                                float var3 = 0f;
                                switch (pathology)
                                {
                                    case 0: var1 = -2.366f; var2 = 6.976f; var3 = 0f; break;
                                    case 3: var1 = -3.55f; var2 = 9.266f; var3 = 20f; break;
                                    case 7: var1 = -2.958f; var2 = 8.721f; var3 = 18f; break;
                                    case 11: var1 = -3.313f; var2 = 9.048f; var3 = 18f; break;
                                    case 15: var1 = -2.485f; var2 = 7.386f; var3 = 18f; break;
                                    case 18: var1 = -2.485f; var2 = 7.386f; var3 = 18f; break;
                                    case 22: var1 = -2.603f; var2 = 7.395f; var3 = 18f; break;
                                    case 26: var1 = -2.721f; var2 = 7.504f; var3 = 18f; break;
                                    case 30: var1 = -3.431f; var2 = 8.757f; var3 = 26f; break;
                                }
                                repeticion = 142 + (int)((4f * pathology) / 30f);//146;// (int)(1.542f * Mathf.Pow(heartRate / 1.765f * 4f * 4f / 3f - 1 / 3f, 4) - 23.42f * Mathf.Pow(heartRate / 1.765f * 4f * 4f / 3f - 1 / 3f, 3) + 136f * Mathf.Pow(heartRate / 1.765f * 4f * 4f / 3f - 1 / 3f, 2) + -379.1f * (heartRate / 1.765f * 4f * 4f / 3f - 1 / 3f) + 525f);
                                //print(heartRate + " " + pathology);
                                horizontalFactor = var1 * heartRate + var2 + 0.5f;//heartRate * -2.958f + 8.2218f + (-7.1f * heartRate + 0.684f * pathology);//pathology / 4f;// pathology / 7.5f;// 6f + pathology / 7.5f;
                                currentValue = Mathf.Cos((indexScan * heartRate / 1.765f * 4f / (4f * incrIndexScan / 7f) % repeticion) * horizontalFactor * Mathf.PI / 180f - ((indexScan * heartRate / 1.765f * 4f / (4f * incrIndexScan / 7f) % repeticion < 8) ? 5f : 45f) - (0.9f + ((-0.5f * pathology) / 30f)));
                                currentValue += 0.85f;
                            }
                            else
                            {
                                float var1 = 0f;
                                float var2 = 0f;
                                float var3 = 0f;
                                switch (pathology)
                                {
                                    case 0: var1 = 0f; var2 = 0f; var3 = 0f; break;
                                    case 3: var1 = -5f; var2 = 32f; var3 = 35f; break;
                                    case 7: var1 = -15f; var2 = 29f; var3 = 25f; break;
                                    case 11: var1 = -15f; var2 = 30f; var3 = 28f; break;
                                    case 15: var1 = -15f; var2 = 30f; var3 = 18f; break;
                                    case 18: var1 = -15f; var2 = 30f; var3 = 18f; break;
                                    case 22: var1 = -16f; var2 = 30f; var3 = 18f; break;
                                    case 26: var1 = -18f; var2 = 30f; var3 = 18f; break;
                                    case 30: var1 = -20f; var2 = 31f; var3 = 26f; break;
                                }
                                horizontalFactor = 2.5f - var1 / 1.87f - pathology * 0.1f;
                                if ((indexScan * heartRate / (4f * incrIndexScan / 7f) % repeticion) <= 65f)
                                {
                                    //horizontalFactor = 2.5f;
                                    //repeticion = 65;// (int)(1.542f * Mathf.Pow(heartRate * 4f / 3f - 1 / 3f, 4) - 23.42f * Mathf.Pow(heartRate * 4f / 3f - 1 / 3f, 3) + 136f * Mathf.Pow(heartRate * 4f / 3f - 1 / 3f, 2) + -379.1f * (heartRate * 4f / 3f - 1 / 3f) + 525f);
                                    currentValue = Mathf.Cos((indexScan * heartRate / (4f * incrIndexScan / 7f) % repeticion) * horizontalFactor * Mathf.PI / 180f - 56.5f + 0.35f * (4f - heartRate) - 0.5f + pathology * 0.05f);// + pathology / 30f);
                                    currentValue *= 1f - var2 / 27f;
                                    currentValue += 0.4f -  var3 / 50f;
                                }
                            }
                        }
                        currentValue *= 0.4f;
                        //lo parada de la grafica
                        currentValue *= 50f + pathology * 2f;
                        //elevado del ultimo tramo
                        currentValue += 65f - pathology * 0.3f;
                    }
                    else
                    {
                        int repeticion = 65;
                        if ((indexScan * heartRate / (4f * incrIndexScan / 7f) % repeticion) < 9f)
                        {
                            horizontalFactor = 20f + 30f / 9f;
                            currentValue = Mathf.Cos((indexScan * heartRate / (4f * incrIndexScan / 7f) % repeticion) * horizontalFactor * Mathf.PI / 180f + ((indexScan * heartRate / (4f * incrIndexScan / 7f) % repeticion < 8) ? 35f : 45f));// + pathology / 30f);
                            currentValue *= 0.7f;
                            currentValue += 0.45f + 30f / 100f;
                        }
                        else
                        {
                            horizontalFactor = 6f + 30f / 7.5f;
                            if ((indexScan * heartRate / (4f * incrIndexScan / 7f) % repeticion) < 28f - 30f / 4f)
                            {
                                currentValue = Mathf.Cos((indexScan * heartRate / (4f * incrIndexScan / 7f) % repeticion) * horizontalFactor * Mathf.PI / 180f - ((indexScan * heartRate / (4f * incrIndexScan / 7f) % repeticion < 8) ? 5f : 45f));// + pathology / 30f);
                                currentValue += 0.65f;
                            }
                            else
                            {
                                horizontalFactor = 3f - 30f / 2.3f - 0.5f - 0.9f;
                                if ((indexScan * heartRate / (4f * incrIndexScan / 7f) % repeticion) <= 34f)
                                {
                                    currentValue = Mathf.Cos((indexScan * heartRate / (4f * incrIndexScan / 7f) % repeticion) * horizontalFactor * Mathf.PI / 180f - 56.5f - 4f);// + pathology / 30f);
                                    currentValue *= 1f - 30f / 24f + 0.7f + 0.3f;
                                    currentValue += 0.4f - 30f / 42f;

                                }
                                else
                                {

                                    horizontalFactor = 4f - 30f / 2.3f - 0.7f + 2.0f;
                                    if ((indexScan * heartRate / (4f * incrIndexScan / 7f) % repeticion) <= 65f)
                                    {
                                        currentValue = Mathf.Cos((indexScan * heartRate / (4f * incrIndexScan / 7f) % repeticion) * horizontalFactor * Mathf.PI / 180f - 56.5f - 1f - 2f);// + pathology / 30f);
                                        currentValue *= 1f - 30f / 24f;
                                        currentValue += 0.4f - 30f / 42f;

                                    }
                                }
                            }
                        }
                        currentValue *= 0.4f;
                        //lo parada de la grafica
                        currentValue *= 50f + 30f * 2f;
                        //elevado del ultimo tramo
                        currentValue += 25f - 30f * 0.3f;

                    }
                }
            }
            //currentValue = Mathf.Cos((indexScan % 60) * 4.5f * Mathf.PI / 180f - 19f) * 20f + 40; FUNCIONA!
            //currentValue = (Mathf.Cos(horizontalFactor * ((indexScan * heartRate / 4) % 67) * Mathf.PI / 180f) * 60f) + 20;
            /*float period = 200f;
            float amplitude = 50f;
            currentValue = (2 * amplitude / Mathf.PI) * Mathf.Atan(1f / Mathf.Tan(indexScan * heartRate * horizontalFactor * Mathf.PI / period)) + 50 + (Mathf.Cos(horizontalFactor * (indexScan % 67) * heartRate * Mathf.PI / 180f) * 30f);
            */
            if (SceneManager.GetActiveScene().name != "EcografiaCerebral")
            {
                if ((SceneManager.GetActiveScene().name == "EcografiaUtero" && cursorController.umbilicalDraw))
                {
                    currentValue -= pathology * 1f;
                    //print("fsafasf");
                }
            }
            currentValue *= (inverseFunction) ? -1 : 1;
            //currentValue *= ((inverseGraph)) ? -1 : 1;
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
			if (Mathf.Abs(currentValue) < WFMLevel * 10f) currentValue = 0f;

			if (cursorController.inVein)
            {
                if (!inverseFunction)
					currentValue = Random.Range (0, 30f);
				else
					currentValue = Random.Range (-30, 0f);
                //print(positiveFunction);

                texture.SetPixel (indexScan % sizeHorizontal, (int)((currentValue * verticalScale + zero)), Color.white);
                if(lineRenderer != null && useLineRenderer) DrawBorder(indexScan % sizeHorizontal, (int)((currentValue * verticalScale + zero)));
				//positionDots.Add(new Vector2(indexScan % sizeHorizontal + incrIndexScan, (int)((currentValue * verticalScale + zero))));
				//positionDots [cicloDots] = new Vector3(indexScan % sizeHorizontal, i, (int)((currentValue * verticalScale + zero)));
			} else {
				texture.SetPixel (indexScan % sizeHorizontal, (int)((currentValue * verticalScale * cursorController.durezasAngle + zero)), Color.white);
                if(lineRenderer != null && useLineRenderer) DrawBorder(indexScan % sizeHorizontal, (int)((currentValue * verticalScale * cursorController.durezasAngle + zero)));
				//texture.SetPixel ((indexScan + 1) % sizeHorizontal, (int)((currentValue * verticalScale * cursorController.durezasAngle + zero)), Color.white);
				//texture.SetPixel ((indexScan + 2) % sizeHorizontal, (int)((currentValue * verticalScale * cursorController.durezasAngle + zero)), Color.white);
				//print (cicloDots + ": " + indexScan % sizeHorizontal + ", " + i);
				//positionDots.Add(new Vector2(indexScan % sizeHorizontal + incrIndexScan, (int)((currentValue * verticalScale * cursorController.durezasAngle + zero))));
				//positionDots [cicloDots] = new Vector3(indexScan % sizeHorizontal, i, (int)((currentValue * verticalScale * cursorController.durezasAngle + zero)));
			}
			cicloDots++;
			if (cicloDots == sizeHorizontal * (5 + (int)((gain + power) / 2)))
				cicloDots = 0;

            if(i == 0)audioSource.pitch = currentValue / 50f;
            quadPoint.x = i;
            quadPoint.y = currentValue;
		}
        texture.Apply();
    }

    public Vector2 quadPoint;

    float nextDrawTime = 0f;
    public float nextSampleTime = 0.1f;
    public GameObject lrAux;
    public LineRenderer lineRenderer;
    private List<Vector3> lrPoints;

    void DrawBorder(float x, float y){
        if(Time.time > nextDrawTime){
            nextDrawTime = Time.time + nextSampleTime;
            lrAux.transform.localPosition = new Vector2(-159 + x, -131 + y);
            lrAux.transform.position = new Vector3(lrAux.transform.position.x, lrAux.transform.position.y, -9);
            lrPoints.Add(lrAux.transform.position);
        }
    }

    public Vector2 WorldSpaceToQuadSpace(Vector3 point){
        GameObject go = (GameObject)Instantiate(lrAux,point,Quaternion.identity);
        go.transform.position = point;
        go.transform.parent = transform.parent;
        Vector2 aux = go.transform.localPosition;
        Destroy(go);
        return new Vector2(aux.x + 159, (aux.y * (0.8f))/verticalScale + 5f - (zero - 128f));
    }

    public Vector3 QuadSpaceToWorldSpace(Vector2 point){
        GameObject go = (GameObject)Instantiate(new GameObject(),point,Quaternion.identity);
        Vector2 aux = go.transform.localPosition;
        return new Vector2(aux.x, aux.y);
    }

    void drawNumbersHorizontal()
    {
        for (int i = 0; i < horizontalNumbers.Count; i++)
        {
            Destroy((GameObject)horizontalNumbers[i]);
        }
        horizontalNumbers.Clear();

        int aux = Mathf.FloorToInt(sizeHorizontal / (pixelsPerSecond()/2));

        int count = 0;
        for (int i = 0; i < aux; i++)
        {
            GameObject g = Instantiate(horizontalNumberPrefab);
            g.transform.parent = transform.parent;
            g.transform.localScale = new Vector3(1f, 1f, 1f);
            //g.transform.localPosition = new Vector2(g.transform.localPosition.x + i * 20f - 65f, -140f);
            g.transform.localPosition = new Vector2(g.transform.localPosition.x + (i+1)*(pixelsPerSecond()/2) - 65f, -140f);
            if(i % 2 == 0){
                g.GetComponent<UILabel>().text = "'";
            }
            else{
                count++;
                g.GetComponent<UILabel>().text = count.ToString();
            }
            g.transform.parent = transform.Find("Horizontal");
            horizontalNumbers.Add(g);
        }
    }


    ArrayList verticalNumbers = new ArrayList(); 
    void drawNumbersVertical(){
        for (int i = 0; i < verticalNumbers.Count; i++)
        {
            Destroy((GameObject)verticalNumbers[i]);
        }
        verticalNumbers.Clear();

        float height = GetWaveHeight(sliderScale.value);

        int step = Mathf.RoundToInt(height/2 / 5);
        int reps = Mathf.RoundToInt(sizeVertical/step);
        int weight = 20;

        //Debug.Log(step);

        if(step < 20){
            step = Mathf.RoundToInt(height/2 / 2);
            reps = Mathf.RoundToInt(sizeVertical/step);
            weight = 50;

            if(step < 20){
                step = Mathf.RoundToInt(height/2);
                reps = Mathf.RoundToInt(sizeVertical/step);
                weight = 100;
            }
        }

        for(int i = 0; i < reps;i++){
            if((i*step + zero - sizeVertical/2 - 6f) > sizeVertical/2 - 5)
                return;

            GameObject g = Instantiate(horizontalNumberPrefab);
            g.transform.parent = transform.parent;
            g.transform.localScale = new Vector3(1f, 1f, 1f);
            g.transform.localPosition = new Vector2(g.transform.localPosition.x - 80f, i*step + zero - sizeVertical/2 - 6f);
            g.GetComponent<UILabel>().text = (weight * i).ToString();
            g.transform.parent = transform.Find("Vertical");
            verticalNumbers.Add(g);
            if(i != 0){
                g = Instantiate(horizontalNumberPrefab);
                g.transform.parent = transform.parent;
                g.transform.localScale = new Vector3(1f, 1f, 1f);
                g.transform.localPosition = new Vector2(g.transform.localPosition.x - 80f, -i*step + zero - sizeVertical/2 - 6f);
                g.GetComponent<UILabel>().text = (weight * i).ToString();
                g.transform.parent = transform.Find("Vertical");
                verticalNumbers.Add(g);
            }
        }
    }

    float GetWaveHeight(float vScale){
        //float comp = (SceneManager.GetActiveScene().name == "EcografiaUtero" ? 1.43f : 1f);
        float comp = 1f;
        string sceneName = SceneManager.GetActiveScene().name;
        switch(sceneName){
            case "EcografiaUtero":
            comp = 1.43f *2;
            break;
            case "EcografiaCerebral":
            comp = 2f;
            break;
            case "EcografiaUmbilical":
            comp = 1.9f;
            break;
            case "EcografiaDuctus":
            if(cursorController.umbilicalDraw)
                comp = 2.2f;
            else
                comp = 2.8f;
            break;
            default:
            comp = 1;
            break;
        }


        if(vScale == 0f)
            return 141.4992f * 0.26f * comp;
        if(vScale == 0.1f)
            return 146.431f * 0.38f * comp;
        if(vScale == 0.2f)
            return 151.852f * 0.48f * comp;
        if(vScale == 0.3f)
            return 157.7186f * 0.6f * comp;
        if(vScale == 0.4f)
            return 164.0001f * 0.72f * comp;
        if(vScale == 0.5f)
            return 197.7435f * 0.7f * comp;
        if(vScale == 0.6f)
            return 219.6802f * 0.8f * comp;
        if(vScale == 0.7f)
            return 243.5302f * 0.97f * comp;
        if(vScale == 0.8f)
            return 269.1509f * comp;
        if(vScale == 0.9f)
            return 296.4272f * 1.05f * comp;
        if(vScale == 1f)
            return 325.2643f * 1.1f * comp;
        return 0f;
    }

    public void adjustTimeFixedScale()
    {
        StartCoroutine(adjustTimeFixedScaleCoroutine());
    }
    IEnumerator adjustTimeFixedScaleCoroutine() {
        yield return new WaitForFixedUpdate();
        float value = 0f;
        /*if (SceneManager.GetActiveScene().name == "EcografiaUtero")
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
                                //print(sliderheart.value);
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
        {*/
            //if (sliderheart.value >= 0.5f)
            //{
			//value = 0.00025f;
			value = 0.0025f;
            /*}
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
                
            }*/
        //}
		//Time.fixedDeltaTime = 0.8f * deltaTime - Time.timeScale * value * 100f * deltaTime;
		Time.fixedDeltaTime = Time.timeScale * (0.0155f - (sliderSpeed.value - 0.5f) * ((1.25f - sliderSpeed.value) * 0.06f)); //0.075f 0.015f 
		//print(Time.fixedDeltaTime + " " + (0.8f * deltaTime - Time.timeScale * value * 1000f * deltaTime));// * 100f * deltaTime);
    }

    public void zeroAdjust()
    {
        labelIndicators.resetZero();
        labelIndicators.adjustZero(Mathf.RoundToInt((sliderZero.value - 0.5f) * 10));
    }

    float velocidadBarrido(float timeScale){
        return (166.97f*Mathf.Pow(timeScale,2) - 367.068f*timeScale + 203.0671f);
    }

    int pixelsPerBeat(){
        string [] s = beatsPerMinute.text.Split(' ');
        float spb = 1f / (float.Parse(s[0]) / 60f);
        //Debug.Log(sizeHorizontal+"*"+spb+"/"+velocidadBarrido(Time.timeScale)+";"+Time.timeScale);
        return (int)(sizeHorizontal*spb/velocidadBarrido(Time.timeScale));
    }

    int pixelsPerSecond(){
        //Debug.Log(velocidadBarrido(Time.timeScale));
        float comp = 1f;
        if(sliderheart.value == 0f){
            if(SceneManager.GetActiveScene ().name == "EcografiaUtero")
                comp = 2.05f;
            else
                comp = 2.05f;

        }
        if(sliderheart.value == 0.125f){
            if(SceneManager.GetActiveScene ().name == "EcografiaUtero")
                comp = 1.72f;
            else
                comp = 1.25f;
        } 
        if(sliderheart.value == 0.250f){
            if(SceneManager.GetActiveScene ().name == "EcografiaUtero")
                comp = 1.52f;
            else
                comp = 1.32f;
        }
        if(sliderheart.value == 0.375f){
            if(SceneManager.GetActiveScene ().name == "EcografiaUtero")
                comp = 1.32f;
            else
                comp = 1.12f;
        }
        if(sliderheart.value == 0.5f){
            if(SceneManager.GetActiveScene ().name == "EcografiaUtero")
                comp = 1.22f;
            else
                comp = 1f;
        }      
        if(sliderheart.value == 0.625f){
            if(SceneManager.GetActiveScene ().name == "EcografiaUtero")
                comp = 1.08f;
            else
                comp = 0.95f;
        }
        if(sliderheart.value == 0.750f){
            if(SceneManager.GetActiveScene ().name == "EcografiaUtero")
                comp = 1f;
            else
                comp = 0.85f;
        }
        if(sliderheart.value == 0.875f){
            if(SceneManager.GetActiveScene ().name == "EcografiaUtero")
                comp = 0.95f;
            else
                comp = 0.9f;
        }
        if(sliderheart.value == 1f){
            if(SceneManager.GetActiveScene ().name == "EcografiaUtero")
                comp = 0.9f;
            else
                comp = 0.82f;
        }

        return (int)(sizeHorizontal*comp/getVelocidadBarrido(sliderSpeed.value));
    }

    // Update is called once per frame
    void FixedUpdate() {

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene("Login");
        }
        if (sleep) return;

        //Debug.Log(Time.timeScale);

        //slider speed
        incrIndexScan = ((sliderSpeed.value / 0.5f * 2.5f) + 1) * 2;
        //slider scale
        verticalScale = (Mathf.Pow(sliderScale.value + ((sliderScale.value < 0.5f)?(0.2f * (1f - sliderScale.value * 2f)):0f), 1.5f) / (0.35355339059f));
        labelIndicators.adjustVerticalSize(sliderScale.value);
        //slider zero
        zero = Mathf.CeilToInt((sliderZero.value - 0.5f) * 140) + sizeVertical / 2;
        //slider gain
        gain = (int)(sliderGain.value * 10);
        //slider power
        power = (int)(sliderPower.value * 10);
        //slider heartrate
        heartRate = ((sliderheart.value + 0.5f) * 6f - 2f);
        beatsPerMinute.text = "" + (((sliderheart.value) * 100) + 70) + " bpm";
		//print (Time.timeScale + " " + Time.fixedDeltaTime + " " + Time.deltaTime);
        //if (sliderheart.value >= 0.5f)
        //{
		if (SceneManager.GetActiveScene ().name == "EcografiaUtero") {
            heartRate = (sliderheart.value * 1.3f - 1.6f) * 1.3f + 3f;
			beatsPerMinute.text = "" + (((sliderheart.value) * 10) + 60) + " bpm";
            //Time.timeScale = ((sliderheart.value + 0.5f) * (sliderheart.value * 0.3f + 0.2f) + 0.15f); //* 0.2f, * 0.35f, * 0.5f
            Time.timeScale = (sliderheart.value + 0.5f) * (1f - ((sliderheart.value * -2f + 1f)) * 0.7f);

            Time.timeScale += 0.25f - getFactorTimeScale(sliderSpeed.value, sliderheart.value, true);
        }
		else{
			//Time.timeScale = ((sliderheart.value + 0.5f) * (1f - (1f - (sliderheart.value + 0.5f)) * 0.7f) + 0.25f - (1f - (sliderheart.value + 0.5f)) * 0.15f); //0.25f
			Time.timeScale = (sliderheart.value + 0.5f) * (1f - ((sliderheart.value * - 2f + 1f)) * 0.7f);

            Time.timeScale += 0.25f - getFactorTimeScale(sliderSpeed.value, sliderheart.value, false);//(((sliderheart.value - 1f)) * -0.5f); //[0.13f] 0.15f 0f
            //print(getFactorTimeScale(sliderSpeed.value, sliderheart.value));
			//print(((sliderheart.value + 0.5f) * (1f - ((sliderheart.value * - 2f + 1f)) * 0.7f) + 0.25f - (1f - (sliderheart.value * 2f)) * 0.15f) + " " + (((sliderheart.value + 0.5f) * (0.3f)) + 0.1f));
		}
			/*}
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
        }*/
        //print(0.8f * Time.deltaTime - Time.timeScale * 0.001f);
        //Time.fixedDeltaTime = 0.8f * Time.deltaTime;
        /*if (SceneManager.GetActiveScene().name == "EcografiaUtero")
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
        }*/
        
        //Time.timeScale = sliderheart.value * 1.5f + 0.3f;
        //incrIndexScan *= heartRate;
        //slider pathology
        pathology = (int)(sliderPathology.value * 30);

		ciclo++;
		//if (ciclo % 2 == 0) {
			//paintLineVerticalBlack(indexScan % sizeHorizontal + incrIndexScan);
			paintLineVerticalBlack(indexScan % sizeHorizontal);
			drawFunction ();
		//}

		if (ciclo % 100 == 0) {
			drawNumbersHorizontal ();
            drawNumbersVertical();
		}

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

    float getFactorTimeScale(float speed, float heart, bool utero)
    {
        if (utero)
        {
            if (speed == 0f)
            {
                if (heart == 0f) return 0.17f;
                if (heart == 0.125f) return 0.26f;
                if (heart == 0.25f) return 0.4f;
                if (heart == 0.375f) return 0.57f;
                if (heart == 0.5f) return 0.8f;
                if (heart == 0.625f) return 1.05f;
                if (heart == 0.75f) return 1.38f;
                if (heart == 0.875f) return 1.73f;
                if (heart == 1f) return 2.12f;
            }
            if (speed == 0.1f)
            {
                if (heart == 0f) return 0.22f;
                if (heart == 0.125f) return 0.31f;
                if (heart == 0.25f) return 0.46f;
                if (heart == 0.375f) return 0.65f;
                if (heart == 0.5f) return 0.88f;
                if (heart == 0.625f) return 1.15f;
                if (heart == 0.75f) return 1.48f;
                if (heart == 0.875f) return 1.63f;
                if (heart == 1f) return 2.25f;
            }
            if (speed == 0.2f)
            {
                if (heart == 0f) return 0.23f;
                if (heart == 0.125f) return 0.33f;
                if (heart == 0.25f) return 0.49f;
                if (heart == 0.375f) return 0.68f;
                if (heart == 0.5f) return 0.92f;
                if (heart == 0.625f) return 1.19f;
                if (heart == 0.75f) return 1.52f;
                if (heart == 0.875f) return 1.87f;
                if (heart == 1f) return 2.25f;
            }
            if (speed == 0.3f)
            {
                if (heart == 0f) return 0.24f;
                if (heart == 0.125f) return 0.33f;
                if (heart == 0.25f) return 0.48f;
                if (heart == 0.375f) return 0.68f;
                if (heart == 0.5f) return 0.91f;
                if (heart == 0.625f) return 1.19f;
                if (heart == 0.75f) return 1.5f;
                if (heart == 0.875f) return 1.87f;
                if (heart == 1f) return 2.28f;
            }
            if (speed == 0.4f)
            {
                if (heart == 0f) return 0.22f;
                if (heart == 0.125f) return 0.31f;
                if (heart == 0.25f) return 0.46f;
                if (heart == 0.375f) return 0.64f;
                if (heart == 0.5f) return 0.88f;
                if (heart == 0.625f) return 1.16f;
                if (heart == 0.75f) return 1.47f;
                if (heart == 0.875f) return 1.85f;
                if (heart == 1f) return 2.25f;
            }
            if (speed == 0.5f)
            {
                if (heart == 0f) return 0.19f;
                if (heart == 0.125f) return 0.28f;
                if (heart == 0.25f) return 0.44f;
                if (heart == 0.375f) return 0.61f;
                if (heart == 0.5f) return 0.84f;
                if (heart == 0.625f) return 1.11f;
                if (heart == 0.75f) return 1.42f;
                if (heart == 0.875f) return 1.78f;
                if (heart == 1f) return 2.17f;
            }
            if (speed == 0.6f)
            {
                if (heart == 0f) return 0.15f;
                if (heart == 0.125f) return 0.24f;
                if (heart == 0.25f) return 0.28f;
                if (heart == 0.375f) return 0.56f;
                if (heart == 0.5f) return 0.76f;
                if (heart == 0.625f) return 1f;
                if (heart == 0.75f) return 1.35f;
                if (heart == 0.875f) return 1.68f;
                if (heart == 1f) return 2.05f;
            }
            if (speed == 0.7f)
            {
                if (heart == 0f) return 0.11f;
                if (heart == 0.125f) return 0.18f;
                if (heart == 0.25f) return 0.3f;
                if (heart == 0.375f) return 0.48f;
                if (heart == 0.5f) return 0.65f;
                if (heart == 0.625f) return 0.84f;
                if (heart == 0.75f) return 1.2f;
                if (heart == 0.875f) return 1.5f;
                if (heart == 1f) return 1.9f;
            }
            if (speed == 0.8f)
            {
                if (heart == 0f) return 0.04f;
                if (heart == 0.125f) return 0.1f;
                if (heart == 0.25f) return 0.23f;
                if (heart == 0.375f) return 0.4f;
                if (heart == 0.5f) return 0.5f;
                if (heart == 0.625f) return 0.78f;
                if (heart == 0.75f) return 0.1f;
                if (heart == 0.875f) return 1.5f;
                if (heart == 1f) return 1.7f;
            }
            if (speed == 0.9f)
            {
                if (heart == 0f) return 0.02f;
                if (heart == 0.125f) return 0.1f;
                if (heart == 0.25f) return 0.23f;
                if (heart == 0.375f) return 0.4f;
                if (heart == 0.5f) return 0.7f;
                if (heart == 0.625f) return 0.9f;
                if (heart == 0.75f) return 1f;
                if (heart == 0.875f) return 1.55f;
                if (heart == 1f) return 1.8f;
            }
            if (speed == 1f)
            {
                if (heart == 0f) return 0.08f;
                if (heart == 0.125f) return 0.2f;
                if (heart == 0.25f) return 0.35f;
                if (heart == 0.375f) return 0.5f;
                if (heart == 0.5f) return 0.7f;
                if (heart == 0.625f) return 1f;
                if (heart == 0.75f) return 1.3f;
                if (heart == 0.875f) return 1.7f;
                if (heart == 1f) return 2f;
            }
        }
        else
        {
            if (speed == 0f)
            {
                if (heart == 0f) return 0.15f;
                if (heart == 0.125f) return 0.1f;
                if (heart == 0.25f) return 0.1f;
                if (heart == 0.375f) return 0.1f;
                if (heart == 0.5f) return 0.1f;
                if (heart == 0.625f) return 0f;
                if (heart == 0.75f) return 0f;
                if (heart == 0.875f) return 0f;
                if (heart == 1f) return 0f;
            }
            if (speed == 0.1f)
            {
                if (heart == 0f) return 0.2f;
                if (heart == 0.125f) return 0.2f;
                if (heart == 0.25f) return 0.2f;
                if (heart == 0.375f) return 0.3f;
                if (heart == 0.5f) return 0.4f;
                if (heart == 0.625f) return 0.5f;
                if (heart == 0.75f) return 0.6f;
                if (heart == 0.875f) return 0.4f;
                if (heart == 1f) return 0.7f;
            }
            if (speed == 0.2f)
            {
                if (heart == 0f) return 0.22f;
                if (heart == 0.125f) return 0.21f;
                if (heart == 0.25f) return 0.25f;
                if (heart == 0.375f) return 0.35f;
                if (heart == 0.5f) return 0.45f;
                if (heart == 0.625f) return 0.6f;
                if (heart == 0.75f) return 0.8f;
                if (heart == 0.875f) return 0.8f;
                if (heart == 1f) return 0.5f;
            }
            if (speed == 0.3f)
            {
                if (heart == 0f) return 0.21f;
                if (heart == 0.125f) return 0.2f;
                if (heart == 0.25f) return 0.2f;
                if (heart == 0.375f) return 0.3f;
                if (heart == 0.5f) return 0.45f;
                if (heart == 0.625f) return 0.6f;
                if (heart == 0.75f) return 0.8f;
                if (heart == 0.875f) return 0.9f;
                if (heart == 1f) return 0.6f;
            }
            if (speed == 0.4f)
            {
                if (heart == 0f) return 0.2f;
                if (heart == 0.125f) return 0.18f;
                if (heart == 0.25f) return 0.2f;
                if (heart == 0.375f) return 0.22f;
                if (heart == 0.5f) return 0.25f;
                if (heart == 0.625f) return 0.4f;
                if (heart == 0.75f) return 0.6f;
                if (heart == 0.875f) return 0.9f;
                if (heart == 1f) return 0.9f;
            }
            if (speed == 0.5f)
            {
                if (heart == 0f) return 0.18f;
                if (heart == 0.125f) return 0.13f;
                if (heart == 0.25f) return 0.13f;
                if (heart == 0.375f) return 0.15f;
                if (heart == 0.5f) return 0.25f;
                if (heart == 0.625f) return 0.34f;
                if (heart == 0.75f) return 0.5f;
                if (heart == 0.875f) return 0.8f;
                if (heart == 1f) return 0.8f;
            }
            if (speed == 0.6f)
            {
                if (heart == 0f) return 0.15f;
                if (heart == 0.125f) return 0.05f;
                if (heart == 0.25f) return 0.05f;
                if (heart == 0.375f) return 0.05f;
                if (heart == 0.5f) return 0.05f;
                if (heart == 0.625f) return 0.1f;
                if (heart == 0.75f) return 0.3f;
                if (heart == 0.875f) return 0f;
                if (heart == 1f) return 0f;
            }
            if (speed == 0.7f)
            {
                if (heart == 0f) return 0.1f;
                if (heart == 0.125f) return 0.05f;
                if (heart == 0.25f) return -0.1f;
                if (heart == 0.375f) return -0.1f;
                if (heart == 0.5f) return -0.1f;
                if (heart == 0.625f) return -0.2f;
                if (heart == 0.75f) return -0.2f;
                if (heart == 0.875f) return -0.2f;
                if (heart == 1f) return -0.2f;
            }
            if (speed == 0.8f)
            {
                if (heart == 0f) return 0.05f;
                if (heart == 0.125f) return 0f;
                if (heart == 0.25f) return -0.05f;
                if (heart == 0.375f) return -0.05f;
                if (heart == 0.5f) return -0.05f;
                if (heart == 0.625f) return -0.05f;
                if (heart == 0.75f) return -0.05f;
                if (heart == 0.875f) return -0.05f;
                if (heart == 1f) return -0.05f;
            }
            if (speed == 0.9f)
            {
                if (heart == 0f) return 0.2f;
                if (heart == 0.125f) return 0f;
                if (heart == 0.25f) return -0.05f;
                if (heart == 0.375f) return -0.05f;
                if (heart == 0.5f) return -0.05f;
                if (heart == 0.625f) return -0.05f;
                if (heart == 0.75f) return -0.05f;
                if (heart == 0.875f) return -0.05f;
                if (heart == 1f) return -0.05f;
            }
            if (speed == 1f)
            {
                if (heart == 0f) return 0.1f;
                if (heart == 0.125f) return 0.1f;
                if (heart == 0.25f) return 0.1f;
                if (heart == 0.375f) return 0.1f;
                if (heart == 0.5f) return 0.1f;
                if (heart == 0.625f) return 0.1f;
                if (heart == 0.75f) return 0.1f;
                if (heart == 0.875f) return 0.1f;
                if (heart == 1f) return 0f;
            }
        }
        return 0f;
    }

    float getVelocidadBarrido(float speed){
        if(speed == 0f)
            return 10.176f;
        if(speed == 0.1f)
            return 8.275f * 0.8f;
        if(speed == 0.2f)
            return 6.6048f * 0.8f;
        if(speed == 0.3f)
            return 5.1647f * 0.8f;
        if(speed == 0.4f)
            return 3.955f * 0.9f;
        if(speed == 0.5f)
            return 2.79f;
        if(speed == 0.6f)
            return 2.227f * 1.1f;
        if(speed == 0.7f)
            return 1.708f * 1.3f;
        if(speed == 0.8f)
            return 1.4208f * 1.45f;
        if(speed == 0.9f)
            return 1.363f * 1.35f;
        if(speed == 1f)
            return 1.536f * 1.1f;
        return 0f;
    }
}
