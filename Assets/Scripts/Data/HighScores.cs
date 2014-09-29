using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum HighScoreType{
	Adventure, Classic
}
public class HighScores  {

	static private Dictionary<HighScoreType,  List<int>> Scores = new Dictionary<HighScoreType, List<int>> (); 
	static private int TrimScores = 100;
	static private int Version = 1;

	static HighScores(){
		LoadScores ();
	}

	public static int GetTopScoresCount(){
		return Scores.Count;
	}

	public static List<int> GetTopScores(int howMany, HighScoreType hst){
		howMany = Scores.Count > howMany ? howMany : Scores.Count;
		return Scores[hst].GetRange (0, howMany);
	}

	public static void AddScore(int score, HighScoreType hst){
		List<int> scores = Scores [hst];
		scores.Add (score);
		scores.Sort ();
		scores.Reverse ();
		if (scores.Count > TrimScores) {
			scores.RemoveRange (TrimScores, Scores.Count-TrimScores);
		}

		SaveScores ();
	}

	public static int GetPlaceFor(int score){
		for (int i=0; i < Scores.Count; i++) {
			if (score >= Scores[i]){
				return i+1;
			}
		}
		return 0;
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
