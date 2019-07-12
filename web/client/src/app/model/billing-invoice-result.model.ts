import {BillingInvoice} from './billing-invoice.model';
import {ProcessResult} from './process-result.model';
export class BillingInvoiceResult {
    public processResult: ProcessResult;
    public billingInvoices: BillingInvoice[];
}

