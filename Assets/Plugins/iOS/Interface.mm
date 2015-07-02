//	
//  Interface.cpp
//  Unity-iPhone
//
//  Created by 刘毅 on 15/6/29.
//
//
#import "BannerView.h"
#import "Interstitial.h"

extern "C"
{
    void loadBanner()
    {
        [[BannerView getInstance]ShowBanner];
    }
	
	void hideBanner()
	{
		[[BannerView getInstance]HideBanner];
	}
    
    BOOL loadInterstitial()
    {
		if(UI_USER_INTERFACE_IDIOM() == UIUserInterfaceIdiomPad)
			return [[Interstitial getInstance]ShowInterstitial];
		else
			return false;
    }
	
	void hideInterstitial()
	{
		if(UI_USER_INTERFACE_IDIOM() == UIUserInterfaceIdiomPad)
			[[Interstitial getInstance]HideInterstitial];
	}
    
}
