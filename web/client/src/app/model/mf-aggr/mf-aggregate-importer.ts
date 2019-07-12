import { Injectable } from '@angular/core';
import { forkJoin, Subject } from 'rxjs';
import { WebApiType } from 'src/app/common/const/kbn.const';
import { PROCESS_RESULT_RESULT_TYPE } from 'src/app/common/const/status.const';
import { StringUtil } from 'src/app/common/util/string-util';
import { DatePipe } from '@angular/common';
import { EbDataHelper } from 'src/app/common/util/eb-data-helper';
import { UserInfoService } from 'src/app/service/common/user-info.service';
import { CurrencyMasterService } from 'src/app/service/Master/currency-master.service';
import { CurrenciesResult } from 'src/app/model/currencies-result.model';
import { JuridicalPersonalityMasterService } from 'src/app/service/Master/juridical-personality-master.service';
import { JuridicalPersonalitysResult } from 'src/app/model/juridical-personalitys-result.model'
import { IgnoreKanaMasterService } from 'src/app/service/Master/ignore-kana-master.service'
import { IgnoreKanasResult } from 'src/app/model/ignore-kanas-result.model';
import { IgnoreKana } from 'src/app/model/ignore-kana.model';
import { BankAccountTypeMasterService } from 'src/app/service/Master/bank-account-type-master.service';
import { BankAccountType } from 'src/app/model/bank-account-type.model';
import { WebApiSettingMasterService } from 'src/app/service/Master/web-api-setting-master.service';
import { WebApiSetting } from 'src/app/model/web-api-setting.model';
import { MfAggrMasterService } from 'src/app/service/Master/mf-aggr-master.service';
import { MfAggrTransactionService } from 'src/app/service/mf-aggr-transaction.service';
import { MfAggregateWebApiClient } from 'src/app/model/mf-aggr/mf-aggregate-web-api-client';
import { Tag } from 'src/app/model/mf-aggr/tag';
import { Account } from 'src/app/model/mf-aggr/accounts';
import { SubAccount } from 'src/app/model/mf-aggr/sub-account';
import { MfAggrTag } from 'src/app/model/mf-aggr-tag.model';
import { MfAggrAccount } from 'src/app/model/mf-aggr-account.model';
import { MfAggrSubAccount } from 'src/app/model/mf-aggr-sub-account.model'
import { MfAggrTransactionSearch } from 'src/app/model/mf-aggr-transaction-search.model';
import { MfAggrTransaction } from 'src/app/model/mf-aggr-transaction.model';
import { Transaction } from 'src/app/model/mf-aggr/transaction';


@Injectable({
    providedIn: 'root'
})

export class MfAggregationImporter {

  public MF_AGGR_IMPORT_RESULT: typeof MF_AGGR_IMPORT_RESULT = MF_AGGR_IMPORT_RESULT;

  private importResult: MF_AGGR_IMPORT_RESULT;
  public get ImportResult() : MF_AGGR_IMPORT_RESULT {
    return this.importResult;
  }
  public set ImportResult(value: MF_AGGR_IMPORT_RESULT) {
    this.importResult = value;
  }

  private receiptCategoryId: number;
  public get ReceiptCategoryId(): number {
    return this.receiptCategoryId;
  }
  public set ReceiptCategoryId(value: number) {
    this.receiptCategoryId = value;
  }

  private sectionId: number;
  public get SectionId(): number {
    return this.sectionId;
  }
  public set SectionId(value: number) {
    this.sectionId = value;
  }

  private mfLastAggregatedAt: string;
  public get MfLastAggregatedAt(): string {
    return this.mfLastAggregatedAt;
  }
  public set MfLastAggregatedAt(value: string) {
    this.mfLastAggregatedAt = value;
  }

  private lastOneEntityCreatedAt: string;
  public get LastOneEntityCreatedAt(): string {
    return this.lastOneEntityCreatedAt;
  }
  public set LastOneEntityCreatedAt(value: string) {
    this.lastOneEntityCreatedAt = value;
  }

  private firstSubAccount: MfAggrSubAccount;
  public get FirstSubAccount(): MfAggrSubAccount {
    return this.firstSubAccount;
  }
  public set FirstSubAccount(value: MfAggrSubAccount) {
    this.firstSubAccount = value;
  }

  public get IsAccessTokenExist(): boolean {
    return !StringUtil.IsNullOrEmpty(this.client.AccessToken);
  }

  public get IsTokenRefresh(): boolean {
    return this.client.TokenRefreshed;
  }

  private isTokenValid: boolean;
  public get IsTokenValid(): boolean {
    return this.isTokenValid;
  }
  public set IsTokenValid(value: boolean) {
    this.isTokenValid = value;
  }

  private extractCount: number;
  public get ExtractCount(): number {
    return this.extractCount;
  }
  public set ExtractCount(value: number) {
    this.extractCount = value;
  }

  private extractInCount: number;
  public get ExtractInCount(): number { 
    return this.extractInCount;
  }
  public set ExtractInCount(value: number) {
    this.extractInCount = value;
  }

  private extractInAmount: number;
  public get ExtractInAmount(): number {
    return this.extractInAmount;
  }
  public set ExtractInAmount(value: number) {
    this.extractInAmount = value;
  }  

  private extractOutCount: number;
  public get ExtractOutCount(): number {
    return this.extractOutCount;
  }
  public set ExtractOutCount(value: number) {
    this.extractOutCount = value;
  }  

  private extractOutAmount: number;
  public get ExtractOutAmount(): number {
    return this.extractOutAmount;
  }
  public set ExtractOutAmount(value: number) {
    this.extractOutAmount = value;
  }  
  
  private registryCount: number;
  public get RegistryCount(): number {
    return this.registryCount;
  }
  public set RegistryCount(value: number) {
    this.registryCount = value;
  }  
  
  private registryAmount: number;
  public get RegistryAmount(): number {
    return this.registryAmount;
  }
  public set RegistryAmount(value: number) {
    this.registryAmount = value;
  }

  public tags = Array<Tag>();
  public accounts = new Array<Account>();
  public VOneTags: Array<MfAggrTag>
  public VOneAccounts: Array<MfAggrAccount>;
  public lastOneEntity: MfAggrTransaction;

  public bankAccountTypes: Array<BankAccountType>;
  public setting: WebApiSetting;

  public currenciesResult: CurrenciesResult;
  public juridicalPersonalitysResult: JuridicalPersonalitysResult;
  public ignoreKanasResult: IgnoreKanasResult;

  constructor(
    public userInfoService: UserInfoService,
    public currencyService:CurrencyMasterService,
    public juridicalPersonalityMasterService: JuridicalPersonalityMasterService,
    public ignoreKanaMasterService: IgnoreKanaMasterService,
    public bankAccountTypeMasterService: BankAccountTypeMasterService,
    public webApiSettingService: WebApiSettingMasterService,
    public mfAggrMasterService: MfAggrMasterService,
    public mfAggrTransactionService: MfAggrTransactionService,
    public client: MfAggregateWebApiClient,
    public datePipe: DatePipe,

  ) {}



  public initializeVOneMaster(): Subject<boolean> {

    let initializeVOneMaster: Subject<boolean> = new Subject();

    let currencyServiceResponse = this.currencyService.GetItems();
    let juridicalPersonalityResponse = this.juridicalPersonalityMasterService.GetItems();
    let ignoreKanaResponse = this.ignoreKanaMasterService.GetItems(new IgnoreKana());
    let bankAccountTypeResponse = this.bankAccountTypeMasterService.GetItems();
    let webApiSettingResponse = this.initializeWebApiSetting();
    let mfAggrAccountsResponse = this.mfAggrMasterService.getAccounts();
    let lastOneEntityResponse = this.mfAggrTransactionService.getLastOne(new MfAggrTransactionSearch());

    forkJoin(
      currencyServiceResponse,
      juridicalPersonalityResponse,
      ignoreKanaResponse,
      bankAccountTypeResponse,
      webApiSettingResponse,
      mfAggrAccountsResponse,
      lastOneEntityResponse
    )
      .subscribe(responseList => {
        if (responseList.length != 7
            ||  responseList[0] ==  PROCESS_RESULT_RESULT_TYPE.FAILURE
            ||  responseList[1] ==  PROCESS_RESULT_RESULT_TYPE.FAILURE
            ||  responseList[2] ==  PROCESS_RESULT_RESULT_TYPE.FAILURE
            ||  responseList[3] ==  PROCESS_RESULT_RESULT_TYPE.FAILURE
            ||  responseList[4] ==  false
            ||  responseList[5] ==  PROCESS_RESULT_RESULT_TYPE.FAILURE
            ||  responseList[6] ==  PROCESS_RESULT_RESULT_TYPE.FAILURE) {

              this.endSubProcess("saveApiSetting", false, initializeVOneMaster);
        }
        else {
          this.currenciesResult = new CurrenciesResult();
          this.currenciesResult.currencies = responseList[0];

          this.juridicalPersonalitysResult = new JuridicalPersonalitysResult();
          this.juridicalPersonalitysResult.juridicalPersonalities = responseList[1];

          this.ignoreKanasResult = new IgnoreKanasResult();
          this.ignoreKanasResult.ignoreKanas = responseList[2];

          this.bankAccountTypes = new Array<BankAccountType>();
          this.bankAccountTypes = responseList[3];

          // responseList[4];  //  WebApiSetting

          this.VOneAccounts = new Array<MfAggrAccount>();
          this.VOneAccounts = responseList[5];
          this.firstSubAccount = this.VOneAccounts[0].subAccounts[0];

          this.lastOneEntity = new MfAggrTransaction();
          this.lastOneEntity = responseList[6];

          this.endSubProcess("initializeVOneMaster", true, initializeVOneMaster);

        }

      });

      initializeVOneMaster = new Subject();
      return initializeVOneMaster;
  }

  //////////////////////////////////////////////////////////////
  public initializeWebApiSetting(): Subject<boolean> {

    let initializeWebApiSetting: Subject<boolean> = new Subject();

    this.webApiSettingService.GetByIdAsync(WebApiType.MfAggregation)
      .subscribe(response => {
        if (response != PROCESS_RESULT_RESULT_TYPE.FAILURE) {
          this.setting = response as WebApiSetting;
          this.setTokenToClient();
          this.endSubProcess("initializeVOneMaster", true, initializeWebApiSetting);
        }
        else {
          this.endSubProcess("initializeVOneMaster", false, initializeWebApiSetting);
        }

      });

    initializeWebApiSetting = new Subject();
    return initializeWebApiSetting;
  }

  //////////////////////////////////////////////////////////////
  public validateToken(): Subject<boolean> {
    
    let validateToken: Subject<boolean> = new Subject();

    this.isTokenValid = false;

    let settingResponse = this.initializeWebApiSetting();
    let loadTagResponse = this.loadTags();
    let persistLatestTokenResponse = this.persistLatestToken();

    forkJoin(
      settingResponse,
      loadTagResponse,
      persistLatestTokenResponse,
    )
      .subscribe(responseList => {

        if (responseList.length != 3 || responseList[0] == false || responseList[1] == false || responseList[2] == false) {
          this.importResult = MF_AGGR_IMPORT_RESULT.TokenExpired;
          this.endSubProcess("validateToken", false, validateToken);
        }
        else {
          if (!this.IsAccessTokenExist) {
            this.importResult = MF_AGGR_IMPORT_RESULT.NotAuthorized;
            this.endSubProcess("validateToken", false, validateToken);
          }
          else {
            this.isTokenValid = true;
            this.importResult = MF_AGGR_IMPORT_RESULT.Success;
            this.endSubProcess("validateToken", true, validateToken);
          }

        }
      });

    validateToken = new Subject();
    return validateToken;
  }

  //////////////////////////////////////////////////////////////
  public loadTags(): Subject<boolean> {

    let loadTags: Subject<boolean> = new Subject();
    
    this.tags.length = 0;

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

          this.endSubProcess("loadTags", true, loadTags);
        }
        else {
          this.endSubProcess("loadTags", false, loadTags);
        }
      });

    loadTags = new Subject();
    return loadTags;
  }

  //////////////////////////////////////////////////////////////
  public loadMfMaster(): Subject<boolean> {

    let loadMfMaster: Subject<boolean> = new Subject();

    this.loadMfMasterData()
      .subscribe(response => {
        if (response) {

          this.persistLatestToken()
            .subscribe(response => {
              if (response) {
                if (this.accounts.length > 0) {

                  const dates = this.accounts.map(x => this.datePipe.transform(x.last_succeeded_at, 'yyyy/MM/dd HH時mm分ss秒'));
                  this.mfLastAggregatedAt = dates.reduce((a, b) => a > b ? a : b);

                  if (this.lastOneEntity != undefined) {
                    this.lastOneEntityCreatedAt = this.datePipe.transform(this.lastOneEntity.createAt, 'yyyy/MM/dd HH時mm分ss秒');
                  }

                }

                this.endSubProcess("loadMfMaster", true, loadMfMaster);
              }
              else {
                this.endSubProcess("loadMfMaster", false, loadMfMaster);
              }

            });

        }
        else {

          this.endSubProcess("loadMfMaster", false, loadMfMaster);
        }

      });

    loadMfMaster = new Subject();
    return loadMfMaster;
  }

  //////////////////////////////////////////////////////////////
  public loadMfMasterData(): Subject<boolean> {

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

                this.endSubProcess("loadMfMasterData", true, loadMfMasterData);

              }
              else {
                this.endSubProcess("loadMfMasterData", false, loadMfMasterData);
              }
            });

          }
          else {
            this.endSubProcess("loadMfMasterData", false, loadMfMasterData);
          }

        });

    }    

    loadMfMasterData = new Subject();
    return loadMfMasterData;
  }

  //////////////////////////////////////////////////////////////
  public persistLatestToken(): Subject<boolean> {

    let persistLatestToken: Subject<boolean> = new Subject();

    if (!this.IsAccessTokenExist) {
      this.endSubProcess("persistLatestToken", false, persistLatestToken);
    }
    else {
      this.setTokenToSetting();
      this.webApiSettingService.SaveAsync(this.setting)
        .subscribe(response => {
          if (response != undefined || response != PROCESS_RESULT_RESULT_TYPE.FAILURE) {
            this.client.TokenRefreshed = false;
            this.endSubProcess("persistLatestToken", true, persistLatestToken);
          }
          else {
            this.endSubProcess("persistLatestToken", false, persistLatestToken);
          }
        });
    }

    persistLatestToken = new Subject();
    return persistLatestToken;
  }

  //////////////////////////////////////////////////////////////
  public saveMfAggrMaster(): Subject<boolean> {

    let saveMfAggrMaster: Subject<boolean> = new Subject();

    this.loadMfMaster()
      .subscribe(response => {
        if (!response) {
          this.importResult = MF_AGGR_IMPORT_RESULT.ApiFailure;
        }
        else {
          this.saveMfAggrMasterData()
            .subscribe(response => {
              if (response) {
                this.importResult = MF_AGGR_IMPORT_RESULT.Success;                
                this.endSubProcess("saveMfAggrMaster", true, saveMfAggrMaster);
              }
              else {
                this.importResult = MF_AGGR_IMPORT_RESULT.SaveMasterFailure;
                this.endSubProcess("saveMfAggrMaster", false, saveMfAggrMaster);
              }
            });
        }
      });

    saveMfAggrMaster = new Subject();
    return saveMfAggrMaster;
  }

  //////////////////////////////////////////////////////////////
  public saveMfAggrMasterData(): Subject<boolean> {

    let saveMfAggrMasterData: Subject<boolean> = new Subject();
    
    this.saveMfAggrTags()
      .subscribe(response => {
        if (response) {

          this.saveMfAggrAccounts()
            .subscribe(response => {
              if (response) {
                this.endSubProcess("saveMfAggrMasterData", true, saveMfAggrMasterData);
              }
              else {
                this.endSubProcess("saveMfAggrMasterData", false, saveMfAggrMasterData);    
              }
            });

        }
        else {
          this.endSubProcess("saveMfAggrMasterData", false, saveMfAggrMasterData);
        }

      });

    saveMfAggrMasterData = new Subject();
    return saveMfAggrMasterData;
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
      const mfAggrAccount = this.accounts[index].GetModel(tagDic, this.getAccountTypeId, this.receiptCategoryId, this.sectionId);
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
            this.endSubProcess("saveMfAggrTags", false, saveMfAggrAccounts);  
        }
      });

    saveMfAggrAccounts = new Subject();
    return saveMfAggrAccounts;
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

  }

  //////////////////////////////////////////////////////////////
  public getAccountTypeId = (name: string) => {
    let bankAccountType = this.bankAccountTypes.find(x => name.startsWith(x.name));
    return bankAccountType == undefined ? null : bankAccountType.id;
  }  

  //////////////////////////////////////////////////////////////
  public importMfAggrTransactions(
    extractDateFrom: string,
    extractDateTo: string,
    account_ids: number[] = null,
    sub_account_ids: number[] = null,
    tags: number[] = null
  ): Subject<boolean> {

    let importMfAggrTransactions: Subject<boolean> = new Subject();

    this.initializeSummary();

    this.saveMfAggrMaster()
      .subscribe(response => {
        if (response) {
          if (this.ImportResult != MF_AGGR_IMPORT_RESULT.Success) {
            this.endSubProcess("importMfAggrTransactions", false, importMfAggrTransactions);
            return;
          }

          this.mfAggrTransactionService.getIds()
            .subscribe(response => {
              if (response != undefined && response != PROCESS_RESULT_RESULT_TYPE.FAILURE) {

                let registeredIds = response as number[];
                this.client.getTransactions(extractDateFrom, extractDateTo, account_ids, sub_account_ids, tags)
                  .subscribe(response => {                    
                    const jsonTransactions = JSON.parse(JSON.stringify(response));

                    let items = jsonTransactions.transactions as Transaction[];
                    items = items.filter(x => !registeredIds.includes(x.id));

                    if (items.length == 0) {
                      this.importResult = MF_AGGR_IMPORT_RESULT.DataNotExist;
                      this.endSubProcess("importMfAggrTransactions", true, importMfAggrTransactions);
                      return;
                    }

                    this.extractCount = items.length;

                    let incomes = items.filter(x => x.amount.value > 0);
                    let outgoings = items.filter(x => !incomes.includes(x));

                    this.extractInCount = incomes.length;
                    this.extractInAmount = incomes.reduce((tis, org) => tis + org.amount.value, 0);
                    this.extractOutCount = outgoings.length;
                    this.extractOutAmount = outgoings.reduce((tis, org) => tis + (org.amount.value * -1), 0);

                    let mfAggrTransactions = new Array<MfAggrTransaction>();
                    for (let index = 0; index < items.length; index ++) {
                      const item = items[index] as Transaction;
                      const transaction = new Transaction();
                      transaction.date = item.date;
                      transaction.amount = item.amount;
                      transaction.content = item.content;
                      transaction.id = item.id;
                      transaction.account_id = item.account_id;
                      transaction.sub_account_id = item.sub_account_id;
                      transaction.created_at = item.created_at != undefined ? item.created_at : item.date ;
                      
                      const mfAggrTransaction = transaction.GetModel(
                        this.userInfoService.Company.id,
                        this.userInfoService.LoginUser.id,
                        this.resolveCurrencyCodeToId,
                        this.solveModelContent
                      );
                      mfAggrTransactions.push(mfAggrTransaction);
                    }

                    this.mfAggrTransactionService.save(mfAggrTransactions)
                      .subscribe(response => {
                        if (response != undefined && response != PROCESS_RESULT_RESULT_TYPE.FAILURE) {
                          this.registryCount = this.ExtractInCount;
                          this.registryAmount = this.ExtractInAmount;
                          
                          let mfAggrAccountsResponse = this.mfAggrMasterService.getAccounts();
                          let lastOneEntityResponse = this.mfAggrTransactionService.getLastOne(new MfAggrTransactionSearch());

                          forkJoin(
                            mfAggrAccountsResponse,
                            lastOneEntityResponse,
                          )
                            .subscribe(responseList => {
                              if (responseList.length != 2
                               || responseList[0] != PROCESS_RESULT_RESULT_TYPE.FAILURE
                               || responseList[1] != PROCESS_RESULT_RESULT_TYPE.FAILURE) {
                                 this.VOneAccounts = responseList[0];
                                 this.lastOneEntity = responseList[1];

                                 if (this.lastOneEntity != undefined) {
                                  this.lastOneEntityCreatedAt = this.datePipe.transform(this.lastOneEntity.createAt, 'yyyy/MM/dd HH時mm分ss秒');
                                }

                                 this.importResult = MF_AGGR_IMPORT_RESULT.Success;
                                 this.endSubProcess("importMfAggrTransactions", true, importMfAggrTransactions);
                               }
                               else {
                                this.endSubProcess("importMfAggrTransactions", false, importMfAggrTransactions);       
                               }
                              
                            });

                        }
                        else {
                          this.importResult = MF_AGGR_IMPORT_RESULT.SaveTransactionFailure;
                          this.endSubProcess("importMfAggrTransactions", false, importMfAggrTransactions);
                          return;
                        }

                      });


                  });

              }

            });

        }
        else {
          this.endSubProcess("importMfAggrTransactions", false, importMfAggrTransactions);
        }

      });

    importMfAggrTransactions = new Subject();
    return importMfAggrTransactions;

  }

  //////////////////////////////////////////////////////////////
  public initializeSummary() {

    this.extractCount     = 0;
    this.extractInCount   = 0;
    this.extractInAmount  = 0;
    this.extractOutCount  = 0;
    this.extractOutAmount = 0;
    this.registryCount    = 0;
    this.registryAmount   = 0;
  }

  //////////////////////////////////////////////////////////////
  public resolveCurrencyCodeToId = (code: string) => {

    const currency = this.currenciesResult.currencies.find(x => x.code == code);
    return  currency == undefined ? 0 : currency.id;
  }

  //////////////////////////////////////////////////////////////
  public solveModelContent = (model: MfAggrTransaction) => {
    model.payerCode = "";
    let content = model.content;
    content = EbDataHelper.convertToValidEbkana(content);
    const pattern = /[^ 0-9A-Z\uFF62-\uFF9F!""#$%&'()-=^~\\|@`[{;+:*\]},<.>/?_｡｢｣､･]/g;
    content = content.replace(pattern, '');
    model.payerNameRaw = content;
    model.payerName = EbDataHelper.removePersonalities(content, this.juridicalPersonalitysResult.juridicalPersonalities);
    const ignore = this.ignoreKanasResult.ignoreKanas.find(x => x.kana == model.payerNameRaw);
    model.excludeCategoryId = ignore != undefined ? ignore.excludeCategoryId : null;

    return model;
  }

  //////////////////////////////////////////////////////////////
  public endSubProcess(processName:string,endFlag:boolean,subject:Subject<boolean>){
    //console.log("endProcess:processName=" + processName + ",endFlag=" + endFlag);
    //this.modalRouterProgressComponentRef.destroy();
    subject.next(endFlag);
    subject.complete();
    // if (!endFlag) {
    //   this.modalRouterProgressComponentRef.destroy();
    // }
  }

  //////////////////////////////////////////////////////////////
  public endSubProcessAny(processName:string,endObj:any,subject:Subject<any>){
    //console.log("endProcess:processName=" + processName + ",endFlag=" + endObj);
    //this.modalRouterProgressComponentRef.destroy();
    subject.next(endObj);
    subject.complete();
    // if (endObj == null) {
    //   this.modalRouterProgressComponentRef.destroy();
    // }
  }

}

export enum MF_AGGR_IMPORT_RESULT
{
  /// <summary>成功</summary>
  Success = 0,
  /// <summary>取込対象のデータが存在せず</summary>
  DataNotExist = 1,
  /// <summary>認可処理 未実施 access_token が空</summary>
  NotAuthorized = 2,
  /// <summary>トークンの有効期限切れ（refresh_token）</summary>
  TokenExpired = 3,
  /// <summary>入金データ自動連携 API呼び出し失敗</summary>
  ApiFailure = 4,
  /// <summary>マスター登録処理に失敗</summary>
  SaveMasterFailure = 5,
  /// <summary>明細登録処理に失敗</summary>
  SaveTransactionFailure = 6,
}