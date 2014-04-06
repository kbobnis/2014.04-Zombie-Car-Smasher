using UnityEngine;
using System.Collections;

public class MainLogic : MonoBehaviour {

	// Use this for initialization
	void Start () {
		WantsToShowSplash();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.Escape)){
			Application.Quit(); 
		}
	}

	public void WantToStartGame(){
		GetComponent<Minigame>().PrepareRace() ;
	}

	public void WantsToShowSplash(){
		GetComponent<Minigame>().UnloadResources();
		GetComponent<SplashScreen>().ShowSplash();
	}





}
