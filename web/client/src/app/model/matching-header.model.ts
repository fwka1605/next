export class MatchingHeader {
    public id: number;
    public companyId: number;
    public currencyId: number;
    public customerId: number | null;
    public paymentAgencyId: number | null;
    public approved: number;
    public receiptCount: number;
    public billingCount: number;
    public amount: number;
    public bankTransferFee: number;
    public taxDifference: number;
    public matchingProcessType: number;
    public memo: string | null;
    public createBy: number;
    public createAt: string;
    public updateBy: number;
    public updateAt: string;
    public receiptAmount: number;
    public billingAmount: number;
    public currencyCode: string | null;
    public dispCustomerCode: string | null;
    public dispCustomerName: string | null;
    public shareTransferFee: number;
    public payerName: string | null;
    public customerCode: string | null;
    public paymentAgencyCode: string | null;
    public matchingUpdateAt: string;
    public billingDisplayOrder: number;
    public receiptDisplayOrder: number;
}

