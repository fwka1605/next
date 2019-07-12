import { Component, OnInit } from '@angular/core';
import { ValidatorManager } from 'ng-form-validator';

@Component({
  selector: 'app-ng-form-validator',
  templateUrl: './ng-form-validator.component.html',
  styleUrls: ['./ng-form-validator.component.css']
})
export class NgFormValidatorComponent implements OnInit {

    public validatorManager: ValidatorManager = null;
 
    constructor(validator: ValidatorManager) {
      this.validatorManager = validator;
    }
   
    public runValidation() {
      this.validatorManager.Validate();
    }
    
    ngOnInit() {
        this.validatorManager.Validate();
    }


}
