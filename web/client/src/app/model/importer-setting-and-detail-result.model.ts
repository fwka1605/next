import {ImporterSettingDetail} from './importer-setting-detail.model';
import {ImporterSetting} from './importer-setting.model';
import {ProcessResult} from './process-result.model';
export class ImporterSettingAndDetailResult {
    public processResult: ProcessResult;
    public importerSetting: ImporterSetting;
    public importerSettingDetail: ImporterSettingDetail[];
}

