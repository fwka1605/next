import { SubAccount } from "./sub-account";
import { MfAggrAccount } from "../mf-aggr-account.model";
import { MfAggrSubAccount } from '../mf-aggr-sub-account.model';
import { BankAccountType } from 'src/app/model/bank-account-type.model'
import { StringUtil } from "src/app/common/util/string-util";

/// <summary>
/// ユーザーの契約口座情報
/// </summary>
export class Account
{    
    /// <summary>口座のID ログインユーザーごとにユニーク</summary>
    public id:number;
    /// <summary>ユーザーが設定した口座の識別名 (未設定なら null)</summary>
    public display_name:string;
    /// <summary>口座の最終更新日時 ISO8601拡張形式 (YYYY-MM-DDTHH:mm:ss+zz:zz) </summary>
    //public last_aggregated_at:Date;
    public last_aggregated_at:string;
    /// <summary>口座の最終ログイン日時  ISO8601拡張形式 (YYYY-MM-DDTHH:mm:ss+zz:zz)  </summary>
    //public last_login_at:Date
    public last_login_at:string;
    /// <summary>口座の最終連携日時  ISO8601拡張形式 (YYYY-MM-DDTHH:mm:ss+zz:zz)  </summary>
    //public last_succeeded_at:Date
    public last_succeeded_at:string;
    /// <summary>口座のデータ取得開始日 ISO8601拡張形式 (YYYY-MM-DD) </summary>
    //public aggregation_start_date:Date;
    public aggregation_start_date:string;
    /// <summary>口座連携の状態 0: 成功, 1: 失敗</summary>
    public status:number;
    /// <summary>取得停止中: true, 取得中: false </summary>
    public is_suspended:boolean;
    /// <summary>銀行コード</summary>
    public bank_code:string;
    /// <summary>サブアカウント</summary>
    public sub_accounts:SubAccount[]


    /// <summary>
    /// MF の model から VONE 用 model への変換
    /// </summary>
    /// <param name="tagDictionary"></param>
    /// <param name="getAccountTypeId"></param>
    /// <param name="receiptCategoryId"></param>
    /// <param name="sectionId"></param>
    /// <returns></returns>
    public GetModel(
        tagDictionary:Map<string, number>,
        getAccountTypeId:(param: string) => any,        
        receiptCategoryId:number,
        sectionId:number = null): MfAggrAccount 
    {
        let model = new MfAggrAccount();
        
        model.id                      = this.id;
        model.displayName             = this.display_name;
        model.lastAggregatedAt        = this.last_aggregated_at;
        model.lastLoginAt             = this.last_login_at;
        model.lastSucceededAt         = this.last_succeeded_at;
        model.aggregationStartDate    = this.aggregation_start_date;
        model.status                  = this.status;
        model.isSuspended             = this.is_suspended ? 1 : 0;
        model.bankCode                = this.bank_code==null ? "":this.bank_code;
        
        let subAccounts = new Array<MfAggrSubAccount>();
        this.sub_accounts.forEach(item => {
            
          if (!item.IsEmpty()) {
            const subAccount = new MfAggrSubAccount();
            subAccount.id = item.id;
            subAccount.accountId = this.id;
            subAccount.name = StringUtil.IsNullOrEmpty(item.name) ? "" : item.name;
            subAccount.accountTypeName = item.type;
            subAccount.accountTypeId = getAccountTypeId(item.type);
            subAccount.accountNumber = StringUtil.IsNullOrEmpty(item.number) ? "" : item.number;
            subAccount.branchCode = StringUtil.IsNullOrEmpty(item.branch_code) ? "" : item.branch_code;
            subAccount.receiptCategoryId = receiptCategoryId;
            subAccount.sectionId = sectionId;
            subAccount.tagIds = item.tags.map(z => tagDictionary.get(z.toString()));

            subAccounts.push(subAccount);
          }

        });
        model.subAccounts = subAccounts;
        
        return model;
    }

}