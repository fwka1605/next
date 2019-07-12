import {FunctionType} from './function-type.model';
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

