import {ProcessResult} from './process-result.model';
import {Section} from './section.model';
export class ImportResultSection {
    public section: Section[];
    public processResult: ProcessResult;
    public insertCount: number;
    public updateCount: number;
    public deleteCount: number;
    public logs: string[];
}

