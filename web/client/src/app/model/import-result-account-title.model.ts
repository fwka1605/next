import {ProcessResult} from './process-result.model';
import {AccountTitle} from './account-title.model';
export class ImportResultAccountTitle {
    public accountTitles: AccountTitle[];
    public processResult: ProcessResult;
    public insertCount: number;
    public updateCount: number;
    public deleteCount: number;
    public logs: string[];
}

