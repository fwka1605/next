import {CustomerDiscount} from './customer-discount.model';
import {ProcessResult} from './process-result.model';
export class CustomerDiscountsResult {
    public processResult: ProcessResult;
    public customerDiscounts: CustomerDiscount[];
}

