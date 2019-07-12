import {PaymentAgency} from './payment-agency.model';
import {ProcessResult} from './process-result.model';
export class PaymentAgenciesResult {
    public processResult: ProcessResult;
    public paymentAgencies: PaymentAgency[];
}

