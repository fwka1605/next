import {Collation} from './collation.model';
export class MatchingSequentialReportSource {
    public companyId: number;
    public priorReceipt: Boolean;
    public precision: number;
    public items: Collation[];
}

