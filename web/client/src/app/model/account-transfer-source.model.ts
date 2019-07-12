import {Billing} from './billing.model';
export class AccountTransferSource {
    public billings: Billing;
    public transferResultCode: number;
    public transferAmount: number;
    public transferBankName: string | null;
    public transferBranchName: string | null;
    public transferCustomerCode: string | null;
    public transferAccountName: string | null;
    public ignoreInitialization: Boolean;
    public newDueAt: string;
}

