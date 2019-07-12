import {MailSetting} from './mail-setting.model';
import {ProcessResult} from './process-result.model';
export class MailSettingsResult {
    public processResult: ProcessResult;
    public mailSettings: MailSetting[];
}

