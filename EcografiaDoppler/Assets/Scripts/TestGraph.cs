using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestGraph : MonoBehaviour {
	int sizeScreen = 128;
	Texture2D texture;
	int indiceActual = 0;
	float zero = 0;
	public float zeroScale;
	public float scale = 1;
	public float gain = 1;
	public float depth = 1;
	public float power = 1;
	float depthAux = 0;
	public float[] red = new float[3];
	float redAux = 0;
	public float[] blue = new float[3];
	float blueAux = 0;
	void Start() {
		texture = new Texture2D(sizeScreen, sizeScreen);
		GetComponent<Renderer>().material.mainTexture = texture;
		Color.RGBToHSV (Color.red, out red[0], out red[1], out red[2]);
		redAux = red [0];
		Color.RGBToHSV (Color.blue, out blue[0], out blue[1], out blue[2]);
		blueAux = blue [0];
	}

	void Update(){
		for (int i = 0; i < sizeScreen; i++) {
			texture.SetPixel(indiceActual % sizeScreen, i, Color.black);
		}
		float aux = 0;

		//float f0 = Mathf.Sin (1.333f * indiceActual * 360f / sizeScreen * Mathf.PI / 180f);
		float test = scale*(depth*Mathf.Sin (0.5f * indiceActual * 360f / sizeScreen * Mathf.PI/180f * Mathf.PI/0.5f - 0.8f + depth*Mathf.Sin(0.5f*indiceActual* 360f / sizeScreen * Mathf.PI/180f * Mathf.PI/0.5f))* Mathf.Log (indiceActual) + 2f + zero);
		if (power <= 0) {
			test = 0;
		}
		for (int i = 0; i < Random.Range (5, 15); i++) {
			aux = (gain * Random.Range (-20, -1)) + sizeScreen / 2 + (5f * test);
			aux = Mathf.Clamp (aux, sizeScreen/2 + (int)(5f*zero), sizeScreen);

			texture.SetPixel(indiceActual % sizeScreen, (int)aux, Color.HSVToRGB(blue[0],blue[1],blue[2]));
		}
		for (int i = 0; i < Random.Range (5, 10); i++) {
			aux = (gain * Random.Range (-40, -1)) + sizeScreen / 2 + (5f * test);
			aux = Mathf.Clamp (aux, sizeScreen/2 + (int)(5f*zero), sizeScreen);
			texture.SetPixel(indiceActual % sizeScreen, (int)aux, Color.HSVToRGB(red[0],red[1],red[2]));
		}
		aux = sizeScreen/ 2 + 5f * test;
		aux = Mathf.Clamp (aux, sizeScreen/2 + (int)(5f*zero), sizeScreen);
		texture.SetPixel(indiceActual % sizeScreen, (int)aux, Color.white);

		texture.SetPixel (indiceActual % sizeScreen, sizeScreen / 2 + (int)(5f * zero), Color.white);

		texture.Apply();
		indiceActual++;
	}

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
			gain = Mathf.Clamp (gain, 0, 2);
		}
	}

	public void GraphDepth(int n){
		if (Mathf.Abs (n) == 1) {
			depthAux += 0.1f * n;
			depth = 1 - Mathf.Clamp (Mathf.Abs(depthAux), 0, 1);
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
}
