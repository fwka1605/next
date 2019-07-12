import { FunctionType } from "../common/const/kbn.const";

export class FunctionAuthority {
    public companyId: number;
    public authorityLevel: number;
    public functionType: FunctionType;
    public available: Boolean;
    public createBy: number;
    public createAt: string;
    public updateBy: number;
    public updateAt: string;
}

