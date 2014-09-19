using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerState  {

	public int Coins;
	public int Exp;
	public CarConfig CarConfig;
	private Dictionary<string, bool> MissionsDone = new Dictionary<string, bool>();

	public int Level{
		get { return Mathf.FloorToInt( Exp / 10) + 1; }
	}

	public void MissionDone(Mission m){
		if (!MissionsDone.ContainsKey (m.Id)) {
			MissionsDone.Add (m.Id, true);	
		}
	}

	public bool IsMissionDone(Mission m){
		return MissionsDone.ContainsKey(m.Id);
	}

}
