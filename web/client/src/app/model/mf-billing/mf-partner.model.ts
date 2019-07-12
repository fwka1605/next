import { MfDepartment } from "./mf-department.model";

export class partner {
    public id: string;
    public code: string;
    public name: string;
    public name_kana: string;
    public name_suffix: string;
    public memo: string;
    public created_at: Date;
    public updated_at: Date;
    public departments: MfDepartment[];
}
