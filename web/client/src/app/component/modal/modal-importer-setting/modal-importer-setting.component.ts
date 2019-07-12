import { Component, OnInit, EventEmitter, ViewContainerRef, ComponentFactoryResolver, ElementRef } from '@angular/core';
import { BaseComponent } from '../../common/base/base-component';
import { Validators, FormControl, FormGroup } from '@angular/forms';
import { CustomValidators } from 'ng5-validation';
import { ModalMasterComponent } from '../modal-master/modal-master.component';
import { ImporterSettingDetailsResult } from 'src/app/model/importer-setting-details-result.model';
import { StringUtil } from 'src/app/common/util/string-util';
import { TABLE_INDEX, TABLE_NAME } from 'src/app/common/const/table-name.const';
import { KEY_CODE, EVENT_TYPE, BUTTON_ACTION } from 'src/app/common/const/event.const';
import { MODAL_STATUS_TYPE, COMPONENT_STATUS_TYPE, PROCESS_RESULT_RESULT_TYPE } from 'src/app/common/const/status.const';
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
import { IMPORTER_RECEIPT_FIELDS } from 'src/app/common/const/importer.const';
import { IMPORTER_BILLING_FIELDS } from 'src/app/common/const/importer.const';
import { CategoryMasterService } from 'src/app/service/Master/category-master.service';
import { SectionMasterService } from 'src/app/service/Master/section-master.service';
import { CurrencyMasterService } from 'src/app/service/Master/currency-master.service';
import { CategoryType, CODE_TYPE } from 'src/app/common/const/kbn.const';
import { BankAccountTypeMasterService } from 'src/app/service/Master/bank-account-type-master.service';
import { BankAccountType } from 'src/app/model/bank-account-type.model';
import { DepartmentMasterService } from 'src/app/service/Master/department-master.service';
import { AccountTitleMasterService } from 'src/app/service/Master/account-title-master.service'
import { StaffMasterService } from 'src/app/service/Master/staff-master.service'
import { HtmlUtil } from 'src/app/common/util/html.util';
import { ProcessResultService } from 'src/app/service/common/process-result.service';
import { MSG_WNG, MSG_ITEM_NUM, MSG_INF } from 'src/app/common/const/message.const';
import { EbDataHelper } from 'src/app/common/util/eb-data-helper';
import { JuridicalPersonalitysResult } from 'src/app/model/juridical-personalitys-result.model';
import { JuridicalPersonalityMasterService } from 'src/app/service/Master/juridical-personality-master.service';
import { ModalRouterProgressComponent } from '../modal-router-progress/modal-router-progress.component';

@Component({
  selector: 'app-modal-importer-setting',
  templateUrl: './modal-importer-setting.component.html',
  styleUrls: ['./modal-importer-setting.component.css']
})
export class ModalImporterSettingComponent extends BaseComponent implements OnInit {

  public FreeImporterFormatType: typeof FreeImporterFormatType = FreeImporterFormatType;

  public importerSettingId: number;

  public settingsResult: SettingsResult;
  public columnNameSettingsResult: ColumnNameSettingsResult
  public importerFileType: number;

  public cmbAttributeList: Array<Array<Setting>>;
  public cmbImportDivisionList: Array<Array<Setting>>;
  public billingAmountSetting: Array<Setting>;
  public taxAmountSetting: Array<Setting>;

  public chkDuplicateCtrls: Array<FormControl>;
  public chkDoOverwriteCtrls: Array<FormControl>;
  public cmbImportDivisionCtrls: Array<FormControl>;
  public fixedValueCtrls: Array<FormControl>;
  public fieldIndexCtrls: Array<FormControl>;
  public cmbAttributeCtrls: Array<FormControl>;

  public selectedImportSetting: ImporterSetting;
  public importerSettingDetaisResult: ImporterSettingDetailsResult = null;
  public hiddenImporterSettingDetails: Array<ImporterSettingDetail>;

  public patternNoCtrl: FormControl;              // パターンNo
  public patternNameCtrl: FormControl;            // パターン名

  public startLineCountCtrl: FormControl;          // 取込開始行	

  public ignoreLastLineCtrl: FormControl;         // 最終行を取込まない	
  public autoCreationCustomerCtrl: FormControl;   // 得意先マスター自動作成フラグ	
  public autoSettingDestinationCtrl: FormControl;   // 送付先コードを自動でセットする。
  public UndefineCtrl: FormControl;

  public juridicalPersonalitiesResult: JuridicalPersonalitysResult;  // 法人格除去用

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



  constructor(
    public elementRef: ElementRef,
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
    public juridicalPersonalityService: JuridicalPersonalityMasterService,
  ) {
    super();
  }

  ngOnInit() {
    this.setControlInit();
    this.setValidator();
    this.setFormatter();
    this.clear();

    let settingResponse = this.settingService.GetItems();

    if (this.Title.startsWith("請求")) {
      this.importerFileType = FreeImporterFormatType.Billing;
    } else if (this.Title.startsWith("入金")) {
      this.importerFileType = FreeImporterFormatType.Receipt;
    }
    let columnNameSetting = this.columnNameSettingService.Get(this.importerFileType);
    let juridicalPersonalityResponse = this.juridicalPersonalityService.GetItems();

    forkJoin(
      settingResponse,
      columnNameSetting,
      juridicalPersonalityResponse
    )
      .subscribe(responseList => {

        this.settingsResult = new SettingsResult();
        this.settingsResult.settings = responseList[0];

        this.columnNameSettingsResult = new ColumnNameSettingsResult();
        this.columnNameSettingsResult.columnNames = responseList[1];

        this.juridicalPersonalitiesResult = new JuridicalPersonalitysResult();
        this.juridicalPersonalitiesResult.juridicalPersonalities = responseList[2];
      });

  }

  public setControlInit() {
    this.patternNoCtrl = new FormControl("", [Validators.required, Validators.maxLength(2)]);
    this.patternNameCtrl = new FormControl("", [Validators.required, Validators.maxLength(40)]);

    this.startLineCountCtrl = new FormControl({ value: "1" }, [Validators.required, CustomValidators.range([1, 9])]);

    this.ignoreLastLineCtrl = new FormControl("");
    this.autoCreationCustomerCtrl = new FormControl("");
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
      autoCreationCustomerCtrl: this.autoCreationCustomerCtrl,
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
          if (field == IMPORTER_BILLING_FIELDS.DepartmentCode) {
            this.fixedValueCtrls[index].setValidators([Validators.maxLength(this.userInfoService.ApplicationControl.departmentCodeLength)]);
            if (this.userInfoService.ApplicationControl.departmentCodeType == CODE_TYPE.NUMBER) {
              FormatterUtil.setNumberFormatter(this.fixedValueCtrls[index]);
            }
            else {
              FormatterUtil.setCodeFormatter(this.fixedValueCtrls[index]);
            }
          }
          if (field == IMPORTER_BILLING_FIELDS.DebitAccountTitleCode) {
            this.fixedValueCtrls[index].setValidators([Validators.maxLength(this.userInfoService.ApplicationControl.accountTitleCodeLength)]);
            if (this.userInfoService.ApplicationControl.accountTitleCodeType == CODE_TYPE.NUMBER) {
              FormatterUtil.setNumberFormatter(this.fixedValueCtrls[index]);
            }
            else {
              FormatterUtil.setCodeFormatter(this.fixedValueCtrls[index]);
            }
          }
          if (field == IMPORTER_BILLING_FIELDS.ClosingAt) {
            this.fixedValueCtrls[index].setValidators([Validators.maxLength(2)]);
            FormatterUtil.setNumberFormatter(this.fixedValueCtrls[index]);
          }
          if (field == IMPORTER_BILLING_FIELDS.StaffCode) {
            this.fixedValueCtrls[index].setValidators([Validators.maxLength(this.userInfoService.ApplicationControl.staffCodeLength)]);
            if (this.userInfoService.ApplicationControl.staffCodeType == CODE_TYPE.NUMBER) {
              FormatterUtil.setNumberFormatter(this.fixedValueCtrls[index]);
            }
            else {
              FormatterUtil.setCodeFormatter(this.fixedValueCtrls[index]);
            }
          }
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
          else if (field == IMPORTER_BILLING_FIELDS.DueAt) {
            this.fixedValueCtrls[index].setValidators([Validators.maxLength(3)]);
            FormatterUtil.setNumberFormatter(this.fixedValueCtrls[index]);
          }
          else if (field == IMPORTER_BILLING_FIELDS.CustomerName) {
            //ImporterSettingDetail.FixedValueがNVarChar(50)のため
            this.fixedValueCtrls[index].setValidators([Validators.maxLength(50)]);
          }
          else if (field == IMPORTER_BILLING_FIELDS.CustomerKana) {
            this.fixedValueCtrls[index].setValidators([Validators.maxLength(50)]);
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
          else if (field == IMPORTER_RECEIPT_FIELDS.SectionCode) {
            this.fixedValueCtrls[index].setValidators([Validators.maxLength(this.userInfoService.ApplicationControl.sectionCodeLength)]);
            if (this.userInfoService.ApplicationControl.sectionCodeType == CODE_TYPE.NUMBER) {
              FormatterUtil.setNumberFormatter(this.fixedValueCtrls[index]);
            }
            else {
              FormatterUtil.setCodeFormatter(this.fixedValueCtrls[index]);
            }
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

      case BUTTON_ACTION.REFERE_NEW:
        this.refereNew();
        break;
      default:
        console.log('buttonAction Error.');
        break;
    }

  }

  public registry() {

    // チェック
    switch (this.importerFileType) {
      case FreeImporterFormatType.Billing:
        {
          if (!this.isValidInputForBillingImporter()) {
            return false;
          }
        }
        break;
      case FreeImporterFormatType.Receipt:
        {
          if (!this.isValidInputForReceiptImporter()) {
            return false;
          }
        }
        break;
    }

    let importerSetting = new ImporterSetting();
    importerSetting.id = this.selectedImportSetting == null ? 0 : this.selectedImportSetting.id;
    importerSetting.companyId = this.userInfoService.Company.id,
      importerSetting.code = this.patternNoCtrl.value;
    importerSetting.name = this.patternNameCtrl.value;
    importerSetting.formatId = this.importerFileType;
    //importerSetting.initialDirectory    = this.importFileCtrl.value;
    importerSetting.initialDirectory = "C:\\Usr";    //仮設定
    importerSetting.startLineCount = this.startLineCountCtrl.value;
    importerSetting.ignoreLastLine = this.ignoreLastLineCtrl.value == true ? 1 : 0;
    importerSetting.autoCreationCustomer = this.autoCreationCustomerCtrl.value == true ? 1 : 0;
    importerSetting.updateBy = this.userInfoService.LoginUser.id;
    //importerSetting.updateAt            = updateAt,
    importerSetting.createBy = this.userInfoService.LoginUser.id;
    importerSetting.postAction = 0;
    importerSetting.details = new Array<ImporterSettingDetail>();

    // 登録処理
    for (var index = 0; index < this.importerSettingDetaisResult.importerSettingDetails.length; index++) {
      let importerSettingDetail = new ImporterSettingDetail();

      importerSettingDetail.sequence = (this.importerSettingDetaisResult.importerSettingDetails[index].sequence);
      importerSettingDetail.isUnique = this.chkDuplicateCtrls[index].value == true ? 1 : 0;
      importerSettingDetail.doOverwrite = this.chkDoOverwriteCtrls[index].value == true ? 1 : 0;

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
      importerSettingDetail.fieldIndex = this.fieldIndexCtrls[index].value;
      importerSettingDetail.attributeDivision = StringUtil.IsNullOrEmpty(this.cmbAttributeCtrls[index].value) ? "0" : this.cmbAttributeCtrls[index].value;

      importerSetting.details.push(importerSettingDetail);

    }

    //退避した非表示項目を戻す
    importerSetting.details.concat(this.hiddenImporterSettingDetails);

    let processComponentFactory = this.componentFactoryResolver.resolveComponentFactory(ModalRouterProgressComponent);
    let processComponentRef = this.viewContainerRef.createComponent(processComponentFactory);
    // processComponentRef.destroy();       

    let importerSettingsResult = new ImporterSettingsResult();
    this.importerSettingService.Save(importerSetting)
      .subscribe(response => {

        this.processResultService.processAtSave(
          this.processModalCustomResult, response, true, this.partsResultMessageComponent);

        importerSettingsResult.importerSettings = new Array<ImporterSetting>();
        let importerSetting: ImporterSetting = response;
        importerSettingsResult.importerSettings.push(importerSetting);

        processComponentRef.destroy();
      });

  }

  public isValidInputForBillingImporter(): boolean {

    let collectOffsetMonth: any;
    let closingDay: any;
    let allDisable = true;
    let allEnabled = true;

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

      if (this.fixedValueCtrls[index].enabled && StringUtil.IsNullOrEmpty(this.fixedValueCtrls[index].value)) {
        this.processModalCustomResult = this.processResultService.processAtWarning(
          this.processModalCustomResult, MSG_WNG.NO_INPUT_FIXED_VALUE.replace(MSG_ITEM_NUM.FIRST, detail.fieldName),
          this.partsResultMessageComponent);
        HtmlUtil.nextFocusByName(this.elementRef, 'fixedValueCtrl' + index, EVENT_TYPE.NONE);
        return false;
      }
      else if (this.fieldIndexCtrls[index].enabled && this.fieldIndexCtrls[index].value == 0) {
        this.processModalCustomResult = this.processResultService.processAtWarning(
          this.processModalCustomResult, MSG_WNG.INPUT_REQUIRED.replace(MSG_ITEM_NUM.FIRST, '項目番号'),
          this.partsResultMessageComponent);
        HtmlUtil.nextFocusByName(this.elementRef, 'fieldIndexCtrl' + index, EVENT_TYPE.NONE);
        return false;
      }
      else if (this.cmbAttributeCtrls[index].enabled && StringUtil.IsNullOrEmpty(this.cmbAttributeCtrls[index].value)) {
        this.processModalCustomResult = this.processResultService.processAtWarning(
          this.processModalCustomResult, MSG_WNG.SELECTION_REQUIRED.replace(MSG_ITEM_NUM.FIRST, '属性情報'),
          this.partsResultMessageComponent);
        HtmlUtil.nextFocusByName(this.elementRef, 'cmbAttributeCtrl' + index, EVENT_TYPE.NONE);
        return false;
      }
      else if (this.fixedValueCtrls[index].enabled && !StringUtil.IsNullOrEmpty(this.fixedValueCtrls[index].value)) {
        //入金予定日チェック
        if (field == IMPORTER_BILLING_FIELDS.DueAt) {
          let dueDate: string = this.fixedValueCtrls[index].value;
          let bRet = dueDate.length == 3
            && (NumberUtil.ParseInt(dueDate.substr(0, 1)) >= 0 && NumberUtil.ParseInt(dueDate.substr(0, 1)) < 10)
            && (NumberUtil.ParseInt(dueDate.substr(1)) > 0 && NumberUtil.ParseInt(dueDate.substr(1)) < 28 || NumberUtil.ParseInt(dueDate.substr(1)) == 99);

          if (!bRet) {
            this.processModalCustomResult = this.processResultService.processAtWarning(
              this.processModalCustomResult, MSG_WNG.INVALID_VALUE_OF_RECEIPT_DUE_DATE_FORMAT,
              this.partsResultMessageComponent);
            HtmlUtil.nextFocusByName(this.elementRef, 'fixedValueCtrl' + index, EVENT_TYPE.NONE);
            return false;
          }
          else {
            collectOffsetMonth = NumberUtil.ParseInt(dueDate.substr(0, 1));
          }
        }
        //請求締日チェック
        if (field == IMPORTER_BILLING_FIELDS.ClosingAt) {
          let closingAt: string = this.fixedValueCtrls[index].value;
          let bRet = NumberUtil.ParseInt(closingAt) >= 0 && NumberUtil.ParseInt(closingAt) < 28 || NumberUtil.ParseInt(closingAt) == 99;

          if (!bRet) {
            this.processModalCustomResult = this.processResultService.processAtWarning(
              this.processModalCustomResult, MSG_WNG.NUMBER_VALUE_VALID0_TO_27_OR_99.replace(MSG_ITEM_NUM.FIRST, '請求締日'),
              this.partsResultMessageComponent);
            HtmlUtil.nextFocusByName(this.elementRef, 'fixedValueCtrl' + index, EVENT_TYPE.NONE);
            return false;
          }
          else {
            closingDay = NumberUtil.ParseInt(closingAt);
          }
        }

      }
      else if (this.cmbAttributeCtrls[index].enabled
        && (StringUtil.IsNullOrEmpty(this.cmbAttributeCtrls[index].value) || this.cmbAttributeCtrls[index].value == "0")
      ) {
        this.processModalCustomResult = this.processResultService.processAtWarning(
          this.processModalCustomResult, MSG_WNG.SELECTION_REQUIRED.replace(MSG_ITEM_NUM.FIRST, '属性情報'),
          this.partsResultMessageComponent);
        HtmlUtil.nextFocusByName(this.elementRef, 'cmbAttributeCtrl' + index, EVENT_TYPE.NONE);
        return false;
      }

      if (field == IMPORTER_BILLING_FIELDS.ExclusiveBankCode) {
        allDisable = allDisable && importDivisionDisplayText.indexOf("取込無") > 0;
        allEnabled = allEnabled && !(importDivisionDisplayText.indexOf("取込無") > 0);
      }
      if (field == IMPORTER_BILLING_FIELDS.ExclusiveBranchCode) {
        allDisable = allDisable && importDivisionDisplayText.indexOf("取込無") > 0;
        allEnabled = allEnabled && !(importDivisionDisplayText.indexOf("取込無") > 0);
      }
      if (field == IMPORTER_BILLING_FIELDS.ExclusiveVirtualBranchCode) {
        allDisable = allDisable && importDivisionDisplayText.indexOf("取込無") > 0;
        allEnabled = allEnabled && !(importDivisionDisplayText.indexOf("取込無") > 0);
      }
      if (field == IMPORTER_BILLING_FIELDS.ExclusiveAccountNumber) {
        allDisable = allDisable && importDivisionDisplayText.indexOf("取込無") > 0;
        allEnabled = allEnabled && !(importDivisionDisplayText.indexOf("取込無") > 0);
      }

    }

    if ((collectOffsetMonth != undefined && closingDay != undefined && closingDay == 0 && collectOffsetMonth != 0)) {
      this.processModalCustomResult = this.processResultService.processAtWarning(
        this.processModalCustomResult, MSG_WNG.NUMBER_VALUE_VALID0_FOR_FIRST_AND00_TO_99_FOR_REST,
        this.partsResultMessageComponent);
      return false;
    }

    if (!allDisable && (!allDisable && !allEnabled)) {
      this.processModalCustomResult = this.processResultService.processAtWarning(
        this.processModalCustomResult, MSG_WNG.BANK_INFO_INCOMPLETE, this.partsResultMessageComponent);
      return false;
    }

    return true;
  }

  public isValidInputForReceiptImporter(): boolean {

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

      if (
        (field == IMPORTER_RECEIPT_FIELDS.RecordedAt || field == IMPORTER_RECEIPT_FIELDS.ReceiptAmount)
        && StringUtil.IsNullOrEmpty(this.fieldIndexCtrls[index].value)) {
        this.processModalCustomResult = this.processResultService.processAtWarning(
          this.processModalCustomResult, MSG_WNG.INPUT_REQUIRED.replace(MSG_ITEM_NUM.FIRST, '項目番号'),
          this.partsResultMessageComponent);
        HtmlUtil.nextFocusByName(this.elementRef, 'fieldIndexCtrl' + index, EVENT_TYPE.NONE);
        return false;
      }
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
        if (field == IMPORTER_RECEIPT_FIELDS.ReceiptCategoryCode) {
          // 設定時に存在チェックをしているので不要
        }

        if ((field == IMPORTER_RECEIPT_FIELDS.BankName
          || field == IMPORTER_RECEIPT_FIELDS.BranchName
          || field == IMPORTER_RECEIPT_FIELDS.AccountName)
          && StringUtil.IsNullOrEmpty(this.fixedValueCtrls[index].value)) {
          this.processModalCustomResult = this.processResultService.processAtWarning(
            this.processModalCustomResult, MSG_WNG.NO_INPUT_FIXED_VALUE.replace(MSG_ITEM_NUM.FIRST, detail.fieldName),
            this.partsResultMessageComponent);
          HtmlUtil.nextFocusByName(this.elementRef, 'fixedValueCtrl' + index, EVENT_TYPE.NONE);
          return false;
        }

        if (field == IMPORTER_RECEIPT_FIELDS.SectionCode) {
          // 設定時に存在チェックをしているので不要
        }
        if (field == IMPORTER_RECEIPT_FIELDS.CurrencyCode) {
          // 設定時に存在チェックをしているので不要
        }
        if (field == IMPORTER_RECEIPT_FIELDS.AccountTypeId) {
          // 設定時に存在チェックをしているので不要
        }
      }
      else if (this.cmbAttributeCtrls[index].enabled
        && (StringUtil.IsNullOrEmpty(this.cmbAttributeCtrls[index].value) || this.cmbAttributeCtrls[index].value == "0")
      ) {
        this.processModalCustomResult = this.processResultService.processAtWarning(
          this.processModalCustomResult, MSG_WNG.SELECTION_REQUIRED.replace(MSG_ITEM_NUM.FIRST, '属性情報'),
          this.partsResultMessageComponent);
        HtmlUtil.nextFocusByName(this.elementRef, 'cmbAttributeCtrl' + index, EVENT_TYPE.NONE);
        return false;
      }
    }
    return true;
  }

  public delete() {

    let componentFactory = this.componentFactoryResolver.resolveComponentFactory(ModalConfirmComponent);
    let componentRef = this.viewContainerRef.createComponent(componentFactory);
    componentRef.instance.ActionName = "削除"
    componentRef.instance.Closing.subscribe((response) => {

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

            this.processResultService.processAtDelete(
              this.processModalCustomResult, response, this.partsResultMessageComponent);

            importerSettingsResult.importerSettings = new Array<ImporterSetting>();
            let importerSetting: ImporterSetting = response;
            importerSettingsResult.importerSettings.push(importerSetting);

            processComponentRef.destroy();
            this.clear();
          });

      }
      else {
        componentRef.destroy();
      }
    });

  }

  public refereNew() {

    let componentFactory = this.componentFactoryResolver.resolveComponentFactory(ModalMasterComponent);
    let componentRef = this.viewContainerRef.createComponent(componentFactory);

    switch (this.importerFileType) {
      case FreeImporterFormatType.Billing:
        componentRef.instance.TableIndex = TABLE_INDEX.MASTER_BILLING_IMPORTER_SETTING;
        break;
      case FreeImporterFormatType.Receipt:
        componentRef.instance.TableIndex = TABLE_INDEX.MASTER_RECEIPT_IMPORTER_SETTING;
        break;
    }

    componentRef.instance.Closing.subscribe(() => {

      if (componentRef.instance.ModalStatus == MODAL_STATUS_TYPE.SELECT) {

        this.selectedImportSetting = componentRef.instance.SelectedObject;

        const searchCode = this.patternNoCtrl.value;
        this.patternNoCtrl.setValue(this.selectedImportSetting.code);
        this.GetDetails(this.importerFileType);
        this.patternNoCtrl.setValue(searchCode);

      }

      this.ComponentStatus = COMPONENT_STATUS_TYPE.UPDATE;

      componentRef.destroy();

    });
  }

  public clear() {
    this.MyFormGroup.reset();
    this.importerSettingDetaisResult = null;

    this.patternNoCtrl.enable();
    this.startLineCountCtrl.setValue("");

    HtmlUtil.nextFocusByName(this.elementRef, 'patternNoCtrl', EVENT_TYPE.NONE);

    this.ComponentStatus = COMPONENT_STATUS_TYPE.CREATE;
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

    switch (this.importerFileType) {
      case FreeImporterFormatType.Billing:
        componentRef.instance.TableIndex = TABLE_INDEX.MASTER_BILLING_IMPORTER_SETTING;
        break;
      case FreeImporterFormatType.Receipt:
        componentRef.instance.TableIndex = TABLE_INDEX.MASTER_RECEIPT_IMPORTER_SETTING;
        break;
    }

    componentRef.instance.Closing.subscribe(() => {

      if (componentRef.instance.ModalStatus == MODAL_STATUS_TYPE.SELECT) {

        this.ComponentStatus = COMPONENT_STATUS_TYPE.UPDATE;

        this.patternNoCtrl.setValue(componentRef.instance.SelectedCode);
        this.patternNameCtrl.setValue(componentRef.instance.SelectedName);
        this.selectedImportSetting = componentRef.instance.SelectedObject;

        this.patternNoCtrl.disable();
        this.GetDetails(this.importerFileType);

      }

      this.ComponentStatus = COMPONENT_STATUS_TYPE.UPDATE;

      componentRef.destroy();

    });
  }

  public openMasterModalForDetail(keyCode: string = null, index: number = -1) {


    let detail = this.importerSettingDetaisResult.importerSettingDetails[index];

    let table: number;

    switch (this.importerFileType) {
      case FreeImporterFormatType.Billing:
        {
          if (detail.sequence == IMPORTER_BILLING_FIELDS.DepartmentCode) {
            table = TABLE_INDEX.MASTER_DEPARTMENT;
          }
          else if (detail.sequence == IMPORTER_BILLING_FIELDS.DebitAccountTitleCode) {
            table = TABLE_INDEX.MASTER_ACCOUNT_TITLE;
          }
          else if (detail.sequence == IMPORTER_BILLING_FIELDS.StaffCode) {
            table = TABLE_INDEX.MASTER_STAFF;
          }
          else if (detail.sequence == IMPORTER_BILLING_FIELDS.CurrencyCode) {
            table = TABLE_INDEX.MASTER_CURRENCY;
          }
          else if (detail.sequence == IMPORTER_BILLING_FIELDS.BillingCategoryCode) {
            table = TABLE_INDEX.MASTER_BILLING_CATEGORY;
          }
          else if (detail.sequence == IMPORTER_BILLING_FIELDS.CollectCategoryCode) {
            table = TABLE_INDEX.MASTER_COLLECT_CATEGORY;
          }
          else {
            return;
          }
          break;
        }
      case FreeImporterFormatType.Receipt:
        {
          if (detail.sequence == IMPORTER_RECEIPT_FIELDS.ReceiptCategoryCode) {
            table = TABLE_INDEX.MASTER_RECEIPT_CATEGORY;
          }
          else if (detail.sequence == IMPORTER_RECEIPT_FIELDS.SectionCode) {
            table = TABLE_INDEX.MASTER_SECTION;
          }
          else if (detail.sequence == IMPORTER_RECEIPT_FIELDS.CurrencyCode) {
            table = TABLE_INDEX.MASTER_CURRENCY;
          }
          else if (detail.sequence == IMPORTER_RECEIPT_FIELDS.AccountTypeId) {
            table = TABLE_INDEX.MASTER_ACCOUNT_TYPE;
          }
          else {
            return;
          }
          break;
        }
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
  public setPatternNo() {

    if (!StringUtil.IsNullOrEmpty(this.patternNoCtrl.value)) {
      this.patternNoCtrl.setValue(StringUtil.setPaddingFrontZero(this.patternNoCtrl.value, 2, true));
      this.loadStart();
      this.importerSettingService.GetHeader(this.importerFileType, this.patternNoCtrl.value)
        .subscribe(response => {
          this.loadEnd();
          //this.ComponentStatus=COMPONENT_STATUS_TYPE.UPDATE;
          if (response != undefined && response.length == 1) {
            this.processModalCustomResult = this.processResultService.processAtSuccess(
              this.processModalCustomResult, MSG_INF.EXIT_PATTERN_NO, this.partsResultMessageComponent);
              
            this.selectedImportSetting = response[0];

            this.ComponentStatus = COMPONENT_STATUS_TYPE.UPDATE;

            this.patternNoCtrl.disable();
            this.patternNameCtrl.setValue(this.selectedImportSetting.name);

            this.GetDetails(this.importerFileType);

          }
          else {
            this.processModalCustomResult = this.processResultService.processAtSuccess(
              this.processModalCustomResult, MSG_INF.NEW_PATTERN_NO, this.partsResultMessageComponent);

            this.patternNameCtrl.enable();
            this.patternNameCtrl.setValue("");

            this.ComponentStatus = COMPONENT_STATUS_TYPE.CREATE;
            this.selectedImportSetting = null;

            this.GetDetails(this.importerFileType);

          }
        });

    }
    else {
      this.patternNoCtrl.setValue("");
      this.patternNameCtrl.setValue("");
      this.importerSettingDetaisResult = null;

    }

  }

  public GetDetails(fileType: number) {
    // インポーター設定詳細情報の取得
    this.importerSettingDetaisResult = new ImporterSettingDetailsResult();

    this.importerSettingService.GetDetail(fileType, this.patternNoCtrl.value)
      .subscribe(response => {
        if (response != undefined && response.length > 0) {
          this.importerSettingDetaisResult.importerSettingDetails = response;

          // 設定情報にフィルターをかける
          let tmpUserInfoService = this.userInfoService;
          let tmpImporterSettingDetails = new Array<ImporterSettingDetail>();
          if (this.importerFileType == FreeImporterFormatType.Billing) {
            this.importerSettingDetaisResult.importerSettingDetails =
              this.importerSettingDetaisResult.importerSettingDetails.filter(
                function (importerSettingDetail: ImporterSettingDetail) {
                  if (importerSettingDetail.targetColumn == "ContractNumber") {
                    if (tmpUserInfoService.ApplicationControl.useLongTermAdvanceReceived) {
                      return true;
                    }
                    else {
                      tmpImporterSettingDetails.push(importerSettingDetail);
                      return false;
                    }
                  }
                  else if (importerSettingDetail.targetColumn == "UseDiscount") {
                    if (tmpUserInfoService.ApplicationControl.useDiscount) {
                      return true;
                    }
                    else {
                      tmpImporterSettingDetails.push(importerSettingDetail);
                      return false;
                    }
                  }
                  else if (importerSettingDetail.targetColumn == "CurrencyCode") {
                    if (tmpUserInfoService.ApplicationControl.useForeignCurrency) {
                      return true;
                    }
                    else {
                      tmpImporterSettingDetails.push(importerSettingDetail);
                      return false;
                    }
                  }
                  else {
                    return true;
                  }
                }
              );
          }
          else if (this.importerFileType == FreeImporterFormatType.Receipt) {
            this.importerSettingDetaisResult.importerSettingDetails =
              this.importerSettingDetaisResult.importerSettingDetails.filter(
                function (importerSettingDetail: ImporterSettingDetail) {
                  if (importerSettingDetail.targetColumn == "SectionCode") {
                    if (tmpUserInfoService.ApplicationControl.useReceiptSection) {
                      return true;
                    }
                    else {
                      tmpImporterSettingDetails.push(importerSettingDetail);
                      return false;
                    }
                  }
                  else if (importerSettingDetail.targetColumn == "CurrencyCode") {
                    if (tmpUserInfoService.ApplicationControl.useForeignCurrency) {
                      return true;
                    }
                    else {
                      tmpImporterSettingDetails.push(importerSettingDetail);
                      return false;
                    }
                  }
                  else {
                    return true;
                  }
                }
              );
          }

          this.hiddenImporterSettingDetails = tmpImporterSettingDetails;

          if (this.selectedImportSetting == null || this.ComponentStatus == COMPONENT_STATUS_TYPE.CREATE) {
            this.startLineCountCtrl.reset();          // 取込開始行
            this.ignoreLastLineCtrl.reset();         // 最終行を取込まない

            if (fileType == FreeImporterFormatType.Billing) {
              this.autoCreationCustomerCtrl.reset();   // 得意先マスター自動作成フラグ
            }
          }
          else {
            this.startLineCountCtrl.setValue(this.selectedImportSetting.startLineCount);          // 取込開始行
            this.ignoreLastLineCtrl.setValue(this.selectedImportSetting.ignoreLastLine);         // 最終行を取込まない

            if (fileType == FreeImporterFormatType.Billing) {
              this.autoCreationCustomerCtrl.setValue(this.selectedImportSetting.autoCreationCustomer);   // 得意先マスター自動作成フラグ
            }
          }

          this.cmbImportDivisionList = new Array<Array<Setting>>();
          this.cmbAttributeList = new Array<Array<Setting>>();

          if (this.importerFileType == FreeImporterFormatType.Billing) {
            this.billingAmountSetting = new Array<Setting>();
            this.taxAmountSetting = new Array<Setting>();
          }

          this.chkDuplicateCtrls = new Array<FormControl>(this.importerSettingDetaisResult.importerSettingDetails.length);
          this.chkDoOverwriteCtrls = new Array<FormControl>(this.importerSettingDetaisResult.importerSettingDetails.length);
          this.cmbImportDivisionCtrls = new Array<FormControl>(this.importerSettingDetaisResult.importerSettingDetails.length);
          this.fixedValueCtrls = new Array<FormControl>(this.importerSettingDetaisResult.importerSettingDetails.length);
          this.fieldIndexCtrls = new Array<FormControl>(this.importerSettingDetaisResult.importerSettingDetails.length);
          this.cmbAttributeCtrls = new Array<FormControl>(this.importerSettingDetaisResult.importerSettingDetails.length);


          for (let index = 0; index < this.importerSettingDetaisResult.importerSettingDetails.length; index++) {

            this.chkDuplicateCtrls[index] = new FormControl("");
            this.chkDoOverwriteCtrls[index] = new FormControl("");
            this.cmbImportDivisionCtrls[index] = new FormControl("");
            this.fixedValueCtrls[index] = new FormControl("");
            this.fieldIndexCtrls[index] = new FormControl("");
            this.cmbAttributeCtrls[index] = new FormControl("");

            this.MyFormGroup.removeControl("chkDuplicateCtrl" + index);
            this.MyFormGroup.removeControl("chkDoOverwriteCtrl" + index);
            this.MyFormGroup.removeControl("cmbImportDivisionCtrl" + index);
            this.MyFormGroup.removeControl("fixedValueCtrl" + index);
            this.MyFormGroup.removeControl("fieldIndexCtrl" + index);
            this.MyFormGroup.removeControl("cmbAttributeCtrl" + index);

            this.MyFormGroup.addControl("chkDuplicateCtrl" + index, this.chkDuplicateCtrls[index]);
            this.MyFormGroup.addControl("chkDoOverwriteCtrl" + index, this.chkDoOverwriteCtrls[index]);
            this.MyFormGroup.addControl("cmbImportDivisionCtrl" + index, this.cmbImportDivisionCtrls[index]);
            this.MyFormGroup.addControl("fixedValueCtrl" + index, this.fixedValueCtrls[index]);
            this.MyFormGroup.addControl("fieldIndexCtrl" + index, this.fieldIndexCtrls[index]);
            this.MyFormGroup.addControl("cmbAttributeCtrl" + index, this.cmbAttributeCtrls[index]);

            let detail = this.importerSettingDetaisResult.importerSettingDetails[index];
            let field = detail.sequence;
            let bPush = true;

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

            if (fileType == FreeImporterFormatType.Billing) {
              if (this.isBillingImportDivisionEnableOrDisable(field)) {
                this.cmbImportDivisionCtrls[index].disable();
              }

              if (field == IMPORTER_BILLING_FIELDS.CustomerName || field == IMPORTER_BILLING_FIELDS.CustomerKana) {
                if (this.autoCreationCustomerCtrl.value == true) {
                  this.cmbImportDivisionList.push(
                    this.settingsResult.settings.filter(
                      function (setting: Setting) {
                        return setting.itemId == "TRKM" + detail.baseImportDivision && setting.itemKey != "0";
                      }
                    )
                  );
                  this.cmbImportDivisionCtrls[index].setValue(detail.importDivision);
                  bPush = false;
                }
                else {
                  this.cmbImportDivisionCtrls[index].disable();
                }
              }
              else if (field == IMPORTER_BILLING_FIELDS.CollationKey) {
                if (this.autoCreationCustomerCtrl.value != true) {
                  this.cmbImportDivisionCtrls[index].disable();
                }
              }
              else if (field == IMPORTER_BILLING_FIELDS.CustomerCode || field == IMPORTER_BILLING_FIELDS.BilledAt) {
                let key = "TRKM" + detail.baseImportDivision;

                this.cmbImportDivisionList.push(
                  this.settingsResult.settings.filter(
                    function (setting: Setting) {
                      return setting.itemId == key;
                    }
                  )
                );

                this.cmbImportDivisionCtrls[index].setValue(1);
                bPush = false;
              }
            }
            else if (fileType == FreeImporterFormatType.Receipt) {
              if (field == IMPORTER_RECEIPT_FIELDS.RecordedAt) {
                let tmp = new Setting();
                tmp.itemId = "";
                tmp.itemKey = "0";
                tmp.itemValue = "1：取込有（必須）";

                let tmpArray = new Array<Setting>();
                tmpArray.push(tmp);
                this.cmbImportDivisionList.push(tmpArray);
                this.cmbImportDivisionCtrls[index].disable();
                this.cmbImportDivisionCtrls[index].setValue(0);
                bPush = false;
              }
              else if (field == IMPORTER_RECEIPT_FIELDS.ReceiptAmount) {
                this.cmbImportDivisionList.push(
                  this.settingsResult.settings.filter(
                    function (setting: Setting) {
                      return setting.itemId == "TRKM" + detail.baseImportDivision && setting.itemKey != "0";
                    }
                  )
                );
                this.cmbImportDivisionCtrls[index].setValue(detail.importDivision == 0 ? 1 : detail.importDivision);
                bPush = false;
              }
            }

            if (bPush) {
              let key = "TRKM" + detail.baseImportDivision;
              let tmpSetting = this.settingsResult.settings.filter(setting => { return setting.itemId == key });

              this.cmbImportDivisionList.push(tmpSetting);
              this.cmbImportDivisionCtrls[index].setValue(detail.importDivision);

              if (fileType == FreeImporterFormatType.Billing) {
                switch (field) {
                  case IMPORTER_BILLING_FIELDS.BillingAmount: this.billingAmountSetting = tmpSetting; break;
                  case IMPORTER_BILLING_FIELDS.TaxAmount: this.taxAmountSetting = tmpSetting; break;
                  default: break;
                }
              }
            }

            let tmpSettingList: Array<Setting> = this.cmbImportDivisionList[index];

            // 表示内容の取得
            let importDivisionDisplayText = "";
            tmpSettingList.forEach(element => {
              if (element.itemKey == this.cmbImportDivisionCtrls[index].value) {
                importDivisionDisplayText = element.itemValue;
              }
            })

            if (importDivisionDisplayText.indexOf("取込有") > 0) {

              if (this.isUniqueDisable(field)) {
                this.chkDuplicateCtrls[index].disable();
                this.chkDuplicateCtrls[index].setValue(false);
              }
              else {
                this.chkDuplicateCtrls[index].enable();
                this.chkDuplicateCtrls[index].setValue(detail.isUnique == 1);
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
                this.cmbAttributeCtrls[index].setValue(detail.attributeDivision);
              }

              if (this.importerFileType == FreeImporterFormatType.Billing) {
                if (this.isOverwriteEnable(field)) {
                  this.chkDoOverwriteCtrls[index].enable();
                  this.chkDoOverwriteCtrls[index].setValue(detail.doOverwrite == 1);
                }
                else {
                  this.chkDoOverwriteCtrls[index].disable();
                  this.chkDoOverwriteCtrls[index].setValue(false);
                }

                if (this.autoCreationCustomerCtrl.value == true
                  && this.isFixedValueEnabledWhenImportDivisionImportOrDisabled(field)) {
                  this.fixedValueCtrls[index].enable();
                  this.fixedValueCtrls[index].setValue(detail.fixedValue);
                }
              }
            }
            else if (importDivisionDisplayText.indexOf("固定値") > 0) {

              this.chkDuplicateCtrls[index].disable();
              this.chkDuplicateCtrls[index].setValue("");

              this.chkDoOverwriteCtrls[index].disable();
              this.chkDoOverwriteCtrls[index].setValue(false);

              this.fixedValueCtrls[index].enable();
              this.fixedValueCtrls[index].setValue(detail.fixedValue);

              this.fieldIndexCtrls[index].disable();
              this.fieldIndexCtrls[index].setValue(null);

              this.cmbAttributeCtrls[index].disable();
            }
            else if (importDivisionDisplayText.indexOf("取込無") > 0) {

              this.chkDuplicateCtrls[index].disable();
              this.chkDuplicateCtrls[index].setValue("");

              this.chkDoOverwriteCtrls[index].disable();
              this.chkDoOverwriteCtrls[index].setValue(false);

              if (this.importerFileType == FreeImporterFormatType.Billing
                && this.autoCreationCustomerCtrl.value == true
                && this.isFixedValueEnabledWhenImportDivisionImportOrDisabled(field)) {
                this.fixedValueCtrls[index].enable();
                this.fixedValueCtrls[index].setValue(detail.fixedValue);
              }
              else {
                this.fixedValueCtrls[index].disable();
                this.fixedValueCtrls[index].setValue(null);
              }

              this.fieldIndexCtrls[index].disable();
              this.fieldIndexCtrls[index].setValue(null);

              this.cmbAttributeCtrls[index].disable();
            }
            else if (this.importerFileType == FreeImporterFormatType.Billing && importDivisionDisplayText.indexOf("空欄") > 0) {
              this.chkDuplicateCtrls[index].enable();
              this.chkDuplicateCtrls[index].setValue(detail.isUnique);

              this.chkDoOverwriteCtrls[index].disable();
              this.chkDoOverwriteCtrls[index].setValue(false);

              this.fieldIndexCtrls[index].enable();
              this.fieldIndexCtrls[index].setValue(detail.fieldIndex);

              this.cmbAttributeCtrls[index].enable();
              this.cmbAttributeCtrls[index].setValue(detail.attributeDivision);

              if (this.autoCreationCustomerCtrl.value == true) {
                this.fixedValueCtrls[index].enable();
                this.fixedValueCtrls[index].setValue(detail.fixedValue);
              }
              else {
                this.fixedValueCtrls[index].disable();
                this.fixedValueCtrls[index].setValue(null);
              }
            }
            else if (this.importerFileType == FreeImporterFormatType.Billing && importDivisionDisplayText.indexOf("得意先") > 0) {
              this.chkDuplicateCtrls[index].disable();
              this.chkDuplicateCtrls[index].setValue(false);

              this.chkDoOverwriteCtrls[index].disable();
              this.chkDoOverwriteCtrls[index].setValue(false);

              this.fixedValueCtrls[index].disable();
              this.fixedValueCtrls[index].setValue(null);

              this.fieldIndexCtrls[index].disable();
              this.fieldIndexCtrls[index].setValue(null);

              this.cmbAttributeCtrls[index].disable();
              this.cmbAttributeCtrls[index].setValue(null);
            }


            // 入力フィールドの入力制限（FormatterとMaxLength)
            this.setDetailFormatter(field, index);

            if (this.isNotesFields(field)) {
              this.columnNameSettingsResult.columnNames.forEach(element => {
                if (element.columnName == detail.targetColumn && !StringUtil.IsNullOrEmpty(element.alias)) {
                  detail.fieldName = element.alias;
                }
              })
            }
          }

          //他項目との連動制御が必要な項目
          if (this.importerFileType == FreeImporterFormatType.Billing) {
            this.setCmbImportDivision('', this.getIndexByField(IMPORTER_BILLING_FIELDS.BillingAmount));
            this.setCmbImportDivision('', this.getIndexByField(IMPORTER_BILLING_FIELDS.TaxAmount));
            this.setCmbImportDivision('', this.getIndexByField(IMPORTER_BILLING_FIELDS.Price));
          }
          else if (this.importerFileType == FreeImporterFormatType.Receipt) {
            this.setCmbImportDivision('', this.getIndexByField(IMPORTER_RECEIPT_FIELDS.CustomerCode));
          }
        }
        else {
          this.importerSettingDetaisResult = null;
        }
      });

    HtmlUtil.nextFocusByName(this.elementRef, 'patternNameCtrl', EVENT_TYPE.NONE);
  }

  ///////////////////////////////////////////////////////////////
  public setPatternName(eventType: string) {
    HtmlUtil.nextFocusByName(this.elementRef, 'startLineCountCtrl', eventType);
  }

  public setCustomerMasterFixedValue(eventType: string) {
    let autoCreation = this.autoCreationCustomerCtrl.value;
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

      if (this.isCustomerMasterFixedValueEnabled(field) && !(importDivisionDisplayText.indexOf("固定値") > 0)) {
        if (autoCreation) {
          this.fixedValueCtrls[index].enable();
        }
        else {
          this.fixedValueCtrls[index].disable();
          this.fixedValueCtrls[index].setValue(null);
        }

      }

      if (!(field == IMPORTER_BILLING_FIELDS.CustomerName || field == IMPORTER_BILLING_FIELDS.CustomerKana || field == IMPORTER_BILLING_FIELDS.CollationKey)) {
        continue;
      }

      if (field == IMPORTER_BILLING_FIELDS.CustomerName || field == IMPORTER_BILLING_FIELDS.CustomerKana) {
        if (autoCreation) {
          if (this.cmbImportDivisionList[index].length > 3) {
            this.cmbImportDivisionList[index].shift();
          }
          this.cmbImportDivisionCtrls[index].setValue("1");
        }
        else {
          if (this.cmbImportDivisionList[index].length == 3) {
            let tmp = this.settingsResult.settings.filter(
              function (setting: Setting) {
                return setting.itemId == "TRKM" + detail.baseImportDivision && setting.itemKey == "0"
              })[0];
            this.cmbImportDivisionList[index].splice(0, 0, tmp);
          }
        }
      }
      else {
        if (!autoCreation) {
          this.chkDuplicateCtrls[index].disable();
          this.chkDuplicateCtrls[index].setValue(false);
        }
      }

      if (autoCreation) {
        this.cmbImportDivisionCtrls[index].enable();
      }
      else {
        this.cmbImportDivisionCtrls[index].disable();
        this.cmbImportDivisionCtrls[index].setValue("0");
        this.fieldIndexCtrls[index].disable();
        this.fieldIndexCtrls[index].setValue(null);
        this.fixedValueCtrls[index].disable();
        this.fixedValueCtrls[index].setValue(null);
        this.cmbAttributeCtrls[index].disable();
        this.cmbAttributeCtrls[index].setValue(null);
      }

    }

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
    else if (detail.sequence == IMPORTER_BILLING_FIELDS.CustomerKana) {
      this.fixedValueCtrls[index].setValue(EbDataHelper.convertToValidEbkana(this.fixedValueCtrls[index].value))
      this.fixedValueCtrls[index].setValue(EbDataHelper.removePersonalities(this.fixedValueCtrls[index].value, this.juridicalPersonalitiesResult.juridicalPersonalities))
    }
    else {
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
      return;
    }
  }

  ///////////////////////////////////////////////////////////////
  public setCmbImportDivision(eventType: string, index: number) {
    let field = this.importerSettingDetaisResult.importerSettingDetails[index].sequence;

    let tmpSettingList: Array<Setting> = this.cmbImportDivisionList[index];

    // 表示内容の取得
    let importDivisionDisplayText = "";
    tmpSettingList.forEach(element => {
      if (element.itemKey == this.cmbImportDivisionCtrls[index].value) {
        importDivisionDisplayText = element.itemValue;
      }
    })

    if (importDivisionDisplayText.indexOf("取込有") > 0) {
      this.enableImportSetting(index, field);
    }
    else if (importDivisionDisplayText.indexOf("固定値") > 0) {
      this.fixedValueCtrls[index].enable();

      this.fieldIndexCtrls[index].disable();
      this.fieldIndexCtrls[index].setValue(null);

      this.cmbAttributeCtrls[index].disable();
      this.cmbAttributeCtrls[index].setValue(null);

      this.chkDuplicateCtrls[index].disable();
      this.chkDuplicateCtrls[index].setValue("");

      this.chkDoOverwriteCtrls[index].disable();
      this.chkDoOverwriteCtrls[index].setValue(false);
    }
    else if (importDivisionDisplayText.indexOf("取込無") > 0) {
      this.disableImportSetting(index, field);
    }
    else if (this.importerFileType == FreeImporterFormatType.Billing && importDivisionDisplayText.indexOf("空欄") > 0) {
      this.chkDuplicateCtrls[index].enable();
      this.fieldIndexCtrls[index].enable();
      this.cmbAttributeCtrls[index].enable();
    }
    else if (this.importerFileType == FreeImporterFormatType.Billing && importDivisionDisplayText.indexOf("得意先") > 0) {
      this.fixedValueCtrls[index].disable();
      this.fixedValueCtrls[index].setValue(null);

      this.fieldIndexCtrls[index].disable();
      this.fieldIndexCtrls[index].setValue(null);

      this.cmbAttributeCtrls[index].disable();
      this.cmbAttributeCtrls[index].setValue(null);
    }
  }

  public enableImportSetting(index: number, field: number) {
    if (this.isUniqueDisable(field)) {
      this.chkDuplicateCtrls[index].disable();
      this.chkDuplicateCtrls[index].setValue(false);
    }
    else {
      this.chkDuplicateCtrls[index].enable();
    }

    this.fieldIndexCtrls[index].enable();

    if (this.isCmbAttributeEnableOrDisable(field)) {
      this.cmbAttributeCtrls[index].disable();
      this.cmbAttributeCtrls[index].setValue(null);
    }
    else {
      this.cmbAttributeCtrls[index].enable();
    }

    if (this.importerFileType == FreeImporterFormatType.Billing) {
      if (this.isOverwriteEnable(field)) {
        this.chkDoOverwriteCtrls[index].enable();
      }
      else {
        this.chkDoOverwriteCtrls[index].disable();
        this.chkDoOverwriteCtrls[index].setValue(false);
      }

      if (this.autoCreationCustomerCtrl.value == true
        && this.isFixedValueEnabledWhenImportDivisionImportOrDisabled(field)) {
        this.fixedValueCtrls[index].enable();
      }
      else {
        this.fixedValueCtrls[index].disable();
        this.fixedValueCtrls[index].setValue(null);
      }

      if (field == IMPORTER_BILLING_FIELDS.Price) {
        let billingAmountIndex = this.getIndexByField(IMPORTER_BILLING_FIELDS.BillingAmount);
        this.cmbImportDivisionList[billingAmountIndex] = this.billingAmountSetting;
        this.chkDuplicateCtrls[billingAmountIndex].disable();
        this.chkDuplicateCtrls[billingAmountIndex].setValue(false);
        this.cmbImportDivisionCtrls[billingAmountIndex].disable();
        this.cmbImportDivisionCtrls[billingAmountIndex].setValue(0);
        this.fieldIndexCtrls[billingAmountIndex].disable();
        this.fieldIndexCtrls[billingAmountIndex].setValue(null);

        let taxRateIndex = this.getIndexByField(IMPORTER_BILLING_FIELDS.TaxRate);
        let taxRateImportDivision = this.cmbImportDivisionCtrls[taxRateIndex].value;

        let taxAmountIndex = this.getIndexByField(IMPORTER_BILLING_FIELDS.TaxAmount);
        this.cmbImportDivisionList[taxAmountIndex] = this.taxAmountSetting.filter(setting => { return setting.itemKey != "0" });
        this.chkDuplicateCtrls[taxAmountIndex].enable();
        this.cmbImportDivisionCtrls[taxAmountIndex].enable();
        this.cmbImportDivisionCtrls[taxAmountIndex].setValue(taxRateImportDivision == 0 ? 2 : 1);
        if (taxRateImportDivision == 0) {
          this.fieldIndexCtrls[taxAmountIndex].enable();
        }
        else {
          this.fieldIndexCtrls[taxAmountIndex].disable();
        }
      }
      else if (field == IMPORTER_BILLING_FIELDS.BillingAmount) {
        let taxAmountIndex = this.getIndexByField(IMPORTER_BILLING_FIELDS.TaxAmount);
        this.cmbImportDivisionCtrls[taxAmountIndex].disable();

        if (this.cmbImportDivisionCtrls[index].value == 3 || this.cmbImportDivisionCtrls[index].value == 4) {
          this.chkDuplicateCtrls[taxAmountIndex].enable();
          this.cmbImportDivisionCtrls[taxAmountIndex].setValue(2);
          this.fieldIndexCtrls[taxAmountIndex].enable();
        }
        else {
          this.chkDuplicateCtrls[taxAmountIndex].disable();
          this.chkDuplicateCtrls[taxAmountIndex].setValue(false);
          this.cmbImportDivisionCtrls[taxAmountIndex].setValue(0);
          this.fieldIndexCtrls[taxAmountIndex].disable();
          this.fieldIndexCtrls[taxAmountIndex].setValue(null);
        }
      }
      else if (field == IMPORTER_BILLING_FIELDS.TaxAmount) {
        let taxRateIndex = this.getIndexByField(IMPORTER_BILLING_FIELDS.TaxRate);
        this.fixedValueCtrls[taxRateIndex].disable();
        this.fixedValueCtrls[taxRateIndex].setValue(null);

        this.fieldIndexCtrls[taxRateIndex].setValue(null);
        this.cmbAttributeCtrls[taxRateIndex].setValue(null);

        if (this.cmbImportDivisionCtrls[index].value == 1) {
          this.cmbImportDivisionCtrls[taxRateIndex].setValue(1);
          this.fieldIndexCtrls[taxRateIndex].enable();
          this.cmbAttributeCtrls[taxRateIndex].enable();
        }
        else {
          this.cmbImportDivisionCtrls[taxRateIndex].setValue(0);
          this.fieldIndexCtrls[taxRateIndex].disable();
          this.cmbAttributeCtrls[taxRateIndex].disable();
        }
      }
    }
    else if (this.importerFileType == FreeImporterFormatType.Receipt) {
      this.chkDoOverwriteCtrls[index].disable();
      this.chkDoOverwriteCtrls[index].setValue(false);

      this.fixedValueCtrls[index].disable();
      this.fixedValueCtrls[index].setValue(null);

      if (field == IMPORTER_RECEIPT_FIELDS.CustomerCode) {
        this.setAllCtrlDisable(this.getIndexByField(IMPORTER_RECEIPT_FIELDS.SourceBankName));
        this.setAllCtrlDisable(this.getIndexByField(IMPORTER_RECEIPT_FIELDS.SourceBranchName));

        this.cmbImportDivisionCtrls[this.getIndexByField(IMPORTER_RECEIPT_FIELDS.PayerName)].enable();
        this.cmbImportDivisionCtrls[this.getIndexByField(IMPORTER_RECEIPT_FIELDS.BillNumber)].enable();
        this.cmbImportDivisionCtrls[this.getIndexByField(IMPORTER_RECEIPT_FIELDS.BillBankCode)].enable();
        this.cmbImportDivisionCtrls[this.getIndexByField(IMPORTER_RECEIPT_FIELDS.BillDrawAt)].enable();
        this.cmbImportDivisionCtrls[this.getIndexByField(IMPORTER_RECEIPT_FIELDS.BillDrawer)].enable();
      }
      else if (field == IMPORTER_RECEIPT_FIELDS.BillBankCode) {
        let billBranchCodeIndex = this.getIndexByField(IMPORTER_RECEIPT_FIELDS.BillBranchCode);

        this.cmbImportDivisionCtrls[billBranchCodeIndex].setValue(1);
        this.fieldIndexCtrls[billBranchCodeIndex].enable();
        this.chkDuplicateCtrls[billBranchCodeIndex].enable();
      }
    }
  }

  public disableImportSetting(index: number, field: number) {
    this.fieldIndexCtrls[index].disable();
    this.fieldIndexCtrls[index].setValue(null);

    this.cmbAttributeCtrls[index].disable();
    this.cmbAttributeCtrls[index].setValue(null);

    this.chkDuplicateCtrls[index].disable();
    this.chkDuplicateCtrls[index].setValue(false);

    this.chkDoOverwriteCtrls[index].disable();
    this.chkDoOverwriteCtrls[index].setValue(false);

    if (this.importerFileType == FreeImporterFormatType.Billing) {
      if (this.autoCreationCustomerCtrl.value == true
        && this.isFixedValueEnabledWhenImportDivisionImportOrDisabled(field)) {
        this.fixedValueCtrls[index].enable();
      }
      else {
        this.fixedValueCtrls[index].disable();
        this.fixedValueCtrls[index].setValue(null);
      }

      if (field == IMPORTER_BILLING_FIELDS.Price) {
        let billingAmountIndex = this.getIndexByField(IMPORTER_BILLING_FIELDS.BillingAmount);
        let billingAmountImportDivision = this.cmbImportDivisionCtrls[billingAmountIndex].value;

        let taxAmountIndex = this.getIndexByField(IMPORTER_BILLING_FIELDS.TaxAmount);
        let taxAmountImportDivision = this.cmbImportDivisionCtrls[taxAmountIndex].value;

        this.cmbImportDivisionList[billingAmountIndex] = this.billingAmountSetting.filter(setting => { return setting.itemKey != "0" });
        this.cmbImportDivisionList[taxAmountIndex] = this.taxAmountSetting;

        //請求金額の元の取込区分が 0：取込無 ならChangeイベント
        if (billingAmountImportDivision == 0) {
          billingAmountImportDivision = 1;
          taxAmountImportDivision = 0;

          let taxRateIndex = this.getIndexByField(IMPORTER_BILLING_FIELDS.TaxRate);
          this.cmbImportDivisionCtrls[taxRateIndex].setValue(0);
          this.fieldIndexCtrls[taxRateIndex].disable();
          this.fieldIndexCtrls[taxRateIndex].setValue(null);
          this.cmbAttributeCtrls[taxRateIndex].disable();
          this.cmbAttributeCtrls[taxRateIndex].setValue(null);
        }

        this.chkDuplicateCtrls[billingAmountIndex].enable();
        this.cmbImportDivisionCtrls[billingAmountIndex].enable();
        this.cmbImportDivisionCtrls[billingAmountIndex].setValue(billingAmountImportDivision);
        this.fieldIndexCtrls[billingAmountIndex].enable();

        this.cmbImportDivisionCtrls[taxAmountIndex].disable();
        this.cmbImportDivisionCtrls[taxAmountIndex].setValue(taxAmountImportDivision);

        if (billingAmountImportDivision == 3 || billingAmountImportDivision == 4) {
          this.chkDuplicateCtrls[taxAmountIndex].enable();
          this.fieldIndexCtrls[taxAmountIndex].enable();
        }
        else {
          this.chkDuplicateCtrls[taxAmountIndex].disable();
          this.chkDuplicateCtrls[taxAmountIndex].setValue(false);
          this.fieldIndexCtrls[taxAmountIndex].disable();
          this.fieldIndexCtrls[taxAmountIndex].setValue(null);
        }
      }
      else if (field == IMPORTER_BILLING_FIELDS.TaxAmount) {
        let taxRateIndex = this.getIndexByField(IMPORTER_BILLING_FIELDS.TaxRate);
        if (this.cmbImportDivisionCtrls[index].value == 1) {
          this.cmbImportDivisionCtrls[taxRateIndex].setValue(1);
          this.fieldIndexCtrls[taxRateIndex].enable();
          this.cmbAttributeCtrls[taxRateIndex].enable();
        }
        else {
          this.cmbImportDivisionCtrls[taxRateIndex].setValue(0);
          this.fieldIndexCtrls[taxRateIndex].disable();
          this.cmbAttributeCtrls[taxRateIndex].disable();
        }
      }
      else if (field == IMPORTER_BILLING_FIELDS.CustomerName || field == IMPORTER_BILLING_FIELDS.CustomerKana) {
        this.cmbImportDivisionCtrls[index].disable();
      }
      else if (field == IMPORTER_BILLING_FIELDS.CollationKey && this.autoCreationCustomerCtrl.value == false) {
        this.cmbImportDivisionCtrls[index].disable();
      }
    }
    else if (this.importerFileType == FreeImporterFormatType.Receipt) {
      this.fixedValueCtrls[index].disable();
      this.fixedValueCtrls[index].setValue(null);

      if (field == IMPORTER_RECEIPT_FIELDS.CustomerCode) {
        let payerNameIndex = this.getIndexByField(IMPORTER_RECEIPT_FIELDS.PayerName);

        this.chkDuplicateCtrls[payerNameIndex].enable();
        this.cmbImportDivisionCtrls[payerNameIndex].disable();
        this.cmbImportDivisionCtrls[payerNameIndex].setValue(1);
        this.fixedValueCtrls[payerNameIndex].disable();
        this.fieldIndexCtrls[payerNameIndex].enable();
        this.cmbAttributeCtrls[payerNameIndex].enable();

        this.cmbImportDivisionCtrls[this.getIndexByField(IMPORTER_RECEIPT_FIELDS.SourceBankName)].enable();
        this.cmbImportDivisionCtrls[this.getIndexByField(IMPORTER_RECEIPT_FIELDS.SourceBranchName)].enable();

        this.setAllCtrlDisable(this.getIndexByField(IMPORTER_RECEIPT_FIELDS.BillNumber));
        this.setAllCtrlDisable(this.getIndexByField(IMPORTER_RECEIPT_FIELDS.BillBankCode));
        this.setAllCtrlDisable(this.getIndexByField(IMPORTER_RECEIPT_FIELDS.BillBranchCode));
        this.setAllCtrlDisable(this.getIndexByField(IMPORTER_RECEIPT_FIELDS.BillDrawAt));
        this.setAllCtrlDisable(this.getIndexByField(IMPORTER_RECEIPT_FIELDS.BillDrawer));
      }
      else if (field == IMPORTER_RECEIPT_FIELDS.BillBankCode) {
        let billBranchCodeIndex = this.getIndexByField(IMPORTER_RECEIPT_FIELDS.BillBranchCode);

        this.cmbImportDivisionCtrls[billBranchCodeIndex].setValue(0);
        this.fieldIndexCtrls[billBranchCodeIndex].disable();
        this.fieldIndexCtrls[billBranchCodeIndex].setValue(null);
        this.chkDuplicateCtrls[billBranchCodeIndex].disable();
        this.chkDuplicateCtrls[billBranchCodeIndex].setValue(false);
      }
    }
  }

  ///////////////////////////////////////////////////////////////
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
      || field == IMPORTER_BILLING_FIELDS.CurrencyCode;

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

  public isFixedValueEnabledWhenImportDivisionImportOrDisabled(field: number): boolean {
    let bResult =
      field == IMPORTER_BILLING_FIELDS.DueAt
      || field == IMPORTER_BILLING_FIELDS.ClosingAt
      || field == IMPORTER_BILLING_FIELDS.CollectCategoryCode
      || field == IMPORTER_BILLING_FIELDS.StaffCode;

    return bResult;
  }

  public getIndexByField(field: number): number {
    return this.importerSettingDetaisResult.importerSettingDetails.findIndex((setting) => {
      return (setting.sequence === field);
    });
  }

  public setAllCtrlDisable(index: number): void {
    this.chkDuplicateCtrls[index].disable();
    this.chkDuplicateCtrls[index].setValue(false);
    this.cmbImportDivisionCtrls[index].disable();
    this.cmbImportDivisionCtrls[index].setValue(0);
    this.fixedValueCtrls[index].disable();
    this.fixedValueCtrls[index].setValue(null);
    this.fieldIndexCtrls[index].disable();
    this.fieldIndexCtrls[index].setValue(null);
    this.cmbAttributeCtrls[index].disable();
    this.cmbAttributeCtrls[index].setValue(null);
  }

  public setPlaceHolder(index: number): string {

    if (this.importerSettingDetaisResult == undefined) return "";

    let result: string;
    let detail = this.importerSettingDetaisResult.importerSettingDetails[index];

    switch (this.importerFileType) {

      case FreeImporterFormatType.Billing:
        if (detail.sequence == IMPORTER_BILLING_FIELDS.DepartmentCode
          || detail.sequence == IMPORTER_BILLING_FIELDS.DebitAccountTitleCode
          || detail.sequence == IMPORTER_BILLING_FIELDS.StaffCode
          || detail.sequence == IMPORTER_BILLING_FIELDS.BillingCategoryCode
          || detail.sequence == IMPORTER_BILLING_FIELDS.CollectCategoryCode
          || detail.sequence == IMPORTER_BILLING_FIELDS.UseDiscount
          || detail.sequence == IMPORTER_BILLING_FIELDS.CurrencyCode) {
          result = this.PlaceHolderConst.F9_SEARCH;
        }
        break;
      case FreeImporterFormatType.Receipt:
        if (detail.sequence == IMPORTER_RECEIPT_FIELDS.ReceiptCategoryCode
          || detail.sequence == IMPORTER_RECEIPT_FIELDS.SectionCode
          || detail.sequence == IMPORTER_RECEIPT_FIELDS.CurrencyCode
          || detail.sequence == IMPORTER_RECEIPT_FIELDS.AccountTypeId) {
          result = this.PlaceHolderConst.F9_SEARCH;
        }
        break;
    }

    return result;
  }

}
