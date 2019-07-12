import {MatchingOrder} from './matching-order.model';
import {CollationOrder} from './collation-order.model';
export class CollationSetting {
    public companyId: number;
    public requiredCustomer: number;
    public autoAssignCustomer: number;
    public learnKanaHistory: number;
    public useApportionMenu: number;
    public reloadCollationData: number;
    public useAdvanceReceived: number;
    public advanceReceivedRecordedDateType: number;
    public autoMatching: number;
    public autoSortMatchingEnabledData: number;
    public useFromToNarrowing: number;
    public setSystemDateToCreateAtFilter: number;
    public prioritizeMatchingIndividuallyMultipleReceipts: number;
    public forceShareTransferFee: number;
    public learnSpecifiedCustomerKana: number;
    public matchingSilentSortedData: number;
    public billingReceiptDisplayOrder: number;
    public removeSpaceFromPayerName: number;
    public prioritizeMatchingIndividuallyTaxTolerance: number;
    public journalizingPattern: number;
    public calculateTaxByInputId: number;
    public sortOrderColumn: number;
    public sortOrder: number;
    public collationOrders: CollationOrder[];
    public billingMatchingOrders: MatchingOrder[];
    public receiptMatchingOrders: MatchingOrder[];
}

