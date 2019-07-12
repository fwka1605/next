import { Component, OnInit, ViewChild, TemplateRef, AfterViewInit, ElementRef, ComponentFactoryResolver, ViewContainerRef, ComponentRef } from '@angular/core';
import { BaseComponent } from 'src/app/component/common/base/base-component';
import { MfAggregateWebApiClient } from 'src/app/model/mf-aggr/mf-aggregate-web-api-client';
import { StringUtil } from 'src/app/common/util/string-util';
import { WebApiSetting } from 'src/app/model/web-api-setting.model';
import { BankAccountType } from 'src/app/model/bank-account-type.model';
import { Tag } from 'src/app/model/mf-aggr/tag';
import { Account } from 'src/app/model/mf-aggr/accounts'
import { MfAggrTag } from 'src/app/model/mf-aggr-tag.model';
import { MfAggrAccount } from 'src/app/model/mf-aggr-account.model';
import { MfAggrSubAccount } from 'src/app/model/mf-aggr-sub-account.model'
import { SafeResourceUrl } from '@angular/platform-browser';
import { Router, ActivatedRoute, ParamMap, NavigationEnd } from '@angular/router';
import { UserInfoService } from 'src/app/service/common/user-info.service';
import { PageUtil } from 'src/app/common/util/page-util';
import { ProcessResultService } from 'src/app/service/common/process-result.service';
import { WebApiSettingMasterService } from 'src/app/service/Master/web-api-setting-master.service';
import { WebApiType, CategoryType, CODE_TYPE } from 'src/app/common/const/kbn.const';
import { FormControl, Validators, FormGroup } from '@angular/forms';
import { SectionMasterService } from 'src/app/service/Master/section-master.service';
import { SectionsResult } from 'src/app/model/sections-result.model';
import { CategoryMasterService } from 'src/app/service/Master/category-master.service';
import { CategoriesResult } from 'src/app/model/categories-result.model';
import { FormatterUtil } from 'src/app/common/util/formatter.util';
import { EVENT_TYPE, KEY_CODE } from 'src/app/common/const/event.const';
import { MatAutocompleteTrigger } from '@angular/material';
import { PROCESS_RESULT_RESULT_TYPE, MODAL_STATUS_TYPE } from 'src/app/common/const/status.const';
import { HtmlUtil } from 'src/app/common/util/html.util';
import { TABLE_INDEX } from 'src/app/common/const/table-name.const';
import { ModalMasterComponent } from 'src/app/component/modal/modal-master/modal-master.component';
import { SettingMasterService } from 'src/app/service/Master/setting-master.service'
import { MfAggrMasterService } from 'src/app/service/Master/mf-aggr-master.service'
import { Setting } from 'src/app/model/setting.model'
import { forkJoin, Subject } from 'rxjs';
import { LINE_FEED_CODE } from 'src/app/common/const/report-setting-kbn.const';
import { SubAccount } from 'src/app/model/mf-aggr/sub-account';
import { DatePipe } from '@angular/common';
import { BUTTON_ACTION } from 'src/app/common/const/event.const';
import { ModalRouterProgressComponent } from 'src/app/component/modal/modal-router-progress/modal-router-progress.component';
import { BankAccountTypeMasterService } from 'src/app/service/Master/bank-account-type-master.service';
import { MSG_ERR } from 'src/app/common/const/message.const';
import { Category } from 'src/app/model/category.model';
import { ComponentId } from 'src/app/common/const/component-name.const';


@Component({
  selector: 'app-ph1801-mf-aggr-web-api-setting',
  templateUrl: './ph1801-mf-aggr-web-api-setting.component.html',
  styleUrls: ['./ph1801-mf-aggr-web-api-setting.component.css']
})
export class Ph1801MfAggrWebApiSettingComponent extends BaseComponent implements OnInit, AfterViewInit {

  public moneyforwardSite = "https://statement.moneyforward.com/oauth/authorize?response_type=code&client_id=I7YY5kjVvG9s8KclAujCSr4jfhYd2KHfkz6gZLOKLco&redirect_uri=https://www.r-ac.co.jp&scope=read";
  public moneyforwardSafeUrl: SafeResourceUrl;

  public paramFrom: ComponentId;

  public modalRouterProgressComponentRef: ComponentRef<ModalRouterProgressComponent>;

  public receiptCategoryId: number;
  public selectedSectionId: number | null;

  public setting: WebApiSetting;

  public bankAccountTypes: Array<BankAccountType>;

  /// <summary>mf-aggr web api から取得した tag[]</summary>
  public tags = Array<Tag>();
  /// <summary>mf-aggr web api から取得した account[]</summary>
  public accounts = new Array<Account>();

  public VOneTags: Array<MfAggrTag>
  public VOneAccounts: Array<MfAggrAccount>

  public connectionStatusCtrl: FormControl;      // 連携状態
  public receiptCategoryCodeCtrl: FormControl;   // 入金区分
  public receiptCategoryNameCtrl: FormControl;   // 入金区分
  public sectionCodeCtrl: FormControl;           // 入金部門
  public sectionNameCtrl: FormControl;           // 入金部門
  public authorizationCodeCtrl: FormControl;    // 認証コード

  public testConnectionResultCtrl: FormControl;  //  テスト接続結果

  public sectionsResult: SectionsResult;

  public receiptCategoryResult: CategoriesResult;

  @ViewChild('sectionCodeInput', { read: MatAutocompleteTrigger }) sectionCodeTrigger: MatAutocompleteTrigger;
  @ViewChild('receiptCategoryCodeInput', { read: MatAutocompleteTrigger }) receiptCategoryCodeTrigger: MatAutocompleteTrigger;

  constructor(
    public router: Router,
    public activatedRoute: ActivatedRoute,
    public elementRef: ElementRef,
    public componentFactoryResolver: ComponentFactoryResolver,
    public viewContainerRef: ViewContainerRef,
    public userInfoService: UserInfoService,
    public processResultService: ProcessResultService,
    public webApiSettingService: WebApiSettingMasterService,
    public settingMasterService: SettingMasterService,
    public sectionService: SectionMasterService,
    public categoryService: CategoryMasterService,
    public mfAggrMasterService: MfAggrMasterService,
    public bankAccountTypeMasterService: BankAccountTypeMasterService,
    public client: MfAggregateWebApiClient,
    public datePipe: DatePipe,

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
    this.clear();
    this.setAutoComplete();

    let componentFactory = this.componentFactoryResolver.resolveComponentFactory(ModalRouterProgressComponent);
    this.modalRouterProgressComponentRef = this.viewContainerRef.createComponent(componentFactory);

    let webApiSettingResponse = this.webApiSettingService.GetByIdAsync(WebApiType.MfAggregation);
    let settingMasterResponse = this.settingMasterService.GetItems(["MfAggrKey"]);
    let voneTagsResponse = this.mfAggrMasterService.getTags();
    let voneAccountsResponse = this.mfAggrMasterService.getAccounts();
    let sectionMasterResponse = this.sectionService.GetItems();
    let receiptCategoryResponse = this.categoryService.GetItems(CategoryType.Receipt);
    let bankAccountTypeResponse = this.bankAccountTypeMasterService.GetItems();

    forkJoin(
      webApiSettingResponse,
      settingMasterResponse,
      voneTagsResponse,
      voneAccountsResponse,
      sectionMasterResponse,
      receiptCategoryResponse,
      bankAccountTypeResponse,
    )
      .subscribe(
        responseList => {
          if (responseList != undefined && responseList.length == 7) {
            this.loadWebApiSetting(responseList[0]);
            this.loadClientKey(responseList[1]);
            this.VOneTags = responseList[2];
            this.VOneAccounts = responseList[3];

            this.sectionsResult = new SectionsResult();
            this.sectionsResult.sections = responseList[4];

            this.receiptCategoryResult = new CategoriesResult();
            this.receiptCategoryResult.categories = responseList[5];

            this.bankAccountTypes = new Array<BankAccountType>();
            this.bankAccountTypes = responseList[6];

            this.getParamFrom();
            this.loadVOneDataAndSetInitialData();

            if (StringUtil.IsNullOrEmpty(this.client.AccessToken)) {
              this.connectionStatusCtrl.setValue("未連携");
              this.modalRouterProgressComponentRef.destroy();
            }
            else {
              this.loadMfMasterData();
              this.connectionStatusCtrl.setValue("連携中");

            }
            this.modalRouterProgressComponentRef.destroy();
          }
        });
  }


  public setControlInit() {

    this.connectionStatusCtrl = new FormControl("");  // 連携状態

    this.sectionCodeCtrl = new FormControl("", [Validators.required, Validators.maxLength(this.userInfoService.ApplicationControl.sectionCodeLength)]); // 入金部門コード
    this.sectionNameCtrl = new FormControl("");

    this.receiptCategoryCodeCtrl = new FormControl("", [Validators.required, Validators.maxLength(2)]); // 入金区分
    this.receiptCategoryNameCtrl = new FormControl("");

    this.authorizationCodeCtrl = new FormControl("", [Validators.required]);

    this.testConnectionResultCtrl = new FormControl("");

  }

  public setValidator() {
    this.MyFormGroup = new FormGroup({

      connectionStatusCtrl: this.connectionStatusCtrl, // 連携状態

      sectionCodeCtrl: this.sectionCodeCtrl, // 入金部門子コード
      sectionNameCtrl: this.sectionNameCtrl,

      receiptCategoryCodeCtrl: this.receiptCategoryCodeCtrl, // 入金区分
      receiptCategoryNameCtrl: this.receiptCategoryNameCtrl,

      authorizationCodeCtrl: this.authorizationCodeCtrl,  //  認証コード

      testConnectionResultCtrl: this.testConnectionResultCtrl,  //  テスト接続結果

    });

  }

  public setFormatter() {

    if (this.userInfoService.ApplicationControl.sectionCodeType == CODE_TYPE.NUMBER) {
      FormatterUtil.setNumberFormatter(this.sectionCodeCtrl); // 入金部門コード
    }
    else {
      FormatterUtil.setCodeFormatter(this.sectionCodeCtrl);
    }

    FormatterUtil.setNumberFormatter(this.receiptCategoryCodeCtrl); // 入金区分

  }

  public clear() {
    this.myFormGroup.reset();
  }

  public ngAfterViewInit() {
    HtmlUtil.nextFocusByName(this.elementRef, 'receiptCategoryCodeCtrl', EVENT_TYPE.NONE);
  }

  public setAutoComplete() {

    // 入金区分
    this.initAutocompleteCategoriesExcludeUseLimitDate(CategoryType.Receipt, this.receiptCategoryCodeCtrl, this.categoryService, 0);

    // 入金部門
    this.initAutocompleteSections(this.sectionCodeCtrl, this.sectionService, 0);

  }

  public openMasterModal(table: TABLE_INDEX) {

    //this.closeAutoCompletePanel();

    let componentFactory = this.componentFactoryResolver.resolveComponentFactory(ModalMasterComponent);
    let componentRef = this.viewContainerRef.createComponent(componentFactory);

    componentRef.instance.TableIndex = table;

    componentRef.instance.Closing.subscribe(() => {

      if (componentRef.instance.ModalStatus == MODAL_STATUS_TYPE.SELECT) {
        switch (table) {
          case TABLE_INDEX.MASTER_RECEIPT_CATEGORY_EXCLUDE_USE_LIMIT_DATE:
            {
              this.receiptCategoryCodeCtrl.setValue(componentRef.instance.SelectedCode);
              this.receiptCategoryNameCtrl.setValue(componentRef.instance.SelectedName);
              this.receiptCategoryId = componentRef.instance.SelectedId;

                break;
              }
            case TABLE_INDEX.MASTER_SECTION:
              {
                this.sectionCodeCtrl.setValue(componentRef.instance.SelectedCode);
                this.sectionNameCtrl.setValue(componentRef.instance.SelectedName);
                this.selectedSectionId = componentRef.instance.SelectedId;

              break;
            }

        }
      }

      componentRef.destroy();
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

      default:
        console.log('buttonAction Error.');
        break;
    }
  }

  //////////////////////////////////////////////////////////////
  public loadWebApiSetting(setting: WebApiSetting) {

    if (setting != undefined) {
      this.setting = setting;
    }
    else {
      this.setting = new WebApiSetting();
      this.setting.companyId = this.userInfoService.Company.id;
      this.setting.apiTypeId = WebApiType.MfAggregation;
      this.setting.baseUri = "";
      this.setting.extractSetting = "";
      this.setting.outputSetting = "";
      this.setting.createBy = this.userInfoService.LoginUser.id;
      this.setting.updateBy = this.userInfoService.LoginUser.id;
    }

    this.setTokenToClient();

  }

  //////////////////////////////////////////////////////////////
  public loadClientKey(settings: Setting[]) {
    
    const ids = ["MfAggrKey"];
    this.settingMasterService.GetItems(ids)
      .subscribe(response => {
        if (response == undefined) return;
        const settings: Setting[] = response;
        // const settingClientId = settings.find(x => x.itemKey == '0').itemValue;
        // const settingClientSecret = settings.find(x => x.itemKey == '1').itemValue;

        // 仮設定
        this.client.ClientId = "I7YY5kjVvG9s8KclAujCSr4jfhYd2KHfkz6gZLOKLco";
        this.client.ClientSecret = "8jHQt37jR1eI1Z9U2wwXAIZ77dOx2URc33HCCD33Qbw";
  
      });

  }

  //////////////////////////////////////////////////////////////
  public getParamFrom() {
    this.activatedRoute.paramMap.subscribe((params: ParamMap) => {
      this.paramFrom = parseInt(params.get('from'));
    });    
  }

  //////////////////////////////////////////////////////////////
  public loadVOneDataAndSetInitialData() {

    let subAccount: MfAggrSubAccount;
    if (this.VOneAccounts.length > 0) {
      subAccount = new MfAggrSubAccount();
      subAccount = this.VOneAccounts[0].subAccounts[0];

      if (subAccount != undefined) {
        const category = this.receiptCategoryResult.categories.find(x => x.id == subAccount.receiptCategoryId);
        if (category != undefined) {
          this.receiptCategoryCodeCtrl.setValue(category.code);
          this.receiptCategoryNameCtrl.setValue(category.name);
          this.receiptCategoryId = category.id;
        }
      }

    }

    if (this.userInfoService.ApplicationControl.useReceiptSection == 1 &&
        subAccount != undefined && subAccount.sectionId != undefined)
    {
      const section = this.sectionsResult.sections.find(x => x.id == subAccount.sectionId);
      this.sectionCodeCtrl.setValue(section.code);
      this.sectionNameCtrl.setValue(section.name);
      this.selectedSectionId = section.id;
    }

  }

  //////////////////////////////////////////////////////////////
  public loadMfMasterData(isRegister: boolean = false): Subject<boolean> {

    let loadMfMasterData: Subject<boolean> = new Subject();

    if (StringUtil.IsNullOrEmpty(this.client.AccessToken)) {
      this.endSubProcess("loadMfMasterData", false, loadMfMasterData);
    }
    else {

      this.tags.length = 0;
      this.accounts.length = 0;

      this.client.getTags()
        .subscribe(response => {
          if (response != undefined && response != PROCESS_RESULT_RESULT_TYPE.FAILURE) {

            const jsonTags = JSON.parse(JSON.stringify(response));
            for (let index = 0; index < jsonTags.tags.length; index++) {
              const item = jsonTags.tags[index] as Tag;
              const tag = new Tag();
              tag.id = item.id;
              tag.name = item.name;

              this.tags.push(tag);
            }

            this.client.getAccounts()
              .subscribe(result => {
                if (result != undefined && result != PROCESS_RESULT_RESULT_TYPE.FAILURE) {
                  const jsonAccounts = JSON.parse(JSON.stringify(result));

                  for (let index = 0; index < jsonAccounts.accounts.length; index++) {
                    const item = jsonAccounts.accounts[index] as Account;
                    const account = new Account();
                    account.id = item.id;
                    account.display_name = item.display_name;
                    account.last_aggregated_at = item.last_aggregated_at;
                    account.last_login_at = item.last_login_at;
                    account.last_succeeded_at = item.last_succeeded_at;
                    account.aggregation_start_date = item.aggregation_start_date;
                    account.status = item.status;
                    account.is_suspended = item.is_suspended;
                    account.bank_code = item.bank_code;

                    let subAccounts = new Array<SubAccount>();
                    for (let index = 0; index < item.sub_accounts.length; index++) {
                      const subItem = item.sub_accounts[index] as SubAccount;
                      const subAccount = new SubAccount();
                      subAccount.id = subItem.id;
                      subAccount.name = subItem.name;
                      subAccount.type = subItem.type;
                      subAccount.number = subItem.number;
                      subAccount.branch_code = subItem.branch_code;
                      subAccount.tags = subItem.tags;

                      subAccounts.push(subAccount);
                    }
                    account.sub_accounts = subAccounts;

                    this.accounts.push(account);
                  }

                if (!isRegister) {
                  this.displayMfMasterInformation();
                }
                this.endSubProcess("loadMfMasterData", true, loadMfMasterData);

              }
              else {
                this.processCustomResult = this.processResultService.processAtFailure(
                  this.processCustomResult, MSG_ERR.SAVE_ERROR, this.partsResultMessageComponent);                
                this.endSubProcess("loadMfMasterData", false, loadMfMasterData);
              }
            });

          }
          else {
            this.processCustomResult = this.processResultService.processAtFailure(
              this.processCustomResult, MSG_ERR.SAVE_ERROR, this.partsResultMessageComponent);            
            this.endSubProcess("loadMfMasterData", false, loadMfMasterData);
          }

        });

    }

    loadMfMasterData = new Subject();
    return loadMfMasterData;

  }

  public displayMfMasterInformation() {

    let dates = this.accounts.map(x => this.datePipe.transform(x.last_succeeded_at, 'yyyy/MM/dd HH:mm:ss'));
    let lastSucceededAt = dates.reduce((a, b) => a > b ? a : b);

    let subAccountcount = 0;
    this.accounts.forEach(x => {
      subAccountcount += x.sub_accounts.filter(y => !y.IsEmpty()).length;
    });

    let voneSubAccCounts = 0;
    if (this.VOneAccounts.length > 0) {
      this.VOneAccounts.map(x => voneSubAccCounts += x.subAccounts.length);
    }



    let masterInfo: string = LINE_FEED_CODE + "入金データ自動連携 マスター情報" + LINE_FEED_CODE;
    masterInfo += "タグ 件数：" + this.tags.length + LINE_FEED_CODE;
    masterInfo += "口座 件数：" + this.accounts.length + LINE_FEED_CODE;
    masterInfo += "サブアカウント 件数：" + subAccountcount + LINE_FEED_CODE;
    masterInfo += "最終連携日時：" + lastSucceededAt + LINE_FEED_CODE;
    masterInfo += LINE_FEED_CODE + "VONE マスター連携状況" + LINE_FEED_CODE;
    masterInfo += "タグ 登録件数：" + this.VOneTags.length + LINE_FEED_CODE;
    masterInfo += "口座 登録件数：" + this.VOneAccounts.length + LINE_FEED_CODE;
    masterInfo += "サブアカウント 登録件数：" + voneSubAccCounts + LINE_FEED_CODE;

    this.testConnectionResultCtrl.setValue(masterInfo);

  }

  //////////////////////////////////////////////////////////////
  public setTokenToClient() {

    if (this.setting == undefined ||
      StringUtil.IsNullOrEmpty(this.setting.accessToken) ||
      StringUtil.IsNullOrEmpty(this.setting.refreshToken)) return;

    this.client.AccessToken = this.setting.accessToken;
    this.client.RefreshToken = this.setting.refreshToken;
  }

  //////////////////////////////////////////////////////////////
  public setTokenToSetting() {
    this.setting.accessToken = this.client.AccessToken;
    this.setting.refreshToken = this.client.RefreshToken;
    this.setting.updateBy = this.userInfoService.LoginUser.id;
  }

  //////////////////////////////////////////////////////////////
  public saveMfAggrTags(): Subject<boolean> {

    let saveMfAggrTags: Subject<boolean> = new Subject();

    let tagModels = new Array<MfAggrTag>();
    for (let index = 0; index < this.tags.length; index++) {
      const tag = this.tags[index];
      tagModels.push(tag.GetModel());

    }

    this.mfAggrMasterService.saveTags(tagModels)
      .subscribe(response => {
        if (response != PROCESS_RESULT_RESULT_TYPE.FAILURE) {
          this.VOneTags = tagModels;
          this.endSubProcess("saveMfAggrTags", true, saveMfAggrTags);
        }
        else {
          this.processCustomResult = this.processResultService.processAtFailure(
            this.processCustomResult, MSG_ERR.SAVE_ERROR, this.partsResultMessageComponent);          
          this.endSubProcess("saveMfAggrTags", false, saveMfAggrTags);
        }

      });

    saveMfAggrTags = new Subject();
    return saveMfAggrTags;
  }

  //////////////////////////////////////////////////////////////
  public saveMfAggrAccounts(): Subject<boolean> {

    let saveMfAggrAccounts: Subject<boolean> = new Subject();

    let tagDic = new Map<string, number>();

    let groups = this.tags.reduce(function (obj, item) {
      obj[item.name] = obj[item.name] || [];
      obj[item.name].push(item.name);
      return obj;
    }, {});

    let names = Object.keys(groups).map(function (key) {
      return key;
    });

    names.forEach(name => {
      let id = this.tags.filter(x => x.name == name)[0].id;
      tagDic.set(name, id);
    });

    let accountModels = new Array<MfAggrAccount>();
    for (let index = 0; index < this.accounts.length; index++) {
      const mfAggrAccount = this.accounts[index].GetModel(tagDic, this.getAccountTypeId, this.receiptCategoryId, this.selectedSectionId);
      accountModels.push(mfAggrAccount);

    }

    let dbAccounts = new Array<MfAggrAccount>();
    let dbSubAccounts = new Array<MfAggrSubAccount>();
    this.mfAggrMasterService.getAccounts()
      .subscribe(response => {
        if (response != undefined) {
          dbAccounts = response;

          dbAccounts.forEach(item => {
            dbSubAccounts.concat(item.subAccounts);
          });

          accountModels.forEach(account => {
            let dbAccount = dbAccounts.find(x => x.id == account.id);

            if (StringUtil.IsNullOrEmpty(account.bankCode) && (dbAccount.bankCode != undefined && !StringUtil.IsNullOrEmpty(dbAccount.bankCode)) ) {
              account.bankCode = dbAccount.bankCode;
            }

            account.subAccounts.forEach(subAccount => {
              let dbSubAccount = dbSubAccounts.find(x => x.id == subAccount.id);
              if (dbSubAccount != undefined) {
                subAccount.receiptCategoryId = dbSubAccount.receiptCategoryId;
                subAccount.sectionId = dbSubAccount.sectionId;
                if (StringUtil.IsNullOrEmpty(subAccount.branchCode) && !StringUtil.IsNullOrEmpty(dbSubAccount.branchCode)) {
                  subAccount.branchCode = dbSubAccount.branchCode;
                }
              }

            });

          });  

        }
      });

    this.mfAggrMasterService.saveAccounts(accountModels)
      .subscribe(response => {
        if (response != PROCESS_RESULT_RESULT_TYPE.FAILURE) {
          this.VOneAccounts = accountModels;
          this.endSubProcess("saveMfAggrTags", true, saveMfAggrAccounts);
        }
        else {
          this.processCustomResult = this.processResultService.processAtFailure(
            this.processCustomResult, MSG_ERR.SAVE_ERROR, this.partsResultMessageComponent);
            this.endSubProcess("saveMfAggrTags", false, saveMfAggrAccounts);  
        }
      });

    saveMfAggrAccounts = new Subject();
    return saveMfAggrAccounts;
  }

  //////////////////////////////////////////////////////////////
  public setReceiptCategoryCode(eventType: string) {

    if (eventType != EVENT_TYPE.BLUR) {
      this.receiptCategoryCodeTrigger.closePanel();
    }

    if (!StringUtil.IsNullOrEmpty(this.receiptCategoryCodeCtrl.value)) {

      this.receiptCategoryCodeCtrl.setValue(StringUtil.setPaddingFrontZero(this.receiptCategoryCodeCtrl.value, 2));

      this.loadStart();
      this.categoryService.GetItems(CategoryType.Receipt, this.receiptCategoryCodeCtrl.value)
        .subscribe(response => {
          this.loadEnd();
          if (response != PROCESS_RESULT_RESULT_TYPE.FAILURE && response.length > 0) {

            const category = response[0] as Category;
            if (category.useLimitDate == 0) {
              this.receiptCategoryCodeCtrl.setValue(category.code);
              this.receiptCategoryNameCtrl.setValue(category.name);
              this.receiptCategoryId = category.id;
            }
            else {
              this.receiptCategoryNameCtrl.setValue("");
              this.receiptCategoryId = 0;
            }
          }
          else {
            //this.receiptCategoryCodeToCtrl.setValue("");
            this.receiptCategoryNameCtrl.setValue("");
            this.receiptCategoryId = 0;
          }
          HtmlUtil.nextFocusByName(this.elementRef, 'sectionCodeCtrl', eventType);
        });

    }
    else {
      this.receiptCategoryCodeCtrl.setValue("");
      this.receiptCategoryNameCtrl.setValue("");
      HtmlUtil.nextFocusByName(this.elementRef, 'sectionCodeCtrl', eventType);
    }

  }

  //////////////////////////////////////////////////////////////
  public setSectionCode(eventType: string) {

    if (eventType != EVENT_TYPE.BLUR) {
      this.sectionCodeTrigger.closePanel();
    }

    if (!StringUtil.IsNullOrEmpty(this.sectionCodeCtrl.value)) {

      this.loadStart();
      this.sectionService.GetItems(this.sectionCodeCtrl.value)
        .subscribe(response => {
          this.loadEnd();
          if (response != PROCESS_RESULT_RESULT_TYPE.FAILURE && response.length > 0) {

            this.sectionCodeCtrl.setValue(response[0].code);
            this.sectionNameCtrl.setValue(response[0].name);
            this.selectedSectionId = response[0].id;
          }
          else {
            this.sectionNameCtrl.setValue("");
            this.selectedSectionId = 0;
          }
          HtmlUtil.nextFocusByName(this.elementRef, 'receiptCategoryCodeCtrl', eventType);
        });

    }
    else {
      this.sectionCodeCtrl.setValue("");
      this.sectionNameCtrl.setValue("");
      HtmlUtil.nextFocusByName(this.elementRef, 'receiptCategoryCodeCtrl', eventType);
    }

  }

  public registry() {

    let componentFactory = this.componentFactoryResolver.resolveComponentFactory(ModalRouterProgressComponent);
    this.modalRouterProgressComponentRef = this.viewContainerRef.createComponent(componentFactory);

    let result: boolean;
    this.getRequestToken()
      .subscribe(response => {
        result = response;
        if (!result) return;

        this.saveApiSetting()
          .subscribe(response => {
            result = response;
            if (!result) return;

            this.loadMfMasterData(true)
              .subscribe(response => {
                result = response;
                if (!result) return;

                this.saveMfAggrTags()
                  .subscribe(response => {
                    result = response;
                    if (!result) return;

                    this.saveMfAggrAccounts()
                      .subscribe(response => {
                        result = response;
                        if (!result) return;
                        this.displayMfMasterInformation();
                        this.processCustomResult = this.processResultService.processAtSave(
                          this.processCustomResult, PROCESS_RESULT_RESULT_TYPE.SUCCESS, true, this.partsResultMessageComponent);
                        this.authorizationCodeCtrl.setValue("");
                      });

                  });

              });

          });
      });
  }

  /**
   * 認証コードを取得
   * 文字列の中から 'code=' より後の文字列を返す
   * 'code=' が含まれない場合、空文字を返す
   * @param value 
   */
  public getAuthorizationCode(value: string): string {
    const pattern = 'code=';
    let index = value.search(pattern);
    if (index == -1) {
      return '';
    }
    index += pattern.length;
    return value.substr(index);
  }

  public getRequestToken(): Subject<boolean> {

    let getRequestToken: Subject<boolean> = new Subject();

    let code = this.getAuthorizationCode(this.authorizationCodeCtrl.value);
    this.client.requestToken(code)
      .subscribe(response => {

        if (response != undefined) {
          this.client.AccessToken = response.access_token;
          this.client.RefreshToken = response.refresh_token;
          this.setTokenToSetting();
          this.endSubProcess("getRequestToken", true, getRequestToken);
        }
        else {
          this.processCustomResult = this.processResultService.processAtFailure(
            this.processCustomResult, MSG_ERR.SAVE_ERROR, this.partsResultMessageComponent);          
          this.endSubProcess("getRequestToken", false, getRequestToken);
        }

      });

    getRequestToken = new Subject();
    return getRequestToken;
  }

  public saveApiSetting(): Subject<boolean> {

    let saveApiSetting: Subject<boolean> = new Subject();
    this.webApiSettingService.SaveAsync(this.setting)
      .subscribe(response => {
        if (response != PROCESS_RESULT_RESULT_TYPE.FAILURE) {
          this.endSubProcess("saveApiSetting", true, saveApiSetting);
        }
        else {
          this.processCustomResult = this.processResultService.processAtFailure(
            this.processCustomResult, MSG_ERR.SAVE_ERROR, this.partsResultMessageComponent);
          this.endSubProcess("saveApiSetting", false, saveApiSetting);
        }

      });

    saveApiSetting = new Subject();
    return saveApiSetting;
  }

  //////////////////////////////////////////////////////////////
  public navigateUrl() {
    window.open(this.client.AuthorizationEndpointUri);
  }

  //////////////////////////////////////////////////////////////
  public navigateAuthUrl() {

    const authEndpointUri = this.client.AuthorizationEndpointUri;
    const clientId = this.client.ClientId
    const redirectUrl = this.client.RedirectUri;
    const url = `${authEndpointUri}/oauth/authorize?response_type=code&client_id=${clientId}&redirect_uri=${redirectUrl}&scope=read`

    window.open(url);
  }

  //////////////////////////////////////////////////////////////
  public endSubProcess(processName:string,endFlag:boolean,subject:Subject<boolean>){
    //console.log("endProcess:processName=" + processName + ",endFlag=" + endFlag);
    this.modalRouterProgressComponentRef.destroy();
    subject.next(endFlag);
    subject.complete();
    if (!endFlag) {
      this.modalRouterProgressComponentRef.destroy();
    }
  }

  //////////////////////////////////////////////////////////////
  public endSubProcessAny(processName:string,endObj:any,subject:Subject<any>){
    //console.log("endProcess:processName=" + processName + ",endFlag=" + endObj);
    this.modalRouterProgressComponentRef.destroy();
    subject.next(endObj);
    subject.complete();
    if (endObj == null) {
      this.modalRouterProgressComponentRef.destroy();
    }
  }

  //////////////////////////////////////////////////////////////
  public getAccountTypeId = (name: string) => {
    let bankAccountType = this.bankAccountTypes.find(x => name.startsWith(x.name));
    return bankAccountType == undefined ? null : bankAccountType.id;
  }

  //////////////////////////////////////////////////////////////
  public isDisableAccountDetailsButton(): boolean {
    return this.VOneAccounts == undefined || this.VOneAccounts.length == 0;
  }

  navigateAccountDetails() {
    if (this.paramFrom == ComponentId.PD1301) {
      this.router.navigate(['main/PH1802', { from: ComponentId.PD1301 }]);
    }
    else {
      this.router.navigate(['main/PH1802', { }]);
    }

  }

  //////////////////////////////////////////////////////////////
  public disableBack(): boolean {
    // PD1301 入金自動明細連携 データ抽出
    if (this.paramFrom == ComponentId.PD1301) return false;

    return true;
  }

  //////////////////////////////////////////////////////////////
  public back() {
    this.router.navigate(['main/PD1301']);
  }

}
