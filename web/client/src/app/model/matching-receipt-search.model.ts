import {MatchingOrder} from './matching-order.model';
export class MatchingReceiptSearch {
    public companyId: number;
    public recordedAtFrom: string | null;
    public recordedAtTo: string | null;
    public parentCustomerId: number | null;
    public clientKey: string | null;
    public billingDataType: number;
    public useReceiptSection: number;
    public useCashOnDueDates: number;
    public useScheduledPayment: number;
    public paymentAgencyId: number | null;
    public matchingHeaderId: number | null;
    public payerName: string | null;
    public currencyId: number;
    public orders: MatchingOrder[];
}

