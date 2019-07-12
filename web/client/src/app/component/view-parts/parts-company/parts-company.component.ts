import { Component, OnInit, ElementRef, ComponentFactoryResolver, ViewContainerRef, Output, EventEmitter, AfterViewInit } from '@angular/core';
import { EXCLUSIVE_ACCOUNT_TYPE } from 'src/app/common/const/kbn.const';
import { FormControl, Validators, FormGroup } from '@angular/forms';
import { Router, ActivatedRoute } from '@angular/router';
import { UserInfoService } from 'src/app/service/common/user-info.service';
import { CompanyMasterService } from 'src/app/service/Master/company-master.service';
import { ProcessResultService } from 'src/app/service/common/process-result.service';
import { BaseComponent } from '../../common/base/base-component';
import { FormatStyle, FormatterUtil } from 'src/app/common/util/formatter.util';
import { CustomValidators } from 'ng5-validation';
import { HtmlUtil } from 'src/app/common/util/html.util';
import { EbDataHelper } from 'src/app/common/util/eb-data-helper';
import { StringUtil } from 'src/app/common/util/string-util';

@Component({
  selector: 'app-parts-company',
  templateUrl: './parts-company.component.html',
  styleUrls: ['./parts-company.component.css']
})
export class PartsCompanyComponent extends BaseComponent implements OnInit {

  public readonly exclusiveAccountType = EXCLUSIVE_ACCOUNT_TYPE; // 預金種別

  public companyCodeCtrl: FormControl; // 会社コード
  public productKeyCtrl: FormControl; // プロダクトコード

  public companyNameCtrl: FormControl; // 会社名
  public companyNameKanaCtrl: FormControl; // 会社カナ

  public postalCodeCtrl: FormControl; // 郵便番号

  public address1Ctrl: FormControl; // 住所
  public address2Ctrl: FormControl; // 

  public telCtrl: FormControl; // Tel
  public faxCtrl: FormControl; // Fax

  public bankAccountNameCtrl: FormControl; // 口座名義人
  public bankAccountNameKanaCtrl: FormControl; // 口座名義人(カナ)

  public bankName1Ctrl: FormControl; // 銀行名
  public branchName1Ctrl: FormControl; // 支店名
  public cmbAccountType1Ctrl: FormControl; // 預金種別
  public accountNumber1Ctrl: FormControl; // 口座番号

  public bankName2Ctrl: FormControl; // 銀行名
  public branchName2Ctrl: FormControl; // 支店名
  public cmbAccountType2Ctrl: FormControl; // 預金種別
  public accountNumber2Ctrl: FormControl; // 口座番号

  public bankName3Ctrl: FormControl; // 銀行名
  public branchName3Ctrl: FormControl; // 支店名
  public cmbAccountType3Ctrl: FormControl; // 預金種別
  public accountNumber3Ctrl: FormControl; // 口座番号

  public logoCtrl: FormControl; // ロゴ
  public squareSealCtrl: FormControl; // 社判
  public roundSealCtrl: FormControl;  // 丸印

  public closingDateCtrl: FormControl;  // 締め日

  public undefineCtrl = new FormControl; // 未定用;

  // public cbxPresetCodeSearchDialogCtrl: FormControl;  // 検索時に全件初期表示する
  // public cbxShowConfirmDialogCtrl: FormControl;  // 検索時に全件初期表示する
  // public cbxShowWarningDialogCtrl: FormControl;  // 警告時ダイアログを表示する
  // public cbxAutoCloseProgressDialogCtrl: FormControl;  // 処理状況ダイアログを自動で閉じる
  public cbxTransferAggregateCtrl: FormControl;  // 口座振替データ作成/取込時に口座単位で集計する

  public validationError: string = null;

  constructor(
    public router: Router,
    public activatedRoute: ActivatedRoute,
    public elementRef: ElementRef,
    public componentFactoryResolver: ComponentFactoryResolver,
    public viewContainerRef: ViewContainerRef,
    public userInfoService: UserInfoService,
    public companyService: CompanyMasterService,
    public processResultService: ProcessResultService,
  ) {
    super();
  }

  ngOnInit() {
    this.setControlInit();
    this.setValidator();
    this.setFormatter();
    this.clear();
  }

  private setControlInit() {
    this.companyCodeCtrl = new FormControl("");  // 会社コード
    this.productKeyCtrl = new FormControl("");  // プロダクトコード

    this.companyNameCtrl = new FormControl("", [Validators.required]);  // 会社名
    this.companyNameKanaCtrl = new FormControl("", [Validators.required]);  // 会社カナ

    this.postalCodeCtrl = new FormControl("", [Validators.pattern(new RegExp(FormatStyle.ZIP))]);  // 郵便番号

    this.address1Ctrl = new FormControl("");  // 住所
    this.address2Ctrl = new FormControl("");

    // this.telCtrl = new FormControl("", [Validators.pattern(new RegExp(FormatStyle.TEL))]);  // TEL
    // this.faxCtrl = new FormControl("", [Validators.pattern(new RegExp(FormatStyle.TEL))]);  // FAX
    this.telCtrl = new FormControl("");  // TEL
    this.faxCtrl = new FormControl("");  // FAX

    this.bankAccountNameCtrl = new FormControl("");  // 口座名義人
    this.bankAccountNameKanaCtrl = new FormControl("");  // 口座名義人(カナ)

    this.bankName1Ctrl = new FormControl("");  // 銀行名
    this.branchName1Ctrl = new FormControl("");  // 支店名
    this.cmbAccountType1Ctrl = new FormControl("");  // 預金種別
    this.accountNumber1Ctrl = new FormControl("", [CustomValidators.number]);  // 口座番号

    this.bankName2Ctrl = new FormControl("");  // 銀行名
    this.branchName2Ctrl = new FormControl("");  // 支店名
    this.cmbAccountType2Ctrl = new FormControl("");  // 預金種別
    this.accountNumber2Ctrl = new FormControl("", [CustomValidators.number]);  // 口座番号

    this.bankName3Ctrl = new FormControl("");  // 銀行名
    this.branchName3Ctrl = new FormControl("");  // 支店名
    this.cmbAccountType3Ctrl = new FormControl("");  // 預金種別
    this.accountNumber3Ctrl = new FormControl("", [CustomValidators.number]);  // 口座番号

    this.logoCtrl = new FormControl("");  // ロゴ
    this.squareSealCtrl = new FormControl("");  // 社判
    this.roundSealCtrl = new FormControl("");  // 丸印

    this.closingDateCtrl = new FormControl("", [Validators.required, CustomValidators.number]);  // 締め日

    this.undefineCtrl = new FormControl(""); // 未定用

    // this.cbxPresetCodeSearchDialogCtrl = new FormControl("");  // 検索時に全件初期表示する
    // this.cbxShowConfirmDialogCtrl = new FormControl("");  // 検索時に全件初期表示する
    // this.cbxShowWarningDialogCtrl = new FormControl("");  // 警告時ダイアログを表示する
    // this.cbxAutoCloseProgressDialogCtrl = new FormControl("");  // 処理状況ダイアログを自動で閉じる
    this.cbxTransferAggregateCtrl = new FormControl("");  // 口座振替データ作成/取込時に口座単位で集計する    
  }

  private setValidator() {
    this.myFormGroup = new FormGroup({
      companyCodeCtrl: this.companyCodeCtrl,// 会社コード
      productKeyCtrl: this.productKeyCtrl,  // プロダクトコード

      companyNameCtrl: this.companyNameCtrl, // 会社名
      companyNameKanaCtrl: this.companyNameKanaCtrl, // 会社カナ

      postalCodeCtrl: this.postalCodeCtrl, // 郵便番号

      address1Ctrl: this.address1Ctrl, // 住所
      address2Ctrl: this.address2Ctrl,

      telCtrl: this.telCtrl, // TEL
      faxCtrl: this.faxCtrl, // FAX

      bankAccountNameCtrl: this.bankAccountNameCtrl, // 口座名義人
      bankAccountNameKanaCtrl: this.bankAccountNameKanaCtrl, // 口座名義人(カナ)

      bankName1Ctrl: this.bankName1Ctrl, // 銀行名
      branchName1Ctrl: this.branchName1Ctrl, // 支店名
      cmbAccountType1Ctrl: this.cmbAccountType1Ctrl, // 預金種別
      accountNumber1Ctrl: this.accountNumber1Ctrl, // 口座番号

      bankName2Ctrl: this.bankName2Ctrl, // 銀行名
      branchName2Ctrl: this.branchName2Ctrl, // 支店名
      cmbAccountType2Ctrl: this.cmbAccountType2Ctrl, // 預金種別
      accountNumber2Ctrl: this.accountNumber2Ctrl, // 口座番号

      bankName3Ctrl: this.bankName3Ctrl, // 銀行名
      branchName3Ctrl: this.branchName3Ctrl, // 支店名
      cmbAccountType3Ctrl: this.cmbAccountType3Ctrl, // 預金種別
      accountNumber3Ctrl: this.accountNumber3Ctrl, // 口座番号

      logoCtrl: this.logoCtrl, // ロゴ
      squareSealCtrl: this.squareSealCtrl, // 社判
      roundSealCtrl: this.roundSealCtrl, // 丸印

      closingDateCtrl: this.closingDateCtrl, // 締め日

      undefineCtrl: this.undefineCtrl, // 未定用;

      // cbxPresetCodeSearchDialogCtrl: this.cbxPresetCodeSearchDialogCtrl,  // 検索時に全件初期表示する
      // cbxShowConfirmDialogCtrl: this.cbxShowConfirmDialogCtrl,  // 検索時に全件初期表示する
      // cbxShowWarningDialogCtrl: this.cbxShowWarningDialogCtrl,  // 警告時ダイアログを表示する
      // cbxAutoCloseProgressDialogCtrl: this.cbxAutoCloseProgressDialogCtrl,  // 処理状況ダイアログを自動で閉じる
      cbxTransferAggregateCtrl: this.cbxTransferAggregateCtrl,  // 口座振替データ作成/取込時に口座単位で集計する      
    });
  }

  private setFormatter() {
    FormatterUtil.setZipFormatter(this.postalCodeCtrl);
    FormatterUtil.setTelFormatter(this.telCtrl);
    FormatterUtil.setTelFormatter(this.faxCtrl);
    FormatterUtil.setNumberFormatter(this.accountNumber1Ctrl);
    FormatterUtil.setNumberFormatter(this.accountNumber2Ctrl);
    FormatterUtil.setNumberFormatter(this.accountNumber3Ctrl);
    FormatterUtil.setNumberFormatter(this.closingDateCtrl);
  }

  public clear() {
    this.companyCodeCtrl.setValue(this.userInfoService.Company.code);       // 会社コード
    this.productKeyCtrl.setValue(this.userInfoService.Company.productKey);  // プロダクトコード

    this.companyNameCtrl.setValue(this.userInfoService.Company.name);     // 会社名
    this.companyNameKanaCtrl.setValue(this.userInfoService.Company.kana); // 会社カナ

    this.postalCodeCtrl.setValue(this.userInfoService.Company.postalCode);  // 郵便番号

    this.address1Ctrl.setValue(this.userInfoService.Company.address1);  // 住所
    this.address2Ctrl.setValue(this.userInfoService.Company.address2);

    this.telCtrl.setValue(this.userInfoService.Company.tel);  // TEL
    this.faxCtrl.setValue(this.userInfoService.Company.fax);  // FAX

    this.bankAccountNameCtrl.setValue(this.userInfoService.Company.bankAccountName);  // 口座名義人
    this.bankAccountNameKanaCtrl.setValue(this.userInfoService.Company.bankAccountKana);  // 口座名義人(カナ)

    this.bankName1Ctrl.setValue(this.userInfoService.Company.bankName1);  // 銀行名
    this.branchName1Ctrl.setValue(this.userInfoService.Company.branchName1);  // 支店名

    let findIndex = this.exclusiveAccountType.findIndex((item) => {
      return (item.val === this.userInfoService.Company.accountType1);
    });
    if (findIndex < 0) {
      this.cmbAccountType1Ctrl.setValue("");  // 預金種別
    }
    else {
      this.cmbAccountType1Ctrl.setValue(this.userInfoService.Company.accountType1);  // 預金種別
    }

    this.accountNumber1Ctrl.setValue(this.userInfoService.Company.accountNumber1);  // 口座番号

    this.bankName2Ctrl.setValue(this.userInfoService.Company.bankName2);  // 銀行名
    this.branchName2Ctrl.setValue(this.userInfoService.Company.branchName2);  // 支店名

    findIndex = this.exclusiveAccountType.findIndex((item) => {
      return (item.val === this.userInfoService.Company.accountType2);
    });
    if (findIndex < 0) {
      this.cmbAccountType2Ctrl.setValue("");  // 預金種別
    }
    else {
      this.cmbAccountType2Ctrl.setValue(this.userInfoService.Company.accountType2);  // 預金種別
    }
    this.accountNumber2Ctrl.setValue(this.userInfoService.Company.accountNumber2);  // 口座番号

    this.bankName3Ctrl.setValue(this.userInfoService.Company.bankName3);  // 銀行名
    this.branchName3Ctrl.setValue(this.userInfoService.Company.branchName3);  // 支店名

    findIndex = this.exclusiveAccountType.findIndex((item) => {
      return (item.val === this.userInfoService.Company.accountType3);
    });
    if (findIndex < 0) {
      this.cmbAccountType3Ctrl.setValue("");  // 預金種別
    }
    else {
      this.cmbAccountType3Ctrl.setValue(this.userInfoService.Company.accountType3);  // 預金種別
    }
    this.accountNumber3Ctrl.setValue(this.userInfoService.Company.accountNumber3);  // 口座番号

    // this.logoCtrl.setValue(this.userInfoService.Company.);  // ロゴ
    // this.squareSealCtrl.setValue(this.userInfoService.Company.);  // 社判
    // this.roundSealCtrl.setValue(this.userInfoService.Company.);  // 丸印

    this.closingDateCtrl.setValue(this.userInfoService.Company.closingDay);  // 締め日

    // this.cbxPresetCodeSearchDialogCtrl.setValue(this.userInfoService.Company.presetCodeSearchDialog);  // 検索時に全件初期表示する
    // this.cbxShowConfirmDialogCtrl.setValue(this.userInfoService.Company.showConfirmDialog);  // 検索時に全件初期表示する
    // this.cbxShowWarningDialogCtrl.setValue(this.userInfoService.Company.showWarningDialog);  // 警告時ダイアログを表示する
    // this.cbxAutoCloseProgressDialogCtrl.setValue(this.userInfoService.Company.autoCloseProgressDialog); // 処理状況ダイアログを自動で閉じる
    this.cbxTransferAggregateCtrl.setValue(this.userInfoService.Company.transferAggregate); // 口座振替データ作成/取込時に口座単位で集計する    
  }

  /**
   * フォームにDBから取得した値を設定する
   * @param userInfo DBから取得したデータ
   */
  public setUserInfo(userInfos: any) {
    let userInfo = userInfos[0];
    this.companyCodeCtrl.setValue(userInfo.code);       // 会社コード
    this.productKeyCtrl.setValue(userInfo.productKey);  // プロダクトコード
    this.companyNameCtrl.setValue(userInfo.name);     // 会社名
    this.companyNameKanaCtrl.setValue(userInfo.kana); // 会社カナ
    this.postalCodeCtrl.setValue(userInfo.postalCode);  // 郵便番号
    this.address1Ctrl.setValue(userInfo.address1);  // 住所
    this.address2Ctrl.setValue(userInfo.address2);

    this.telCtrl.setValue(userInfo.tel);  // TEL
    this.faxCtrl.setValue(userInfo.fax);  // FAX

    this.bankAccountNameCtrl.setValue(userInfo.bankAccountName);  // 口座名義人
    this.bankAccountNameKanaCtrl.setValue(userInfo.bankAccountKana);  // 口座名義人(カナ)

    this.bankName1Ctrl.setValue(userInfo.bankName1);  // 銀行名
    this.branchName1Ctrl.setValue(userInfo.branchName1);  // 支店名
    if (userInfo.accountType1 == "0") {
      this.cmbAccountType1Ctrl.setValue(Number(userInfo.accountType1));  // 預金種別
    }
    else {
      this.cmbAccountType1Ctrl.setValue(userInfo.accountType1);  // 預金種別
    }
    this.accountNumber1Ctrl.setValue(userInfo.accountNumber1);  // 口座番号

    this.bankName2Ctrl.setValue(userInfo.bankName2);  // 銀行名
    this.branchName2Ctrl.setValue(userInfo.branchName2);  // 支店名
    if (userInfo.accountType2 == "0") {
      this.cmbAccountType2Ctrl.setValue(Number(userInfo.accountType2));  // 預金種別
    }
    else {
      this.cmbAccountType2Ctrl.setValue(userInfo.accountType2);  // 預金種別
    }
    this.accountNumber2Ctrl.setValue(userInfo.accountNumber2);  // 口座番号

    this.bankName3Ctrl.setValue(userInfo.bankName3);  // 銀行名
    this.branchName3Ctrl.setValue(userInfo.branchName3);  // 支店名
    if (userInfo.accountType3 == "0") {
      this.cmbAccountType3Ctrl.setValue(Number(userInfo.accountType3));  // 預金種別
    }
    else {
      this.cmbAccountType3Ctrl.setValue(userInfo.accountType3);  // 預金種別
    }
    this.accountNumber3Ctrl.setValue(userInfo.accountNumber3);  // 口座番号

    this.closingDateCtrl.setValue(userInfo.closingDay);  // 締め日

    // this.cbxPresetCodeSearchDialogCtrl.setValue(userInfo.presetCodeSearchDialog == 1 ? true : false);  // 検索時に全件初期表示する
    // this.cbxShowConfirmDialogCtrl.setValue(userInfo.showConfirmDialog == 1 ? true : false);  // 検索時に全件初期表示する
    // this.cbxShowWarningDialogCtrl.setValue(userInfo.showWarningDialog == 1 ? true : false);  // 警告時ダイアログを表示する
    // this.cbxAutoCloseProgressDialogCtrl.setValue(userInfo.autoCloseProgressDialog == 1 ? true : false); // 処理状況ダイアログを自動で閉じる
    this.cbxTransferAggregateCtrl.setValue(userInfo.transferAggregate == 1 ? true : false); // 口座振替データ作成/取込時に口座単位で集計する    
  }

  public checkInvalid(): boolean {
    if (this.myFormGroup == undefined || this.myFormGroup == null) return false;
    return this.myFormGroup.invalid;
  }


  ///// Enter キー押下時の処理 /////////////////////////////////////////////////////
  public setCompanyName(eventType: string) {
    HtmlUtil.nextFocusByName(this.elementRef, 'companyNameKanaCtrl', eventType);
  }

  public setCompanyNameKana(eventType: string) {
    HtmlUtil.nextFocusByName(this.elementRef, 'postalCodeCtrl', eventType);
  }

  public inputCompanyNameKana() {
    this.companyNameKanaCtrl.setValue(EbDataHelper.convertToKanaHalf(this.companyNameKanaCtrl.value));
  }

  public setPostalCode(eventType: string) {
    HtmlUtil.nextFocusByName(this.elementRef, 'address1Ctrl', eventType);
  }

  public setAddress1(eventType: string) {
    HtmlUtil.nextFocusByName(this.elementRef, 'address2Ctrl', eventType);
  }

  public setAddress2(eventType: string) {
    HtmlUtil.nextFocusByName(this.elementRef, 'telCtrl', eventType);
  }

  public setTel(eventType: string) {
    HtmlUtil.nextFocusByName(this.elementRef, 'faxCtrl', eventType);
  }

  public setFax(eventType: string) {
    HtmlUtil.nextFocusByName(this.elementRef, 'bankAccountNameCtrl', eventType);
  }

  public setBankAccountName(eventType: string) {
    HtmlUtil.nextFocusByName(this.elementRef, 'bankAccountNameKanaCtrl', eventType);
  }

  public setBankAccountNameKana(eventType: string) {
    HtmlUtil.nextFocusByName(this.elementRef, 'bankName1Ctrl', eventType);
  }

  public inputBankAccountNameKana() {
    this.bankAccountNameKanaCtrl.setValue(EbDataHelper.convertToKanaHalf(this.bankAccountNameKanaCtrl.value));
  }

  public setBankName1(eventType: string) {
    HtmlUtil.nextFocusByName(this.elementRef, 'branchName1Ctrl', eventType);
  }

  public setBranchName1(eventType: string) {
    HtmlUtil.nextFocusByName(this.elementRef, 'cmbAccountType1Ctrl', eventType);
  }

  public setCmbAccountType1(eventType: string) {
    HtmlUtil.nextFocusByName(this.elementRef, 'accountNumber1Ctrl', eventType);
  }

  public setAccountNumber1(eventType: string) {
    HtmlUtil.nextFocusByName(this.elementRef, 'bankName2Ctrl', eventType);
  }

  public setBankName2(eventType: string) {
    HtmlUtil.nextFocusByName(this.elementRef, 'branchName2Ctrl', eventType);
  }

  public setBranchName2(eventType: string) {
    HtmlUtil.nextFocusByName(this.elementRef, 'cmbAccountType2Ctrl', eventType);
  }

  public setCmbAccountType2(eventType: string) {
    HtmlUtil.nextFocusByName(this.elementRef, 'accountNumber2Ctrl', eventType);
  }

  public setAccountNumber2(eventType: string) {
    HtmlUtil.nextFocusByName(this.elementRef, 'bankName3Ctrl', eventType);
  }

  public setBankName3(eventType: string) {
    HtmlUtil.nextFocusByName(this.elementRef, 'branchName3Ctrl', eventType);
  }

  public setBranchName3(eventType: string) {
    HtmlUtil.nextFocusByName(this.elementRef, 'cmbAccountType3Ctrl', eventType);
  }

  public setCmbAccountType3(eventType: string) {
    HtmlUtil.nextFocusByName(this.elementRef, 'accountNumber3Ctrl', eventType);
  }

  public setAccountNumber3(eventType: string) {
    HtmlUtil.nextFocusByName(this.elementRef, 'closingDateCtrl', eventType);
  }

  public setClosingDate(eventType: string) {
    HtmlUtil.nextFocusByName(this.elementRef, 'companyNameCtrl', eventType);
  }

}
