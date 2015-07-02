//
//  ViewController.h
//  TestiAd
//
//  Created by 刘毅 on 15/6/28.
//  Copyright (c) 2015年 刘毅. All rights reserved.
//

#import <UIKit/UIKit.h>
#import <iAd/iAd.h>

@interface BannerView : UIViewController<ADBannerViewDelegate>
{
    ADBannerView *bannerView;
}
+(BannerView*)getInstance;
-(void) ShowBanner;
-(void) HideBanner;

@end

