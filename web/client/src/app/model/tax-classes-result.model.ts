import {TaxClass} from './tax-class.model';
import {ProcessResult} from './process-result.model';
export class TaxClassesResult {

    public processResult: ProcessResult;
    public get ProcessResult() : ProcessResult {
        return this.processResult;
    }
    public set ProcessResult(processResult: ProcessResult) {
        this.processResult = processResult;
    }
    public taxClasses: TaxClass[];
    public get TaxClasses() : TaxClass[] {
        return this.taxClasses;
    }
    public set TaxClasses(taxClassex: TaxClass[]) {
        this.taxClasses = taxClassex;
    }
}

