import { Component, OnInit, ComponentFactoryResolver, ViewContainerRef, ElementRef, ViewChild } from '@angular/core';
import { BaseComponent } from 'src/app/component/common/base/base-component';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { FormatterUtil } from 'src/app/common/util/formatter.util';
import { HtmlUtil } from 'src/app/common/util/html.util';
import { FunctionType, EXCLUSIVE_ACCOUNT_TYPE } from 'src/app/common/const/kbn.const';
import { MenuAuthorityMasterService } from 'src/app/service/Master/menu-authority-master.service';
import { FunctionAuthorityMasterService } from 'src/app/service/Master/function-authority-master.service';
import { MenuAuthoritiesResult } from 'src/app/model/menu-authorities-result.model';
import { FunctionAuthoritiesResult } from 'src/app/model/function-authorities-result.model';
import { UserInfoService } from 'src/app/service/common/user-info.service';
import { StringUtil } from 'src/app/common/util/string-util';
import { Company } from 'src/app/model/company.model';
import { CompanyMasterService } from 'src/app/service/Master/company-master.service';
import { ApplicationControlMasterService } from 'src/app/service/Master/application-control-master.service';
import { PasswordPolicyMasterService } from 'src/app/service/Master/password-policy-master.service';
import { ApplicationControl } from 'src/app/model/application-control.model';
import { PasswordPolicy } from 'src/app/model/password-policy.model';
import { LoginUserLicense } from 'src/app/model/login-user-license.model';
import { LoginUserLicenseMasterService } from 'src/app/service/Master/login-user-license-master.service';
import { forkJoin } from 'rxjs';
import { ClosingService } from 'src/app/service/closing.service';
import { ClosingInformation } from 'src/app/model/closing-information.model';
import { FunctionAuthority } from 'src/app/model/function-authority.model';
import { CompanySource } from 'src/app/model/company-source.model';
import { BUTTON_ACTION, EVENT_TYPE } from 'src/app/common/const/event.const';
import { PROCESS_RESULT_RESULT_TYPE } from 'src/app/common/const/status.const';
import { MSG_WNG, MSG_ITEM_NUM } from 'src/app/common/const/message.const';
import { CompanyLogo } from 'src/app/model/company-logo.model';
import { MenuAuthority } from 'src/app/model/menu-authority.model';
import { ProcessResultService } from 'src/app/service/common/process-result.service';
import { Router, ActivatedRoute, NavigationEnd } from '@angular/router';
import { PageUtil } from 'src/app/common/util/page-util';
import { ModalRouterProgressComponent } from 'src/app/component/modal/modal-router-progress/modal-router-progress.component';
import { MenuOption } from 'src/app/component/common/COM0300/com0301-menu/menu';
import { PartsCompanyComponent } from 'src/app/component/view-parts/parts-company/parts-company.component';
import { CompanySearch } from 'src/app/model/company-search.model';
import { LoginService } from 'src/app/service/Master/login.service';

@Component({
  selector: 'app-ph0101-setting-ssecurity',
  templateUrl: './ph0101-setting-ssecurity.component.html',
  styleUrls: ['./ph0101-setting-ssecurity.component.css']
})
export class Ph0101SettingSsecurityComponent extends BaseComponent implements OnInit {
  /** 会社情報フォーム */
  @ViewChild('partsCompany') public readonly partsCompany: PartsCompanyComponent;

  public panelCompanyOpenState: boolean = true;
  public panelFunctionOpenState: boolean = true;
  public panelPrivilegeOpenState: boolean = true;
  public panelPasswordOpenState: boolean = true;

  public readonly exclusiveAccountType = EXCLUSIVE_ACCOUNT_TYPE; // 預金種別
  public readonly passwordSymbolRow = new Array<String[]>();  // パスワード記号
  public symbolColNumber = 9; // パスワード記号の1行に表示する個数


  public selectedCompany: boolean = false;

  public menuAuthoritiesResult: MenuAuthoritiesResult;
  public functionAuthoritiesResult: FunctionAuthoritiesResult;

  ////////////////////////////////////////////////////////////////////////////////////////////////
  public rdoUseReceiptSectionCtrl: FormControl; // 入金部門管理
  public rdoUseAuthorizationCtrl: FormControl; // 承認機能
  // 締め処理
  public rdoUseForeignCurrencyCtrl: FormControl; // 外貨対応
  public rdoUseCashOnDueDatesCtrl: FormControl; // 期日入金管理
  // ファクタリング
  public rdoUseAccountTransferCtrl: FormControl; // 口座振替対応
  // 決済結果照合機能
  public rdoUseBillingFilterCtrl: FormControl; // 請求絞込機能
  public rdoUseScheduledPaymentCtrl: FormControl; // 入金予定入力
  public rdoUseDeclaredAmountCtrl: FormControl; // 予定額を消込額に使用
  public rdoUsePublishInvoiceCtrl: FormControl; // 請求書発行機能利用
  public rdoUseReminderCtrl: FormControl; // 督促管理利用
  public rdoUseDistributionCtrl: FormControl; // 配信機能利用
  public rdoUseMFWebApiCtrl: FormControl; // MFクラウド請求書 WebAPI利用
  public rdoUseHatarakuDBWebApiCtrl: FormControl; // 働くDB WebAPI 利用
  public rdoUsePCADXWebApiCtrl: FormControl; // PCA会計DX WebAPI利用  
  public rdoUseMfAggregationCtrl: FormControl; // 入金データ自動連携 WebAPI 連携

  // 奉行クラウド WebAPI 連携
  //public rdoRegisterContractInAdvanceCtrl:FormControl; // 契約マスターの自動登録
  //public rdoUseDiscountCtrl:FormControl; // 歩引対応

  public departmentCodeLengthCtrl: FormControl; // 請求部門コード桁数
  public cmbDepartmentCodeTypeCtrl: FormControl;

  public accountTitleCodeLengthCtrl: FormControl; // 科目コード桁数
  public cmbAccountTitleCodeTypeCtrl: FormControl;

  public customerCodeLengthCtrl: FormControl; // 得意先コード桁数
  public cmbCustomerCodeTypeCtrl: FormControl;

  public loginUserCodeLengthCtrl: FormControl; // ユーザーコード桁数
  public cmbLoginUserCodeTypeCtrl: FormControl;

  public sectionCodeLengthCtrl: FormControl; // 入金部門桁数
  public cmbSectionCodeTypeCtrl: FormControl;

  public staffCodeLengthCtrl: FormControl; // 営業担当者コード桁数
  public cmbStaffCodeTypeCtrl: FormControl;

  public licenseNumCtrl: FormControl; // ライセンス数

  ////////////////////////////////////////////////////////////////////////////////////////////////
  public cbxMenuAuthorityCtrls: FormControl[][];  // メニュー権限チェック
  public cbxSecurityAuthorityCtrls: FormControl[][]; // セキュリティ

  public cmbMenuAuthorityLevelCtrl // 一括メニュー権限レベル

  ////////////////////////////////////////////////////////////////////////////////////////////////
  public passwordMinLengthCtrl: FormControl; // パスワード文字数
  public passwordMaxLengthCtrl: FormControl; // パスワード文字数（文字以上）

  public cbxPasswordUseAlphabetCtrl: FormControl; // パスワード文字種類（アルファベット）
  public passwordMinAlphabetUseCountCtrl: FormControl; // パスワード文字種類（アルファベット）（文字以上含む）
  public cbxPasswordCaseSensitiveCtrl: FormControl; // パスワード文字種類（アルファベット）（大文字と小文字の区別をする）

  public cbxPasswordUseNumberCtrl: FormControl; // パスワード文字種類（数字）
  public passwordMinNumberUseCountCtrl: FormControl; // パスワード文字種類（数字）（文字以上含む）

  public cbxPasswordUseSymbolCtrl: FormControl; // パスワード文字種類（記号）
  public passwordMinSymbolUseCountCtrl: FormControl; // パスワード文字種類（記号）（文字以上含む）
  public cbxPasswordCheckAllSymbolsCtrl: FormControl; // パスワード文字種類（記号）（全てにチェックを入れる）

  public cbxPasswordSymbolCtrls: FormControl[]; // パスワード記号

  public cbxPasswordMinSameCharacterRepeatCtrl: FormControl; // 同じ文字を連続して使用しない
  public passwordMinSameCharacterRepeatCtrl: FormControl; // 同じ文字を連続して使用しない（文字以上）

  public cbxPasswordExpirationDaysCtrl: FormControl; // パスワード有効期限
  public passwordExpirationDaysCtrl: FormControl; // パスワード有効期限（日経過したらパスワードを変更する）

  public passwordHistoryCountCtrl: FormControl; // パスワード履歴の保存数（個：最大10個）


  public undefineCtrl = new FormControl; // 未定用;  

  constructor(
    public router: Router,
    public activatedRoute: ActivatedRoute,
    public elementRef: ElementRef,
    public componentFactoryResolver: ComponentFactoryResolver,
    public viewContainerRef: ViewContainerRef,
    public userInfoService: UserInfoService,
    public companyService: CompanyMasterService,
    public applicationControlService: ApplicationControlMasterService,
    public passwordPolicyService: PasswordPolicyMasterService,
    public loginUserLicenseService: LoginUserLicenseMasterService,
    public closingService: ClosingService,
    public menuAuthorityService: MenuAuthorityMasterService,
    public functionAuthorityService: FunctionAuthorityMasterService,
    public processResultService: ProcessResultService,
    public loginService: LoginService,

  ) {
    super();

    router.events.subscribe(event => {
      if (event instanceof NavigationEnd) {
        this.Title = PageUtil.GetTitle(router.routerState, router.routerState.root).join('');
        this.ComponentId = parseInt(PageUtil.GetComponentId(router.routerState, router.routerState.root).join(''));
        const path: string[] = PageUtil.GetPath(this.activatedRoute.snapshot.pathFromRoot);
        this.Path = path[1];
        this.processCustomResult = this.processResultService.processResultInit(this.ComponentId, this.Title);
        this.processModalCustomResult = this.processResultService.processResultInit(this.ComponentId, this.Title, true);
      }
    });
  }

  ngOnInit() {
    this.setControlInit();
    this.setValidator();
    this.setFormatter();
    this.clear();
  }

  private setControlInit() {

    ////////////////////////////////////////////////////////////////////////////////////////////////
    this.rdoUseReceiptSectionCtrl = new FormControl("0");// 入金部門管理
    this.rdoUseAuthorizationCtrl = new FormControl("0");// 承認機能
    // 締め処理
    this.rdoUseForeignCurrencyCtrl = new FormControl("0");// 外貨対応
    this.rdoUseCashOnDueDatesCtrl = new FormControl("0");// 期日入金管理
    // ファクタリング
    this.rdoUseAccountTransferCtrl = new FormControl("0");// 口座振替対応
    // 決済結果照合機能
    this.rdoUseBillingFilterCtrl = new FormControl("0");// 請求絞込機能
    this.rdoUseScheduledPaymentCtrl = new FormControl("0");// 入金予定入力
    this.rdoUseDeclaredAmountCtrl = new FormControl("0");// 予定額を消込額に使用
    this.rdoUsePublishInvoiceCtrl = new FormControl("0");// 請求書発行機能利用
    this.rdoUseReminderCtrl = new FormControl("0");// 督促管理利用
    this.rdoUseDistributionCtrl = new FormControl("0");// 配信機能利用
    this.rdoUseMFWebApiCtrl = new FormControl("0");// MFクラウド請求書 WebAPI利用
    this.rdoUseHatarakuDBWebApiCtrl = new FormControl("0");// 働くDB WebAPI 利用
    this.rdoUsePCADXWebApiCtrl = new FormControl("0");// PCA会計DX WebAPI利用
    this.rdoUseMfAggregationCtrl = new FormControl("0");
    // 奉行クラウド WebAPI 連携
    // 契約マスターの自動登録
    // 歩引対応

    this.departmentCodeLengthCtrl = new FormControl("", [Validators.required, Validators.maxLength(2)]); // 請求部門コード桁数
    this.cmbDepartmentCodeTypeCtrl = new FormControl("", [Validators.required]);

    this.accountTitleCodeLengthCtrl = new FormControl("", [Validators.required, Validators.maxLength(2)]); // 科目コード桁数
    this.cmbAccountTitleCodeTypeCtrl = new FormControl("", [Validators.required]);

    this.customerCodeLengthCtrl = new FormControl("", [Validators.required, Validators.maxLength(2)]); // 得意先コード桁数
    this.cmbCustomerCodeTypeCtrl = new FormControl("", [Validators.required]);

    this.loginUserCodeLengthCtrl = new FormControl("", [Validators.required, Validators.maxLength(2)]); // ユーザーコード桁数
    this.cmbLoginUserCodeTypeCtrl = new FormControl("", [Validators.required]);

    this.sectionCodeLengthCtrl = new FormControl("", [Validators.required, Validators.maxLength(2)]); // 入金部門桁数
    this.cmbSectionCodeTypeCtrl = new FormControl("", [Validators.required]);

    this.staffCodeLengthCtrl = new FormControl("", [Validators.required, Validators.maxLength(2)]); // 営業担当者コード桁数
    this.cmbStaffCodeTypeCtrl = new FormControl("", [Validators.required]);

    this.licenseNumCtrl = new FormControl(""); // ライセンス数

    this.undefineCtrl = new FormControl(""); // 未定用

    ////////////////////////////////////////////////////////////////////////////////////////////////
    this.cmbMenuAuthorityLevelCtrl = new FormControl(""); // 一括メニュー権限レベル

    /*
    this.cbxMenuAuthorityCtrls = new Array<FormControl[]>(this.menuAuthoritiesResult.menuAuthorities.length);
    for(let index=0;index<this.cbxMenuAuthorityCtrls.length;index++){
      this.cbxMenuAuthorityCtrls[index] = new Array<FormControl>(4);
      for(let index2=0;index2<this.cbxMenuAuthorityCtrls[index].length;index2++){
        this.cbxMenuAuthorityCtrls[index][index2]=new FormControl("");
      }
    }

    this.cbxSecurityAuthorityCtrls = new Array<FormControl[]>(this.functionAuthoritiesResul.functionAuthorities.length);
    for(let index=0;index<this.cbxSecurityAuthorityCtrls.length;index++){
      this.cbxSecurityAuthorityCtrls[index] = new Array<FormControl>(6);
      for(let index2=0;index2<this.cbxSecurityAuthorityCtrls[index].length;index2++){
        this.cbxSecurityAuthorityCtrls[index][index2]=new FormControl("");
      }
    }
    */

    ////////////////////////////////////////////////////////////////////////////////////////////////
    this.passwordMinLengthCtrl = new FormControl("", [Validators.required, Validators.maxLength(2), Validators.min(1)]); // パスワード文字数
    this.passwordMaxLengthCtrl = new FormControl("", [Validators.required, Validators.maxLength(2), Validators.max(15)]); // パスワード文字数（文字以上）

    this.cbxPasswordUseAlphabetCtrl = new FormControl(""); // パスワード文字種類（アルファベット）
    this.passwordMinAlphabetUseCountCtrl = new FormControl("", [Validators.maxLength(2)]); // パスワード文字種類（アルファベット）（文字以上含む）
    this.cbxPasswordCaseSensitiveCtrl = new FormControl(""); // パスワード文字種類（アルファベット）（大文字と小文字の区別をする）

    this.cbxPasswordUseNumberCtrl = new FormControl(""); // パスワード文字種類（数字）
    this.passwordMinNumberUseCountCtrl = new FormControl("", [Validators.maxLength(2)]); // パスワード文字種類（数字）（文字以上含む）

    this.cbxPasswordUseSymbolCtrl = new FormControl(""); // パスワード文字種類（記号）
    this.passwordMinSymbolUseCountCtrl = new FormControl("", [Validators.maxLength(2)]); // パスワード文字種類（記号）（文字以上含む）
    this.cbxPasswordCheckAllSymbolsCtrl = new FormControl(""); // パスワード文字種類（記号）（全てにチェックを入れる）

    this.cbxPasswordMinSameCharacterRepeatCtrl = new FormControl(""); // 同じ文字を連続して使用しない
    this.passwordMinSameCharacterRepeatCtrl = new FormControl("", [Validators.maxLength(2)]); // 同じ文字を連続して使用しない（文字以上）

    this.cbxPasswordExpirationDaysCtrl = new FormControl(""); // パスワード有効期限
    this.passwordExpirationDaysCtrl = new FormControl("", [Validators.maxLength(2)]); // パスワード有効期限（日経過したらパスワードを変更する）

    this.passwordHistoryCountCtrl = new FormControl("", [Validators.maxLength(2)]); // パスワード履歴の保存数（個：最大10個）
  }

  private setValidator() {
    this.MyFormGroup = new FormGroup({
      rdoUseReceiptSectionCtrl: this.rdoUseReceiptSectionCtrl,// 入金部門管理
      rdoUseAuthorizationCtrl: this.rdoUseAuthorizationCtrl,// 承認機能
      // 締め処理
      rdoUseForeignCurrencyCtrl: this.rdoUseForeignCurrencyCtrl,// 外貨対応
      rdoUseCashOnDueDatesCtrl: this.rdoUseCashOnDueDatesCtrl,// 期日入金管理
      // ファクタリング
      rdoUseAccountTransferCtrl: this.rdoUseAccountTransferCtrl,// 口座振替対応
      // 決済結果照合機能
      rdoUseBillingFilterCtrl: this.rdoUseBillingFilterCtrl,// 請求絞込機能
      rdoUseScheduledPaymentCtrl: this.rdoUseScheduledPaymentCtrl,// 入金予定入力
      rdoUseDeclaredAmountCtrl: this.rdoUseDeclaredAmountCtrl,// 予定額を消込額に使用
      rdoUsePublishInvoiceCtrl: this.rdoUsePublishInvoiceCtrl,// 請求書発行機能利用
      rdoUseReminderCtrl: this.rdoUseReminderCtrl,// 督促管理利用
      rdoUseDistributionCtrl: this.rdoUseDistributionCtrl,// 配信機能利用
      rdoUseMFWebApiCtrl: this.rdoUseMFWebApiCtrl,// MFクラウド請求書 WebAPI利用
      rdoUseHatarakuDBWebApiCtrl: this.rdoUseHatarakuDBWebApiCtrl,// 働くDB WebAPI 利用
      rdoUsePCADXWebApiCtrl: this.rdoUsePCADXWebApiCtrl,// PCA会計DX WebAPI利用
      rdoUseMfAggregationCtrl: this.rdoUseMfAggregationCtrl,// PCA会計DX WebAPI利用
      // 奉行クラウド WebAPI 連携
      //rdoRegisterContractInAdvanceCtrl:this.rdoRegisterContractInAdvanceCtrl,// 契約マスターの自動登録
      //rdoUseDiscountCtrl:this.rdoUseDiscountCtrl,// 歩引対応

      departmentCodeLengthCtrl: this.departmentCodeLengthCtrl, // 請求部門コード桁数
      cmbDepartmentCodeTypeCtrl: this.cmbDepartmentCodeTypeCtrl,

      accountTitleCodeLengthCtrl: this.accountTitleCodeLengthCtrl, // 科目コード桁数
      cmbAccountTitleCodeTypeCtrl: this.cmbAccountTitleCodeTypeCtrl,

      customerCodeLengthCtrl: this.customerCodeLengthCtrl, // 得意先コード桁数
      cmbCustomerCodeTypeCtrl: this.cmbCustomerCodeTypeCtrl,

      loginUserCodeLengthCtrl: this.loginUserCodeLengthCtrl, // ユーザーコード桁数
      cmbLoginUserCodeTypeCtrl: this.cmbLoginUserCodeTypeCtrl,

      sectionCodeLengthCtrl: this.sectionCodeLengthCtrl, // 入金部門桁数
      cmbSectionCodeTypeCtrl: this.cmbSectionCodeTypeCtrl,

      staffCodeLengthCtrl: this.staffCodeLengthCtrl, // 営業担当者コード桁数
      cmbStaffCodeTypeCtrl: this.cmbStaffCodeTypeCtrl,

      licenseNumCtrl: this.licenseNumCtrl, // ライセンス数


      ////////////////////////////////////////////////////////////////////////////////////////////////
      passwordMinLengthCtrl: this.passwordMinLengthCtrl, // パスワード文字数
      passwordMaxLengthCtrl: this.passwordMaxLengthCtrl, // パスワード文字数（文字以上）

      cbxPasswordUseAlphabetCtrl: this.cbxPasswordUseAlphabetCtrl, // パスワード文字種類（アルファベット）
      passwordMinAlphabetUseCountCtrl: this.passwordMinAlphabetUseCountCtrl, // パスワード文字種類（アルファベット）（文字以上含む）
      cbxPasswordCaseSensitiveCtrl: this.cbxPasswordCaseSensitiveCtrl, // パスワード文字種類（アルファベット）（大文字と小文字の区別をする）

      cbxPasswordUseNumberCtrl: this.cbxPasswordUseNumberCtrl, // パスワード文字種類（数字）
      passwordMinNumberUseCountCtrl: this.passwordMinNumberUseCountCtrl, // パスワード文字種類（数字）（文字以上含む）

      cbxPasswordUseSymbolCtrl: this.cbxPasswordUseSymbolCtrl, // パスワード文字種類（記号）
      passwordMinSymbolUseCountCtrl: this.passwordMinSymbolUseCountCtrl, // パスワード文字種類（記号）（文字以上含む）
      cbxPasswordCheckAllSymbolsCtrl: this.cbxPasswordCheckAllSymbolsCtrl, // パスワード文字種類（記号）（全てにチェックを入れる）

      cbxPasswordMinSameCharacterRepeatCtrl: this.cbxPasswordMinSameCharacterRepeatCtrl, // 同じ文字を連続して使用しない
      passwordMinSameCharacterRepeatCtrl: this.passwordMinSameCharacterRepeatCtrl, // 同じ文字を連続して使用しない（文字以上）

      cbxPasswordExpirationDaysCtrl: this.cbxPasswordExpirationDaysCtrl, // パスワード有効期限
      passwordExpirationDaysCtrl: this.passwordExpirationDaysCtrl, // パスワード有効期限（日経過したらパスワードを変更する）

      passwordHistoryCountCtrl: this.passwordHistoryCountCtrl, // パスワード履歴の保存数（個：最大10個）

      undefineCtrl: this.undefineCtrl, // 未定用;

      cmbMenuAuthorityLevelCtrl: this.cmbMenuAuthorityLevelCtrl, // 一括メニュー権限レベル      
    });

    ////////////////////////////////////////////////////////////////////////////////////////////////
    /*
    for(let index=0;index<this.cbxMenuAuthorityCtrls.length;index++){
          for(let index2=0;index2<this.cbxMenuAuthorityCtrls[index].length;index2++){
            this.MyFormGroup.addControl("cbxMenuAuthorityCtrl"+index+"-"+index2,this.cbxMenuAuthorityCtrls[index][index2]);
          }
     }

    for(let index=0;index<this.cbxSecurityAuthorityCtrls.length;index++){
      for(let index2=0;index2<this.cbxSecurityAuthorityCtrls[index].length;index2++){
        this.MyFormGroup.addControl("cbxSecurityAuthorityCtrl"+index+"-"+index2,this.cbxSecurityAuthorityCtrls[index][index2]);
      }
    }
    */
  }

  private setFormatter() {
    FormatterUtil.setNumberFormatter(this.departmentCodeLengthCtrl); // 請求部門コード桁数
    FormatterUtil.setNumberFormatter(this.accountTitleCodeLengthCtrl); // 科目コード桁数
    FormatterUtil.setNumberFormatter(this.customerCodeLengthCtrl); // 得意先コード桁数
    FormatterUtil.setNumberFormatter(this.loginUserCodeLengthCtrl); // ユーザーコード桁数
    FormatterUtil.setNumberFormatter(this.sectionCodeLengthCtrl); // 入金部門桁数
    FormatterUtil.setNumberFormatter(this.staffCodeLengthCtrl); // 営業担当者コード桁数

    FormatterUtil.setNumberFormatter(this.passwordMinLengthCtrl); // パスワード文字数
    FormatterUtil.setNumberFormatter(this.passwordMaxLengthCtrl); // パスワード文字数（文字以上）
    FormatterUtil.setNumberFormatter(this.passwordMinAlphabetUseCountCtrl); // パスワード文字種類（アルファベット）（文字以上含む）
    FormatterUtil.setNumberFormatter(this.passwordMinNumberUseCountCtrl); // パスワード文字種類（数字）（文字以上含む）
    FormatterUtil.setNumberFormatter(this.passwordMinSymbolUseCountCtrl); // パスワード文字種類（記号）（文字以上含む）
    FormatterUtil.setNumberFormatter(this.passwordMinSameCharacterRepeatCtrl); // 同じ文字を連続して使用しない（文字以上）
    FormatterUtil.setNumberFormatter(this.passwordExpirationDaysCtrl); // パスワード有効期限（日経過したらパスワードを変更する）
    FormatterUtil.setNumberFormatter(this.passwordHistoryCountCtrl); // パスワード履歴の保存数
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

      case BUTTON_ACTION.REDISPLAY:
        this.redisplay();
        break;

      default:
        console.log('buttonAction Error.');
        break;
    }
  }

  public clear() {
    this.MyFormGroup.reset();

    this.selectedCompany = false;

    this.updateScreen();

    this.panelCompanyOpenState = true;
    this.panelFunctionOpenState = true;
    this.panelPrivilegeOpenState = true;
    this.panelPasswordOpenState = true;
  }

  public currentCompany: Company = this.userInfoService.Company;
  public currentApplicationControl: ApplicationControl;
  public currentPasswordPolicy: PasswordPolicy;
  public currentLoginUserLicenses: LoginUserLicense[];
  public currentClosingInformation: ClosingInformation;

  public currentMenuSet = new Set();
  public currentFunctionSet = new Set();

  //////////////////////////////////////////////////////////////////////

  public updateScreen() {
    let componentFactory = this.componentFactoryResolver.resolveComponentFactory(ModalRouterProgressComponent);
    let modalRouterProgressComponentRef = this.viewContainerRef.createComponent(componentFactory);

    let companyId = this.userInfoService.Company.id;
    let loginUserLicense = new LoginUserLicense();
    loginUserLicense.companyId = companyId;

    let applicationResponse = this.applicationControlService.Get(companyId);
    let passwordPolicyResponse = this.passwordPolicyService.Get(companyId);
    let licenceResponse = this.loginUserLicenseService.GetItems(loginUserLicense);
    let closingResponse = this.closingService.GetClosingInformation(companyId);

    forkJoin(
      applicationResponse,
      passwordPolicyResponse,
      licenceResponse,
      closingResponse
    ).subscribe(responseList => {
      if (responseList != undefined && responseList.length == 4) {
        this.currentApplicationControl = responseList[0];
        this.currentPasswordPolicy = responseList[1];
        this.currentLoginUserLicenses = responseList[2];
        this.currentClosingInformation = responseList[3];
        this.setFormValue();
        // this.processResultService.processAtSuccess(this.processCustomResult, MSG_INF.PROCESS_FINISH);
        HtmlUtil.nextFocusByName(this.elementRef, 'companyNameCtrl', EVENT_TYPE.NONE);
      }
      modalRouterProgressComponentRef.destroy();
    });
    this.selectedCompany = true;
  }

  /**
   * 再表示
   */
  public redisplay() {
    let componentFactory = this.componentFactoryResolver.resolveComponentFactory(ModalRouterProgressComponent);
    let componentRef = this.viewContainerRef.createComponent(componentFactory);
    // componentRef.destroy();

    let companySearch = new CompanySearch();
    companySearch.id = this.userInfoService.Company.id;
    companySearch.code = this.userInfoService.Company.code;
    companySearch.name = this.userInfoService.Company.name;

    this.companyService.GetItems(companySearch.code)
      .subscribe(result => {
        this.processCustomResult = this.processResultService.processAtGetData(
          this.processCustomResult, result, true, this.partsResultMessageComponent);
        if (this.processCustomResult.result == PROCESS_RESULT_RESULT_TYPE.SUCCESS) {
          this.partsCompany.setUserInfo(result);
        }
        componentRef.destroy();
      });
  }

  private setFormValue() {
    ///////////////////////////////////////////////////////////////
    // 機能設定
    ///////////////////////////////////////////////////////////////
    if (this.currentApplicationControl) {
      let ac = this.currentApplicationControl;

      this.rdoUseReceiptSectionCtrl.setValue("" + ac.useReceiptSection);   // 入金部門管理
      this.rdoUseAuthorizationCtrl.setValue("" + ac.useAuthorization);     // 承認機能
      //this.rdoUseClosingCtrl.setValue(ac.useClosing);// 締め処理
      this.rdoUseForeignCurrencyCtrl.setValue("" + ac.useForeignCurrency);// 外貨対応
      this.rdoUseCashOnDueDatesCtrl.setValue("" + ac.useCashOnDueDates);// 期日入金管理
      //this.rdoUseFactoringCtrl.setValue(ac.useFactoring);// ファクタリング 
      this.rdoUseAccountTransferCtrl.setValue("" + ac.useAccountTransfer);// 口座振替対応
      //this.rdoUseSettlementResultCollationCtrl.setValue(ac.UseSettlementResultCollation);// 決済結果照合機能
      this.rdoUseBillingFilterCtrl.setValue("" + ac.useBillingFilter);// 請求絞込機能
      this.rdoUseScheduledPaymentCtrl.setValue("" + ac.useScheduledPayment);// 入金予定入力
      this.rdoUseDeclaredAmountCtrl.setValue("" + ac.useDeclaredAmount);// 予定額を消込額に使用
      this.rdoUsePublishInvoiceCtrl.setValue("" + ac.usePublishInvoice);// 請求書発行機能利用
      this.rdoUseReminderCtrl.setValue("" + ac.useReminder);// 督促管理利用
      this.rdoUseDistributionCtrl.setValue("" + ac.useDistribution);// 配信機能利用
      this.rdoUseMFWebApiCtrl.setValue("" + ac.useMFWebApi);// MFクラウド請求書 WebAPI利用
      this.rdoUseHatarakuDBWebApiCtrl.setValue("" + ac.useHatarakuDBWebApi);// 働くDB WebAPI 利用
      this.rdoUsePCADXWebApiCtrl.setValue("" + ac.usePCADXWebApi);// PCA会計DX WebAPI利用
      this.rdoUseMfAggregationCtrl.setValue("" + ac.useMfAggregation);// PCA会計DX WebAPI利用
      //this.rdoUseBugyoWebApiCtrl.setValue(ac.UseBugyoWebApi);// 奉行クラウド WebAPI 連携
      //this.rdoRegisterContractInAdvanceCtrl.setValue(ac.registerContractInAdvance);// 契約マスターの自動登録
      //this.rdoUseDiscountCtrl.setValue(ac.useDiscount);// 歩引対応

      //btnAddLicense.Enabled = true;
      //lblLicense.Enabled = true;
      this.licenseNumCtrl.setValue(this.currentLoginUserLicenses.length);

      this.departmentCodeLengthCtrl.setValue(ac.departmentCodeLength);
      this.accountTitleCodeLengthCtrl.setValue(ac.accountTitleCodeLength);
      this.customerCodeLengthCtrl.setValue(ac.customerCodeLength);
      this.loginUserCodeLengthCtrl.setValue(ac.loginUserCodeLength);
      this.sectionCodeLengthCtrl.setValue(ac.sectionCodeLength);
      this.staffCodeLengthCtrl.setValue(ac.staffCodeLength);

      this.cmbDepartmentCodeTypeCtrl.setValue(ac.departmentCodeType);
      this.cmbAccountTitleCodeTypeCtrl.setValue(ac.accountTitleCodeType);
      this.cmbCustomerCodeTypeCtrl.setValue(ac.customerCodeType);
      this.cmbLoginUserCodeTypeCtrl.setValue(ac.loginUserCodeType);
      this.cmbSectionCodeTypeCtrl.setValue(ac.sectionCodeType);
      this.cmbStaffCodeTypeCtrl.setValue(ac.staffCodeType);

      this.rdoUseReceiptSectionCtrl.disable();   // 入金部門管理
      this.rdoUseAuthorizationCtrl.disable();    // 承認機能
      //this.rdoUseClosingCtrl.disable(); // 締め処理
      this.rdoUseForeignCurrencyCtrl.disable();// 外貨対応
      this.rdoUseCashOnDueDatesCtrl.disable();// 期日入金管理
      //this.rdoUseFactoringCtrl.disable();// ファクタリング 
      this.rdoUseAccountTransferCtrl.disable();// 口座振替対応
      //this.rdoUseSettlementResultCollationCtrl.disable();// 決済結果照合機能
      this.rdoUseBillingFilterCtrl.disable();// 請求絞込機能
      this.rdoUseScheduledPaymentCtrl.disable();// 入金予定入力
      this.rdoUseDeclaredAmountCtrl.disable();// 予定額を消込額に使用
      this.rdoUsePublishInvoiceCtrl.disable();// 請求書発行機能利用
      this.rdoUseReminderCtrl.disable();// 督促管理利用
      this.rdoUseDistributionCtrl.disable();// 配信機能利用
      this.rdoUseMFWebApiCtrl.disable();// MFクラウド請求書 WebAPI利用
      this.rdoUseHatarakuDBWebApiCtrl.disable();// 働くDB WebAPI 利用
      this.rdoUsePCADXWebApiCtrl.disable();// PCA会計DX WebAPI利用
      this.rdoUseMfAggregationCtrl.disable();// PCA会計DX WebAPI利用
      //this.rdoUseBugyoWebApiCtrl.disable();// 奉行クラウド WebAPI 連携
      //this.rdoRegisterContractInAdvanceCtrl.disable();// 契約マスターの自動登録
      //this.rdoUseDiscountCtrl.disable();// 歩引対応

      //btnAddLicense.Enabled = true;
      //lblLicense.Enabled = true;
      this.licenseNumCtrl.disable();

      this.departmentCodeLengthCtrl.disable();
      this.accountTitleCodeLengthCtrl.disable();
      this.customerCodeLengthCtrl.disable();
      this.loginUserCodeLengthCtrl.disable();
      this.sectionCodeLengthCtrl.disable();
      this.staffCodeLengthCtrl.disable();

      this.cmbDepartmentCodeTypeCtrl.disable();
      this.cmbAccountTitleCodeTypeCtrl.disable();
      this.cmbCustomerCodeTypeCtrl.disable();
      this.cmbLoginUserCodeTypeCtrl.disable();
      this.cmbSectionCodeTypeCtrl.disable();
      this.cmbStaffCodeTypeCtrl.disable();
    }

    ///////////////////////////////////////////////////////////////
    // 権限設定
    ///////////////////////////////////////////////////////////////
    if (this.currentCompany != null) // 新規登録時は初期値をセット
    {
      this.menuAuthoritiesResult = new MenuAuthoritiesResult();
      this.menuAuthorityService.GetItemsByCompany(this.currentCompany.id)
        .subscribe(response => {
          this.menuAuthoritiesResult.menuAuthorities = response;

          let app = this.userInfoService.ApplicationControl;
          this.menuAuthoritiesResult.menuAuthorities
            = this.menuAuthoritiesResult.menuAuthorities.filter(
              function (value, index) {

                if (
                  value.menuId == "PB1101"
                  || value.menuId == "PB1201"
                  || value.menuId == "PB1301"
                  || value.menuId == "PD0801"
                ) {
                  return app.useReceiptSection == 1;
                }
                else if (
                  value.menuId == "PB2101"
                  || value.menuId == "PD1102"
                ) {
                  return app.useForeignCurrency == 1;
                }
                else {
                  return true;
                }

              });

          let hiddenMenuIds = MenuOption.getHiddenMenuIds(this.userInfoService.ApplicationControl);
          this.menuAuthoritiesResult.menuAuthorities.sort(
            function (a, b) {
              if (a.sequence < b.sequence) return -1;
              if (a.sequence > b.sequence) return 1;
              return 0;
            });
          this.menuAuthoritiesResult.menuAuthorities.forEach(element => {
            if (hiddenMenuIds.indexOf(element.menuId) < 0) {
              this.currentMenuSet.add(element.menuId + ":" + element.menuName);
            }
          });

          this.cbxMenuAuthorityCtrls = new Array<FormControl[]>(this.currentMenuSet.size);  // メニュー権限チェック

          let index = 0;
          this.currentMenuSet.forEach(menuItem => {
            this.cbxMenuAuthorityCtrls[index] = new Array<FormControl>(4);
            let menuAuthorityIndex = this.menuAuthoritiesResult.menuAuthorities.findIndex((item) => {
              return (item.menuId === this.getMenuId(String(menuItem)));
            });
            if (0 <= menuAuthorityIndex) {
              for (let innerIndex = 0; innerIndex < 4; innerIndex++) {
                this.cbxMenuAuthorityCtrls[index][innerIndex] = new FormControl("");

                if (innerIndex == 0) {
                  this.cbxMenuAuthorityCtrls[index][innerIndex].disable();
                }

                this.menuAuthoritiesResult.menuAuthorities[menuAuthorityIndex].available == 1 ?
                  this.cbxMenuAuthorityCtrls[index][innerIndex].setValue("true")
                  : this.cbxMenuAuthorityCtrls[index][innerIndex].setValue(null);

                this.MyFormGroup.removeControl("cbxMenuAuthorityCtrl_" + this.getMenuId(String(menuItem)) + "_" + (innerIndex + 1));
                this.MyFormGroup.addControl("cbxMenuAuthorityCtrl_" + this.getMenuId(String(menuItem)) + "_" + (innerIndex + 1), this.cbxMenuAuthorityCtrls[index][innerIndex]);

                menuAuthorityIndex++;
              }
              index++;
            }
          })
        })

      this.functionAuthoritiesResult = new FunctionAuthoritiesResult();
      this.functionAuthorityService.GetItems(this.currentCompany.id)
        .subscribe(response => {
          this.functionAuthoritiesResult.functionAuthorities = response;

          this.functionAuthoritiesResult.functionAuthorities
            = this.functionAuthoritiesResult.functionAuthorities.sort(
              function (a: FunctionAuthority, b: FunctionAuthority) {
                if (a.functionType < b.functionType) return -1;
                if (a.functionType > b.functionType) return 1;
                if (a.authorityLevel < b.authorityLevel) return -1;
                if (a.authorityLevel > b.authorityLevel) return 1;
                return 0;
              });

          this.functionAuthoritiesResult.functionAuthorities.forEach(
            element => {
              this.currentFunctionSet.add(element.functionType)
            });

          this.cbxSecurityAuthorityCtrls = new Array<FormControl[]>(this.currentFunctionSet.size);  // セキュリティ権限チェック

          let allCount = 0;
          let index = 0;
          this.currentFunctionSet.forEach(menuItem => {

            this.cbxSecurityAuthorityCtrls[index] = new Array<FormControl>(6);

            for (let innerIndex = 0; innerIndex < 6; innerIndex++) {
              this.cbxSecurityAuthorityCtrls[index][innerIndex] = new FormControl("");

              if (innerIndex == 0) {
                this.cbxSecurityAuthorityCtrls[index][innerIndex].disable();
              }

              this.functionAuthoritiesResult.functionAuthorities[allCount].available ?
                this.cbxSecurityAuthorityCtrls[index][innerIndex].setValue("true")
                : this.cbxSecurityAuthorityCtrls[index][innerIndex].setValue(null);

              this.MyFormGroup.removeControl("cbxSecurityAuthorityCtrl_" + menuItem + "_" + (innerIndex + 1));
              this.MyFormGroup.addControl("cbxSecurityAuthorityCtrl_" + menuItem + "_" + (innerIndex + 1), this.cbxSecurityAuthorityCtrls[index][innerIndex]);

              allCount++;
            }
            index++;
          })
        })

      /*
      var task = Task.Run(async () =>
      {
          initialMenuAuthorityList = await GetMenuAuthorityListAsync(Login.SessionKey, null, null);
      });
      ProgressDialog.Start(BaseForm, task, false, SessionKey);

      grdMenuAuthority.DataSource = initialMenuAuthorityList
      .GroupBy(ma => ma.MenuId)
      .Select(group => new MenuAuthorityGridRow(group.ToList()))
      .ToList();

      var initialFunctionAuthorityList = new List<FunctionAuthorityGridRow>
      {
          new FunctionAuthorityGridRow(FunctionType.MasterImport),
          new FunctionAuthorityGridRow(FunctionType.MasterExport),
          new FunctionAuthorityGridRow(FunctionType.ModifyBilling),
          new FunctionAuthorityGridRow(FunctionType.RecoverBilling),
          new FunctionAuthorityGridRow(FunctionType.ModifyReceipt),
          new FunctionAuthorityGridRow(FunctionType.RecoverReceipt),
          new FunctionAuthorityGridRow(FunctionType.CancelMatching),
      };
      grdFunctionAuthority.DataSource = initialFunctionAuthorityList;
      return;
      */
    }

    /*
    var menuAuthorityList = await GetMenuAuthorityListAsync(Login.SessionKey, CurrentCompany.Id, null);
    grdMenuAuthority.DataSource = menuAuthorityList
        .GroupBy(ma => ma.MenuId)
        .Select(group => new MenuAuthorityGridRow(group.ToList()))
        .ToList();

    var functionAuthorityList = await GetFunctionAuthorityListAsync(Login.SessionKey, CurrentCompany.Id);
    grdFunctionAuthority.DataSource = functionAuthorityList
        .GroupBy(fa => fa.FunctionType)
        .Select(group => new FunctionAuthorityGridRow(group.ToList()))
        .ToList();
    */

    ///////////////////////////////////////////////////////////////
    // パスワードの設定
    ///////////////////////////////////////////////////////////////
    var pw = this.currentPasswordPolicy;
    if (pw != null) {
      // 統一的な制御はできそうになく逆に複雑になりそうだったので、下記の考えで実装した。
      // (1) 一度全コントロールの値をDB取得値のままセットする(Min/Max範囲外のものはnull値に変換)。
      // (2) その後、チェックされていないチェックボックスに関連するコントロールを無効化し入力値をクリアする。

      // (1)
      this.passwordMinLengthCtrl.setValue(pw.minLength);
      this.passwordMaxLengthCtrl.setValue(pw.maxLength);

      pw.useAlphabet == 1 ?
        this.cbxPasswordUseAlphabetCtrl.setValue("true")
        : this.cbxPasswordUseAlphabetCtrl.setValue(null);

      pw.useNumber == 1 ?
        this.cbxPasswordUseNumberCtrl.setValue("true")
        : this.cbxPasswordUseNumberCtrl.setValue(null);

      pw.useSymbol == 1 ?
        this.cbxPasswordUseSymbolCtrl.setValue("true")
        : this.cbxPasswordUseSymbolCtrl.setValue(null);

      this.passwordMinAlphabetUseCountCtrl.setValue(pw.minAlphabetUseCount);
      this.passwordMinNumberUseCountCtrl.setValue(pw.minNumberUseCount);
      this.passwordMinSymbolUseCountCtrl.setValue(pw.minSymbolUseCount);

      pw.caseSensitive == 1 ?
        this.cbxPasswordCaseSensitiveCtrl.setValue("true")
        : this.cbxPasswordCaseSensitiveCtrl.setValue(null);
      this.cbxPasswordCaseSensitiveCtrl.disable();

      let rowIndex = 0;
      let pwSymbol = this.loginService.fullSymbolChars;
      this.passwordSymbolRow[rowIndex] = new Array<string>();
      this.cbxPasswordSymbolCtrls = new Array<FormControl>(pwSymbol.length);
      for (let i = 0; i < pwSymbol.length; i++) {
        this.cbxPasswordSymbolCtrls[i] = new FormControl("");
        if (0 <= pw.symbolType.indexOf(pwSymbol[i])) {
          this.cbxPasswordSymbolCtrls[i].setValue("true");
        } else {
          this.cbxPasswordSymbolCtrls[i].setValue(null);
        }
        this.MyFormGroup.removeControl("cbxPasswordSymbolCtrl" + i);
        this.MyFormGroup.addControl("cbxPasswordSymbolCtrl" + i, this.cbxPasswordSymbolCtrls[i]);
        this.passwordSymbolRow[rowIndex].push(pwSymbol[i]);
        if (this.passwordSymbolRow[rowIndex].length == this.symbolColNumber) {
          rowIndex++;
          this.passwordSymbolRow[rowIndex] = new Array<string>();
        }
      }

      this.cbxPasswordCheckAllSymbolsCtrl.setValue("true");
      this.cbxPasswordSymbolCtrls.forEach(element => {
        if (!element.value) {
          this.cbxPasswordCheckAllSymbolsCtrl.setValue(null);
        }
      });

      this.passwordMinSameCharacterRepeatCtrl.setValue(pw.minSameCharacterRepeat);
      this.passwordHistoryCountCtrl.setValue(pw.historyCount);

      pw.minSameCharacterRepeat == 0 ?
        this.cbxPasswordMinSameCharacterRepeatCtrl.setValue(null) :
        this.cbxPasswordMinSameCharacterRepeatCtrl.setValue("true");

      if (pw.expirationDays > 0) {
        this.cbxPasswordExpirationDaysCtrl.setValue(true);
        this.passwordExpirationDaysCtrl.setValue(pw.expirationDays);
      } else {
        this.cbxPasswordExpirationDaysCtrl.setValue(false);
        this.passwordExpirationDaysCtrl.setValue(null);
      }

      // (2)
      if (!this.cbxPasswordUseAlphabetCtrl.value) {
        this.passwordMinAlphabetUseCountCtrl.setValue("");
        this.passwordMinAlphabetUseCountCtrl.disable();
      }

      if (!this.cbxPasswordUseNumberCtrl.value) {
        this.passwordMinNumberUseCountCtrl.setValue("");
        this.passwordMinNumberUseCountCtrl.disable();
      }

      if (!this.cbxPasswordUseSymbolCtrl.value) {
        this.passwordMinSymbolUseCountCtrl.setValue(null);
        this.cbxPasswordCheckAllSymbolsCtrl.setValue(null);
        this.passwordMinSymbolUseCountCtrl.disable();
        this.cbxPasswordCheckAllSymbolsCtrl.disable();
        this.changeAvailability(false);
      }

      if (!this.cbxPasswordMinSameCharacterRepeatCtrl.value) {
        this.passwordMinSameCharacterRepeatCtrl.setValue("");
        this.passwordMinSameCharacterRepeatCtrl.disable();
      }

      if (!this.cbxPasswordExpirationDaysCtrl.value) {
        this.passwordExpirationDaysCtrl.setValue("");
        this.passwordExpirationDaysCtrl.disable();
      }
    }
  }

  public getMenuName(menuItem: string): string {
    let menuItems = menuItem.split(":");
    return menuItems[1];
  }

  public getMenuId(menuItem: string): string {
    let menuItems = menuItem.split(":");
    return menuItems[0];
  }

  public getFunctionName(functionType: FunctionType): string {
    let strRtn: string;
    switch (functionType) {
      /// <summary>0:マスターインポート</summary>
      case FunctionType.MasterImport:
        {
          strRtn = "マスターインポート";
          break;
        }
      /// <summary>1:マスターエクスポート</summary>
      case FunctionType.MasterExport:
        {
          strRtn = "マスターエクスポート";
          break;
        }
      /// <summary>2:請求データ修正・削除</summary>
      case FunctionType.ModifyBilling:
        {
          strRtn = "請求データ修正・削除";
          break;
        }
      /// <summary>3:請求データ復活</summary>
      case FunctionType.RecoverBilling:
        {
          strRtn = "請求データ復活";
          break;
        }
      /// <summary>4:入金データ修正・削除</summary>
      case FunctionType.ModifyReceipt:
        {
          strRtn = "入金データ修正・削除";
          break;
        }
      /// <summary>5:入金データ復活</summary>
      case FunctionType.RecoverReceipt:
        {
          strRtn = "入金データ復活";
          break;
        }
      /// <summary>6:消込解除</summary>
      case FunctionType.CancelMatching:
        {
          strRtn = "消込解除";
          break;
        }
    }
    return strRtn;
  }

  public setAuthorityAllLevel(level: number, checked: boolean = null) {
    for (let index = 0; index < this.cbxMenuAuthorityCtrls.length; index++) {
      this.cbxMenuAuthorityCtrls[index][level - 1].setValue(checked);
    }
  }

  public setSecurityAllLevel(level: number, checked: boolean = null) {
    for (let index = 0; index < this.cbxSecurityAuthorityCtrls.length; index++) {
      this.cbxSecurityAuthorityCtrls[index][level - 1].setValue(checked);
    }
  }

  public changeAvailability(enable: boolean, value: boolean = false) {
    this.cbxPasswordSymbolCtrls.forEach(control => {
      if (enable) {
        control.enable();
        control.setValue(value);
      }
      else {
        control.disable();
        control.setValue(null);
      }
    });
  }

  ///////////////////////////////////////////////////////////////////////  
  /**
   * データ登録・編集
   */
  public registry() {
    if (!this.validateInput()) {
      return;
    }

    let componentFactory = this.componentFactoryResolver.resolveComponentFactory(ModalRouterProgressComponent);
    let componentRef = this.viewContainerRef.createComponent(componentFactory);
    componentRef.instance.Closing.subscribe(() => {
      componentRef.destroy();
    });

    let companyLogos = new Array<CompanyLogo>();
    let companySource = new CompanySource();
    companySource.company = this.getCompanyData();
    companySource.applicationControl = this.getApplicationControl();
    companySource.passwordPolicy = this.getPasswordPolicy();
    companySource.loginUserLicense = this.getLoginUserLicense();
    companySource.menuAuthorities = this.getMenuAuthorities();
    companySource.functionAuthorities = this.getFunctionAuthorities();
    companySource.saveCompanyLogos = companyLogos;
    companySource.deleteCompanyLogos = companyLogos;

    this.companyService.Initialize(companySource)
      .subscribe(result => {
        this.processCustomResult = this.processResultService.processAtSave(
          this.processCustomResult, result, false, this.partsResultMessageComponent);
        if (result != PROCESS_RESULT_RESULT_TYPE.FAILURE && this.processCustomResult.result == PROCESS_RESULT_RESULT_TYPE.SUCCESS) {
          this.clear();
        }
        componentRef.destroy();

      });

  }

  public validateInput(): boolean {

    let arg1: string = "";
    if (this.cbxPasswordUseAlphabetCtrl.value && StringUtil.IsNullOrEmpty(this.passwordMinAlphabetUseCountCtrl.value)) {
      arg1 = "アルファベット文字数";
    }
    else if (this.cbxPasswordUseNumberCtrl.value && StringUtil.IsNullOrEmpty(this.passwordMinNumberUseCountCtrl.value)) {
      arg1 = "数字文字数";
    }
    else if (this.cbxPasswordUseSymbolCtrl.value && StringUtil.IsNullOrEmpty(this.passwordMinSymbolUseCountCtrl.value)) {
      arg1 = "記号文字数";
    }
    else if (this.cbxPasswordMinSameCharacterRepeatCtrl.value && StringUtil.IsNullOrEmpty(this.passwordMinSameCharacterRepeatCtrl.value)) {
      arg1 = "連続使用不可文字数";
    }
    else if (this.cbxPasswordExpirationDaysCtrl.value && StringUtil.IsNullOrEmpty(this.passwordExpirationDaysCtrl.value)) {
      arg1 = "パスワード有効期間日数";
    }

    if (!StringUtil.IsNullOrEmpty(arg1)) {
      this.processResultService.processAtWarning(
        this.processCustomResult, MSG_WNG.INPUT_REQUIRED.replace(MSG_ITEM_NUM.FIRST, arg1),
        this.partsResultMessageComponent);
      return false;
    }

    if (this.passwordMaxLengthCtrl.value < this.passwordMinLengthCtrl.value) {
      arg1 = "パスワード文字数";
    }

    if (!StringUtil.IsNullOrEmpty(arg1)) {
      this.processResultService.processAtWarning(
        this.processCustomResult, MSG_WNG.INPUT_RANGE_CHECKED.replace(MSG_ITEM_NUM.FIRST, arg1),
        this.partsResultMessageComponent);
      return false;
    }

    if ((!this.cbxPasswordUseAlphabetCtrl.value) && (!this.cbxPasswordUseNumberCtrl.value) && (!this.cbxPasswordUseSymbolCtrl.value)) {
      arg1 = "使用する文字の種類";
    }
    else if (this.cbxPasswordUseSymbolCtrl.value && this.cbxPasswordSymbolCtrls.every(x => !x.value)) {
      arg1 = "使用する文字の種類";
    }

    if (!StringUtil.IsNullOrEmpty(arg1)) {
      this.processResultService.processAtWarning(
        this.processCustomResult, MSG_WNG.SELECTION_REQUIRED.replace(MSG_ITEM_NUM.FIRST, arg1),
        this.partsResultMessageComponent);
      return false;
    }

    return true;
  }

  public getCompanyData(): Company {
    let company = new Company();

    if (this.currentCompany == undefined) {
      company.createBy = this.userInfoService.LoginUser.id;
      company.updateBy = this.userInfoService.LoginUser.id;
    }
    else {
      company = this.userInfoService.Company;
    }

    company.productKey = this.partsCompany.productKeyCtrl.value;
    //会社情報
    company.code = this.partsCompany.companyCodeCtrl.value;
    company.name = this.partsCompany.companyNameCtrl.value;
    company.kana = this.partsCompany.companyNameKanaCtrl.value;
    company.postalCode = this.partsCompany.postalCodeCtrl.value;
    company.address1 = this.partsCompany.address1Ctrl.value;
    company.address2 = this.partsCompany.address2Ctrl.value;
    company.tel = this.partsCompany.telCtrl.value;
    company.fax = this.partsCompany.faxCtrl.value;
    company.bankAccountName = this.partsCompany.bankAccountNameCtrl.value;
    company.bankAccountKana = this.partsCompany.bankAccountNameKanaCtrl.value;

    //銀行情報
    company.bankName1 = this.partsCompany.bankName1Ctrl.value;
    company.branchName1 = this.partsCompany.branchName1Ctrl.value;
    company.accountType1 = this.partsCompany.cmbAccountType1Ctrl.value;
    company.accountNumber1 = this.partsCompany.accountNumber1Ctrl.value;
    company.bankName2 = this.partsCompany.bankName2Ctrl.value;
    company.branchName2 = this.partsCompany.branchName2Ctrl.value;
    company.accountType2 = this.partsCompany.cmbAccountType2Ctrl.value;
    company.accountNumber2 = this.partsCompany.accountNumber2Ctrl.value;
    company.bankName3 = this.partsCompany.bankName3Ctrl.value;
    company.branchName3 = this.partsCompany.branchName3Ctrl.value;
    company.accountType3 = this.partsCompany.cmbAccountType3Ctrl.value;
    company.accountNumber3 = this.partsCompany.accountNumber3Ctrl.value;

    //締日
    company.closingDay = this.partsCompany.closingDateCtrl.value;

    //オプション
    //company.presetCodeSearchDialog = this.partsCompany.cbxPresetCodeSearchDialogCtrl.value == true ? 1 : 0;
    //company.showConfirmDialog = this.partsCompany.cbxShowConfirmDialogCtrl.value == true ? 1 : 0;
    //company.showWarningDialog = this.partsCompany.cbxShowWarningDialogCtrl.value == true ? 1 : 0;
    //company.autoCloseProgressDialog = this.partsCompany.cbxAutoCloseProgressDialogCtrl.value == true ? 1 : 0;
    company.transferAggregate = this.partsCompany.cbxTransferAggregateCtrl.value == true ? 1 : 0;

    return company;
  }

  public getApplicationControl(): ApplicationControl {
    let applicationControl: ApplicationControl;
    if (this.currentCompany == undefined && this.currentApplicationControl == undefined) {
      applicationControl = new ApplicationControl();
      applicationControl.departmentCodeLength = this.departmentCodeLengthCtrl.value;
      applicationControl.departmentCodeType = this.cmbDepartmentCodeTypeCtrl.value;
      applicationControl.accountTitleCodeLength = this.accountTitleCodeLengthCtrl.value;
      applicationControl.accountTitleCodeType = this.cmbAccountTitleCodeTypeCtrl.value;
      applicationControl.customerCodeLength = this.customerCodeLengthCtrl.value;
      applicationControl.customerCodeType = this.cmbCustomerCodeTypeCtrl.value;
      applicationControl.loginUserCodeLength = this.loginUserCodeLengthCtrl.value;
      applicationControl.loginUserCodeType = this.cmbLoginUserCodeTypeCtrl.value;
      applicationControl.sectionCodeLength = this.sectionCodeLengthCtrl.value;
      applicationControl.sectionCodeType = this.cmbSectionCodeTypeCtrl.value;
      applicationControl.staffCodeLength = this.staffCodeLengthCtrl.value;
      applicationControl.staffCodeType = this.cmbStaffCodeTypeCtrl.value;

      applicationControl.useReceiptSection = this.rdoUseReceiptSectionCtrl.value;
      applicationControl.useAuthorization = this.rdoUseAuthorizationCtrl.value;
      applicationControl.useForeignCurrency = this.rdoUseForeignCurrencyCtrl.value;
      applicationControl.useCashOnDueDates = this.rdoUseCashOnDueDatesCtrl.value;
      applicationControl.useAccountTransfer = this.rdoUseAccountTransferCtrl.value;
      applicationControl.useBillingFilter = this.rdoUseBillingFilterCtrl.value;
      applicationControl.useScheduledPayment = this.rdoUseScheduledPaymentCtrl.value;
      applicationControl.useDeclaredAmount = this.rdoUseDeclaredAmountCtrl.value;
      applicationControl.usePublishInvoice = this.rdoUsePublishInvoiceCtrl.value;
      applicationControl.useReminder = this.rdoUseReminderCtrl.value;
      applicationControl.useDistribution = this.rdoUseDistributionCtrl.value;
      applicationControl.useMFWebApi = this.rdoUseMFWebApiCtrl.value;
      applicationControl.useHatarakuDBWebApi = this.rdoUseHatarakuDBWebApiCtrl.value;
      applicationControl.usePCADXWebApi = this.rdoUsePCADXWebApiCtrl.value;
      applicationControl.useMfAggregation = this.rdoUseMfAggregationCtrl.value;
      applicationControl.createBy = this.userInfoService.LoginUser.id;
      applicationControl.updateBy = this.userInfoService.LoginUser.id;

      //利用していないオプション
      //applicationControl.useLongTermAdvanceReceived = this.rdoUseLongTermAdvanceReceivedCtrl.value;
      //applicationControl.registerContractInAdvance = this.rdoRegisterContractInAdvanceCtrl.value;
      //applicationControl.useDiscount = this.rdoUseDiscountCtrl.value;
      //applicationControl.useClosing = this.rdoUseClosingCtrl.value;
      //applicationControl.useFactoring = this.rdoUseFactoringCtrl.value;
    }

    return applicationControl;
  }

  public getPasswordPolicy(): PasswordPolicy {
    let passwordPolicy: PasswordPolicy;
    if (this.currentPasswordPolicy == undefined) {
      passwordPolicy = new PasswordPolicy();
    }
    else {
      passwordPolicy = this.currentPasswordPolicy;
    }

    passwordPolicy.minLength = Number(this.passwordMinLengthCtrl.value);
    passwordPolicy.maxLength = Number(this.passwordMaxLengthCtrl.value);
    passwordPolicy.useAlphabet = this.cbxPasswordUseAlphabetCtrl.value ? 1 : 0;
    passwordPolicy.minAlphabetUseCount = Number(this.passwordMinAlphabetUseCountCtrl.value);
    passwordPolicy.caseSensitive = this.cbxPasswordCaseSensitiveCtrl.value ? 1 : 0;
    passwordPolicy.useNumber = this.cbxPasswordUseNumberCtrl.value ? 1 : 0;
    passwordPolicy.minNumberUseCount = Number(this.passwordMinNumberUseCountCtrl.value);
    passwordPolicy.useSymbol = this.cbxPasswordUseSymbolCtrl.value ? 1 : 0;

    if (passwordPolicy.useSymbol == 0) {
      passwordPolicy.minSymbolUseCount = 0;
      passwordPolicy.symbolType = "";
    }
    else {
      passwordPolicy.minSymbolUseCount = Number(this.passwordMinSymbolUseCountCtrl.value);

      let symbols: string = "";
      for (let i = 0; i < this.cbxPasswordSymbolCtrls.length;i++) {
        if (this.cbxPasswordSymbolCtrls[i].value) symbols += this.loginService.fullSymbolChars[i];  
      }
      passwordPolicy.symbolType = symbols;
    }
    passwordPolicy.minSameCharacterRepeat = this.cbxPasswordMinSameCharacterRepeatCtrl.value ?
      Number(this.passwordMinSameCharacterRepeatCtrl.value) : 0;
    passwordPolicy.expirationDays = this.cbxPasswordExpirationDaysCtrl.value ?
      Number(this.passwordExpirationDaysCtrl.value) : 0;
    passwordPolicy.historyCount = Number(this.passwordHistoryCountCtrl.value);

    return passwordPolicy;
  }

  public getLoginUserLicense(): LoginUserLicense {
    let loginUserLicense: LoginUserLicense;

    this.companyService.GetItems(this.userInfoService.Company.code)
      .subscribe(response => {
        if (response == undefined) {
          loginUserLicense = new LoginUserLicense();
          //loginUserLicense.licenseKey = //LoginUserLicenseからLicenseKeyを取得する
        }
      });

    return loginUserLicense;
  }

  public getMenuAuthorities(): MenuAuthority[] {
    let menuAuthorities = new Array<MenuAuthority>();

    let index = 0;

    this.currentMenuSet.forEach(menuItem => {

      for (let innerIndex = 0; innerIndex < 4; innerIndex++) {
        let menuAuthority: MenuAuthority = new MenuAuthority();
        menuAuthority.companyId = this.menuAuthoritiesResult.menuAuthorities[0].companyId;
        menuAuthority.menuId = this.getMenuId(String(menuItem));
        menuAuthority.authorityLevel = innerIndex + 1;

        if (innerIndex == 0) {
          menuAuthority.available = 1;
        }
        else {
          menuAuthority.available = this.cbxMenuAuthorityCtrls[index][innerIndex].value ? 1 : 0;
        }
        menuAuthority.createBy = this.userInfoService.LoginUser.id;
        menuAuthority.updateBy = this.userInfoService.LoginUser.id;

        menuAuthorities.push(menuAuthority);

      }
      index++;
    });

    return menuAuthorities;
  }

  public getFunctionAuthorities(): FunctionAuthority[] {
    let functionAuthorities = new Array<FunctionAuthority>();

    let index = 0;

    this.currentFunctionSet.forEach(menuItem => {

      for (let innerIndex = 0; innerIndex < 6; innerIndex++) {

        let functionAuthority: FunctionAuthority = new FunctionAuthority();
        functionAuthority.companyId = this.functionAuthoritiesResult.functionAuthorities[0].companyId;
        functionAuthority.authorityLevel = innerIndex + 1;
        functionAuthority.functionType = Number(menuItem);

        if (innerIndex == 0) {
          functionAuthority.available = true;
        }
        else {
          functionAuthority.available = this.cbxSecurityAuthorityCtrls[index][innerIndex].value ? true : false;
        }
        functionAuthority.createBy = this.userInfoService.LoginUser.id;
        functionAuthority.updateBy = this.userInfoService.LoginUser.id;

        functionAuthorities.push(functionAuthority);
      }
      index++;
    });

    return functionAuthorities;
  }

  ///////////////////////////////////////////////////////////////////////2019/03/12
  public setPasswordMinLength(eventType: string) {
    if (!StringUtil.IsNullOrEmpty(this.passwordMinLengthCtrl.value)) {
      if (15 < this.passwordMinLengthCtrl.value) {
        this.passwordMinLengthCtrl.setValue(15);
      }
    }
    HtmlUtil.nextFocusByName(this.elementRef, 'passwordMaxLengthCtrl', eventType);
  }

  ///////////////////////////////////////////////////////////////////////
  public setPasswordMaxLength(eventType: string) {
    if (!StringUtil.IsNullOrEmpty(this.passwordMaxLengthCtrl.value)) {
      if (15 < this.passwordMaxLengthCtrl.value) {
        this.passwordMaxLengthCtrl.setValue(15);
      }
    }
    HtmlUtil.nextFocusByName(this.elementRef, 'passwordMinAlphabetUseCountCtrl', eventType);
  }

  ///////////////////////////////////////////////////////////////////////
  public setPasswordMinAlphabetUseCount(eventType: string) {
    if (!StringUtil.IsNullOrEmpty(this.passwordMinAlphabetUseCountCtrl.value)) {
      if (15 < this.passwordMinAlphabetUseCountCtrl.value) {
        this.passwordMinAlphabetUseCountCtrl.setValue(15);
      }
    }
    HtmlUtil.nextFocusByName(this.elementRef, 'passwordMinNumberUseCountCtrl', eventType);
  }

  ///////////////////////////////////////////////////////////////////////
  public setPasswordMinNumberUseCount(eventType: string) {
    if (!StringUtil.IsNullOrEmpty(this.passwordMinNumberUseCountCtrl.value)) {
      if (15 < this.passwordMinNumberUseCountCtrl.value) {
        this.passwordMinNumberUseCountCtrl.setValue(15);
      }
    }
    HtmlUtil.nextFocusByName(this.elementRef, 'passwordMinSymbolUseCountCtrl', eventType);
  }

  ///////////////////////////////////////////////////////////////////////
  public setPasswordMinSymbolUseCount(eventType: string) {
    if (!StringUtil.IsNullOrEmpty(this.passwordMinSymbolUseCountCtrl.value)) {
      if (15 < this.passwordMinSymbolUseCountCtrl.value) {
        this.passwordMinSymbolUseCountCtrl.setValue(15);
      }
    }
    HtmlUtil.nextFocusByName(this.elementRef, 'passwordMinSameCharacterRepeatCtrl', eventType);
  }

  ///////////////////////////////////////////////////////////////////////
  public setPasswordMinSameCharacterRepeat(eventType: string) {
    if (!StringUtil.IsNullOrEmpty(this.passwordMinSameCharacterRepeatCtrl.value)) {
      if (15 < this.passwordMinSameCharacterRepeatCtrl.value) {
        this.passwordMinSameCharacterRepeatCtrl.setValue(15);
      }
    }
    HtmlUtil.nextFocusByName(this.elementRef, 'passwordExpirationDaysCtrl', eventType);
  }

  ///////////////////////////////////////////////////////////////////////
  public setPasswordExpirationDays(eventType: string) {
    HtmlUtil.nextFocusByName(this.elementRef, 'passwordHistoryCountCtrl', eventType);
  }

  ///////////////////////////////////////////////////////////////////////
  public setCbxPasswordExpirationDays() {
    if (this.cbxPasswordExpirationDaysCtrl.value) {
      this.passwordExpirationDaysCtrl.enable();
    }
    else {
      this.passwordExpirationDaysCtrl.setValue("");
      this.passwordExpirationDaysCtrl.disable();
    }
  }

  ///////////////////////////////////////////////////////////////////////
  public setPasswordHistoryCount(eventType: string) {
    if (!StringUtil.IsNullOrEmpty(this.passwordHistoryCountCtrl.value)) {
      if (10 < this.passwordHistoryCountCtrl.value) {
        this.passwordHistoryCountCtrl.setValue(10);
      }
    }
  }

  ///////////////////////////////////////////////////////////////////////
  public setCbxPasswordUseAlphabet() {
    if (this.cbxPasswordUseAlphabetCtrl.value) {
      this.passwordMinAlphabetUseCountCtrl.enable();
    }
    else {
      this.passwordMinAlphabetUseCountCtrl.setValue("");
      this.passwordMinAlphabetUseCountCtrl.disable();
    }
  }

  ///////////////////////////////////////////////////////////////////////
  public setCbxPasswordUseNumber() {
    if (this.cbxPasswordUseNumberCtrl.value) {
      this.passwordMinNumberUseCountCtrl.enable();
    }
    else {
      this.passwordMinNumberUseCountCtrl.setValue("");
      this.passwordMinNumberUseCountCtrl.disable();
    }
  }

  ///////////////////////////////////////////////////////////////////////
  public setCbxPasswordUseSymbol() {
    if (this.cbxPasswordUseSymbolCtrl.value) {
      this.passwordMinSymbolUseCountCtrl.enable();
      this.cbxPasswordCheckAllSymbolsCtrl.enable();
    }
    else {
      this.passwordMinSymbolUseCountCtrl.setValue("");
      this.cbxPasswordCheckAllSymbolsCtrl.setValue(null);
      this.passwordMinSymbolUseCountCtrl.disable();
      this.cbxPasswordCheckAllSymbolsCtrl.disable();
    }
    this.changeAvailability(this.cbxPasswordUseSymbolCtrl.value);
  }

  ///////////////////////////////////////////////////////////////////////
  public setCbxPasswordCheckAllSymbols() {
    this.changeAvailability(true, this.cbxPasswordCheckAllSymbolsCtrl.value);
  }

  public setCbxPasswordMinSameCharacterRepeat() {
    if (this.cbxPasswordMinSameCharacterRepeatCtrl.value) {
      this.passwordMinSameCharacterRepeatCtrl.enable();
    }
    else {
      this.passwordMinSameCharacterRepeatCtrl.disable();
      this.passwordMinSameCharacterRepeatCtrl.setValue("");
    }
  }

}
