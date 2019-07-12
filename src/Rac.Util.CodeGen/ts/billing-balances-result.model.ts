import {BillingBalance} from './billing-balance.model';
import {ProcessResult} from './process-result.model';
export class BillingBalancesResult {
    public processResult: ProcessResult;
    public billingBalances: BillingBalance[];
}

