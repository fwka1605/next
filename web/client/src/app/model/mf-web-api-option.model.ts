export class MFWebApiOption {
    public companyId: number;
    public loginUserId: number;
    public clientId: string | null;
    public clientSecret: string | null;
    public extractSetting: string | null;
    public authorizationCode: string | null;
    public billingDateFrom: string | null;
    public billingDateTo: string | null;
    public mfIds: string[];
    public isMatched: Boolean;
    public partnerId: string | null;
    public apiVersion: string | null;
}

