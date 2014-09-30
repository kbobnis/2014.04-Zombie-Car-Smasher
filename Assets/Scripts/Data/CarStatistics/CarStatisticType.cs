using UnityEngine;

public enum CarStatisticType{
	SHIELD, COMBUSTION, FUEL_TANK, STARTING_OIL, WHEEL
}

static public class CarStatisticTypeMethods{
	
	public static string Name(this CarStatisticType type){
		switch (type) {
		case CarStatisticType.COMBUSTION: return "Combustion";
		case CarStatisticType.FUEL_TANK: return "Fuel tank";
		case CarStatisticType.SHIELD: return "Shield";
		case CarStatisticType.WHEEL: return "Wheel";
		case CarStatisticType.STARTING_OIL: return "Starting Oil";
		default:
			throw new UnityException("there is no type: " + type);
		}
	}
	
	public static string Description(this CarStatisticType type){
		switch(type){
		case CarStatisticType.COMBUSTION: return "Combustion determines how much oil will be used to drive one distance.";
		case CarStatisticType.FUEL_TANK: return "Fuel tank lets you gather more fuel at a time.";
		case CarStatisticType.SHIELD: return "Shield destroys an obstacle. Shields regenerate after each race.";
		case CarStatisticType.WHEEL: return "Wheel determines how fast the car is changing lanes.";
		case CarStatisticType.STARTING_OIL: return "Starting oil determines how much oil is in the car at start of the race.";
		default:
			throw new UnityException("there is no type: " + type);
		}
	}
	
	public static string UpgradeText(this CarStatisticType type){
		switch(type){
		case CarStatisticType.COMBUSTION: return "Do you want to increase combustion value from {value} to {valueAfterUpgrade} for {upgradeCost} coins?";
		case CarStatisticType.FUEL_TANK: return "Do you want to increase fuel tank from {value} to {valueAfterUpgrade} for {upgradeCost} coins?";
		case CarStatisticType.SHIELD: return "Do you want to increase shield from {value} to {valueAfterUpgrade} for {upgradeCost} coins?";
		case CarStatisticType.WHEEL: return "Do you want to increase wheel from {value} to {valueAfterUpgrade} for {upgradeCost} coins?";
		case CarStatisticType.STARTING_OIL: return "Do you want to increase starting oil value from {value} to {valueAfterUpgrade} for {upgradeCost} coins?";
		default:
			throw new UnityException("there is no type: " + type);
		}
	}

	public static float StartingValue(this CarStatisticType type){
		switch(type){
		case CarStatisticType.COMBUSTION: return 1.5f;
		case CarStatisticType.FUEL_TANK: return 50;
		case CarStatisticType.SHIELD: return 0;
		case CarStatisticType.STARTING_OIL: return 35;
		case CarStatisticType.WHEEL: return 0.75f;
		default:
			throw new UnityException("there is no type: " + type);
		}
	}

	public static float ValueForLevel(this CarStatisticType type, float _level){
		float level = _level - 1; // because we start from level 1 not zero
		switch(type){
		case CarStatisticType.COMBUSTION: return type.StartingValue() - (level * 0.05f) ;
		case CarStatisticType.FUEL_TANK: return type.StartingValue() + (level * 1);
		case CarStatisticType.SHIELD: return type.StartingValue() + (level * 1);
		case CarStatisticType.WHEEL: return type.StartingValue() + (level * 0.15f);
		case CarStatisticType.STARTING_OIL: return type.StartingValue() + (level * 1);
		default:
			throw new UnityException("there is no type: " + type);
		}
	}
	
	public static int UpgradeCost(this CarStatisticType type, int level){
		switch(type){
		case CarStatisticType.COMBUSTION: return 10 + level * level;
		case CarStatisticType.FUEL_TANK: return Mathf.RoundToInt( type.ValueForLevel(level) / 5 );
		case CarStatisticType.SHIELD: return 100 + level * level * level;
		case CarStatisticType.WHEEL: return 15 * level ;
		case CarStatisticType.STARTING_OIL: return 1 * level;
		default: 
			throw new UnityException("there is no type: " + type);
		}
	}

	public static bool AboveMinimum(this CarStatisticType type, float value){
		switch(type){
		case CarStatisticType.COMBUSTION: return value > 0.45;
		case CarStatisticType.FUEL_TANK: return true;
		case CarStatisticType.SHIELD: return true;
		case CarStatisticType.WHEEL: return value < 2.5;
		case CarStatisticType.STARTING_OIL: return true;
		default: 
			throw new UnityException("there is no type: " + type);
		}
	}

}
