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
		float f0 = Mathf.Sin (1.333f * indiceActual * 360f / sizeScreen * Mathf.PI / 180f);
		for (int i = 0; i < Random.Range (5, 15); i++) {
			texture.SetPixel(indiceActual % sizeScreen, Random.Range(-20, -1) + sizeScreen / 2 + (int)(10f * Mathf.Sin(1.333f * indiceActual * 360f / sizeScreen  * Mathf.PI / 180f)), Color.blue);
		}
		for (int i = 0; i < Random.Range (5, 10); i++) {
			texture.SetPixel(indiceActual % sizeScreen, Random.Range(-40, -1) + sizeScreen / 2 + (int)(10f * Mathf.Sin(1.333f * indiceActual * 360f / sizeScreen  * Mathf.PI / 180f)), Color.red);
		}
		texture.SetPixel(indiceActual % sizeScreen, sizeScreen / 2 + (int)(10f * f0), Color.white);
		texture.Apply();
		indiceActual++;
	}
}
