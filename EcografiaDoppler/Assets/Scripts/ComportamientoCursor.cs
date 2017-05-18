using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComportamientoCursor : MonoBehaviour {
    public Texture2D durezas;
    public Vector2 sizeDurezas;
	// Use this for initialization
	void Start () {
		
	}
    
    public void clickOnImage()
    {
        Vector2 posicionClick = new Vector2((Input.mousePosition - Camera.main.WorldToScreenPoint(transform.position)).x + sizeDurezas.x/2f, (Input.mousePosition - Camera.main.WorldToScreenPoint(transform.position)).y + sizeDurezas.y / 2f);
        print(posicionClick);
    }

    void OnMouseDown()
    {
        print("aqui");
    }

    // Update is called once per frame
    void Update () {
        
    }
}
