using System.Collections.Generic;

abstract public class CarStatistic
{
	public int Level;
	public float Value;

	abstract public string TopText();
	abstract protected string BuyText ();
	abstract protected void InnerUpgrade ();
	abstract public int UpgradeCost();
	abstract protected string Description();

	protected string NotAffordText(){
		return "You need " + UpgradeCost() +" coins to upgrade this";
	}

	public void Upgrade (){
		Level++;
		InnerUpgrade ();
	}

	public string Serialize(){
		return MiniJSON.Json.Serialize( new List<string>(){ ""+Level, ""+Value} );
	}

	public void Deserialize(string serialized){
		List<object> list = (List<object>)MiniJSON.Json.Deserialize(serialized);
		Level = int.Parse( (string)list[0] );
		Value = float.Parse( (string)list [1]);
	}

	public string Info(bool canAffordUpgrade){
		
		string text = Description()+"\n";
		if (canAffordUpgrade){
			text += BuyText();
		} else {
			text += NotAffordText();
		}
		return text;
	}



}

