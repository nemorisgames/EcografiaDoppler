using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LabelIndicators : MonoBehaviour {
	public UILabel verticalLabel;
	public UILabel horizontalLabel;
	public float verticalScale = 100;
	public int verticalPosition = 1;
	public int horizontalScale = 52;
	float horizontalStringLength;
	string horizontalSpace = "";
	float verticalStep = 1f;
	// Use this for initialization
	void Start (){
		changeVerticalPosition (0);
		BuildString (horizontalScale);
		horizontalStringLength = horizontalSpace.Length;
	}

	void BuildString(int length){
		horizontalSpace = "";
		for (int i = 0; i < length; i++) {
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
		//changeVerticalScale (10);
		verticalStep*=1.5f;
	}
	public void scaleDownVertical(){
		//changeVerticalScale (-10);
		verticalStep/=1.5f;
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
			horizontalStringLength *= 2f;
			float aux = Mathf.Clamp(horizontalStringLength,horizontalScale,horizontalScale*Mathf.Pow(2f,2));
			BuildString ((int)aux);
		} else {
			/*float aux = horizontalSpace.Length - horizontalSpace.Length / 2;
			aux = Mathf.Clamp (aux, horizontalScale * Mathf.Pow (0.5f, 2), horizontalScale);
			horizontalSpace = horizontalSpace.Substring (0, (int)aux);*/
			/*horizontalStringLength /= 1.8f;
			BuildString ((int)horizontalStringLength);*/
			horizontalStringLength /= 2f;
			float aux = Mathf.Clamp(horizontalStringLength,horizontalScale*Mathf.Pow(2f,-2),horizontalScale);
			BuildString ((int)aux);
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
			verticalLabel.text += "" + (Mathf.Round((verticalPosition - i) * (verticalScale/verticalStep))).ToString() + "\n\n"; 
		}

		horizontalLabel.text = "";
		for (float i = 0; i <= 20; i++) {
			horizontalLabel.text += (i/2).ToString() + horizontalSpace;
		}
		//horizontalLabel.text = "0" + horizontalSpace + "1" + horizontalSpace + "2" + horizontalSpace + "3" + horizontalSpace + "4" + horizontalSpace + "5" + horizontalSpace + "6";
	}
}
