import { Amount } from './amount';
import { MfAggrTransaction } from "../mf-aggr-transaction.model";


/// <summary>
/// 取引明細
/// </summary>
export class Transaction {
  /// <summary>取引日時</summary>
  public date: Date;
  /// <summary>金額</summary>
  public amount: Amount;
  /// <summary>入出金履歴の内容</summary>
  public content: string;
  /// <summary>入出金履歴のID</summary>
  public id: number;
  /// <summary>口座のID</summary>
  public account_id: number;
  /// <summary>サブアカウントのID</summary>
  public sub_account_id: number;

  public created_at: Date;

  /// <summary>
  /// <see cref="MfAggrTransaction"/> model への変換 が必要
  /// </summary>
  /// <param name="companyId"></param>
  /// <param name="loginUserId"></param>
  /// <param name="currencyCodeToIdResolver">通貨コード -> ID 変換の処理</param>
  /// <param name="contentSolver">振込依頼人名の処理</param>
  /// <returns></returns>
  public GetModel(
    companyId: number,
    loginUserId: number,
    currencyCodeToIdResolver:(param: string) => number,
    contentSolver: (param: MfAggrTransaction) => MfAggrTransaction
  ): MfAggrTransaction {

    let model = new MfAggrTransaction();
    
    model.id = this.id;
    model.companyId = companyId;
    model.currencyId = currencyCodeToIdResolver(this.amount.currency);
    model.amount = this.amount.value;
    model.accountId = this.account_id;
    model.subAccountId = this.sub_account_id;
    model.content = this.content;
    model.recordedAt = '' + this.date;
    model.mfCreatedAt = '' + this.created_at;
    model.rate = this.amount.rate;
    model.convertedAmount = this.amount.converted_value;
    model.toCurrencyId = currencyCodeToIdResolver(this.amount.target_currency);
    model.createBy = loginUserId;

    if (model.IsIncome()) {
        contentSolver(model);
    }

    return model;

  }

}