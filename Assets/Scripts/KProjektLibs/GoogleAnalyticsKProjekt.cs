using System.Collections;
using UnityEngine;

public enum ANALYTICS_SCREENS {
	GAME, FAIL, SPLASH
}

public class GoogleAnalyticsKProjekt {

	private static string LastLogged = "";
	private static bool IsActive = true;

	public static void LogScreenOnce(ANALYTICS_SCREENS screen){

		string name = "";
		switch (screen) {
			case ANALYTICS_SCREENS.GAME: 
				name = "Screen game";
				break;
			case ANALYTICS_SCREENS.FAIL: 
				name = "Screen fail";
				break;
			case ANALYTICS_SCREENS.SPLASH:
				name = "Screen splash";
				break;
		}
		if (GoogleAnalytics.instance && name != LastLogged ) {
			if (IsActive){
				LastLogged = name;
				LogScreen(name);
			} else {
				Debug.Log("Can not log screen " + name + " application inactive");
			}
		}
	}

	public static void LogScreen(string name){
		if (GoogleAnalytics.instance) {
			GoogleAnalytics.instance.LogScreen (name);
			Debug.Log("logged screen: " + name);
		}
	}

	public static void LogIsActive(bool isActive){
		return; // i dont want screen inactive
	}
}
