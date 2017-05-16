using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorPoint : MonoBehaviour {
	public bool pivot;
	Texture2D hitTexture;
	ControladorGrafica cg;
	ControladorCursores cc;

	// Use this for initialization
	void Start () {
		cg = GameObject.FindGameObjectWithTag ("GameController").GetComponent<ControladorGrafica> ();
		cc = GameObject.FindGameObjectWithTag ("CursorController").GetComponent<ControladorCursores> ();
		GetPixelColor ();
	}
	
	// Update is called once per frame
	void Update () {
		//Debug.DrawRay (transform.position, Vector3.forward,Color.red);
	}

	void OnDragEnd(){
		if (pivot) {
			GetPixelColor ();
		}
		cg.angle = cc.CursorAngleValue ();
	}

	void GetPixelColor(){
		RaycastHit hit;
		if (Physics.Raycast (transform.position, Vector3.forward, out hit)) {
			hitTexture = hit.transform.gameObject.GetComponentInChildren<UITexture> ().mainTexture as Texture2D;
			Vector2 hitPoint = hit.textureCoord;
			hitPoint.x *= hitTexture.width;
			hitPoint.y *= hitTexture.height;
			Color c = hitTexture.GetPixel ((int)hitPoint.x, (int)hitPoint.y);
			Debug.Log ("RGB: "+c.r+","+c.g+","+c.b);
			if (c.r > 0.6f && c.b < 0.5f ) {
				Debug.Log ("R");
				cg.GraphCursorNull (true);
				cg.GraphTIC (1);
			} else if (c.b > 0.6f && c.r < 0.5f) {
				Debug.Log ("B");
				cg.GraphCursorNull (true);
				cg.GraphTIC (-1);
			} else {
				cg.GraphCursorNull (false);
				Debug.Log ("N/A");
			}
		}
	}
}
