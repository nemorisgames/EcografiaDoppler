﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Login : MonoBehaviour {

	public UIInput[] inputs;
	public UILabel error;
	int active = -1;

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
}