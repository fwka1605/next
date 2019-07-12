import { MfAggrSubAccount } from './mf-aggr-sub-account.model';
export class MfAggrAccount {
    public id: number;
    public displayName: string | null;
    public lastAggregatedAt: string;
    public lastLoginAt: string;
    public lastSucceededAt: string;
    public aggregationStartDate: string | null;
    public status: number;
    public isSuspended: number;
    public bankCode: string;
    public subAccounts: MfAggrSubAccount[];
}
