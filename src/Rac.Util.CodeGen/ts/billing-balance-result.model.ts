import {ProcessResult} from './process-result.model';
export class BillingBalanceResult {
    public processResult: ProcessResult;
    public lastCarryOverAt: string | null;
}

