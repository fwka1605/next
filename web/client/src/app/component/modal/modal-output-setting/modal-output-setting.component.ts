import { Component, OnInit, ComponentFactoryResolver, ViewContainerRef, EventEmitter } from '@angular/core';
import { FormBuilder, FormGroup, FormArray } from '@angular/forms';
import { CdkDragDrop, moveItemInArray } from '@angular/cdk/drag-drop';
import { BaseComponent } from 'src/app/component/common/base/base-component';
import { ProcessResultService } from 'src/app/service/common/process-result.service';
import { BUTTON_ACTION } from 'src/app/common/const/event.const';
import { PROCESS_RESULT_RESULT_TYPE } from 'src/app/common/const/status.const';
import { MODAL_STATUS_TYPE } from 'src/app/common/const/status.const';
import { CategoryType, CsvExportFileType } from 'src/app/common/const/kbn.const';
import { MSG_WNG } from 'src/app/common/const/message.const';
import { ColumnNameSettingMasterService } from 'src/app/service/Master/column-name-setting-master.service';
import { ColumnNameSetting } from 'src/app/model/column-name-setting.model';
import { ExportFieldSettingMasterService } from 'src/app/service/Master/export-field-setting-master.service';
import { ExportFieldSetting } from 'src/app/model/export-field-setting.model';
import { ModalRouterProgressComponent } from 'src/app/component/modal/modal-router-progress/modal-router-progress.component';
import { UserInfoService } from 'src/app/service/common/user-info.service';
import { forkJoin } from 'rxjs';
import { StringUtil } from 'src/app/common/util/string-util';



@Component({
  selector: 'app-modal-output-setting',
  templateUrl: './modal-output-setting.component.html',
  styleUrls: ['./modal-output-setting.component.css']
})
export class ModalOutputSettingComponent extends BaseComponent implements OnInit {

  public exportFileType: CsvExportFileType;

  public closing = new EventEmitter<{}>();

  public exportFieldSettings: Array<ExportFieldSetting>;
  public exportFieldSettingForms: FormArray;

  public billingColumnName: Array<ColumnNameSetting>;
  public receiptColumnName: Array<ColumnNameSetting>;

  constructor(
    public processResultService: ProcessResultService,
    public componentFactoryResolver: ComponentFactoryResolver,
    public formBuilder: FormBuilder,
    public viewContainerRef: ViewContainerRef,
    public userInfoService: UserInfoService,
    public columnNameSettingMasterService: ColumnNameSettingMasterService,
    public exportFieldSettingMasterService: ExportFieldSettingMasterService,
  ) {
    super();
  }

  public dateFormatItems = [
    { id: 0, itemName: 'yyyy/MM/dd'},
    { id: 1, itemName: 'yy/MM/dd' },
    { id: 2, itemName: 'yyyyMMdd'},
    { id: 3, itemName: 'yyMMdd'},
  ];


  ngOnInit() {
    this.search();
  }

  public get Closing() {
    return this.closing;
  }
  
  public set Closing(value) {
    this.closing = value;
  }

  public get ExportFileType(): CsvExportFileType {
    return this.exportFileType;
  }

  public set ExportFileType(value: CsvExportFileType) {
    this.exportFileType = value;    
  }

  public search() {

    let componentFactory  = this.componentFactoryResolver.resolveComponentFactory(ModalRouterProgressComponent);
    let componentRef      = this.viewContainerRef.createComponent(componentFactory);

    let setting = new ExportFieldSetting();
    setting.companyId       = this.userInfoService.Company.id;
    setting.exportFileType  = this.exportFileType;

    let billingColumnNameResponse   = this.columnNameSettingMasterService.Get(CategoryType.Billing);
    let receiptColumnNameResponse   = this.columnNameSettingMasterService.Get(CategoryType.Receipt);
    let exportFieldSettingResponse  = this.exportFieldSettingMasterService.GetItems(setting);

    forkJoin(
      billingColumnNameResponse,
      receiptColumnNameResponse,
      exportFieldSettingResponse,
    )
      .subscribe(responseList => {
        if ( responseList.length != 3
            || responseList[0] != PROCESS_RESULT_RESULT_TYPE.FAILURE
            || responseList[1] != PROCESS_RESULT_RESULT_TYPE.FAILURE
            || responseList[2] != PROCESS_RESULT_RESULT_TYPE.FAILURE
            )
        {
          this.billingColumnName = new Array<ColumnNameSetting>();
          this.billingColumnName = responseList[0];

          this.receiptColumnName = new Array<ColumnNameSetting>();
          this.receiptColumnName = responseList[1];

          this.exportFieldSettings = new Array<ExportFieldSetting>();
          this.exportFieldSettings = responseList[2];

          const headerSetting = this.exportFieldSettings.find(x => x.columnName === 'RequireHeader');
          const requireHeader = headerSetting == undefined ? true : headerSetting.allowExport === 1;

          const billedAtSetting = this.exportFieldSettings.find(x => x.columnName === 'BilledAt');
          const dateType = billedAtSetting == undefined ? 0 : billedAtSetting.dataFormat;

          this.exportFieldSettings = this.exportFieldSettings.filter(x => x.columnOrder != 0);          

          if (this.userInfoService.ApplicationControl.useForeignCurrency == 0) {
            this.exportFieldSettings = this.exportFieldSettings.filter(x => x.columnName != "CurrencyCode");
          }

          if (this.userInfoService.ApplicationControl.useReceiptSection == 0) {
            this.exportFieldSettings = this.exportFieldSettings.filter(x => (x.columnName != "SectionCode") && (x.columnName != "SectionName") );
          }

          this.initFormGroup(requireHeader, dateType);

        }
        componentRef.destroy();
      });

  }

  public initFormGroup(requireHeader: boolean, dateType: number) {
    if (this.exportFieldSettingForms == undefined) {      
      this.exportFieldSettingForms = this.formBuilder.array([]);
    }

    this.resetFormArray(this.exportFieldSettingForms);
    this.exportFieldSettings.map(x => this.exportFieldSettingForms.push(this.getExportFielsSettingFormGroup(x)));

    this.MyFormGroup = this.formBuilder.group({
      //  日付の書式
      dateFormatType:           [dateType],
      //  項目名を出力する
      requireHeader:            [requireHeader],
      //  項目明細
      exportFieldSettingForms:  this.exportFieldSettingForms,
    });

  }

  public getExportFielsSettingFormGroup(setting: ExportFieldSetting): FormGroup {
    return this.formBuilder.group({
      columnName:   [setting.columnName],
      columnOrder:  [setting.columnOrder],
      allowExport:  [setting.allowExport === 1],
      caption:      [this.getCaption(setting)],
    });
  }

  public getCaption(setting: ExportFieldSetting): string {

    if (this.exportFileType == CsvExportFileType.MatchedReceiptData) {
      return this.getMatchedReceiptCaption(setting);
    }
    else {
      return this.getPublishInvoiceCaption(setting);
    }

  }

  public getMatchedReceiptCaption(setting: ExportFieldSetting): string {

    let columnNameSetting: ColumnNameSetting;
    switch (setting.columnName) {
      case 'BillingNote1':
        columnNameSetting = this.billingColumnName.find(x => x.columnName == 'Note1');
        setting.caption   = StringUtil.IsNullOrEmpty(columnNameSetting.alias) ? setting.caption : columnNameSetting.alias;
        break;
      case 'BillingNote2':
        columnNameSetting = this.billingColumnName.find(x => x.columnName == 'Note2');
        setting.caption   = StringUtil.IsNullOrEmpty(columnNameSetting.alias) ? setting.caption : columnNameSetting.alias;
        break;
      case 'BillingNote3':
        columnNameSetting = this.billingColumnName.find(x => x.columnName == 'Note3');
        setting.caption   = StringUtil.IsNullOrEmpty(columnNameSetting.alias) ? setting.caption : columnNameSetting.alias;
        break;
      case 'BillingNote4':
        columnNameSetting = this.billingColumnName.find(x => x.columnName == 'Note4');
        setting.caption   = StringUtil.IsNullOrEmpty(columnNameSetting.alias) ? setting.caption : columnNameSetting.alias;
        break;
      case 'ReceiptNote1':
        columnNameSetting = this.receiptColumnName.find(x => x.columnName == 'Note1');
        setting.caption   = StringUtil.IsNullOrEmpty(columnNameSetting.alias) ? setting.caption : columnNameSetting.alias;
        break;
      case 'ReceiptNote2':
        columnNameSetting = this.receiptColumnName.find(x => x.columnName == 'Note2');
        setting.caption   = StringUtil.IsNullOrEmpty(columnNameSetting.alias) ? setting.caption : columnNameSetting.alias;
        break;
      case 'ReceiptNote3':
        columnNameSetting = this.receiptColumnName.find(x => x.columnName == 'Note3');
        setting.caption   = StringUtil.IsNullOrEmpty(columnNameSetting.alias) ? setting.caption : columnNameSetting.alias;
        break;
      case 'ReceiptNote4':
        columnNameSetting = this.receiptColumnName.find(x => x.columnName == 'Note4');
        setting.caption   = StringUtil.IsNullOrEmpty(columnNameSetting.alias) ? setting.caption : columnNameSetting.alias;
        break;
    }

    return setting.caption;
  }

  public getPublishInvoiceCaption(setting: ExportFieldSetting): string {

    let columnNameSetting: ColumnNameSetting;
    switch (setting.columnName) {
      case 'Note1':
        columnNameSetting = this.billingColumnName.find(x => x.columnName == 'Note1');
        setting.caption   = StringUtil.IsNullOrEmpty(columnNameSetting.alias) ? setting.caption : columnNameSetting.alias;
        break;
      case 'Note2':
        columnNameSetting = this.billingColumnName.find(x => x.columnName == 'Note2');
        setting.caption   = StringUtil.IsNullOrEmpty(columnNameSetting.alias) ? setting.caption : columnNameSetting.alias;
        break;
      case 'Note3':
        columnNameSetting = this.billingColumnName.find(x => x.columnName == 'Note3');
        setting.caption   = StringUtil.IsNullOrEmpty(columnNameSetting.alias) ? setting.caption : columnNameSetting.alias;
        break;
      case 'Note4':
        columnNameSetting = this.billingColumnName.find(x => x.columnName == 'Note4');
        setting.caption   = StringUtil.IsNullOrEmpty(columnNameSetting.alias) ? setting.caption : columnNameSetting.alias;
        break;
      case 'Note5':
        columnNameSetting = this.billingColumnName.find(x => x.columnName == 'Note5');
        setting.caption   = StringUtil.IsNullOrEmpty(columnNameSetting.alias) ? setting.caption : columnNameSetting.alias;
        break;
      case 'Note6':
        columnNameSetting = this.billingColumnName.find(x => x.columnName == 'Note6');
        setting.caption   = StringUtil.IsNullOrEmpty(columnNameSetting.alias) ? setting.caption : columnNameSetting.alias;
        break;
      case 'Note7':
        columnNameSetting = this.billingColumnName.find(x => x.columnName == 'Note7');
        setting.caption   = StringUtil.IsNullOrEmpty(columnNameSetting.alias) ? setting.caption : columnNameSetting.alias;
        break;
      case 'Note8':
        columnNameSetting = this.billingColumnName.find(x => x.columnName == 'Note8');
        setting.caption   = StringUtil.IsNullOrEmpty(columnNameSetting.alias) ? setting.caption : columnNameSetting.alias;
        break;
    }

    return setting.caption;
  }

  public resetFormArray(array: FormArray) {
    for (let i = array.length - 1; i >= 0; i--) {
      array.removeAt(i);
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
      case BUTTON_ACTION.REDISPLAY:
        this.reDisplay();
        break;

      default:
        console.log('buttonAction Error.');
        break;
    }
  }

  public registry() {
    if (!this.validateInput()) {
      this.processModalCustomResult = this.processResultService.processAtWarning(
        this.processModalCustomResult, MSG_WNG.SELECT_ANY_OUTPUT_ITEM, this.partsResultMessageComponent);      
      return;
    }

    let componentFactory = this.componentFactoryResolver.resolveComponentFactory(ModalRouterProgressComponent);
    let componentRef = this.viewContainerRef.createComponent(componentFactory);
    componentRef.instance.Closing.subscribe(() => {
      componentRef.destroy();
    });
    
    const saveSource = this.getSetting();
    this.exportFieldSettingMasterService.Save(saveSource)
    .subscribe(response => {
      this.processModalCustomResult = this.processResultService.processAtSave(
        this.processModalCustomResult, response, true, this.partsResultMessageComponent);
      if ( response != PROCESS_RESULT_RESULT_TYPE.FAILURE ) {
        this.search();
      }
      componentRef.destroy();      
    });

  }

  public validateInput(): boolean {

    for (let group of this.exportFieldSettingForms.controls as FormGroup[]) {
      const allowExport = group.get('allowExport').value;
      if (allowExport) return true;
    }    
    return false;
  }

  public getSetting(): Array<ExportFieldSetting> {

    const settings = new Array<ExportFieldSetting>();
    let columnOrder = 0;

    settings.push(this.getRequiredHeaderSetting());

    for (let group of this.exportFieldSettingForms.controls as FormGroup[]) {

      const allowExport = group.get('allowExport').value;
      const columnName  = group.get('columnName').value;
      const setting = new ExportFieldSetting();      
      setting.companyId       = this.userInfoService.Company.id;
      setting.columnName      = columnName;
      setting.columnOrder     = ++columnOrder;
      setting.allowExport     = allowExport ? 1 : 0;
      setting.exportFileType  = this.exportFileType;
      setting.updateBy        = this.userInfoService.LoginUser.id;
      setting.dataFormat      = this.getDataFormat(columnName);

      settings.push(setting);
    }

    return settings;
  }

  public getRequiredHeaderSetting(): ExportFieldSetting {
    const setting = new ExportFieldSetting();
    setting.companyId       = this.userInfoService.Company.id;
    setting.columnName      = 'RequireHeader';
    setting.columnOrder     = 0;
    setting.allowExport     = this.MyFormGroup.get('requireHeader').value ? 1 : 0;
    setting.exportFileType  = this.exportFileType;
    setting.updateBy        = this.userInfoService.LoginUser.id;
    setting.dataFormat      = 0;

    return setting;
  }

  public getDataFormat(columnName: string): number {

    switch (this.exportFileType) {
      case CsvExportFileType.MatchedReceiptData:
          if (columnName == 'BilledAt' || columnName == 'RecordedAt' || columnName == 'DueAt' || columnName == 'BillDrawAt') {
            return this.MyFormGroup.get('dateFormatType').value;
          }
      case CsvExportFileType.PublishInvoiceData:
          if (columnName == 'BilledAt'  || columnName == 'ClosingAt' || columnName == 'SalesAt'
           || columnName == 'DueAt'     || columnName == 'PublishAt' || columnName == 'PublishAt1st')
          {
            return this.MyFormGroup.get('dateFormatType').value;
          }
    }

    return 0;
  }


  public reDisplay() {
    this.search();
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

  public onDropExportField(event: CdkDragDrop<string[]>) {
    this.onDropInner(event, this.exportFieldSettingForms);
    this.applyExportFieldSettingModels();
  }
  
  public onDropInner(event: CdkDragDrop<string[]>, orders: FormArray) {
    moveItemInArray(orders.controls,  event.previousIndex, event.currentIndex);
    moveItemInArray(orders.value,     event.previousIndex, event.currentIndex);
  }
  
  public applyExportFieldSettingModels() {
    let order = 0;
    const orders = this.exportFieldSettings;
    for (let group of this.exportFieldSettingForms.controls as FormGroup[]) {
      const columnName  = group.get('columnName').value;
      const allowExport = group.get('allowExport').value;
      const model       = orders.filter(x => x.columnName === columnName)[0];
      model.columnOrder = ++order;
      model.allowExport = allowExport ? 1 : 0;
      model.updateBy    = this.userInfoService.LoginUser.id;
    }
    this.exportFieldSettings = orders.sort((x, y) => x.columnOrder - y.columnOrder);
  }

  public getTitle(): string {
    return this.exportFileType == CsvExportFileType.MatchedReceiptData ? "消込済み入金データ出力の設定"
                                                                        : "請求書発行データ出力の設定";
  }

}
