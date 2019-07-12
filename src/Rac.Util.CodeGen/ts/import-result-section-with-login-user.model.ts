import {ProcessResult} from './process-result.model';
import {SectionWithLoginUser} from './section-with-login-user.model';
export class ImportResultSectionWithLoginUser {
    public sectionWithLoginUser: SectionWithLoginUser[];
    public processResult: ProcessResult;
    public insertCount: number;
    public updateCount: number;
    public deleteCount: number;
    public logs: string[];
}

