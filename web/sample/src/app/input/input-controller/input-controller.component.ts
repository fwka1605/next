import { Component, OnInit } from '@angular/core';
import { ConstantPool } from '@angular/compiler';
import { FormGroup, FormControl } from '@angular/forms';
import { stringify } from '@angular/compiler/src/util';
import { FormatterUtil } from './formatter.util';

@Component({
  selector: 'app-input-controller',
  templateUrl: './input-controller.component.html',
  styleUrls: ['./input-controller.component.css']
})
export class InputControllerComponent implements OnInit {

  private myFormGroup: FormGroup;

  private Data1Ctrl: FormControl;
  private Data2Ctrl: FormControl;
  private Data3Ctrl: FormControl;
  private Data4Ctrl: FormControl;
  private Data5Ctrl: FormControl;

  private UndefineCtrl: FormControl;

  constructor() { }

  ngOnInit() {
    this.setControlInit();
    this.setValidator();
    this.clear();

    let value3: string;

    FormatterUtil.setNumberFormatter(this.Data1Ctrl);
    FormatterUtil.setNumberFormatter(this.Data2Ctrl);
    FormatterUtil.setNumberFormatter(this.Data3Ctrl);
    /*
    this.Data3Ctrl.valueChanges.subscribe(
      (value: string) => {
        if (value.match(/[^0-9,]/)) {
          this.Data3Ctrl.setValue(value.replace(/[^0-9,]/g, ''));
        }
      }
    );
    */
  }

  private setControlInit() {
    this.Data1Ctrl = new FormControl();
    this.Data2Ctrl = new FormControl();
    this.Data3Ctrl = new FormControl();
    this.Data4Ctrl = new FormControl();
    this.Data5Ctrl = new FormControl();

    this.UndefineCtrl = new FormControl();

  }

  private setValidator() {
    this.MyFormGroup = new FormGroup({
      Data1Ctrl: this.Data1Ctrl,
      Data2Ctrl: this.Data2Ctrl,
      Data3Ctrl: this.Data3Ctrl,
      Data4Ctrl: this.Data4Ctrl,
      Data5Ctrl: this.Data5Ctrl,

      UndefineCtrl: this.UndefineCtrl,

    });
  }

  private clear() {
    this.myFormGroup.reset();
  }

  public get MyFormGroup(): FormGroup {
    return this.myFormGroup;
  }
  public set MyFormGroup(myFormGroup: FormGroup) {
    this.myFormGroup = myFormGroup;
  }

  private alphaData: string = "";

  public maskNumberForKeyup(event: any) {

    let key = event.which

    if ((key >= 48 && key <= 57) || key === 8 || key === 44) {

    }
    else {
      event.preventDefault();
    }
  }

  public maskNumberForInput(formContrl: FormControl) {
    formContrl.setValue(formContrl.value.replace(/[^0-9,]/g, ''));
  }


}
