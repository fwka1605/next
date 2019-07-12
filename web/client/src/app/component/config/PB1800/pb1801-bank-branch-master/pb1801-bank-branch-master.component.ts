import { Component, OnInit, ComponentFactoryResolver, ViewContainerRef, ElementRef } from '@angular/core';
import { BaseComponent } from 'src/app/component/common/base/base-component';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { HtmlUtil } from 'src/app/common/util/html.util';
import { KEY_CODE, BUTTON_ACTION } from 'src/app/common/const/event.const';
import { FormatterUtil } from 'src/app/common/util/formatter.util';
import { COMPONENT_STATUS_TYPE, MODAL_STATUS_TYPE, PROCESS_RESULT_RESULT_TYPE, PROCESS_RESULT_STATUS_TYPE } from 'src/app/common/const/status.const';
import { ModalMasterBankComponent } from 'src/app/component/modal/modal-master-bank/modal-master-bank.component';
import { BankBranch } from 'src/app/model/bank-branch.model';
import { ModalImportMethodSelectorComponent } from 'src/app/component/modal/modal-import-method-selector/modal-import-method-selector.component';
import { ModalConfirmComponent } from 'src/app/component/modal/modal-confirm/modal-confirm.component';
import { BankBranchMasterService } from 'src/app/service/Master/bank-branch-master.service';
import { DateUtil } from 'src/app/common/util/date-util';
import { UserInfoService } from 'src/app/service/common/user-info.service';
import { StringUtil } from 'src/app/common/util/string-util';
import { FILE_EXTENSION } from 'src/app/common/const/eb-file.const';
import { FileUtil } from 'src/app/common/util/file.util';
import { REPORT_HEADER, LINE_FEED_CODE } from 'src/app/common/const/report-setting-kbn.const';
import { TABLES } from 'src/app/common/const/table-name.const';
import { MSG_WNG } from 'src/app/common/const/message.const';
import { EbDataHelper } from 'src/app/common/util/eb-data-helper';
import { ProcessResultService } from 'src/app/service/common/process-result.service';
import { Router, ActivatedRoute, NavigationEnd } from '@angular/router';
import { PageUtil } from 'src/app/common/util/page-util';
import { BankBranchSearch } from 'src/app/model/bank-branch-search.model';
import { ModalRouterProgressComponent } from 'src/app/component/modal/modal-router-progress/modal-router-progress.component';
import { FunctionType } from 'src/app/common/const/kbn.const';

@Component({
  selector: 'app-pb1801-bank-branch-master',
  templateUrl: './pb1801-bank-branch-master.component.html',
  styleUrls: ['./pb1801-bank-branch-master.component.css']
})
export class Pb1801BankBranchMasterComponent extends BaseComponent implements OnInit {

  public FunctionType: typeof FunctionType = FunctionType;

  public bankBranch: BankBranch;

  public bankCodeCtrl: FormControl; // 銀行コード
  public bankNameCtrl: FormControl;
  public bankNameKanaCtrl: FormControl;

  public branchCodeCtrl: FormControl; // 支店コード
  public branchNameCtrl: FormControl;
  public branchNameKanaCtrl: FormControl;


  constructor(
    public router: Router,
    public activatedRoute: ActivatedRoute,
    public elementRef: ElementRef,
    public componentFactoryResolver: ComponentFactoryResolver,
    public viewContainerRef: ViewContainerRef,
    public userInfoService: UserInfoService,
    public bankBranchService: BankBranchMasterService,
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

    if (!this.userInfoService.isFunctionAvailable(FunctionType.MasterImport)
      && !this.userInfoService.isFunctionAvailable(FunctionType.MasterExport)) {
      this.securityHideShow = false;
    } else {
      this.securityHideShow = true;
    }
  }

  public setControlInit() {

    this.bankCodeCtrl = new FormControl('', [Validators.required, Validators.maxLength(4)]); // 銀行コード
    this.bankNameCtrl = new FormControl('', [Validators.required, Validators.maxLength(120)]);
    this.bankNameKanaCtrl = new FormControl('', [, Validators.maxLength(120)]);

    this.branchCodeCtrl = new FormControl('', [Validators.required, Validators.maxLength(3)]); // 支店コード
    this.branchNameCtrl = new FormControl('', [Validators.required, Validators.maxLength(120)]);
    this.branchNameKanaCtrl = new FormControl('', [, Validators.maxLength(120)]);
  }

  public setValidator() {
    this.MyFormGroup = new FormGroup({

      bankCodeCtrl: this.bankCodeCtrl,  // 銀行コード
      bankNameCtrl: this.bankNameCtrl,
      bankNameKanaCtrl: this.bankNameKanaCtrl,

      branchCodeCtrl: this.branchCodeCtrl,  // 支店コード
      branchNameCtrl: this.branchNameCtrl,
      branchNameKanaCtrl: this.branchNameKanaCtrl,

    })
  }

  public setFormatter() {
    FormatterUtil.setNumberFormatter(this.bankCodeCtrl);
    FormatterUtil.setNumberFormatter(this.branchCodeCtrl);
  }

  public setControlData() {
    this.bankCodeCtrl.setValue(this.bankBranch.bankCode); // 銀行コード
    this.bankNameCtrl.setValue(this.bankBranch.bankName);
    this.bankNameKanaCtrl.setValue(this.bankBranch.bankKana);

    this.branchCodeCtrl.setValue(this.bankBranch.branchCode); // 支店コード
    this.branchNameCtrl.setValue(this.bankBranch.branchName);
    this.branchNameKanaCtrl.setValue(this.bankBranch.branchKana);
  }

  public setObjectData() {
    this.bankBranch.bankCode = this.bankCodeCtrl.value; // 銀行コード
    this.bankBranch.bankName = this.bankNameCtrl.value;
    this.bankBranch.bankKana = this.bankNameKanaCtrl.value;

    this.bankBranch.branchCode = this.branchCodeCtrl.value; // 支店コード
    this.bankBranch.branchName = this.branchNameCtrl.value;
    this.bankBranch.branchKana = this.branchNameKanaCtrl.value;
  }

  public clear() {
    this.MyFormGroup.reset();
    this.ComponentStatus = COMPONENT_STATUS_TYPE.CREATE;
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
   * 登録
   */
  public registry() {
    let registryData = new BankBranch();
    let isRegistry: boolean = false;

    if (this.ComponentStatus == COMPONENT_STATUS_TYPE.CREATE) {
      registryData.createBy = this.userInfoService.LoginUser.id;
      isRegistry = true;
    }


    let processComponentFactory = this.componentFactoryResolver.resolveComponentFactory(ModalRouterProgressComponent);
    let processComponentRef = this.viewContainerRef.createComponent(processComponentFactory);

    registryData.bankCode = this.bankCodeCtrl.value;
    registryData.branchCode = this.branchCodeCtrl.value;
    registryData.bankName = this.bankNameCtrl.value;
    registryData.bankKana = this.bankNameKanaCtrl.value;
    registryData.branchName = this.branchNameCtrl.value;
    registryData.branchKana = this.branchNameKanaCtrl.value;

    this.bankBranchService.Save(registryData)
      .subscribe(result => {
        this.processCustomResult = this.processResultService.processAtSave(
          this.processCustomResult, result, isRegistry, this.partsResultMessageComponent);
        if (this.processCustomResult.result == PROCESS_RESULT_RESULT_TYPE.SUCCESS) {
          this.clear();
        }
        processComponentRef.destroy();
      })
  }

  /**
   * 削除
   */
  public delete() {
    let componentFactory = this.componentFactoryResolver.resolveComponentFactory(ModalConfirmComponent);
    let componentRef = this.viewContainerRef.createComponent(componentFactory);
    componentRef.instance.ActionName = "削除"
    componentRef.instance.Closing.subscribe(() => {

      this.setObjectData();
      if (componentRef.instance.ModalStatus == MODAL_STATUS_TYPE.OK) {

        let processComponentFactory = this.componentFactoryResolver.resolveComponentFactory(ModalRouterProgressComponent);
        let processComponentRef = this.viewContainerRef.createComponent(processComponentFactory);

        this.bankBranchService.Delete(this.bankBranch)
          .subscribe(result => {
            this.processCustomResult = this.processResultService.processAtDelete(
              this.processCustomResult, result, this.partsResultMessageComponent);
            if (this.processCustomResult.result == PROCESS_RESULT_RESULT_TYPE.SUCCESS) {
              this.clear();
            }
            processComponentRef.destroy();
          })
      }
      componentRef.destroy();
    });
  }

  /**
   * 各テーブルのデータをフォームに表示
   * @param keyCode キーイベント
   */
  public openBankMasterModal() {
    let componentFactory = this.componentFactoryResolver.resolveComponentFactory(ModalMasterBankComponent);
    let componentRef = this.viewContainerRef.createComponent(componentFactory);

    componentRef.instance.Closing.subscribe(() => {

      if (componentRef.instance.ModalStatus == MODAL_STATUS_TYPE.SELECT) {
        this.bankBranch = componentRef.instance.SelectedObject;
        this.ComponentStatus = COMPONENT_STATUS_TYPE.UPDATE;

        this.setControlData();
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

    componentRef.instance.TableIndex = TABLES.MASTER_BANK_BRANCH.id;
    componentRef.instance.processModalCustomResult = this.processModalCustomResult;

    componentRef.instance.Closing.subscribe(() => {
      componentRef.destroy();
      this.clear();
    });
    this.openOptions();
  }

  /**
   * エクスポート
   */
  public export() {

    // let headers = FileUtil.encloseItemBySymbol(REPORT_HEADER.BANK_BRANCH_MASTER);
    // let data: string = headers.join(",") + LINE_FEED_CODE;

    let processComponentFactory = this.componentFactoryResolver.resolveComponentFactory(ModalRouterProgressComponent);
    let processComponentRef = this.viewContainerRef.createComponent(processComponentFactory);

    let data: string = "";
    this.bankBranchService.GetItems()
      .subscribe(result => {

        for (let index = 0; index < result.length; index++) {
          let dataItem: Array<any> = [];
          dataItem.push(result[index].bankCode);
          dataItem.push(result[index].branchCode);
          dataItem.push(result[index].bankKana);
          dataItem.push(result[index].bankName);
          dataItem.push(result[index].branchKana);
          dataItem.push(result[index].branchName);
          dataItem = FileUtil.encloseItemBySymbol(dataItem);

          data = data + dataItem.join(",") + LINE_FEED_CODE;
        }
        let resultDatas: Array<any> = [];
        resultDatas.push(data);

        // 件数チェック
        if (resultDatas.length <= 0) {
          this.processCustomResult = this.processResultService.processAtWarning(
            this.processCustomResult, MSG_WNG.NO_EXPORT_DATA, this.partsResultMessageComponent);

        } else {
          try {
            FileUtil.download(resultDatas, this.Title + DateUtil.getYYYYMMDD(0), FILE_EXTENSION.CSV);
            result = true;

          } catch (error) {
            console.error(error);
          }

          this.processResultService.processAtOutput(
            this.processCustomResult, result, 0, this.partsResultMessageComponent);
          this.openOptions();
        }
        processComponentRef.destroy();
      })
  }


  ///// Enterキー押下時の処理 //////////////////////////////////////////////////////
  public setBankCode(eventType: string) {
    if (!StringUtil.IsNullOrEmpty(this.bankCodeCtrl.value)) {
      this.bankCodeCtrl.setValue(StringUtil.setPaddingFrontZero(this.bankCodeCtrl.value, 4, true));
      this.searchBankBranch();
    }
    HtmlUtil.nextFocusByName(this.elementRef, 'bankNameCtrl', eventType);
  }

  public setBankName(eventType: string) {
    HtmlUtil.nextFocusByName(this.elementRef, 'bankNameKanaCtrl', eventType);
  }

  public setBankNameKana(eventType: string) {
    HtmlUtil.nextFocusByName(this.elementRef, 'branchCodeCtrl', eventType);
  }

  public inputBankNameKana() {
    this.bankNameKanaCtrl.setValue(EbDataHelper.convertToKanaHalf(this.bankNameKanaCtrl.value));
  }

  public setBranchCode(eventType: string) {
    if (!StringUtil.IsNullOrEmpty(this.branchCodeCtrl.value)) {
      this.branchCodeCtrl.setValue(StringUtil.setPaddingFrontZero(this.branchCodeCtrl.value, 3, true));
      this.searchBankBranch();
    }
    HtmlUtil.nextFocusByName(this.elementRef, 'branchNameCtrl', eventType);
  }

  public setBranchName(eventType: string) {
    HtmlUtil.nextFocusByName(this.elementRef, 'branchNameKanaCtrl', eventType);
  }

  public setBranchNameKana(eventType: string) {
    HtmlUtil.nextFocusByName(this.elementRef, 'bankCodeCtrl', eventType);
  }

  public inputBranchNameKana() {
    this.branchNameKanaCtrl.setValue(EbDataHelper.convertToKanaHalf(this.branchNameKanaCtrl.value));
  }

  /**
   * 銀行IDの検索
   */
  public searchBankBranch() {
    if (!StringUtil.IsNullOrEmpty(this.bankCodeCtrl.value) && !StringUtil.IsNullOrEmpty(this.branchCodeCtrl.value)) {
      let bankBranchSearch = new BankBranchSearch()
      bankBranchSearch.bankCodes = [this.bankCodeCtrl.value];
      bankBranchSearch.branchCodes = [this.branchCodeCtrl.value];
      this.bankBranchService.GetItems(bankBranchSearch)
        .subscribe(response => {
          if (response != undefined && response != null && 0 < response.length) {
            this.bankBranch = response[0];
            this.setControlData();
            this.ComponentStatus = COMPONENT_STATUS_TYPE.UPDATE;
          }
        });
    }
  }
}
