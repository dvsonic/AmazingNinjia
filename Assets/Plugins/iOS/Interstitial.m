//
//  ViewController.m
//  TestiAd
//
//  Created by ¡ı“„ on 15/6/28.
//  Copyright (c) 2015ƒÍ ¡ı“„. All rights reserved.
//

#import "Interstitial.h"

@interface Interstitial ()
-(void)cycleInterstitial;
@end

@implementation Interstitial

static Interstitial* _instant2;
+(Interstitial*)getInstance
{
    if(_instant2 == nil)
        _instant2 = [[Interstitial alloc]init];
    return _instant2;
}

-(BOOL)ShowInterstitial
{
    [self cycleInterstitial];
    
    return true;
}
-(void)HideInterstitial
{
    [self.view removeFromSuperview];
}

- (void)cycleInterstitial
{
    // Release the old interstial and create a new one.
    interstitial.delegate = nil;
    [interstitial release];
    interstitial = [[ADInterstitialAd alloc] init];
    interstitial.delegate = self;
    NSLog(@"cycleInterstitial");
}

- (void)viewDidLoad {
    [super viewDidLoad];
    // Do any additional setup after loading the view, typically from a nib.
    NSLog(@"viewDidLoad");
}

- (void)didReceiveMemoryWarning {
    [super didReceiveMemoryWarning];
    // Dispose of any resources that can be recreated.
}

-(void)interstitialAd:(ADInterstitialAd *)interstitialAd didFailWithError:(NSError *)error
{
    NSLog(@"interstitial error");
    interstitial.delegate = nil;
    [interstitial release];
	[self.view removeFromSuperview];
    UnitySendMessage("Canvas", "OnInterstitialError", "");
    UnitySendMessage("ResultCanvas", "OnInterstitialError", "");
}
-(void)interstitialAdActionDidFinish:(ADInterstitialAd *)interstitialAd
{
    [self.view removeFromSuperview];
    NSLog(@"interstitialAdActionDidFinish");
}

-(void)interstitialAdDidLoad:(ADInterstitialAd *)interstitialAd
{
    [interstitial presentInView:self.view];
	[[[UIApplication sharedApplication]keyWindow]addSubview:self.view];
    NSLog(@"interstitialAdDidLoad");
}
-(void)interstitialAdDidUnload:(ADInterstitialAd *)interstitialAd
{
	[self.view removeFromSuperview];
    NSLog(@"interstitialAdDidUnload");
}
-(void)interstitialAdWillLoad:(ADInterstitialAd *)interstitialAd
{
    NSLog(@"interstitialAdWillLoad");
}
@end
