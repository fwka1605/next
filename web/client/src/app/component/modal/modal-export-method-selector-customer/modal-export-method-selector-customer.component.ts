import { Component, OnInit, ComponentFactoryResolver, ViewContainerRef, EventEmitter, ElementRef } from '@angular/core';
import { BaseComponent } from '../../common/base/base-component';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { SHARE_TRANSFER_FEE_DICTIONARY, CODE_TYPE } from 'src/app/common/const/kbn.const';
import { TABLE_INDEX } from 'src/app/common/const/table-name.const';
import { KEY_CODE, BUTTON_ACTION } from 'src/app/common/const/event.const';
import { ModalMasterComponent } from '../modal-master/modal-master.component';
import { MODAL_STATUS_TYPE, PROCESS_RESULT_RESULT_TYPE, PROCESS_RESULT_STATUS_TYPE } from 'src/app/common/const/status.const';
import { CustomerSearch } from 'src/app/model/customer-search.model';
import { UserInfoService } from 'src/app/service/common/user-info.service';
import { CustomerMasterService } from 'src/app/service/Master/customer-master.service';
import { Customer } from 'src/app/model/customer.model';
import { Staff } from 'src/app/model/staff.model';
import { StaffMasterService } from 'src/app/service/Master/staff-master.service';
import { FormatterUtil } from 'src/app/common/util/formatter.util';
import { NgbDateParserFormatter } from '@ng-bootstrap/ng-bootstrap';
import { NgbDateCustomParserFormatter } from 'src/app/common/util/dateformat-util';
import { FileUtil } from 'src/app/common/util/file.util';
import { DateUtil } from 'src/app/common/util/date-util';
import { FILE_EXTENSION } from 'src/app/common/const/eb-file.const';
import { CustomerFeeSearch } from 'src/app/model/customer-fee-search.model';
import { REPORT_HEADER, LINE_FEED_CODE } from 'src/app/common/const/report-setting-kbn.const';
import { CustomerFeeMasterService } from 'src/app/service/Master/customer-fee-master.service';
import { MSG_WNG } from 'src/app/common/const/message.const';
import { ProcessResultService } from 'src/app/service/common/process-result.service';
import { StringUtil } from 'src/app/common/util/string-util';
import { HtmlUtil } from 'src/app/common/util/html.util';
import { LocalStorageItem } from 'src/app/model/custom-model/local-storage-item';
import { RangeSearchKey } from 'src/app/common/const/local-storage-key-const';
import { LocalStorageManageService } from 'src/app/service/common/local-storage-manage.service';
import { ModalRouterProgressComponent } from '../modal-router-progress/modal-router-progress.component';

@Component({
  selector: 'app-modal-export-method-selector-customer',
  templateUrl: './modal-export-method-selector-customer.component.html',
  styleUrls: ['./modal-export-method-selector-customer.component.css'],
  providers: [
    { provide: NgbDateParserFormatter, useClass: NgbDateCustomParserFormatter }
  ]
})
export class ModalExportMethodSelectorCustomerComponent extends BaseComponent implements OnInit {
  /** 手数料区分 */
  public readonly shareTransferFeeDictionary = SHARE_TRANSFER_FEE_DICTIONARY;

  /** モーダルタイトル名 */
  public outputTitle: string;
  /** 出力対象 */
  public rdoOutputTargetCtrl: FormControl;
  /** 得意先コード(From) */
  public customerCodeFromCtrl: FormControl;
  /** 得意先コード(To) */
  public customerCodeToCtrl: FormControl;
  /** 得意先名(From) */
  public customerNameFromCtrl: FormControl;
  /** 得意先名(To) */
  public customerNameToCtrl: FormControl;
  /** 得意先コードのチェック */
  public cbxCustomerCtrl: FormControl;
  /** 締日 */
  public closingDateCtrl: FormControl;
  /** 手数料負担区分 */
  public cmbShareTransferFeeCtrl: FormControl;
  /** 営業担当者コード(From) */
  public staffCodeFromCtrl: FormControl;
  /** 営業担当者コード(To) */
  public staffCodeToCtrl: FormControl;
  /** 営業担当者名(From) */
  public staffNameFromCtrl: FormControl;
  /** 営業担当者名(To) */
  public staffNameToCtrl: FormControl;
  /** 営業担当者コードのチェック */
  public cbxStaffCtrl: FormControl;
  /** 最終更新日(From) */
  public updateDatefromCtrl: FormControl;
  /** 最終更新日(To) */
  public updateDateToCtrl: FormControl;
  /** 得意先ID(From) */
  public customerCodeFromId: number;
  /** 得意先ID(To) */
  public customerCodeToId: number;
  /** 得意先IDデータ */
  public customers: Array<Customer> = new Array<Customer>();
  /** 営業担当者ID(From) */
  public staffCodeFromId: number;
  /** 営業担当者ID(To) */
  public staffCodeToId: number;
  /** 営業担当者データ */
  public staffs: Array<Staff>;


  constructor(
    public componentFactoryResolver: ComponentFactoryResolver,
    public viewContainerRef: ViewContainerRef,
    public userInfoService: UserInfoService,
    public customerService: CustomerMasterService,
    public staffService: StaffMasterService,
    public customerFeeService: CustomerFeeMasterService,
    public processResultService: ProcessResultService,
    public elementRef: ElementRef,
    public localStorageManageService: LocalStorageManageService

  ) {
    super();
    this.Title = this.Title + "印刷の設定";
  }

  ngOnInit() {
    this.setControlInit();
    this.setValidator();
    this.clear();
  }

  public setControlInit() {
    this.rdoOutputTargetCtrl = new FormControl("");
    this.customerCodeFromCtrl = new FormControl("", [Validators.maxLength(this.userInfoService.ApplicationControl.customerCodeLength)]);
    this.customerCodeToCtrl = new FormControl("", [Validators.maxLength(this.userInfoService.ApplicationControl.customerCodeLength)]);
    this.customerNameFromCtrl = new FormControl("");
    this.customerNameToCtrl = new FormControl("");
    this.closingDateCtrl = new FormControl("");
    this.cmbShareTransferFeeCtrl = new FormControl("");
    this.staffCodeFromCtrl = new FormControl("", [Validators.maxLength(this.userInfoService.ApplicationControl.staffCodeLength)]);
    this.staffCodeToCtrl = new FormControl("", [Validators.maxLength(this.userInfoService.ApplicationControl.staffCodeLength)]);
    this.staffNameFromCtrl = new FormControl("");
    this.staffNameToCtrl = new FormControl("");
    this.updateDatefromCtrl = new FormControl("");
    this.updateDateToCtrl = new FormControl("");
    this.cbxCustomerCtrl = new FormControl("");
    this.cbxStaffCtrl = new FormControl("");
  }

  public setValidator() {
    this.MyFormGroup = new FormGroup({
      rdoOutputTargetCtrl: this.rdoOutputTargetCtrl,
      customerCodeFromCtrl: this.customerCodeFromCtrl,
      customerCodeToCtrl: this.customerCodeToCtrl,
      customerNameFromCtrl: this.customerNameFromCtrl,
      customerNameToCtrl: this.customerNameToCtrl,
      closingDateCtrl: this.closingDateCtrl,
      cmbShareTransferFeeCtrl: this.cmbShareTransferFeeCtrl,
      staffCodeFromCtrl: this.staffCodeFromCtrl,
      staffCodeToCtrl: this.staffCodeToCtrl,
      staffNameFromCtrl: this.staffNameFromCtrl,
      staffNameToCtrl: this.staffNameToCtrl,
      updateDatefromCtrl: this.updateDatefromCtrl,
      updateDateToCtrl: this.updateDateToCtrl,
      cbxCustomerCtrl: this.cbxCustomerCtrl,
      cbxStaffCtrl: this.cbxStaffCtrl
    })
  }

  public setFormatter() {

    if (this.userInfoService.ApplicationControl.customerCodeType == CODE_TYPE.NUMBER) {
      FormatterUtil.setNumberFormatter(this.customerCodeFromCtrl); // 得意先コード
      FormatterUtil.setNumberFormatter(this.customerCodeToCtrl);
    }
    else if (this.userInfoService.ApplicationControl.customerCodeType == CODE_TYPE.HANKANA) {
      FormatterUtil.setCustomerCodeKanaFormatter(this.customerCodeFromCtrl);
      FormatterUtil.setCustomerCodeKanaFormatter(this.customerCodeToCtrl);
    }
    else {
      FormatterUtil.setCustomerCodeAlphaFormatter(this.customerCodeFromCtrl);
      FormatterUtil.setCustomerCodeAlphaFormatter(this.customerCodeToCtrl);
    }

    if (this.userInfoService.ApplicationControl.staffCodeType == CODE_TYPE.NUMBER) {
      FormatterUtil.setNumberFormatter(this.staffCodeFromCtrl); // 担当者コード
      FormatterUtil.setNumberFormatter(this.staffCodeToCtrl);
    }
    else {
      FormatterUtil.setCodeFormatter(this.staffCodeFromCtrl);
      FormatterUtil.setCodeFormatter(this.staffCodeToCtrl);
    }

  }

  public clear() {
    this.MyFormGroup.reset();
    this.MyFormGroup.enable();
    this.customerCodeFromId = 0;
    this.customerCodeToId = 0;

    this.setRangeCheckbox();

    this.rdoOutputTargetCtrl.setValue('1');
  }

  public closing = new EventEmitter<{}>();
  public get Closing() {
    return this.closing;
  }
  public set Closing(value) {
    this.closing = value;
  }

  public close() {
    this.ModalStatus = MODAL_STATUS_TYPE.CLOSE;
    this.processModalCustomResult = this.processResultService.clearProcessCustomMsg(this.processModalCustomResult);
    this.closing.emit({});
  }

  public cancel() {
    this.ModalStatus = MODAL_STATUS_TYPE.CLOSE;
    this.processModalCustomResult = this.processResultService.clearProcessCustomMsg(this.processModalCustomResult);
    this.closing.emit({});
  }

  public get OutputTitle(): string {
    return this.outputTitle;
  }
  public set OutputTitle(value: string) {
    this.outputTitle = value;
  }

  public get ProcessModalCustomResult() {
    return this.processModalCustomResult;
  }
  public set ProcessModalCustomResult(value) {
    this.processModalCustomResult = value;
  }

  /**
   * 各テーブルからデータを取得
   * @param table 対象テーブル
   * @param keyCode イベント
   * @param type フォーム種別
   */
  public openMasterModal(table: TABLE_INDEX, type: string = null) {
    let componentFactory = this.componentFactoryResolver.resolveComponentFactory(ModalMasterComponent);
    let componentRef = this.viewContainerRef.createComponent(componentFactory);
    componentRef.instance.TableIndex = table;
    componentRef.instance.zIndex = componentRef.instance.zIndexDefSize * 2;

    componentRef.instance.Closing.subscribe(() => {

      if (componentRef.instance.ModalStatus == MODAL_STATUS_TYPE.SELECT) {
        switch (table) {

          case TABLE_INDEX.MASTER_CUSTOMER:
            {
              if (type === "from") {
                this.customerCodeFromId = componentRef.instance.SelectedObject.id
                this.customerCodeFromCtrl.setValue(componentRef.instance.SelectedCode);
                this.customerNameFromCtrl.setValue(componentRef.instance.SelectedName);

                if (this.cbxCustomerCtrl.value) {
                  this.customerCodeToCtrl.setValue(componentRef.instance.SelectedCode);
                  this.customerNameToCtrl.setValue(componentRef.instance.SelectedName);
                }
              }
              else if (type === "to") {
                this.customerCodeToId = componentRef.instance.SelectedObject.id
                this.customerCodeToCtrl.setValue(componentRef.instance.SelectedCode);
                this.customerNameToCtrl.setValue(componentRef.instance.SelectedName);
              }

              this.getCustomerCodes();
              break;
            }
          case TABLE_INDEX.MASTER_STAFF:
            {
              if (type === "from") {
                this.staffCodeFromId = componentRef.instance.SelectedObject.id
                this.staffCodeFromCtrl.setValue(componentRef.instance.SelectedCode);
                this.staffNameFromCtrl.setValue(componentRef.instance.SelectedName);

                if (this.cbxStaffCtrl.value) {
                  this.staffCodeToCtrl.setValue(componentRef.instance.SelectedCode);
                  this.staffNameToCtrl.setValue(componentRef.instance.SelectedName);
                }

              }
              else if (type === "to") {
                this.staffCodeToId = componentRef.instance.SelectedObject.id
                this.staffCodeToCtrl.setValue(componentRef.instance.SelectedCode);
                this.staffNameToCtrl.setValue(componentRef.instance.SelectedName);
              }
              this.getStaffCodes();
              break;
            }
        }
      }
      componentRef.destroy();
    });
  }


  /**
   * 検索項目の表示切替
   */
  public selectOutputType() {
    let rdoValue = this.rdoOutputTargetCtrl.value;
    this.clear();
    if (rdoValue == '3') {
      this.MyFormGroup.disable();
      this.rdoOutputTargetCtrl.enable();
    } else {
      this.MyFormGroup.enable();
    }
    this.rdoOutputTargetCtrl.setValue(rdoValue);
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
      case BUTTON_ACTION.PRINT:
        this.outputValue();
        break;

      default:
        console.log('buttonAction Error.');
        break;
    }
  }

  /**
   * 印刷・エクスポート
   */
  public outputValue() {

    let processComponentFactory = this.componentFactoryResolver.resolveComponentFactory(ModalRouterProgressComponent);
    let processComponentRef = this.viewContainerRef.createComponent(processComponentFactory);
    // processComponentRef.destroy();    


    let search = new CustomerSearch();
    let customerIds = new Array<number>();
    let result: boolean = false;

    // 得意先コードのID取得
    if (0 < this.customerCodeFromId) {
      let formIndex = this.customers.findIndex((item) => {
        return (item.id === this.customerCodeFromId);
      });

      if (0 < this.customerCodeToId) {
        let toIndex = this.customers.findIndex((item) => {
          return (item.id === this.customerCodeToId);
        });
        let addCustomers = this.customers.slice(formIndex, toIndex + 1);
        for (let i = 0; i < addCustomers.length; i++) {
          customerIds.push(addCustomers[i].id);
        }

      } else {
        customerIds.push(this.customers[formIndex].id);
      }
      search.ids = customerIds;
    }

    search.closingDay = this.closingDateCtrl.value;
    search.shareTransferFee = this.cmbShareTransferFeeCtrl.value;
    if (this.staffCodeFromCtrl.value != null) {
      search.staffCodeFrom = this.staffCodeFromCtrl.value;
      if (this.staffCodeToCtrl.value == null) {
        search.staffCodeTo = search.staffCodeFrom
      } else {
        search.staffCodeTo = this.staffCodeToCtrl.value;
      }
    }
    search.updateAtFrom = DateUtil.ConvertFromDatepicker(this.updateDatefromCtrl);
    search.updateAtTo = DateUtil.ConvertFromDatepicker(this.updateDateToCtrl);

    if (this.outputTitle == '印刷') {
      switch (this.rdoOutputTargetCtrl.value) {
        case '1':
          this.customerService.GetReport(search)
            .subscribe(response => {
              try {
                FileUtil.download([response.body], '得意先マスター一覧' + DateUtil.getYYYYMMDD(0), FILE_EXTENSION.PDF);
                result = true;

              } catch (error) {
                console.error(error);
              }
              this.processResultService.processAtOutput(
                this.processModalCustomResult, result, 1, this.partsResultMessageComponent);
              if (this.processModalCustomResult.result == PROCESS_RESULT_RESULT_TYPE.SUCCESS) {
                // this.close();
              }
              processComponentRef.destroy();
            });
          break;
        case '2':
          this.customerService.GetRegisterReport(search)
            .subscribe(response => {
              try {
                FileUtil.download([response.body], '得意先台帳' + DateUtil.getYYYYMMDD(0), FILE_EXTENSION.PDF);
                result = true;

              } catch (error) {
                console.error(error);
              }
              this.processResultService.processAtOutput(
                this.processModalCustomResult, result, 1, this.partsResultMessageComponent);
              if (this.processModalCustomResult.result == PROCESS_RESULT_RESULT_TYPE.SUCCESS) {
                // this.close();
              }
              processComponentRef.destroy();
            });
          break;
        case '3':
          this.customerService.GetFeeReport(new CustomerFeeSearch())
            .subscribe(response => {
              try {
                FileUtil.download([response.body], '登録手数料一覧' + DateUtil.getYYYYMMDD(0), FILE_EXTENSION.PDF);
                result = true;

              } catch (error) {
                console.error(error);
              }
              this.processResultService.processAtOutput(
                this.processModalCustomResult, result, 1, this.partsResultMessageComponent);
              if (this.processModalCustomResult.result == PROCESS_RESULT_RESULT_TYPE.SUCCESS) {
                // this.close();
              }
              processComponentRef.destroy();
            });
          break;
      }

    } else {
      if (this.rdoOutputTargetCtrl.value == '1') {
        this.customerService.GetItemsByCustomerSearch(search)
          .subscribe(response => {
            let dataList = response;
            let headers = FileUtil.encloseItemBySymbol(REPORT_HEADER.CUSTOMER_MASTER);
            let data: string = headers.join(",") + LINE_FEED_CODE;

            // 件数チェック
            if (dataList.length <= 0) {
              this.processCustomResult = this.processResultService.processAtWarning(
                this.processCustomResult, MSG_WNG.NO_EXPORT_DATA, this.partsResultMessageComponent);
              return;
            }

            // CSV作成
            data += this.createCsv(dataList);

            let resultDatas: Array<any> = [];
            resultDatas.push(data);
            try {
              FileUtil.download(resultDatas, '得意先マスター一覧' + DateUtil.getYYYYMMDD(0), FILE_EXTENSION.CSV);
              result = true;

            } catch (error) {
              console.error(error);
            }
            this.processResultService.processAtOutput(
              this.processModalCustomResult, result, 0, this.partsResultMessageComponent);
            if (this.processModalCustomResult.result == PROCESS_RESULT_RESULT_TYPE.SUCCESS) {
              // this.close();
            }
            processComponentRef.destroy();
          });

      } else {
        this.customerFeeService.Get(new CustomerFeeSearch())
          .subscribe(response => {
            let dataList = response;
            let headers = FileUtil.encloseItemBySymbol(REPORT_HEADER.CUSTOMER_MASTER_FEE);
            let data: string = headers.join(",") + LINE_FEED_CODE;

            // 件数チェック
            if (dataList.length <= 0) {
              this.processCustomResult = this.processResultService.processAtWarning(
                this.processCustomResult, MSG_WNG.NO_EXPORT_DATA, this.partsResultMessageComponent);
              return;
            }

            // CSV作成
            for (let index = 0; index < dataList.length; index++) {
              let dataItem: Array<any> = [];
              dataItem.push(this.userInfoService.Company.code);
              dataItem.push(dataList[index].customerCode);
              dataItem.push(dataList[index].fee);
              dataItem = FileUtil.encloseItemBySymbol(dataItem);

              data += dataItem.join(",") + LINE_FEED_CODE;

            }

            let resultDatas: Array<any> = [];
            resultDatas.push(data);
            try {
              FileUtil.download(resultDatas, '登録手数料一覧' + DateUtil.getYYYYMMDD(0), FILE_EXTENSION.CSV);
              result = true;

            } catch (error) {
              console.error(error);
            }
            this.processResultService.processAtOutput(
              this.processModalCustomResult, result, 0, this.partsResultMessageComponent);
            if (this.processModalCustomResult.result == PROCESS_RESULT_RESULT_TYPE.SUCCESS) {
              // this.close();
            }
            processComponentRef.destroy();
          });
      }

    }
  }

  /**
   * CSV作成
   * @param dataList 出力するデータ
   */
  public createCsv(dataList: Array<any>): string {
    let result = '';

    for (let index = 0; index < dataList.length; index++) {
      let dataItem: Array<any> = [];
      dataItem.push(this.userInfoService.Company.code);
      dataItem.push(dataList[index].code);
      dataItem.push(dataList[index].name);
      dataItem.push(dataList[index].kana);
      dataItem.push(dataList[index].exclusiveBankCode);
      dataItem.push(dataList[index].exclusiveBankName);
      dataItem.push(dataList[index].exclusiveBranchCode);
      dataItem.push(dataList[index].exclusiveBranchName);
      dataItem.push(dataList[index].exclusiveAccountNumber);
      dataItem.push(dataList[index].exclusiveAccountTypeId);
      dataItem.push(dataList[index].shareTransferFee);
      dataItem.push(dataList[index].creditLimit);
      dataItem.push(dataList[index].closingDay);
      dataItem.push(dataList[index].collectCategoryCode);
      dataItem.push(dataList[index].collectOffsetMonth);
      dataItem.push(dataList[index].collectOffsetDay);
      dataItem.push(dataList[index].staffCode);
      dataItem.push(dataList[index].isParent);
      dataItem.push(dataList[index].postalCode);
      dataItem.push(dataList[index].address1);
      dataItem.push(dataList[index].address2);
      dataItem.push(dataList[index].tel);
      dataItem.push(dataList[index].fax);
      dataItem.push(dataList[index].customerStaffName);
      dataItem.push(dataList[index].note);
      dataItem.push(dataList[index].useFeeLearning);
      dataItem.push(dataList[index].sightOfBill);
      dataItem.push(dataList[index].densaiCode);
      dataItem.push(dataList[index].creditCode);
      dataItem.push(dataList[index].creditRank);
      dataItem.push(dataList[index].transferBankCode);
      dataItem.push(dataList[index].transferBankName);
      dataItem.push(dataList[index].transferBranchCode);
      dataItem.push(dataList[index].transferBranchName);
      dataItem.push(dataList[index].transferAccountNumber);
      dataItem.push(dataList[index].transferAccountTypeId);
      dataItem.push(dataList[index].transferCustomerCode);
      dataItem.push(dataList[index].transferNewCode);
      dataItem.push(dataList[index].transferAccountName);
      dataItem.push(dataList[index].thresholdValue);
      dataItem.push(dataList[index].lessThanCollectCategoryId);
      dataItem.push(dataList[index].greaterThanCollectCategoryId1);
      dataItem.push(dataList[index].greaterThanRate1);
      dataItem.push(dataList[index].greaterThanRoundingMode1);
      dataItem.push(dataList[index].greaterThanSightOfBill1);
      dataItem.push(dataList[index].greaterThanCollectCategoryId2);
      dataItem.push(dataList[index].greaterThanRate2);
      dataItem.push(dataList[index].greaterThanRoundingMode2);
      dataItem.push(dataList[index].greaterThanSightOfBill2);
      dataItem.push(dataList[index].greaterThanCollectCategoryId3);
      dataItem.push(dataList[index].greaterThanRate3);
      dataItem.push(dataList[index].greaterThanRoundingMode3);
      dataItem.push(dataList[index].greaterThanSightOfBill3);
      dataItem.push(dataList[index].useKanaLearning);
      dataItem.push(dataList[index].holidayFlag);
      dataItem.push(dataList[index].useFeeTolerance);
      dataItem.push(dataList[index].prioritizeMatchingIndividually);
      dataItem.push(dataList[index].collationKey);
      dataItem.push(dataList[index].destinationDepartmentName);
      dataItem.push(dataList[index].honorific);
      dataItem = FileUtil.encloseItemBySymbol(dataItem);

      result = result + dataItem.join(",") + LINE_FEED_CODE;
    }

    return result;
  }

  ///// Enterキー押下時の処理 ////////////////////////////////////////////////////////
  public setCustomerCode(eventType: string, type: string = 'from') {

    let ctrlCodeValue = type == 'from' ? this.customerCodeFromCtrl.value : this.customerCodeToCtrl.value;

    if (!StringUtil.IsNullOrEmpty(ctrlCodeValue)) {
      this.loadStart();
      this.customerService.GetItems(ctrlCodeValue)
        .subscribe(response => {
          this.loadEnd();
          if (response != undefined && response != null && 0 < response.length) {

            let codeValue: string;
            let nameValue: string;
            let idValue = 0;

            idValue = response[0].id;
            codeValue = response[0].code;
            nameValue = response[0].name;

            if (type == 'from') {
              this.customerCodeFromId = idValue;
              this.customerCodeFromCtrl.setValue(codeValue);
              this.customerNameFromCtrl.setValue(nameValue);
              HtmlUtil.nextFocusByName(this.elementRef, 'customerCodeToCtrl', eventType);

              if (this.cbxCustomerCtrl.value) {
                this.customerCodeToId = idValue;
                this.customerCodeToCtrl.setValue(codeValue);
                this.customerNameToCtrl.setValue(nameValue);
              }
            } else {
              this.customerCodeToId = idValue;
              this.customerCodeToCtrl.setValue(codeValue);
              this.customerNameToCtrl.setValue(nameValue);
              HtmlUtil.nextFocusByName(this.elementRef, 'closingDateCtrl', eventType);
            }
          }
          else {
            if (type == 'from') {
              this.customerCodeFromId = null;

              if (this.cbxCustomerCtrl.value) {
                this.customerCodeFromId = null;
                this.customerCodeToCtrl.setValue(this.customerCodeFromCtrl.value);
                HtmlUtil.nextFocusByName(this.elementRef, 'closingDateCtrl', eventType);
              }
              else {
                HtmlUtil.nextFocusByName(this.elementRef, 'customerCodeToCtrl', eventType);
              }
            }
            else {
              this.customerCodeToId = null;
              HtmlUtil.nextFocusByName(this.elementRef, 'closingDateCtrl', eventType);
            }
          }

          this.getCustomerCodes();
        });
    }

  }

  public setStaffCode(eventType: string, type: string = 'from') {

    let ctrlCodeValue = type == 'from' ? this.staffCodeFromCtrl.value : this.staffCodeToCtrl.value;

    if (!StringUtil.IsNullOrEmpty(ctrlCodeValue)) {
      this.loadStart();
      this.staffService.GetItems(ctrlCodeValue)
        .subscribe(response => {
          this.loadEnd();
          if (response != undefined && response != null && 0 < response.length) {

            let codeValue: string;
            let nameValue: string;
            let idValue = 0;

            idValue = response[0].id;
            codeValue = response[0].code;
            nameValue = response[0].name;

            if (type == 'from') {
              this.staffCodeFromId = idValue != 0 ? idValue : null;
              this.staffCodeFromCtrl.setValue(codeValue);
              this.staffNameFromCtrl.setValue(nameValue);
              HtmlUtil.nextFocusByName(this.elementRef, 'staffCodeToCtrl', eventType);

              if (this.cbxStaffCtrl.value) {
                this.staffCodeToId = idValue != 0 ? idValue : null;
                this.staffCodeToCtrl.setValue(codeValue);
                this.staffNameToCtrl.setValue(nameValue);
              }
            }
            else {
              this.staffCodeToId = idValue != 0 ? idValue : null;
              this.staffCodeToCtrl.setValue(codeValue);
              this.staffNameToCtrl.setValue(nameValue);
              HtmlUtil.nextFocusByName(this.elementRef, 'updateDatefromCtrl', eventType);
            }
          }
          else {
            if (type == 'from') {
              this.staffCodeFromId = null;

              if (this.cbxStaffCtrl.value) {
                this.staffCodeToId = null;
                this.staffCodeToCtrl.setValue(this.staffCodeFromCtrl.value);
                HtmlUtil.nextFocusByName(this.elementRef, 'updateDatefromCtrl', eventType);
              }
              else {
                HtmlUtil.nextFocusByName(this.elementRef, 'staffCodeToCtrl', eventType);
              }
            }
            else {
              this.staffCodeToId = null;
              HtmlUtil.nextFocusByName(this.elementRef, 'updateDatefromCtrl', eventType);
            }
          }
          //this.getStaffCodes();
        });
    }
  }

  public setCbxCustomer(eventType: string) {

    let localstorageItem = new LocalStorageItem();
    localstorageItem.key = RangeSearchKey.PB0501_CUSTOMER;
    localstorageItem.value = this.cbxCustomerCtrl.value;
    this.localStorageManageService.set(localstorageItem);

    HtmlUtil.nextFocusByName(this.elementRef, 'customerCodeFromCtrl', eventType);

  }

  public setCbxStaff(eventType: string) {

    let localstorageItem = new LocalStorageItem();
    localstorageItem.key = RangeSearchKey.PB0501_STAFF;
    localstorageItem.value = this.cbxStaffCtrl.value;
    this.localStorageManageService.set(localstorageItem);

    HtmlUtil.nextFocusByName(this.elementRef, 'staffCodeFromCtrl', eventType);

  }

  public setRangeCheckbox() {
    let cbxCustomer = this.localStorageManageService.get(RangeSearchKey.PB0501_CUSTOMER);
    let cbxStaff = this.localStorageManageService.get(RangeSearchKey.PB0501_STAFF);

    if (cbxCustomer != null) {
      this.cbxCustomerCtrl.setValue(cbxCustomer.value);
    }
    if (cbxStaff != null) {
      this.cbxStaffCtrl.setValue(cbxStaff.value);
    }
  }

  private getCustomerCodes() {
    if (this.customers.length != 0) return;
    this.customerService.GetItemsByCustomerSearch(new CustomerSearch())
      .subscribe(result => {
        this.processModalCustomResult = this.processResultService.processAtGetData(
          this.processModalCustomResult, result, false, this.partsResultMessageComponent);
        if (this.processModalCustomResult.result == PROCESS_RESULT_RESULT_TYPE.SUCCESS) {
          this.customers = result;
        }
      });
  }

  public getStaffCodes() {
    this.staffService.GetItems()
      .subscribe(result => {
        this.processModalCustomResult = this.processResultService.processAtGetData(
          this.processModalCustomResult, result, false, this.partsResultMessageComponent);
        if (this.processModalCustomResult.result == PROCESS_RESULT_RESULT_TYPE.SUCCESS) {
          this.staffs = result;
        }
      });
  }

  public setClosingDate(eventType: string) {
    HtmlUtil.nextFocusByName(this.elementRef, 'staffCodeFromCtrl', eventType);

  }
}
