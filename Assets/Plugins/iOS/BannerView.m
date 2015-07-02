//
//  ViewController.m
//  TestiAd
//
//  Created by 刘毅 on 15/6/28.
//  Copyright (c) 2015年 刘毅. All rights reserved.
//

#import "BannerView.h"

@interface BannerView ()

@end

@implementation BannerView

static BannerView* _instant;
+(BannerView*)getInstance
{
    if(_instant == nil)
        _instant = [[BannerView alloc]init];
    return _instant;
}

-(void)ShowBanner
{
    [[[UIApplication sharedApplication]keyWindow]addSubview:self.view];
}
-(void)HideBanner
{
    [self.view removeFromSuperview];
}
- (void)viewDidLoad {
    [super viewDidLoad];
    // Do any additional setup after loading the view, typically from a nib.
    self.view.frame = CGRectMake(0, [[UIScreen mainScreen] bounds].size.height-50, 320, 50);
    
    // On iOS 6 ADBannerView introduces a new initializer, use it when available.
    if ([ADBannerView instancesRespondToSelector:@selector(initWithAdType:)]) {
        bannerView = [[ADBannerView alloc] initWithAdType:ADAdTypeBanner];
    } else {
        bannerView = [[ADBannerView alloc] init];
    }
    bannerView.frame = CGRectMake(0, 0, 320, 50);
    bannerView.delegate = self;
    [bannerView setBackgroundColor:[UIColor clearColor]];
    [self.view addSubview:bannerView];
    NSLog(@"viewDidLoad");
}

- (void)didReceiveMemoryWarning {
    [super didReceiveMemoryWarning];
    // Dispose of any resources that can be recreated.
}

#pragma mark -AdViewDelegates
-(void)bannerViewDidLoadAd:(ADBannerView *)banner
{
    NSLog(@"Ad loaded");
}

-(void)bannerViewWillLoadAd:(ADBannerView *)banner
{
    NSLog(@"Ad will load");
}

-(void)bannerViewActionDidFinish:(ADBannerView *)banner
{
    NSLog(@"Ad did finish");
}

-(void)bannerView:(ADBannerView *)banner didFailToReceiveAdWithError:(NSError *)error
{
    NSLog(@"didFailToReceiveAdWithError %@", error);
    UnitySendMessage("Canvas", "OnAdError", "");
	UnitySendMessage("MainCanvas", "OnAdError", "");
}
@end
