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
    int variation = 0;
	int ciclo = 0;

	public int size = 17;
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

    public void resetZero()
    {
        if (variation < 0)
        {
            for (int i = 0; i < Mathf.Abs(variation); i++)
            {
                lowerVerticalPosition();
            }
        }
        if (variation > 0)
        {
            for (int i = 0; i < variation; i++)
            {
                upperVerticalPosition();
            }
        }
    }

    public void adjustZero(int adjust)
    {
        variation = adjust;
        //changeVerticalPosition(-variation);
        if(adjust < 0)
        {
            for(int i = 0; i < Mathf.Abs(adjust); i++)
            {
                upperVerticalPosition();
            }
        }
        if(adjust > 0)
        {
            for (int i = 0; i < adjust; i++)
            {
                lowerVerticalPosition();
            }
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
		verticalStep*=2f;
	}
	public void scaleDownVertical(){
		//changeVerticalScale (-10);
		verticalStep/=2f;
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
		ciclo++;
		if (ciclo % 20 > 0)
			return;
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
		for(int i = 0; i <= size; i++){
            if (verticalPosition - i == 0) verticalLabel.text += "[333333]CM/SEC[-]          ";
            //verticalLabel.text += "" + (Mathf.Round((verticalPosition - i)*2 * (verticalScale/verticalStep))).ToString() + "\n\n";
			verticalLabel.text += ""+ (Mathf.Round(verticalPosition - i)*20).ToString()+"\n\n";
		}

		horizontalLabel.text = "";
		for (float i = 0; i <= 20; i++) {
			horizontalLabel.text += (i/2).ToString() + horizontalSpace;
		}
		//horizontalLabel.text = "0" + horizontalSpace + "1" + horizontalSpace + "2" + horizontalSpace + "3" + horizontalSpace + "4" + horizontalSpace + "5" + horizontalSpace + "6";
	}

	public int offset = 0;
	public void adjustVerticalSize(float vScale){
		//offset = (int)Mathf.RoundToInt((10f*(Mathf.Abs(0.5f - vScale))));
		if(vScale == 0.5f) offset = 0;
		if(vScale == 0.6f) offset = 2;
		if(vScale == 0.7f) offset = 3;
		if(vScale == 0.8f) offset = 4;
		if(vScale == 0.9f) offset = 4;
		if(vScale == 1f) offset = 5;



		//size = 17 - Mathf.Min(offset,3);
		//verticalLabel.spacingY = 1 + (int)offset;
		//verticalPosition = 9 - Mathf.Min(offset,3);
		
	}
}
