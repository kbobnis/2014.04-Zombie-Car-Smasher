﻿using System;
using UnityEngine;
using GoogleMobileAds;
using GoogleMobileAds.Api;

public class GoogleMobileAdsKProjekt : MonoBehaviour {

	private BannerView bannerView;

	// Use this for initialization
	void Start () {
		RequestBanner ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void ShowBanner()
	{
		bannerView.Show ();
	}

	public void HideBanner()
	{
		bannerView.Hide ();
	}

	private void RequestBanner()
	{
		#if UNITY_EDITOR
		string adUnitId = "unused";
		#elif UNITY_ANDROID
		string adUnitId = "ca-app-pub-1834171387831407/7841823179";
		#elif UNITY_IPHONE
		string adUnitId = "INSERT_IOS_BANNER_AD_UNIT_ID_HERE";
		#else
		string adUnitId = "unexpected_platform";
		#endif
		
		// Create a 320x50 banner at the top of the screen.
		bannerView = new BannerView(adUnitId, AdSize.SmartBanner, AdPosition.Top);
		// Register for ad events.
		//bannerView.AdLoaded += HandleAdLoaded;
		//bannerView.AdFailedToLoad += HandleAdFailedToLoad;
		//bannerView.AdOpened += HandleAdOpened;
		//bannerView.AdClosing += HandleAdClosing;
		//bannerView.AdClosed += HandleAdClosed;
		//bannerView.AdLeftApplication += HandleAdLeftApplication;
		// Load a banner ad.
		bannerView.LoadAd(CreateAdRequest());
		bannerView.Show();
	}

	// Returns an ad request with custom ad targeting.
	private AdRequest CreateAdRequest()
	{
		return new AdRequest.Builder ()
			.AddTestDevice (AdRequest.TestDeviceSimulator)
				.AddTestDevice ("6F40308545B64E7C558974067F213E12")
				.AddTestDevice("1E02B70CE2CB41B6A7B3E674ACB07BA8")
				.AddTestDevice("AE27A3312978A75326F52FE0E4D65736")
				.Build ();
	}
}
