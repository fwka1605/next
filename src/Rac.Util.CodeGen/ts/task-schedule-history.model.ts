export class TaskScheduleHistory {
    public id: number;
    public companyId: number;
    public importType: number;
    public importSubType: number;
    public startAt: string;
    public endAt: string;
    public result: number;
    public errors: string | null;
}

