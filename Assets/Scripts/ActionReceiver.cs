using UnityEngine;
using System.Collections.Generic;

public enum Fate
{
	FELL_INTO_HOLE,
	CRASHED_INTO_WALL
}


public class ActionReceiver : MonoBehaviour
{
	public Dictionary<Fate, bool> Fates = new Dictionary<Fate, bool>();

	public void FellIntoHole(){
		if (!Fates.ContainsKey(Fate.FELL_INTO_HOLE)){
			Fates.Add(Fate.FELL_INTO_HOLE, true);
		}
	}

	public void CrashedIntoWall(){
		if (!Fates.ContainsKey(Fate.CRASHED_INTO_WALL)){
			Fates.Add(Fate.CRASHED_INTO_WALL, true);
		}
	}

}

