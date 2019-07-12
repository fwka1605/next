import {ScheduledPaymentImport} from './scheduled-payment-import.model';
import {ProcessResult} from './process-result.model';
export class ScheduledPaymentImportsResult {
    public processResult: ProcessResult;
    public scheduledPaymentImports: ScheduledPaymentImport[];
}

