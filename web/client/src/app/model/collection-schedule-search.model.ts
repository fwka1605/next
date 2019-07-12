export class CollectionScheduleSearch {
    public companyId: number;
    public yearMonth: string;
    public departmentCodeFrom: string | null;
    public departmentCodeTo: string | null;
    public staffCodeFrom: string | null;
    public staffCodeTo: string | null;
    public customerCodeFrom: string | null;
    public customerCodeTo: string | null;
    public unitPrice: number;
    public useMasterStaff: Boolean;
    public displayParent: Boolean;
    public displayDepartment: Boolean;
    public newPagePerDepartment: Boolean;
    public newPagePerStaff: Boolean;
    public isPrint: Boolean;
}

