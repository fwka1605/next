import {CustomerPaymentContract} from './customer-payment-contract.model';
import {ProcessResult} from './process-result.model';
export class CustomerPaymentContractResult {
    public processResult: ProcessResult;
    public payment: CustomerPaymentContract;
}

