using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class ControladorGrafica : MonoBehaviour {
	//int sizeScreen = 256;
	int sizeScreen = 384;
	int sizeVertical = 256;
	Texture2D texture;

	int indiceActual = 0;
	public float zero = 0;
	public float zeroScale;
	public float scale = 1; //old: 2
	public float gain = 1;
	public float speed = 1; //old: 0.39f
	public float power = 1;
	public int sign = 1;
	public int heartRate = 120;
	[HideInInspector]
	public int cursorNull = 1;
	float speedAux = 0;

	[HideInInspector]
	public float[] red = new float[3];
	float redAux = 0;
	[HideInInspector]
	public float[] blue = new float[3];
	float blueAux = 0;

	int pathology = 2;
	[HideInInspector]
	public bool pause = false;
	public UISprite pauseButton;
	public int rangoRnd = 20;
	public int lowRnd = 20;
	public int highRnd = 40;

	MovieTexture mov;
	public UITexture mov1;
	public UILabel ticLabel;
	public LabelIndicators labels;

	public UISlider [] sliders;
	public float[] sliderLastValue;
	[HideInInspector]
	public float val;
	[HideInInspector]
	public float angle = 1;
	//indices sliders
	[HideInInspector]
	public int speedIndex = 0;
	[HideInInspector]
	public int scaleIndex = 1;
	[HideInInspector]
	public int zeroIndex = 2;
	[HideInInspector]
	public int gainIndex = 3;
	[HideInInspector]
	public int powerIndex = 4;
	[HideInInspector]
	public int pathologyIndex = 5;
	[HideInInspector]
	public int hScaleIndex = 6;

	public float C_SPEED = 30f;
	public float C_SCALE = 15f;

	public float refreshDelay;
	bool refreshing;

	public UILabel scaleNumber;
	GameObject horizontalScale;
	GameObject verticalScale;

	public UILabel hrLabel;
	public float patologiaLimite = 3f;
	public float patologiaSuma = 5f;

	float time;

	void Start() {
		//obtiene tamaño de la pantalla del tamaño del transform
		sizeScreen = (int)transform.localScale.x;
		sizeVertical = (int)transform.localScale.y;
		horizontalScale = transform.FindChild ("Horizontal").gameObject;
		verticalScale = transform.FindChild ("Vertical").gameObject;
		//texture = new Texture2D(sizeScreen, sizeScreen);
		texture = new Texture2D(sizeScreen, sizeVertical);

		//pinta el quad completamente de negro + linea central
		PaintItBlack ();
		GetComponent<Renderer>().material.mainTexture = texture;

		//define colores rojo y azul a utilizar
		Color.RGBToHSV (Color.red, out red[0], out red[1], out red[2]);
		redAux = red [0];
		Color.RGBToHSV (Color.blue, out blue[0], out blue[1], out blue[2]);
		blueAux = blue [0];
		mov = (MovieTexture)mov1.mainTexture;
		mov.loop = true;
        mov.Play();
		speedAux = speed;

		//crea arreglo de ultimo valor para sliders a partir de la cantidad de sliders existentes
		//y almacena sus valores
		sliderLastValue = new float[sliders.Length];
		for (int i = 0; i < sliderLastValue.Length; i++) {
			sliderLastValue [i] = sliders [i].value;
		}

		//ejecuta cada slider una vez para compensar (ngui ejecuta el metodo contrario de forma automatica)
		//ej: al ejecutarse la aplicacion, se ejecuta automaticamente GraphSpeed2() -> aqui se ejecuta GraphSpeed1()
		//para compensar y mantener los valores correctos
		GraphSpeed1 ();
		GraphScale2 ();
		GraphZero2 ();
		GraphGain2 ();
		GraphPower2 ();
		GraphPathology2 ();
		GraphHScale2 ();
		time = Time.time;
		transform.localScale = new Vector3 (384, transform.localScale.y, transform.localScale.z);
	}

	void UpdateHScale(){
		//Limpiar numeros antiguos
		GameObject[] auxH = GameObject.FindGameObjectsWithTag ("HScale");
		for (int i = 0; i < auxH.Length; i++)
			Destroy (auxH [i]);
		
		//Crear numeros nuevos con separacion actual
		float cnt = 0;
		float hScale = 0;
		scaleNumber.fontSize = Mathf.Clamp(Mathf.RoundToInt((C_SPEED / speed) / 3f),3,8);
		while(cnt <= sizeScreen){
			scaleNumber.text = (Mathf.Round(hScale*100f)/100f).ToString ();
			GameObject aux = (GameObject)Instantiate (scaleNumber.gameObject, horizontalScale.transform.position, horizontalScale.transform.rotation, horizontalScale.transform);
			aux.transform.localPosition = new Vector2 (aux.transform.localPosition.x + cnt, aux.transform.localPosition.y);
			aux.tag = "HScale";
			cnt += C_SPEED / speed;
			float spb = Mathf.Round((60f / heartRate)*100f)/100f;
			hScale += spb;
		}
	}

	void PaintItBlack(){
		///pinta el quad completamente de negro
		Color ini = new Color(0,0,0,0);
		Color[] colArray = texture.GetPixels ();
		for (int i = 0; i < colArray.Length; i++) {
			colArray [i] = ini;
		}
		texture.SetPixels (colArray);
		//pinta linea blanca horizontal en el centro del quad
		for (int i = 0; i < sizeScreen; i++) {
			texture.SetPixel (i, sizeVertical / pathology + (int)(val * zero), Color.white);
		}
		texture.Apply ();
	}

	public void SliderChange(int index){
		//metodo compartido por todos los sliders para actualizar su valor
		//valor de entrada: indice del slider
		//compara el valor actual del slider con el anterior, para ver si se movió hacia
		//derecha o izquierda y dependiendo del caso, ejecuta el metodo adecuado
		float aux = sliders [index].value;
		if (aux < sliderLastValue [index]) {
			switch (index) {
			case 0:
				GraphSpeed1 ();
				break;
			case 1:
				GraphScale2 ();
				break;
			case 2:
				GraphZero2 ();
				break;
			case 3:
				GraphGain2 ();
				break;
			case 4:
				GraphPower2 ();
				break;
			case 5:
				GraphPathology2 ();
				break;
			case 6:
				GraphHScale2 ();
				break;
			}
			
		} else {
			switch (index) {
			case 0:
				GraphSpeed2 ();
				break;
			case 1:
				GraphScale1 ();
				break;
			case 2:
				GraphZero1 ();
				break;
			case 3:
				GraphGain1 ();
				break;
			case 4:
				GraphPower1 ();
				break;
			case 5:
				GraphPathology1 ();
				break;
			case 6:
				GraphHScale1 ();
				break;
			}
		}
		//actuliza el valor anterior al actual
		sliderLastValue [index] = sliders [index].value;
	}

	void Update(){
		if (pause || refreshing)
			return;
		//cuando se completa el barrido
		if (indiceActual % sizeScreen == 0) {
			indiceActual = 0; //indice vuelve a cero
			speed = speedAux; //actualiza velocidad con el valor auxiliar utilizado por el slider
			UpdateHScale (); //actualiza numeros de escala horizontal
			Debug.Log (Time.time - time);
			time = Time.time;
		}

		//pinta verticalmente el indice actual con negro
		for (int i = 0; i < sizeVertical; i++) {
			texture.SetPixel (indiceActual % sizeScreen, i, Color.black);
		}
		float aux = 0;

		//define el limite vertical de la funcion en que se comienza a modificar la señal por patologia
		//ej: c_speed * scale = 30 -> patologiaLimite = 20
		//desde el pixel 20 se empieza a desplazar la señal
		patologiaLimite = (C_SPEED * scale)*(2f/3f);
		//v0: seno
		//float test = Mathf.Sin (5f * indiceActual * 360f / sizeScreen * Mathf.PI / 180f)*5f+ 2f + zero;

		//v1: seno ajustado
		//float test = cursorNull*angle*sign*scale*(Mathf.Sin ((0.5f * indiceActual * 360f / sizeScreen * Mathf.PI/180f * Mathf.PI/speed - 0.8f + Mathf.Sin(0.5f*indiceActual* 360f / sizeScreen * Mathf.PI/180f * Mathf.PI/speed)))* Mathf.Log (indiceActual) + 2f + zero);

		//v2: sawtooth
		//funcion modulo descendiente
		float waveAux = (C_SPEED * scale) - ((indiceActual * scale * speed) % (C_SPEED * scale));
		if (waveAux < patologiaLimite)
			waveAux += Random.Range(2f,patologiaSuma*scale*0.5f);
		if (waveAux > waveAux*0.95f)
			waveAux -= Random.Range(0f,1f);
		float test = cursorNull * angle * sign * (0.15f * (waveAux)) + 2f * sign + zero;
	
		//Debug.Log (test);
		if (power <= 0) {
			//test = 0;
		}

		//posicion del cero
		int zeroAux = sizeVertical / pathology + (int)(val * zero);

        //rellenar espacio entre cero y funcion con pixeles aleatorios (dos niveles: alto y bajo)
        //realiza clamp para mantener pixeles entre el cero y la funcion

        //positivo
        if (sign == 1)
        {
            //relleno menor
            for (int i = 0; i < Random.Range(5, 15) + (power * 40) + (gain * 40); i++)
            {
                aux = (Random.Range(-lowRnd, -5)) + sizeVertical / pathology + (val * test);
                aux = Mathf.Clamp(aux, (int)Mathf.Min(zeroAux, zeroAux + waveAux), aux + Random.Range(10, rangoRnd));
                texture.SetPixel(indiceActual % sizeScreen, (int)aux, Color.white);
            }
            //relleno mayor
            for (int i = 0; i < Random.Range(5, 20) + (power * 40) + (gain * 40); i++)
            {
                aux = (Random.Range(-highRnd, -5)) + sizeVertical / pathology + (val * test);
                aux = Mathf.Clamp(aux, (int)Mathf.Min(zeroAux, zeroAux + waveAux), aux + Random.Range(10, rangoRnd));
                if (aux < zero)
                    aux += sizeVertical / pathology + (val * test);
                else
                    texture.SetPixel(indiceActual % sizeScreen, (int)aux, Color.white);
            }
            //DrawWave (test, sizeScreen / pathology + (int)(val * zero), aux + Random.Range (10, rangoRnd), true);
        }
        //negativo
        else
        {
            //relleno menor
            for (int i = 0; i > Random.Range(-5, -15) - (power * 40) - (gain * 40); i--)
            {
                aux = (Random.Range(lowRnd, 5)) + sizeVertical / pathology + (val * test);
                aux = Mathf.Clamp(aux, aux - Random.Range(10, rangoRnd), sizeVertical / pathology + (int)(val * zero));
                texture.SetPixel(indiceActual % sizeScreen, (int)aux, Color.white);
            }
            //relleno mayor
            for (int i = 0; i > Random.Range(-5, -20) - (power * 40) - (gain * 40); i--)
            {
                aux = (Random.Range(highRnd, 5)) + sizeVertical / pathology + (val * test);
                aux = Mathf.Clamp(aux, aux - Random.Range(10, rangoRnd), sizeVertical / pathology + (int)(val * zero));
                if (aux < zero)
                    aux += sizeVertical / pathology + (val * test);
                else
                    texture.SetPixel(indiceActual % sizeScreen, (int)aux, Color.white);
            }
            //DrawWave (test, aux - Random.Range (10, rangoRnd), sizeScreen / pathology + (int)(val * zero), false);

        }
        /*if (sign == 1) {
			//relleno menor
			for (int i = 0; i < Random.Range (5, 15); i++) {
				aux = (gain * Random.Range (-lowRnd, -5)) + sizeVertical / pathology + (val * test);
				aux = Mathf.Clamp (aux, (int)Mathf.Min(zeroAux, zeroAux + waveAux), aux + Random.Range (10, rangoRnd));
				texture.SetPixel (indiceActual % sizeScreen, (int)aux, Color.white);
			}
			//relleno mayor
			for (int i = 0; i < Random.Range (5, 20); i++) {
				aux = (gain * Random.Range (-highRnd, -5)) + sizeVertical / pathology + (val * test);
				aux = Mathf.Clamp (aux, (int)Mathf.Min(zeroAux, zeroAux + waveAux), aux + Random.Range (10, rangoRnd));
				if (aux < zero)
					aux += sizeVertical / pathology + (val * test);
				else
					texture.SetPixel (indiceActual % sizeScreen, (int)aux, Color.white);
			}
			//DrawWave (test, sizeScreen / pathology + (int)(val * zero), aux + Random.Range (10, rangoRnd), true);
		} 
		//negativo
		else {
			//relleno menor
			for (int i = 0; i > Random.Range (-5, -15); i--) {
				aux = (gain * Random.Range (lowRnd, 5)) + sizeVertical / pathology + (val * test);
				aux = Mathf.Clamp (aux, aux - Random.Range (10, rangoRnd), sizeVertical / pathology + (int)(val * zero));
				texture.SetPixel (indiceActual % sizeScreen, (int)aux, Color.white);
			}
			//relleno mayor
			for (int i = 0; i > Random.Range (-5, -20); i--) {
				aux = (gain * Random.Range (highRnd, 5)) + sizeVertical / pathology + (val * test);
				aux = Mathf.Clamp (aux, aux - Random.Range (10, rangoRnd), sizeVertical / pathology + (int)(val * zero));
				if (aux < zero)
					aux += sizeVertical / pathology + (val * test);
				else
					texture.SetPixel (indiceActual % sizeScreen, (int)aux, Color.white);
			}
			//DrawWave (test, aux - Random.Range (10, rangoRnd), sizeScreen / pathology + (int)(val * zero), false);
				
		}*/

        //mantiene la linea de cero
        texture.SetPixel (indiceActual % sizeScreen, sizeVertical / pathology + (int)(val * zero), Color.white);

		texture.Apply();

		//detiene la ejecucion por tiempo de refresco
		refreshing = true;
		StartCoroutine(refreshRate());
		//libera la ejecucion


		//escribe el label de TIC
		ticLabel.text = (1+(heartRate-100f)/100f).ToString();
		if (ticLabel.text == "1")
			ticLabel.text = "1.0";

		if (Input.GetKeyDown (KeyCode.Escape)) {
			SceneManager.LoadScene ("Login");
		}
	}

	public void GraphZero1(){ 
		labels.lowerVerticalPosition ();
		GraphZero (1);
	}
	public void GraphZero2(){ 
		GraphZero (-1);
		labels.upperVerticalPosition ();
	}
	public void GraphScale1(){ 
		labels.scaleUpVertical();
		GraphScale (1);
	}
	public void GraphScale2(){
		labels.scaleDownVertical ();
		GraphScale (-1);
	}
	public void GraphGain1(){ GraphGain (1); }
	public void GraphGain2(){ GraphGain (-1); }

	public void GraphSpeed1(){ 
		labels.scaleDownHorizontal ();
		GraphSpeed (1);
	}
	public void GraphSpeed2(){
		labels.scaleUpHorizontal ();
		GraphSpeed (-1);
	}
	public void GraphHScale1(){ 
		labels.scaleDownHorizontal ();
		GraphHScale (1);
	}
	public void GraphHScale2(){
		labels.scaleUpHorizontal ();
		GraphHScale (-1);
	}
	public void GraphPower1(){ GraphPower (1); }
	public void GraphPower2(){ GraphPower (-1); }
	public void GraphTIC1(){ GraphTIC (1); }
	public void GraphTIC2(){ GraphTIC(-1);}
	public void GraphPathology1(){GraphPathology (-1);}
	public void GraphPathology2(){GraphPathology (1);}

	public void GraphZero(int n){
		//posicion del cero aumenta/disminuye en la cantidad de zeroScale
		if (Mathf.Abs (n) == 1) {
			zero += n * zeroScale;
		}
		indiceActual = 0;
		PaintItBlack ();
	}

	public void GraphScale(int n){
		//escala aumenta/disminuye en 50%, entre 0.5 y 4.5
		if (Mathf.Abs (n) == 1) {
			scale += 0.5f * n;
			scale = Mathf.Clamp (scale, 0.5f, 4.5f);
		}
	}

	public void GraphGain(int n){
		//gain aumenta/disminuye en 20%, entre 0 y 1
		if (Mathf.Abs (n) == 1) {
			gain += 1f/11f * n;
			gain = Mathf.Clamp (gain, 0, 1);
            print("gain " + gain);
		}
	}

	public void GraphPathology(int n){
		//posicion de desfase patologia aumenta/disminuye en 5 pixeles
		if (Mathf.Abs (n) == 1) {
			patologiaSuma += 8f*Mathf.Sign(n);
		}
	}

	public void GraphSpeed(int n){
		//velocidad aumenta/disminuye en C_SCALE %
		if (Mathf.Abs (n) == 1) {
			//speed = Mathf.Clamp(speed + 0.1f*n,0.1f,1);
			if (n > 0) {
				//speedAux = Mathf.Clamp(speedAux *= 2, 0.1f, 1 * Mathf.Pow(2,2));
				speedAux = Mathf.Clamp (speedAux /= C_SCALE, 2f * Mathf.Pow (C_SCALE, -5), float.MaxValue);
			} else {
				//speedAux = Mathf.Clamp (speedAux /= 2, 1 * Mathf.Pow (2, -2), 0.8f);
				speedAux = Mathf.Clamp (speedAux *= C_SCALE, 0, 2f * Mathf.Pow (C_SCALE, 4));
			}
		}
		//actualiza heart rate aumentando/disminuyendo en 10
		heartRate = (int)Mathf.Clamp (heartRate + -n*10f/11f, 70f, 140f);
		hrLabel.text = heartRate.ToString ();
		//reinicia el barrido
		indiceActual = 0;
		speed = speedAux;
		//refreshDelay = (speed / 100)*(speed/100);
		PaintItBlack ();
		UpdateHScale ();
	}

	//ARREGLAR
	public void GraphHScale(int n){
		if (Mathf.Abs (n) == 1) {
			//speed = Mathf.Clamp(speed + 0.1f*n,0.1f,1);
			if (n > 0) {
				//speedAux = Mathf.Clamp(speedAux *= 2, 0.1f, 1 * Mathf.Pow(2,2));
				speedAux = Mathf.Clamp (speedAux /= C_SCALE, 2f * Mathf.Pow (C_SCALE, -3), float.MaxValue);
			} else {
				//speedAux = Mathf.Clamp (speedAux /= 2, 1 * Mathf.Pow (2, -2), 0.8f);
				speedAux = Mathf.Clamp (speedAux *= C_SCALE, 0, 2f * Mathf.Pow (C_SCALE, 4));
			}
		}
		//heartRate = (int)Mathf.Clamp (heartRate + -n*10f, 70f, 140f);
		//hrLabel.text = heartRate.ToString ();
		indiceActual = 0;
		speed = speedAux;
		//refreshDelay = (1 - sliders [hScaleIndex].value) / 50f;
		PaintItBlack ();
		UpdateHScale ();
	}



	public void GraphPower(int n){
		if (Mathf.Abs (n) == 1) {
			power += 1f/11f * n;
			power = Mathf.Clamp (power, 0, 1);
			/*blueAux += 1f / 11f * n;
			blue [0] = Mathf.Clamp (blueAux, 0, 1);
			redAux += 1f / 11f * n;
			red [0] = Mathf.Clamp (redAux, 0, 1);*/
            print("power " + power);
		}
	}

	public void GraphTIC(int n){
		if (Mathf.Abs (n) == 1) {
			sign = n;
		}
	}

	public void PathologyMode_old(){
		if (pathology == 1) {
			pathology = 2;
			zero = 0;
		}
		else if (pathology == 2){
			pathology = 1;
			zero = 25.6f;
		}
		Mathf.Clamp (pathology, 1, 2);
		print (pathology);
	}

	public void PauseGUI(){
		pause = !pause;
		if (pause)
			pauseButton.alpha = 255f;
		else
			pauseButton.alpha = 0f;
	}

	public void GraphCursorNull(bool b){
		if (b)
			cursorNull = 1;
		else
			cursorNull = 0;
	}

	IEnumerator refreshRate(){
		yield return new WaitForSeconds (refreshDelay);
		indiceActual++;
		refreshing = false;
	}
}
