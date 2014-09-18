using UnityEngine;

public abstract class CarBuff : MonoBehaviour
{

	public abstract void DoAction();
	public abstract bool CanDoAction();
	public abstract string GetActionName();

	void OnGUI(){
		if (false && CanDoAction() && GUI.RepeatButton(new Rect(GuiHelper.oneThirdW, Screen.height -GuiHelper.twentyPercent, GuiHelper.oneThirdW, GuiHelper.twentyPercent), GetActionName()) ){
			DoAction();
		}
	}

}

