using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum HighScoreType{
	Adventure, Classic
}
public static class HighScoreTypeMethods{
	public static string KeyName(this HighScoreType hst, int place){
		string prefix = "HighScores";
		switch(hst){
			case HighScoreType.Classic: return prefix + "1" + place;
			default: 
				return prefix + hst.ToString() + place;
		}
			
	}
}
public class HighScores  {

	static private Dictionary<HighScoreType,  List<int>> Scores = new Dictionary<HighScoreType, List<int>> (); 
	static private int TrimScores = 100;

	static HighScores(){

		Scores[HighScoreType.Adventure] =  LoadScores (HighScoreType.Adventure);
		Scores[HighScoreType.Classic] =  LoadScores (HighScoreType.Classic);
	}

	public static int GetTopScoresCount(){
		return Scores.Count;
	}

	public static List<int> GetTopScores(int howMany, HighScoreType hst){
		howMany = Scores[hst].Count > howMany ? howMany : Scores[hst].Count;
		return Scores[hst].GetRange (0, howMany);
	}
	public static int GetTopScore(HighScoreType hst){
		return Scores [hst].Count > 0 ? Scores [hst] [0] : 0;
	}

	public static int GetTopScoreAll(){
		int top = 0;
		foreach (KeyValuePair<HighScoreType, List<int>> kvp in Scores) {
			int tmpTop = GetTopScore( kvp.Key );
			if (tmpTop > top){
				top = tmpTop;
			}
		}
		return top;
	}

	public static void AddScore(int score, HighScoreType hst){
		List<int> scores = Scores [hst];
		scores.Add (score);
		scores.Sort ();
		scores.Reverse ();
		if (scores.Count > TrimScores) {
			scores.RemoveRange (TrimScores, Scores.Count-TrimScores);
		}

		SaveScores (hst);
	}

	public static int GetPlaceFor(int score, HighScoreType hst){
		for (int i=0; i < Scores[hst].Count; i++) {
			if (score >= Scores[hst][i]){
				return i+1;
			}
		}
		return 0;
	}

	private static List<int> LoadScores(HighScoreType hst){
		int i = 0; 
		int lastScore = 0;
		List<int> tmp = new List<int> ();
		while ((lastScore = PlayerPrefs.GetInt (hst.KeyName(++i))) != 0) {
			tmp.Add(lastScore);
		};
		tmp.Sort ();
		tmp.Reverse ();
		return tmp;
	}

	private static void SaveScores(HighScoreType hst){
		int i = 0; 
		foreach(int score2 in Scores[hst]){
			PlayerPrefs.SetInt(hst.KeyName(++i), score2);
		}


	}
}
