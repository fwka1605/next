import { Component, OnInit, ComponentFactoryResolver, ViewContainerRef, ElementRef } from '@angular/core';
import { FormBuilder, FormArray, FormControl, FormGroup, Validators } from '@angular/forms';
import { CdkDragDrop, moveItemInArray } from '@angular/cdk/drag-drop';
import { Observable } from 'rxjs';

import { BaseComponent } from 'src/app/component/common/base/base-component';
import { GridId, MATCHING_ASSIGNMENT_FLAG_DICTIONARY } from 'src/app/common/const/kbn.const';
import { HtmlUtil } from 'src/app/common/util/html.util';
import { FormatterUtil } from 'src/app/common/util/formatter.util';
import { StringUtil } from 'src/app/common/util/string-util';
import { GridSetting } from 'src/app/model/grid-setting.model';
import { GridSettingSearch } from 'src/app/model/grid-setting-search.model';
import { UserInfoService } from 'src/app/service/common/user-info.service';
import { GridSettingMasterService } from 'src/app/service/Master/grid-setting-master.service';
import { EVENT_TYPE, BUTTON_ACTION } from 'src/app/common/const/event.const';
import {ActivatedRoute, Router, NavigationEnd} from '@angular/router';
import { PageUtil } from 'src/app/common/util/page-util';
import { ProcessResultService } from 'src/app/service/common/process-result.service';
import { PROCESS_RESULT_RESULT_TYPE } from 'src/app/common/const/status.const';
import { MSG_INF, MSG_ERR } from 'src/app/common/const/message.const';
import { SortItem, SortOrder } from 'src/app/model/collation/sort-item';
import { ModalRouterProgressComponent } from 'src/app/component/modal/modal-router-progress/modal-router-progress.component';

@Component({
  selector: 'app-ph0701-grid-setting',
  templateUrl: './ph0701-grid-setting.component.html',
  styleUrls: ['./ph0701-grid-setting.component.css'],
})
export class Ph0701GridSettingComponent extends BaseComponent implements OnInit {

  public gridSettings: GridSetting[];

  public gridTypeCtrl:FormControl; // 画面
  public rdoOperatorTypeCtrl:FormControl;  // 更新対象

  public settingForms: FormArray;

  public sortItem: SortItem;

  constructor(
    public router: Router,
    public activatedRoute: ActivatedRoute,
    public elementRef: ElementRef,
    public componentFactoryResolver: ComponentFactoryResolver,
    public viewContainerRef: ViewContainerRef,
    public formBuilder: FormBuilder,
    public userInfoService: UserInfoService,
    public gridSettingService: GridSettingMasterService,
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

  ngOnInit() {
    this.setControlInit();
    this.setValidator();
    this.setFormatter();

    this.clear();
  }

  public setControlInit() {

    this.gridTypeCtrl = new FormControl('', [Validators.required]); // 画面種別
    this.rdoOperatorTypeCtrl = new FormControl('', [Validators.required]); // 更新対象

  }

  public setValidator() {
    this.settingForms = this.formBuilder.array([]);
    this.MyFormGroup = new FormGroup({
      // 区分種別
      gridTypeCtrl: this.gridTypeCtrl,
      // 更新対象
      rdoOperatorTypeCtrl: this.rdoOperatorTypeCtrl,
      // グリッド設定
      settingForms: this.settingForms,
    })
  }  

  public setFormatter() {

  }
  
  public clear() {
    this.MyFormGroup.reset();
    this.gridSettings = undefined;
    this.gridSource   = [];
    this.resetFormArray(this.settingForms);
    this.rdoOperatorTypeCtrl.setValue('0');
    this.sortItem     = undefined;

    HtmlUtil.nextFocusByName(this.elementRef, 'gridTypeCtrl', EVENT_TYPE.NONE);
    
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
      case BUTTON_ACTION.RESET:
        this.reset();
        break;

      default:
        console.log('buttonAction Error.');
        break;
    }
  }


  /**
   * 呼び出している GridSetting の登録
   */
  public registry() {

    let componentFactory = this.componentFactoryResolver.resolveComponentFactory(ModalRouterProgressComponent);
    let componentRef = this.viewContainerRef.createComponent(componentFactory);
    // componentRef.destroy();

    const source = this.gridSettings.map(x => {
      const columnName = x.columnName;
      const setting = this.settingForms.controls.filter(y => y.get('columnName').value === columnName)[0];
      if (setting !== undefined) {
        x.displayOrder = setting.get('displayOrder').value;
        x.displayWidth = setting.get('displayWidth').value;
      }
      else
      {
        x.displayOrder = 999;
        x.displayWidth = 0;
      }
      return x;
    });
    this.gridSettingService.save(source)
      .subscribe(response => {
        
        this.processCustomResult = this.processResultService.processAtSave(
          this.processCustomResult, response, true, this.partsResultMessageComponent);

        this.setGridType();
        componentRef.destroy();
      });
  }

  /**
   * グリッド表示設定を初期化
   */
  public reset() {
    this.setGridType(true);
  }

  public setGridType(isDefault: boolean = false){

    let componentFactory = this.componentFactoryResolver.resolveComponentFactory(ModalRouterProgressComponent);
    let componentRef = this.viewContainerRef.createComponent(componentFactory);
    // componentRef.destroy();

    this.resetFormArray(this.settingForms);

    const gridId = this.gridTypeCtrl.value as number;
    this.getGridSettings(gridId, isDefault)
      .subscribe(response => {

        if(response != PROCESS_RESULT_RESULT_TYPE.FAILURE){
          const settings: GridSetting[] = response;
          this.setGridSource(gridId);
          this.gridSettings = settings;
          
          settings
          .filter(x => this.filterDisplaySetting(x))
          .map(x => this.settingForms.push(this.getSettingFormGroup(x)));
          
          this.settingForms.controls.map((setting: FormGroup) => {
          const control = setting.get('displayWidth') as FormControl;
          FormatterUtil.setNumberFormatter(control);
          });
        }
        else{
          this.processResultService.processAtFailure(
            this.processCustomResult, MSG_ERR.POST_PROCESS_FAILURE, this.partsResultMessageComponent);
        }
        componentRef.destroy();
      });

  }

  public getGridSettings(gridId: number, isDefault: boolean = false): Observable<any> {
    const option: GridSettingSearch = {
      companyId:    this.userInfoService.Company.id,
      loginUserId:  this.userInfoService.LoginUser.id,
      gridId:       gridId,
      isDefault:    isDefault,
    };
    return this.gridSettingService.GetItemsBySearchObj(option);
  }

  public filterDisplaySetting(setting: GridSetting): boolean {
    switch (setting.gridId as GridId) {
      case GridId.BillingSearch: {
        if (!this.userInfoService.ApplicationControl.useForeignCurrency &&
          (setting.columnName === 'CurrencyCode')) {
          return false;
        }
        if (!this.userInfoService.ApplicationControl.useLongTermAdvanceReceived &&
          (setting.columnName === 'ContractNumber' ||
           setting.columnName === 'Confirm')) {
          return false;
        }
        if (!this.userInfoService.ApplicationControl.useDiscount &&
          (setting.columnName.startsWith('DiscountAmount'))) {
          return false;
        }
        if (!this.userInfoService.ApplicationControl.useAccountTransfer &&
          (setting.columnName === 'RequestDate' ||
           setting.columnName === 'ResultCode')) {
          return false;
        }
        break;
      }
      case GridId.ReceiptSearch: {
        if (!this.userInfoService.ApplicationControl.useForeignCurrency &&
          (setting.columnName === 'CurrencyCode')) {
          return false;
        }
        if (!this.userInfoService.ApplicationControl.useReceiptSection &&
          (setting.columnName === 'SectionCode' ||
           setting.columnName === 'SectionName')) {
          return false;
        }
        break;
      }
      case GridId.BillingMatchingIndividual: {
        if (!this.userInfoService.ApplicationControl.useDiscount &&
          (setting.columnName === 'DiscountAmountSummary')) {
          return false;
        }
        if (!this.userInfoService.ApplicationControl.useScheduledPayment &&
          (setting.columnName === 'ScheduledPaymentKey')) {
          return false;
        }
      }
      case GridId.ReceiptMatchingIndividual: {
        if (!this.userInfoService.ApplicationControl.useReceiptSection &&
          (setting.columnName === 'SectionCode' ||
           setting.columnName === 'SectionName')) {
          return false;
        }
      }
      case GridId.PaymentScheduleInput: {
        if (!this.userInfoService.ApplicationControl.useForeignCurrency &&
          (setting.columnName === 'CurrencyCode')) {
          return false;
        }
        if (!this.userInfoService.ApplicationControl.useDiscount &&
          (setting.columnName === 'DiscountAmountSummary')) {
          return false;
        }
      }
    }
    return true;
  }

  /**
   * FormArray の 中身をリセット
   * @param {FormArray} array 
   */
  public resetFormArray(array: FormArray) {
    for (let i = array.length - 1; i >= 0; i--) {
      array.removeAt(i);
    }
  }

  /**
   * GridSetting から FromGroup を作成し、返す
   * @param {GridSetting} setting 
   * @returns {FormGroup} gridId, dispayOrder, dipalyWidth, columnName, columnNameJp を所持する FormGroup を返す
   */
  public getSettingFormGroup(setting: GridSetting): FormGroup {
    return this.formBuilder.group({
      gridId:         [setting.gridId],
      displayOrder:   [setting.displayOrder],
      displayWidth:   [setting.displayWidth],
      columnName:     [setting.columnName],
      columnNameJp:   [setting.columnNameJp],
    })
  }

  /**
   * GridSetting での優先順位
   * @param {GridSetting} setting
   * @returns {number} 優先順位 1..999 特定の優先順位があるものは 999 以外の値が返る
   */
  public getGridSettingPriority(setting: GridSetting): number {
    switch (setting.gridId as GridId) {
      case GridId.ReceiptSearch: {
        if      (setting.columnName === 'ExcludeFlag') {
          return 1;
        }
        else if (setting.columnName === 'ExcludeCategory') {
          return 2;
        }
        break;
      }
      case GridId.BillingMatchingIndividual: {
        if      (setting.columnName === 'AssignmentFlag') {
          return 1;
        }
        else if (setting.columnName === 'CustomerCode') {
          return 2;
        }
        else if (setting.columnName === 'CustomerName') {
          return 3;
        }
        break;
      }
      case GridId.ReceiptMatchingIndividual: {
        if      (setting.columnName === 'AssignmentFlag') {
          return 1;
        }
        else if (setting.columnName === 'PayerName') {
          return 2;
        }
        break;
      }
      case GridId.PaymentScheduleInput: {
        if      (setting.columnName === 'UpdateFlag') {
          return 1;
        }
        break;
      }
      case GridId.BillingInvoicePublish: {
        if      (setting.columnName === 'Checked') {
          return 1;
        }
      }
      break;
    }
    return 999;
  }

  /**
   * 表示設定の 優先順位を取得
   * @param {FormGroup} group 
   * @return {number} 優先順位 1...999 特定の優先順位があるものは 999 以外の値が返る
   */
  public getGridSettingFormPriority(group: FormGroup): number {
    const setting       = new GridSetting();
    setting.gridId      = group.get('gridId').value;
    setting.columnName  = group.get('columnName').value;
    return this.getGridSettingPriority(setting);
  }

  /**
   * ソートボタンが必要かどうか
   * @param {GridSetting} setting
   */
  public requireSortButton(setting: GridSetting): boolean {
    const gridId = setting.gridId as GridId;
    return (
      gridId === GridId.BillingMatchingIndividual ||
      gridId === GridId.ReceiptMatchingIndividual
    ) && setting.columnName !== 'AssignmentFlag';
  }

  public requireSortMark(setting: GridSetting): boolean {
    return this.requireSortButton(setting) &&
      !StringUtil.IsNullOrEmpty(this.getSortMark(setting.columnName));
  }


  public setSort(columnName: string) {

    let item: SortItem = undefined;

    if (this.sortItem !== undefined &&
      this.sortItem.propertyName.toUpperCase() == columnName.toUpperCase()) {
      this.sortItem.sortOrder = this.sortItem.sortOrder == SortOrder.Ascending ? SortOrder.Descending : SortOrder.Ascending;
      item = this.sortItem;
    }
    else{
      item = new SortItem(columnName, SortOrder.Ascending);
    }

    this.sortItem = item;
    this.gridSource.sort((x, y) => (x[item.propertyName] < y[item.propertyName] ? -1 : 1) * (item.sortOrder == SortOrder.Descending ? -1 : 1));

  }

  public getSortMark(columnName: string) {
    if (this.sortItem !== undefined &&
        this.sortItem.propertyName.toUpperCase() == columnName.toUpperCase()) {
      return this.sortItem.sortOrder === SortOrder.Ascending ? '↑' : '↓';
    }

    return '';
  }


 //////////////////////////////////////////////////////////////////
 // 詳細
 //////////////////////////////////////////////////////////////////

 //////////////////////////////////////////////////////////////////
  public setItemWidth(eventType: any, index: number) {
    const setting       = this.settingForms.controls[index] as FormGroup;
    const columnName    = setting.get('columnName').value;
    const value         = setting.get('displayWidth').value;
    if (!StringUtil.IsNullOrEmpty(value)) {
      let width  = value as number;
      width = Math.ceil(width / 10) * 10;
      setting.get('displayWidth').setValue(width);
      const entity = this.gridSettings.filter(x => x.columnName === columnName)[0];
      if (entity !== undefined) {
        entity.displayWidth = width;
      }
      // HtmlUtil.nextFocusByName(this.elementRef, 'itemWidthCtrl' + index, eventType);
    }
    // else {
    //   HtmlUtil.nextFocusByName(this.elementRef, 'itemWidthCtrl' + (index+1), eventType);
    // }
  }

  /**
   * ドロップハンドラ
   * @param {CdkDragDrop<string[]>} event 
   */
  public onDrop(event: CdkDragDrop<string[]>) {
    moveItemInArray(this.settingForms.controls, event.previousIndex, event.currentIndex);
    moveItemInArray(this.settingForms.value,    event.previousIndex, event.currentIndex);

    // 強制的に 優先順位のあるものをソート
    this.settingForms.controls.sort((x: FormGroup, y: FormGroup) => this.getGridSettingFormPriority(x) - this.getGridSettingFormPriority(y));
    // displayOrder を 並び順に設定 1 から連番
    this.settingForms.controls.map((setting: FormGroup, index: number) => {
      const order = index + 1;
      setting.get('displayOrder').setValue(order);
      const columnName = setting.get('columnName').value;
      const entity = this.gridSettings.filter(x => x.columnName === columnName)[0];
      if (entity !== undefined) {
        entity.displayOrder = order;
      }
    });
    this.gridSettings.sort((x, y) => x.displayOrder - y.displayOrder);
  }

  gridSource = [];

  /**
   * 設定プレビューに表示する グリッドの source を設定
   * @param gridId 
   */
  public setGridSource(gridId: number) {
    this.sortItem = undefined;
    if (gridId == GridId.BillingSearch) {
      this.setBillingSearchSource();
    }
    if (gridId == GridId.ReceiptSearch) {
      this.setReceiptSearchSource();
    }
    if (gridId == GridId.BillingMatchingIndividual) {
      this.setBillingMatchingIndividualSource();
    }
    if (gridId == GridId.ReceiptMatchingIndividual) {
      this.setReceiptMatchingIndividualSource();
    }
    if (gridId == GridId.PaymentScheduleInput) {
      this.setPaymentScheduleInputSource();
    }
    if (gridId == GridId.BillingInvoicePublish) {
      this.setBillingInvoicePublishSource();
    }
  }

  public setBillingSearchSource() {
    this.gridSource = [{
      Id: 1,
      AssignmentState:  '未消込',
      CustomerCode:     '0000000001',
      CustomerName:     '△△△株式会社',
      BilledAt:         '2016/02/04',
      SalesAt:          '2016/02/04',
      ClosingAt:        '2016/02/29',
      DueAt:            '2016/03/31',
      CurrencyCode:     'JPY',
      BillingAmount:    5000,
      RemainAmount:     5000,
      InvoiceCode:      'TEST00001',
      BillingCategory:  '01:売上',
      CollectCategory:  '01:振込',
      InputType:        '1:取込',
      Memo:             '請求メモ1',
      DepartmentCode:   '1',
      DepartmentName:   '東京営業部',
      StaffCode:        '1',
      StaffName:        '営業太郎',
      ContractNumber:   '111',
      Confirm:          '未',
      RequestDate:      '2016/03/01',
      ResultCode:       '振替済',
      Note1:            '備考1',
      Note2:            '備考2',
      Note3:            '備考3',
      Note4:            '備考4',
      Note5:            '備考5',
      Note6:            '備考6',
      Note7:            '備考7',
      Note8:            '備考8',
      DiscountAmount1:  10,
      DiscountAmount2:  5,
      DiscountAmount3:  0,
      DiscountAmount4:  10,
      DiscountAmount5:  0,
      DiscountAmountSummary: 25,
      FirstRecordedAt:  '2016/03/31',
      LastRecordedAt:   '2016/04/30',
      Price:            4630,
      TaxAmount:        370,
    }, {
      Id:               2,
      AssignmentState:  '一部消込',
      CustomerCode:     '0000000002',
      CustomerName:     '□□株式会社',
      BilledAt:         '2016/03/05',
      SalesAt:          '2016/03/05',
      ClosingAt:        '2016/03/31',
      DueAt:            '2016/04/30',
      CurrencyCode:     'JPY',
      BillingAmount:    12000,
      RemainAmount:     3000,
      InvoiceCode:      'TEST00002',
      BillingCategory:  '01:売上',
      CollectCategory:  '01:振込',
      InputType:        '2:入力',
      Memo:             '請求メモ1',
      DepartmentCode:   '2',
      DepartmentName:   '関東営業第二',
      StaffCode:        '2',
      StaffName:        '営業次郎',
      ContractNumber:   '112',
      Confirm:          '済',
      RequestDate:      ' ',
      ResultCode:       ' ',
      Note1:            '備考1',
      Note2:            '備考2',
      Note3:            '備考3',
      Note4:            '備考4',
      Note5:            '備考5',
      Note6:            '備考6',
      Note7:            '備考7',
      Note8:            '備考8',
      DiscountAmount1:  20,
      DiscountAmount2:  8,
      DiscountAmount3:  0,
      DiscountAmount4:  20,
      DiscountAmount5:  0,
      DiscountAmountSummary: 48,
      FirstRecordedAt:  '2016/04/30',
      LastRecordedAt:   '2016/05/31',
      Price:            11111,
      TaxAmount:        889,
    }];
  }

  public setReceiptSearchSource() {
    this.gridSource = [{
      ExcludeFlag:          false,
      ExcludeCategory:      '',
      Id:                   1,
      AssignmentState:      '未消込',
      RecordedAt:           '2015/12/28',
      CustomerCode:         '0000000001',
      CustomerName:         '△△△株式会社',
      PayerName:            'ｶﾌﾞｼｷｶﾞｲｼﾔ1',
      CurrencyCode:         'JPY',
      ReceiptAmount:        15000,
      RemainAmount:         15000,
      ExcludeAmount:        0,
      ReceiptCategoryName:  '01:振込',
      InputType:            '1:EB取込',
      Note1:                '備考1',
      Memo:                 '入金メモ1',
      DueAt:                '',
      SectionCode:          1,
      SectionName:          '入金部門1',
      BankCode:             '1001',
      BankName:             'ﾃｽﾄｷﾞﾝｺｳ',
      BranchCode:           '101',
      BranchName:           'ﾃｽﾄｼﾃﾝ',
      AccountNumber:        '0123456',
      SourceBankName:       'ﾃｽﾄｷﾞﾝｺｳ',
      SourceBranchName:     'ﾃｽﾄｼﾃﾝ',
      VirtualBranchCode:    '123',
      VirtualAccountNumber: '4567890',
      OutputAt:             '',
      Note2:                '',
      Note3:                '',
      Note4:                '',
      BillNumber:           '',
      BillBankCode:         '',
      BillBranchCode:       '',
      BillDrawAt:           '',
      BillDrawer:           '',
    }, {
      ExcludeFlag:          true,
      ExcludeCategory:      '01:対象外',
      Id:                   2,
      AssignmentState:      '一部消込',
      RecordedAt:           '2016/01/05',
      CustomerCode:         '0000000002',
      CustomerName:         '□□株式会社',
      PayerName:            'ｶﾌﾞｼｷｶﾞｲｼﾔ2',
      CurrencyCode:         'JPY',
      ReceiptAmount:        3500,
      RemainAmount:         2500,
      ExcludeAmount:        1000,
      ReceiptCategoryName:  '03:期日現金',
      InputType:            '2:入力',
      Note1:                '備考1',
      Memo:                 '入金メモ2',
      DueAt:                '2016/02/10',
      SectionCode:          2,
      SectionName:          '入金部門2',
      BankCode:             '', 
      BankName:             '',
      BranchCode:           '',
      BranchName:           '',
      AccountNumber:        '',
      SourceBankName:       '',
      SourceBranchName:     '',
      VirtualBranchCode:    '',
      VirtualAccountNumber: '',
      OutputAt:             '2016/02/01 13:50:08',
      Note2:                '備考2',
      Note3:                '備考3',
      Note4:                '備考4',
      BillNumber:           '12345',
      BillBankCode:         '1234',
      BillBranchCode:       '123',
      BillDrawAt:           '2015/12/20',
      BillDrawer:           '株式会社○○',
    }];
  }

  public setBillingMatchingIndividualSource() {
    this.gridSource = [{
      AssignmentFlag:         true,
      CustomerCode:           '0000000001',
      CustomerName:           '△△△株式会社',
      BilledAt:               '2016/06/01',
      SalesAt:                '2016/06/01',
      DueAt:                  '2016/06/17',
      BillingAmount:          2500,
      RemainAmount:           2500,
      DiscountAmountSummary:  120,
      TargetAmount:           2500,
      MatchingAmount:         2500,
      InvoiceCode:            'TEST00001',
      BillingCategory:        '01:売上',
      DepartmentName:         '請求部門1',
      Note1:                  '備考1',
      Note2:                  '備考2',
      Note3:                  '備考3',
      Note4:                  '備考4',
      Note5:                  '備考5',
      Note6:                  '備考6',
      Note7:                  '備考7',
      Note8:                  '備考8',
      Memo:                   '請求メモ',
      InputType:              '入力',
      ScheduledPaymentKey:    'キー1',
    }, {
      AssignmentFlag:         false,
      CustomerCode:           '0000000002',
      CustomerName:           '□□株式会社',
      BilledAt:               '2016/06/01',
      SalesAt:                '2016/06/01',
      DueAt:                  '2016/06/17',
      BillingAmount:          1000,
      RemainAmount:           1000,
      DiscountAmountSummary:  230,
      TargetAmount:           1000,
      MatchingAmount:         1000,
      InvoiceCode:            'TEST00002',
      BillingCategory:        '01:売上',
      DepartmentName:         '請求部門1',
      Note1:                  '備考1',
      Note2:                  '備考2',
      Note3:                  '備考3',
      Note4:                  '備考4',
      Note5:                  '備考5',
      Note6:                  '備考6',
      Note7:                  '備考7',
      Note8:                  '備考8',
      Memo:                   '請求メモ',
      InputType:              '入力',
      ScheduledPaymentKey:    'キー2',
    }];
  }

  public setReceiptMatchingIndividualSource() {
    this.gridSource = [{
      AssignmentFlag:           true,
      PayerName:                'ｶﾌﾞｼｷｶﾞｲｼﾔ1',
      RecordedAt:               '2016/06/01',
      ReceiptCategoryName:      '振込',
      ReceiptAmount:            1500,
      RemainAmount:             1500,
      TargetAmount:             1500,
      NettingState:             '',
      SourceBank:               'ﾃｽﾄﾊﾞﾝｸ / ﾃｽﾄｼﾃﾝ',
      BankCode:                 '1001',
      BankName:                 'ﾃｽﾄｷﾞﾝｺｳ',
      BranchCode:               '101',
      BranchName:               'ﾃｽﾄｼﾃﾝ',
      AccountTypeName:          '普通',
      AccountNumber:            '0123456',
      SectionCode:              1,
      SectionName:              '入金部門1',
      Note1:                    '備考1',
      Memo:                     '入金メモ1 ',
      DueAt:                    ' ',
      ExcludeCategoryName:      ' ',
      VirtualBranchCode:        '123',
      VirtualAccountNumber:     '4567890',
      CustomerCode:             '0000000001',
      CustomerName:             '△△△株式会社',
      Note2:                    '備考2',
      Note3:                    '備考3',
      Note4:                    '備考4',
      BillNumber:               ' ',
      BillBankCode:             ' ',
      BillBranchCode:           ' ',
      BillDrawAt:               ' ',
      BillDrawer:               ' ',
      PayerNameRaw:             'ｶ)ﾃｽﾄｶｲｼﾔ',
      
    }, {
      AssignmentFlag:           false,
      PayerName:                'ｶﾌﾞｼｷｶﾞｲｼﾔ2',
      RecordedAt:               '2016/06/01',
      ReceiptCategoryName:      '小切手',
      ReceiptAmount:            150,
      RemainAmount:             150,
      TargetAmount:             150,
      NettingState:             '*',
      SourceBank:               '',
      BankCode:                 '',
      BankName:                 '',
      BranchCode:               '',
      BranchName:               '',
      AccountTypeName:          '',
      AccountNumber:            '',
      SectionCode:              2,
      SectionName:              '入金部門2',
      Note1:                    '備考1',
      Memo:                     '入金メモ2',
      DueAt:                    '2016/06/15',
      ExcludeCategoryName:      '01:対象外',
      VirtualBranchCode:        ' ',
      VirtualAccountNumber:     ' ',
      CustomerCode:             '0000000001',
      CustomerName:             '△△△株式会社',
      Note2:                    '備考2',
      Note3:                    '備考3',
      Note4:                    '備考4',
      BillNumber:               '12345',
      BillBankCode:             '1234',
      BillBranchCode:           '123',
      BillDrawAt:               '2015/12/20',
      BillDrawer:               '株式会社○○',
      PayerNameRaw:             'ｶ)ﾃｽﾄｶｲｼﾔ',
    }];
  }

  public setPaymentScheduleInputSource() {
    this.gridSource = [{
      UpdateFlag:             true,
      BillingId:              1,
      InvoiceCode:            'TEST00001',
      CustomerCode:           '0000000001',
      CustomerName:           '△△△株式会社',
      DepartmentCode:         '1',
      DepartmentName:         '東京営業部',
      CurrencyCode:           'JPY',
      BillingAmount:          5000,
      DiscountAmountSummary:  10,
      RemainAmount:           5000,
      PaymentAmount:          5000,
      OffsetAmount:           5000,
      BilledAt:               '2016/02/04',
      BillingDueAt:           '2016/03/31',
      BillingCategory:        '01:売上',
      ScheduledPaymentKey:    'キー1',
      SalesAt:                '2016/02/04',
      ClosingAt:              '2016/02/29',
      CollectCategory:        '01:振込',
      Note1:                  '備考1',
      Note2:                  '備考2',
      Note3:                  '備考3',
      Note4:                  '備考4',
      StaffCode:              '1',
      StaffName:              '営業太郎',
      InputType:              '1:取込',
      Memo:                   '請求メモ1',
    }, {
      UpdateFlag:             false,
      BillingId:              2,
      InvoiceCode:            'TEST00002',
      CustomerCode:           '0000000002',
      CustomerName:           '□□株式会社',
      DepartmentCode:         '2',
      DepartmentName:         '関東営業第二',
      CurrencyCode:           'JPY',
      BillingAmount:          12000,
      DiscountAmountSummary:  20,
      RemainAmount:           3000,
      PaymentAmount:          3000,
      OffsetAmount:           3000,
      BilledAt:               '2016/03/05',
      BillingDueAt:           '2016/04/30',
      BillingCategory:        '01:売上',
      ScheduledPaymentKey:    'キー2',
      SalesAt:                '2016/03/05',
      ClosingAt:              '2016/03/31',
      CollectCategory:        '01:振込',
      Note1:                  '備考1',
      Note2:                  '備考2',
      Note3:                  '備考3',
      Note4:                  '備考4',
      StaffCode:              '2',
      StaffName:              '営業次郎',
      InputType:              '2:入力',
      Memo:                   '請求メモ2',
    }];
  }

  public setBillingInvoicePublishSource() {
    this.gridSource = [{
      Checked:                    true,
      InvoiceTemplateId:          '銀行振込',
      InvoiceCode:                'TEST00001',
      DetailsCount:               '5',
      CustomerCode:               '0000000001',
      CustomerName:               '△△△株式会社',
      AmountSum:                  '5,000',
      RemainAmountSum:            '5,000',
      CollectCategoryCodeAndNeme: '01:振込',
      ClosingAt:                  '2016/02/29',
      BilledAt:                   '2016/02/04',
      DepartmentCode:             '1',
      DepartmentName:             '東京営業部',
      StaffCode:                  '1',
      StaffName:                  '営業太郎',
      DestnationCode:             '01',
      DestnationButton:           '…',
      DestnationContent:          '〒001-0001 東京都千代田区X-X-XX 〇〇ビル △△△株式会社御中',
      PublishAt:                  '2016/03/01',
      PublishAt1st:               '2016/03/01',
    }, {
      Checked:                    false,
      InvoiceTemplateId:          '銀行振込',
      InvoiceCode:                'TEST00002',
      DetailsCount:               '3',
      CustomerCode:               '0000000002',
      CustomerName:               '□□株式会社',
      AmountSum:                  '12,000',
      RemainAmountSum:            '3,000',
      CollectCategoryCodeAndNeme: '01:振込',
      ClosingAt:                  '2016/03/31',
      BilledAt:                   '2016/03/05',
      DepartmentCode:             '2',
      DepartmentName:             '関東営業第二',
      StaffCode:                  '2',
      StaffName:                  '営業太郎',
      DestnationCode:             '02',
      DestnationButton:           '…',
      DestnationContent:          '〒002-0002 東京都港区X-X-XX 〇〇ビル □□株式会社御中',
      PublishAt:                  '2016/04/20',
      PublishAt1st:               '2016/04/02',
    }];
  }

  public isNumber(value: any): boolean {
    return typeof value === 'number';
  }

  public isBoolean(value: any): boolean {
    return typeof value === 'boolean';
  }

  public isString(value: any): boolean {
    return typeof value === 'string';
  }

  public getAssingmentState(lineNo:number){
    let strRtn = "";
    let assignmentState = this.gridSource[lineNo]['AssignmentState']

    if(assignmentState==MATCHING_ASSIGNMENT_FLAG_DICTIONARY[0].val){
      strRtn = '<span class="tag--noAssignment">';
    }
    if(assignmentState==MATCHING_ASSIGNMENT_FLAG_DICTIONARY[1].val){
      strRtn = '<span class="tag--partAssignment">';
    }
    if(assignmentState==MATCHING_ASSIGNMENT_FLAG_DICTIONARY[2].val){
      strRtn = '<span class="tag--fullAssignment">';
    }
     return strRtn + assignmentState + "</span>";

  } 

}

