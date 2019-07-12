export class JournalizingOption {
    public companyId: number;
    public currencyId: number | null;
    public customerId: number | null;
    public isOutputted: Boolean;
    public loginUserId: number;
    public updateAt: string;
    public recordedAtFrom: string | null;
    public recordedAtTo: string | null;
    public outputAt: string[];
    public useDiscount: Boolean;
    public containAdvanceReceivedOccured: Boolean;
    public containAdvanceReceivedMatching: Boolean;
    public createAtFrom: string | null;
    public createAtTo: string | null;
    public journalizingTypes: number[];
    public isGeneral: Boolean;
    public outputCustoemrName: Boolean;
    public precision: number;
}

