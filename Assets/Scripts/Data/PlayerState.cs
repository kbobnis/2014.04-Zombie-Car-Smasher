using UnityEngine;
using System.Collections;

public class PlayerState  {

	public int Coins;
	public int Exp;
	public CarConfig CarConfig;

	public int Level{
		get { return Mathf.FloorToInt( Exp / 10) + 1; }
	}


}
