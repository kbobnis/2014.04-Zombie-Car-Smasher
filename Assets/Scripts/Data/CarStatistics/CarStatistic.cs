using System.Collections.Generic;
using UnityEngine;

public class CarStatistic
{
	public CarStatisticType Type;
	private List<Dependency> Dependencies = new List<Dependency> ();
	private int _Level;
	private float ManuallySetValue = -1;

	public CarStatistic(CarStatisticType type){
		Type = type;
		Level = 1;
	}

	public int Level{
		set { 
			_Level = value; 
			if (Level < 1) {
				throw new UnityException("You can not have level lower than 1: " + Level);
			}
		}
		get { return _Level; }
	}

	public float Value{
		get { return ManuallySetValue!=-1?ManuallySetValue:Type.ValueForLevel (_Level); }
		set { ManuallySetValue = value; }
	}


	public bool CanUpgrade(int coins){
		bool canAfford = Type.UpgradeCost(Level+1) < coins;

		bool dependenciesOk = true;
		foreach (Dependency dependency in Dependencies) {
			if (!dependency.IsMet()){
				dependenciesOk = false;
			}
		}
		return canAfford && dependenciesOk;
	}

	public int UpgradeCost(){
		return Type.UpgradeCost (Level + 1);
	}

	public string TopText(){
		return "Upgrade " + Type.Name();
	}

	public void AddDependency(Dependency dependency){
		Dependencies.Add (dependency);
	}

	protected string NotAffordText(){
		return "You need " + UpgradeCost() +" coins to upgrade this";
	}

	public void Upgrade (){
		Level++;
	}

	public void Downgrade(int howManyLevels){ 
		Level -= howManyLevels;
	}

	public string Serialize(){
		Dictionary<string, string> dict = new Dictionary<string, string> ();
		dict ["Type"] = Type.ToString ();
		dict ["Level"] = "" + Level;
		return MiniJSON.Json.Serialize(dict);
	}

	public static CarStatistic Deserialize(string serialized){
		Dictionary<string, object> dict = (Dictionary<string, object>)MiniJSON.Json.Deserialize(serialized);
		CarStatistic cs = null;
		if (dict.ContainsKey ("Type")) {
			CarStatisticType type = (CarStatisticType)System.Enum.Parse(typeof(CarStatisticType), (string)dict["Type"]);
			cs = new CarStatistic(type);
			cs.Level = int.Parse((string) dict["Level"]);
		}
		return cs;
	}

	public string Info(bool canAffordUpgrade){
		
		string text = Type.Description()+"\n\n";
		bool dependenciesOk = true;
		string dependencyText = "";
		foreach (Dependency dependency in Dependencies) {
			if (!dependency.IsMet()){
				dependenciesOk = false;
				dependencyText = dependency.FailText;
			}
		}

		if (canAffordUpgrade && dependenciesOk){
			text += Type.UpgradeText();
		} else if (!dependenciesOk){
			text += dependencyText;
		} else if (!canAffordUpgrade){
			text += NotAffordText();
		}


		text = text.Replace("{value}", ""+string.Format("{0:0.00}", Value));
		text = text.Replace("{valueAfterUpgrade}", ""+string.Format("{0:0.00}", Type.ValueForLevel(Level+1)));
		text = text.Replace("{upgradeCost}", ""+UpgradeCost());
		return text;
	}
}

