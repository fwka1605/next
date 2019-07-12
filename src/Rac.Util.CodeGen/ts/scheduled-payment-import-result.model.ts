import {ScheduledPaymentImport} from './scheduled-payment-import.model';
import {ProcessResult} from './process-result.model';
export class ScheduledPaymentImportResult {
    public processResult: ProcessResult;
    public scheduledPaymentImport: ScheduledPaymentImport[];
}

