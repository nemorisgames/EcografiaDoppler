using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorPoint : MonoBehaviour {
	public bool pivot;
	public Texture2D hitTexture;
    public UITexture durezasUITexture;
    public Texture2D durezasTexture;
    public float durezasAngle = 0f;
    MovieTexture movTexture;
	ControladorGrafica cg;
	public ControladorCursores cc;
	public bool movie = true;
	public Camera renderCam;

	// Use this for initialization
	void Start ()
    {
        durezasTexture = (Texture2D)durezasUITexture.mainTexture;
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

    public void changePosition(Transform t)
    {
        cc = GameObject.FindGameObjectWithTag("CursorController").GetComponent<ControladorCursores>();
        transform.position = t.position;
        switch (t.name)
        {
            case "0":
                cc.changeImages(0);
                break;
            case "45":
                cc.changeImages(1);
                break;
            case "90":
                cc.changeImages(2);
                break;
            case "-45":
                cc.changeImages(3);
                break;
            case "-90":
                cc.changeImages(4);
                break;
        }
    }

	void GetPixelColor(){
		RaycastHit hit;
		if (Physics.Raycast (transform.position, Vector3.forward, out hit)) {
			if (!movie) {
				hitTexture = hit.transform.gameObject.GetComponentInChildren<UITexture> ().mainTexture as Texture2D;
			} else if (renderCam != null) {
				movTexture = hit.transform.gameObject.GetComponentInChildren<UITexture> ().mainTexture as MovieTexture;
				RenderTexture rt = RenderTexture.GetTemporary (movTexture.width, movTexture.height, 24);
				renderCam.targetTexture = rt;
				renderCam.Render ();
				RenderTexture.active = rt;
				hitTexture = new Texture2D (rt.width, rt.height);
				hitTexture.ReadPixels (new Rect (0, 0, rt.width, rt.height), 0, 0);
				hitTexture.Apply ();
			} else
				return;
			Vector2 hitPoint = hit.textureCoord;
			hitPoint.x *= hitTexture.width;
			hitPoint.y *= hitTexture.height;
			Color c = hitTexture.GetPixel ((int)hitPoint.x, (int)hitPoint.y);
            Color cDurezas = durezasTexture.GetPixel((int)hitPoint.x, (int)hitPoint.y);
            Debug.Log ("RGB: " + c.r + "," + c.g + "," + c.b);
			if (c.r > 0.6f && c.b < 0.5f) {
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
            durezasAngle = cDurezas.g * 90f + 90f;

        }
	}
}
