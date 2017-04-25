using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LabelIndicators : MonoBehaviour {
	public UILabel verticalLabel;
	public UILabel horizontalLabel;
	public int verticalScale = 0;
	public int verticalPosition = 1;
	public int horizontalScale = 0;
	// Use this for initialization
	void Start (){
		verticalLabel.text = "";
		for(int i = 0; i <= 17; i++){
			verticalLabel.text += "" + ((verticalPosition - i) * 100) + "\n\n"; 
		}
	}

	public void changeVerticalPosition(int v){
		verticalPosition += v;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
