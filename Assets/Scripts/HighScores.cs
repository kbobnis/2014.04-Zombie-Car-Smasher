using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HighScores  {

	static private List<int> Scores = new List<int>(); 
	static private int TrimScores = 100;
	static private int Version = 1;

	static HighScores(){
		LoadScores ();
	}


	public static List<int> GetTopScores(int howMany){
		howMany = Scores.Count > howMany ? howMany : Scores.Count;
		return Scores.GetRange (0, howMany);
	}

	public static void AddScore(int score){
		Scores.Add (score);
		Scores.Sort ();
		Scores.Reverse ();
		if (Scores.Count > TrimScores) {
			Scores.RemoveRange (TrimScores, Scores.Count-TrimScores);
		}

		SaveScores ();
	}

	private static void LoadScores(){
		int i = 0; 
		int lastScore = 0;
		while ((lastScore = PlayerPrefs.GetInt ("HighScores" + Version + ++i)) != 0) {
			Scores.Add(lastScore);
		};
		Scores.Sort ();
		Scores.Reverse ();

	}

	private static void SaveScores(){
		int i = 0; 
		foreach(int score2 in Scores){
			PlayerPrefs.SetInt("HighScores" + Version + ++i, score2);
		}

	}
}
