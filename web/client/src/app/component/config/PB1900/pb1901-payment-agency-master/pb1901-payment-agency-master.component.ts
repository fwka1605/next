
import { Component, OnInit, ComponentFactoryResolver, ViewContainerRef, ElementRef, ViewChild } from '@angular/core';
import { BaseComponent } from 'src/app/component/common/base/base-component';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { FormatterUtil } from 'src/app/common/util/formatter.util';
import { HtmlUtil } from 'src/app/common/util/html.util';
import { SHARE_TRANSFER_FEE_DICTIONARY, CategoryType, EXCLUSIVE_ACCOUNT_TYPE } from 'src/app/common/const/kbn.const';
import { KEY_CODE, BUTTON_ACTION, EVENT_TYPE } from 'src/app/common/const/event.const';
import { ModalMasterBankComponent } from 'src/app/component/modal/modal-master-bank/modal-master-bank.component';
import { TABLE_INDEX } from 'src/app/common/const/table-name.const';
import { ModalMasterComponent } from 'src/app/component/modal/modal-master/modal-master.component';
import { MODAL_STATUS_TYPE, COMPONENT_STATUS_TYPE, PROCESS_RESULT_RESULT_TYPE } from 'src/app/common/const/status.const';
import { PaymentAgencyMasterService } from 'src/app/service/Master/payment-agency-master.service';
import { PaymentFileFormatResult } from 'src/app/model/payment-file-format-result.model';
import { CategoryMasterService } from 'src/app/service/Master/category-master.service';
import { CategoriesResult } from 'src/app/model/categories-result.model';
import { StringUtil } from 'src/app/common/util/string-util';
import { PaymentAgency } from 'src/app/model/payment-agency.model';
import { UserInfoService } from 'src/app/service/common/user-info.service';
import { Category } from 'src/app/model/category.model';
import { forkJoin } from 'rxjs';
import { ModalConfirmComponent } from 'src/app/component/modal/modal-confirm/modal-confirm.component';
import { FileUtil } from 'src/app/common/util/file.util';
import { PaymentFileFormatMasterService } from 'src/app/service/Master/payment-file-format-master.service';
import { JuridicalPersonalitysResult } from 'src/app/model/juridical-personalitys-result.model';
import { JuridicalPersonalityMasterService } from 'src/app/service/Master/juridical-personality-master.service';
import { EbDataHelper } from 'src/app/common/util/eb-data-helper';
import { ProcessResultService } from 'src/app/service/common/process-result.service';
import { ExistDeleteResult } from 'src/app/model/custom-model/process-result-custom.model';
import { Router, ActivatedRoute, NavigationEnd, ParamMap } from '@angular/router';
import { PageUtil } from 'src/app/common/util/page-util';
import { PaymentAgenciesResult } from 'src/app/model/payment-agencies-result.model';
import { ProcessResult } from 'src/app/model/process-result.model';
import { ModalRouterProgressComponent } from 'src/app/component/modal/modal-router-progress/modal-router-progress.component';
import { MatAutocompleteTrigger } from '@angular/material';

@Component({
  selector: 'app-pb1901-payment-agency-master',
  templateUrl: './pb1901-payment-agency-master.component.html',
  styleUrls: ['./pb1901-payment-agency-master.component.css']
})
export class Pb1901PaymentAgencyMasterComponent extends BaseComponent implements OnInit {

  public paymentAgency: PaymentAgency;

  public readonly exclusiveAccountType = EXCLUSIVE_ACCOUNT_TYPE; // 預金種別
  public readonly shareTransferFeeDictionary = SHARE_TRANSFER_FEE_DICTIONARY;  // 手数料区分

  public paymentFileFormatResult: PaymentFileFormatResult;
  public categoriesResult: CategoriesResult;
  public juridicalPersonalitysResult: JuridicalPersonalitysResult;

  public paymentAgencyCodeCtrl: FormControl; // 決済代行会社コード
  public paymentAgencyNameCtrl: FormControl;
  public paymentAgencyKanaCtrl: FormControl;

  public consigneeCodeCtrl: FormControl; // 委託者コード

  public bankCodeCtrl: FormControl; // 銀行
  public bankKanaCtrl: FormControl;
  public branchCodeCtrl: FormControl;
  public branchKanaCtrl: FormControl;
  public cmbAccountTypeCtrl: FormControl;
  public accountNumberCtrl: FormControl;

  public cmbShareTransferFeeCtrl: FormControl; // 手数料負担

  public cbxUseFeeToleranceCtrl: FormControl; // 手数料誤差利用
  public cbxUseFeeLearningCtrl: FormControl; // 手数料自動学習
  public cbxUseKanaLearningCtrl: FormControl; // カナ自動学習

  public dueDateOffsetCtrl: FormControl; // 引き落とし予定日

  public cmbFileFormatCtrl: FormControl; // 口座振替データフォーマット
  public contractCodeCtrl: FormControl; // 契約種別コード

  public outputFileNameCtrl: FormControl; // 書込みファイル名
  public cbxAppendDateCtrl: FormControl; // 自動的に日付を付与する

  public cbxConsiderUncollectedCtrl: FormControl; // 振替不可データの回収区分を自動更新する

  public cmbConsiderUncollectedCtrl: FormControl; // 振替不可データの回収区分

  public undefineCtrl = new FormControl; // 未定用;  

  public paramFrom: number;

  public isDisableBackBtn: boolean;

  @ViewChild('paymentAgencyCodeInput', { read: MatAutocompleteTrigger }) paymentAgencyCodeigger: MatAutocompleteTrigger;

  constructor(
    public router: Router,
    public activatedRoute: ActivatedRoute,
    public elementRef: ElementRef,
    public componentFactoryResolver: ComponentFactoryResolver,
    public viewContainerRef: ViewContainerRef,
    public userInfoService: UserInfoService,
    public paymentAgencyService: PaymentAgencyMasterService,
    public categoryService: CategoryMasterService,
    public paymentFileFormatService: PaymentFileFormatMasterService,
    public juridicalPersonalityService: JuridicalPersonalityMasterService,
    public processResultService: ProcessResultService

  ) {
    super();

    router.events.subscribe(event => {
      if (event instanceof NavigationEnd) {
        this.Title = PageUtil.GetTitle(router.routerState, router.routerState.root).join('');
        this.ComponentId = parseInt(PageUtil.GetComponentId(router.routerState, router.routerState.root).join(''));
        const path: string[] = PageUtil.GetPath(this.activatedRoute.snapshot.pathFromRoot);
        this.Path = path[1];
        this.processCustomResult = this.processResultService.processResultInit(this.ComponentId, this.Title);
      }
    });

  }

  ngOnInit() {

    this.activatedRoute.paramMap.subscribe((params: ParamMap) => {

      let paymentAgencyCode = params.get("paymentAgencyCode");
      if (!StringUtil.IsNullOrEmpty(paymentAgencyCode)) {

        this.isDisableBackBtn = false;

        // 検索条件の設定
        this.setPaymentAgencyCode(null, paymentAgencyCode);

      }
      else {
        this.isDisableBackBtn = true;

      }
    });

    this.setControlInit();
    this.setValidator();
    this.setFormatter();
    this.clear();
    this.setAutoComplete();

    this.cmbConsiderUncollectedCtrl.disable();

    let paymentFileFormatResponse = this.paymentFileFormatService.Get();
    let juridicalPersonalityResponse = this.juridicalPersonalityService.GetItems();

    forkJoin(
      paymentFileFormatResponse,
      juridicalPersonalityResponse
    )
      .subscribe(responseList => {
        if (responseList != undefined && responseList.length == 2) {
          this.paymentFileFormatResult = new PaymentFileFormatResult();
          this.juridicalPersonalitysResult = new JuridicalPersonalitysResult();

          this.paymentFileFormatResult.paymentFileFormats = responseList[0];
          this.juridicalPersonalitysResult.juridicalPersonalities = responseList[1];
        }
      });
  }

  public setControlInit() {
    this.paymentAgencyCodeCtrl = new FormControl("", [Validators.required, Validators.maxLength(2)]); // 決済代行会社コード
    this.paymentAgencyNameCtrl = new FormControl("", [Validators.required, Validators.maxLength(40)]);
    this.paymentAgencyKanaCtrl = new FormControl("", [Validators.required, , Validators.maxLength(48)]);  // 会社カナ

    this.consigneeCodeCtrl = new FormControl("", [Validators.maxLength(10), Validators.required]); // 委託者コード

    this.bankCodeCtrl = new FormControl("", [Validators.required, Validators.maxLength(4)]); // 銀行
    this.bankKanaCtrl = new FormControl("", [Validators.required, Validators.maxLength(15)]);
    this.branchCodeCtrl = new FormControl("", [Validators.required, Validators.maxLength(3)]);
    this.branchKanaCtrl = new FormControl("", [Validators.required, Validators.maxLength(15)]);
    this.cmbAccountTypeCtrl = new FormControl("", [Validators.required]);
    this.accountNumberCtrl = new FormControl("", [Validators.required, Validators.maxLength(7)]);

    this.cmbShareTransferFeeCtrl = new FormControl("", [Validators.required]); // 手数料負担

    this.cbxUseFeeToleranceCtrl = new FormControl(""); // 手数料誤差利用
    this.cbxUseFeeLearningCtrl = new FormControl(""); // 手数料自動学習
    this.cbxUseKanaLearningCtrl = new FormControl(""); // カナ自動学習

    this.dueDateOffsetCtrl = new FormControl("", [Validators.required, Validators.maxLength(3)]); // 引き落とし予定日

    this.cmbFileFormatCtrl = new FormControl("", [Validators.required]); // 口座振替データフォーマット
    this.contractCodeCtrl = new FormControl("", [Validators.maxLength(2)]); // 契約種別コード
    //this.contractCodeCtrl = new FormControl("",[Validators.required,Validators.maxLength(2)]); // 契約種別コード

    this.outputFileNameCtrl = new FormControl("", [Validators.required]); // 書込みファイル名
    this.cbxAppendDateCtrl = new FormControl(""); // 自動的に日付を付与する

    this.cbxConsiderUncollectedCtrl = new FormControl(""); // 振替不可データの回収区分を自動更新する

    this.cmbConsiderUncollectedCtrl = new FormControl(""); // 振替不可データの回収区分

    this.undefineCtrl = new FormControl(""); // 未定用
  }

  public setValidator() {
    this.MyFormGroup = new FormGroup({

      paymentAgencyCodeCtrl: this.paymentAgencyCodeCtrl, // 決済代行会社コード
      paymentAgencyNameCtrl: this.paymentAgencyNameCtrl,
      paymentAgencyKanaCtrl: this.paymentAgencyKanaCtrl,

      consigneeCodeCtrl: this.consigneeCodeCtrl, // 委託者コード

      bankCodeCtrl: this.bankCodeCtrl, // 銀行
      bankKanaCtrl: this.bankKanaCtrl,
      branchCodeCtrl: this.branchCodeCtrl,
      branchKanaCtrl: this.branchKanaCtrl,
      cmbAccountTypeCtrl: this.cmbAccountTypeCtrl,
      accountNumberCtrl: this.accountNumberCtrl,

      cmbShareTransferFeeCtrl: this.cmbShareTransferFeeCtrl, // 手数料負担

      cbxUseFeeToleranceCtrl: this.cbxUseFeeToleranceCtrl, // 手数料誤差利用
      cbxUseFeeLearningCtrl: this.cbxUseFeeLearningCtrl, // 手数料自動学習
      cbxUseKanaLearningCtrl: this.cbxUseKanaLearningCtrl, // カナ自動学習

      dueDateOffsetCtrl: this.dueDateOffsetCtrl, // 引き落とし予定日

      cmbFileFormatCtrl: this.cmbFileFormatCtrl, // 口座振替データフォーマット
      contractCodeCtrl: this.contractCodeCtrl, // 契約種別コード

      outputFileNameCtrl: this.outputFileNameCtrl, // 書込みファイル名
      cbxAppendDateCtrl: this.cbxAppendDateCtrl, // 自動的に日付を付与する

      cbxConsiderUncollectedCtrl: this.cbxConsiderUncollectedCtrl, // 振替不可データの回収区分を自動更新する

      cmbConsiderUncollectedCtrl: this.cmbConsiderUncollectedCtrl, // 振替不可データの回収区分


      undefineCtrl: this.undefineCtrl, // 未定用;

    });
  }

  public setFormatter() {
    FormatterUtil.setCodeFormatter(this.paymentAgencyCodeCtrl); // 決済代行会社コード

    FormatterUtil.setCodeFormatter(this.consigneeCodeCtrl); // 委託者コード

    FormatterUtil.setCodeFormatter(this.bankCodeCtrl);  // 銀行
    FormatterUtil.setCodeFormatter(this.branchCodeCtrl);
    FormatterUtil.setNumberFormatter(this.accountNumberCtrl);

    FormatterUtil.setNumberFormatter(this.dueDateOffsetCtrl); // 入金予定日

    FormatterUtil.setNumberFormatter(this.contractCodeCtrl);  // 契約種別コード
  }

  public setAutoComplete() {

    // 決済手段コート
    this.initAutocompletePaymentAgencies(this.paymentAgencyCodeCtrl, this.paymentAgencyService, 0);

  }


  public setControlData() {
    this.paymentAgencyCodeCtrl.setValue(this.paymentAgency.code); // 決済代行会社コード
    this.paymentAgencyNameCtrl.setValue(this.paymentAgency.name);
    this.paymentAgencyKanaCtrl.setValue(this.paymentAgency.kana);

    this.consigneeCodeCtrl.setValue(this.paymentAgency.consigneeCode); // 委託者コード

    this.bankCodeCtrl.setValue(this.paymentAgency.bankCode); // 銀行
    this.bankKanaCtrl.setValue(this.paymentAgency.bankName);
    this.branchCodeCtrl.setValue(this.paymentAgency.branchCode);
    this.branchKanaCtrl.setValue(this.paymentAgency.branchName);
    this.cmbAccountTypeCtrl.setValue(this.paymentAgency.accountTypeId);
    this.accountNumberCtrl.setValue(this.paymentAgency.accountNumber);

    this.cmbShareTransferFeeCtrl.setValue(this.paymentAgency.shareTransferFee); // 手数料負担

    this.cbxUseFeeToleranceCtrl.setValue(this.paymentAgency.useFeeTolerance); // 手数料誤差利用
    this.cbxUseFeeLearningCtrl.setValue(this.paymentAgency.useFeeLearning); // 手数料自動学習
    this.cbxUseKanaLearningCtrl.setValue(this.paymentAgency.useKanaLearning); // カナ自動学習

    this.dueDateOffsetCtrl.setValue(this.paymentAgency.dueDateOffset); // 引き落とし予定日

    this.cmbFileFormatCtrl.setValue(this.paymentAgency.fileFormatId); // 口座振替データフォーマット
    this.contractCodeCtrl.setValue(this.paymentAgency.contractCode); // 契約種別コード

    this.outputFileNameCtrl.setValue(this.paymentAgency.outputFileName); // 書込みファイル名
    this.cbxAppendDateCtrl.setValue(this.paymentAgency.appendDate); // 自動的に日付を付与する

    this.cbxConsiderUncollectedCtrl.setValue(this.paymentAgency.considerUncollected);
    this.cmbConsiderUncollectedCtrl.setValue(this.paymentAgency.collectCategoryId); // 振替不可データの回収区分    
  }

  public clear() {
    this.MyFormGroup.reset();

    this.ComponentStatus = this.COMPONENT_STATUS_TYPE.CREATE;

    this.paymentAgencyCodeCtrl.enable();

    this.cbxUseFeeToleranceCtrl.disable(); // 手数料誤差利用
    this.cbxUseFeeLearningCtrl.disable(); // 手数料自動学習
    this.cbxUseKanaLearningCtrl.disable(); // カナ自動学習

    this.cmbConsiderUncollectedCtrl.disable(); // 振替不可データの回収区分
  }

  /**
   * 銀行マスタ取得
   * @param keyCode キーベント
   */
  public openBankMasterModal() {
    this.paymentAgencyCodeigger.closePanel();
    let componentFactory = this.componentFactoryResolver.resolveComponentFactory(ModalMasterBankComponent);
    let componentRef = this.viewContainerRef.createComponent(componentFactory);

    componentRef.instance.Closing.subscribe(() => {

      if (componentRef.instance.ModalStatus == MODAL_STATUS_TYPE.SELECT) {
        this.bankCodeCtrl.setValue(componentRef.instance.SelectedBankCode);
        this.bankKanaCtrl.setValue(componentRef.instance.SelectedBankKana);
        this.branchCodeCtrl.setValue(componentRef.instance.SelectedBranchCode);
        this.branchKanaCtrl.setValue(componentRef.instance.SelectedBranchKana);
      }

      componentRef.destroy();
    });

  }

  /**
   * 各テーブルのデータをフォームに表示
   * @param table テーブル種別
   * @param keyCode キーイベント
   * @param type 
   */
  public openMasterModal(table: TABLE_INDEX) {

    this.paymentAgencyCodeigger.closePanel();

    let componentFactory = this.componentFactoryResolver.resolveComponentFactory(ModalMasterComponent);
    let componentRef = this.viewContainerRef.createComponent(componentFactory);

    componentRef.instance.TableIndex = table;

    componentRef.instance.Closing.subscribe(() => {
      if (componentRef.instance.ModalStatus == MODAL_STATUS_TYPE.SELECT) {
        switch (table) {
          case TABLE_INDEX.MASTER_PAYMENT_AGENCY:
            {
              this.paymentAgency = componentRef.instance.SelectedObject;
              this.setControlData();

              // 更新の場合は状態を変化させる。
              this.ComponentStatus = this.COMPONENT_STATUS_TYPE.UPDATE;
              break;
            }
        }
      }
      componentRef.destroy();
    });
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

  /**
   * データ登録・編集
   */
  public registry() {
    let registryData = new PaymentAgency();
    let isRegistry: boolean = false;

    if (this.ComponentStatus == this.COMPONENT_STATUS_TYPE.CREATE) {
      registryData.id = 0;
      registryData.createBy = this.userInfoService.LoginUser.id;
      isRegistry = true;

    } else if (this.ComponentStatus == COMPONENT_STATUS_TYPE.UPDATE) {
      registryData = this.paymentAgency;

    } else {
      return;
    }

    let processComponentFactory = this.componentFactoryResolver.resolveComponentFactory(ModalRouterProgressComponent);
    let processComponentRef = this.viewContainerRef.createComponent(processComponentFactory);
    // processComponentRef.destroy();       

    // フォームの内容取り込み
    registryData.code = this.paymentAgencyCodeCtrl.value;
    registryData.name = this.paymentAgencyNameCtrl.value;
    registryData.kana = this.paymentAgencyKanaCtrl.value;
    registryData.consigneeCode = this.consigneeCodeCtrl.value;
    registryData.bankCode = this.bankCodeCtrl.value;
    registryData.bankName = this.bankKanaCtrl.value;
    registryData.branchCode = this.branchCodeCtrl.value;
    registryData.branchName = this.branchKanaCtrl.value;
    registryData.accountTypeId = this.cmbAccountTypeCtrl.value;
    registryData.accountNumber = this.accountNumberCtrl.value;
    registryData.shareTransferFee = this.cmbShareTransferFeeCtrl.value;
    registryData.useFeeTolerance = this.cbxUseFeeToleranceCtrl.value == true ? 1 : 0;
    registryData.useFeeLearning = this.cbxUseFeeLearningCtrl.value == true ? 1 : 0;
    registryData.useKanaLearning = this.cbxUseKanaLearningCtrl.value == true ? 1 : 0;
    registryData.dueDateOffset = this.dueDateOffsetCtrl.value;
    registryData.fileFormatId = this.cmbFileFormatCtrl.value;
    registryData.contractCode = this.contractCodeCtrl.value;
    registryData.outputFileName = this.outputFileNameCtrl.value;
    registryData.appendDate = this.cbxAppendDateCtrl.value == true ? 1 : 0;
    registryData.considerUncollected = this.cbxConsiderUncollectedCtrl.value == true ? 1 : 0;
    registryData.collectCategoryId = this.cmbConsiderUncollectedCtrl.value;
    registryData = FileUtil.replaceNull(registryData);

    this.paymentAgencyService.Save(registryData)
      .subscribe(result => {
        this.processCustomResult = this.processResultService.processAtSave(
          this.processCustomResult, result, isRegistry, this.partsResultMessageComponent);
        if (this.processCustomResult.result == PROCESS_RESULT_RESULT_TYPE.SUCCESS) {
          this.clear();

          if (!this.isDisableBackBtn) {
            this.router.navigate(['main/PE0101', { process: "back" }]);
          }
        }

        processComponentRef.destroy();
      })
  }

  /**
   * データ削除
   */
  public delete() {
    let paymentAgency = this.paymentAgency;
    let existPaymentAgency = this.categoryService.ExistPaymentAgency(paymentAgency.id);
    let existDeleteResultList = new Array<ExistDeleteResult>();

    for (let i = 0; i < 3; i++) {
      existDeleteResultList[i] = new ExistDeleteResult();
      existDeleteResultList[i].idName = '決済代行会社コード';
    }

    // 処理の待機
    forkJoin(
      existPaymentAgency
    ).subscribe(
      response => {
        let responseList = new Array<any>();
        responseList.push(response);
        this.processResultService.processAtExist(
          this.processCustomResult, responseList, existDeleteResultList, this.partsResultMessageComponent);

        if (this.processCustomResult.result == PROCESS_RESULT_RESULT_TYPE.WARNING) {
          return;
        }

        // 削除処理
        let componentFactory = this.componentFactoryResolver.resolveComponentFactory(ModalConfirmComponent);
        let componentRef = this.viewContainerRef.createComponent(componentFactory);
        componentRef.instance.ActionName = "削除"
        componentRef.instance.Closing.subscribe(() => {

          if (componentRef.instance.ModalStatus == MODAL_STATUS_TYPE.OK) {

            let processComponentFactory = this.componentFactoryResolver.resolveComponentFactory(ModalRouterProgressComponent);
            let processComponentRef = this.viewContainerRef.createComponent(processComponentFactory);
            // processComponentRef.destroy();   

            this.paymentAgencyService.Delete(paymentAgency)
              .subscribe(result => {
                this.processCustomResult = this.processResultService.processAtDelete(
                  this.processCustomResult, result, this.partsResultMessageComponent);
                if (this.processCustomResult.result == PROCESS_RESULT_RESULT_TYPE.SUCCESS) {
                  this.clear();
                }
                processComponentRef.destroy();
              });
          }
          componentRef.destroy();
        });
      },
      error => {
        console.log(error)
      }
    );
  }

  public back() {
    this.router.navigate(['main/PE0101', { process: "back" }]);

  }

  ///// Enterキー押下時の処理 //////////////////////////////////////////////////////

  public setPaymentAgencyCode(eventType: string, paymentAgencyCode: string = null) {

    if (eventType != EVENT_TYPE.BLUR) {
      this.paymentAgencyCodeigger.closePanel();
    }

    let paymentAgencyCodeTmp: string;
    if (StringUtil.IsNullOrEmpty(paymentAgencyCode)) {
      paymentAgencyCodeTmp = this.paymentAgencyCodeCtrl.value;
    }
    else {
      paymentAgencyCodeTmp = paymentAgencyCode;
    }

    if (!StringUtil.IsNullOrEmpty(paymentAgencyCodeTmp)) {

      let mastersResult = new PaymentAgenciesResult();
      mastersResult.processResult = new ProcessResult();
      mastersResult.processResult.result = false;

      this.paymentAgencyService.GetItems()
        .subscribe(response => {

          if (response != PROCESS_RESULT_RESULT_TYPE.FAILURE) {
            mastersResult.processResult.result = true;
            mastersResult.paymentAgencies = response;

            let paymentAgencyCode = paymentAgencyCodeTmp;
            if (response.length > 0) {
              mastersResult.paymentAgencies = mastersResult.paymentAgencies.filter(
                function (paymentAgency: PaymentAgency) {
                  return paymentAgency.code == paymentAgencyCode;
                }
              )
            }

            if (mastersResult.paymentAgencies.length == 1) {

              this.paymentAgency = mastersResult.paymentAgencies[0];
              this.setControlData();

              // 更新の場合は状態を変化させる。
              this.paymentAgencyCodeCtrl.disable();
              this.ComponentStatus = this.COMPONENT_STATUS_TYPE.UPDATE;
              HtmlUtil.nextFocusByName(this.elementRef, 'paymentAgencyNameCtrl', eventType);
            }
            else {
              this.paymentAgencyNameCtrl.setValue("");
              HtmlUtil.nextFocusByName(this.elementRef, 'consigneeCodeCtrl', eventType);
            }

          }
          else {
            this.paymentAgencyNameCtrl.setValue("");
            HtmlUtil.nextFocusByName(this.elementRef, 'consigneeCodeCtrl', eventType);
          }
        });

    }
    else {
      this.paymentAgencyCodeCtrl.setValue("");
      this.paymentAgencyNameCtrl.setValue("");
      HtmlUtil.nextFocusByName(this.elementRef, 'consigneeCodeCtrl', eventType);
    }

  }

  public setPaymentAgencyName(eventType: string) {
    HtmlUtil.nextFocusByName(this.elementRef, 'paymentAgencyKanaCtrl', eventType);
  }

  public setPaymentAgencyKana(eventType: string) {
    HtmlUtil.nextFocusByName(this.elementRef, 'consigneeCodeCtrl', eventType);
  }

  public inputPaymentAgencyKana() {
    let value = this.paymentAgencyKanaCtrl.value;
    value = EbDataHelper.convertToValidEbkana(value);
    value = EbDataHelper.removePersonalities(value, this.juridicalPersonalitysResult.juridicalPersonalities);
    this.paymentAgencyKanaCtrl.setValue(value);
  }

  public setConsigneeCode(eventType: string) {
    HtmlUtil.nextFocusByName(this.elementRef, 'bankCodeCtrl', eventType);
  }

  public setBankCode(eventType: string) {
    if (!this.StringUtil.IsNullOrEmpty(this.bankCodeCtrl.value)) {
      this.bankCodeCtrl.setValue(StringUtil.setPaddingFrontZero(this.bankCodeCtrl.value, 4, true));
      this.bankKanaCtrl.setValue("ｷﾞﾝｺｳﾒｲｶﾅ");
      HtmlUtil.nextFocusByName(this.elementRef, 'branchCodeCtrl', eventType);
    }
    else {
      HtmlUtil.nextFocusByName(this.elementRef, 'bankKanaCtrl', eventType);
    }
  }

  public setBankName(eventType: string) {
    HtmlUtil.nextFocusByName(this.elementRef, 'branchCodeCtrl', eventType);
  }

  public inputBankName() {
    this.bankKanaCtrl.setValue(EbDataHelper.convertToValidEbkana(this.bankKanaCtrl.value));
  }

  public setBranchCode(eventType: string) {

    if (!this.StringUtil.IsNullOrEmpty(this.branchCodeCtrl.value)) {
      this.branchCodeCtrl.setValue(StringUtil.setPaddingFrontZero(this.branchCodeCtrl.value, 3, true));
      this.branchKanaCtrl.setValue("ｼﾃﾝﾒｲｶﾅ");
      HtmlUtil.nextFocusByName(this.elementRef, 'cmbAccountTypeCtrl', eventType);
    }
    else {
      HtmlUtil.nextFocusByName(this.elementRef, 'branchKanaCtrl', eventType);
    }
  }

  public setBranchName(eventType: string) {
    HtmlUtil.nextFocusByName(this.elementRef, 'cmbAccountTypeCtrl', eventType);
  }

  public inputBranchName() {
    this.branchKanaCtrl.setValue(EbDataHelper.convertToValidEbkana(this.branchKanaCtrl.value));
  }

  public setCmbAccountType(eventType: string) {
    HtmlUtil.nextFocusByName(this.elementRef, 'accountNumberCtrl', eventType);
  }

  public setAccountNumber(eventType: string) {

    if (!this.StringUtil.IsNullOrEmpty(this.accountNumberCtrl.value)) {
      this.accountNumberCtrl.setValue(StringUtil.setPaddingFrontZero(this.accountNumberCtrl.value, 7, true));
      HtmlUtil.nextFocusByName(this.elementRef, 'cmbShareTransferFeeCtrl', eventType);
    }
    else {
      HtmlUtil.nextFocusByName(this.elementRef, 'cmbShareTransferFeeCtrl', eventType);
    }

  }

  public setCmbShareTransferFee(eventType: string) {
    if (this.cmbShareTransferFeeCtrl.value == 0) {

      this.cbxUseFeeToleranceCtrl.disable(); // 手数料誤差利用
      this.cbxUseFeeToleranceCtrl.setValue(""); // 手数料誤差利用
      this.cbxUseFeeLearningCtrl.disable(); // 手数料自動学習
      this.cbxUseFeeLearningCtrl.setValue(""); // 手数料自動学習

      this.cbxUseKanaLearningCtrl.enable(); // カナ自動学習

    }
    else if (this.cmbShareTransferFeeCtrl.value == 1) {
      this.cbxUseFeeToleranceCtrl.enable(); // 手数料誤差利用
      this.cbxUseFeeLearningCtrl.enable(); // 手数料自動学習
      this.cbxUseKanaLearningCtrl.enable(); // カナ自動学習

    }
    HtmlUtil.nextFocusByName(this.elementRef, 'dueDateOffsetCtrl', eventType);
  }

  public setDueDateOffset(eventType: string) {
    HtmlUtil.nextFocusByName(this.elementRef, 'cmbFileFormatCtrl', eventType);
  }

  public setCmbFileFormat(eventType: string) {
    HtmlUtil.nextFocusByName(this.elementRef, 'contractCodeCtrl', eventType);
  }

  public setContractCode(eventType: string) {
    HtmlUtil.nextFocusByName(this.elementRef, 'outputFileNameCtrl', eventType);
  }

  public setCbxConsiderUncollected(eventType: string) {
    if (this.cbxConsiderUncollectedCtrl.value == true) {
      this.cmbConsiderUncollectedCtrl.enable();

      // Validationの追加
      this.cmbConsiderUncollectedCtrl.setValidators([Validators.required]);
      this.cmbConsiderUncollectedCtrl.updateValueAndValidity();

      this.categoryService.GetItemsByCategoryType(CategoryType.Receipt)
        .subscribe(result => {
          if (result) {
            this.categoriesResult = new CategoriesResult();
            this.categoriesResult.categories = new Array<Category>();
            this.categoriesResult.categories = result;
            HtmlUtil.nextFocusByName(this.elementRef, 'cmbConsiderUncollectedCtrl', eventType);
          }
        })
    }
    else {
      this.cmbConsiderUncollectedCtrl.reset();
      this.cmbConsiderUncollectedCtrl.disable();
      HtmlUtil.nextFocusByName(this.elementRef, 'paymentAgencyCodeCtrl', eventType);
    }
  }

  public setCmbConsiderUncollected(eventType: string) {
    HtmlUtil.nextFocusByName(this.elementRef, 'paymentAgencyCodeCtrl', eventType);
  }

}
