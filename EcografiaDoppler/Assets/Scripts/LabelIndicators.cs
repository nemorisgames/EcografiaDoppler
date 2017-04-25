using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LabelIndicators : MonoBehaviour {
	public UILabel verticalLabel;
	public UILabel horizontalLabel;
	public int verticalScale = 100;
	public int verticalPosition = 1;
	public int horizontalScale = 0;
	// Use this for initialization
	void Start (){
		changeVerticalPosition (0);
	}

	public void lowerVerticalPosition(){
		changeVerticalPosition (-1);
	}
	public void upperVerticalPosition(){
		changeVerticalPosition (1);
	}

	public void changeVerticalPosition(int v){
		verticalPosition += v;
	}


	public void scaleUpVertical(){
		changeVerticalScale (10);
	}
	public void scaleDownVertical(){
		changeVerticalScale (-10);
	}
	public void changeVerticalScale(int i){
		verticalScale += i;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.W)) {
			changeVerticalPosition (1);
		}
		if (Input.GetKeyDown (KeyCode.S)) {
			changeVerticalPosition (-1);
		}
		if (Input.GetKeyDown (KeyCode.A)) {
			verticalScale -= 10;
		}
		if (Input.GetKeyDown (KeyCode.D)) {
			verticalScale += 10;
		}


		verticalLabel.text = "";
		for(int i = 0; i <= 17; i++){
			verticalLabel.text += "" + ((verticalPosition - i) * verticalScale) + "\n\n"; 
		}
	}
}
