using UnityEngine;
using System.Collections;

public class MainLogic : MonoBehaviour {

	// Use this for initialization
	void Start () {
		GetComponent<SplashScreen>().ShowSplash();
	}
	
	// Update is called once per frame
	void Update () {
	}

	public void WantToStartGame(){
		GetComponent<Minigame>().StartRace();
	}





}
