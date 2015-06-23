using UnityEngine;
using System.Collections;
using GoogleMobileAds.Api;
using GoogleMobileAds.Common;

public class ADMobStatic
{
	// Use this for initialization
    private static BannerView banner;
    private static InterstitialAd interstitial;
    private const string banerUnitId = "ca-app-pub-1392235012379391/5435918069";
    private const string interstitialId = "ca-app-pub-1392235012379391/8389384463";
    private static AdRequest bannerReq = null;
    private static AdRequest interstitialReq = null;
    public static void Init()
    {
        banner = new BannerView(
           banerUnitId, AdSize.Banner, AdPosition.Bottom);
        interstitial = new InterstitialAd(interstitialId);
        interstitialReq = new AdRequest.Builder().Build();
        interstitial.LoadAd(interstitialReq);
    }

	public static void ShowBanner()
    {
        if (null == bannerReq)
        {
            bannerReq = new AdRequest.Builder().Build();
            banner.LoadAd(bannerReq);
        }
        else
        {
            banner.Show();
        }
    }

    public static void ShowInterstitial()
    {
        if(interstitial.IsLoaded())
            interstitial.Show();
    }

    public static void Hide()
    {
        banner.Hide();
    }
}
