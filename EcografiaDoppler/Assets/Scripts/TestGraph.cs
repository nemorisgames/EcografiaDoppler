using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class TestGraph : MonoBehaviour {
	int sizeScreen = 256;
	Texture2D texture;
	int indiceActual = 0;
	public float zero = 0;
	public float zeroScale;
	public float scale = 1;
	public float gain = 1;
	public float speed = 0.5f;
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
	}

	void Update(){
		if (pause)
			return;
		for (int i = 0; i < sizeScreen; i++) {
			texture.SetPixel(indiceActual % sizeScreen, i, Color.black);
		}
		float aux = 0;

		//float f0 = Mathf.Sin (1.333f * indiceActual * 360f / sizeScreen * Mathf.PI / 180f);
		float test = sign*scale*(Mathf.Sin ((0.5f * indiceActual * 360f / sizeScreen * Mathf.PI/180f * Mathf.PI/speed - 0.8f + Mathf.Sin(0.5f*indiceActual* 360f / sizeScreen * Mathf.PI/180f * Mathf.PI/speed)))* Mathf.Log (indiceActual) + 2f + zero);
		if (power <= 0) {
			test = 0;
		}
		if (pathology == 2) {
			if (sign == 1) {
				//bordes azules
				for (int i = 0; i < Random.Range (5, 15); i++) {
					aux = (gain * Random.Range (-lowRnd, -5)) + sizeScreen / pathology + (5f * test);
					aux = Mathf.Clamp (aux, sizeScreen / pathology + (int)(5f * zero), aux + Random.Range (10, rangoRnd));
					//texture.SetPixel (indiceActual % sizeScreen, (int)aux, Color.HSVToRGB (blue [0], blue [1], blue [2]));
					texture.SetPixel (indiceActual % sizeScreen, (int)aux, Color.white);
				}
				for (int i = 0; i < Random.Range (5, 20); i++) {
					aux = (gain * Random.Range (-highRnd, -5)) + sizeScreen / pathology + (5f * test);
					aux = Mathf.Clamp (aux, sizeScreen / pathology + (int)(5f * zero), aux + Random.Range (10, rangoRnd));
					if (indiceActual % sizeScreen == 0)
						print (aux);
					if (aux < zero)
						aux += sizeScreen / pathology + (5f * test);
					else
						//texture.SetPixel (indiceActual % sizeScreen, (int)aux, Color.HSVToRGB (red [0], red [1], red [2]));
						texture.SetPixel (indiceActual % sizeScreen, (int)aux, Color.white);
				}
			} else {
				//bordes azules
				for (int i = 0; i > Random.Range (-5, -15); i--) {
					aux = (gain * Random.Range (lowRnd, 5)) + sizeScreen / pathology + (5f * test);
					aux = Mathf.Clamp (aux, aux - Random.Range (10, rangoRnd), sizeScreen / pathology + (int)(5f * zero));
					//texture.SetPixel (indiceActual % sizeScreen, (int)aux, Color.HSVToRGB (blue [0], blue [1], blue [2]));
					texture.SetPixel (indiceActual % sizeScreen, (int)aux, Color.white);
				}
				for (int i = 0; i > Random.Range (-5, -20); i--) {
					aux = (gain * Random.Range (highRnd, 5)) + sizeScreen / pathology + (5f * test);
					aux = Mathf.Clamp (aux, aux - Random.Range (10, rangoRnd), sizeScreen / pathology + (int)(5f * zero));
					if (indiceActual % sizeScreen == 0)
						print (aux);
					if (aux < zero)
						aux += sizeScreen / pathology + (5f * test);
					else
						//texture.SetPixel (indiceActual % sizeScreen, (int)aux, Color.HSVToRGB (red [0], red [1], red [2]));
						texture.SetPixel (indiceActual % sizeScreen, (int)aux, Color.white);
				}
				
			}
		} else if (pathology == 3) {
			for (int i = 0; i < Random.Range (5, 15); i++) {
				aux = (gain * Random.Range (-lowRnd, -5)) + sizeScreen / pathology + (5f * test);
				aux = Mathf.Clamp (aux, sizeScreen / pathology + (int)(5f * zero), aux + Random.Range(10,rangoRnd));
				//texture.SetPixel (indiceActual % sizeScreen, (int)aux, Color.HSVToRGB (blue [0], blue [1], blue [2]));
				texture.SetPixel (indiceActual % sizeScreen, (int)aux, Color.white);
			}
			for (int i = 0; i < Random.Range (5, 20); i++) {
				aux = (gain * Random.Range (-highRnd, -5)) + sizeScreen / pathology + (5f * test);
				aux = Mathf.Clamp (aux, sizeScreen / pathology + (int)(5f * zero), aux + Random.Range(10,rangoRnd));
				if (indiceActual % sizeScreen == 0)
					print (aux);
				if (aux < zero)
					aux += sizeScreen / pathology + (5f * test);
				else
					//texture.SetPixel (indiceActual % sizeScreen, (int)aux, Color.HSVToRGB (red [0], red [1], red [2]));
					texture.SetPixel (indiceActual % sizeScreen, (int)aux, Color.white);
			}

			for (int i = 0; i > Random.Range (-5, -15); i--) {
				aux = (gain * Random.Range (lowRnd, 5)) + sizeScreen / pathology + (5f * test);
				aux = Mathf.Clamp (aux, aux - Random.Range (10, rangoRnd), sizeScreen / pathology + (int)(5f * zero));
				//texture.SetPixel (indiceActual % sizeScreen, (int)aux, Color.HSVToRGB (blue [0], blue [1], blue [2]));
				aux = sizeScreen / pathology + (int)(5f * zero) - aux;
				texture.SetPixel (indiceActual % sizeScreen, (int)aux, Color.white);
			}
			for (int i = 0; i > Random.Range (-5, -20); i--) {
				aux = (gain * Random.Range (highRnd, 5)) + sizeScreen / pathology + (5f * test);
				aux = Mathf.Clamp (aux, aux - Random.Range (10, rangoRnd), sizeScreen / pathology + (int)(5f * zero));
				aux = sizeScreen / pathology + (int)(5f * zero) - aux;
				if (indiceActual % sizeScreen == 0)
					print (aux);
				if (aux < zero)
					aux += sizeScreen / pathology + (5f * test);
				else
					//texture.SetPixel (indiceActual % sizeScreen, (int)aux, Color.HSVToRGB (red [0], red [1], red [2]));
					texture.SetPixel (indiceActual % sizeScreen, (int)aux, Color.white);
			}
		}
		else if (pathology == 1) {
			//25 = zero
			if (sign == 1) {
				if (test > 25) {
					for (int i = 0; i < Random.Range (5, 15); i++) {
						aux = (gain * Random.Range (-lowRnd, -5)) + sizeScreen / pathology + (5f * test);
						aux = Mathf.Clamp (aux, sizeScreen / pathology + (int)(5f * zero), aux + Random.Range(10,rangoRnd));
						//texture.SetPixel (indiceActual % sizeScreen, (int)aux, Color.HSVToRGB (blue [0], blue [1], blue [2]));
						texture.SetPixel (indiceActual % sizeScreen, (int)aux, Color.white);
					}
					for (int i = 0; i < Random.Range (5, 20); i++) {
						aux = (gain * Random.Range (-highRnd, -5)) + sizeScreen / pathology + (5f * test);
						aux = Mathf.Clamp (aux, sizeScreen / pathology + (int)(5f * zero), aux + Random.Range(10,rangoRnd));
						if (indiceActual % sizeScreen == 0)
							print (aux);
						if (aux < zero)
							aux += sizeScreen / pathology + (5f * test);
						else
							//texture.SetPixel (indiceActual % sizeScreen, (int)aux, Color.HSVToRGB (red [0], red [1], red [2]));
							texture.SetPixel (indiceActual % sizeScreen, (int)aux, Color.white);
					}
				} else {
					for (int i = 0; i > Random.Range (-5, -15); i--) {
						aux = (gain * Random.Range (lowRnd, 5)) + sizeScreen / pathology + (5f * test);
						aux = Mathf.Clamp (aux, aux - Random.Range(10,rangoRnd), sizeScreen / pathology + (int)(5f * zero));
						//texture.SetPixel (indiceActual % sizeScreen, (int)aux, Color.HSVToRGB (blue [0], blue [1], blue [2]));
						texture.SetPixel (indiceActual % sizeScreen, (int)aux, Color.white);

					}
					for (int i = 0; i > Random.Range (-5, -20); i--) {
						aux = (gain * Random.Range (highRnd, 5)) + sizeScreen / pathology + (5f * test);
						aux = Mathf.Clamp (aux, aux - Random.Range(10,rangoRnd), sizeScreen / pathology + (int)(5f * zero));
						if (indiceActual % sizeScreen == 0)
							print (aux);
						if (aux < zero)
							aux += sizeScreen / pathology + (5f * test);
						else
							//texture.SetPixel (indiceActual % sizeScreen, (int)aux, Color.HSVToRGB (red [0], red [1], red [2]));
							texture.SetPixel (indiceActual % sizeScreen, (int)aux, Color.white);
					}
				}
			} else {
				if (test > -25) {
					for (int i = 0; i < Random.Range (5, 15); i++) {
						aux = (gain * Random.Range (-lowRnd, -5)) + sizeScreen / pathology + (5f * test);
						aux = Mathf.Clamp (aux, sizeScreen / (pathology*2) + (int)(5f * (zero-25.6f)), (sizeScreen/ pathology + 5f * test) + Random.Range(10,rangoRnd));
						//texture.SetPixel (indiceActual % sizeScreen, (int)aux, Color.HSVToRGB (blue [0], blue [1], blue [2]));
						texture.SetPixel (indiceActual % sizeScreen, (int)aux, Color.white);
					}
					for (int i = 0; i < Random.Range (5, 20); i++) {
						aux = (gain * Random.Range (-highRnd, -5)) + sizeScreen / pathology + (5f * test);
						aux = Mathf.Clamp (aux, sizeScreen / (pathology*2) + (int)(5f * (zero-25.6f)), sizeScreen/ pathology + 5f * test + Random.Range(10,rangoRnd));
						if (indiceActual % sizeScreen == 0)
							print (aux);
						if (aux < zero)
							aux += sizeScreen / pathology + (5f * test);
						else
							//texture.SetPixel (indiceActual % sizeScreen, (int)aux, Color.HSVToRGB (red [0], red [1], red [2]));
							texture.SetPixel (indiceActual % sizeScreen, (int)aux, Color.white);
					}
				} else {
					for (int i = 0; i > Random.Range (-5, -15); i--) {
						aux = (gain * Random.Range (lowRnd, 5)) + sizeScreen / pathology + (5f * test);
						aux = Mathf.Clamp (aux, sizeScreen/ pathology + 5f * test - Random.Range(10,rangoRnd), sizeScreen / (pathology*2) + (int)(5f * (zero-25.6f)));
						//texture.SetPixel (indiceActual % sizeScreen, (int)aux, Color.HSVToRGB (blue [0], blue [1], blue [2]));
						texture.SetPixel (indiceActual % sizeScreen, (int)aux, Color.white);

					}
					for (int i = 0; i > Random.Range (-5, -20); i--) {
						aux = (gain * Random.Range (highRnd, 5)) + sizeScreen / pathology + (5f * test);
						aux = Mathf.Clamp (aux, sizeScreen/ pathology + 5f * test - Random.Range(10,rangoRnd), sizeScreen / (pathology*2) + (int)(5f * (zero-25.6f)));
						if (indiceActual % sizeScreen == 0)
							print (aux);
						if (aux < zero)
							aux += sizeScreen / pathology + (5f * test);
						else
							//texture.SetPixel (indiceActual % sizeScreen, (int)aux, Color.HSVToRGB (red [0], red [1], red [2]));
							texture.SetPixel (indiceActual % sizeScreen, (int)aux, Color.white);
					}
				}
			}
		}

		aux = sizeScreen/ pathology + 5f * test;
		if (pathology != 1) {
			if (scale == 1) {
				aux = Mathf.Clamp (aux, sizeScreen / pathology + (int)(5f * zero), sizeScreen);
			} else {
				aux = Mathf.Clamp (aux, 0, sizeScreen / pathology + (int)(5f * zero));
			}
		}
		//texture.SetPixel(indiceActual % sizeScreen, (int)aux, Color.white);

		texture.SetPixel (indiceActual % sizeScreen, sizeScreen / pathology + (int)(5f * zero), Color.white);

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
			scale += 0.1f * n;
			scale = Mathf.Clamp (scale, 0, 2);
		}
	}

	public void GraphGain(int n){
		if (Mathf.Abs (n) == 1) {
			gain += 0.1f * n;
			gain = Mathf.Clamp (gain, 0, 1);
		}
	}

	public void GraphSpeed(int n){
		if (Mathf.Abs (n) == 1) {
			//speedAux += 0.1f * n;
			speed = Mathf.Clamp(speed + 0.1f*n,0,0.9f);
		}
	}

	public void GraphPower(int n){
		if (Mathf.Abs (n) == 1) {
			power += 0.1f * n;
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
