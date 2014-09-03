using System.Collections;
using UnityEngine;

public class GoogleAnalyticsKProjekt {

	private static string LastLogged = "";
	private static bool IsActive = true;
	private const string SCREEN_INACTIVE = "Screen inactive";

	public static void LogScreenOnce(string name){
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
		if (GoogleAnalytics.instance && isActive != IsActive ) {
			IsActive = isActive;

			if (IsActive){
				LogScreen(LastLogged);
			} else {
				LogScreen(GoogleAnalyticsKProjekt.SCREEN_INACTIVE);
			}
		}
	}
}
