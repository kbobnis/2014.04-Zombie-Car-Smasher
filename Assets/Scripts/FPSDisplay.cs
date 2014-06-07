using UnityEngine;
using System.Collections;

public class FPSDisplay : MonoBehaviour {

	static float timeA;
	static public int fps;
	static public int lastFPS;
	// Use this for initialization
	void Start () {
		timeA = Time.timeSinceLevelLoad;
		DontDestroyOnLoad (this);
	}
	
	// Update is called once per frame
	void Update () {
		FPSDisplay.UpdateFrom ("followGM");
	}

	void OnGUI(){
		GUI.Label (new Rect (0, 0, Screen.width, Screen.height), "FPS: " + lastFPS);
	}

	public static void UpdateFrom(string what){
		if (Time.timeSinceLevelLoad - timeA <= 1) {
			fps ++;
		} else {
			lastFPS = fps + 1;
			timeA = Time.timeSinceLevelLoad;
			fps = 0;
		}
	}
}
