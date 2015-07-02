//
//  TestiAd
//
//  Created by ¡ı“„ on 15/6/28.
//  Copyright (c) 2015ƒÍ ¡ı“„. All rights reserved.
//

#import <UIKit/UIKit.h>
#import <iAd/iAd.h>

@interface Interstitial : UIViewController<ADInterstitialAdDelegate>
{
    ADInterstitialAd *interstitial;
}
+(Interstitial*)getInstance;
-(BOOL) ShowInterstitial;
-(void) HideInterstitial;

@end

