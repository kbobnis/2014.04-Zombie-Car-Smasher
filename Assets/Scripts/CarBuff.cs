using UnityEngine;

public abstract class CarBuff : MonoBehaviour
{

	public abstract void DoAction();
	public abstract bool CanDoAction();
	public abstract string GetActionName();
}

