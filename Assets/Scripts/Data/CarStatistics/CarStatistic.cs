using System.Collections.Generic;
using UnityEngine;

public class CarStatistic
{
	public CarStatisticType Type;
	private List<Dependency> _Dependencies = new List<Dependency> ();
	private int _Level;
	private float ManuallySetValue = -1;

	public CarStatistic(CarStatisticType type){
		Type = type;
		Level = 1;
	}

	public List<Dependency> Dependencies {
		get { return _Dependencies; } 
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

	public string ValueFormatted{
		get { return string.Format("{0:0.00}", Value); }
	}


	public bool CanUpgrade(int coins){
		bool canAfford = Type.UpgradeCost(Level+1) <= coins;

		bool dependenciesOk = true;
		foreach (Dependency dependency in _Dependencies) {
			if (!dependency.IsMet()){
				dependenciesOk = false;
			}
		}

		bool aboveMinimum = Type.AboveMinimum ( Type.ValueForLevel(Level + 1));

		return canAfford && dependenciesOk && aboveMinimum;
	}

	public int UpgradeCost(){
		return Type.UpgradeCost (Level + 1);
	}

	public void AddDependency(Dependency dependency){
		_Dependencies.Add (dependency);
	}

	public void Upgrade (){
		Level++;
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

}

