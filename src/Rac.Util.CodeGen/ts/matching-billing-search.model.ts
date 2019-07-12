import {MatchingOrder} from './matching-order.model';
export class MatchingBillingSearch {
    public companyId: number;
    public currencyId: number | null;
    public dueAtFrom: string | null;
    public dueAtTo: string | null;
    public departmentId: number | null;
    public paymentAgencyId: number | null;
    public parentCustomerId: number | null;
    public clientKey: string | null;
    public billingDataType: number;
    public useReceiptSection: number;
    public useCashOnDueDates: number;
    public isParent: number;
    public matchingHeaderId: number | null;
    public useDepartmentWork: Boolean;
    public orders: MatchingOrder[];
}

