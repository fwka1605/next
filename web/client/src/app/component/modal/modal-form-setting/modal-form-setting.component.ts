import { Component, OnInit, EventEmitter, ElementRef, ComponentFactoryResolver, ViewContainerRef } from '@angular/core';
import { BaseComponent } from '../../common/base/base-component';
import { FormControl, FormGroup } from '@angular/forms';
import { ComponentId, ComponentName } from 'src/app/common/const/component-name.const';
import { UserInfoService } from 'src/app/service/common/user-info.service';
import { ReportSettingMasterService } from 'src/app/service/Master/report-setting-master.service';
import { ReportSetting } from 'src/app/model/report-setting.model';
import { SettingMasterService } from 'src/app/service/Master/setting-master.service';
import { Setting } from 'src/app/model/setting.model';
import { ReportSettingsResult } from 'src/app/model/report-settings-result.model';
import { SettingsResult } from 'src/app/model/settings-result.model';
import { ProcessResultService } from 'src/app/service/common/process-result.service';
import { BUTTON_ACTION, EVENT_TYPE } from 'src/app/common/const/event.const';
import { PROCESS_RESULT_RESULT_TYPE, MODAL_STATUS_TYPE } from 'src/app/common/const/status.const';
import { MSG_WNG } from 'src/app/common/const/message.const';
import { HtmlUtil } from 'src/app/common/util/html.util';
import { ModalRouterProgressComponent } from '../modal-router-progress/modal-router-progress.component';

@Component({
  selector: 'app-modal-form-setting',
  templateUrl: './modal-form-setting.component.html',
  styleUrls: ['./modal-form-setting.component.css']
})
export class ModalFormSettingComponent extends BaseComponent implements OnInit {

  public closing = new EventEmitter<{}>();

  public undefineCtrl: FormControl;

  public reportSettingsResult = new ReportSettingsResult();

  public reportSettingCtrls: FormControl[];

  public reportId: string;

  constructor(
    public elementRef: ElementRef,
    public componentFactoryResolver: ComponentFactoryResolver,
    public viewContainerRef: ViewContainerRef,
    public userInfoService: UserInfoService,
    public reportSettingService: ReportSettingMasterService,
    public settingService: SettingMasterService,
    public processResultService: ProcessResultService
  ) {
    super();
  }

  ngOnInit() {
    this.setControlInit();
    this.setValidator();
    this.clear();
    this.processResultService.clearProcessCustomMsg(this.processModalCustomResult);
    this.initControl();
  }

  public setControlInit() {
    this.undefineCtrl = new FormControl("");
  }

  public setValidator() {
    this.MyFormGroup = new FormGroup({
      UndefineCtrl: this.undefineCtrl,
    });
  }

 
  /**
   * 設定呼込
   */
  public initControl() {

    let processComponentFactory = this.componentFactoryResolver.resolveComponentFactory(ModalRouterProgressComponent);
    let processComponentRef = this.viewContainerRef.createComponent(processComponentFactory);
    // processComponentRef.destroy();   

    // API呼出引数
    let reportSetting = new ReportSetting();
    reportSetting.companyId = this.userInfoService.Company.id;
    reportSetting.reportId = this.reportId;

    // API呼出結果
    let settingsResult = new SettingsResult();
    this.reportSettingService.GetItems(reportSetting)
      .subscribe(response => {
        this.processModalCustomResult = this.processResultService.processAtGetData(
          this.processModalCustomResult, response, false, this.partsResultMessageComponent);
        if (this.processModalCustomResult.result == PROCESS_RESULT_RESULT_TYPE.SUCCESS) {
          this.reportSettingsResult.reportSettings = response;

          // API呼出し引数
          let itemIds = new Array<string>();

          // 一意のItemID配列を作成+FormControlの初期化
          let index = 0;
          this.reportSettingCtrls = new Array<FormControl>(this.reportSettingsResult.reportSettings.length);
          this.reportSettingsResult.reportSettings.forEach(element => {

            this.reportSettingCtrls[index] = new FormControl(null);
            this.MyFormGroup.removeControl("reportSettingCtrl" + index);
            this.MyFormGroup.addControl("reportSettingCtrl" + index, this.reportSettingCtrls[index]);
            this.reportSettingCtrls[index].setValue(element.itemKey);
            index++;

            if (!itemIds.includes(element.itemId)) {
              itemIds.push(element.itemId)
            }
          });

          this.settingService.GetItems(itemIds)
            .subscribe(response => {
              this.processModalCustomResult = this.processResultService.processAtGetData(
                this.processModalCustomResult, response, false, this.partsResultMessageComponent);
              if (this.processModalCustomResult.result == PROCESS_RESULT_RESULT_TYPE.SUCCESS) {
                settingsResult.settings = response;

                this.reportSettingsResult.reportSettings.forEach(reportSetting => {

                  settingsResult.settings.forEach(setting => {
                    if (reportSetting.itemId == setting.itemId) {
                      if (reportSetting.settingList == null) {
                        reportSetting.settingList = new Array<Setting>();
                      }
                      reportSetting.settingList.push(setting);
                    }
                  });
                });

                let index = 0;
                index = index + 1;
              }
              processComponentRef.destroy();  
            });
        }
        else{
          processComponentRef.destroy();            
        }
      });
  }

  public get Closing() {
    return this.closing;
  }
  public set Closing(value) {
    this.closing = value;
  }

  public get ReportId(): string {
    return this.reportId;
  }
  public set ReportId(reportId: string) {
    this.reportId = reportId;
  }

  public get ProcessModalCustomResult() {
    return this.processModalCustomResult;
  }
  public set ProcessModalCustomResult(value) {
    this.processModalCustomResult = value;
  }

  public clear() {
    this.MyFormGroup.reset();
    HtmlUtil.nextFocusByName(this.elementRef, 'reportSettingCtrl0', EVENT_TYPE.NONE);
    
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
        this.redisplay();
        break;

      default:
        console.log('buttonAction Error.');
        break;
    }

  }

  /**
   * データ登録
   */
  public registry() {
    if (!this.ValidateInput()) return;

    let processComponentFactory = this.componentFactoryResolver.resolveComponentFactory(ModalRouterProgressComponent);
    let processComponentRef = this.viewContainerRef.createComponent(processComponentFactory);
    // processComponentRef.destroy();    


    this.reportSettingService.Save(this.reportSettingsResult.reportSettings)
      .subscribe(response => {
        this.processModalCustomResult = this.processResultService.processAtSave(
          this.processModalCustomResult, response, false, this.partsResultMessageComponent);
        if (this.processModalCustomResult.result == PROCESS_RESULT_RESULT_TYPE.SUCCESS) {
          // 戻り値を取得するとsettingテーブルの再設定が必要なため取得しない。
          //this.reportSettingsResult.reportSettings=response;
        }
        processComponentRef.destroy();  
      });
  }

  /**
   * バリデーションチェック
   */
  public ValidateInput(): boolean {
    this.SetReportSettingInfo();

    let msg = '';
    let totalByDayNone: boolean = false;

    this.reportSettingsResult.reportSettings.forEach(element => {
      if (
        element.caption == "請求データ集計"
        && element.itemKey == "2"
        && element.itemValue == "しない"
      ) {
        totalByDayNone = true;
      }
    });
    let totalOnly: boolean = false;
    this.reportSettingsResult.reportSettings.forEach(element => {
      if (
        element.caption == "伝票集計方法"
        && element.itemKey == "0"
        && element.itemValue == "合計"
      ) {
        totalOnly = true;
      }
    });

    if (ComponentName[this.ComponentId] == ComponentName[ComponentId.PF0501]
      && !totalByDayNone
      && totalOnly) {
      msg = MSG_WNG.NOT_ALLOWED_SHEET_SUMMARY_WHEN_BILLING_SUMMARY;
      this.processResultService.processAtWarning(
        this.processModalCustomResult, msg, this.partsResultMessageComponent);
      return false;
    }

    let doPageBreak: boolean = false;
    this.reportSettingsResult.reportSettings.forEach(element => {
      if (
        element.caption == "請求部門ごと改ページ"
        && element.itemKey == "1"
        && element.itemValue == "する"
      ) {
        doPageBreak = true;
      }
    });

    let hideDepartment: boolean = false;
    this.reportSettingsResult.reportSettings.forEach(element => {
      if (
        element => element.Caption == "請求部門ごと表示"
          && element.ItemKey == "0"
          && element.ItemValue == "しない"
      ) {
        hideDepartment = true;
      }
    });


    if (
      ComponentName[this.ComponentId] == ComponentName[ComponentId.PF0601]
      && doPageBreak
      && hideDepartment) {
      msg = MSG_WNG.NOT_ALLOWED_DEPT_PAGE_BREAKE_WHEN_NOT_DISPLAY;
      this.processResultService.processAtWarning(
        this.processModalCustomResult, msg, this.partsResultMessageComponent);
      return false;
    }

    return true;
  }


  /**
   * 帳票設定取得
   */
  public SetReportSettingInfo() {
    for (let i = 0; i < this.reportSettingsResult.reportSettings.length; i++) {
      let reportSetting = new ReportSetting();
      reportSetting = this.reportSettingsResult.reportSettings[i];

      if (reportSetting.isText != 1) {
        reportSetting.itemKey = this.reportSettingCtrls[i].value;
        //reportSetting.itemValue = "comboCell.DisplayText.ToString()";
      }
      else {
        reportSetting.itemKey = this.reportSettingCtrls[i].value;;

      }
      this.reportSettingsResult.reportSettings[i] = reportSetting;
    }
  }

  /**
   * モーダルを閉じる
   */
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

  /**
   * 再表示
   */
  public redisplay() {
    this.initControl();
  }

}
