import { Component, OnInit, ElementRef, ComponentFactoryResolver, ViewContainerRef, ComponentRef, AfterViewInit, ViewChild } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { forkJoin } from 'rxjs';
import { FILE_EXTENSION, MAX_FILE_SIZE } from 'src/app/common/const/eb-file.const';
import { ENCODE } from 'src/app/common/const/eb-file.const';
import { KEY_CODE, EVENT_TYPE, BUTTON_ACTION } from 'src/app/common/const/event.const';
import { CategoryType } from 'src/app/common/const/kbn.const';
import { LINE_FEED_CODE } from 'src/app/common/const/report-setting-kbn.const';
import { MODAL_STATUS_TYPE, PROCESS_RESULT_RESULT_TYPE } from 'src/app/common/const/status.const';
import { TABLE_INDEX } from 'src/app/common/const/table-name.const';
import { DateUtil } from 'src/app/common/util/date-util';
import { FileUtil } from 'src/app/common/util/file.util';
import { FormatterUtil } from 'src/app/common/util/formatter.util';
import { HtmlUtil } from 'src/app/common/util/html.util';
import { StringUtil } from 'src/app/common/util/string-util';
import { BaseComponent } from 'src/app/component/common/base/base-component';
import { ModalMasterComponent } from 'src/app/component/modal/modal-master/modal-master.component';
import { ModalRouterProgressComponent } from 'src/app/component/modal/modal-router-progress/modal-router-progress.component';
import { AccountTransferImportSource } from 'src/app/model/account-transfer-import-source.model';
import { AccountTransferImportResult } from 'src/app/model/account-transfer-import-result.model';
import { AccountTransferSource } from 'src/app/model/account-transfer-source.model';
import { Billing } from 'src/app/model/billing.model';
import { CategoriesResult } from 'src/app/model/categories-result.model';
import { PaymentAgency } from 'src/app/model/payment-agency.model';
import { PaymentFileFormatResult } from 'src/app/model/payment-file-format-result.model'
import { CategoryMasterService } from 'src/app/service/Master/category-master.service';
import { PaymentAgencyMasterService } from 'src/app/service/Master/payment-agency-master.service';
import { AccountTransferService } from 'src/app/service/account-transfer.service';
import { UserInfoService } from 'src/app/service/common/user-info.service';
import { Router, ActivatedRoute, NavigationEnd } from '@angular/router';
import { PageUtil } from 'src/app/common/util/page-util';
import { ProcessResultService } from 'src/app/service/common/process-result.service';
import { MSG_ERR, MSG_INF, MSG_WNG, MSG_ITEM_NUM } from 'src/app/common/const/message.const';
import { ModalWarningFileSizeComponent } from 'src/app/component/modal/modal-warning-file-size/modal-warning-file-size.component';
import { MatAutocompleteTrigger } from '@angular/material';
import { runInNewContext } from 'vm';

@Component({
  selector: 'app-pc0801-account-transfer-import',
  templateUrl: './pc0801-account-transfer-import.component.html',
  styleUrls: ['./pc0801-account-transfer-import.component.css']
})
export class Pc0801AccountTransferImportComponent extends BaseComponent implements OnInit,AfterViewInit {

  // これをしないとEnum型がテンプレートで見えなかったため
  public TABLE_INDEX: typeof TABLE_INDEX = TABLE_INDEX;

  public collectCategoriesResult: CategoriesResult;
  public paymentFileFormatResult: PaymentFileFormatResult;
  public accountTransferImportResult: AccountTransferImportResult;
  public accountTransferSources = new Array<AccountTransferSource>();

  public modalRouterProgressComponentRef: ComponentRef<ModalRouterProgressComponent>;
  public importFile: any; // インポートファイルのイベント

  public paymentAgencyCodeCtrl: FormControl; // 決済代行会社
  public paymentAgencyNameCtrl: FormControl;

  public fileFormatNameCtrl: FormControl; // フォーマット

  public importFileNameCtrl: FormControl; // 取込ファイル名

  public cbxConsiderUncollectedCtrl: FormControl; // 振替不可データの回収区分を自動更新する

  public cmbConsiderUncollectedCtrl: FormControl; // 振替不可データの回収区分

  public transferYearCtrl: FormControl;  // 引落年
  public paymentAgencyId: number;

  public collectCategoryId: number | null;

  public readCount: number;
  public validCount: number;
  public validAmount: number;
  public invalidCount: number;
  public invalidAmount: number;
  public invalidSourcesCount: number;

  public importDisableFlag: boolean;

  public undefineCtrl: FormControl; // 未定用;

  @ViewChild('paymentAgencyCodeInput', { read: MatAutocompleteTrigger }) paymentAgencyCodeigger: MatAutocompleteTrigger;

  constructor(
    public router: Router,
    public activatedRoute: ActivatedRoute,
    public elementRef: ElementRef,
    public componentFactoryResolver: ComponentFactoryResolver,
    public viewContainerRef: ViewContainerRef,
    public userInfoService: UserInfoService,
    public categoryService: CategoryMasterService,
    public accountTransferService: AccountTransferService,
    public paymentAgencyService: PaymentAgencyMasterService,
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
        this.processModalCustomResult = this.processResultService.processResultInit(this.ComponentId,this.Title,true);
      }
    });
  }

  ngOnInit() {
    this.setControlInit();
    this.setValidator();
    this.setFormatter();
    this.clear();
    this.setAutoComplete()

    let collectCategoryResponse = this.categoryService.GetItems(CategoryType.Collection);
    let fileFormatResponse = this.paymentAgencyService.GetFileFormat();

    forkJoin(
      collectCategoryResponse,
      fileFormatResponse,
    )
      .subscribe(
        responseList => {
          if (
                responseList != undefined 
            &&  responseList.length == 2
            &&  responseList[0] != PROCESS_RESULT_RESULT_TYPE.FAILURE
            &&  responseList[1] != PROCESS_RESULT_RESULT_TYPE.FAILURE
            ) {
            this.collectCategoriesResult = new CategoriesResult();
            this.paymentFileFormatResult = new PaymentFileFormatResult();
            this.collectCategoriesResult.categories = responseList[0];
            this.paymentFileFormatResult.paymentFileFormats = responseList[1];
          }
        });
  }

  ngAfterViewInit(){
    //AutoCompleteの準備が整っていない。
    //HtmlUtil.nextFocusByName(this.elementRef, 'paymentAgencyCodeCtrl', EVENT_TYPE.NONE);
  }

  public setControlInit() {

    this.paymentAgencyCodeCtrl = new FormControl("", [Validators.required, Validators.maxLength(2)]); // 決済代行会社
    this.paymentAgencyNameCtrl = new FormControl("");

    this.fileFormatNameCtrl = new FormControl(""); // フォーマット

    this.importFileNameCtrl = new FormControl("", [Validators.required]); // 取込ファイル名

    this.cbxConsiderUncollectedCtrl = new FormControl(""); // 振替不可データの回収区分を自動更新する

    this.cmbConsiderUncollectedCtrl = new FormControl(""); // 振替不可データの回収区分

    this.transferYearCtrl = new FormControl(""); // 引落年

    this.undefineCtrl = new FormControl(""); // 未定用;
  }

  public setValidator() {
    this.MyFormGroup = new FormGroup({

      paymentAgencyCodeCtrl: this.paymentAgencyCodeCtrl, // 決済代行会社
      paymentAgencyNameCtrl: this.paymentAgencyNameCtrl,

      fileFormatNameCtrl: this.fileFormatNameCtrl, // フォーマット

      importFileNameCtrl: this.importFileNameCtrl, // 取込ファイル名

      cbxConsiderUncollectedCtrl: this.cbxConsiderUncollectedCtrl, // 振替不可データの回収区分を自動更新する

      cmbConsiderUncollectedCtrl: this.cmbConsiderUncollectedCtrl, // 振替不可データの回収区分

      transferYearCtrl: this.transferYearCtrl, // 引落年

    });
  }

  public setFormatter() {

    FormatterUtil.setNumberFormatter(this.paymentAgencyCodeCtrl);
  }

  public clear() {
    this.MyFormGroup.reset();
    this.accountTransferSources = null;

    this.paymentAgencyCodeCtrl.enable();
    let now = new Date();
    this.transferYearCtrl.disable();
    this.transferYearCtrl.setValue(now.getFullYear());
    this.importFileNameCtrl.setValue("");

    this.importFile = null;

    this.readCount = 0;
    this.validCount = 0;
    this.validAmount = 0;
    this.invalidCount = 0;
    this.invalidAmount = 0;
    this.invalidSourcesCount = 0;
    this.cmbConsiderUncollectedCtrl.disable();

    this.importDisableFlag = true;

    //HtmlUtil.nextFocusByName(this.elementRef, 'paymentAgencyCodeCtrl', EVENT_TYPE.NONE);

  }

  public setAutoComplete(){

    // 決済手段コート
    this.initAutocompletePaymentAgencies(this.paymentAgencyCodeCtrl,this.paymentAgencyService,0);

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

      case BUTTON_ACTION.READ:
        this.read();
        break;
      case BUTTON_ACTION.IMPORT:
        this.import();
        break;


      default:
        console.log('buttonAction Error.');
        break;
    }
    
  }    

  /**
   * 読込・検証処理
   */
  public read() {

    this.processCustomResult = this.processResultService.clearProcessCustomMsg(this.processCustomResult);

    // 動作中のコンポーネントを開く
    let componentFactory = this.componentFactoryResolver.resolveComponentFactory(ModalRouterProgressComponent);
    this.modalRouterProgressComponentRef = this.viewContainerRef.createComponent(componentFactory);
    this.modalRouterProgressComponentRef.instance.Closing.subscribe(() => {
      this.modalRouterProgressComponentRef.destroy();
    });

    let reader = new FileReader();
    // CSV読込
    reader.readAsText(this.importFile, ENCODE[1].val);
    new Promise((resolve, reject) => {
      reader.onload = () => {
        let transactionImportSource = this.getAccountTransferImportSource();
        transactionImportSource.data = btoa(unescape(encodeURIComponent(reader.result.toString())));

        this.accountTransferService.Read(transactionImportSource)
          .subscribe(
            response => {
              if (response == PROCESS_RESULT_RESULT_TYPE.FAILURE) {
                this.processCustomResult = this.processResultService.processAtFailure(
                  this.processCustomResult, MSG_ERR.READING_ERROR, this.partsResultMessageComponent);
                this.importDisableFlag = true;
                this.modalRouterProgressComponentRef.destroy();
                return;
              }
              else {
                const result = response as AccountTransferImportResult;
                this.accountTransferImportResult = result;

                this.readCount = result.readCount;
                this.validCount = result.validCount;
                this.validAmount = result.validAmount;
                this.invalidCount = result.invalidCount;
                this.invalidAmount = result.invalidAmount;
                this.invalidSourcesCount = result.invalidSources.length;
                this.accountTransferSources = result.invalidSources;

                if (result.logs != undefined && result.logs.length > 0
                  && result.invalidSources.find(result => result.billings == null) != null) {
                  this.processCustomResult = this.processResultService.processAtFailure(
                    this.processCustomResult, MSG_ERR.READING_ERROR + "詳細はログファイルを確認してください。",
                    this.partsResultMessageComponent);

                  this.importDisableFlag = true;
                  this.downloadErrorLog(result.logs);
                }
                else {
                  this.processCustomResult = this.processResultService.processAtSuccess(
                    this.processCustomResult, MSG_INF.PROCESS_FINISH, this.partsResultMessageComponent);

                  this.paymentAgencyCodeCtrl.disable();
                  this.importDisableFlag = false;
                  this.processResultService.createdLog(this.processCustomResult.logData);
                }

                // 確認モーダルを閉じる
                this.modalRouterProgressComponentRef.destroy();
              }
            });
      };

      reader.onerror = () => {
        this.downloadErrorLog(reader.error);

        // 確認モーダルを閉じる
        this.modalRouterProgressComponentRef.destroy();
      };
    });
  }

  /**
   * 印刷
   */
  public print() {

    this.processCustomResult = this.processResultService.clearProcessCustomMsg(this.processCustomResult);

    // 動作中のコンポーネントを開く
    let componentFactory = this.componentFactoryResolver.resolveComponentFactory(ModalRouterProgressComponent);
    this.modalRouterProgressComponentRef = this.viewContainerRef.createComponent(componentFactory);
    this.modalRouterProgressComponentRef.instance.Closing.subscribe(() => {
      this.modalRouterProgressComponentRef.destroy();
    });

    let accountTransferImportSource = this.getAccountTransferImportSource();

    this.accountTransferService.GetReport(accountTransferImportSource)
      .subscribe(response => {
        FileUtil.download([response.body], this.fileFormatNameCtrl.value + DateUtil.getYYYYMMDD(0), FILE_EXTENSION.PDF);
        this.modalRouterProgressComponentRef.destroy();
      });

  }

  /**
   * 登録
   */
  public import() {

    this.processCustomResult = this.processResultService.clearProcessCustomMsg(this.processCustomResult);

    // 動作中のコンポーネントを開く
    let componentFactory = this.componentFactoryResolver.resolveComponentFactory(ModalRouterProgressComponent);
    this.modalRouterProgressComponentRef = this.viewContainerRef.createComponent(componentFactory);
    this.modalRouterProgressComponentRef.instance.Closing.subscribe(() => {
      this.modalRouterProgressComponentRef.destroy();
    });

    let accountTransferImportSource = this.getAccountTransferImportSource();

    this.accountTransferService.Import(accountTransferImportSource)
      .subscribe(response => {
        if (response == 500) {
          this.processCustomResult = this.processResultService.processAtFailure(
            this.processCustomResult, MSG_ERR.SAVE_ERROR, this.partsResultMessageComponent);
          this.modalRouterProgressComponentRef.destroy();
          return;
        }
        else {
          this.accountTransferImportResult = response;

          if (response.logs != undefined && response.logs.length > 0) {
            let msg = MSG_ERR.SAVE_ERROR;
            msg += '詳細はログファイルを確認してください。';
            this.processCustomResult = this.processResultService.processAtFailure(
              this.processCustomResult, msg, this.partsResultMessageComponent);

            this.downloadErrorLog(response.logs);
          }
          else if (response.logs == undefined || response.logs.length == 0) {
            this.processCustomResult = this.processResultService.processAtSuccess(
              this.processCustomResult, MSG_INF.SAVE_SUCCESS, this.partsResultMessageComponent);

              this.processResultService.createdLog(this.processCustomResult.logData);

          }
        }
      });

      this.importDisableFlag = true;

    // 確認モーダルを閉じる
    this.modalRouterProgressComponentRef.destroy();
  }

  public getAccountTransferImportSource(): AccountTransferImportSource {

    let accountTransferImportSource = new AccountTransferImportSource();
    accountTransferImportSource.companyId = this.userInfoService.Company.id;
    accountTransferImportSource.encodingCodePage = ENCODE[0].id;
    accountTransferImportSource.paymentAgencyId = this.paymentAgencyId;
    accountTransferImportSource.fileName = this.importFile.name;
    accountTransferImportSource.transferYear = this.transferYearCtrl.value;
    accountTransferImportSource.loginUserId = this.userInfoService.LoginUser.id;
    if (this.collectCategoryId != undefined) {
      accountTransferImportSource.newCollectCategoryId = this.collectCategoryId;
    }
    else {
      accountTransferImportSource.newCollectCategoryId = null;
    }
    if (this.accountTransferImportResult != undefined) {
      accountTransferImportSource.importDataId = this.accountTransferImportResult.importData.id;
    }

    return accountTransferImportSource;
  }

  /**
   * エラーログを出力する
   * @param error エラー
   */
  public downloadErrorLog(error: any) {
    let errorMsg = new Array<String>();
    errorMsg.push(DateUtil.getYYYYMMDD(3));
    errorMsg.push('口座振替結果データ：' + this.importFileNameCtrl.value);
    errorMsg = errorMsg.concat(error);

    let errorData = new Array<any>();
    errorData.push(errorMsg.join(LINE_FEED_CODE));

    FileUtil.download(errorData, DateUtil.getYYYYMMDD(0) + "_Import", FILE_EXTENSION.LOG);
  }

  public openMasterModal(table: TABLE_INDEX) {

    this.paymentAgencyCodeigger.closePanel();
      
    let componentFactory = this.componentFactoryResolver.resolveComponentFactory(ModalMasterComponent);
    let componentRef = this.viewContainerRef.createComponent(componentFactory);

    componentRef.instance.TableIndex = table;

    componentRef.instance.Closing.subscribe(() => {
      if (componentRef.instance.ModalStatus == MODAL_STATUS_TYPE.SELECT) {
        switch (table) {
          case TABLE_INDEX.MASTER_PAYMENT_AGENCY:
            {
              this.paymentAgencyCodeCtrl.setValue(componentRef.instance.SelectedCode);
              this.paymentAgencyNameCtrl.setValue(componentRef.instance.SelectedName);

              let paymentAgency: PaymentAgency = componentRef.instance.SelectedObject;
              let paymentFileFormat = this.paymentFileFormatResult.paymentFileFormats.find(format => format.id == paymentAgency.fileFormatId);
              this.paymentAgencyId = paymentAgency.id;
              this.fileFormatNameCtrl.setValue(paymentFileFormat.name);
              paymentFileFormat.isNeedYear == 1 ? this.transferYearCtrl.enable() : this.transferYearCtrl.disable();
              break;
            }
        }
      }
      componentRef.destroy();
    });

  }

  ///////////////////////////////////////////////////////////////////////
  public setPaymentAgencyCode(eventType: string) {

    if(eventType!=EVENT_TYPE.BLUR){
      this.paymentAgencyCodeigger.closePanel();
    }    

    if (!this.StringUtil.IsNullOrEmpty(this.paymentAgencyCodeCtrl.value)) {
      let code = StringUtil.setPaddingFrontZero(this.paymentAgencyCodeCtrl.value, 2);
      this.loadStart();
      this.paymentAgencyService.GetItems()
        .subscribe(response => {
          this.loadEnd();
          if (response != PROCESS_RESULT_RESULT_TYPE && response.length > 0) {
            let paymentAgencies: PaymentAgency[] = response;
            let paymentAgency = paymentAgencies.find(x => x.code == code);
            if (paymentAgency != undefined) {
              let paymentFileFormat = this.paymentFileFormatResult.paymentFileFormats.find(x => x.id == paymentAgency.fileFormatId);

              this.paymentAgencyCodeCtrl.setValue(paymentAgency.code);
              this.paymentAgencyNameCtrl.setValue(paymentAgency.name);
              this.fileFormatNameCtrl.setValue(paymentFileFormat.name);
              this.paymentAgencyId = paymentAgency.id;
              paymentFileFormat.isNeedYear == 1 ? this.transferYearCtrl.enable() : this.transferYearCtrl.disable();
              this.processResultService.clearProcessCustomMsg(this.processCustomResult);
              HtmlUtil.nextFocusByNames(this.elementRef, ['transferYearCtrl', 'importFileNameCtrl'], eventType);
            }
            else {
              let msg = MSG_WNG.MASTER_NOT_EXIST;
              msg = msg.replace(MSG_ITEM_NUM.FIRST, '決済手段');
              msg = msg.replace(MSG_ITEM_NUM.SECOND, code);
              this.processCustomResult = this.processResultService.processAtWarning(
                this.processCustomResult, msg, this.partsResultMessageComponent);
              this.paymentAgencyCodeCtrl.setValue("");
              this.paymentAgencyNameCtrl.setValue("");
              this.fileFormatNameCtrl.setValue("");
              this.paymentAgencyId = 0;
              this.transferYearCtrl.disable();
            }
            
          }
          else {
            this.paymentAgencyCodeCtrl.setValue("");
            this.paymentAgencyNameCtrl.setValue("");
            this.fileFormatNameCtrl.setValue("");
            this.paymentAgencyId = 0;
            this.transferYearCtrl.setValue("");
            this.transferYearCtrl.disable();
          }
        });

    }
    else {
      this.paymentAgencyCodeCtrl.setValue("");
      this.paymentAgencyNameCtrl.setValue("");
      this.fileFormatNameCtrl.setValue("");
      this.paymentAgencyId = 0;
      HtmlUtil.nextFocusByNames(this.elementRef, ['transferYearCtrl', 'importFileNameCtrl'], eventType);
    }
  }


  ////////////////////////////////////////////////////////////////////////
  public setCbxConsiderUncollected(eventType: string) {
    if (this.cbxConsiderUncollectedCtrl.value == true) {
      this.cmbConsiderUncollectedCtrl.enable();
      HtmlUtil.nextFocusByName(this.elementRef, 'cmbConsiderUncollectedCtrl', eventType);
    }
    else {
      this.collectCategoryId = null;
      this.cmbConsiderUncollectedCtrl.reset();
      this.cmbConsiderUncollectedCtrl.disable();
      HtmlUtil.nextFocusByName(this.elementRef, 'paymentAgencyCodeCtrl', eventType);
    }
  }

  ////////////////////////////////////////////////////////////////////////
  public setCmbConsiderUncollected(eventType: string) {
    if (!StringUtil.IsNullOrEmpty(this.cmbConsiderUncollectedCtrl.value)) {
      this.collectCategoryId = this.cmbConsiderUncollectedCtrl.value;
    }
  }

  public fileSelect(event: any) {
    this.processCustomResult = this.processResultService.clearProcessCustomMsg(this.processCustomResult);
    if (event.target.files.length == 0) return;
    if (event.target.files[0].size > MAX_FILE_SIZE) {
      let componentFactory = this.componentFactoryResolver.resolveComponentFactory(ModalWarningFileSizeComponent);
      let componentRef = this.viewContainerRef.createComponent(componentFactory);

      componentRef.instance.MainDetail = "ファイルサイズ超過";
      componentRef.instance.SufixDetail = "上限は" + Math.round(MAX_FILE_SIZE / 1024 / 1024) + "MBです。"

      componentRef.instance.Closing.subscribe(() => {
        componentRef.destroy();
      });
      this.importFile = null;
      this.importFileNameCtrl.setValue("");
    }
    else {
      this.importFile = event.target.files[0];
      this.importFileNameCtrl.setValue(event.target.files[0].name);
    }
  }

  public onDragOver(event: any) {
    event.preventDefault();
  }

  public onDrop(event: any) {
    this.processCustomResult = this.processResultService.clearProcessCustomMsg(this.processCustomResult);
    event.preventDefault();

    if (event.dataTransfer.files[0].size > MAX_FILE_SIZE) {
      let componentFactory = this.componentFactoryResolver.resolveComponentFactory(ModalWarningFileSizeComponent);
      let componentRef = this.viewContainerRef.createComponent(componentFactory);

      componentRef.instance.MainDetail = "ファイルサイズ超過";
      componentRef.instance.SufixDetail = "上限は" + Math.round(MAX_FILE_SIZE / 1024 / 1024) + "MBです。"

      componentRef.instance.Closing.subscribe(() => {
        componentRef.destroy();
      });
      this.importFile = null;
      this.importFileNameCtrl.setValue("");
    }
    else {
      this.importFile = event.dataTransfer.files[0];
      this.importFileNameCtrl.setValue(event.dataTransfer.files[0].name)
    }
  }

  public getTransferCustomerCode(billing: Billing): string {
    return billing == undefined ? "---" : billing[0].customerCode;
  }

  public getTransferCustomerName(billing: Billing): string {
    return billing == undefined ? "得意先不明" : billing[0].customerName;
  }

  public getTransferResult(resultCode: number): string {
    return resultCode == 0 ? "振替済" : "振替不能";
  }

}

