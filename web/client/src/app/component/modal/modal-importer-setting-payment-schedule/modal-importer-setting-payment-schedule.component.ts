import { Component, OnInit, EventEmitter, ViewContainerRef, ComponentFactoryResolver, ElementRef } from '@angular/core';
import { BaseComponent } from '../../common/base/base-component';
import { Validators, FormControl, FormGroup } from '@angular/forms';
import { CustomValidators } from 'ng5-validation';
import { ModalMasterComponent } from '../modal-master/modal-master.component';
import { ImporterSettingDetailsResult } from 'src/app/model/importer-setting-details-result.model';
import { StringUtil } from 'src/app/common/util/string-util';
import { TABLE_INDEX } from 'src/app/common/const/table-name.const';
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
import { IMPORTER_PAYMENT_SCHEDULE_FIELDS } from 'src/app/common/const/importer.const';
import { CategoryType } from 'src/app/common/const/kbn.const';
import { HtmlUtil } from 'src/app/common/util/html.util';
import { ProcessResultService } from 'src/app/service/common/process-result.service';
import { MSG_WNG, MSG_ITEM_NUM, MSG_INF } from 'src/app/common/const/message.const';
import { ModalRouterProgressComponent } from '../modal-router-progress/modal-router-progress.component';

@Component({
  selector: 'app-modal-importer-setting-payment-schedule',
  templateUrl: './modal-importer-setting-payment-schedule.component.html',
  styleUrls: ['./modal-importer-setting-payment-schedule.component.css']
})
export class ModalImporterSettingPaymentScheduleComponent extends BaseComponent implements OnInit {

  public FreeImporterFormatType: typeof FreeImporterFormatType = FreeImporterFormatType;

  public importerSettingId: number;

  public settingsResult: SettingsResult;
  public columnNameSettingsResult: ColumnNameSettingsResult
  public importerFileType: number;

  public cmbAttributeList: Array<Array<Setting>>;
  public billingAmountSetting: Array<Setting>;
  public taxAmountSetting: Array<Setting>;

  public chkKeyCtrls: Array<FormControl>;
  public fieldIndexCtrls: Array<FormControl>;
  public fieldCaptionCtrls: Array<FormControl>;
  public fieldIndexPriorityCtrls: Array<FormControl>;
  public cmbSortOrderCtrls: Array<FormControl>;
  public cmbAttributeCtrls: Array<FormControl>;

  public selectedImportSetting: ImporterSetting;
  public importerSettingDetaisResult: ImporterSettingDetailsResult = null;
  public hiddenImporterSettingDetails: Array<ImporterSettingDetail>;

  public patternNoCtrl: FormControl;              // パターンNo
  public patternNameCtrl: FormControl;            // パターン名
  public startLineCountCtrl: FormControl;         // 取込開始行	
  public ignoreLastLineCtrl: FormControl;         // 最終行を取込まない	
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

  public sortOrders = [
    { id: 0, orderName: '昇順' },
    { id: 1, orderName: '降順' },
  ];

  constructor(
    public elementRef: ElementRef,
    public componentFactoryResolver: ComponentFactoryResolver,
    public viewContainerRef: ViewContainerRef,
    public importerSettingService: ImporterSettingService,
    public userInfoService: UserInfoService,
    public settingService: SettingMasterService,
    public columnNameSettingService: ColumnNameSettingMasterService,
    public processResultService: ProcessResultService,
  ) {
    super();
  }

  ngOnInit() {
    this.setControlInit();
    this.setValidator();
    this.setFormatter();
    this.clear();

    let settingResponse = this.settingService.GetItems();

    this.importerFileType = FreeImporterFormatType.PaymentSchedule;
    let columnNameSetting = this.columnNameSettingService.Get(CategoryType.Billing);

    forkJoin(
      settingResponse,
      columnNameSetting,
    )
      .subscribe(responseList => {

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
    this.UndefineCtrl = new FormControl();

  }

  public setValidator() {
    this.MyFormGroup = new FormGroup({
      patternNoCtrl: this.patternNoCtrl,
      patternNameCtrl: this.patternNameCtrl,
      ignoreLastLineCtrl: this.ignoreLastLineCtrl,
      startLineCountCtrl: this.startLineCountCtrl,
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

    this.fieldCaptionCtrls[index].setValidators([Validators.maxLength(50)]);

    this.fieldIndexPriorityCtrls[index].setValidators([Validators.maxLength(3)]);
    FormatterUtil.setNumberFormatter(this.fieldIndexPriorityCtrls[index]);

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
    if (!this.isValidInputForPaymentScheduleImporter()) {
      return;
    }

    const nowDateTime = new Date().toLocaleString();
    let importerSetting = new ImporterSetting();
    importerSetting.id = this.selectedImportSetting == null ? 0 : this.selectedImportSetting.id;
    importerSetting.companyId = this.userInfoService.Company.id,
      importerSetting.code = this.patternNoCtrl.value;
    importerSetting.name = this.patternNameCtrl.value;
    importerSetting.formatId = this.importerFileType;
    importerSetting.initialDirectory = "";
    importerSetting.startLineCount = NumberUtil.ParseInt(this.startLineCountCtrl.value);
    importerSetting.ignoreLastLine = this.ignoreLastLineCtrl.value == true ? 1 : 0;
    importerSetting.autoCreationCustomer = 0;
    importerSetting.updateBy = this.userInfoService.LoginUser.id;
    importerSetting.createBy = this.userInfoService.LoginUser.id;
    importerSetting.postAction = 0;
    importerSetting.details = new Array<ImporterSettingDetail>();

    // 登録処理
    for (var index = 0; index < this.importerSettingDetaisResult.importerSettingDetails.length; index++) {
      const field = this.importerSettingDetaisResult.importerSettingDetails[index].sequence;
      let importerSettingDetail = new ImporterSettingDetail();

      importerSettingDetail.sequence = field;
      importerSettingDetail.isUnique = 0;
      importerSettingDetail.doOverwrite = 0;
      importerSettingDetail.fixedValue = "";

      importerSettingDetail.importerSettingId = importerSetting.id;
      importerSettingDetail.updateBy = this.userInfoService.LoginUser.id;
      importerSettingDetail.createBy = this.userInfoService.LoginUser.id;
      importerSettingDetail.importDivision = this.chkKeyCtrls[index].value ? 1 : 0;

      if (importerSettingDetail.importDivision == 1) {
        importerSettingDetail.fieldIndex = NumberUtil.ParseInt(this.fieldIndexCtrls[index].value);
      }
      else if (importerSettingDetail.importDivision == 0
        && (field == IMPORTER_PAYMENT_SCHEDULE_FIELDS.ReceiptAmount || field == IMPORTER_PAYMENT_SCHEDULE_FIELDS.ScheduledPaymentKey)) {
        importerSettingDetail.fieldIndex = NumberUtil.ParseInt(this.fieldIndexCtrls[index].value);
      }
      else {
        importerSettingDetail.fieldIndex = NumberUtil.ParseInt(this.fieldIndexPriorityCtrls[index].value);
      }

      importerSettingDetail.caption = StringUtil.IsNullOrEmpty(this.fieldCaptionCtrls[index].value) ? "" : this.fieldCaptionCtrls[index].value;
      importerSettingDetail.itemPriority = StringUtil.IsNullOrEmpty(this.cmbSortOrderCtrls[index].value) ? 0 : this.cmbSortOrderCtrls[index].value;
      importerSettingDetail.attributeDivision = StringUtil.IsNullOrEmpty(this.cmbAttributeCtrls[index].value) ? 0 : NumberUtil.ParseInt(this.cmbAttributeCtrls[index].value);

      importerSetting.details.push(importerSettingDetail);

    }

    //退避した非表示項目を戻す
    importerSetting.details.concat(this.hiddenImporterSettingDetails);

    let processComponentFactory = this.componentFactoryResolver.resolveComponentFactory(ModalRouterProgressComponent);
    let processComponentRef = this.viewContainerRef.createComponent(processComponentFactory);

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

  public isValidInputForPaymentScheduleImporter(): boolean {

    let bCheckKey = false;
    for (let index = 0; index < this.importerSettingDetaisResult.importerSettingDetails.length; index++) {
      if (this.chkKeyCtrls[index].disabled) continue;
      if (this.chkKeyCtrls[index].value) {
        bCheckKey = true;
        break;
      }
    }
    if (!bCheckKey) {
      this.processModalCustomResult = this.processResultService.processAtWarning(
        this.processModalCustomResult, MSG_WNG.SELECTION_REQUIRED.replace(MSG_ITEM_NUM.FIRST, '取込キー'),
        this.partsResultMessageComponent);
      return false;
    }

    for (let index = 0; index < this.importerSettingDetaisResult.importerSettingDetails.length; index++) {

      let detail = this.importerSettingDetaisResult.importerSettingDetails[index];
      let field = detail.sequence;

      if (field == IMPORTER_PAYMENT_SCHEDULE_FIELDS.ReceiptAmount && StringUtil.IsNullOrEmpty(this.fieldIndexCtrls[index].value)) {
        this.processModalCustomResult = this.processResultService.processAtWarning(
          this.processModalCustomResult, MSG_WNG.INPUT_REQUIRED.replace(MSG_ITEM_NUM.FIRST, '項目番号'),
          this.partsResultMessageComponent);
        HtmlUtil.nextFocusByName(this.elementRef, 'fieldIndexCtrl' + index, EVENT_TYPE.NONE);
        return false;
      }

      if (this.chkKeyCtrls[index].value && StringUtil.IsNullOrEmpty(this.fieldIndexCtrls[index].value)) {
        this.processModalCustomResult = this.processResultService.processAtWarning(
          this.processModalCustomResult, MSG_WNG.INPUT_REQUIRED.replace(MSG_ITEM_NUM.FIRST, '項目番号'),
          this.partsResultMessageComponent);
        HtmlUtil.nextFocusByName(this.elementRef, 'fieldIndexCtrl' + index, EVENT_TYPE.NONE);
        return false;
      }
      else if ((!StringUtil.IsNullOrEmpty(this.fieldIndexPriorityCtrls[index].value)) && StringUtil.IsNullOrEmpty(this.cmbSortOrderCtrls[index].value)) {
        this.processModalCustomResult = this.processResultService.processAtWarning(
          this.processModalCustomResult, MSG_WNG.SELECTION_REQUIRED.replace(MSG_ITEM_NUM.FIRST, '項目内'),
          this.partsResultMessageComponent);
        HtmlUtil.nextFocusByName(this.elementRef, 'cmbSortOrderCtrl' + index, EVENT_TYPE.NONE);
        return false;
      }
      else if (this.cmbAttributeCtrls[index].enabled && (!StringUtil.IsNullOrEmpty(this.fieldIndexCtrls[index].value))
        && (StringUtil.IsNullOrEmpty(this.cmbAttributeCtrls[index].value) || this.cmbAttributeCtrls[index].value == "0")) {
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
    componentRef.instance.TableIndex = TABLE_INDEX.MASTER_SCHEDULE_PAYMENT_IMPORTER_SETTING;

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
    this.processModalCustomResult = this.processResultService.clearProcessCustomMsg(this.processModalCustomResult);
    this.closing.emit({});
  }

  ///////////////////////////////////////////////////////////////
  public openMasterModal() {
    let componentFactory = this.componentFactoryResolver.resolveComponentFactory(ModalMasterComponent);
    let componentRef = this.viewContainerRef.createComponent(componentFactory);
    componentRef.instance.zIndex = componentRef.instance.zIndexDefSize * 2;
    componentRef.instance.TableIndex = TABLE_INDEX.MASTER_SCHEDULE_PAYMENT_IMPORTER_SETTING;

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

  ///////////////////////////////////////////////////////////////
  public setPatternNo() {

    if (!StringUtil.IsNullOrEmpty(this.patternNoCtrl.value)) {
      this.patternNoCtrl.setValue(StringUtil.setPaddingFrontZero(this.patternNoCtrl.value, 2, true));
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

          this.importerSettingDetaisResult.importerSettingDetails =
            this.importerSettingDetaisResult.importerSettingDetails.filter(
              function (importerSettingDetail: ImporterSettingDetail) {
                if (importerSettingDetail.targetColumn == "CurrencyCode") {
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

          this.hiddenImporterSettingDetails = tmpImporterSettingDetails;

          if (this.selectedImportSetting == null || this.ComponentStatus == COMPONENT_STATUS_TYPE.CREATE) {
            this.startLineCountCtrl.reset();          // 取込開始行
            this.ignoreLastLineCtrl.reset();         // 最終行を取込まない

          }
          else {
            this.startLineCountCtrl.setValue(this.selectedImportSetting.startLineCount);          // 取込開始行
            this.ignoreLastLineCtrl.setValue(this.selectedImportSetting.ignoreLastLine);         // 最終行を取込まない

          }

          this.cmbAttributeList = new Array<Array<Setting>>();
          this.chkKeyCtrls = new Array<FormControl>(this.importerSettingDetaisResult.importerSettingDetails.length);
          this.fieldIndexCtrls = new Array<FormControl>(this.importerSettingDetaisResult.importerSettingDetails.length);
          this.fieldCaptionCtrls = new Array<FormControl>(this.importerSettingDetaisResult.importerSettingDetails.length);
          this.fieldIndexPriorityCtrls = new Array<FormControl>(this.importerSettingDetaisResult.importerSettingDetails.length);
          this.cmbSortOrderCtrls = new Array<FormControl>(this.importerSettingDetaisResult.importerSettingDetails.length);
          this.cmbAttributeCtrls = new Array<FormControl>(this.importerSettingDetaisResult.importerSettingDetails.length);

          for (let index = 0; index < this.importerSettingDetaisResult.importerSettingDetails.length; index++) {

            this.chkKeyCtrls[index] = new FormControl("");
            this.fieldIndexCtrls[index] = new FormControl("");
            this.fieldCaptionCtrls[index] = new FormControl("");
            this.fieldIndexPriorityCtrls[index] = new FormControl("");
            this.cmbSortOrderCtrls[index] = new FormControl("");
            this.cmbAttributeCtrls[index] = new FormControl("");

            this.MyFormGroup.removeControl("chkKeyCtrl" + index);
            this.MyFormGroup.removeControl("fieldIndexCtrl" + index);
            this.MyFormGroup.removeControl("fieldCaptionCtrl" + index);
            this.MyFormGroup.removeControl("fieldIndexPriorityCtrl" + index);
            this.MyFormGroup.removeControl("cmbSortOrderCtrl" + index);
            this.MyFormGroup.removeControl("cmbAttributeCtrl" + index);

            this.MyFormGroup.addControl("chkKeyCtrl" + index, this.chkKeyCtrls[index]);
            this.MyFormGroup.addControl("fieldIndexCtrl" + index, this.fieldIndexCtrls[index]);
            this.MyFormGroup.addControl("fieldCaptionCtrl" + index, this.fieldCaptionCtrls[index]);
            this.MyFormGroup.addControl("fieldIndexPriorityCtrl" + index, this.fieldIndexPriorityCtrls[index]);
            this.MyFormGroup.addControl("cmbSortOrderCtrl" + index, this.cmbSortOrderCtrls[index]);
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

            this.chkKeyCtrls[index].setValue(detail.importDivision);
            this.fieldCaptionCtrls[index].setValue(detail.caption);

            if (this.isImportDivisionEnabled(field)) {
              this.chkKeyCtrls[index].enable();
            }
            else {
              this.chkKeyCtrls[index].disable();
            }

            const fieldIndexEnabled = this.isFieldIndexEnabled(detail);
            if (fieldIndexEnabled) {
              this.fieldIndexCtrls[index].enable();
              this.fieldCaptionCtrls[index].enable();
            }
            else {
              this.fieldIndexCtrls[index].disable();
              this.fieldCaptionCtrls[index].disable();
            }

            if (fieldIndexEnabled && detail.fieldIndex > 0) {
              this.fieldIndexCtrls[index].setValue(detail.fieldIndex);
            }

            const priorityEnabled = this.isItemPriorityEnabled(detail);
            if (priorityEnabled) {
              this.fieldIndexPriorityCtrls[index].enable();
              this.cmbSortOrderCtrls[index].enable();
            }
            else {
              this.fieldIndexPriorityCtrls[index].disable();
              this.cmbSortOrderCtrls[index].disable();
            }

            if (priorityEnabled && detail.fieldIndex > 0) {
              this.fieldIndexPriorityCtrls[index].setValue(detail.fieldIndex);
              this.cmbSortOrderCtrls[index].setValue(detail.itemPriority);
            }

            if (this.isAttributeEnabled(detail)) {
              this.cmbAttributeCtrls[index].enable();
              this.cmbAttributeCtrls[index].setValue(detail.attributeDivision);
            }
            else {
              this.cmbAttributeCtrls[index].disable();
            }

            if (this.isNotesFields(field)) {
              this.columnNameSettingsResult.columnNames.forEach(element => {
                if (element.columnName == detail.targetColumn && !StringUtil.IsNullOrEmpty(element.alias)) {
                  detail.fieldName = element.alias;
                }
              })
            }
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

  ///////////////////////////////////////////////////////////////
  public isNotesFields(field: number): boolean {
    return field == IMPORTER_PAYMENT_SCHEDULE_FIELDS.Note1
      || field == IMPORTER_PAYMENT_SCHEDULE_FIELDS.Note2
      || field == IMPORTER_PAYMENT_SCHEDULE_FIELDS.Note3
      || field == IMPORTER_PAYMENT_SCHEDULE_FIELDS.Note4
      || field == IMPORTER_PAYMENT_SCHEDULE_FIELDS.Note5
      || field == IMPORTER_PAYMENT_SCHEDULE_FIELDS.Note6
      || field == IMPORTER_PAYMENT_SCHEDULE_FIELDS.Note7
      || field == IMPORTER_PAYMENT_SCHEDULE_FIELDS.Note8;
  }

  ///////////////////////////////////////////////////////////////
  public setChkKey(index: number) {
    if (this.chkKeyCtrls[index].value) {
      this.fieldIndexCtrls[index].enable();
      this.fieldCaptionCtrls[index].enable();

      const detail = this.importerSettingDetaisResult.importerSettingDetails[index];
      if (this.isAttributeEnabledInner(detail.sequence)) {
        this.cmbAttributeCtrls[index].enable();
      }

      this.fieldIndexPriorityCtrls[index].reset();
      this.cmbSortOrderCtrls[index].reset();
      this.fieldIndexPriorityCtrls[index].disable();
      this.cmbSortOrderCtrls[index].disable();
    }
    else {

      this.fieldIndexCtrls[index].reset();
      this.fieldCaptionCtrls[index].reset();
      this.cmbAttributeCtrls[index].reset();
      this.fieldIndexCtrls[index].disable();
      this.fieldCaptionCtrls[index].disable();
      this.cmbAttributeCtrls[index].disable();

      this.fieldIndexPriorityCtrls[index].enable();
      this.cmbSortOrderCtrls[index].enable();
    }
  }

  public isImportRequired(field: number): boolean {
    return field == IMPORTER_PAYMENT_SCHEDULE_FIELDS.ReceiptAmount ||
      field == IMPORTER_PAYMENT_SCHEDULE_FIELDS.ScheduledPaymentKey;
  }

  public isImportIgnore(field: number): boolean {
    return field == IMPORTER_PAYMENT_SCHEDULE_FIELDS.CompanyCode;
  }

  public isCollationIgnore(field: number): boolean {
    return field == IMPORTER_PAYMENT_SCHEDULE_FIELDS.TaxAmount ||
      field == IMPORTER_PAYMENT_SCHEDULE_FIELDS.StaffCode;
  }

  /// <summary>
  /// キー チェックボックスの有効無効
  /// </summary>
  public isImportDivisionEnabled(field: number): boolean {
    return !(this.isImportRequired(field) ||
      this.isImportIgnore(field) ||
      this.isCollationIgnore(field));
  }

  /// <summary>
  /// 項目番号 有効無効
  /// </summary>
  public isFieldIndexEnabledInner(field: number): boolean {
    return !(this.isImportIgnore(field) ||
      this.isCollationIgnore(field));
  }

  /// <summary>
  /// 項目番号 有効無効 項目名（注釈）も連動
  /// </summary>
  public isFieldIndexEnabled(detail: ImporterSettingDetail): boolean {
    return this.isFieldIndexEnabledInner(detail.sequence) &&
      (this.isImportRequired(detail.sequence) || detail.importDivision == 1);
  }

  /// <summary>
  /// 優先順位 項目間 有効無効 項目内 も連動
  /// </summary>
  public isItemPriorityEnabled(detail: ImporterSettingDetail): boolean {
    return !this.isImportRequired(detail.sequence) &&
      !this.isImportIgnore(detail.sequence) &&
      detail.importDivision == 0;
  }

  public isDateAttribute(field: number): boolean {
    return field == IMPORTER_PAYMENT_SCHEDULE_FIELDS.BilledAt ||
      field == IMPORTER_PAYMENT_SCHEDULE_FIELDS.DueAt ||
      field == IMPORTER_PAYMENT_SCHEDULE_FIELDS.SalesAt ||
      field == IMPORTER_PAYMENT_SCHEDULE_FIELDS.ClosingAt;
  }

  public isStringAttribute(field: number): boolean {
    return field == IMPORTER_PAYMENT_SCHEDULE_FIELDS.InvoiceCode ||
      field == IMPORTER_PAYMENT_SCHEDULE_FIELDS.Note1 ||
      field == IMPORTER_PAYMENT_SCHEDULE_FIELDS.Note2 ||
      field == IMPORTER_PAYMENT_SCHEDULE_FIELDS.Note3 ||
      field == IMPORTER_PAYMENT_SCHEDULE_FIELDS.Note4 ||
      field == IMPORTER_PAYMENT_SCHEDULE_FIELDS.Note5 ||
      field == IMPORTER_PAYMENT_SCHEDULE_FIELDS.Note6 ||
      field == IMPORTER_PAYMENT_SCHEDULE_FIELDS.Note7 ||
      field == IMPORTER_PAYMENT_SCHEDULE_FIELDS.Note8 ||
      field == IMPORTER_PAYMENT_SCHEDULE_FIELDS.ScheduledPaymentKey;
  }

  public isExternalCodeAttribute(field: number): boolean {
    return field == IMPORTER_PAYMENT_SCHEDULE_FIELDS.BillingCategoryCode;
  }

  /// <summary>
  /// 属性区分 有効項目
  /// </summary>  
  public isAttributeEnabledInner(field: number): boolean {
    return this.isDateAttribute(field) ||
      this.isStringAttribute(field) ||
      this.isExternalCodeAttribute(field);
  }

  public isAttributeEnabled(detail: ImporterSettingDetail): boolean {
    return this.isAttributeEnabledInner(detail.sequence) &&
      (this.isImportRequired(detail.sequence) || detail.importDivision == 1);
  }

}
