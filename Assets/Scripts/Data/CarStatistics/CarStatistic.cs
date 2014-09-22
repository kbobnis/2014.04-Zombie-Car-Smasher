using System.Collections.Generic;

abstract public class CarStatistic
{
	protected int Level;
	protected float _Value;

	abstract public string TopText();
	abstract protected string BuyText ();
	abstract protected void InnerUpgrade ();
	abstract public int UpgradeCost();
	abstract protected string Description();

	public CarStatistic(){
		Level = 1;
	}

	public float Value{
		get { return _Value; }
	}

	virtual public void SetValue(float v){
		_Value = v;
	}

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
		SetValue( float.Parse( (string)list [1]) );
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

