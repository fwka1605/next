export class PdfOutputSetting {
    public id: number;
    public companyId: number;
    public reportType: number;
    public outputUnit: number;
    public fileName: string | null;
    public useCompression: number;
    public maximumSize: number | null;
    public createBy: number;
    public createAt: string;
    public updateBy: number;
    public updateAt: string;
}

