using System.Collections;
using UnityEngine;

public enum AnalyticsScreen {
	GameClassic, GameAdv
}


public class GoogleAnalyticsKProjekt {

	private static string LastLogged = "";
	private static bool IsActive = true;

	public static void LogScreenOnce(AnalyticsScreen screen){

		if (GoogleAnalytics.instance && screen.ToString() != LastLogged ) {
			if (IsActive){
				LastLogged = screen.ToString();
				LogScreen(screen.ToString());
			} else {
				Debug.Log("Can not log screen " + screen + " application inactive");
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
