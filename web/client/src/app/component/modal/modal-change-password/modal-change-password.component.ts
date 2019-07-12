import { Component, OnInit, EventEmitter, ComponentFactoryResolver, ViewContainerRef, ElementRef } from '@angular/core';
import { BaseComponent } from '../../common/base/base-component';
import { MODAL_STATUS_TYPE, PROCESS_RESULT_RESULT_TYPE } from 'src/app/common/const/status.const';
import { ProcessResultService } from 'src/app/service/common/process-result.service';
import { FormControl, Validators, FormGroup } from '@angular/forms';
import { FormatterUtil } from 'src/app/common/util/formatter.util';
import { CustomValidators } from 'ng5-validation';
import { LoginParameters } from 'src/app/model/login-parameters.model';
import { COMPANY_CODE, COMPANY_ID } from 'src/app/common/const/company.const';
import { LoginService } from 'src/app/service/Master/login.service';
import { BUTTON_ACTION } from 'src/app/common/const/event.const';
import { HtmlUtil } from 'src/app/common/util/html.util';
import { PasswordPolicyMasterService } from 'src/app/service/Master/password-policy-master.service';
import { UserInfoService } from 'src/app/service/common/user-info.service';
import { WebApiLoginResult } from 'src/app/model/web-api-login-result.model';
import { MSG_ERR, MSG_ITEM_NUM, MSG_WNG } from 'src/app/common/const/message.const';

@Component({
  selector: 'app-modal-change-password',
  templateUrl: './modal-change-password.component.html',
  styleUrls: ['./modal-change-password.component.css']
})
export class ModalChangePasswordComponent extends BaseComponent implements OnInit {
  /** ユーザーコード */
  public userCodeCtrl:FormControl;
  /** 古いパスワード */
  public oldPassWordCtrl:FormControl;
  /** パスワード */
  public passWordCtrl: FormControl;
  /** パスワード（確認） */
  public passWordConfirmationCtrl: FormControl;

  constructor(
    public componentFactoryResolver: ComponentFactoryResolver,
    public viewContainerRef: ViewContainerRef,
    public elementRef: ElementRef,
    public processResultService: ProcessResultService,
    public loginService: LoginService,
    private passwordPolicyService: PasswordPolicyMasterService,
    public userInfoService: UserInfoService,
  ) {
    super();
  }

  ngOnInit() {
    // setControlInit
    this.userCodeCtrl = new FormControl(this.loginUserCode);
    this.oldPassWordCtrl = new FormControl(this.oldPassword);
    this.passWordCtrl = new FormControl("", [Validators.required]);
    this.passWordConfirmationCtrl = new FormControl("", [CustomValidators.equalTo(this.passWordCtrl)]);

    // setValidator
    this.MyFormGroup = new FormGroup({
      userCodeCtrl: this.userCodeCtrl,
      oldPassWordCtrl: this.oldPassWordCtrl,
      passWordCtrl: this.passWordCtrl,
      passWordConfirmationCtrl: this.passWordConfirmationCtrl,
    });

    // setFormatter
    FormatterUtil.setCodeFormatter(this.userCodeCtrl);
    FormatterUtil.setCodeFormatter(this.oldPassWordCtrl);
    FormatterUtil.setCodeFormatter(this.passWordCtrl);
    FormatterUtil.setCodeFormatter(this.passWordConfirmationCtrl);

    if (this.userInfoService.AccessToken == null || this.userInfoService.AccessToken.length == 0) {
      let userCode: string = this.loginUserCode;
      let userPassword: string = this.oldPassword;
      if (this.loginUserCode.length == 0) userCode = this.userCodeCtrl.value;
      if (this.oldPassword.length == 0) userCode = this.oldPassWordCtrl.value;
      if (userCode.length == 0 || userPassword.length == 0) {
        this.processModalCustomResult = this.processResultService.processAtWarning(
          this.processModalCustomResult, MSG_WNG.INPUT_REQUIRED.replace(MSG_ITEM_NUM.FIRST, 'ユーザーID、またはパスワード'),
          this.partsResultMessageComponent);
          return;
      }
      // アクセストークンの取得
      this.loginService.Login(COMPANY_CODE, userCode, userPassword)
        .subscribe(response => {
          const loginResult = response as WebApiLoginResult;

          if (response == PROCESS_RESULT_RESULT_TYPE.FAILURE || loginResult == undefined) {
            this.processModalCustomResult = this.processResultService.processAtFailure(
              this.processModalCustomResult, MSG_ERR.SOMETHING_ERROR.replace(MSG_ITEM_NUM.FIRST, '認証'),
              this.partsResultMessageComponent);
            return;
          }

          this.userInfoService.AccessToken = loginResult.sessionKey;
          this.getPasswordPolicy();
        });
    } else {
      if (this.loginService.passwordPolicy == null) {
        this.getPasswordPolicy();
      }
    }
  }

  /**
   * 更新
   */
  public update() {
    this.processResultService.processResultStart(this.processModalCustomResult, BUTTON_ACTION.UPDATE);
    if (this.processModalCustomResult.result == PROCESS_RESULT_RESULT_TYPE.FAILURE) {
      return;
    }

    let isPassWord = this.loginService.checkPassword(this.processModalCustomResult, this.passWordCtrl.value);
    if (isPassWord) {
      this.processResultService.processAtWarning(
        this.processModalCustomResult, this.processModalCustomResult.message, this.partsResultMessageComponent);
      return;
    }

    let parameters = new LoginParameters();
    parameters.companyCode = COMPANY_CODE;
    parameters.userCode = this.loginUserCode;
    parameters.oldPassword = this.oldPassword;
    parameters.password = this.passWordCtrl.value;

    this.loginService.ChangePassword(parameters)
      .subscribe(result => {
        this.processModalCustomResult = this.processResultService.processAtSave(
          this.processModalCustomResult, result, false, this.partsResultMessageComponent);
      });
  }

  public setPassword(eventType: string, type: string) {
    let nextFormName = type == 'input' ? 'passWordConfirmationCtrl' : 'passWordCtrl';
    HtmlUtil.nextFocusByName(this.elementRef, nextFormName, eventType);
  }

  /**
   * パスワードポリシーの取得
   */
  private getPasswordPolicy() {
    this.passwordPolicyService.Get(COMPANY_ID).subscribe(reponse => {
      if (reponse != -1 || reponse != PROCESS_RESULT_RESULT_TYPE.FAILURE) {
        this.loginService.passwordPolicy = reponse;
      }
    });
  }


  ///// Button /////////////////////////////////////////////////////////////////////
  public close() {
    this.ModalStatus = MODAL_STATUS_TYPE.CLOSE;
    this.processModalCustomResult = this.processResultService.clearProcessCustomMsg(this.processModalCustomResult);
    this.closing.emit({});
  }

  public ok() {
    this.ModalStatus = MODAL_STATUS_TYPE.OK;
    this.processModalCustomResult = this.processResultService.clearProcessCustomMsg(this.processModalCustomResult);
    this.closing.emit({});
  }

  public cancel() {
    this.ModalStatus = MODAL_STATUS_TYPE.CANCEL;
    this.processModalCustomResult = this.processResultService.clearProcessCustomMsg(this.processModalCustomResult);
    this.closing.emit({});
  }

  ///// Getter Setter //////////////////////////////////////////////////////////////
  public closing = new EventEmitter<{}>();
  public get Closing() {
    return this.closing;
  }
  public set Closing(value) {
    this.closing = value;
  }

  public get ProcessModalCustomResult() {
    return this.processModalCustomResult;
  }
  public set ProcessModalCustomResult(value) {
    this.processModalCustomResult = value;
  }

  public loginUserCode: string;
  public get LoginUserCode() {
    return this.loginUserCode;
  }
  public set LoginUserCode(value) {
    this.loginUserCode = value;
  }

  public oldPassword: string;
  public get OldPassword() {
    return this.oldPassword;
  }
  public set OldPassword(value) {
    this.oldPassword = value;
  }

}

