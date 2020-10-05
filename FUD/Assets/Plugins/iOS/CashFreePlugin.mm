//
//  CashFreePlugin.m
//  CashFreeSample
//
//  Created by Prashamsa on 27/08/20.
//  Copyright © 2020 Sample. All rights reserved.
//

#import <Foundation/Foundation.h>
#import "CashFreeHelper/CashFreeHelper.h"
#import "CashFreeHelper/CashFreeHelper-Swift.h"

typedef void (*PaymentCompletionHandler)(const char*);

@interface CashFreePlugin : NSObject <CashFreeHelperDelegate> {
    NSString *_appID;
    CashFreeHelper *_helper;
    PaymentCompletionHandler completionHandler;
    Environment _env;
}

@end

@implementation CashFreePlugin

static CashFreePlugin *_sharedInstance;

+(CashFreePlugin*)sharedInstance {
    static dispatch_once_t onceToken;
    dispatch_once(&onceToken, ^{
        _sharedInstance = [[CashFreePlugin alloc] init];
    });
    return _sharedInstance;
}

-(void)initializeCashFreeHelper:(NSString *)appID envronment: (Environment)env {
    _appID = appID;
    _helper = [[CashFreeHelper alloc] initWithAppID:appID environment:env];
    [_helper setDelegate:self];
}

- (void)processPayment:(NSString *)paymentInfo completion:(PaymentCompletionHandler)completion {
    completionHandler = completion;
    NSError *jsonError = nil;
    NSData *objectData = [paymentInfo dataUsingEncoding:NSUTF8StringEncoding];
    NSDictionary *json = [NSJSONSerialization JSONObjectWithData:objectData
                                          options:NSJSONReadingMutableContainers
                                            error:&jsonError];
    if (jsonError != nil) {
        completionHandler([[jsonError localizedDescription] cStringUsingEncoding:NSUTF8StringEncoding]);
        return;
    }
    [_helper processPaymentWithOptions:json];
    
}

- (void)didReceiveResponseWithResponse:(NSString *)response {
    completionHandler([response cStringUsingEncoding:NSUTF8StringEncoding]);
}

@end


extern "C"
{
    enum mode {
        test,
        production
    };

    void initializeWithAppID(const char *appID, mode mode = test) {
        NSString *aID = [NSString stringWithCString:appID encoding:NSUTF8StringEncoding];
        if (mode == production) {
            [[CashFreePlugin sharedInstance] initializeCashFreeHelper:aID envronment:EnvironmentProduction];
        } else {
            [[CashFreePlugin sharedInstance] initializeCashFreeHelper:aID envronment:EnvironmentTest];
        }
    }
    
    void processPayment(const char *paymentInfo, PaymentCompletionHandler completion) {
        NSString *pInfo = [NSString stringWithCString:paymentInfo encoding:NSUTF8StringEncoding];
        [[CashFreePlugin sharedInstance] processPayment:pInfo completion:completion];
    }
}
