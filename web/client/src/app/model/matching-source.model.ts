import {CollationSearch} from './collation-search.model';
import {BillingScheduledIncome} from './billing-scheduled-income.model';
import {MatchingBillingDiscount} from './matching-billing-discount.model';
import {Receipt} from './receipt.model';
import {Billing} from './billing.model';
import {Matching} from './matching.model';
import {MatchingHeader} from './matching-header.model';
export class MatchingSource {
    public matchingHeader: MatchingHeader;
    public matchings: Matching[];
    public billings: Billing[];
    public receipts: Receipt[];
    public matchingBillingDiscounts: MatchingBillingDiscount[];
    public billingDiscounts: number;
    public billingScheduledIncomes: BillingScheduledIncome[];
    public billingRemainTotal: number;
    public receiptRemainTotal: number;
    public billingDiscountTotal: number;
    public remainType: number;
    public taxDifference: number;
    public bankTransferFee: number;
    public clientKey: string | null;
    public companyId: number;
    public childCustomerIds: number[];
    public advanceReceivedCustomerId: number | null;
    public matchingProcessType: number;
    public customerId: number | null;
    public paymentAgencyId: number | null;
    public loginUserId: number;
    public useKanaLearning: number;
    public useFeeLearning: number;
    public option: CollationSearch;
}

