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
	public float scale = 2;
	public float gain = 1;
	public float speed = 0.39f;
	public float power = 1;
	public int sign = 1;
	float speedAux = 0;
	public float[] red = new float[3];
	float redAux = 0;
	public float[] blue = new float[3];
	float blueAux = 0;
	public int pathology = 2;
	public bool pause = false;
	public UISprite pauseButton;
	public int rangoRnd = 20;
	public int lowRnd = 20;
	public int highRnd = 40;
	MovieTexture mov;
	public UITexture mov1;
	public UILabel ticLabel;
	public LabelIndicators labels;
	public float val;
	public float angle = 1;
	public UISlider [] sliders;
	public float [] sliderLastValue;
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

	void Start() {
		texture = new Texture2D(sizeScreen, sizeScreen);
		GetComponent<Renderer>().material.mainTexture = texture;
		Color.RGBToHSV (Color.red, out red[0], out red[1], out red[2]);
		redAux = red [0];
		Color.RGBToHSV (Color.blue, out blue[0], out blue[1], out blue[2]);
		blueAux = blue [0];
		mov = (MovieTexture)mov1.mainTexture;
		mov.loop = true;
		mov.Play ();
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
		if (pause)
			return;
		if (indiceActual % sizeScreen == 0) {
			indiceActual = 0;
			speed = speedAux;
		}
		for (int i = 0; i < sizeScreen; i++) {
			texture.SetPixel(indiceActual % sizeScreen, i, Color.black);
		}
		float aux = 0;

		//float test = Mathf.Sin (5f * indiceActual * 360f / sizeScreen * Mathf.PI / 180f)*5f+ 2f + zero;
		float test = angle*sign*scale*(Mathf.Sin ((0.5f * indiceActual * 360f / sizeScreen * Mathf.PI/180f * Mathf.PI/speed - 0.8f + Mathf.Sin(0.5f*indiceActual* 360f / sizeScreen * Mathf.PI/180f * Mathf.PI/speed)))* Mathf.Log (indiceActual) + 2f + zero);
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
		indiceActual++;

		ticLabel.text = (Mathf.Round((1-speed+0.5f)*10)/10).ToString();
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
			if (n > 0)
				speedAux = Mathf.Clamp(speedAux *= 2, 0.1f, 0.39f * Mathf.Pow(2,2));
			else
				speedAux = Mathf.Clamp (speedAux /= 2, 0.39f * Mathf.Pow (2, -2), 0.8f);
		}
		indiceActual = 0;
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
}
