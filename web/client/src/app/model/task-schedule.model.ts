export class TaskSchedule {
    public id: number;
    public companyId: number;
    public importType: number;
    public importSubType: number;
    public duration: number;
    public startDate: string;
    public interval: number;
    public weekDay: string | null;
    public importDirectory: string | null;
    public successDirectory: string | null;
    public failedDirectory: string | null;
    public logDestination: number;
    public targetBillingAssignment: number;
    public billingAmount: number;
    public updateSameCustomer: number;
    public createBy: number;
    public createUserName: string | null;
    public createAt: string;
    public updateBy: number;
    public updateUserName: string | null;
    public updateAt: string;
    public importMode: number;
}

