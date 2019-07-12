import { Component, OnInit, ElementRef, ComponentFactoryResolver, ViewContainerRef, AfterViewInit } from '@angular/core';
import { BaseComponent } from 'src/app/component/common/base/base-component';
import { Router, NavigationEnd, ActivatedRoute } from '@angular/router';
import { LoginService } from 'src/app/service/Master/login.service';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { FormatterUtil } from 'src/app/common/util/formatter.util';
import { CompanyMasterService } from 'src/app/service/Master/company-master.service';
import { ApplicationControlMasterService } from 'src/app/service/Master/application-control-master.service';
import { MenuAuthorityMasterService } from 'src/app/service/Master/menu-authority-master.service';
import { FunctionAuthorityMasterService } from 'src/app/service/Master/function-authority-master.service';
import { TENANT_LIST, COMPANY_CODE, COMPANY_ID } from 'src/app/common/const/company.const';
import { forkJoin, Subject } from 'rxjs';
import { UserInfoService } from 'src/app/service/common/user-info.service';
import { ModalRouterProgressComponent } from 'src/app/component/modal/modal-router-progress/modal-router-progress.component';
import { GeneralSettingMasterService } from 'src/app/service/Master/general-setting-master.service';
import { CurrencyMasterService } from 'src/app/service/Master/currency-master.service';
import { LoginUserMasterService } from 'src/app/service/Master/login-user-master.service';
import { StringUtil } from 'src/app/common/util/string-util';
import { EVENT_TYPE } from 'src/app/common/const/event.const';
import { HtmlUtil } from 'src/app/common/util/html.util';
import { PageUtil } from 'src/app/common/util/page-util';

import * as _ from 'lodash';
import { ProcessResultService } from 'src/app/service/common/process-result.service';
import { MSG_ERR, MSG_ITEM_NUM, MSG_WNG } from 'src/app/common/const/message.const';
import { PROCESS_RESULT_RESULT_TYPE } from 'src/app/common/const/status.const';
import { WebApiLoginResult } from 'src/app/model/web-api-login-result.model';
import { LoginResult } from 'src/app/common/const/kbn.const';
import { ModalChangePasswordComponent } from 'src/app/component/modal/modal-change-password/modal-change-password.component';
import { PasswordPolicyMasterService } from 'src/app/service/Master/password-policy-master.service';
import { LocalStorageItem } from 'src/app/model/custom-model/local-storage-item';
import { RangeSearchKey } from 'src/app/common/const/local-storage-key-const';
import { LocalStorageManageService } from 'src/app/service/common/local-storage-manage.service';

@Component({
  selector: 'app-pa0101-login',
  templateUrl: './pa0101-login.component.html',
  styleUrls: ['./pa0101-login.component.css']
})
export class Pa0101LoginComponent extends BaseComponent implements OnInit,AfterViewInit {

  public readonly TENANT_LIST: typeof TENANT_LIST = TENANT_LIST;

  public userCodeCtrl: FormControl;
  public passwordCtrl: FormControl;

  public tenantCode: string;


  constructor(
    public router: Router,
    public activatedRoute: ActivatedRoute,
    public elementRef: ElementRef,
    public componentFactoryResolver: ComponentFactoryResolver,
    public viewContainerRef: ViewContainerRef,
    public userInfoService: UserInfoService,
    public loginService: LoginService,
    public companyService: CompanyMasterService,
    public applicationControlService: ApplicationControlMasterService,
    public menyuAuthorityService: MenuAuthorityMasterService,
    public functionAuthorityService: FunctionAuthorityMasterService,
    public loginUserService: LoginUserMasterService,
    public generalSettingService: GeneralSettingMasterService,
    public currencyService: CurrencyMasterService,
    public processResultService: ProcessResultService,
    private passwordPolicyService: PasswordPolicyMasterService,
    public localStorageManageService:LocalStorageManageService,
  ) {
    super();

    router.events.subscribe(event => {
      if (event instanceof NavigationEnd) {
        this.Title = PageUtil.GetTitle(router.routerState, router.routerState.root).join('');
        this.ComponentId = parseInt(PageUtil.GetComponentId(router.routerState, router.routerState.root).join(''));
        const path: string[] = PageUtil.GetPath(this.activatedRoute.snapshot.pathFromRoot);
        this.Path = path[0];
        this.processCustomResult = this.processResultService.processResultInit(this.ComponentId, this.Title);
        this.processModalCustomResult = this.processResultService.processResultInit(this.ComponentId, this.Title, true);
      }
    });
  }



  ngOnInit() {
    // URL取得とScarlet検索処理を追加。
    this.setControlInit();
    this.setValidator();
    this.setFormatter();
    this.clear();

    this.tenantCode = window.location.href;
    this.tenantCode = this.tenantCode.replace("http://", "");
    this.tenantCode = this.tenantCode.replace("https://", "");
    this.tenantCode = this.tenantCode.split(".")[0];
    if (this.tenantCode.startsWith("localhost")) {
      this.tenantCode = "localhost";
    }

    if (!StringUtil.IsNullOrEmpty(this.TENANT_LIST[this.tenantCode])) {
      this.tenantCode = this.TENANT_LIST[this.tenantCode];
    }
    else {
      this.tenantCode = this.tenantCode;
    }

    this.userInfoService.TenantCode = this.tenantCode;

    this.loginService.validateTenantCode()
      .subscribe(response => {
        if (response == PROCESS_RESULT_RESULT_TYPE.FAILURE) {
          this.router.navigate(['404-error']);
        }
      });

  }


  public setControlInit() {
    // this.userCodeCtrl = new FormControl("0000000001", [Validators.required]);  // ユーザーコード
    this.passwordCtrl = new FormControl("pas", [Validators.required]);  // パスワード
    this.userCodeCtrl = new FormControl("", [Validators.required]);  // ユーザーコード
    //this.passwordCtrl = new FormControl("", [Validators.required]);  // パスワード

  }

  public setValidator() {
    this.MyFormGroup = new FormGroup({
      userCodeCtrl: this.userCodeCtrl,
      passwordCtrl: this.passwordCtrl,
    });

  }

  public setFormatter() {
    FormatterUtil.setCodeFormatter(this.userCodeCtrl);
    FormatterUtil.setCodeFormatter(this.passwordCtrl);
  }

  public clear() {

    let userCode = this.localStorageManageService.get(RangeSearchKey.LOGIN_USER_CODE);

    if (userCode != null) {
      this.userCodeCtrl.setValue(userCode.value);
      HtmlUtil.nextFocusByName(this.elementRef, 'passwordCtrl', EVENT_TYPE.NONE);
    }
    else{
      HtmlUtil.nextFocusByName(this.elementRef, 'userCodeCtrl', EVENT_TYPE.NONE);
    }


  }

  ngAfterViewInit(){
    if (!StringUtil.IsNullOrEmpty(this.userCodeCtrl.value)) {
      HtmlUtil.nextFocusByName(this.elementRef, 'passwordCtrl', EVENT_TYPE.NONE);
    }
    else{
      HtmlUtil.nextFocusByName(this.elementRef, 'userCodeCtrl', EVENT_TYPE.NONE);
    }  
  }

  public setUserCode(eventType: string) {
    HtmlUtil.nextFocusByName(this.elementRef, 'passwordCtrl', eventType);
  }

  public setPassword(eventType: string) {
    HtmlUtil.nextFocusByName(this.elementRef, 'userCodeCtrl', eventType);
  }  


  public login() {

    let componentFactory = this.componentFactoryResolver.resolveComponentFactory(ModalRouterProgressComponent);
    let componentRef = this.viewContainerRef.createComponent(componentFactory);

    // アクセストークンの取得
    this.loginService.Login(COMPANY_CODE, this.userCodeCtrl.value, this.passwordCtrl.value)
      .subscribe(response => {
        const loginResult = response as WebApiLoginResult;
        
        if (response == PROCESS_RESULT_RESULT_TYPE.FAILURE || loginResult == undefined) {
          this.processCustomResult = this.processResultService.processAtFailure(
            this.processCustomResult, MSG_ERR.SOMETHING_ERROR.replace(MSG_ITEM_NUM.FIRST, 'ログイン'),
            this.partsResultMessageComponent);
          componentRef.destroy();
          return;
        }

        if (loginResult.loginResult == LoginResult.Expired) {
          //  password
          this.processCustomResult.logData.clientName = this.userInfoService.TenantCode;
          this.processCustomResult.logData.companyId = COMPANY_ID;
          this.processCustomResult.logData.loginUserCode = this.userCodeCtrl.value;         
          this.processCustomResult = this.processResultService.processAtFailure(
            this.processCustomResult, "LOGIN_PASSWORD_EXPIRED", this.partsResultMessageComponent);
          componentRef.destroy();    
          return;
        }
        else if(loginResult.loginResult == LoginResult.Failed){
          this.processCustomResult = this.processResultService.processAtFailure(
            this.processCustomResult, MSG_ERR.INVALID_PASSWORD, this.partsResultMessageComponent);
          componentRef.destroy();
          return;
        }

        this.userInfoService.AccessToken = loginResult.sessionKey;

        // 企業情報の取得
        this.companyService.GetItems(COMPANY_CODE)
          .subscribe(reponse => {
            if (response == PROCESS_RESULT_RESULT_TYPE.FAILURE) {
              this.processCustomResult = this.processResultService.processAtFailure(
                this.processCustomResult, MSG_ERR.SOMETHING_ERROR.replace(MSG_ITEM_NUM.FIRST, 'ログイン'),
                this.partsResultMessageComponent);
              componentRef.destroy();
              return;
            }
            this.userInfoService.Company = reponse[0];

            // アプリケーションコントロール、メニュー権限、機能権限の取得、管理マスタ、通貨
            let applicationControlResponse =
              this.applicationControlService.Get(this.userInfoService.Company.id);

            let menyuAuthorityResponse =
              this.menyuAuthorityService.GetItemsByMyCompany();

            let functionAuthorityResponse =
              this.functionAuthorityService.GetItems();

            let loginUserResponse =
              this.loginUserService.GetItems(this.userCodeCtrl.value);

            let generalSettingResponse =
              this.generalSettingService.GetItems();

            let currencyResponse =
              this.currencyService.GetItems();

            let passwordPolicyResponse =
              this.passwordPolicyService.Get(COMPANY_ID);

            // ３つの処理の待機
            forkJoin(
              applicationControlResponse,
              menyuAuthorityResponse,
              functionAuthorityResponse,
              loginUserResponse,
              generalSettingResponse,
              currencyResponse,
              passwordPolicyResponse
            )
              .subscribe(responseList => {

                if (responseList.length != 7
                  || responseList[0] == PROCESS_RESULT_RESULT_TYPE.FAILURE || responseList[1] == PROCESS_RESULT_RESULT_TYPE.FAILURE
                  || responseList[2] == PROCESS_RESULT_RESULT_TYPE.FAILURE || responseList[3] == PROCESS_RESULT_RESULT_TYPE.FAILURE
                  || responseList[4] == PROCESS_RESULT_RESULT_TYPE.FAILURE || responseList[5] == PROCESS_RESULT_RESULT_TYPE.FAILURE
                  || responseList[6] == PROCESS_RESULT_RESULT_TYPE.FAILURE
                ) {
                  componentRef.destroy();
                  this.processCustomResult = this.processResultService.processAtFailure(
                    this.processCustomResult, MSG_ERR.SOMETHING_ERROR.replace(MSG_ITEM_NUM.FIRST, 'ログイン'),
                    this.partsResultMessageComponent);
                  return;
                }

                this.userInfoService.ApplicationControl = responseList[0];
                this.userInfoService.MenyuAuthority = responseList[1];
                this.userInfoService.FunctionAuthoritys = responseList[2];
                this.userInfoService.LoginUser = responseList[3][0];
                this.userInfoService.GeneralSettings = responseList[4];
                this.userInfoService.Currency = responseList[5][0];
                this.loginService.passwordPolicy = responseList[6]
                componentRef.destroy();
                this.router.navigate(['main']);

                let localstorageItem = new LocalStorageItem();
                localstorageItem.key = RangeSearchKey.LOGIN_USER_CODE;
                localstorageItem.value = this.userCodeCtrl.value;
                this.localStorageManageService.set(localstorageItem);


              },
                () => {
                  this.userInfoService.Company = null;
                  this.userInfoService.ApplicationControl = null;
                  this.userInfoService.MenyuAuthority = null;
                  this.userInfoService.FunctionAuthoritys = null;
                  this.userInfoService.TenantCode = null;
                  componentRef.destroy();
                }
              );
          }
          );

      }
      );

  }

  public changePassword() {
    let componentFactory = this.componentFactoryResolver.resolveComponentFactory(ModalChangePasswordComponent);
    let componentRef = this.viewContainerRef.createComponent(componentFactory);

    this.processModalCustomResult.isModal = true;
    componentRef.instance.processModalCustomResult = this.processModalCustomResult;
    componentRef.instance.loginUserCode = this.userCodeCtrl.value;
    componentRef.instance.oldPassword = this.passwordCtrl.value;

    componentRef.instance.Closing.subscribe(() => {
      componentRef.destroy();
    });
    
  }

}

