import {CustomerPaymentContract} from './customer-payment-contract.model';
import {ProcessResult} from './process-result.model';
export class CustomerPaymentContractsResult {
    public processResult: ProcessResult;
    public payments: CustomerPaymentContract[];
}

