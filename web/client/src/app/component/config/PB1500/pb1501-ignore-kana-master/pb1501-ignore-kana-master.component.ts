
import { Component, OnInit, ComponentFactoryResolver, ViewContainerRef, ElementRef, ViewChild } from '@angular/core';
import { BaseComponent } from 'src/app/component/common/base/base-component';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { ModalMasterComponent } from 'src/app/component/modal/modal-master/modal-master.component';
import { BankAccountMasterService } from 'src/app/service/Master/bank-account-master.service';
import { HtmlUtil } from 'src/app/common/util/html.util';
import { KEY_CODE, BUTTON_ACTION, EVENT_TYPE } from 'src/app/common/const/event.const';
import { FormatterUtil } from 'src/app/common/util/formatter.util';
import { IgnoreKanasResult } from 'src/app/model/ignore-kanas-result.model';
import { IgnoreKanaMasterService } from 'src/app/service/Master/ignore-kana-master.service';
import { UserInfoService } from 'src/app/service/common/user-info.service';
import { CustomValidators } from 'ng5-validation';
import { IgnoreKana } from 'src/app/model/ignore-kana.model';
import { COMPONENT_STATUS_TYPE, MODAL_STATUS_TYPE, PROCESS_RESULT_RESULT_TYPE } from 'src/app/common/const/status.const';
import { ModalConfirmComponent } from 'src/app/component/modal/modal-confirm/modal-confirm.component';
import { DateUtil } from 'src/app/common/util/date-util';
import { TABLE_INDEX, TABLES } from 'src/app/common/const/table-name.const';
import { FILE_EXTENSION } from 'src/app/common/const/eb-file.const';
import { FileUtil } from 'src/app/common/util/file.util';
import { REPORT_HEADER, LINE_FEED_CODE } from 'src/app/common/const/report-setting-kbn.const';
import { ModalImportMethodSelectorComponent } from 'src/app/component/modal/modal-import-method-selector/modal-import-method-selector.component';
import { EbDataHelper } from 'src/app/common/util/eb-data-helper';
import { ProcessResultService } from 'src/app/service/common/process-result.service';
import { MSG_WNG } from 'src/app/common/const/message.const';
import { Router, ActivatedRoute, NavigationEnd } from '@angular/router';
import { PageUtil } from 'src/app/common/util/page-util';
import { StringUtil } from 'src/app/common/util/string-util';
import { CategoryMasterService } from 'src/app/service/Master/category-master.service';
import { CategoryType, FunctionType } from 'src/app/common/const/kbn.const';
import { ModalRouterProgressComponent } from 'src/app/component/modal/modal-router-progress/modal-router-progress.component';
import { MatAutocompleteTrigger, MatExpansionPanel } from '@angular/material';

@Component({
  selector: 'app-pb1501-ignore-kana-master',
  templateUrl: './pb1501-ignore-kana-master.component.html',
  styleUrls: ['./pb1501-ignore-kana-master.component.css']
})
export class Pb1501IgnoreKanaMasterComponent extends BaseComponent implements OnInit {

  public FunctionType: typeof FunctionType = FunctionType;

  public panelOpenState;

  public ignoreKanasResult: IgnoreKanasResult;

  public kanaCtrl: FormControl; // カナ

  public excludeCategoryCodeCtrl: FormControl; // 対象外区分コード
  public excludeCategoryNameCtrl: FormControl;

  public excludeCategoryId: number;
  public selectIndex: number;

  @ViewChild('excludeCategoryCodeToInput', { read: MatAutocompleteTrigger }) excludeCategoryCodeTrigger: MatAutocompleteTrigger;
  @ViewChild('panel') panel: MatExpansionPanel;

  constructor(
    public router: Router,
    public activatedRoute: ActivatedRoute,
    public elementRef: ElementRef,
    public componentFactoryResolver: ComponentFactoryResolver,
    public viewContainerRef: ViewContainerRef,
    public userInfoService: UserInfoService,
    public ignoreKanaService: IgnoreKanaMasterService,
    public processResultService: ProcessResultService,
    public categoryService: CategoryMasterService
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

    this.setAutoComplete();
    this.getKanaResult();
  }

  public setControlInit() {
    this.kanaCtrl = new FormControl(""); // カナ

    this.excludeCategoryCodeCtrl = new FormControl('', [Validators.required, CustomValidators.number]); // 対象外区分コード
    this.excludeCategoryNameCtrl = new FormControl('');
  }

  public setValidator() {
    this.MyFormGroup = new FormGroup({

      kanaCtrl: this.kanaCtrl,  // カナ     

      excludeCategoryCodeCtrl: this.excludeCategoryCodeCtrl,  // 対象外区分コード
      excludeCategoryNameCtrl: this.excludeCategoryNameCtrl,

    })
  }

  public setFormatter() {
    FormatterUtil.setNumberFormatter(this.excludeCategoryCodeCtrl);
  }

  public clear() {
    this.MyFormGroup.reset();
    this.excludeCategoryId = null;
    this.kanaCtrl.enable();
    this.ComponentStatus = COMPONENT_STATUS_TYPE.CREATE;

    this.panelOpenState = false;
    this.panel.close();
  }

  public setAutoComplete() {
    this.initAutocompleteCategories(CategoryType.Exclude, this.excludeCategoryCodeCtrl, this.categoryService, 0);
  }

  /**
   * データ取得
   */
  public getKanaResult() {
    this.ignoreKanasResult = new IgnoreKanasResult();
    this.ignoreKanasResult.ignoreKanas = new Array<IgnoreKana>();

    let processComponentFactory = this.componentFactoryResolver.resolveComponentFactory(ModalRouterProgressComponent);
    let processComponentRef = this.viewContainerRef.createComponent(processComponentFactory);
    // processComponentRef.destroy();   

    this.ignoreKanaService.GetItems(new IgnoreKana())
      .subscribe(response => {
        if (response) {
          this.processCustomResult = this.processResultService.processAtGetData(
            this.processCustomResult, response, false, this.partsResultMessageComponent);
          if (this.processCustomResult.result == PROCESS_RESULT_RESULT_TYPE.SUCCESS) {
            this.ignoreKanasResult.ignoreKanas = response;
          }
        }
        processComponentRef.destroy();
      })
  }

  /**
   * 各テーブルのデータを取得する
   * @param keyCode イベント種別
   */
  public openMasterModal() {
    this.excludeCategoryCodeTrigger.closePanel();

    let componentFactory = this.componentFactoryResolver.resolveComponentFactory(ModalMasterComponent);
    let componentRef = this.viewContainerRef.createComponent(componentFactory);
    componentRef.instance.TableIndex = TABLE_INDEX.MASTER_EXCLUDE_CATEGORY;

    componentRef.instance.Closing.subscribe(() => {
      if (componentRef.instance.ModalStatus == MODAL_STATUS_TYPE.SELECT) {

        this.excludeCategoryId = componentRef.instance.SelectedObject.id;
        this.excludeCategoryCodeCtrl.setValue(componentRef.instance.SelectedCode);
        this.excludeCategoryNameCtrl.setValue(componentRef.instance.SelectedName);

      }
      componentRef.destroy();
    });
  }

  /**
   * 選択したデータをフォームに表示する
   * @param index 選択した行番号
   */
  public selectLine(index: number) {

    this.panelOpenState = true;
    this.panel.open();

    this.selectIndex = index;
    this.kanaCtrl.setValue(this.ignoreKanasResult.ignoreKanas[index].kana); // カナ
    this.excludeCategoryCodeCtrl.setValue(this.ignoreKanasResult.ignoreKanas[index].excludeCategoryCode); // 対象外区分コード
    this.excludeCategoryNameCtrl.setValue(this.ignoreKanasResult.ignoreKanas[index].excludeCategoryName);
    this.excludeCategoryId = this.ignoreKanasResult.ignoreKanas[index].excludeCategoryId;
    this.kanaCtrl.disable();

    this.ComponentStatus = COMPONENT_STATUS_TYPE.UPDATE;
    this.processResultService.clearProcessCustomMsg(this.processCustomResult);
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
   * データ登録・編集
   */
  public registry() {
    let registryData = new IgnoreKana();
    let isRegistry: boolean = false;

    if (this.ComponentStatus == COMPONENT_STATUS_TYPE.CREATE) {
      registryData.kana = this.kanaCtrl.value == null ? "" : this.kanaCtrl.value;
      isRegistry = true;

    } else if (this.ComponentStatus == COMPONENT_STATUS_TYPE.UPDATE) {
      registryData = this.ignoreKanasResult.ignoreKanas[this.selectIndex];

    } else {
      return;
    }

    let processComponentFactory = this.componentFactoryResolver.resolveComponentFactory(ModalRouterProgressComponent);
    let processComponentRef = this.viewContainerRef.createComponent(processComponentFactory);
    // processComponentRef.destroy();   

    registryData.excludeCategoryId = this.excludeCategoryId;

    this.ignoreKanaService.Save(registryData)
      .subscribe(result => {
        this.processCustomResult = this.processResultService.processAtSave(
          this.processCustomResult, result, isRegistry, this.partsResultMessageComponent);
        if (this.processCustomResult.result == PROCESS_RESULT_RESULT_TYPE.SUCCESS) {
          this.clear();
          this.getKanaResult();
        }
        processComponentRef.destroy();
      })
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

        let processComponentFactory = this.componentFactoryResolver.resolveComponentFactory(ModalRouterProgressComponent);
        let processComponentRef = this.viewContainerRef.createComponent(processComponentFactory);
        // processComponentRef.destroy();   

        this.ignoreKanaService.Delete(this.ignoreKanasResult.ignoreKanas[this.selectIndex])
          .subscribe(result => {
            this.processCustomResult = this.processResultService.processAtDelete(
              this.processCustomResult, result, this.partsResultMessageComponent);
            if (this.processCustomResult.result == PROCESS_RESULT_RESULT_TYPE.SUCCESS) {
              this.clear();
              this.getKanaResult();
            }
            processComponentRef.destroy();
          })
      }
      componentRef.destroy();
    });
  }

  /**
   * インポート
   */
  public openImportMethodSelector() {
    let componentFactory = this.componentFactoryResolver.resolveComponentFactory(ModalImportMethodSelectorComponent);
    let componentRef = this.viewContainerRef.createComponent(componentFactory);

    componentRef.instance.TableIndex = TABLES.MASTER_IGNORE_KANA.id;
    componentRef.instance.processModalCustomResult = this.processModalCustomResult;

    componentRef.instance.Closing.subscribe(() => {
      componentRef.destroy();
      this.getKanaResult();
    });
    this.openOptions();
  }

  /**
   * エクスポート
   */
  public export() {
    let headers = FileUtil.encloseItemBySymbol(REPORT_HEADER.IGNORE_KANA_MASTER);
    let data: string = headers.join(",") + LINE_FEED_CODE;
    let dataList = this.ignoreKanasResult.ignoreKanas;

    // 件数チェック
    if (dataList.length <= 0) {
      this.processCustomResult = this.processResultService.processAtWarning(
        this.processCustomResult, MSG_WNG.NO_EXPORT_DATA, this.partsResultMessageComponent);
      return;
    }

    let processComponentFactory = this.componentFactoryResolver.resolveComponentFactory(ModalRouterProgressComponent);
    let processComponentRef = this.viewContainerRef.createComponent(processComponentFactory);

    for (let index = 0; index < dataList.length; index++) {
      let dataItem: Array<any> = [];
      dataItem.push(this.userInfoService.Company.code);
      dataItem.push(dataList[index].kana);
      dataItem.push(dataList[index].excludeCategoryCode);
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


  /////  Enter キー押下時処理 //////////////////////////////////////////////////////////////
  public setKana(eventType: string) {
    this.kanaCtrl.setValue(EbDataHelper.convertToKanaHalf(this.kanaCtrl.value));
    HtmlUtil.nextFocusByName(this.elementRef, 'excludeCategoryCodeCtrl', eventType);
  }

  public setExcludeCategoryCode(eventType: string) {
    this.excludeCategoryId = null;
    if (eventType != EVENT_TYPE.BLUR) {
      this.excludeCategoryCodeTrigger.closePanel();
    }

    if (!StringUtil.IsNullOrEmpty(this.excludeCategoryCodeCtrl.value)) {

      this.loadStart();
      this.categoryService.GetItems(CategoryType.Exclude, this.excludeCategoryCodeCtrl.value)
        .subscribe(response => {
          this.loadEnd();
          if (response != PROCESS_RESULT_RESULT_TYPE.FAILURE && response.length > 0) {
            this.excludeCategoryId = response[0].id;
            this.excludeCategoryCodeCtrl.setValue(response[0].code);
            this.excludeCategoryNameCtrl.setValue(response[0].name);
            HtmlUtil.nextFocusByName(this.elementRef, 'kanaCtrl', eventType);
          }
          else {
            // this.excludeCategoryCodeCtrl.setValue("");
            this.excludeCategoryNameCtrl.setValue("");
          }
        });
    }
    else {
      this.excludeCategoryCodeCtrl.setValue("");
      this.excludeCategoryNameCtrl.setValue("");
      HtmlUtil.nextFocusByName(this.elementRef, 'kanaCtrl', eventType);
    }
  }

}
