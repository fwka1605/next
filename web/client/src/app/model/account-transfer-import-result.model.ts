import {AccountTransferSource} from './account-transfer-source.model';
import {ImportData} from './import-data.model';
import {ProcessResult} from './process-result.model';
export class AccountTransferImportResult {
    public processResult: ProcessResult;
    public importData: ImportData;
    public invalidSources: AccountTransferSource[];
    public readCount: number;
    public validCount: number;
    public validAmount: number;
    public invalidCount: number;
    public invalidAmount: number;
    public logs: string[];
}

