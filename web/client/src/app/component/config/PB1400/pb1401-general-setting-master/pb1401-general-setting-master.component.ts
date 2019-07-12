import { Component, OnInit, ComponentFactoryResolver, ViewContainerRef, Input, ViewChild } from '@angular/core';
import { BaseComponent } from 'src/app/component/common/base/base-component';
import { FormControl, FormGroup } from '@angular/forms';
import { GeneralSettingsResult } from 'src/app/model/general-settings-result.model';
import { GeneralSettingMasterService } from 'src/app/service/Master/general-setting-master.service';
import { UserInfoService } from 'src/app/service/common/user-info.service';
import { COMPONENT_STATUS_TYPE, PROCESS_RESULT_RESULT_TYPE, PROCESS_RESULT_STATUS_TYPE, MODAL_STATUS_TYPE } from 'src/app/common/const/status.const';
import { GeneralSetting } from 'src/app/model/general-setting.model';
import { BUTTON_ACTION, KEY_CODE, EVENT_TYPE } from 'src/app/common/const/event.const';
import { MSG_WNG, MSG_ITEM_NUM } from 'src/app/common/const/message.const';
import { FileUtil } from 'src/app/common/util/file.util';
import { DateUtil } from 'src/app/common/util/date-util';
import { FILE_EXTENSION } from 'src/app/common/const/eb-file.const';
import { ProcessResultService } from 'src/app/service/common/process-result.service';
import { Router, ActivatedRoute, NavigationEnd } from '@angular/router';
import { PageUtil } from 'src/app/common/util/page-util';
import { ModalRouterProgressComponent } from 'src/app/component/modal/modal-router-progress/modal-router-progress.component';
import { StringUtil } from 'src/app/common/util/string-util';
import { CODE_TYPE } from 'src/app/common/const/kbn.const';
import { DepartmentMasterService } from 'src/app/service/Master/department-master.service';
import { FormatterUtil } from 'src/app/common/util/formatter.util';
import { NgbDateParserFormatter, NgbCalendar } from '@ng-bootstrap/ng-bootstrap';
import { NgbDateCustomParserFormatter } from 'src/app/common/util/dateformat-util';
import { TABLE_INDEX } from 'src/app/common/const/table-name.const';
import { ModalMasterComponent } from 'src/app/component/modal/modal-master/modal-master.component';
import { AccountTitleMasterService } from 'src/app/service/Master/account-title-master.service';
import { DepartmentSearch } from 'src/app/model/department-search.model';
import { MatAutocompleteTrigger, MatExpansionPanel } from '@angular/material';


@Component({
  selector: 'app-pb1401-general-setting-master',
  templateUrl: './pb1401-general-setting-master.component.html',
  styleUrls: ['./pb1401-general-setting-master.component.css'],
  providers: [
    { provide: NgbDateParserFormatter, useClass: NgbDateCustomParserFormatter }
  ]
})
export class Pb1401GeneralSettingMasterComponent extends BaseComponent implements OnInit {
  public readonly InputType: typeof InputType = InputType;

  public panelOpenState;

  public generalSettingsResult: GeneralSettingsResult;

  public managementCodeCtrl: FormControl;   // 管理コード
  public managementNameCtrl: FormControl;   // 管理名称

  public managementValueCtrl: FormControl;  // データ
  public managementValueCodeCtrl: FormControl;  // データ
  public managementValueNumberCtrl: FormControl;  // データ

  public departmentCodeCtrl: FormControl; // データ
  public accountTitleCodeCtrl: FormControl; // データ

  public managementValueNameCtrl: FormControl;  // データ
  public managementDescriptionCtrl: FormControl;  // 説明
  public managementLengthCtrl: FormControl;  // 有効桁数

  public selectIndex: number;
  public maxlength: number = 0;
  public inputType: InputType = InputType.NORMAL;
  public masterTableIndex: number = 0;
  public isInvalidButton: boolean = true;


  @ViewChild('departmentCodeInput', { read: MatAutocompleteTrigger }) departmentCodeigger: MatAutocompleteTrigger;
  @ViewChild('accountTitleCodeInput', { read: MatAutocompleteTrigger }) accountTitleCodeigger: MatAutocompleteTrigger;

  @ViewChild('panel') panel: MatExpansionPanel;


  constructor(
    public router: Router,
    public activatedRoute: ActivatedRoute,
    public componentFactoryResolver: ComponentFactoryResolver,
    public viewContainerRef: ViewContainerRef,
    public userInfoService: UserInfoService,
    public generalSettingService: GeneralSettingMasterService,
    public processResultService: ProcessResultService,
    public departmentService: DepartmentMasterService,
    public accountTitleService: AccountTitleMasterService,
    public calendar: NgbCalendar,

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
    this.setControlInit();
    this.setValidator();
    this.setFormatter();
    this.clear();
    this.setAutoComplete();
    this.getGeneralSettingData();
  }

  public setControlInit() {
    this.managementCodeCtrl = new FormControl('');   // 管理コード

    this.managementValueCtrl = new FormControl(''); // データ
    this.managementValueCodeCtrl = new FormControl(''); // データ
    this.managementValueNumberCtrl = new FormControl(''); // データ

    this.departmentCodeCtrl = new FormControl(''); // データ
    this.accountTitleCodeCtrl = new FormControl(''); // データ

    this.managementValueNameCtrl = new FormControl(''); // データ
    this.managementDescriptionCtrl = new FormControl(''); // 説明
    this.managementLengthCtrl = new FormControl('');  // 有効桁数
  }

  public setValidator() {
    this.MyFormGroup = new FormGroup({
      managementCodeCtrl: this.managementCodeCtrl,   // 管理コード

      managementValueCtrl: this.managementValueCtrl, // データ
      managementValueCodeCtrl: this.managementValueCodeCtrl, // データ
      managementValueNumberCtrl: this.managementValueNumberCtrl, // データ
      departmentCodeCtrl: this.departmentCodeCtrl, // データ      
      accountTitleCodeCtrl: this.accountTitleCodeCtrl, // データ      

      managementValueNameCtrl: this.managementValueNameCtrl,
      managementDescriptionCtrl: this.managementDescriptionCtrl, // 説明
      managementLengthCtrl: this.managementLengthCtrl,  // 有効桁数
    })
  }

  public clear() {
    this.MyFormGroup.reset();
    this.isInvalidButton = true;
    this.ComponentStatus = COMPONENT_STATUS_TYPE.CREATE;

    this.panelOpenState = false;
    this.panel.close();
  }

  public setFormatter() {
    if (this.userInfoService.ApplicationControl.accountTitleCodeType == CODE_TYPE.NUMBER) {
      FormatterUtil.setNumberFormatter(this.accountTitleCodeCtrl);
    }
    else {
      FormatterUtil.setCodeFormatter(this.accountTitleCodeCtrl);
    }

    if (this.userInfoService.ApplicationControl.departmentCodeType == CODE_TYPE.NUMBER) {
      FormatterUtil.setNumberFormatter(this.departmentCodeCtrl);
    }
    else {
      FormatterUtil.setCodeFormatter(this.departmentCodeCtrl);
    }

    FormatterUtil.setCodeFormatter(this.managementValueCodeCtrl);
    FormatterUtil.setNumberFormatter(this.managementValueNumberCtrl);
  }


  public setAutoComplete() {

    // 請求部門
    this.initAutocompleteDepartments(this.departmentCodeCtrl, this.departmentService, 0);

    // 最終更新者
    this.initautocompleteAccountTitle(this.accountTitleCodeCtrl, this.accountTitleService, 0);


  }
  /**
   * データ取得
   */
  public getGeneralSettingData() {

    let processComponentFactory = this.componentFactoryResolver.resolveComponentFactory(ModalRouterProgressComponent);
    let processComponentRef = this.viewContainerRef.createComponent(processComponentFactory);
    // processComponentRef.destroy();  

    this.generalSettingService.GetItems()
      .subscribe(result => {
        this.processCustomResult = this.processResultService.processAtGetData(
          this.processCustomResult, result, false, this.partsResultMessageComponent);
        if (this.processCustomResult.result == PROCESS_RESULT_RESULT_TYPE.SUCCESS) {
          this.generalSettingsResult = new GeneralSettingsResult();
          this.generalSettingsResult.generalSettings = new Array<GeneralSetting>();
          this.generalSettingsResult.generalSettings = result;
        }
        processComponentRef.destroy();
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

      case BUTTON_ACTION.PRINT:
        this.print();
        break;

      default:
        console.log('buttonAction Error.');
        break;
    }
  }

  /**
   * データ編集
   */
  public registry() {
    let processComponentFactory = this.componentFactoryResolver.resolveComponentFactory(ModalRouterProgressComponent);
    let processComponentRef = this.viewContainerRef.createComponent(processComponentFactory);
    // processComponentRef.destroy();  

    if (this.ComponentStatus == COMPONENT_STATUS_TYPE.UPDATE) {
      let registryData = this.generalSettingsResult.generalSettings[this.selectIndex];

      switch (this.inputType) {
        case InputType.CODE:
          registryData.value = this.managementValueCodeCtrl.value;
          break;

        case InputType.NUMBER:
          registryData.value = this.managementValueNumberCtrl.value;
          break;

        case InputType.ACCOUNT_TITLE:
          registryData.value = this.accountTitleCodeCtrl.value;
          break;

        case InputType.DEPARTMENT:
          registryData.value = this.departmentCodeCtrl.value;
          break;

        case InputType.CALENDAR:
          let dtStr = DateUtil.ConvertFromDatepicker(this.managementValueCtrl);
          registryData.value = DateUtil.getYYYYMMDD(0, dtStr);
          break;

        default:
          registryData.value = this.managementValueCtrl.value;
          break;
      }

      this.generalSettingService.Save(registryData)
        .subscribe(result => {
          this.processCustomResult = this.processResultService.processAtSave(
            this.processCustomResult, result, false, this.partsResultMessageComponent);
          if (this.processCustomResult.result == PROCESS_RESULT_RESULT_TYPE.SUCCESS) {
            this.clear();
            this.getGeneralSettingData();
          }
          processComponentRef.destroy();
        });
    } else {
      this.processCustomResult.status = PROCESS_RESULT_STATUS_TYPE.DONE;
      this.processCustomResult.result = PROCESS_RESULT_RESULT_TYPE.WARNING;
      this.processCustomResult.message = MSG_WNG.SELECTION_REQUIRED.replace(MSG_ITEM_NUM.FIRST, "登録する管理コード");
      processComponentRef.destroy();
    }
  }

  /**
   * 選択した行のデータをフォームに表示する
   * @param index 選択した行番号
   */
  public selectLine(index: number) {
    this.selectIndex = index;
    this.clear();

    this.panelOpenState = true;
    this.panel.open();

    this.isInvalidButton = false;

    this.managementCodeCtrl.setValue(this.generalSettingsResult.generalSettings[index].code);  // 管理コード
    this.managementDescriptionCtrl.setValue(this.generalSettingsResult.generalSettings[index].description);  // 説明
    this.managementLengthCtrl.setValue(this.generalSettingsResult.generalSettings[index].length);   // 有効桁数

    this.ComponentStatus = COMPONENT_STATUS_TYPE.UPDATE;
    this.maxlength = this.generalSettingsResult.generalSettings[index].length;

    this.setManagementFormatter(this.managementCodeCtrl.value);
    switch (this.inputType) {
      case InputType.CODE:
        this.managementValueCodeCtrl.setValue(this.generalSettingsResult.generalSettings[index].value);
        break;

      case InputType.NUMBER:
        this.managementValueNumberCtrl.setValue(this.generalSettingsResult.generalSettings[index].value);
        break;

      case InputType.ACCOUNT_TITLE:
        this.accountTitleCodeCtrl.setValue(this.generalSettingsResult.generalSettings[index].value);
        this.setMasterCode();
        break;

      case InputType.DEPARTMENT:
        this.departmentCodeCtrl.setValue(this.generalSettingsResult.generalSettings[index].value);
        this.setMasterCode();
        break;

      case InputType.CALENDAR:
        let dt = DateUtil.convertStringFromNgbDate(this.generalSettingsResult.generalSettings[index].value);
        this.managementValueCtrl.setValue(dt);
        break;

      default:
        this.managementValueCtrl.setValue(this.generalSettingsResult.generalSettings[index].value);
        break;
    }
    this.processResultService.clearProcessCustomMsg(this.processCustomResult);
  }

  /**
   * 印刷用のPDFをダウンロードする
   */
  public print() {
    let isTryResult = false;
    this.generalSettingService.GetReport()
      .subscribe(response => {
        console.log(JSON.stringify(response));
        try {
          FileUtil.download([response.body], this.Title + DateUtil.getYYYYMMDD(0), FILE_EXTENSION.PDF);
          isTryResult = true;
        } catch (error) {
          console.log(error);
        }
        this.processResultService.processAtOutput(
          this.processCustomResult, isTryResult, 1, this.partsResultMessageComponent);
        this.openOptions();
      });
  }


  public setMasterCode(eventType: string = null) {

    if (eventType != EVENT_TYPE.BLUR) {
      if (this.departmentCodeigger != undefined) {
        this.departmentCodeigger.closePanel();
      }
      if (this.accountTitleCodeigger != undefined) {
        this.accountTitleCodeigger.closePanel();
      }
    }

    switch (this.masterTableIndex) {
      case TABLE_INDEX.MASTER_ACCOUNT_TITLE:
        if (0 < this.masterTableIndex && !StringUtil.IsNullOrEmpty(this.accountTitleCodeCtrl.value)) {
          this.loadStart();
          this.accountTitleService.Get(this.accountTitleCodeCtrl.value)
            .subscribe(response => {
              this.loadEnd();
              if (response != PROCESS_RESULT_RESULT_TYPE.FAILURE && response.length > 0) {
                this.accountTitleCodeCtrl.setValue(response[0].code);
                this.managementValueNameCtrl.setValue(response[0].name);
              }
              else {
                //this.accountTitleCodeCtrl.setValue("");
                this.managementValueNameCtrl.setValue("");
              }
            });
        }
        else {
          this.accountTitleCodeCtrl.setValue("");
          this.managementValueNameCtrl.setValue("");
        }
        break;

      case TABLE_INDEX.MASTER_DEPARTMENT:

        if (0 < this.masterTableIndex && !StringUtil.IsNullOrEmpty(this.departmentCodeCtrl.value)) {
          let search = new DepartmentSearch();
          search.codes = [this.departmentCodeCtrl.value];
          this.loadStart();
          this.departmentService.GetItemsByDepartmentSearch(search)
            .subscribe(response => {
              this.loadEnd();
              if (response != PROCESS_RESULT_RESULT_TYPE.FAILURE && response.length > 0) {
                this.departmentCodeCtrl.setValue(response[0].code);
                this.managementValueNameCtrl.setValue(response[0].name);
              }
              else {
                //this.departmentCodeCtrl.setValue("");
                this.managementValueNameCtrl.setValue("");
              }
            });
        }
        else {
          this.departmentCodeCtrl.setValue("");
          this.managementValueNameCtrl.setValue("");
        }

        break;
    }
  }

  public openMasterModal() {
    if (0 < this.masterTableIndex) {
      if (this.departmentCodeigger != undefined) {
        this.departmentCodeigger.closePanel();
      }
      if (this.accountTitleCodeigger != undefined) {
        this.accountTitleCodeigger.closePanel();
      }      
      
      let componentFactory = this.componentFactoryResolver.resolveComponentFactory(ModalMasterComponent);
      let componentRef = this.viewContainerRef.createComponent(componentFactory);
      componentRef.instance.TableIndex = this.masterTableIndex;

      componentRef.instance.Closing.subscribe(() => {
        if (componentRef.instance.ModalStatus == MODAL_STATUS_TYPE.SELECT) {

          if(this.inputType=InputType.DEPARTMENT){
            this.departmentCodeCtrl.setValue(componentRef.instance.SelectedCode);
          }
          else{
            this.accountTitleCodeCtrl.setValue(componentRef.instance.SelectedCode);
          }
        }
        componentRef.destroy();
      });
    }
  }

  private setManagementFormatter(code: string) {
    this.masterTableIndex = 0;

    switch (code) {
      case '仮受科目コード':
      case '借方消費税誤差科目コード':
      case '振込手数料科目コード':
      case '貸方消費税誤差科目コード':
      case '長期前受金科目コード':
        this.masterTableIndex = TABLE_INDEX.MASTER_ACCOUNT_TITLE;
        this.inputType = InputType.ACCOUNT_TITLE;
        break;

      case '仮受部門コード':
      case '借方消費税誤差部門コード':
      case '振込手数料部門コード':
      case '貸方消費税誤差部門コード':
      case '長期前受金部門コード':
      case '入金部門コード':
        this.masterTableIndex = TABLE_INDEX.MASTER_DEPARTMENT;
        this.inputType = InputType.DEPARTMENT;
        break;

      // コード
      case '仮受補助コード':
      case '借方消費税誤差補助コード':
      case '振込手数料補助コード':
      case '貸方消費税誤差補助コード':
      case '長期前受金補助コード':
      case '入金区分前受コード':
        this.inputType = InputType.CODE;
        break;

      // 数値
      case '回収予定範囲':
      case '金額計算端数処理':
      case '取込時端数処理':
      case '消費税計算端数処理':
      case '旧消費税率':
      case '新消費税率':
      case '新消費税率2':
      case '新消費税率3':
      case '請求データ検索開始月範囲':
      case '手数料誤差':
      case '消費税誤差':
      case '請求入力明細件数':
        this.inputType = InputType.NUMBER;
        break;

      // 日付
      case '新税率開始年月日':
      case '新税率開始年月日2':
      case '新税率開始年月日3':
        this.inputType = InputType.CALENDAR;
        break;

      default:
        this.inputType = InputType.NORMAL;
        break;
    }
  }

}

export enum InputType {
  NORMAL,
  DEPARTMENT,
  ACCOUNT_TITLE,
  CODE,
  NUMBER,
  CALENDAR
}