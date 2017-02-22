using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestGraph : MonoBehaviour {
	int sizeScreen = 128;
	Texture2D texture;
	int indiceActual = 0;
	void Start() {
		texture = new Texture2D(sizeScreen, sizeScreen);
		GetComponent<Renderer>().material.mainTexture = texture;
	}

	void Update(){
		for (int i = 0; i < sizeScreen; i++) {
			texture.SetPixel(indiceActual % sizeScreen, i, Color.black);
		}
		float aux = 1f;
		//float f0 = Mathf.Sin (1.333f * indiceActual * 360f / sizeScreen * Mathf.PI / 180f);
		float test = aux*(Mathf.Sin (0.5f * indiceActual * 360f / sizeScreen * Mathf.PI/180f * Mathf.PI/0.5f - 0.8f + Mathf.Sin(0.5f*indiceActual* 360f / sizeScreen * Mathf.PI/180f * Mathf.PI/0.5f))* Mathf.Log (indiceActual) + 1.8f);
		for (int i = 0; i < Random.Range (5, 15); i++) {
			texture.SetPixel(indiceActual % sizeScreen, Random.Range(-20, -1) + sizeScreen / 2 + (int)(5f * test), Color.blue);
		}
		for (int i = 0; i < Random.Range (5, 10); i++) {
			texture.SetPixel(indiceActual % sizeScreen, Random.Range(-40, -1) + sizeScreen / 2 + (int)(5f * test), Color.red);
		}
		texture.SetPixel(indiceActual % sizeScreen, sizeScreen / 2 + (int)(5f * test), Color.white);
		texture.Apply();
		indiceActual++;
	}
}
