using UnityEngine;
using System.Collections.Generic;

public enum Fate
{
	FELL_INTO_HOLE,
	CRASHED_INTO_WALL
}


public class ActionReceiver : MonoBehaviour
{
	public List<Fate> Fates = new List<Fate>();

	public void FellIntoHole(){
		Fates.Add(Fate.FELL_INTO_HOLE);
	}

	public void CrashedIntoWall(){
		Fates.Add(Fate.CRASHED_INTO_WALL);
	}

}

