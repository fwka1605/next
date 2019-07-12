import {BankAccount} from './bank-account.model';
import {ProcessResult} from './process-result.model';
export class BankAccountsResult {
    public processResult: ProcessResult;
    public bankAccounts: BankAccount[];
}

