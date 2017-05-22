using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class ControladorGrafica : MonoBehaviour {
	int sizeScreen = 256;
	Texture2D texture;
	int indiceActual = 0;
	public float zero = 0;
	public float zeroScale;
	public float scale = 1; //old: 2
	public float gain = 1;
	public float speed = 1; //old: 0.39f
	public float power = 1;
	public int sign = 1;
	public int heartRate = 100;
	[HideInInspector]
	public int cursorNull = 1;
	float speedAux = 0;
	[HideInInspector]
	public float[] red = new float[3];
	float redAux = 0;
	[HideInInspector]
	public float[] blue = new float[3];
	float blueAux = 0;
	public int pathology = 2;
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

	public float C_SPEED = 30f;
	public float C_SCALE = 15f;

	public float refreshDelay;
	bool refreshing;

	void Start() {
		texture = new Texture2D(sizeScreen, sizeScreen);
		PaintItBlack ();
		GetComponent<Renderer>().material.mainTexture = texture;
		Color.RGBToHSV (Color.red, out red[0], out red[1], out red[2]);
		redAux = red [0];
		Color.RGBToHSV (Color.blue, out blue[0], out blue[1], out blue[2]);
		blueAux = blue [0];
		mov = (MovieTexture)mov1.mainTexture;
		mov.loop = true;
		speedAux = speed;
		sliderLastValue = new float[sliders.Length];
		for (int i = 0; i < sliderLastValue.Length; i++) {
			sliderLastValue [i] = sliders [i].value;
		}
		GraphSpeed1 ();
		GraphScale2 ();
		GraphZero2 ();
		GraphGain2 ();
		GraphPower2 ();
	}

	void PaintItBlack(){
		Color ini = new Color(0,0,0,0);
		Color[] colArray = texture.GetPixels ();
		for (int i = 0; i < colArray.Length; i++) {
			colArray [i] = ini;
		}
		texture.SetPixels (colArray);
		for (int i = 0; i < sizeScreen; i++) {
			texture.SetPixel (i, sizeScreen / pathology + (int)(val * zero), Color.white);
		}
		texture.Apply ();
	}

	public void SliderChange(int index){
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
			}
		}
		sliderLastValue [index] = sliders [index].value;
	}

	void Update(){
		if (pause || refreshing)
			return;
		if (indiceActual % sizeScreen == 0) {
			indiceActual = 0;
			speed = speedAux;
		}
		for (int i = 0; i < sizeScreen; i++) {
			texture.SetPixel(indiceActual % sizeScreen, i, Color.black);
		}
		float aux = 0;

		//v0: seno
		//float test = Mathf.Sin (5f * indiceActual * 360f / sizeScreen * Mathf.PI / 180f)*5f+ 2f + zero;

		//v1: seno ajustado
		//float test = cursorNull*angle*sign*scale*(Mathf.Sin ((0.5f * indiceActual * 360f / sizeScreen * Mathf.PI/180f * Mathf.PI/speed - 0.8f + Mathf.Sin(0.5f*indiceActual* 360f / sizeScreen * Mathf.PI/180f * Mathf.PI/speed)))* Mathf.Log (indiceActual) + 2f + zero);

		//v2: sawtooth

		float test = cursorNull*angle*sign*(0.33f*((C_SPEED*scale) - ((indiceActual*scale*speed) % (C_SPEED*scale)))) + 2f*sign + zero; 
		//Debug.Log (test);
		if (power <= 0) {
			test = 0;
		}

		if (pathology == 2) {
			if (sign == 1) {
				//bordes azules
				for (int i = 0; i < Random.Range (5, 15); i++) {
					aux = (gain * Random.Range (-lowRnd, -5)) + sizeScreen / pathology + (val * test);
					aux = Mathf.Clamp (aux, sizeScreen / pathology + (int)(val * zero), aux + Random.Range (10, rangoRnd));
					texture.SetPixel (indiceActual % sizeScreen, (int)aux, Color.white);
				}
				for (int i = 0; i < Random.Range (5, 20); i++) {
					aux = (gain * Random.Range (-highRnd, -5)) + sizeScreen / pathology + (val * test);
					aux = Mathf.Clamp (aux, sizeScreen / pathology + (int)(val * zero), aux + Random.Range (10, rangoRnd));
					if (aux < zero)
						aux += sizeScreen / pathology + (val * test);
					else
						texture.SetPixel (indiceActual % sizeScreen, (int)aux, Color.white);
				}
				//DrawWave (test, sizeScreen / pathology + (int)(val * zero), aux + Random.Range (10, rangoRnd), true);
			} else {
				//bordes azules
				for (int i = 0; i > Random.Range (-5, -15); i--) {
					aux = (gain * Random.Range (lowRnd, 5)) + sizeScreen / pathology + (val * test);
					aux = Mathf.Clamp (aux, aux - Random.Range (10, rangoRnd), sizeScreen / pathology + (int)(val * zero));
					texture.SetPixel (indiceActual % sizeScreen, (int)aux, Color.white);
				}
				for (int i = 0; i > Random.Range (-5, -20); i--) {
					aux = (gain * Random.Range (highRnd, 5)) + sizeScreen / pathology + (val * test);
					aux = Mathf.Clamp (aux, aux - Random.Range (10, rangoRnd), sizeScreen / pathology + (int)(val * zero));
					if (aux < zero)
						aux += sizeScreen / pathology + (val * test);
					else
						texture.SetPixel (indiceActual % sizeScreen, (int)aux, Color.white);
				}
				//DrawWave (test, aux - Random.Range (10, rangoRnd), sizeScreen / pathology + (int)(val * zero), false);
				
			}
		}/* else if (pathology == 3) {
			for (int i = 0; i < Random.Range (5, 15); i++) {
				aux = (gain * Random.Range (-lowRnd, -5)) + sizeScreen / pathology + (val * test);
				aux = Mathf.Clamp (aux, sizeScreen / pathology + (int)(val * zero), aux + Random.Range(10,rangoRnd));
				texture.SetPixel (indiceActual % sizeScreen, (int)aux, Color.white);
			}
			for (int i = 0; i < Random.Range (5, 20); i++) {
				aux = (gain * Random.Range (-highRnd, -5)) + sizeScreen / pathology + (val * test);
				aux = Mathf.Clamp (aux, sizeScreen / pathology + (int)(val * zero), aux + Random.Range(10,rangoRnd));
				if (indiceActual % sizeScreen == 0)
					print (aux);
				if (aux < zero)
					aux += sizeScreen / pathology + (val * test);
				else
					texture.SetPixel (indiceActual % sizeScreen, (int)aux, Color.white);
			}

			for (int i = 0; i > Random.Range (-5, -15); i--) {
				aux = (gain * Random.Range (lowRnd, 5)) + sizeScreen / pathology + (val * test);
				aux = Mathf.Clamp (aux, aux - Random.Range (10, rangoRnd), sizeScreen / pathology + (int)(val * zero));
				aux = sizeScreen / pathology + (int)(val * zero) - aux;
				texture.SetPixel (indiceActual % sizeScreen, (int)aux, Color.white);
			}
			for (int i = 0; i > Random.Range (-5, -20); i--) {
				aux = (gain * Random.Range (highRnd, 5)) + sizeScreen / pathology + (val * test);
				aux = Mathf.Clamp (aux, aux - Random.Range (10, rangoRnd), sizeScreen / pathology + (int)(val * zero));
				aux = sizeScreen / pathology + (int)(val * zero) - aux;
				if (indiceActual % sizeScreen == 0)
					print (aux);
				if (aux < zero)
					aux += sizeScreen / pathology + (val * test);
				else
					texture.SetPixel (indiceActual % sizeScreen, (int)aux, Color.white);
			}
		}*/
		else if (pathology == 1) {
			//25 = zero
			if (sign == 1) {
				if (test > 25) {
					for (int i = 0; i < Random.Range (5, 15); i++) {
						aux = (gain * Random.Range (-lowRnd, -5)) + sizeScreen / pathology + (val * test);
						aux = Mathf.Clamp (aux, sizeScreen / pathology + (int)(val * zero), aux + Random.Range(10,rangoRnd));
						texture.SetPixel (indiceActual % sizeScreen, (int)aux, Color.white);
					}
					for (int i = 0; i < Random.Range (5, 20); i++) {
						aux = (gain * Random.Range (-highRnd, -5)) + sizeScreen / pathology + (val * test);
						aux = Mathf.Clamp (aux, sizeScreen / pathology + (int)(val * zero), aux + Random.Range(10,rangoRnd));
						if (aux < zero)
							aux += sizeScreen / pathology + (val * test);
						else
							texture.SetPixel (indiceActual % sizeScreen, (int)aux, Color.white);
					}
					//DrawWave (test, sizeScreen / pathology + (int)(val * zero), aux + Random.Range (10, rangoRnd), true);
				} else {
					for (int i = 0; i > Random.Range (-5, -15); i--) {
						aux = (gain * Random.Range (lowRnd, 5)) + sizeScreen / pathology + (val * test);
						aux = Mathf.Clamp (aux, aux - Random.Range(10,rangoRnd), sizeScreen / pathology + (int)(val * zero));
						texture.SetPixel (indiceActual % sizeScreen, (int)aux, Color.white);

					}
					for (int i = 0; i > Random.Range (-5, -20); i--) {
						aux = (gain * Random.Range (highRnd, 5)) + sizeScreen / pathology + (val * test);
						aux = Mathf.Clamp (aux, aux - Random.Range(10,rangoRnd), sizeScreen / pathology + (int)(val * zero));
						if (aux < zero)
							aux += sizeScreen / pathology + (val * test);
						else
							texture.SetPixel (indiceActual % sizeScreen, (int)aux, Color.white);
					}
					//DrawWave (test, aux - Random.Range (10, rangoRnd), sizeScreen / pathology + (int)(val * zero), false);

				}
			} else {
				if (test > -25) {
					for (int i = 0; i < Random.Range (5, 15); i++) {
						aux = (gain * Random.Range (-lowRnd, -5)) + sizeScreen / pathology + (val * test);
						aux = Mathf.Clamp (aux, sizeScreen / (pathology*2) + (int)(val * (zero-25.6f)), (sizeScreen/ pathology + val * test) + Random.Range(10,rangoRnd));
						texture.SetPixel (indiceActual % sizeScreen, (int)aux, Color.white);
					}
					for (int i = 0; i < Random.Range (5, 20); i++) {
						aux = (gain * Random.Range (-highRnd, -5)) + sizeScreen / pathology + (val * test);
						aux = Mathf.Clamp (aux, sizeScreen / (pathology*2) + (int)(val * (zero-25.6f)), sizeScreen/ pathology + val * test + Random.Range(10,rangoRnd));
						if (aux < zero)
							aux += sizeScreen / pathology + (val * test);
						else
							texture.SetPixel (indiceActual % sizeScreen, (int)aux, Color.white);
					}
					//DrawWave (test, sizeScreen / (pathology * 2) + (int)(val * (zero - 25.6f)), (sizeScreen / pathology + val * test) + Random.Range (10, rangoRnd), true);
				} else {
					for (int i = 0; i > Random.Range (-5, -15); i--) {
						aux = (gain * Random.Range (lowRnd, 5)) + sizeScreen / pathology + (val * test);
						aux = Mathf.Clamp (aux, sizeScreen/ pathology + val * test - Random.Range(10,rangoRnd), sizeScreen / (pathology*2) + (int)(val * (zero-25.6f)));
						texture.SetPixel (indiceActual % sizeScreen, (int)aux, Color.white);

					}
					for (int i = 0; i > Random.Range (-5, -20); i--) {
						aux = (gain * Random.Range (highRnd, 5)) + sizeScreen / pathology + (val * test);
						aux = Mathf.Clamp (aux, sizeScreen/ pathology + val * test - Random.Range(10,rangoRnd), sizeScreen / (pathology*2) + (int)(val * (zero-25.6f)));
						if (aux < zero)
							aux += sizeScreen / pathology + (val * test);
						else
							texture.SetPixel (indiceActual % sizeScreen, (int)aux, Color.white);
					}
					//DrawWave (test, (sizeScreen / pathology + val * test) - Random.Range (10, rangoRnd),  sizeScreen / (pathology * 2) + (int)(val * (zero - 25.6f)), false);
				}
			}
		}

		aux = sizeScreen/ pathology + val * test;
		if (pathology != 1) {
			if (scale == 1) {
				aux = Mathf.Clamp (aux, sizeScreen / pathology + (int)(val * zero), sizeScreen);
			} else {
				aux = Mathf.Clamp (aux, 0, sizeScreen / pathology + (int)(val * zero));
			}
		}
		//texture.SetPixel(indiceActual % sizeScreen, (int)aux, Color.white);

		texture.SetPixel (indiceActual % sizeScreen, sizeScreen / pathology + (int)(val * zero), Color.white);

		texture.Apply();
		refreshing = true;
		//indiceActual++;
		StartCoroutine(refreshRate());

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
	public void GraphPower1(){ GraphPower (1); }
	public void GraphPower2(){ GraphPower (-1); }
	public void GraphTIC1(){ GraphTIC (1); }
	public void GraphTIC2(){ GraphTIC(-1);}

	public void GraphZero(int n){
		if (Mathf.Abs (n) == 1) {
			zero += n * zeroScale;
		}
		indiceActual = 0;
		PaintItBlack ();
	}

	public void GraphScale(int n){
		if (Mathf.Abs (n) == 1) {
			scale += 0.5f * n;
			scale = Mathf.Clamp (scale, 0, 4);
		}
	}

	public void GraphGain(int n){
		if (Mathf.Abs (n) == 1) {
			gain += 0.2f * n;
			gain = Mathf.Clamp (gain, 0, 1);
		}
	}

	public void GraphSpeed(int n){
		if (Mathf.Abs (n) == 1) {
			//speed = Mathf.Clamp(speed + 0.1f*n,0.1f,1);
			if (n > 0) {
				//speedAux = Mathf.Clamp(speedAux *= 2, 0.1f, 1 * Mathf.Pow(2,2));
				speedAux = Mathf.Clamp (speedAux /= C_SCALE, 1 * Mathf.Pow (C_SCALE, -3), float.MaxValue);
			} else {
				//speedAux = Mathf.Clamp (speedAux /= 2, 1 * Mathf.Pow (2, -2), 0.8f);
				speedAux = Mathf.Clamp (speedAux *= C_SCALE, 0, 1 * Mathf.Pow (C_SCALE, 3));
			}
		}
		heartRate = (int)Mathf.Clamp (heartRate + -n*10f, 70f, 140f);
		indiceActual = 0;
		speed = speedAux;
		PaintItBlack ();
	}

	public void GraphPower(int n){
		if (Mathf.Abs (n) == 1) {
			power += 0.2f * n;
			power = Mathf.Clamp (power, 0, 2);
			blueAux += 0.05f * n;
			blue [0] = Mathf.Clamp (blueAux, 0, 1);
			redAux += 0.05f * n;
			red [0] = Mathf.Clamp (redAux, 0, 1);
		}
	}

	public void GraphTIC(int n){
		if (Mathf.Abs (n) == 1) {
			sign = n;
		}
	}

	public void PathologyMode(){
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
