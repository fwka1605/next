export class CustomerPaymentContract {
    public customerId: number;
    public thresholdValue: number;
    public lessThanCollectCategoryId: number;
    public greaterThanCollectCategoryId1: number;
    public greaterThanRate1: number | null;
    public greaterThanRoundingMode1: number | null;
    public greaterThanSightOfBill1: number | null;
    public greaterThanCollectCategoryId2: number | null;
    public greaterThanRate2: number | null;
    public greaterThanRoundingMode2: number | null;
    public greaterThanSightOfBill2: number | null;
    public greaterThanCollectCategoryId3: number | null;
    public greaterThanRate3: number | null;
    public greaterThanRoundingMode3: number | null;
    public greaterThanSightOfBill3: number | null;
    public createBy: number;
    public createAt: string;
    public updateBy: number;
    public updateAt: string;
    public greaterThanCode1: string | null;
    public greaterThanName1: string | null;
    public greaterThanCode2: string | null;
    public greaterThanName2: string | null;
    public greaterThanCode3: string | null;
    public greaterThanName3: string | null;
    public lessThanCode: string | null;
    public lessThanName: string | null;
    public collectCategoryCode: string | null;
    public collectCategoryName: string | null;
}

