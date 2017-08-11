using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class NavigationScenes : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}

    public void goCerebral()
    {
        SceneManager.LoadScene("EcografiaCerebral");
    }

    public void goDuctus()
    {
        SceneManager.LoadScene("EcografiaDuctus");
    }

    public void goUtero()
    {
        SceneManager.LoadScene("EcografiaUtero");
    }

    public void goUmbilical()
    {
        SceneManager.LoadScene("EcografiaUmbilical");
    }

    public void exit()
    {
        Application.Quit();
    }

    public void home()
    {
        SceneManager.LoadScene("Login");
    }
    // Update is called once per frame
    void Update () {
		
	}
}
