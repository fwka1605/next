import {ProcessResult} from './process-result.model';
export class ImportResultLoginUser {
    public licenseIsOrver: Boolean;
    public notExistsLoginUser: Boolean;
    public loginUserHasNotLoginLicense: Boolean;
    public processResult: ProcessResult;
    public insertCount: number;
    public updateCount: number;
    public deleteCount: number;
    public logs: string[];
}

