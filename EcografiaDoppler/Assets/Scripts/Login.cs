using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Login : MonoBehaviour {

	public UIInput[] inputs;
	public UILabel error;
	int active = -1;
	public bool withPass = false;

	void Start(){
		/*if (!withPass) {
			StartCoroutine (countdownStart (3f));
		}*/
	}

	// Update is called once per frame
	void Update () {
		for (int i = 0; i < inputs.Length; i++) {
			if (inputs [i].isSelected) {
				active = i;
			}
		}
		if (Input.GetKeyDown (KeyCode.Tab)) {
			NextInput ();
		}

		if (Input.GetKeyDown (KeyCode.Escape)) {
			Application.Quit ();
		}
	}

	public void NextInput(){
		if (active < inputs.Length - 1) {
			active += 1;
			inputs [active].isSelected = true;
		} else if (active == inputs.Length -1) {
			inputs [active].RemoveFocus ();
			active = -1;
		}
	}

	public void Submit(){
		if (inputs [0].value != "" && inputs [1].value != "") {
			SceneManager.LoadScene (1);
		} else {
			if (inputs [0].value == "") {
				error.text = "Please enter your username.";
			}
			else if (inputs [1].value == "") {
				error.text = "Please enter your password.";
			}
		}
	}

    public void cargarUmbilical()
    {
        SceneManager.LoadScene("EcografiaUmbilical");
    }

    public void cargarCorazon()
    {
        SceneManager.LoadScene("EcografiaDuctus");
    }

    public void cargarCerebral()
    {
        SceneManager.LoadScene("EcografiaCerebral");
    }

    public void cargarUtero()
    {
        SceneManager.LoadScene("EcografiaUtero");
    }

    IEnumerator countdownStart(float s){
		yield return new WaitForSeconds (s);
		SceneManager.LoadScene (1);
	}
}
