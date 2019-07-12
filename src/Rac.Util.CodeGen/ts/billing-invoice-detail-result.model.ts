import {BillingInvoiceDetailForPrint} from './billing-invoice-detail-for-print.model';
import {ProcessResult} from './process-result.model';
export class BillingInvoiceDetailResult {
    public processResult: ProcessResult;
    public billingInvoicesDetails: BillingInvoiceDetailForPrint[];
}

