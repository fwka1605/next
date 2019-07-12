export class TransactionImportSource {
    public companyId: number;
    public loginUserId: number;
    public encodingCodePage: number;
    public importerSettingId: number;
    public data: string | null;
    public importDataId: number | null;
    public isValidData: Boolean;
    public doTargetNotMatchedData: Boolean;
    public doReplaceAmount: Boolean;
    public doIgnoreSameCustomerGroup: Boolean;
}

