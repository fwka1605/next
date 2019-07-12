import {ProcessResult} from './process-result.model';
export class ImportResult {
    public processResult: ProcessResult;
    public insertCount: number;
    public updateCount: number;
    public deleteCount: number;
    public logs: string[];
}

