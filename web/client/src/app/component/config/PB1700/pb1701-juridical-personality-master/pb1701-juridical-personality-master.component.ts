import { Component, OnInit, ComponentFactoryResolver, ViewContainerRef, ViewChild } from '@angular/core';
import { BaseComponent } from 'src/app/component/common/base/base-component';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { JuridicalPersonalitysResult } from 'src/app/model/juridical-personalitys-result.model';
import { JuridicalPersonalityMasterService } from 'src/app/service/Master/juridical-personality-master.service';
import { UserInfoService } from 'src/app/service/common/user-info.service';
import { COMPONENT_STATUS_TYPE, MODAL_STATUS_TYPE, PROCESS_RESULT_RESULT_TYPE, PROCESS_RESULT_STATUS_TYPE } from 'src/app/common/const/status.const';
import { JuridicalPersonality } from 'src/app/model/juridical-personality.model';
import { DateUtil } from 'src/app/common/util/date-util';
import { FILE_EXTENSION } from 'src/app/common/const/eb-file.const';
import { FileUtil } from 'src/app/common/util/file.util';
import { REPORT_HEADER, LINE_FEED_CODE } from 'src/app/common/const/report-setting-kbn.const';
import { ModalConfirmComponent } from 'src/app/component/modal/modal-confirm/modal-confirm.component';
import { ModalImportMethodSelectorComponent } from 'src/app/component/modal/modal-import-method-selector/modal-import-method-selector.component';
import { TABLE_INDEX } from 'src/app/common/const/table-name.const';
import { BUTTON_ACTION } from 'src/app/common/const/event.const';
import { MSG_WNG } from 'src/app/common/const/message.const';
import { EbDataHelper } from 'src/app/common/util/eb-data-helper';
import { ProcessResultService } from 'src/app/service/common/process-result.service';
import { Router, ActivatedRoute, NavigationEnd } from '@angular/router';
import { PageUtil } from 'src/app/common/util/page-util';
import { ModalRouterProgressComponent } from 'src/app/component/modal/modal-router-progress/modal-router-progress.component';
import { FunctionType } from 'src/app/common/const/kbn.const';
import { MatExpansionPanel } from '@angular/material';

@Component({
  selector: 'app-pb1701-juridical-personality-master',
  templateUrl: './pb1701-juridical-personality-master.component.html',
  styleUrls: ['./pb1701-juridical-personality-master.component.css']
})
export class Pb1701JuridicalPersonalityMasterComponent extends BaseComponent implements OnInit {

  public FunctionType: typeof FunctionType = FunctionType;

  public juridicalPersonalitysResult: JuridicalPersonalitysResult;

  public kanaCtrl: FormControl; // 法人格

  public selectIndex: number;

  public panelOpenState:boolean = true;

  @ViewChild('panel') panel: MatExpansionPanel;

  constructor(
    public router: Router,
    public activatedRoute: ActivatedRoute,
    public componentFactoryResolver: ComponentFactoryResolver,
    public viewContainerRef: ViewContainerRef,
    public userInfoService: UserInfoService,
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
        this.processModalCustomResult = this.processResultService.processResultInit(this.ComponentId, this.Title, true);
      }
    });
  }

  ngOnInit() {
    this.setControlInit();
    this.setValidator();
    this.setFormatter();
    this.clear();

    if (!this.userInfoService.isFunctionAvailable(FunctionType.MasterImport)
      && !this.userInfoService.isFunctionAvailable(FunctionType.MasterExport)) {
      this.securityHideShow = false;
    } else {
      this.securityHideShow = true;
    }
        
    this.getJuridicalPersonalitysResult();
  }

  public setControlInit() {
    this.kanaCtrl = new FormControl('', [Validators.required]); // カナ
  }

  public setValidator() {
    this.MyFormGroup = new FormGroup({

      kanaCtrl: this.kanaCtrl  // 法人格

    })
  }

  public setFormatter() {

  }

  public clear() {
    this.MyFormGroup.reset();
    this.ComponentStatus = COMPONENT_STATUS_TYPE.CREATE;
    this.panelOpenState = false;
    this.panel.close();
  }

  /**
   * データ取得
   */
  public getJuridicalPersonalitysResult() {
    this.juridicalPersonalitysResult = new JuridicalPersonalitysResult();

    let processComponentFactory = this.componentFactoryResolver.resolveComponentFactory(ModalRouterProgressComponent);
    let processComponentRef = this.viewContainerRef.createComponent(processComponentFactory);
    // processComponentRef.destroy();   

    this.juridicalPersonalityService.GetItems()
      .subscribe(response => {
        this.processCustomResult = this.processResultService.processAtGetData(
          this.processCustomResult, response, false, this.partsResultMessageComponent);
        if (this.processCustomResult.result == PROCESS_RESULT_RESULT_TYPE.SUCCESS) {
          this.juridicalPersonalitysResult.juridicalPersonalities = response;
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

      case BUTTON_ACTION.DELETE:
        this.delete();
        break;

      case BUTTON_ACTION.PRINT:
        this.print();
        break;

      case BUTTON_ACTION.IMPORT:
        this.openImportMethodSelector();
        break;

      case BUTTON_ACTION.EXPORT:
        this.export();
        break;

      default:
        console.log('buttonAction Error.');
        break;
    }
  }

  /**
   * 選択した行のデータをフォームに表示する
   * @param index 選択した行番号
   */
  public selectLine(index: number) {
    this.selectIndex = index;
    this.panelOpenState = true;
    this.panel.open();
    this.kanaCtrl.setValue(this.juridicalPersonalitysResult.juridicalPersonalities[index].kana); // 法人格
    this.ComponentStatus = COMPONENT_STATUS_TYPE.UPDATE;
    this.processResultService.clearProcessCustomMsg(this.processCustomResult);
  }

  /**
   * データ登録・編集
   */
  public registry() {
    let registryData = new JuridicalPersonality();
    registryData.createBy = this.userInfoService.LoginUser.id;
    registryData.kana = this.kanaCtrl.value;


    let processComponentFactory = this.componentFactoryResolver.resolveComponentFactory(ModalRouterProgressComponent);
    let processComponentRef = this.viewContainerRef.createComponent(processComponentFactory);
    // processComponentRef.destroy();       

    this.juridicalPersonalityService.Save(registryData)
      .subscribe(result => {
        this.processCustomResult = this.processResultService.processAtSave(
          this.processCustomResult, result, true, this.partsResultMessageComponent);
        if (this.processCustomResult.result == PROCESS_RESULT_RESULT_TYPE.SUCCESS) {
          this.clear();
          this.getJuridicalPersonalitysResult();
        }
        processComponentRef.destroy();       
      });
  }

  /**
   * データ削除
   */
  public delete() {
    let juridicalPersonalitie = this.juridicalPersonalitysResult.juridicalPersonalities[this.selectIndex];
    let componentFactory = this.componentFactoryResolver.resolveComponentFactory(ModalConfirmComponent);
    let componentRef = this.viewContainerRef.createComponent(componentFactory);
    componentRef.instance.ActionName = "削除"
    componentRef.instance.Closing.subscribe(() => {

      if (componentRef.instance.ModalStatus == MODAL_STATUS_TYPE.OK) {

        let processComponentFactory = this.componentFactoryResolver.resolveComponentFactory(ModalRouterProgressComponent);
        let processComponentRef = this.viewContainerRef.createComponent(processComponentFactory);
        // processComponentRef.destroy();   

        this.juridicalPersonalityService.Delete(juridicalPersonalitie)
          .subscribe(result => {
            this.processCustomResult = this.processResultService.processAtDelete(
              this.processCustomResult, result, this.partsResultMessageComponent);
            if (this.processCustomResult.result == PROCESS_RESULT_RESULT_TYPE.SUCCESS) {
              this.clear();
              this.getJuridicalPersonalitysResult();
            }
            processComponentRef.destroy();  
          });
      }
      componentRef.destroy();
    });

  }

  /**
   * 印刷
   */
  public print() {
    let isTryResult: boolean = false;
    this.juridicalPersonalityService.GetReport()
      .subscribe(response => {
        try {
          FileUtil.download([response.body], this.Title + DateUtil.getYYYYMMDD(0), FILE_EXTENSION.PDF);
          isTryResult = true;

        } catch (error) {
          console.error(error);
        }
        this.processResultService.processAtOutput(
          this.processCustomResult, isTryResult, 1, this.partsResultMessageComponent);
        this.openOptions();
      });
  }

  /**
   * インポート
   */
  public openImportMethodSelector() {
    let componentFactory = this.componentFactoryResolver.resolveComponentFactory(ModalImportMethodSelectorComponent);
    let componentRef = this.viewContainerRef.createComponent(componentFactory);

    componentRef.instance.TableIndex = TABLE_INDEX.MASTER_JURDICAL_PERSONALITY;
    componentRef.instance.processModalCustomResult = this.processModalCustomResult;

    componentRef.instance.Closing.subscribe(() => {
      componentRef.destroy();
      this.getJuridicalPersonalitysResult();
    });
    this.openOptions();
  }

  /**
   * エクスポート
   */
  public export() {
    let headers = FileUtil.encloseItemBySymbol(REPORT_HEADER.JURIDICAL_PERSONALITY_MASTER);
    let data: string = headers.join(",") + LINE_FEED_CODE;
    let dataList = this.juridicalPersonalitysResult.juridicalPersonalities;

    // 件数チェック
    if (dataList.length <= 0) {
      this.processCustomResult.status = PROCESS_RESULT_STATUS_TYPE.DONE;
      this.processCustomResult.result = PROCESS_RESULT_RESULT_TYPE.WARNING;
      this.processCustomResult.message = MSG_WNG.NO_EXPORT_DATA;
      return;
    }


    let processComponentFactory = this.componentFactoryResolver.resolveComponentFactory(ModalRouterProgressComponent);
    let processComponentRef = this.viewContainerRef.createComponent(processComponentFactory);
    // processComponentRef.destroy();       

    for (let index = 0; index < dataList.length; index++) {
      let dataItem: Array<any> = [];
      dataItem.push(this.userInfoService.Company.code);
      dataItem.push(dataList[index].kana);
      dataItem = FileUtil.encloseItemBySymbol(dataItem);

      data = data + dataItem.join(",") + LINE_FEED_CODE;
    }

    let isTryResult: boolean = false;
    let resultDatas: Array<any> = [];
    resultDatas.push(data);

    try {
      FileUtil.download(resultDatas, this.Title + DateUtil.getYYYYMMDD(0), FILE_EXTENSION.CSV);
      isTryResult = true;

    } catch (error) {
      console.error(error);
    }

    this.processResultService.processAtOutput(
      this.processCustomResult, isTryResult, 0, this.partsResultMessageComponent);
    this.openOptions();

    processComponentRef.destroy();   
  }

  ///// フォーカス移動時の処理 //////////////////////////////////////////////////////
  public inputKana() {
    this.kanaCtrl.setValue(EbDataHelper.convertToValidEbkana(this.kanaCtrl.value));
  }
}
