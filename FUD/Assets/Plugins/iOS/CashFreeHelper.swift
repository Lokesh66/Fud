//
//  CashFreeSample.swift
//  CashFreeHelper
//
//  Created by Prashamsa on 22/08/20.
//  Copyright © 2020 Sample. All rights reserved.
//

import UIKit
import CFSDK

@objc public protocol CashFreeHelperDelegate: NSObjectProtocol {
    @objc func didReceiveResponse(response:String)
}

@objc public enum Environment: NSInteger {
    case production
    case test
}

@objc public class CashFreeHelper: NSObject, ResultDelegate {
    fileprivate(set) var appID = ""
    fileprivate(set) var environment: Environment = .test
    @objc public weak var delegate: CashFreeHelperDelegate? = nil
    
    @objc public init(appID: String, environment: Environment) {
        super.init()
        self.appID = appID
        self.environment = environment
    }
    
    @objc public func processPayment(options: [String: String]) {
        let rootViewController = PaymentProcessingViewController(nibName: "PaymentProcessingViewController", bundle: Bundle(for: type(of: self)))
        let navigationViewController = UINavigationController(rootViewController: rootViewController)
        if let window = UIApplication.shared.windows.first {
            window.rootViewController?.present(navigationViewController, animated: true, completion: {
                var env = "TEST"
                if self.environment == .production {
                    env = "PROD"
                }
                let cfViewController = CFViewController(params: options, appId: self.appID, env: env, callBack: self)
                navigationViewController.pushViewController(cfViewController, animated: true)
            })
        }
        
    }
    
    public func onPaymentCompletion(msg: String) {
        delegate?.didReceiveResponse(response: msg)
    }
}
