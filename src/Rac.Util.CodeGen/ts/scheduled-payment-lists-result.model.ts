import {ScheduledPaymentList} from './scheduled-payment-list.model';
import {ProcessResult} from './process-result.model';
export class ScheduledPaymentListsResult {
    public processResult: ProcessResult;
    public scheduledPaymentLists: ScheduledPaymentList[];
}

