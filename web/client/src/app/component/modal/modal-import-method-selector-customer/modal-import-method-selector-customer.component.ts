import { Component, OnInit, EventEmitter, ViewContainerRef, ComponentFactoryResolver, ViewChild } from '@angular/core';
import { TABLE_INDEX } from 'src/app/common/const/table-name.const';
import { FormControl, Validators, FormGroup } from '@angular/forms';
import { BaseComponent } from '../../common/base/base-component';
import { FormatterUtil } from 'src/app/common/util/formatter.util';
import { KEY_CODE, EVENT_TYPE } from 'src/app/common/const/event.const';
import { ModalMasterComponent } from '../modal-master/modal-master.component';
import { MODAL_STATUS_TYPE, PROCESS_RESULT_RESULT_TYPE } from 'src/app/common/const/status.const';
import { ModalImportMethodSelectorComponent } from '../modal-import-method-selector/modal-import-method-selector.component';
import { ProcessResultService } from 'src/app/service/common/process-result.service';
import { StringUtil } from 'src/app/common/util/string-util';
import { ImporterSettingsResult } from 'src/app/model/importer-settings-result.model';
import { ProcessResult } from 'src/app/model/process-result.model';
import { ImporterSetting } from 'src/app/model/importer-setting.model';
import { ImporterSettingService } from 'src/app/service/Master/importer-setting.service';
import { FreeImporterFormatType } from 'src/app/common/common-const';
import { MatAutocompleteTrigger } from '@angular/material';

@Component({
  selector: 'app-modal-import-method-selector-customer',
  templateUrl: './modal-import-method-selector-customer.component.html',
  styleUrls: ['./modal-import-method-selector-customer.component.css']
})
export class ModalImportMethodSelectorCustomerComponent extends BaseComponent implements OnInit {

  @ViewChild('patternNoInput', { read: MatAutocompleteTrigger }) patternNoTrigger: MatAutocompleteTrigger;

  /** インポート結果（画面表示用） */
  public importDisplayResult: string = '';
  /** 出力対象 */
  public rdoOutputTargetCtrl: FormControl;
  /** パターンNo */
  public patternNumberCtrl: FormControl;
  /** パターン名 */
  public patternNameCtrl: FormControl;


  constructor(
    public componentFactoryResolver: ComponentFactoryResolver,
    public viewContainerRef: ViewContainerRef,
    public importerSettingService: ImporterSettingService,
    public processResultService: ProcessResultService

  ) {
    super();
  }

  ngOnInit() {
    this.setControlInit();
    this.setValidator();
    this.setFormatter();
    this.clear();
    this.setAutoComplete();
  }

  public setControlInit() {
    this.rdoOutputTargetCtrl = new FormControl("");
    this.patternNumberCtrl = new FormControl("", [Validators.required]);
    this.patternNameCtrl = new FormControl("");
  }

  public setValidator() {
    this.MyFormGroup = new FormGroup({
      rdoOutputTargetCtrl: this.rdoOutputTargetCtrl,
      patternNumberCtrl: this.patternNumberCtrl,
      patternNameCtrl: this.patternNameCtrl
    })
  }

  public setFormatter() {
    FormatterUtil.setCodeFormatter(this.patternNumberCtrl);
  }

  public setAutoComplete() {
    // パターンNo
    this.initAutocompleteImporterSetting(FreeImporterFormatType.Customer, this.patternNameCtrl, this.importerSettingService, 0);
  }

  public clear() {
    this.MyFormGroup.reset();
    this.rdoOutputTargetCtrl.setValue('1');

    this.patternNameCtrl.enable();
    this.patternNumberCtrl.enable();
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
    this.ModalStatus = MODAL_STATUS_TYPE.CANCEL;
    this.processModalCustomResult = this.processResultService.clearProcessCustomMsg(this.processModalCustomResult);
    this.closing.emit({});
  }


  public get ProcessModalCustomResult() {
    return this.processModalCustomResult;
  }
  public set ProcessModalCustomResult(value) {
    this.processModalCustomResult = value;
  }

  /**
   * 検索項目の表示切替
   */
  public selectOutputType() {

    this.patternNumberCtrl.setValue("");
    this.patternNameCtrl.setValue("");

    if (this.rdoOutputTargetCtrl.value == '2') {

      this.patternNameCtrl.disable();
      this.patternNumberCtrl.disable();

    } else {

      this.patternNameCtrl.enable();
      this.patternNumberCtrl.enable();

      this.MyFormGroup.enable();
    }
  }

  /**
   * パターン情報取得
   * @param keyCode キーイベント
   */
  public openMasterModal() {
    let componentFactory = this.componentFactoryResolver.resolveComponentFactory(ModalMasterComponent);
    let componentRef = this.viewContainerRef.createComponent(componentFactory);
    componentRef.instance.zIndex = componentRef.instance.zIndexDefSize * 2;

    componentRef.instance.TableIndex = TABLE_INDEX.MASTER_CUSTOMER_IMPORTER_SETTING;

    componentRef.instance.Closing.subscribe(() => {

      if (componentRef.instance.ModalStatus == MODAL_STATUS_TYPE.SELECT) {

        this.patternNumberCtrl.setValue(componentRef.instance.SelectedCode);
        this.patternNameCtrl.setValue(componentRef.instance.SelectedName);
      }
      componentRef.destroy();
    });
  }

  public setPatternNumber(keyCode: string = null) {

    if (keyCode != EVENT_TYPE.BLUR) {
      this.patternNoTrigger.closePanel();
    }

    if (!StringUtil.IsNullOrEmpty(this.patternNumberCtrl.value)) {
      this.patternNumberCtrl.setValue(StringUtil.setPaddingFrontZero(this.patternNumberCtrl.value, 2));

      let mastersResult = new ImporterSettingsResult();
      mastersResult.processResult = new ProcessResult();
      mastersResult.processResult.result = false;
      mastersResult.importerSettings = new Array<ImporterSetting>();


      this.importerSettingService.GetHeader(FreeImporterFormatType.Customer, this.patternNumberCtrl.value)
        .subscribe(response => {

          if (response != PROCESS_RESULT_RESULT_TYPE.FAILURE) {

            this.patternNumberCtrl.setValue(response[0].code);
            this.patternNameCtrl.setValue(response[0].name);

          }
          else {
            this.patternNumberCtrl.setValue("");
            this.patternNameCtrl.setValue("");
          }
        }
        );
    }
    else {
      this.patternNumberCtrl.setValue("");
      this.patternNameCtrl.setValue("");
    }

  }


  /**
   * インポート
   */
  public openImportMethodSelector() {
    let componentFactory = this.componentFactoryResolver.resolveComponentFactory(ModalImportMethodSelectorComponent);
    let componentRef = this.viewContainerRef.createComponent(componentFactory);

    if (this.rdoOutputTargetCtrl.value == '1') {
      componentRef.instance.TableIndex = TABLE_INDEX.MASTER_CUSTOMER;
    } else {
      componentRef.instance.TableIndex = TABLE_INDEX.MASTER_CUSTOMER_FEE;
    }
    componentRef.instance.ImporterSettingCode = this.patternNumberCtrl.value;
    componentRef.instance.processModalCustomResult = this.processModalCustomResult;

    componentRef.instance.Closing.subscribe(() => {
      this.processModalCustomResult = componentRef.instance.processModalCustomResult;
      componentRef.destroy();
      this.close();
    });
  }

}
