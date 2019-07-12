import { Component, OnInit, ComponentFactoryResolver, ViewContainerRef, ElementRef } from '@angular/core';
import { BaseComponent } from 'src/app/component/common/base/base-component';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { TABLE_INDEX } from 'src/app/common/const/table-name.const';
import { HtmlUtil } from 'src/app/common/util/html.util';
import { UserInfoService } from 'src/app/service/common/user-info.service';
import { WebApiSettingMasterService } from 'src/app/service/Master/web-api-setting-master.service';
import { WebApiSetting } from 'src/app/model/web-api-setting.model';
import { StringUtil } from 'src/app/common/util/string-util';
import { ModalConfirmComponent } from 'src/app/component/modal/modal-confirm/modal-confirm.component';
import { MODAL_STATUS_TYPE, PROCESS_RESULT_RESULT_TYPE, PROCESS_RESULT_STATUS_TYPE } from 'src/app/common/const/status.const';
import { LINE_FEED_CODE } from 'src/app/common/const/report-setting-kbn.const';
import { MfHttpRequestServiceService } from 'src/app/service/common/mf-http-request-service.service';
//import { MfRedirectUri } from 'src/app/common/const/http.const';
import { WebApiType } from 'src/app/common/const/kbn.const';
import { MfOffice } from 'src/app/model/mf-billing/mf-office.model';
import { MFWebApiOption } from 'src/app/model/mf-web-api-option.model';
import { MSG_WNG, MSG_ITEM_NUM, MSG_INF } from 'src/app/common/const/message.const';
import { BUTTON_ACTION } from 'src/app/common/const/event.const';
import { ComponentId } from 'src/app/common/const/component-name.const';
import { ProcessResultService } from 'src/app/service/common/process-result.service';
import { Router, ActivatedRoute,ParamMap, NavigationEnd } from '@angular/router';
import { PageUtil } from 'src/app/common/util/page-util';
import { MFBillingService } from 'src/app/service/mfbilling.service';
import { ProcessResultCustom } from 'src/app/model/custom-model/process-result-custom.model';
import { ModalRouterProgressComponent } from 'src/app/component/modal/modal-router-progress/modal-router-progress.component';

@Component({
  selector: 'app-ph1401-mf-web-api-setting',
  templateUrl: './ph1401-mf-web-api-setting.component.html',
  styleUrls: ['./ph1401-mf-web-api-setting.component.css']
})
export class Ph1401MfWebApiSettingComponent extends BaseComponent implements OnInit {

  public TABLE_INDEX: typeof TABLE_INDEX = TABLE_INDEX;

  public webApiSetting: WebApiSetting;

  public clientIdCtrl: FormControl;  // Client ID

  public clientSecretCtrl: FormControl; // Client Secret

  public scopeDescriptionCtrl: FormControl; // Scope

  public callbackURLDescriptionCtrl: FormControl;  // Collback URL

  public authorizationCodeCtrl: FormControl;  // 認証コード

  public connectionStatusDescriptionCtrl: FormControl; // 連携状態

  public connectionResultDescriptionCtrl: FormControl;

  public paramFrom:ComponentId;

  public undefineCtrl:FormControl;


  constructor(
    public router: Router,
    public activatedRoute: ActivatedRoute,
    public elementRef: ElementRef,
    public componentFactoryResolver: ComponentFactoryResolver,
    public viewContainerRef: ViewContainerRef,
    public userInfoService: UserInfoService,
    public webApiSettingService:WebApiSettingMasterService,
    public mfHttpRequestServiceService:MfHttpRequestServiceService,
    public processResultService: ProcessResultService,
    public mfBillingService: MFBillingService,

  ) {
    super();

    router.events.subscribe(event => {
      if (event instanceof NavigationEnd) {
        this.Title = PageUtil.GetTitle(router.routerState, router.routerState.root).join('');
        this.ComponentId = parseInt(PageUtil.GetComponentId(router.routerState, router.routerState.root).join(''));
        const path:string[] = PageUtil.GetPath(this.activatedRoute.snapshot.pathFromRoot);
        this.Path = path[1];
        this.processCustomResult = this.processResultService.processResultInit(this.ComponentId,this.Title);
      }
    });
  }

  ngOnInit() {
    this.setControlInit();
    this.setValidator();
    this.clear();

    this.activatedRoute.paramMap.subscribe((params: ParamMap) => {
      this.paramFrom = parseInt(params.get('from'));
      });

    this.loadWebApiSetting();
  }

  public loadWebApiSetting() {
    this.webApiSettingService.GetByIdAsync(WebApiType.MoneyForward)
      .subscribe(response => {
        if (response != undefined) {
          this.webApiSetting = response as WebApiSetting;
          this.setApiSetting();
        }
        this.displayConnectionStatus();
      });
  }

  public setApiSetting() {
    this.clientIdCtrl.setValue(this.webApiSetting.clientId);
    this.clientSecretCtrl.setValue(this.webApiSetting.clientSecret);
    this.scopeDescriptionCtrl.setValue("write");
    //this.callbackURLDescriptionCtrl.setValue(MfRedirectUri);

  }

  public displayConnectionStatus() {
    if (this.webApiSetting == undefined || StringUtil.IsNullOrEmpty(this.webApiSetting.accessToken)) {
      this.connectionStatusDescriptionCtrl.setValue("未連携");
      return;
    }

    let mfWebApiOption = new MFWebApiOption();
    mfWebApiOption.companyId = this.userInfoService.Company.id;

    this.mfHttpRequestServiceService.ValidateToken(mfWebApiOption)
      .subscribe(result => {
        let message = result ? "連携中" : "トークン有効期限切れ";
        this.connectionStatusDescriptionCtrl.setValue(message);
      });
  }

  public setControlInit() {

    this.clientIdCtrl = new FormControl("", [Validators.maxLength(100), Validators.required]); // Client ID

    this.clientSecretCtrl = new FormControl("", [Validators.maxLength(100), Validators.required]);  // Client Secret

    this.scopeDescriptionCtrl = new FormControl("", [Validators.maxLength(100), Validators.required]);// Scope

    this.callbackURLDescriptionCtrl = new FormControl("", [Validators.maxLength(100), Validators.required]);// Collback URL

    this.authorizationCodeCtrl = new FormControl("", [Validators.maxLength(100), Validators.required]);// 認証コード

    this.connectionStatusDescriptionCtrl = new FormControl("");// 連携状態

    this.connectionResultDescriptionCtrl = new FormControl("");

    this.undefineCtrl = new FormControl("");
  }

  public setValidator() {
    this.MyFormGroup = new FormGroup({

      clientIdCtrl: this.clientIdCtrl,
      clientSecretCtrl: this.clientSecretCtrl,

      scopeDescriptionCtrl: this.scopeDescriptionCtrl,
      callbackURLDescriptionCtrl: this.callbackURLDescriptionCtrl,

      authorizationCodeCtrl: this.authorizationCodeCtrl,
      connectionStatusDescriptionCtrl: this.connectionStatusDescriptionCtrl,

      connectionResultDescriptionCtrl: this.connectionResultDescriptionCtrl,

      undefineCtrl: this.undefineCtrl,

    })
  }

  /**
 * ボタン操作によるメソッド呼び出し
 * @param action ボタン操作
 */
  public buttonActionInner(action: BUTTON_ACTION) {
    this.processResultService.processResultStart(this.processCustomResult, action);
    if (this.processCustomResult.result == PROCESS_RESULT_RESULT_TYPE.FAILURE) {
      return;
    }

    switch (action) {
      case BUTTON_ACTION.REGISTRY:
        this.registry();
        break;

      case BUTTON_ACTION.DELETE:
        this.delete();
        break;

      default:
        console.log('buttonAction Error.');
        break;
    }
  }

  public clear() {
    this.MyFormGroup.reset();
    this.webApiSetting = null;
    this.scopeDescriptionCtrl.setValue("write");
    //this.callbackURLDescriptionCtrl.setValue(MfRedirectUri);
  }

  public setProcessResult(
    processResult:  ProcessResultCustom,
    message:        string                      = '',
    status:         PROCESS_RESULT_STATUS_TYPE  = PROCESS_RESULT_STATUS_TYPE.DONE,
    result:         PROCESS_RESULT_RESULT_TYPE  = PROCESS_RESULT_RESULT_TYPE.FAILURE,
    ) {
    if (!StringUtil.IsNullOrEmpty(message)) {
      processResult.message = message;
    }
    processResult.status = status;
    processResult.result = result;
  }

  public registry() {

    let componentFactory = this.componentFactoryResolver.resolveComponentFactory(ModalRouterProgressComponent);
    let componentRef = this.viewContainerRef.createComponent(componentFactory);
    // componentRef.destroy();

    let mfWebApiOption = this.getMfWebApiOption();
    this.mfHttpRequestServiceService.Save(mfWebApiOption)
      .subscribe(result => {
        if (result == undefined || result != -1) {
          this.processCustomResult = this.processResultService.processAtSave(
            this.processCustomResult, result, true, this.partsResultMessageComponent);
        }
        if (result == -1) {
          this.setProcessResult(this.processCustomResult, '認証処理に失敗しました');
        }
        if (this.processCustomResult.result == PROCESS_RESULT_RESULT_TYPE.SUCCESS) {
          this.loadWebApiSetting();
        }
        componentRef.destroy();
      });
  }

  /**
   * 認証コードを取得
   * 文字列の中から 'code=' より後の文字列を返す
   * 'code=' が含まれない場合、空文字を返す
   * @param value 
   */
  public getAuthorizationCode(value: string): string {
    const pattern = 'code=';
    let index = value.search(pattern);
    if (index == -1) {
      return '';
    }
    index += pattern.length;
    return value.substr(index);
  }

  /**
   * 連携設定用のパラメータを返す 認証 + 登録処理
   * 抽出用の設定が登録されている場合、ローカルに保持している 抽出設定を設定して返す
   */
  public getMfWebApiOption(): MFWebApiOption {
    const option = {
      companyId:    this.userInfoService.Company.id,
      loginUserId:  this.userInfoService.LoginUser.id,
      clientId:     this.clientIdCtrl.value,
      clientSecret: this.clientSecretCtrl.value,
      apiVersion:   'v1',
    } as MFWebApiOption;

    if (this.webApiSetting != undefined &&
       !StringUtil.IsNullOrEmpty(this.webApiSetting.extractSetting)) {
      option.extractSetting = this.webApiSetting.extractSetting;
    }
    option.authorizationCode = this.getAuthorizationCode(this.authorizationCodeCtrl.value);

    return option;
  }

  public displayOfficeInfo() {

    let mfWebApiOption = new MFWebApiOption();
    mfWebApiOption.companyId = this.userInfoService.Company.id;
    mfWebApiOption.loginUserId = this.userInfoService.LoginUser.id;

    this.mfHttpRequestServiceService.GetOffice(mfWebApiOption)
      .subscribe(result => {
        if (result != undefined) {
          let office = result as MfOffice;

          let officeInfo: string = LINE_FEED_CODE + "連携に成功しました。" + LINE_FEED_CODE;
          officeInfo += "事業所名：" + (office.name == null ? "" : office.name) + LINE_FEED_CODE;
          officeInfo += "郵便番号：" + (office.zip == null ? "" : office.zip) + LINE_FEED_CODE;
          officeInfo += "都道府県：" + (office.prefecture == null ? "" : office.prefecture) + LINE_FEED_CODE;
          officeInfo += "住所１　：" + (office.address1 == null ? "" : office.address1) + LINE_FEED_CODE;
          officeInfo += "住所２　：" + (office.address2 == null ? "" : office.address2) + LINE_FEED_CODE;
          officeInfo += "電話番号：" + (office.tel == null ? "" : office.tel) + LINE_FEED_CODE;
          officeInfo += "FAX番号 ：" + (office.fax == null ? "" : office.fax) + LINE_FEED_CODE;

          //事業所情報を表示
          this.connectionResultDescriptionCtrl.setValue(officeInfo);

        }
      });
  }

  public delete() {
    // 削除処理
    let componentFactory = this.componentFactoryResolver.resolveComponentFactory(ModalConfirmComponent);
    let componentRef = this.viewContainerRef.createComponent(componentFactory);
    componentRef.instance.ActionName = "削除"
    componentRef.instance.Closing.subscribe(() => {
      if (componentRef.instance.ModalStatus == MODAL_STATUS_TYPE.OK) {

        let componentFactory = this.componentFactoryResolver.resolveComponentFactory(ModalRouterProgressComponent);
        let componentRef = this.viewContainerRef.createComponent(componentFactory);
        // componentRef.destroy();

        this.webApiSettingService.DeleteAsync(WebApiType.MoneyForward)
          .subscribe(result => {
            this.processCustomResult = this.processResultService.processAtDelete(
              this.processCustomResult, result, this.partsResultMessageComponent);
            if (result != undefined) {
              this.clear();
              this.displayConnectionStatus();
            }
            componentRef.destroy();
          });
      }
      else{
        this.processResultService.processAtWarning(
          this.processCustomResult, MSG_WNG.PROCESS_CANCELED, this.partsResultMessageComponent);
      }
      componentRef.destroy();
    });
  }

  public back() {
    
    if (!this.enableBack()) return;

    this.router.navigate(['main/PC1801', { "process": "back" }]);
    
  }

  ///////////////////////////////////////////////////////////////////////
  public setClientId(eventType: string) {
    HtmlUtil.nextFocusByName(this.elementRef, 'clientSecretCtrl', eventType);
  }

  ///////////////////////////////////////////////////////////////////////
  public setClientSecret(eventType: string) {
    HtmlUtil.nextFocusByName(this.elementRef, 'scopeDescriptionCtrl', eventType);
  }

  ///////////////////////////////////////////////////////////////////////
  public setScopeDescription(eventType: string) {
    HtmlUtil.nextFocusByName(this.elementRef, 'callbackURLDescriptionCtrl', eventType);
  }

  ///////////////////////////////////////////////////////////////////////
  public setCallbackURLDescription(eventType: string) {
    HtmlUtil.nextFocusByName(this.elementRef, 'authorizationCodeCtrl', eventType);
  }

  ///////////////////////////////////////////////////////////////////////
  public setAuthorizationCode(eventType: string) {
    HtmlUtil.nextFocusByName(this.elementRef, 'connectionStatusDescriptionCtrl', eventType);
  }

  ///////////////////////////////////////////////////////////////////////
  public setConnectionStatusDescription(eventType: string) {
    HtmlUtil.nextFocusByName(this.elementRef, 'clientIdCtrl', eventType);
  }

  public enableBack():boolean{
    // MFクラウド請求書　データ抽出から遷移した場合
    return (this.paramFrom == ComponentId.PC1801);
  }

}

