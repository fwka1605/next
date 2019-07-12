import {ProcessResult} from './process-result.model';
import {Staff} from './staff.model';
export class ImportResultStaff {
    public staffs: Staff[];
    public processResult: ProcessResult;
    public insertCount: number;
    public updateCount: number;
    public deleteCount: number;
    public logs: string[];
}

