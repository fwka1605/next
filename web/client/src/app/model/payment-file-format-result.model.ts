import {PaymentFileFormat} from './payment-file-format.model';
import {ProcessResult} from './process-result.model';
export class PaymentFileFormatResult {
    public processResult: ProcessResult;
    public paymentFileFormats: PaymentFileFormat[];
}

