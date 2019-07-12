import {GridSetting} from './grid-setting.model';
import {ExportMatchingIndividual} from './export-matching-individual.model';
export class MatchingIndividualReportSource {
    public companyId: number;
    public priorReceipt: Boolean;
    public precision: number;
    public billingTaxDiff: number;
    public receiptTaxDiff: number;
    public bankFee: number;
    public discountAmount: number;
    public items: ExportMatchingIndividual[];
    public billingGridSettings: GridSetting[];
    public receiptGridSettings: GridSetting[];
}

