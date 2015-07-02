using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using UnityEngine;


class IAd:MonoBehaviour
{
    public enum iAdType { Banner, Interstitial }
    [DllImport("__Internal")]
    private static extern void loadBanner();
    [DllImport("__Internal")]
    private static extern bool loadInterstitial();
    [DllImport("__Internal")]
    private static extern void hideBanner();
    [DllImport("__Internal")]
    private static extern void hideInterstitial();
    public iAdType type;
    void Start()
    {
#if UNITY_IOS && !UNITY_EDITOR
        if (type == iAdType.Banner && AdController.bannerInventor == AdController.ADInventor.iad)
            loadBanner();
        else if (AdController.interstitialInventor == AdController.ADInventor.iad)
        {
            bool canShow = loadInterstitial();
            if (!canShow)
            {
                AdController.interstitialInventor = AdController.ADInventor.admob;
                gameObject.SendMessage("RestartAdmob");
            }
        }
#endif
    }

    void OnEnable()
    {
        Start();
    }

    void OnDisable()
    {
#if UNITY_IOS && !UNITY_EDITOR
        if (type == iAdType.Banner)
            hideBanner();
        else
            hideInterstitial();
#endif
    }

    public void OnAdError()
	{
        AdController.bannerInventor = AdController.ADInventor.admob;
        gameObject.SendMessage("RestartAdmob");
	}

    public void OnInterstitialError()
    {
        AdController.interstitialInventor = AdController.ADInventor.admob;
        gameObject.SendMessage("RestartAdmob");
    }
}
