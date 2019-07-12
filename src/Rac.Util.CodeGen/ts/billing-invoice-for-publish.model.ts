export class BillingInvoiceForPublish {
    public companyId: number;
    public clientKey: string | null;
    public temporaryBillingInputId: number;
    public billingInputId: number | null;
    public invoiceCode: string | null;
    public closingAt: string;
    public billedAt: string;
    public invoiceTemplateId: number;
    public invoiceTemplateFixedString: string | null;
    public updateAt: string;
    public destinationId: number | null;
}

