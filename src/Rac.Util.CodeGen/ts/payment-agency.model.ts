export class PaymentAgency {
    public id: number;
    public companyId: number;
    public code: string | null;
    public name: string | null;
    public kana: string | null;
    public consigneeCode: string | null;
    public shareTransferFee: number;
    public useFeeLearning: number;
    public useFeeTolerance: number;
    public dueDateOffset: number;
    public bankCode: string | null;
    public bankName: string | null;
    public branchCode: string | null;
    public branchName: string | null;
    public accountTypeId: number;
    public accountNumber: string | null;
    public fileFormatId: number;
    public considerUncollected: number;
    public collectCategoryId: number | null;
    public useKanaLearning: number;
    public createBy: number;
    public createAt: string;
    public updateBy: number;
    public updateAt: string;
    public outputFileName: string | null;
    public appendDate: number;
    public contractCode: string | null;
}

