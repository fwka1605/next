import {AccountTransferLog} from './account-transfer-log.model';
import {ProcessResult} from './process-result.model';
export class AccountTransferLogsResult {
    public processResult: ProcessResult;
    public accountTransferLog: AccountTransferLog[];
}

