import { Component, OnInit, ComponentFactoryResolver, ViewContainerRef } from '@angular/core';
import { FormBuilder, FormGroup, FormArray } from '@angular/forms';
import { CdkDragDrop, moveItemInArray } from '@angular/cdk/drag-drop';

import { BaseComponent } from 'src/app/component/common/base/base-component';
import { CategoryType } from 'src/app/common/const/kbn.const';
import { CollationSetting } from 'src/app/model/collation-setting.model';
import { MatchingOrder } from 'src/app/model/matching-order.model';
import { MatchingUtil } from 'src/app/common/util/matching-util';
import { CollationOrder } from 'src/app/model/collation-order.model';
import { UserInfoService } from 'src/app/service/common/user-info.service';
import { CollationSettingMasterService } from 'src/app/service/Master/collation-setting-master.service';
import { Router, ActivatedRoute, NavigationEnd } from '@angular/router';
import { PageUtil } from 'src/app/common/util/page-util';
import { ProcessResultService } from 'src/app/service/common/process-result.service';
import { BUTTON_ACTION } from 'src/app/common/const/event.const';
import { PROCESS_RESULT_RESULT_TYPE } from 'src/app/common/const/status.const';
import { ModalRouterProgressComponent } from 'src/app/component/modal/modal-router-progress/modal-router-progress.component';

@Component({
  selector: 'app-ph0801-collation-setting',
  templateUrl: './ph0801-collation-setting.component.html',
  styleUrls: ['./ph0801-collation-setting.component.css']
})
export class Ph0801CollationSettingComponent extends BaseComponent implements OnInit {

  constructor(
    public router: Router,
    public activatedRoute: ActivatedRoute,    
    public componentFactoryResolver: ComponentFactoryResolver,
    public viewContainerRef: ViewContainerRef,
    public formBuilder: FormBuilder,
    public userInfoService: UserInfoService,
    public collationSettingService:CollationSettingMasterService,
    public processResultService:ProcessResultService
  ) {
    super();

    router.events.subscribe(event => {
      if (event instanceof NavigationEnd) {
        this.Title = PageUtil.GetTitle(router.routerState, router.routerState.root).join('');
        this.ComponentId = parseInt(PageUtil.GetComponentId(router.routerState, router.routerState.root).join(''));
        const path:string[] = PageUtil.GetPath(this.activatedRoute.snapshot.pathFromRoot);
        this.Path = path[1];
        this.processCustomResult = this.processResultService.processResultInit(this.ComponentId,this.Title);
      }
    });    
  }


  public collationSetting: CollationSetting;

  public collationOrderForms: FormArray;
  public billingOrderForms: FormArray;
  public receiptOrderForms: FormArray;

  public sortOrders = [
    { id: 0, orderName: '昇順' },
    { id: 1, orderName: '降順' },
  ];
  public receiptRecordedAtItems = [
    { id: 0, itemName: '未入力'       },
    { id: 1, itemName: 'システム日付' },
    { id: 2, itemName: '請求日'       },
    { id: 3, itemName: '売上日'       },
    { id: 4, itemName: '請求締日'     },
    { id: 5, itemName: '入金予定日'   },
    { id: 6, itemName: '入金日'       },
  ];

  ngOnInit() {
    this.search();
  }

  public initFormGroup(setting: CollationSetting) {
    if (this.collationOrderForms == undefined) {
      this.collationOrderForms = this.formBuilder.array([]);
    }
    if (this.billingOrderForms == undefined) {
      this.billingOrderForms = this.formBuilder.array([]);
    }
    if (this.receiptOrderForms == undefined) {
      this.receiptOrderForms = this.formBuilder.array([]);
    }
    this.resetFormArray(this.collationOrderForms);
    setting.collationOrders.map(x => this.collationOrderForms.push(this.getCollationOrderFormGroup(x)));

    this.resetFormArray(this.billingOrderForms);
    setting.billingMatchingOrders.map(x => this.billingOrderForms.push(this.getMatchingOrderFormGroup(x)));

    this.resetFormArray(this.receiptOrderForms);
    setting.receiptMatchingOrders.map(x => this.receiptOrderForms.push(this.getMatchingOrderFormGroup(x)));

    this.MyFormGroup = this.formBuilder.group({
      // 得意先コードセットを必須にする
      requiredCustomer:                 [setting.requiredCustomer === 1],
      // 得意先コードを自動でセットする
      autoAssignCustomer:               [setting.autoAssignCustomer === 1],
      // 学習履歴機能を利用する
      learnKanaHistory:                 [setting.learnKanaHistory === 1],
      // 入金データ振分画面を使用する
      useApportionMenu:                 [setting.useApportionMenu === 1],
      // 自動更新を許可する
      reloadCollationData:              [setting.reloadCollationData === 1],
      // 前受振替を使用する
      useAdvanceReceived:               [setting.useAdvanceReceived === 1],
      // 前受伝票日付設定方法
      advanceReceivedRecordedDateType:  [setting.advanceReceivedRecordedDateType],
      // 一括消込チェックONのデータを消込実行させておく
      autoMatching:                     [setting.autoMatching === 1],
      // 一括消込チェックONのソートをあらかじめ実行しておく
      autoSortMatchingEnabledData:      [setting.autoSortMatchingEnabledData === 1],
      // 入金日・入金予定日の絞込開始日を指定する
      useFromToNarrowing:               [setting.useFromToNarrowing === 1],
      // 消込済データ表示時、消込日時にシステム日時を設定
      setSystemDateToCreateAtFilter:    [setting.setSystemDateToCreateAtFilter === 1],
      // 入金が複数件の場合は一括消込対象から外す
      prioritizeMatchingIndividuallyMultipleReceipts: [setting.prioritizeMatchingIndividuallyMultipleReceipts === 1],
      // 金額一致していても手数料自社負担の場合には、一括消込対象から外す
      forceShareTransferFee:            [setting.forceShareTransferFee === 1],
      // 入金データ修正時に得意先を付与した場合、学習履歴に登録する
      learnSpecifiedCustomerKana:       [setting.learnSpecifiedCustomerKana === 1],
      // 個別消込時の消込順序
      matchingSilentSortedData:         [setting.matchingSilentSortedData],
      // 消込時の請求情報・入金情報表示設定
      billingReceiptDisplayOrder:       [setting.billingReceiptDisplayOrder],
      // 入金データ取込 取込時スペースを除去
      removeSpaceFromPayerName:         [setting.removeSpaceFromPayerName === 1],
      // 差額が消費税誤差の範囲内でも一括消込対象外から外す（消費税誤差時に、個別消込優先）
      prioritizeMatchingIndividuallyTaxTolerance: [setting.prioritizeMatchingIndividuallyTaxTolerance === 1],
      // 仕訳出力内容設定 0:標準, 1:汎用
      journalizingPattern:              [setting.journalizingPattern],
      // 請求書単位で消費税計算を行う BillingInputId 単位で消費税計算
      calculateTaxByInputId:            [setting.calculateTaxByInputId === 1],
      // 一括消込入金情報ソート順カラム
      sortOrderColumn:                  [setting.sortOrderColumn],
      // 一括消込入金情報ソート順 0:昇順・1:降順
      sortOrder:                        [setting.sortOrder],
      // 照合処理 照合順序
      collationOrderForms:              this.collationOrderForms,
      // 消込処理 請求側実行順序
      billingOrderForms:                this.billingOrderForms,
      // 消込処理 入金側実行順序
      receiptOrderForms:                this.receiptOrderForms,
    });

    this.MyFormGroup.get('useAdvanceReceived').valueChanges
    .subscribe(checked => {
      if (checked) {
        this.MyFormGroup.get('advanceReceivedRecordedDateType').enable();
      }
      else {
        this.MyFormGroup.get('advanceReceivedRecordedDateType').disable();
      }
    });
  }

  public getCollationOrderFormGroup(order: CollationOrder): FormGroup {
    return this.formBuilder.group({
      executionOrder:     [order.executionOrder],
      available:          [order.available === 1],
      collationTypeId:    [order.collationTypeId],
    });
  }

  public getMatchingOrderFormGroup(order: MatchingOrder): FormGroup {
    return this.formBuilder.group({
      available:    [order.available],
      itemName:     [order.itemName],
      sortOrder:    [order.sortOrder],
    });
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
    this.processResultService.processResultStart(this.processCustomResult, action);
    if (this.processCustomResult.result == PROCESS_RESULT_RESULT_TYPE.FAILURE) {
      return;
    }

    switch (action) {
      case BUTTON_ACTION.REGISTRY:
        this.registry();
        break;
      case BUTTON_ACTION.SEARCH:
        this.search();
        break;

      default:
        console.log('buttonAction Error.');
        break;
    }
  }


  public search() {

    let componentFactory = this.componentFactoryResolver.resolveComponentFactory(ModalRouterProgressComponent);
    let componentRef = this.viewContainerRef.createComponent(componentFactory);
    // componentRef.destroy();

    this.collationSettingService.Get()
    .subscribe(response => {
      if ( response != PROCESS_RESULT_RESULT_TYPE.FAILURE ) {
        const setting = response as CollationSetting;
        this.setCollationName(setting);
        this.initFormGroup(setting);
        this.collationSetting = setting;

        //EBデータ取込時照合設定
        if(this.userInfoService.ApplicationControl.useReceiptSection==1){
          this.MyFormGroup.get('useApportionMenu').setValue(true);
          this.MyFormGroup.get('useApportionMenu').disable();

          this.MyFormGroup.get('requiredCustomer').enable();
          this.MyFormGroup.get('autoAssignCustomer').enable();
          this.MyFormGroup.get('learnKanaHistory').enable();
        }
        else if(this.MyFormGroup.get('useApportionMenu').value==1){
          this.MyFormGroup.get('useApportionMenu').setValue(true);
          this.MyFormGroup.get('useApportionMenu').enable();

          this.MyFormGroup.get('requiredCustomer').enable();
          this.MyFormGroup.get('autoAssignCustomer').enable();
          this.MyFormGroup.get('learnKanaHistory').enable();
        }
        else{
          this.MyFormGroup.get('useApportionMenu').setValue(null);
          this.MyFormGroup.get('useApportionMenu').enable();

          this.MyFormGroup.get('requiredCustomer').setValue(null);
          this.MyFormGroup.get('requiredCustomer').disable();
          this.MyFormGroup.get('autoAssignCustomer').setValue(null);
          this.MyFormGroup.get('autoAssignCustomer').disable();
          this.MyFormGroup.get('learnKanaHistory').setValue(null);
          this.MyFormGroup.get('learnKanaHistory').disable();         
        }

      }
      componentRef.destroy();
    });
  }


  public registry() {


    let componentFactory = this.componentFactoryResolver.resolveComponentFactory(ModalRouterProgressComponent);
    let componentRef = this.viewContainerRef.createComponent(componentFactory);
    componentRef.instance.Closing.subscribe(() => {
      componentRef.destroy();
    });   

    const saveSource = this.getSetting();
    this.collationSettingService.save(saveSource)
    .subscribe(response => {
      this.processCustomResult = this.processResultService.processAtSave(
        this.processCustomResult, response, true, this.partsResultMessageComponent);
      if ( response != PROCESS_RESULT_RESULT_TYPE.FAILURE ) {
        const setting = response as CollationSetting;
        this.setCollationName(setting);
        this.initFormGroup(setting);
        this.collationSetting = setting;
      }
      componentRef.destroy();
    });
  }

  public setCollationName(setting: CollationSetting) {
    const collations = setting.collationOrders;
    const billings   = setting.billingMatchingOrders;
    const receipts   = setting.receiptMatchingOrders;
    
    collations.map(x => x.collationTypeName = this.getCollationTypeName(x.collationTypeId));
    billings.map(x => x.itemNameJp = this.getBillingMatchingOrderName(x.itemName));
    receipts.map(x => x.itemNameJp = this.getReceiptMatchingOrderName(x.itemName));
    
    setting.collationOrders       = collations;
    setting.billingMatchingOrders = billings;
    setting.receiptMatchingOrders = receipts;
  }
  
  public getSetting(): CollationSetting {
    
    this.applyCollationOrderModels();
    this.applyBillingOrderModels();
    this.applyReceiptOrderModels();
  
    const setting = this.collationSetting;
    setting.requiredCustomer                                = this.MyFormGroup.get('requiredCustomer').value ? 1 : 0;
    setting.autoAssignCustomer                              = this.MyFormGroup.get('autoAssignCustomer').value ? 1 : 0;
    setting.learnKanaHistory                                = this.MyFormGroup.get('learnKanaHistory').value ? 1 : 0;
    setting.useApportionMenu                                = this.MyFormGroup.get('useApportionMenu').value ? 1 : 0;
    setting.reloadCollationData                             = this.MyFormGroup.get('reloadCollationData').value ? 1 : 0;
    setting.useAdvanceReceived                              = this.MyFormGroup.get('useAdvanceReceived').value ? 1 : 0;
    setting.advanceReceivedRecordedDateType                 = this.MyFormGroup.get('advanceReceivedRecordedDateType').value;
    setting.autoMatching                                    = this.MyFormGroup.get('autoMatching').value ? 1 : 0;
    setting.autoSortMatchingEnabledData                     = this.MyFormGroup.get('autoSortMatchingEnabledData').value ? 1 : 0;
    setting.useFromToNarrowing                              = this.MyFormGroup.get('useFromToNarrowing').value ? 1 : 0;
    setting.setSystemDateToCreateAtFilter                   = this.MyFormGroup.get('setSystemDateToCreateAtFilter').value ? 1 : 0;
    setting.prioritizeMatchingIndividuallyMultipleReceipts  = this.MyFormGroup.get('prioritizeMatchingIndividuallyMultipleReceipts').value ? 1 : 0;
    setting.forceShareTransferFee                           = this.MyFormGroup.get('forceShareTransferFee').value ? 1 : 0;
    setting.learnSpecifiedCustomerKana                      = this.MyFormGroup.get('learnSpecifiedCustomerKana').value ? 1 : 0;
    setting.matchingSilentSortedData                        = this.MyFormGroup.get('matchingSilentSortedData').value ? 1 : 0;
    setting.billingReceiptDisplayOrder                      = this.MyFormGroup.get('billingReceiptDisplayOrder').value ? 1 : 0;
    setting.removeSpaceFromPayerName                        = this.MyFormGroup.get('removeSpaceFromPayerName').value ? 1 : 0;
    setting.prioritizeMatchingIndividuallyTaxTolerance      = this.MyFormGroup.get('prioritizeMatchingIndividuallyTaxTolerance').value ? 1 : 0;
    setting.journalizingPattern                             = this.MyFormGroup.get('journalizingPattern').value;
    setting.calculateTaxByInputId                           = this.MyFormGroup.get('calculateTaxByInputId').value ? 1 : 0;
    setting.sortOrderColumn                                 = this.MyFormGroup.get('sortOrderColumn').value;
    setting.sortOrder                                       = this.MyFormGroup.get('sortOrder').value;

    return setting;
  }

  public getCollationTypeName(id: number): string {
    switch(id) {
      case 0: { return '専用入金口座照合'; }
      case 1: { return '得意先コード照合'; }
      case 2: { return '学習履歴照合';     }
      case 3: { return 'マスターカナ照合'; }
      case 4: { return '番号照合';         }
  }
  return undefined;
  }

  public getBillingMatchingOrderName(item: string): string {
    return MatchingUtil.getMatchingOrderName(item, CategoryType.Billing);
    // switch(item) {
    //   case 'BillingRemainSign':   {    return '請求残の正負';                 }
    //   case 'CashOnDueDatesFlag':  {    return '期日入金予定フラグ';           }
    //   case 'DueAt':               {    return  '入金予定日';                  }
    //   case 'CustomerCode':        {    return '得意先コード';                 }
    //   case 'BilledAt':            {    return '請求日';                       }
    //   case 'BillingRemainAmount': {    return '請求残（入金予定額）の絶対値'; }
    //   case 'BillingCategory':     {    return '請求区分';                     }
    // }
    // return undefined;
  }

  public getReceiptMatchingOrderName(item: string): string {
    return MatchingUtil.getMatchingOrderName(item, CategoryType.Receipt);
    // switch(item) {
    //   case 'NettingFlag':         { return '相殺データ';      }
    //   case 'ReceiptRemainSign':   { return '入金残の正負';    }
    //   case 'RecordedAt':          { return '入金日';          }
    //   case 'PayerName':           { return '振込依頼人名';    }
    //   case 'SourceBankName':      { return '仕向銀行';        }
    //   case 'SourceBranchName':    { return '仕向支店';        }
    //   case 'ReceiptRemainAmount': { return '入金残の絶対値';  }
    //   case 'ReceiptCategory':     { return '入金区分';        }
    // }
    // return undefined;
  }

  public applyCollationOrderModels() {
    let order = 0;
    const orders = this.collationSetting.collationOrders;
    for (let group of this.collationOrderForms.controls as FormGroup[]) {
      const typeId    = group.get('collationTypeId').value;
      const available = group.get('available').value;
      const model     = orders.filter(x => x.collationTypeId === typeId)[0];
      model.executionOrder  = ++order;
      model.available       = available ? 1 : 0;
      model.updateBy        = this.userInfoService.LoginUser.id;
    }
    this.collationSetting.collationOrders = orders.sort((x, y) => x.executionOrder - y.executionOrder);
  }

  public applyBillingOrderModels() {
    this.applyMatchingOrderModels(this.collationSetting.billingMatchingOrders, this.billingOrderForms);
  }

  public applyReceiptOrderModels() {
    this.applyMatchingOrderModels(this.collationSetting.receiptMatchingOrders, this.receiptOrderForms);
  }

  public applyMatchingOrderModels(models: MatchingOrder[], formArray: FormArray) {
    let order = 0;
    for (let group of formArray.controls as FormGroup[]) {
      const itemName  = group.get('itemName').value;
      const available = group.get('available').value;
      const sortOrder = group.get('sortOrder').value;
      const model     = models.filter(x => x.itemName === itemName)[0];
      model.executionOrder  = ++order;
      model.available       = available ? 1 : 0;
      model.sortOrder       = sortOrder;
      model.updateBy        = this.userInfoService.LoginUser.id;
    }
    models = models.sort((x, y) => x.executionOrder - y.executionOrder);
  }
  
  public onDropCollationOrder(event: CdkDragDrop<string[]>) {
    this.onDropInner(event, this.collationOrderForms);
    this.applyCollationOrderModels();
  }

  public onDropBillingOrder(event: CdkDragDrop<string[]>) {
    this.onDropInner(event, this.billingOrderForms);
    this.applyBillingOrderModels();
  }

  public onDropReceiptOrder(event: CdkDragDrop<string[]>) {
    this.onDropInner(event, this.receiptOrderForms);
    this.applyReceiptOrderModels();
  }

  public onDropInner(event: CdkDragDrop<string[]>, orders: FormArray) {
    moveItemInArray(orders.controls,  event.previousIndex, event.currentIndex);
    moveItemInArray(orders.value,     event.previousIndex, event.currentIndex);
  }

  public setCbxUseApportionMenu(){

    if(this.MyFormGroup.get('useApportionMenu').value==1){

      this.MyFormGroup.get('requiredCustomer').enable();
      this.MyFormGroup.get('autoAssignCustomer').enable();
      this.MyFormGroup.get('learnKanaHistory').enable();
    }
    else{

      this.MyFormGroup.get('requiredCustomer').setValue(null);
      this.MyFormGroup.get('requiredCustomer').disable();
      this.MyFormGroup.get('autoAssignCustomer').setValue(null);
      this.MyFormGroup.get('autoAssignCustomer').disable();
      this.MyFormGroup.get('learnKanaHistory').setValue(null);
      this.MyFormGroup.get('learnKanaHistory').disable();         
    }    
  }
}
