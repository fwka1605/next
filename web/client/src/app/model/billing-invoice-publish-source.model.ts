import {BillingInvoiceForPublish} from './billing-invoice-for-publish.model';
export class BillingInvoicePublishSource {
    public loginUserId: number;
    public connectionId: string | null;
    public items: BillingInvoiceForPublish[];
}

