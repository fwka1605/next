import { Component, OnInit } from '@angular/core';

import { FormGroup, FormControl,  Validators } from '@angular/forms';
import { CustomValidators } from 'ng5-validation';

@Component({
  selector: 'app-ng5-validation',
  templateUrl: './ng5-validation.component.html',
  styleUrls: ['./ng5-validation.component.css']
})
export class Ng5ValidationComponent implements OnInit {

    private myForm:FormGroup;

    constructor() { }

  
    show() {
    }
  
    ngOnInit() {

        this.setValidator();

    }

    private setValidator(){
        let requireCheck = new FormControl('',Validators.required);
        let minLengthCheck= new FormControl('',Validators.minLength(3));
        let maxLengthCheck= new FormControl('',Validators.maxLength(5));
        let patternCheck= new FormControl('',Validators.pattern(new RegExp('ab+c')));
      
        let rangeLengthCheck = new FormControl('',CustomValidators.rangeLength([5, 9]));
        let minCheck = new FormControl('',CustomValidators.min(10));
        let gtCheck = new FormControl('',CustomValidators.gt(10));
        let gteCheck = new FormControl('',CustomValidators.gte(10));
        let maxCheck = new FormControl('',CustomValidators.max(20));
        let ltCheck = new FormControl('',CustomValidators.lt(20));
        let lteCheck = new FormControl('',CustomValidators.lte(20));
        let rangeCheck = new FormControl('',CustomValidators.range([10, 20]));
        let digitsCheck = new FormControl('',CustomValidators.digits);
        let numberCheck = new FormControl('',CustomValidators.number);
        let urlCheck = new FormControl('',CustomValidators.url);
        let emailCheck = new FormControl('',CustomValidators.email);
        let dateCheck = new FormControl('',CustomValidators.date);
        let minDateCheck = new FormControl('',CustomValidators.minDate("2016-09-09"));
        let maxDateCheck = new FormControl('',CustomValidators.maxDate("2016-09-09"));
        let dateISOCheck = new FormControl('',CustomValidators.dateISO);
        let creditCardCheck = new FormControl('',CustomValidators.creditCard);
        let jsonCheck = new FormControl('',CustomValidators.json);
        let base64Check = new FormControl('',CustomValidators.base64);
        let phoneCheck = new FormControl('',CustomValidators.phone("CN"));
        let uuidCheck = new FormControl('',CustomValidators.uuid("all"));
    
        let equalCheck = new FormControl('',CustomValidators.equal("aaa"));
        let notEqualCheck = new FormControl('',CustomValidators.notEqual('aaa'));
        let equalToCheck = new FormControl('',CustomValidators.equalTo(emailCheck));
        let notEqualToCheck = new FormControl('',CustomValidators.notEqualTo(emailCheck));

       this.myForm  = new FormGroup({
            requireCheck:requireCheck,
            minLengthCheck:minLengthCheck,
            maxLengthCheck:maxLengthCheck,
            patternCheck:patternCheck,
            rangeLengthCheck:rangeLengthCheck,
            minCheck:minCheck,
            gtCheck:gtCheck,
            gteCheck:gteCheck,
            maxCheck:maxCheck,
            ltCheck:ltCheck,
            lteCheck:lteCheck,
            rangeCheck:rangeCheck,
            digitsCheck:digitsCheck,
            numberCheck:numberCheck,
            urlCheck:urlCheck,
            emailCheck:emailCheck,
            dateCheck:dateCheck,
            minDateCheck:minDateCheck,
            maxDateCheck:maxDateCheck,
            dateISOCheck:dateISOCheck,
            creditCardCheck:creditCardCheck,
            jsonCheck:jsonCheck,
            base64Check:base64Check,
            phoneCheck:phoneCheck,
            uuidCheck:uuidCheck,
            equalCheck:equalCheck,
            notEqualCheck:notEqualCheck,
            equalToCheck:equalToCheck,
            notEqualToCheck:notEqualToCheck, 
        });            
    }
}

