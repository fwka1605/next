import { Component, OnInit, EventEmitter, ViewContainerRef, ComponentFactoryResolver, ElementRef } from '@angular/core';
import { BaseComponent } from '../../common/base/base-component';
import { Validators, FormControl, FormGroup } from '@angular/forms';
import { CustomValidators } from 'ng5-validation';
import { ModalMasterComponent } from '../modal-master/modal-master.component';
import { ImporterSettingDetailsResult } from 'src/app/model/importer-setting-details-result.model';
import { StringUtil } from 'src/app/common/util/string-util';
import { TABLE_INDEX } from 'src/app/common/const/table-name.const';
import { KEY_CODE, BUTTON_ACTION, EVENT_TYPE } from 'src/app/common/const/event.const';
import { MODAL_STATUS_TYPE, COMPONENT_STATUS_TYPE, PROCESS_RESULT_STATUS_TYPE, PROCESS_RESULT_RESULT_TYPE } from 'src/app/common/const/status.const';
import { FormatterUtil } from 'src/app/common/util/formatter.util';
import { NumberUtil } from 'src/app/common/util/number-util';
import { ModalConfirmComponent } from '../modal-confirm/modal-confirm.component';
import { ImporterSetting } from 'src/app/model/importer-setting.model';
import { FreeImporterFormatType } from 'src/app/common/common-const';
import { UserInfoService } from 'src/app/service/common/user-info.service';
import { ImporterSettingDetail } from 'src/app/model/importer-setting-detail.model';
import { ImporterSettingService } from 'src/app/service/Master/importer-setting.service';
import { SettingMasterService } from 'src/app/service/Master/setting-master.service';
import { ColumnNameSettingMasterService } from 'src/app/service/Master/column-name-setting-master.service';
import { forkJoin } from 'rxjs';
import { SettingsResult } from 'src/app/model/settings-result.model';
import { ColumnNameSettingsResult } from 'src/app/model/column-name-settings-result.model';
import { ImporterSettingsResult } from 'src/app/model/importer-settings-result.model';
import { Setting } from 'src/app/model/setting.model';
import { IMPORTER_RECEIPT_FIELDS, IMPORTER_CUSTOMER_FIELDS } from 'src/app/common/const/importer.const';
import { IMPORTER_BILLING_FIELDS } from 'src/app/common/const/importer.const';
import { CategoryMasterService } from 'src/app/service/Master/category-master.service';
import { SectionMasterService } from 'src/app/service/Master/section-master.service';
import { CurrencyMasterService } from 'src/app/service/Master/currency-master.service';
import { CategoryType, CUSTOMER_HOLIDAY_FLAG_DICTIONARY } from 'src/app/common/const/kbn.const';
import { BankAccountTypeMasterService } from 'src/app/service/Master/bank-account-type-master.service';
import { BankAccountType } from 'src/app/model/bank-account-type.model';
import { DepartmentMasterService } from 'src/app/service/Master/department-master.service';
import { AccountTitleMasterService } from 'src/app/service/Master/account-title-master.service'
import { StaffMasterService } from 'src/app/service/Master/staff-master.service'
import { MSG_WNG, MSG_ERR, MSG_ITEM_NUM, MSG_INF } from 'src/app/common/const/message.const';
import { ProcessResultService } from 'src/app/service/common/process-result.service';
import { HtmlUtil } from 'src/app/common/util/html.util';
import { ModalRouterProgressComponent } from '../modal-router-progress/modal-router-progress.component';

@Component({
  selector: 'app-modal-importer-setting-customer',
  templateUrl: './modal-importer-setting-customer.component.html',
  styleUrls: ['./modal-importer-setting-customer.component.css']
})
export class ModalImporterSettingCustomerComponent extends BaseComponent implements OnInit {

  public FreeImporterFormatType: typeof FreeImporterFormatType = FreeImporterFormatType;

  public importerSettingId: number;

  public settingsResult: SettingsResult;
  public columnNameSettingsResult: ColumnNameSettingsResult
  public importerFileType: number;

  public cmbAttributeList: Array<Array<Setting>>;
  public cmbImportDivisionList: Array<Array<Setting>>;

  public chkDoUpdateCtrls: Array<FormControl>;
  public cmbImportDivisionCtrls: Array<FormControl>;
  public fixedValueCtrls: Array<FormControl>;
  public fieldIndexCtrls: Array<FormControl>;
  public cmbAttributeCtrls: Array<FormControl>;

  public selectedImportSetting: ImporterSetting;
  public importerSettingDetaisResult: ImporterSettingDetailsResult = null;


  public patternNoCtrl: FormControl;              // パターンNo
  public patternNameCtrl: FormControl;            // パターン名

  //public importFileCtrl: FormControl;             // 取込フォルダー

  public startLineCountCtrl: FormControl;          // 取込開始行	

  public ignoreLastLineCtrl: FormControl;         // 最終行を取込まない	
  public autoSettingDestinationCtrl: FormControl;   // 送付先コードを自動でセットする。
  public UndefineCtrl: FormControl;

  public closing = new EventEmitter<{}>();
  public get Closing() {
    return this.closing;
  }
  public set Closing(value) {
    this.closing = value;
  }

  public id: number;
  public get Id(): number {
    return this.id;
  }
  public set Id(value: number) {
    this.id = value;
  }

  public get ProcessModalCustomResult() {
    return this.processModalCustomResult;
  }
  public set ProcessModalCustomResult(value) {
    this.processModalCustomResult = value;
  }

  constructor(
    public componentFactoryResolver: ComponentFactoryResolver,
    public viewContainerRef: ViewContainerRef,
    public importerSettingService: ImporterSettingService,
    public userInfoService: UserInfoService,
    public settingService: SettingMasterService,
    public columnNameSettingService: ColumnNameSettingMasterService,
    public categoryService: CategoryMasterService,
    public sectionService: SectionMasterService,
    public currencyService: CurrencyMasterService,
    public bankAccountTypeService: BankAccountTypeMasterService,
    public departmentService: DepartmentMasterService,
    public accountTitleService: AccountTitleMasterService,
    public staffService: StaffMasterService,
    public processResultService: ProcessResultService,
    public elementRef: ElementRef,

  ) {
    super();
  }

  ngOnInit() {
    this.setControlInit();
    this.setValidator();
    this.setFormatter();
    this.clear();

    let settingResponse = this.settingService.GetItems();

    this.importerFileType = FreeImporterFormatType.Customer;

    let columnNameSetting = this.columnNameSettingService.Get(this.importerFileType);

    forkJoin(
      settingResponse,
      columnNameSetting
    ).subscribe(responseList => {
      if (responseList.length != 2 || responseList.indexOf(undefined) >= 0) {
        this.processModalCustomResult.status = PROCESS_RESULT_STATUS_TYPE.DONE;
        this.processModalCustomResult.result = PROCESS_RESULT_RESULT_TYPE.FAILURE;
        this.processModalCustomResult.message = MSG_ERR.EB_FILE_IMPORT_ERROR;
        return;
      }

      this.settingsResult = new SettingsResult();
      this.settingsResult.settings = responseList[0];

      this.columnNameSettingsResult = new ColumnNameSettingsResult();
      this.columnNameSettingsResult.columnNames = responseList[1];

    });

  }

  public setControlInit() {
    this.patternNoCtrl = new FormControl("", [Validators.required, Validators.maxLength(2)]);
    this.patternNameCtrl = new FormControl("", [Validators.required, Validators.maxLength(40)]);

    this.startLineCountCtrl = new FormControl({ value: "1" }, [Validators.required, CustomValidators.range([1, 9])]);

    this.ignoreLastLineCtrl = new FormControl("");
    this.autoSettingDestinationCtrl = new FormControl("");   // 送付先コードを自動でセットする。

    this.UndefineCtrl = new FormControl();

  }

  public setValidator() {
    this.MyFormGroup = new FormGroup({
      patternNoCtrl: this.patternNoCtrl,
      patternNameCtrl: this.patternNameCtrl,
      //importFileCtrl: this.importFileCtrl,

      ignoreLastLineCtrl: this.ignoreLastLineCtrl,
      startLineCountCtrl: this.startLineCountCtrl,
      autoSettingDestinationCtrl: this.autoSettingDestinationCtrl,

      UndefineCtrl: this.UndefineCtrl,
    });
  }

  public setFormatter() {
    FormatterUtil.setNumberFormatter(this.patternNoCtrl);
    FormatterUtil.setNumberFormatter(this.startLineCountCtrl);
  }

  public setDetailFormatter(field: number, index: number) {

    this.fieldIndexCtrls[index].setValidators([Validators.maxLength(3)]);
    FormatterUtil.setNumberFormatter(this.fieldIndexCtrls[index]);

    switch (this.importerFileType) {
      case FreeImporterFormatType.Billing:
        {
          if (field == IMPORTER_BILLING_FIELDS.BillingCategoryCode || field == IMPORTER_BILLING_FIELDS.CollectCategoryCode) {
            this.fixedValueCtrls[index].setValidators([Validators.maxLength(2)]);
            FormatterUtil.setNumberFormatter(this.fixedValueCtrls[index]);
          }
          else if (field == IMPORTER_BILLING_FIELDS.CurrencyCode) {
            this.fixedValueCtrls[index].setValidators([Validators.maxLength(3)]);
            FormatterUtil.setAlphabetFormatter(this.fixedValueCtrls[index]);
          }
          else if (field == IMPORTER_BILLING_FIELDS.ExclusiveBankCode) {
            this.fixedValueCtrls[index].setValidators([Validators.maxLength(4)]);
            FormatterUtil.setNumberFormatter(this.fixedValueCtrls[index]);
          }
          else if (field == IMPORTER_BILLING_FIELDS.ExclusiveBranchCode) {
            this.fixedValueCtrls[index].setValidators([Validators.maxLength(3)]);
            FormatterUtil.setNumberFormatter(this.fixedValueCtrls[index]);
          }
          else if (field == IMPORTER_BILLING_FIELDS.ExclusiveVirtualBranchCode) {
            this.fixedValueCtrls[index].setValidators([Validators.maxLength(3)]);
            FormatterUtil.setNumberFormatter(this.fixedValueCtrls[index]);
          }
        }
        break;
      case FreeImporterFormatType.Receipt:
        {
          if (field == IMPORTER_RECEIPT_FIELDS.ReceiptCategoryCode) {
            this.fixedValueCtrls[index].setValidators([Validators.maxLength(2)]);
            FormatterUtil.setNumberFormatter(this.fixedValueCtrls[index]);
          }
          else if (field == IMPORTER_RECEIPT_FIELDS.CurrencyCode) {
            this.fixedValueCtrls[index].setValidators([Validators.maxLength(3)]);
            FormatterUtil.setAlphabetFormatter(this.fixedValueCtrls[index]);
          }
          else if (field == IMPORTER_RECEIPT_FIELDS.BankCode) {
            this.fixedValueCtrls[index].setValidators([Validators.maxLength(4)]);
            FormatterUtil.setNumberFormatter(this.fixedValueCtrls[index]);
          }
          else if (field == IMPORTER_RECEIPT_FIELDS.BankName) {
            this.fixedValueCtrls[index].setValidators([Validators.maxLength(30)]);
          }
          else if (field == IMPORTER_RECEIPT_FIELDS.BranchCode) {
            this.fixedValueCtrls[index].setValidators([Validators.maxLength(3)]);
            FormatterUtil.setNumberFormatter(this.fixedValueCtrls[index]);
          }
          else if (field == IMPORTER_RECEIPT_FIELDS.BranchName) {
            this.fixedValueCtrls[index].setValidators([Validators.maxLength(30)]);
          }
          else if (field == IMPORTER_RECEIPT_FIELDS.AccountTypeId) {
            this.fixedValueCtrls[index].setValidators([Validators.maxLength(1)]);
            FormatterUtil.setNumberFormatter(this.fixedValueCtrls[index]);
          }
          else if (field == IMPORTER_RECEIPT_FIELDS.AccountNumber) {
            this.fixedValueCtrls[index].setValidators([Validators.maxLength(7)]);
            FormatterUtil.setNumberFormatter(this.fixedValueCtrls[index]);
          }
          else if (field == IMPORTER_RECEIPT_FIELDS.AccountName) {
            this.fixedValueCtrls[index].setValidators([Validators.maxLength(50)]);
          }
          else if (field == IMPORTER_RECEIPT_FIELDS.AccountName) {
            this.fixedValueCtrls[index].setValidators([Validators.maxLength(10)]);
            FormatterUtil.setCodeFormatter(this.fixedValueCtrls[index]);
          }
        }
        break;
    }

  }

  /**
   * ボタン操作によるメソッド呼び出し
   * @param action ボタン操作
   */
  public buttonActionInner(action: BUTTON_ACTION) {
    this.processResultService.processResultStart(this.processModalCustomResult, action);
    if (this.processModalCustomResult.result == PROCESS_RESULT_RESULT_TYPE.FAILURE) {
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
   * データ登録・更新
   */
  public registry() {
    let isRegistry: boolean = false;

    if (!this.isValidateForSave()) {
      return false;
    }

    let processComponentFactory = this.componentFactoryResolver.resolveComponentFactory(ModalRouterProgressComponent);
    let processComponentRef = this.viewContainerRef.createComponent(processComponentFactory);
    // processComponentRef.destroy();       

    let importerSetting = new ImporterSetting();
    if (this.selectedImportSetting == null) {
      importerSetting.id = 0;
      isRegistry = true;
    } else {
      importerSetting.id = this.selectedImportSetting.id;
    }
    importerSetting.companyId = this.userInfoService.Company.id;
    importerSetting.code = this.patternNoCtrl.value;
    importerSetting.name = this.patternNameCtrl.value;
    importerSetting.formatId = this.importerFileType;
    importerSetting.initialDirectory = "C:\\Usr";    //仮設定
    importerSetting.startLineCount = this.startLineCountCtrl.value;
    importerSetting.ignoreLastLine = this.ignoreLastLineCtrl.value == true ? 1 : 0;
    importerSetting.updateBy = this.userInfoService.LoginUser.id;
    importerSetting.createBy = this.userInfoService.LoginUser.id;
    importerSetting.details = new Array<ImporterSettingDetail>();

    // 登録処理
    for (var index = 0; index < this.importerSettingDetaisResult.importerSettingDetails.length; index++) {
      let importerSettingDetail = new ImporterSettingDetail();

      importerSettingDetail.sequence = (index + 1);
      importerSettingDetail.updateKey = this.chkDoUpdateCtrls[index].value == true ? 1 : 0;

      let tmpSettingList: Array<Setting> = this.cmbImportDivisionList[index];
      // 表示内容の取得
      let importDivisionDisplayText = "";
      tmpSettingList.forEach(element => {
        if (element.itemKey == this.cmbImportDivisionCtrls[index].value) {
          importDivisionDisplayText = element.itemValue;
        }
      })

      if (StringUtil.IsNullOrEmpty(importDivisionDisplayText)) {
        importerSettingDetail.fixedValue = "";
      }
      else {
        importerSettingDetail.fixedValue = StringUtil.IsNullOrEmpty(this.fixedValueCtrls[index].value) ? "" : this.fixedValueCtrls[index].value;
      }


      importerSettingDetail.importerSettingId = importerSetting.id;
      importerSettingDetail.updateBy = this.userInfoService.LoginUser.id;
      //importerSettingDetail.UpdateAt = nowDateTime;
      importerSettingDetail.createBy = this.userInfoService.LoginUser.id;
      //detail.CreateAt = nowDateTime;
      importerSettingDetail.importDivision = this.cmbImportDivisionCtrls[index].value;;
      importerSettingDetail.caption = "";

      if (importerSettingDetail.importDivision == 1) {
        importerSettingDetail.fieldIndex = this.fieldIndexCtrls[index].value;
      }
      else if (this.importerFileType == FreeImporterFormatType.Receipt &&
        (importerSettingDetail.sequence == IMPORTER_RECEIPT_FIELDS.RecordedAt || importerSetting.sequence == IMPORTER_RECEIPT_FIELDS.ReceiptAmount)) {
        importerSettingDetail.fieldIndex = this.fieldIndexCtrls[index].value;
      }

      importerSettingDetail.attributeDivision = StringUtil.IsNullOrEmpty(this.cmbAttributeCtrls[index].value) ? "0" : this.cmbAttributeCtrls[index].value;

      importerSetting.details.push(importerSettingDetail);
    }

    let importerSettingsResult = new ImporterSettingsResult();
    this.importerSettingService.Save(importerSetting)
      .subscribe(response => {
        this.processModalCustomResult = this.processResultService.processAtSave(
          this.processModalCustomResult, response, isRegistry, this.partsResultMessageComponent);
        if (this.processModalCustomResult.result == PROCESS_RESULT_RESULT_TYPE.SUCCESS) {
          importerSettingsResult.importerSettings = new Array<ImporterSetting>();
          let importerSetting: ImporterSetting = response;
          importerSettingsResult.importerSettings.push(importerSetting);
        }
        processComponentRef.destroy();
      });

  }

  public isValidateForSave(): boolean {

    let allDisable = true;
    let allEnabled = true;

    let shareTransferFee: number;
    let useFeeLearning: number;
    let useFeeTolerance: number;
    let closingDay: number;

    for (let index = 0; index < this.importerSettingDetaisResult.importerSettingDetails.length; index++) {
      let detail = this.importerSettingDetaisResult.importerSettingDetails[index];
      let field = detail.sequence;

      let tmpSettingList: Array<Setting> = this.cmbImportDivisionList[index];

      // 表示内容の取得
      let importDivisionDisplayText = "";
      tmpSettingList.forEach(element => {
        if (element.itemKey == this.cmbImportDivisionCtrls[index].value) {
          importDivisionDisplayText = element.itemValue;
        }
      })

      if (this.cmbImportDivisionCtrls[index].value == 1 && this.fieldIndexCtrls[index].value == 0) {
        this.processModalCustomResult = this.processResultService.processAtWarning(
          this.processModalCustomResult, MSG_WNG.INPUT_REQUIRED.replace(MSG_ITEM_NUM.FIRST, '項目番号'),
          this.partsResultMessageComponent);
        HtmlUtil.nextFocusByName(this.elementRef, 'fieldIndexCtrl' + index, EVENT_TYPE.NONE);
        return false;
      }
      else if (this.fixedValueCtrls[index].enabled && StringUtil.IsNullOrEmpty(this.fixedValueCtrls[index].value)) {
        this.processModalCustomResult = this.processResultService.processAtWarning(
          this.processModalCustomResult, MSG_WNG.NO_INPUT_FIXED_VALUE.replace(MSG_ITEM_NUM.FIRST, detail.fieldName),
          this.partsResultMessageComponent);
        HtmlUtil.nextFocusByName(this.elementRef, 'fixedValueCtrl' + index, EVENT_TYPE.NONE);
        return false;
      }
      else if (this.fixedValueCtrls[index].enabled && !StringUtil.IsNullOrEmpty(this.fixedValueCtrls[index].value)) {
        // 
        if (field == IMPORTER_CUSTOMER_FIELDS.ShareTransferFee) {
          shareTransferFee = NumberUtil.ParseInt(this.fixedValueCtrls[index].value);

          let bRet = shareTransferFee == 0 || shareTransferFee == 1;
          if (!bRet) {
            this.processModalCustomResult = this.processResultService.processAtWarning(
              this.processModalCustomResult, MSG_WNG.INPUT_INVALID_LETTER.replace(MSG_ITEM_NUM.FIRST, '手数料負担区分'),
              this.partsResultMessageComponent);
            HtmlUtil.nextFocusByName(this.elementRef, 'fixedValueCtrl' + index, EVENT_TYPE.NONE);
            return false;
          }

        }
        else if (field == IMPORTER_CUSTOMER_FIELDS.UseFeeLearning) {
          useFeeLearning = NumberUtil.ParseInt(this.fixedValueCtrls[index].value);
          let bRet = useFeeLearning == 0 || useFeeLearning == 1;
          if (!bRet) {
            this.processModalCustomResult = this.processResultService.processAtWarning(
              this.processModalCustomResult, MSG_WNG.INPUT_INVALID_LETTER.replace(MSG_ITEM_NUM.FIRST, '手数料自動学習'),
              this.partsResultMessageComponent);
            HtmlUtil.nextFocusByName(this.elementRef, 'fixedValueCtrl' + index, EVENT_TYPE.NONE);
            return false;
          }

          if (shareTransferFee == 0 && useFeeLearning != 0) {
            this.processModalCustomResult = this.processResultService.processAtWarning(
              this.processModalCustomResult, MSG_WNG.SHARE_TRANSFER_FEE_MIS_MATCH.replace(MSG_ITEM_NUM.FIRST, '手数料自動学習'),
              this.partsResultMessageComponent);
            HtmlUtil.nextFocusByName(this.elementRef, 'fixedValueCtrl' + index, EVENT_TYPE.NONE);
            return false;
          }
        }
        else if (field == IMPORTER_CUSTOMER_FIELDS.UseFeeTolerance) {
          useFeeTolerance = NumberUtil.ParseInt(this.fixedValueCtrls[index].value);
          let bRet = useFeeTolerance == 0 || useFeeTolerance == 1;
          if (!bRet) {
            this.processModalCustomResult = this.processResultService.processAtWarning(
              this.processModalCustomResult, MSG_WNG.INPUT_INVALID_LETTER.replace(MSG_ITEM_NUM.FIRST, '手数料誤差利用'),
              this.partsResultMessageComponent);
            HtmlUtil.nextFocusByName(this.elementRef, 'fixedValueCtrl' + index, EVENT_TYPE.NONE);
            return false;
          }

          if (shareTransferFee == 0 && useFeeTolerance != 0) {
            this.processModalCustomResult = this.processResultService.processAtWarning(
              this.processModalCustomResult, MSG_WNG.SHARE_TRANSFER_FEE_MIS_MATCH.replace(MSG_ITEM_NUM.FIRST, '手数料誤差利用'),
              this.partsResultMessageComponent);
            HtmlUtil.nextFocusByName(this.elementRef, 'fixedValueCtrl' + index, EVENT_TYPE.NONE);
            return false;
          }
        }
        else if (
          field == IMPORTER_CUSTOMER_FIELDS.IsParent
          || field == IMPORTER_CUSTOMER_FIELDS.UseKanaLearning
          || field == IMPORTER_CUSTOMER_FIELDS.PrioritizeMatchingIndividually
          || field == IMPORTER_CUSTOMER_FIELDS.ExcludeInvoicePublish
          || field == IMPORTER_CUSTOMER_FIELDS.ExcludeReminderPublish
        ) {
          // 入力チェック
          let fixedValue = NumberUtil.ParseInt(this.fixedValueCtrls[index].value);
          let bRet = fixedValue == 0 || fixedValue == 1;
          if (!bRet) {

            if (field == IMPORTER_CUSTOMER_FIELDS.IsParent) {
              this.processModalCustomResult = this.processResultService.processAtWarning(
                this.processModalCustomResult, MSG_WNG.INPUT_INVALID_LETTER.replace(MSG_ITEM_NUM.FIRST, '債権代表者フラグ'),
                this.partsResultMessageComponent);
              HtmlUtil.nextFocusByName(this.elementRef, 'fixedValueCtrl' + index, EVENT_TYPE.NONE);
            }
            else if (field == IMPORTER_CUSTOMER_FIELDS.UseKanaLearning) {
              this.processModalCustomResult = this.processResultService.processAtWarning(
                this.processModalCustomResult, MSG_WNG.INPUT_INVALID_LETTER.replace(MSG_ITEM_NUM.FIRST, 'カナ自動学習'),
                this.partsResultMessageComponent);
              HtmlUtil.nextFocusByName(this.elementRef, 'fixedValueCtrl' + index, EVENT_TYPE.NONE);
            }
            else if (field == IMPORTER_CUSTOMER_FIELDS.PrioritizeMatchingIndividually) {
              this.processModalCustomResult = this.processResultService.processAtWarning(
                this.processModalCustomResult, MSG_WNG.INPUT_INVALID_LETTER.replace(MSG_ITEM_NUM.FIRST, '一括消込対象外'),
                this.partsResultMessageComponent);
              HtmlUtil.nextFocusByName(this.elementRef, 'fixedValueCtrl' + index, EVENT_TYPE.NONE);
            }
            else if (field == IMPORTER_CUSTOMER_FIELDS.ExcludeInvoicePublish) {
              this.processModalCustomResult = this.processResultService.processAtWarning(
                this.processModalCustomResult, MSG_WNG.INPUT_INVALID_LETTER.replace(MSG_ITEM_NUM.FIRST, '請求書発行対象外'),
                this.partsResultMessageComponent);
              HtmlUtil.nextFocusByName(this.elementRef, 'fixedValueCtrl' + index, EVENT_TYPE.NONE);
            }
            else if (field == IMPORTER_CUSTOMER_FIELDS.ExcludeReminderPublish) {
              this.processModalCustomResult = this.processResultService.processAtWarning(
                this.processModalCustomResult, MSG_WNG.INPUT_INVALID_LETTER.replace(MSG_ITEM_NUM.FIRST, '督促状発行対象外'),
                this.partsResultMessageComponent);
              HtmlUtil.nextFocusByName(this.elementRef, 'fixedValueCtrl' + index, EVENT_TYPE.NONE);
            }
            return false;
          }


        }
        else if (field == IMPORTER_CUSTOMER_FIELDS.CollectCategoryId) {
          var code = this.fixedValueCtrls[index].value;
          // 存在チェック
        }
        else if (field == IMPORTER_CUSTOMER_FIELDS.StaffCode) {
          var code = this.fixedValueCtrls[index].value;
          // 存在チェック
        }
        //締日チェック
        else if (field == IMPORTER_CUSTOMER_FIELDS.ClosingDay) {
          let closingAt: string = this.fixedValueCtrls[index].value;
          let bRet = NumberUtil.ParseInt(closingAt) >= 0 && NumberUtil.ParseInt(closingAt) < 28 || NumberUtil.ParseInt(closingAt) == 99;

          if (!bRet) {
            this.processModalCustomResult = this.processResultService.processAtWarning(
              this.processModalCustomResult, MSG_WNG.NUMBER_VALUE_VALID0_TO_27_OR_99.replace(MSG_ITEM_NUM.FIRST, '締日'),
              this.partsResultMessageComponent);
            HtmlUtil.nextFocusByName(this.elementRef, 'fixedValueCtrl' + index, EVENT_TYPE.NONE);
            return false;
          }
          else {
            closingDay = NumberUtil.ParseInt(closingAt);
          }
        }
        else if (field == IMPORTER_CUSTOMER_FIELDS.CollectOffsetMonth) {

          let offsetMonth: string = this.fixedValueCtrls[index].value;
          let bRet = NumberUtil.ParseInt(offsetMonth) >= 0 && NumberUtil.ParseInt(offsetMonth) <= 9;
          if (!bRet) {
            this.processModalCustomResult = this.processResultService.processAtWarning(
              this.processModalCustomResult, MSG_WNG.INPUTABLE_RANGE_IS_0_TO_9.replace(MSG_ITEM_NUM.FIRST, '回収予定（月）'),
              this.partsResultMessageComponent);
            HtmlUtil.nextFocusByName(this.elementRef, 'fixedValueCtrl' + index, EVENT_TYPE.NONE);
            return false;
          }

        }
        else if (field == IMPORTER_CUSTOMER_FIELDS.CollectOffsetDay) {

          let day: string = this.fixedValueCtrls[index].value;
          let bRet = NumberUtil.ParseInt(day) >= 0 && NumberUtil.ParseInt(day) < 28 || NumberUtil.ParseInt(day) == 99;

          if (!bRet) {
            this.processModalCustomResult = this.processResultService.processAtWarning(
              this.processModalCustomResult, MSG_WNG.NUMBER_VALUE_VALID0_TO_27_OR_99.replace(MSG_ITEM_NUM.FIRST, '回収予定（日）'),
              this.partsResultMessageComponent);
            HtmlUtil.nextFocusByName(this.elementRef, 'fixedValueCtrl' + index, EVENT_TYPE.NONE);
            return false;
          }

        }
        else if (field == IMPORTER_CUSTOMER_FIELDS.CollectOffsetDayPerBilling) {

          let day: string = this.fixedValueCtrls[index].value;
          let bRet = NumberUtil.ParseInt(day) >= 0 && NumberUtil.ParseInt(day) <= 99;
          if (!bRet) {
            this.processModalCustomResult = this.processResultService.processAtWarning(
              this.processModalCustomResult, MSG_WNG.NUMBER_VALUE_VALID0_TO_99.replace(MSG_ITEM_NUM.FIRST, '回収予定（都度請求）'),
              this.partsResultMessageComponent);
            HtmlUtil.nextFocusByName(this.elementRef, 'fixedValueCtrl' + index, EVENT_TYPE.NONE);
            return false;
          }

        }
        else if (field == IMPORTER_CUSTOMER_FIELDS.HolidayFlag) {
          let day: string = this.fixedValueCtrls[index].value;
          let bRet = NumberUtil.ParseInt(day) >= 0 && NumberUtil.ParseInt(day) < 3;
          if (!bRet) {
            this.processModalCustomResult = this.processResultService.processAtWarning(
              this.processModalCustomResult, MSG_WNG.INPUT_INVALID_LETTER.replace(MSG_ITEM_NUM.FIRST, '休業日の設定'),
              this.partsResultMessageComponent);
            HtmlUtil.nextFocusByName(this.elementRef, 'fixedValueCtrl' + index, EVENT_TYPE.NONE);
            return false;
          }
        }
        else if (field == IMPORTER_CUSTOMER_FIELDS.Honorific) {
          let fixedValue: string = this.fixedValueCtrls[index].value;

          if (!StringUtil.IsNullOrEmpty(fixedValue) && fixedValue.length > 6) {
            const msg = MSG_WNG.INPUT_VALID_DATE_FOR_PARAMETERS.replace(MSG_ITEM_NUM.FIRST, '敬称').replace(MSG_ITEM_NUM.SECOND, '6');
            this.processModalCustomResult = this.processResultService.processAtWarning(
              this.processModalCustomResult, msg, this.partsResultMessageComponent);
            HtmlUtil.nextFocusByName(this.elementRef, 'fixedValueCtrl' + index, EVENT_TYPE.NONE);
            return false;
          }
        }


      }
      else if (this.cmbAttributeCtrls[index].enabled
        && (StringUtil.IsNullOrEmpty(this.cmbAttributeCtrls[index].value) || this.cmbAttributeCtrls[index].value == "0")
      ) {
        this.processModalCustomResult = this.processResultService.processAtWarning(
          this.processModalCustomResult, MSG_WNG.SELECTION_REQUIRED.replace(MSG_ITEM_NUM.FIRST, '属性情報'),
          this.partsResultMessageComponent
        );
        HtmlUtil.nextFocusByName(this.elementRef, 'cmbAttributeCtrl' + index, EVENT_TYPE.NONE);
        return false;
      }

    }
    return true;
  }



  /**
   * データ削除
   */
  public delete() {

    let componentFactory = this.componentFactoryResolver.resolveComponentFactory(ModalConfirmComponent);
    let componentRef = this.viewContainerRef.createComponent(componentFactory);
    componentRef.instance.ActionName = "削除"
    componentRef.instance.Closing.subscribe(() => {

      if (componentRef.instance.ModalStatus == MODAL_STATUS_TYPE.OK) {

        componentRef.destroy();

        let processComponentFactory = this.componentFactoryResolver.resolveComponentFactory(ModalRouterProgressComponent);
        let processComponentRef = this.viewContainerRef.createComponent(processComponentFactory);
        // processComponentRef.destroy();   

        // 実際の削除を実行
        this.selectedImportSetting.details = this.importerSettingDetaisResult.importerSettingDetails;

        let importerSettingsResult = new ImporterSettingsResult();
        this.importerSettingService.Delete(this.selectedImportSetting)
          .subscribe(response => {

            this.processModalCustomResult = this.processResultService.processAtDelete(
              this.processModalCustomResult, response, this.partsResultMessageComponent);
              
            if (this.processModalCustomResult.result == PROCESS_RESULT_RESULT_TYPE.SUCCESS) {
              importerSettingsResult.importerSettings = new Array<ImporterSetting>();
              let importerSetting: ImporterSetting = response;
              importerSettingsResult.importerSettings.push(importerSetting);

              processComponentRef.destroy();
              this.clear();
            }
          });
      }
      else {
        componentRef.destroy();
      }
    });

  }

  public clear() {
    this.MyFormGroup.reset();
    this.importerSettingDetaisResult = null;

    this.patternNoCtrl.enable();
    this.startLineCountCtrl.setValue("1");

    this.ComponentStatus = null;

    HtmlUtil.nextFocusByName(this.elementRef, 'patternNoCtrl', EVENT_TYPE.NONE);
  }

  public close() {
    this.ModalStatus = MODAL_STATUS_TYPE.CLOSE;
    this.processModalCustomResult = this.processResultService.clearProcessCustomMsg(this.processModalCustomResult);
    this.closing.emit({});
  }


  public cancel() {
    this.ModalStatus = MODAL_STATUS_TYPE.CANCEL;
    this.processModalCustomResult = this.processResultService.clearProcessCustomMsg(this.processModalCustomResult);
    this.closing.emit({});
  }

  ///////////////////////////////////////////////////////////////
  public openMasterModal() {
    let componentFactory = this.componentFactoryResolver.resolveComponentFactory(ModalMasterComponent);
    let componentRef = this.viewContainerRef.createComponent(componentFactory);
    componentRef.instance.zIndex = componentRef.instance.zIndexDefSize * 2;

    componentRef.instance.TableIndex = TABLE_INDEX.MASTER_CUSTOMER_IMPORTER_SETTING;

    componentRef.instance.Closing.subscribe(() => {

      if (componentRef.instance.ModalStatus == MODAL_STATUS_TYPE.SELECT) {

        this.ComponentStatus = COMPONENT_STATUS_TYPE.UPDATE;

        this.patternNoCtrl.setValue(componentRef.instance.SelectedCode);
        this.patternNameCtrl.setValue(componentRef.instance.SelectedName);
        this.selectedImportSetting = componentRef.instance.SelectedObject;

        this.patternNoCtrl.disable();
        this.GetDtails(this.importerFileType);

      }

      this.ComponentStatus = COMPONENT_STATUS_TYPE.UPDATE;

      componentRef.destroy();

    });
  }

  public openMasterModalForDetail(keyCode: string = null, index: number = -1) {


    let detail = this.importerSettingDetaisResult.importerSettingDetails[index];

    let table: number;


    if (detail.sequence == IMPORTER_CUSTOMER_FIELDS.ShareTransferFee) {
      table = TABLE_INDEX.MASTER_SHARE_TRANSFER_FEE;
    }
    else if (detail.sequence == IMPORTER_CUSTOMER_FIELDS.CollectCategoryId) {
      table = TABLE_INDEX.MASTER_COLLECT_CATEGORY;
    }
    else if (detail.sequence == IMPORTER_CUSTOMER_FIELDS.StaffCode) {
      table = TABLE_INDEX.MASTER_STAFF;
    }
    else if (detail.sequence == IMPORTER_CUSTOMER_FIELDS.IsParent) {
      table = TABLE_INDEX.MASTER_PARENT_CUSTOMER_FLAG;
    }
    else if (detail.sequence == IMPORTER_CUSTOMER_FIELDS.HolidayFlag) {
      table = TABLE_INDEX.MASTER_CUSTOMER_HOLIDAY_FLAG;
    }
    else if (detail.sequence == IMPORTER_CUSTOMER_FIELDS.UseFeeLearning) {
      table = TABLE_INDEX.USE_FEE_LEARNING_YES_NO_FLAG;
    }
    else if (detail.sequence == IMPORTER_CUSTOMER_FIELDS.UseKanaLearning) {
      table = TABLE_INDEX.USE_KANA_LEARNING_YES_NO_FLAG;
    }
    else if (detail.sequence == IMPORTER_CUSTOMER_FIELDS.UseFeeTolerance) {
      table = TABLE_INDEX.USE_FEE_TOLERANCE_YES_NO_FLAG;
    }
    else if (detail.sequence == IMPORTER_CUSTOMER_FIELDS.PrioritizeMatchingIndividually) {
      table = TABLE_INDEX.PRIORITIZE_MATCHING_INDIVIDUAL_YES_NO_FLAG;
    }


    if (keyCode == KEY_CODE.F9) {
      let componentFactory = this.componentFactoryResolver.resolveComponentFactory(ModalMasterComponent);
      let componentRef = this.viewContainerRef.createComponent(componentFactory);
      componentRef.instance.TableIndex = table;
      componentRef.instance.zIndex = componentRef.instance.zIndexDefSize * 2;

      componentRef.instance.Closing.subscribe(() => {

        if (componentRef.instance.ModalStatus == MODAL_STATUS_TYPE.SELECT) {
          this.fixedValueCtrls[index].setValue(componentRef.instance.SelectedCode);
        }
        componentRef.destroy();
      });
    }
  }

  ///////////////////////////////////////////////////////////////
  public setPatternNo(eventType: string) {

    if (!StringUtil.IsNullOrEmpty(this.patternNoCtrl.value)) {
      this.patternNoCtrl.setValue(StringUtil.setPaddingFrontZero(this.patternNoCtrl.value, 2));
      this.loadStart();
      this.importerSettingService.GetHeader(this.importerFileType, this.patternNoCtrl.value)
        .subscribe(response => {
          this.loadEnd();
          if (response != undefined && response.length == 1) {
            this.processModalCustomResult = this.processResultService.processAtSuccess(
              this.processModalCustomResult, MSG_INF.EXIT_PATTERN_NO, this.partsResultMessageComponent);

            this.selectedImportSetting = response[0];

            this.ComponentStatus = COMPONENT_STATUS_TYPE.UPDATE;

            this.patternNoCtrl.disable();
            this.patternNameCtrl.setValue(this.selectedImportSetting.name);

            this.GetDtails(this.importerFileType);

          }
          else {

            this.processModalCustomResult = this.processResultService.processAtSuccess(
              this.processModalCustomResult, MSG_INF.NEW_PATTERN_NO, this.partsResultMessageComponent);

            this.patternNameCtrl.enable();
            this.patternNameCtrl.setValue("");

            this.ComponentStatus = COMPONENT_STATUS_TYPE.CREATE;
            this.selectedImportSetting = null;

            this.GetDtails(this.importerFileType);

          }
        });

      HtmlUtil.nextFocusByName(this.elementRef, 'patternNameCtrl', eventType);

    }
    else {
      this.patternNoCtrl.setValue("");
      this.patternNameCtrl.setValue("");
      this.importerSettingDetaisResult = null;
      this.ComponentStatus = null;
    }

  }

  public GetDtails(fileType: number) {
    // インポーター設定詳細情報の取得
    this.importerSettingDetaisResult = new ImporterSettingDetailsResult();

    this.importerSettingService.GetDetail(fileType, this.patternNoCtrl.value)
      .subscribe(response => {
        if (response != undefined && response.length > 0) {
          this.importerSettingDetaisResult.importerSettingDetails = response;

          this.importerSettingDetaisResult.importerSettingDetails =
            this.importerSettingDetaisResult.importerSettingDetails.filter(element => {
              if (element.sequence == IMPORTER_CUSTOMER_FIELDS.ExcludeInvoicePublish) return this.userInfoService.ApplicationControl.usePublishInvoice == 1;
              if (element.sequence == IMPORTER_CUSTOMER_FIELDS.ExcludeReminderPublish) return this.userInfoService.ApplicationControl.useReminder == 1;
              return true;
            });


          if (this.selectedImportSetting == null || this.ComponentStatus == COMPONENT_STATUS_TYPE.CREATE) {
            //this.importFileCtrl.reset();             // 取込フォルダー

            this.startLineCountCtrl.reset();          // 取込開始行	

            this.ignoreLastLineCtrl.reset();         // 最終行を取込まない

          }
          else {

            this.startLineCountCtrl.setValue(this.selectedImportSetting.startLineCount);          // 取込開始行	

            this.ignoreLastLineCtrl.setValue(this.selectedImportSetting.ignoreLastLine);         // 最終行を取込まない


          }

          this.cmbImportDivisionList = new Array<Array<Setting>>();
          this.cmbAttributeList = new Array<Array<Setting>>();

          this.chkDoUpdateCtrls = new Array<FormControl>(this.importerSettingDetaisResult.importerSettingDetails.length);
          this.cmbImportDivisionCtrls = new Array<FormControl>(this.importerSettingDetaisResult.importerSettingDetails.length);
          this.fixedValueCtrls = new Array<FormControl>(this.importerSettingDetaisResult.importerSettingDetails.length);
          this.fieldIndexCtrls = new Array<FormControl>(this.importerSettingDetaisResult.importerSettingDetails.length);
          this.cmbAttributeCtrls = new Array<FormControl>(this.importerSettingDetaisResult.importerSettingDetails.length);


          for (let index = 0; index < this.importerSettingDetaisResult.importerSettingDetails.length; index++) {

            this.chkDoUpdateCtrls[index] = new FormControl("");
            this.cmbImportDivisionCtrls[index] = new FormControl("");
            this.fixedValueCtrls[index] = new FormControl("");
            this.fieldIndexCtrls[index] = new FormControl("");
            this.cmbAttributeCtrls[index] = new FormControl("");

            this.MyFormGroup.removeControl("chkDoUpdateCtrl" + index);
            this.MyFormGroup.removeControl("cmbImportDivisionCtrl" + index);
            this.MyFormGroup.removeControl("fixedValueCtrl" + index);
            this.MyFormGroup.removeControl("fieldIndexCtrl" + index);
            this.MyFormGroup.removeControl("cmbAttributeCtrl" + index);

            this.MyFormGroup.addControl("chkDoUpdateCtrl" + index, this.chkDoUpdateCtrls[index]);
            this.MyFormGroup.addControl("cmbImportDivisionCtrl" + index, this.cmbImportDivisionCtrls[index]);
            this.MyFormGroup.addControl("fixedValueCtrl" + index, this.fixedValueCtrls[index]);

            this.MyFormGroup.addControl("fieldIndexCtrl" + index, this.fieldIndexCtrls[index]);
            FormatterUtil.setNumberFormatter(this.fieldIndexCtrls[index]);

            this.MyFormGroup.addControl("cmbAttributeCtrl" + index, this.cmbAttributeCtrls[index]);

            let detail = this.importerSettingDetaisResult.importerSettingDetails[index];
            let field = detail.sequence;
            let bPush = true;

            // 1.更新
            this.chkDoUpdateCtrls[index].setValue(detail.updateKey == 1 ? true : false);

            // ２．項目名
            // HTMLで表示

            // ４．固定値
            this.fixedValueCtrls[index].setValue(detail.fixedValue);

            // ５．項目番号
            if (detail.fieldIndex == undefined || detail.fieldIndex == 0) {
              this.fieldIndexCtrls[index].setValue("");
            }
            else {
              this.fieldIndexCtrls[index].setValue("" + detail.fieldIndex);
            }

            // ３．取込有無
            let key = "TRKM" + detail.baseImportDivision;

            this.cmbImportDivisionList.push(
              this.settingsResult.settings.filter(
                function (setting: Setting) {
                  if (setting.itemId == key) {
                    return true;
                  }
                  else {
                    return false;
                  }
                }
              )
            );

            let i = 0;

            if (detail.baseImportDivision == 2) { // 会社コード、取込なし
              this.cmbImportDivisionCtrls[index].disable();
              this.cmbImportDivisionCtrls[index].setValue(0);
            }
            else if (detail.baseImportDivision == 0) {
              this.cmbImportDivisionCtrls[index].disable();
              this.cmbImportDivisionCtrls[index].setValue(1);
            }
            else {
              this.cmbImportDivisionCtrls[index].setValue(detail.importDivision);
              if (this.IsImportDivisionEnabled(detail)) {
                this.cmbImportDivisionCtrls[index].enable();
              }
              else {
                this.cmbImportDivisionCtrls[index].disable();
              }

            }

            if (detail.importerSettingId == 0 && detail.baseImportDivision == 0) {
              detail.importDivision = parseInt(this.cmbImportDivisionCtrls[index].value);
            }

            if (this.IsUpdateKeyEnabled(detail)) {
              this.chkDoUpdateCtrls[index].enable();
            }
            else {
              this.chkDoUpdateCtrls[index].disable();
            }

            if (this.IsFixedValueEnabled(detail)) {
              this.fixedValueCtrls[index].enable();
            }
            else {
              this.fixedValueCtrls[index].disable();
            }


            if (this.IsFieldIndexEnabled(detail)) {
              this.fieldIndexCtrls[index].enable();
            }
            else {
              this.fieldIndexCtrls[index].disable();
            }

            if (this.IsAttribudeEnabled(detail)) {
              this.cmbAttributeCtrls[index].enable();
            }
            else {
              this.cmbAttributeCtrls[index].disable();
            }

            if (detail.baseAttributeDivision > 0) {
              let key = "ATTR" + detail.baseAttributeDivision;
              this.cmbAttributeList.push(
                this.settingsResult.settings.filter(
                  function (setting: Setting) {
                    return setting.itemId == key;
                  }
                )
              );
            }
            else {
              this.cmbAttributeList.push(null);
            }

            if (this.cmbAttributeCtrls[index].enabled) {
              this.cmbAttributeCtrls[index].setValue(detail.attributeDivision)
            }
          }


        }
        else {
          this.importerSettingDetaisResult = null;
        }
      });
  }

  ///////////////////////////////////////////////////////////////
  public setPatternName(eventType: string) {
    HtmlUtil.nextFocusByName(this.elementRef, 'startLineCountCtrl', eventType);
  }

  //////////////////////////////////////////////////////////////////


  /// <summary>
  /// 得意先コード、名称、カナ等 必須項目 ImporterSettingDetail.BaseImportDivision == 0
  /// </summary>
  /// <param name="detail"></param>
  /// <returns></returns>
  public IsCustomerRequiredImportDivision(detail: ImporterSettingDetail): boolean {
    let bRtn = detail.baseImportDivision == 0;

    return bRtn;
  }

  /// <summary>
  /// 通常の 取込有/無 の取込区分 ImporterSettingDetail.BaseImportDivision == 1
  /// </summary>
  /// <param name="detail"></param>
  /// <returns></returns>
  public IsDoOrNotImportDivision(detail: ImporterSettingDetail): boolean {
    let bRtn = detail.baseImportDivision == 1;
    return bRtn;


  }

  /// <summary>
  /// 固定値/取込 の取込区分 detail.BaseImportDivision == 12
  /// </summary>
  /// <param name="detail"></param>
  /// <returns></returns>
  public IsFixedImportDivision(detail: ImporterSettingDetail): boolean {
    let bRtn = detail.baseImportDivision == 12;
    return bRtn;
  }

  /// <summary>
  /// 0 : 取込無, 1 : 取込有, 2 : 固定値 の3項目
  /// </summary>
  /// <param name="detail"></param>
  /// <returns></returns>
  public IsDoOrNotWithFixedImportDivision(detail: ImporterSettingDetail): boolean {
    let bRtn = detail.baseImportDivision == 11;
    return bRtn;
  }

  /// <summary>
  /// 約定関連項目 か 否か
  /// </summary>
  /// <param name="detail"></param>
  /// <returns></returns>
  public IsThresholdRelatedField(detail: ImporterSettingDetail): boolean {
    return this.IsThresholdRelatedField2(<IMPORTER_CUSTOMER_FIELDS>detail.sequence);
  }

  /// <summary>
  /// 約定関連項目 か 否か
  /// </summary>
  /// <param name="detail"></param>
  /// <returns></returns>
  public IsThresholdRelatedField2(field: IMPORTER_CUSTOMER_FIELDS): boolean {
    let bRtn = field == IMPORTER_CUSTOMER_FIELDS.LessThanCollectCategoryId
      || field == IMPORTER_CUSTOMER_FIELDS.GreaterThanCollectCategoryId1
      || field == IMPORTER_CUSTOMER_FIELDS.GreaterThanRate1
      || field == IMPORTER_CUSTOMER_FIELDS.GreaterThanRoundingMode1
      || field == IMPORTER_CUSTOMER_FIELDS.GreaterThanSightOfBill1
      || field == IMPORTER_CUSTOMER_FIELDS.GreaterThanCollectCategoryId2
      || field == IMPORTER_CUSTOMER_FIELDS.GreaterThanRate2
      || field == IMPORTER_CUSTOMER_FIELDS.GreaterThanRoundingMode2
      || field == IMPORTER_CUSTOMER_FIELDS.GreaterThanSightOfBill2
      || field == IMPORTER_CUSTOMER_FIELDS.GreaterThanCollectCategoryId3
      || field == IMPORTER_CUSTOMER_FIELDS.GreaterThanRate3
      || field == IMPORTER_CUSTOMER_FIELDS.GreaterThanRoundingMode3
      || field == IMPORTER_CUSTOMER_FIELDS.GreaterThanSightOfBill3;

    return bRtn;
  }

  public IsCollectOffsetRelatedField(detail: ImporterSettingDetail): boolean {
    return this.IsCollectOffsetRelatedField2(<IMPORTER_CUSTOMER_FIELDS>detail.sequence);
  }

  /// <summary>
  /// 回収予定関連（締日）
  /// </summary>
  /// <param name="field"></param>
  /// <returns></returns>
  public IsCollectOffsetRelatedField2(field: IMPORTER_CUSTOMER_FIELDS): boolean {
    let bRtn = field == IMPORTER_CUSTOMER_FIELDS.CollectOffsetMonth
      || field == IMPORTER_CUSTOMER_FIELDS.CollectOffsetDay
      || field == IMPORTER_CUSTOMER_FIELDS.CollectOffsetDayPerBilling;

    return bRtn;
  }

  /// <summary>
  /// 上書 チェックボックス 有効無効
  /// 約定関連項目ではない
  /// かつ
  ///     得意先 名称、カナ
  ///     通常の 取込有/無 で 1 : 取込有 のもの
  ///     固定値/取込 で 債権代表者フラグ 以外
  /// </summary>
  /// <param name="detail"></param>
  /// <returns></returns>
  public IsUpdateKeyEnabled(detail: ImporterSettingDetail): boolean {

    let bRtn =
      !this.IsThresholdRelatedField(detail)
      && (
        (
          this.IsCustomerRequiredImportDivision(detail)
          && <IMPORTER_CUSTOMER_FIELDS>detail.sequence != IMPORTER_CUSTOMER_FIELDS.CustomerCode
        )
        || (this.IsDoOrNotImportDivision(detail) && detail.importDivision == 1)
        || (this.IsFixedImportDivision(detail) && <IMPORTER_CUSTOMER_FIELDS>detail.sequence != IMPORTER_CUSTOMER_FIELDS.IsParent)
        || (this.IsDoOrNotWithFixedImportDivision(detail) && detail.importDivision != 0 && <IMPORTER_CUSTOMER_FIELDS>detail.sequence == IMPORTER_CUSTOMER_FIELDS.Honorific)
      );

    return bRtn;
  }

  /// <summary>
  /// 取込区分 有効無効
  /// 約定関連項目ではない
  /// かつ
  ///     通常の 取込有/無
  ///     固定値/取込
  /// </summary>
  /// <param name="detail"></param>
  /// <returns></returns>
  public IsImportDivisionEnabled(detail: ImporterSettingDetail): boolean {
    let bRtn =
      !this.IsThresholdRelatedField(detail)
      && (
        this.IsDoOrNotImportDivision(detail)
        || this.IsFixedImportDivision(detail)
        || this.IsDoOrNotWithFixedImportDivision(detail)
      );
    return bRtn;
  }

  /// <summary>
  /// 固定値 有効無効
  /// 固定値/取込    で 0 : 固定値 のもの
  /// </summary>
  /// <param name="detail"></param>
  /// <returns></returns>
  public IsFixedValueEnabled(detail: ImporterSettingDetail): boolean {

    let bRtn =
      this.IsFixedImportDivision(detail) && detail.importDivision == 0
      || this.IsDoOrNotWithFixedImportDivision(detail) && detail.importDivision == 2;

    return bRtn;

  }

  /// <summary>
  /// 属性区分 有効無効
  /// 属性区分 が 0, 2 以外
  /// かつ
  ///    取込区分 1 : 取込有 のもの
  /// </summary>
  /// <param name="detail"></param>
  /// <returns></returns>
  public IsAttribudeEnabled(detail: ImporterSettingDetail): boolean {
    let bRtn =
      detail.baseAttributeDivision != 0
      && detail.baseAttributeDivision != 2
      && detail.importDivision == 1;
    return bRtn;
  }


  /// <summary>
  /// 項目番号 有効無効
  /// 得意先 コード、名称、カナ
  /// 通常 取込有/無 で 1 : 取込有 のもの
  /// 固定値/取込    で 1 : 取込有 のもの
  /// </summary>
  /// <param name="detail"></param>
  /// <returns></returns>
  public IsFieldIndexEnabled(detail: ImporterSettingDetail): boolean {
    let bRtn =
      this.IsCustomerRequiredImportDivision(detail)
      || (this.IsDoOrNotImportDivision(detail) && detail.importDivision == 1)
      || (this.IsFixedImportDivision(detail) && detail.importDivision == 1)
      || (this.IsDoOrNotWithFixedImportDivision(detail) && detail.importDivision == 1);

    return bRtn;
  }


  ///////////////////////////////////////////////////////////////
  public setFixedValue(eventType: string, index: number) {

    if (StringUtil.IsNullOrEmpty(this.fixedValueCtrls[index].value)) {
      return;
    }

    let detail = this.importerSettingDetaisResult.importerSettingDetails[index];

    switch (this.importerFileType) {
      case FreeImporterFormatType.Billing:
        this.setBillingImporterFixedValue(detail, index);
        break;
      case FreeImporterFormatType.Receipt:
        this.setReceiptImporterFixedValue(detail, index);
        break;
    }

  }

  public setBillingImporterFixedValue(detail: ImporterSettingDetail, index: number) {

    if (detail.sequence == IMPORTER_BILLING_FIELDS.DepartmentCode) {
      this.loadStart();
      this.departmentService.GetItems(this.fixedValueCtrls[index].value)
        .subscribe(response => {
          this.loadEnd();
          if (response != undefined && response.length > 0) {
            this.fixedValueCtrls[index].setValue(response[0].code);
          }
          else {
            this.fixedValueCtrls[index].setValue("");
          }
        });
    }
    else if (detail.sequence == IMPORTER_BILLING_FIELDS.DebitAccountTitleCode) {
      this.loadStart();
      this.accountTitleService.Get(this.fixedValueCtrls[index].value)
        .subscribe(response => {
          this.loadEnd();
          if (response != undefined && response.length > 0) {
            this.fixedValueCtrls[index].setValue(response[0].code);
          }
          else {
            this.fixedValueCtrls[index].setValue("");
          }
        });
    }
    else if (detail.sequence == IMPORTER_BILLING_FIELDS.StaffCode) {
      this.loadStart();
      this.staffService.GetItems(this.fixedValueCtrls[index].value)
        .subscribe(response => {
          this.loadEnd();
          if (response != undefined && response.length > 0) {
            this.fixedValueCtrls[index].setValue(response[0].code);
          }
          else {
            this.fixedValueCtrls[index].setValue("");
          }
        });
    }
    else if (detail.sequence == IMPORTER_BILLING_FIELDS.CurrencyCode) {
      this.loadStart();
      this.currencyService.GetItems(this.fixedValueCtrls[index].value)
        .subscribe(response => {
          this.loadEnd();
          if (response != undefined && response.length > 0) {
            this.fixedValueCtrls[index].setValue(response[0].code);
          }
          else {
            this.fixedValueCtrls[index].setValue("");
          }
        });
    }
    else if (detail.sequence == IMPORTER_BILLING_FIELDS.BillingCategoryCode) {
      this.fixedValueCtrls[index].setValue(StringUtil.setPaddingFrontZero(this.fixedValueCtrls[index].value, 2));

      this.loadStart();
      this.categoryService.GetItems(CategoryType.Billing, this.fixedValueCtrls[index].value)
        .subscribe(response => {
          this.loadEnd();
          if (response != undefined && response.length > 0) {
            this.fixedValueCtrls[index].setValue(response[0].code);
          }
          else {
            this.fixedValueCtrls[index].setValue("");
          }
        });
    }
    else if (detail.sequence == IMPORTER_BILLING_FIELDS.CollectCategoryCode) {
      this.fixedValueCtrls[index].setValue(StringUtil.setPaddingFrontZero(this.fixedValueCtrls[index].value, 2));

      this.loadStart();
      this.categoryService.GetItems(CategoryType.Collection, this.fixedValueCtrls[index].value)
        .subscribe(response => {
          this.loadEnd();
          if (response != undefined && response.length > 0) {
            this.fixedValueCtrls[index].setValue(response[0].code);
          }
          else {
            this.fixedValueCtrls[index].setValue("");
          }
        });
    }
    else if (detail.sequence == IMPORTER_BILLING_FIELDS.ExclusiveBankCode) {
      this.fixedValueCtrls[index].setValue(StringUtil.setPaddingFrontZero(this.fixedValueCtrls[index].value, 4, true));
    }
    else if (detail.sequence == IMPORTER_BILLING_FIELDS.ExclusiveBranchCode) {
      this.fixedValueCtrls[index].setValue(StringUtil.setPaddingFrontZero(this.fixedValueCtrls[index].value, 3, true));
    }
    else if (detail.sequence == IMPORTER_BILLING_FIELDS.ExclusiveVirtualBranchCode) {
      this.fixedValueCtrls[index].setValue(StringUtil.setPaddingFrontZero(this.fixedValueCtrls[index].value, 3, true));
    }
    else if (detail.sequence == IMPORTER_BILLING_FIELDS.ExclusiveAccountNumber) {
      this.fixedValueCtrls[index].setValue(StringUtil.setPaddingFrontZero(this.fixedValueCtrls[index].value, 7, true));
    }
    else {
      this.fixedValueCtrls[index].setValue("");
      return;
    }

  }

  public setReceiptImporterFixedValue(detail: ImporterSettingDetail, index: number) {

    if (detail.sequence == IMPORTER_RECEIPT_FIELDS.ReceiptCategoryCode) {
      this.fixedValueCtrls[index].setValue(StringUtil.setPaddingFrontZero(this.fixedValueCtrls[index].value, 2));

      this.loadStart();
      this.categoryService.GetItems(CategoryType.Receipt, this.fixedValueCtrls[index].value)
        .subscribe(response => {
          this.loadEnd();
          if (response != undefined && response.length > 0) {
            this.fixedValueCtrls[index].setValue(response[0].code);
          }
          else {
            this.fixedValueCtrls[index].setValue("");
          }
        });
    }
    else if (detail.sequence == IMPORTER_RECEIPT_FIELDS.SectionCode) {
      this.loadStart();
      this.sectionService.GetItems(this.fixedValueCtrls[index].value)
        .subscribe(response => {
          this.loadEnd();
          if (response != undefined && response.length > 0) {
            this.fixedValueCtrls[index].setValue(response[0].code);
          }
          else {
            this.fixedValueCtrls[index].setValue("");
          }
        });
    }
    else if (detail.sequence == IMPORTER_RECEIPT_FIELDS.CurrencyCode) {
      this.loadStart();
      this.currencyService.GetItems(this.fixedValueCtrls[index].value)
        .subscribe(response => {
          this.loadEnd();
          if (response != undefined && response.length > 0) {
            this.fixedValueCtrls[index].setValue(response[0].code);
          }
          else {
            this.fixedValueCtrls[index].setValue("");
          }
        });
    }
    else if (detail.sequence == IMPORTER_RECEIPT_FIELDS.AccountTypeId) {
      this.loadStart();
      this.bankAccountTypeService.GetItems()
        .subscribe(response => {
          this.loadEnd();
          if (response != undefined && response.length > 0) {
            let bankAccountTypes: Array<BankAccountType> = response;
            let exist: boolean = false;
            bankAccountTypes.forEach(element => {
              if (element.id == this.fixedValueCtrls[index].value && element.useReceipt == 1) {
                exist = true;
              }
            });

            if (!exist) {
              this.fixedValueCtrls[index].setValue("");
            }
          }
          else {
            this.fixedValueCtrls[index].setValue("");
          }
        });
    }
    else if (detail.sequence == IMPORTER_RECEIPT_FIELDS.BankCode) {
      this.fixedValueCtrls[index].setValue(StringUtil.setPaddingFrontZero(this.fixedValueCtrls[index].value, 4, true));
    }
    else if (detail.sequence == IMPORTER_RECEIPT_FIELDS.BranchCode) {
      this.fixedValueCtrls[index].setValue(StringUtil.setPaddingFrontZero(this.fixedValueCtrls[index].value, 3, true));
    }
    else if (detail.sequence == IMPORTER_RECEIPT_FIELDS.AccountNumber) {
      this.fixedValueCtrls[index].setValue(StringUtil.setPaddingFrontZero(this.fixedValueCtrls[index].value, 7, true));
    }
    else {
      this.fixedValueCtrls[index].setValue("");
      return;
    }
  }

  ///////////////////////////////////////////////////////////////
  public setChkDoUpdate(eventType: string, index: number) {
    if (this.IsRowField(index, IMPORTER_CUSTOMER_FIELDS.ClosingDay)) {
      this.chkDoUpdateCtrls[IMPORTER_CUSTOMER_FIELDS.CollectOffsetMonth - 1].setValue(this.chkDoUpdateCtrls[IMPORTER_CUSTOMER_FIELDS.ClosingDay - 1].value);
      this.chkDoUpdateCtrls[IMPORTER_CUSTOMER_FIELDS.CollectOffsetDay - 1].setValue(this.chkDoUpdateCtrls[IMPORTER_CUSTOMER_FIELDS.ClosingDay - 1].value);
    }
  }


  public setCmbImportDivision(eventType: string, index: number) {
    let detail = this.importerSettingDetaisResult.importerSettingDetails[index];
    let field = detail.sequence;


    if (StringUtil.IsNullOrEmpty(this.cmbImportDivisionCtrls[index].value)) {
      return;
    }

    detail.importDivision = this.cmbImportDivisionCtrls[index].value;

    let isUpdateKeyEnabled = this.IsUpdateKeyEnabled(detail);
    if (this.IsUpdateKeyEnabled(detail)) {
      this.chkDoUpdateCtrls[index].enable()
    }
    else {
      this.chkDoUpdateCtrls[index].disable()
      this.fixedValueCtrls[index].setValue("");
    }

    if (this.IsFixedValueEnabled(detail)) {
      this.fixedValueCtrls[index].enable()
    }
    else {
      this.fixedValueCtrls[index].disable()
      this.fixedValueCtrls[index].setValue("");
    }


    var isUpdateKeyChecked = isUpdateKeyEnabled && !this.IsFixedValueEnabled(detail);
    if (
      this.IsDoOrNotWithFixedImportDivision(detail)
      && this.IsCollectOffsetRelatedField(detail)
    ) /* 入金予定日 関連項目 */ {
      // 0 : 取込無 は強制 false それ以外は、締日 のチェック状態を参照
      if (detail.importDivision == 0) {
        isUpdateKeyChecked = false;
      }
      else {
        isUpdateKeyChecked = this.GetClosingDayUpdateKeyChecked();
      }

    }
    this.chkDoUpdateCtrls[index].setValue(isUpdateKeyChecked);

    let isFieldIndexEnabled = this.IsFieldIndexEnabled(detail);
    if (isFieldIndexEnabled) {
      this.fieldIndexCtrls[index].enable();
    }
    else {
      this.fieldIndexCtrls[index].disable();
      this.fieldIndexCtrls[index].setValue("");
    }

    if (this.IsAttribudeEnabled(detail)) {
      this.cmbAttributeCtrls[index].enable();
    }
    else {
      this.cmbAttributeCtrls[index].disable();
      this.cmbAttributeCtrls[index].setValue("");

    }

    if (this.IsRowField(index, IMPORTER_CUSTOMER_FIELDS.ThresholdValue)) {
      var maxOffset = this.GetThresholdRelatedFieldMaxOffset();
      for (let offset = 1; offset <= maxOffset; offset++) {
        let detailOffset = this.importerSettingDetaisResult.importerSettingDetails[index + offset];

        if (isFieldIndexEnabled) {
          this.fieldIndexCtrls[index + offset].enable()
        }
        else {
          this.fieldIndexCtrls[index + offset].disable();
          this.fieldIndexCtrls[index + offset].setValue("");
        }

        if (isUpdateKeyEnabled) {
          this.chkDoUpdateCtrls[index + offset].setValue(true);
        }
        else {
          this.chkDoUpdateCtrls[index + offset].setValue(null);
        }

        this.cmbImportDivisionCtrls[index + offset].setValue("1");

        detailOffset.importDivision = this.cmbImportDivisionCtrls[index].value;
        if (this.IsAttribudeEnabled(detailOffset)) {
          this.cmbAttributeCtrls[index + offset].enable();
        }
        else {
          this.cmbAttributeCtrls[index + offset].disable();
        }

        if (detailOffset.baseAttributeDivision > 0) {
          let key = "ATTR" + detailOffset.baseAttributeDivision;
          this.cmbAttributeList.push(
            this.settingsResult.settings.filter(
              function (setting: Setting) {
                return setting.itemId == key;
              }
            )
          );
        }
        else {
          this.cmbAttributeList.push(null);
        }

        if (this.cmbAttributeCtrls[index + offset].enabled) {
          this.cmbAttributeCtrls[index + offset].setValue(detailOffset.attributeDivision)
        }
        else {
          this.cmbAttributeCtrls[index + offset].setValue(null)
        }

      }
    }

    if (this.IsRowField(index, IMPORTER_CUSTOMER_FIELDS.ClosingDay)) {
      let start = IMPORTER_CUSTOMER_FIELDS.CollectOffsetMonth - IMPORTER_CUSTOMER_FIELDS.ClosingDay;
      let maxOffset = IMPORTER_CUSTOMER_FIELDS.CollectOffsetDayPerBilling - IMPORTER_CUSTOMER_FIELDS.CollectOffsetMonth + 1;

      for (let offset = start; offset <= maxOffset; offset++) {
        this.chkDoUpdateCtrls[index + offset].setValue(isUpdateKeyChecked);
      }

    }
  }



  public GetClosingDayUpdateKeyChecked(): boolean {

    return this.chkDoUpdateCtrls[IMPORTER_CUSTOMER_FIELDS.ClosingDay].value;

  }

  public IsRowField(index: number, field: IMPORTER_CUSTOMER_FIELDS): boolean {
    return index == (field - 1);
  }


  /// <summary>
  /// 約定関連項目 max offset 取得
  /// </summary>
  /// <returns></returns>
  public GetThresholdRelatedFieldMaxOffset(): number {
    return IMPORTER_CUSTOMER_FIELDS.GreaterThanSightOfBill3 - IMPORTER_CUSTOMER_FIELDS.ThresholdValue;

  }

  /*

  public setCmbImportDivision(eventType: string, index: number) {
    let detail = this.importerSettingDetaisResult.importerSettingDetails[index];
    let field = detail.sequence;

    if (StringUtil.IsNullOrEmpty(this.cmbImportDivisionCtrls[index].value)) {
      return;
    }

    let tmpSettingList: Array<Setting> = this.cmbImportDivisionList[index];

    // 表示内容の取得
    let importDivisionDesplayText = "";
    tmpSettingList.forEach(element => {
      if (element.itemKey == this.cmbImportDivisionCtrls[index].value) {
        importDivisionDesplayText = element.itemValue;
      }
    })

    if (importDivisionDesplayText.indexOf("取込有") > 0) {

      if (this.importerFileType == FreeImporterFormatType.Billing) {
        if (this.isOverwriteEnable(field)) {
          this.chkDoUpdateCtrls[index].enable();
        }
        else {
          this.chkDoUpdateCtrls[index].disable();
          this.chkDoUpdateCtrls[index].setValue(false);
        }
      }

      this.fixedValueCtrls[index].disable();
      this.fixedValueCtrls[index].setValue(null);

      this.fieldIndexCtrls[index].enable();
      this.fieldIndexCtrls[index].setValue(detail.fieldIndex);

      if (this.isCmbAttributeEnableOrDisable(field)) {
        this.cmbAttributeCtrls[index].disable();
        this.cmbAttributeCtrls[index].setValue(null);
      }
      else {
        this.cmbAttributeCtrls[index].enable();
      }

      if (this.isReceiptImporterBillBankCode(field)) {
        this.cmbImportDivisionCtrls[index].setValue(1);

        this.fieldIndexCtrls[index].enable();
        this.fieldIndexCtrls[index].setValue(detail.fieldIndex);

      }
      if (this.isReceiptImporterCustomerCode(field)) {
        // 顧客(仕様が不明)
        this.fieldIndexCtrls[index].enable();
        this.fieldIndexCtrls[index].setValue(detail.fieldIndex);
      }

    }
    else if (importDivisionDesplayText.indexOf("固定値") > 0) {

      this.fixedValueCtrls[index].enable();
      this.fixedValueCtrls[index].setValue(detail.fixedValue);

      this.fieldIndexCtrls[index].disable();
      this.fieldIndexCtrls[index].setValue(null);

      this.cmbAttributeCtrls[index].disable();
      this.cmbAttributeCtrls[index].setValue(null);

      this.chkDoUpdateCtrls[index].disable();
      this.chkDoUpdateCtrls[index].setValue(false);

    }
    else if (importDivisionDesplayText.indexOf("取込無") > 0) {

      this.fixedValueCtrls[index].disable();
      this.fixedValueCtrls[index].setValue(null);

      this.fieldIndexCtrls[index].disable();
      this.fieldIndexCtrls[index].setValue(null);

      this.cmbAttributeCtrls[index].disable();
      this.cmbAttributeCtrls[index].setValue(null);

      this.chkDoUpdateCtrls[index].disable();
      this.chkDoUpdateCtrls[index].setValue(false);

      if (this.isReceiptImporterBillBankCode(field)) {
        this.cmbImportDivisionCtrls[index].setValue(0);

        this.fieldIndexCtrls[index].setValue(null);
        this.fieldIndexCtrls[index].disable();

      }
      if (this.isReceiptImporterCustomerCode(field)) {
        // 顧客(仕様が不明)
        this.fieldIndexCtrls[index].disable();
        this.fieldIndexCtrls[index].setValue("");
      }

    }

  }


  public isBillingImportDivisionEnableOrDisable(field: number) {

    let bEnable = field == IMPORTER_BILLING_FIELDS.CompanyCode
      || field == IMPORTER_BILLING_FIELDS.CustomerCode
      || field == IMPORTER_BILLING_FIELDS.BilledAt
      || field == IMPORTER_BILLING_FIELDS.Quantity
      || field == IMPORTER_BILLING_FIELDS.UnitSymbol
      || field == IMPORTER_BILLING_FIELDS.UnitPrice
      || field == IMPORTER_BILLING_FIELDS.TaxRate
      || (field == IMPORTER_BILLING_FIELDS.ContractNumber && this.userInfoService.ApplicationControl.registerContractInAdvance == 0);

    return bEnable;
  }
  */

  public isCmbAttributeEnableOrDisable(field: number): boolean {

    let bEnable = false;
    switch (this.importerFileType) {
      case FreeImporterFormatType.Billing:
        {
          bEnable = field == IMPORTER_BILLING_FIELDS.CompanyCode
            || field == IMPORTER_BILLING_FIELDS.CustomerCode
            || field == IMPORTER_BILLING_FIELDS.BillingAmount
            || field == IMPORTER_BILLING_FIELDS.TaxAmount
            || field == IMPORTER_BILLING_FIELDS.DepartmentCode
            || field == IMPORTER_BILLING_FIELDS.DebitAccountTitleCode
            || field == IMPORTER_BILLING_FIELDS.StaffCode
            || field == IMPORTER_BILLING_FIELDS.Quantity
            || field == IMPORTER_BILLING_FIELDS.UnitSymbol
            || field == IMPORTER_BILLING_FIELDS.UnitPrice
            || field == IMPORTER_BILLING_FIELDS.Price
            || field == IMPORTER_BILLING_FIELDS.TaxClassId;
        }
        break;
      case FreeImporterFormatType.Receipt:
        {
          bEnable = field == IMPORTER_RECEIPT_FIELDS.CustomerCode
            || field == IMPORTER_RECEIPT_FIELDS.ReceiptAmount
            || field == IMPORTER_RECEIPT_FIELDS.SectionCode
            || field == IMPORTER_RECEIPT_FIELDS.CurrencyCode
            || field == IMPORTER_RECEIPT_FIELDS.BillBankCode
            || field == IMPORTER_RECEIPT_FIELDS.BillBranchCode
            || field == IMPORTER_RECEIPT_FIELDS.BankCode
            || field == IMPORTER_RECEIPT_FIELDS.BranchCode
            || field == IMPORTER_RECEIPT_FIELDS.AccountTypeId
            || field == IMPORTER_RECEIPT_FIELDS.AccountNumber
            || field == IMPORTER_RECEIPT_FIELDS.BillNumber;
        }
        break;
    }
    return bEnable;
  }

  public isNotesFields(field: number): boolean {

    let bResult = false;
    switch (this.importerFileType) {
      case FreeImporterFormatType.Billing:
        {
          bResult = field == IMPORTER_BILLING_FIELDS.Note1
            || field == IMPORTER_BILLING_FIELDS.Note2
            || field == IMPORTER_BILLING_FIELDS.Note3
            || field == IMPORTER_BILLING_FIELDS.Note4
            || field == IMPORTER_BILLING_FIELDS.Note5
            || field == IMPORTER_BILLING_FIELDS.Note6
            || field == IMPORTER_BILLING_FIELDS.Note7
            || field == IMPORTER_BILLING_FIELDS.Note8;
          break;
        }
      case FreeImporterFormatType.Receipt:
        {
          bResult = field == IMPORTER_RECEIPT_FIELDS.Note1
            || field == IMPORTER_RECEIPT_FIELDS.Note2
            || field == IMPORTER_RECEIPT_FIELDS.Note3
            || field == IMPORTER_RECEIPT_FIELDS.Note4;
          break;
        }
    }
    return bResult;

  }

  public isUniqueDisable(field: number): boolean {
    let bResult = false;

    switch (this.importerFileType) {
      case FreeImporterFormatType.Billing:
        {
          bResult =
            field == IMPORTER_BILLING_FIELDS.CompanyCode
            || field == IMPORTER_BILLING_FIELDS.ContractNumber
            || field == IMPORTER_BILLING_FIELDS.CustomerName
            || field == IMPORTER_BILLING_FIELDS.CustomerKana
            || field == IMPORTER_BILLING_FIELDS.UseDiscount
            || field == IMPORTER_BILLING_FIELDS.ExclusiveBankCode
            || field == IMPORTER_BILLING_FIELDS.ExclusiveBranchCode
            || field == IMPORTER_BILLING_FIELDS.ExclusiveVirtualBranchCode
            || field == IMPORTER_BILLING_FIELDS.ExclusiveAccountNumber
            || field == IMPORTER_BILLING_FIELDS.TaxRate;
        }
        break;
      case FreeImporterFormatType.Receipt:
        //入金フリーインポーターの場合は全項目使用可能
        break;
    }

    return bResult;
  }

  public isOverwriteEnable(field: number): boolean {

    let bResult =
      field == IMPORTER_BILLING_FIELDS.CustomerCode
      || field == IMPORTER_BILLING_FIELDS.BilledAt
      || field == IMPORTER_BILLING_FIELDS.InvoiceCode
      || field == IMPORTER_BILLING_FIELDS.Note1
      || field == IMPORTER_BILLING_FIELDS.Note2
      || field == IMPORTER_BILLING_FIELDS.Note3
      || field == IMPORTER_BILLING_FIELDS.Note4
      || field == IMPORTER_BILLING_FIELDS.Note5
      || field == IMPORTER_BILLING_FIELDS.Note6
      || field == IMPORTER_BILLING_FIELDS.Note7
      || field == IMPORTER_BILLING_FIELDS.Note8
      || field == IMPORTER_BILLING_FIELDS.CurrencyCode
      || field == IMPORTER_BILLING_FIELDS.TaxRate;

    return bResult;
  }

  public isCustomerMasterFixedValueEnabled(field: number): boolean {

    let bResult =
      field == IMPORTER_BILLING_FIELDS.DueAt
      || field == IMPORTER_BILLING_FIELDS.ClosingAt
      || field == IMPORTER_BILLING_FIELDS.CollectCategoryCode
      || field == IMPORTER_BILLING_FIELDS.StaffCode
      || field == IMPORTER_BILLING_FIELDS.CustomerName
      || field == IMPORTER_BILLING_FIELDS.CustomerKana;

    return bResult;

  }

  public isReceiptImporterBillBankCode(field: number): boolean {
    return this.importerFileType == FreeImporterFormatType.Receipt
      && field == IMPORTER_RECEIPT_FIELDS.BillBankCode;
  }

  public isReceiptImporterCustomerCode(field: number): boolean {
    return this.importerFileType == FreeImporterFormatType.Receipt
      && field == IMPORTER_RECEIPT_FIELDS.CustomerCode;
  }

  public setPlaceHolder(index: number): string {

    if (this.importerSettingDetaisResult == undefined) return "";

    let result: string;
    let detail = this.importerSettingDetaisResult.importerSettingDetails[index];

    if (detail.sequence == IMPORTER_CUSTOMER_FIELDS.ShareTransferFee
      || detail.sequence == IMPORTER_CUSTOMER_FIELDS.CollectCategoryId
      || detail.sequence == IMPORTER_CUSTOMER_FIELDS.StaffCode
      || detail.sequence == IMPORTER_CUSTOMER_FIELDS.HolidayFlag
      || detail.sequence == IMPORTER_CUSTOMER_FIELDS.UseFeeLearning
      || detail.sequence == IMPORTER_CUSTOMER_FIELDS.UseKanaLearning
      || detail.sequence == IMPORTER_CUSTOMER_FIELDS.UseFeeTolerance
      || detail.sequence == IMPORTER_CUSTOMER_FIELDS.PrioritizeMatchingIndividually
      || detail.sequence == IMPORTER_CUSTOMER_FIELDS.IsParent
      || detail.sequence == IMPORTER_CUSTOMER_FIELDS.ExcludeInvoicePublish
      || detail.sequence == IMPORTER_CUSTOMER_FIELDS.ExcludeReminderPublish) {
      result = this.PlaceHolderConst.F9_SEARCH;
    }

    return result;
  }

}
