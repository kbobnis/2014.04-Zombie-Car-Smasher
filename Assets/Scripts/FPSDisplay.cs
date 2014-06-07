﻿using UnityEngine;
using System.Collections;

public class FPSDisplay : MonoBehaviour {

	float timeA;
	public int fps;
	public int lastFPS;
	// Use this for initialization
	void Start () {
		timeA = Time.timeSinceLevelLoad;
		DontDestroyOnLoad (this);
	}
	
	// Update is called once per frame
	void Update () {
		if (Time.timeSinceLevelLoad - timeA <= 1) {
			fps ++;
		} else {
			lastFPS = fps + 1;
			timeA = Time.timeSinceLevelLoad;
			fps = 0;
		}
	}

	void OnGUI(){
		GUI.Label (new Rect (0, 0, Screen.width, Screen.height), "FPS: " + lastFPS);
	}
}
