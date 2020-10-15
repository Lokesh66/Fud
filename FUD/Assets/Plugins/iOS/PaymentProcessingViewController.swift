//
//  PaymentProcessingViewController.swift
//  CashFreeHelper
//
//  Created by Prashamsa on 22/08/20.
//  Copyright © 2020 Sample. All rights reserved.
//

import UIKit

class PaymentProcessingViewController: UIViewController {

    override func viewDidLoad() {
        super.viewDidLoad()

        // Do any additional setup after loading the view.
    }
    
    override func viewDidAppear(_ animated: Bool) {
        super.viewDidAppear(animated)
        
        if isMovingToParent == false {
            self.navigationController?.dismiss(animated: true, completion: nil)
        }
    }
}
