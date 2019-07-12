import {InvoiceTemplateSetting} from './invoice-template-setting.model';
import {ProcessResult} from './process-result.model';
export class InvoiceTemplateSettingsResult {
    public processResult: ProcessResult;
    public invoiceTemplateSettings: InvoiceTemplateSetting[];
}

