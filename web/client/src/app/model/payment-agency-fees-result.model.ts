import {PaymentAgencyFee} from './payment-agency-fee.model';
import {ProcessResult} from './process-result.model';
export class PaymentAgencyFeesResult {
    public processResult: ProcessResult;
    public paymentAgencyFees: PaymentAgencyFee[];
}

