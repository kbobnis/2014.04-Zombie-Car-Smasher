using UnityEngine;

public enum CarStatisticParam{
	Value, Level
}

public class Dependency {
	
	private CarStatisticParam Param;
	private SIGN Sign;
	private CarStatistic ThisStatistic, OtherStatistic;
	
	public Dependency(CarStatistic otherStatistic, CarStatisticParam param, SIGN sign, CarStatistic thisStatistic){
		Param = param;
		Sign = sign;
		OtherStatistic = otherStatistic;
		ThisStatistic = thisStatistic;
	}
	
	public bool IsMet(){
		bool isMet = false;
		switch (Param) {
		case CarStatisticParam.Level:
			isMet = Sign.IsMet(OtherStatistic.Level, ThisStatistic.Level );
			break;
		case CarStatisticParam.Value:
			isMet = Sign.IsMet(OtherStatistic.Value, ThisStatistic.Value);
			break;
		default: 
			throw new UnityException("There is no param: " + Param);
		}
		return isMet;
	}
	
	public string FailText{
		get { 
			string text = "";
			switch(Param){
				case CarStatisticParam.Value:
					text = OtherStatistic.Type.Name() + " value ("+OtherStatistic.Value+")" + " must be " + Sign.Text() + " than " + ThisStatistic.Type.Name() + " value ("+ThisStatistic.Value+")" ;
					break;
				case CarStatisticParam.Level:
					text = OtherStatistic.Type.Name() + " level ("+OtherStatistic.Level+")" + " must be " + Sign.Text() + " than " + ThisStatistic.Type.Name() + " level ("+ThisStatistic.Level+")" ;
					break;
				default:
					throw new UnityException("There is no param: " + Param);
			}
			return text;
		} 
	}
}