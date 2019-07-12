import {Customer} from './customer.model';
import {Billing} from './billing.model';
export class MFBillingSource {
    public billings: Billing[];
    public customers: Customer[];
    public companyId: number | null;
    public ids: number[];
    public isMatched: Boolean | null;
}

