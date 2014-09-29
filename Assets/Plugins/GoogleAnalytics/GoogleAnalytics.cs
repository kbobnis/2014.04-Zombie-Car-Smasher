using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;


public class GoogleAnalytics : MonoBehaviour {
	
	public string propertyID;
	
	public static GoogleAnalytics instance;
	
	public string bundleID;
	public string appName;
	public string appVersion;
	public bool test = true;

	private string screenRes;
	private string clientID;

	void Awake()
	{
		if(instance)
			DestroyImmediate(gameObject);
		else
		{
			DontDestroyOnLoad(gameObject);
			instance = this;
		}
	}

	public static string GetUniqueID(){
		string key = "uniqueId";
		
		string uniqueId = "";
		if(PlayerPrefs.HasKey(key)){
			uniqueId = PlayerPrefs.GetString(key);
		} else {
			uniqueId = System.Guid.NewGuid ().ToString();
			if (Application.platform == RuntimePlatform.WindowsEditor){
				uniqueId = "windowsEditor";
			}
			PlayerPrefs.SetString(key, uniqueId);
			PlayerPrefs.Save();
		}

		Debug.Log("Generated Unique ID: "+uniqueId);
		
		return uniqueId;
	}
	
	void Start() 
	{
		screenRes = Screen.width + "x" + Screen.height;
		clientID = GetUniqueID ();
	}

	public void LogScreen(string title)
	{
		if (test) {
			Debug.Log("this is a test: " + title);
		} else {
			title = WWW.EscapeURL(title);
			var url = "http://www.google-analytics.com/collect?v=1&ul=en-us&t=appview&sr="+screenRes+"&an="+WWW.EscapeURL(appName)+"&a=448166238&tid="+propertyID+"&aid="+bundleID+"&cid="+WWW.EscapeURL(clientID)+"&_u=.sB&av="+appVersion+"&_v=ma1b3&cd="+title+"&qt=2500&z=185";
			WWW request = new WWW(url);
		}
	}
	
	
}