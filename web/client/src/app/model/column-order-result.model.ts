import {ColumnOrder} from './column-order.model';
import {ProcessResult} from './process-result.model';
export class ColumnOrderResult {
    public processResult: ProcessResult;
    public get ProcessResult() : ProcessResult {
        return this.processResult;
    }
    public set ProcessResult(processResult: ProcessResult) {
        this.processResult = processResult;
    }
    public columnOrder: ColumnOrder;
    public get ColumnOrder() : ColumnOrder {
        return this.columnOrder;
    }
    public set ColumnOrder(columnOrder: ColumnOrder) {
        this.columnOrder = columnOrder;
    }
}

