import { MfStatus } from "./mf-status.model";
import { Customer } from "../customer.model";

export class MfBilling {
    public id: string;
    public partner_id: string;
    public partner_name: string;
    public department_id: string;
    public billing_date: string;
    public sales_date: string;
    public due_date: string;
    public total_price: number;
    public billing_number: string;
    public title: string;
    public memo: string;
    public payment_condition: string;
    public note: string;
    public tags: string[];
    public status: MfStatus;
    public selected: boolean = true;
    public customer: Customer;
    public customerCode: string;
    //private const string dummyKana = "DUMMY";
}