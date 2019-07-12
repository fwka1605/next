import {ProcessResult} from './process-result.model';
import {CustomerGroup} from './customer-group.model';
export class ImportResultCustomerGroup {
    public customerGroup: CustomerGroup[];
    public processResult: ProcessResult;
    public insertCount: number;
    public updateCount: number;
    public deleteCount: number;
    public logs: string[];
}

