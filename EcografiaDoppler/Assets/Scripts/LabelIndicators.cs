using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LabelIndicators : MonoBehaviour {
	public UILabel verticalLabel;
	public UILabel horizontalLabel;
	public int verticalScale = 100;
	public int verticalPosition = 1;
	public int horizontalScale = 36;
	string horizontalSpace = "";
	// Use this for initialization
	void Start (){
		changeVerticalPosition (0);
		for (int i = 0; i < horizontalScale; i++) {
			horizontalSpace += " ";
		}
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

	public void scaleUpHorizontal(){
		changeHorizontalScale (-1);
	}

	public void scaleDownHorizontal(){
		changeHorizontalScale (1);
	}

	public void changeHorizontalScale(int i){
		i = Mathf.Clamp (i, -1, 1);
		if (i > 0) {
			horizontalSpace += horizontalSpace.Substring(0,horizontalSpace.Length/4);
		} else {
			horizontalSpace = horizontalSpace.Substring (0, 3*horizontalSpace.Length/4);
		}
	}
	
	// Update is called once per frame
	void Update () {
		/*if (Input.GetKeyDown (KeyCode.W)) {
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
		}*/


		verticalLabel.text = "";
		for(int i = 0; i <= 17; i++){
			verticalLabel.text += "" + ((verticalPosition - i) * verticalScale) + "\n\n"; 
		}

		horizontalLabel.text = "";
		for (int i = 0; i <= 8; i++) {
			horizontalLabel.text += i.ToString() + horizontalSpace;
		}
		//horizontalLabel.text = "0" + horizontalSpace + "1" + horizontalSpace + "2" + horizontalSpace + "3" + horizontalSpace + "4" + horizontalSpace + "5" + horizontalSpace + "6";
	}
}
