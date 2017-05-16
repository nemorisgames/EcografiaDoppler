using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControladorCursores : MonoBehaviour {

	public Transform cursorP1;
	public Transform cursorP2;
	float angle;
	ControladorGrafica controller;

	// Use this for initialization
	void Start () {
		if (cursorP1 == null)
			cursorP1 = GameObject.Find ("cursorP1").GetComponent<Transform>();
		if (cursorP2 == null)
			cursorP2 = GameObject.Find ("cursorP2").GetComponent<Transform>();
		if (controller == null)
			controller = GameObject.FindGameObjectWithTag ("GameController").GetComponent<ControladorGrafica> ();
	}
	
	// Update is called once per frame
	void Update () {
		angle = AngleBetweenAnB (cursorP1.position, cursorP2.position);
	}

	float AngleBetweenAnB(Vector2 a, Vector2 b){
		Vector2 dif = b - a;
		float sign = (b.y < a.y) ? -1f : 1f;
		return Vector2.Angle (Vector2.right, dif) * sign;
	}

	public float CursorAngleValue(){
		float aux = Mathf.Abs (angle);
		if (aux < 90f) {
			aux = (90f - aux) / 90f;
		} else if (aux > 90f) {
			aux = 1f - ((180f - aux) / 90f);
		} else
			aux = 0f;
		return aux;
	}


}
