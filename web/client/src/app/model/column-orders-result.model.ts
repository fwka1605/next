import {ColumnOrder} from './column-order.model';
import {ProcessResult} from './process-result.model';
export class ColumnOrdersResult {
    public processResult: ProcessResult;
    public get ProcessResult() : ProcessResult {
        return this.processResult;
    }
    public set ProcessResult(processResult: ProcessResult) {
        this.processResult = processResult;
    }
    public columnOrders: ColumnOrder[];
    public get ColumnOrders() : ColumnOrder[] {
        return this.columnOrders;
    }
    public set ColumnOrders(columnOrders: ColumnOrder[]) {
        this.columnOrders = columnOrders;
    }
}

