import { Injectable } from '@angular/core';
import { NgbDateParserFormatter, NgbDateStruct } from '@ng-bootstrap/ng-bootstrap';
import { isNumber, toInteger, padNumber } from '../util/ng-bootstrap-util';

@Injectable()
export class NgbDateCustomParserFormatter extends NgbDateParserFormatter {
    private regex = new RegExp(/^[0-9]*$/);

    parse(value: string): NgbDateStruct {
        if (value) {

            if (value.length == 8 && this.regex.test(value)) {
                return { year: toInteger(value.substr(0, 4)), month: toInteger(value.substr(4, 2)), day: toInteger(value.substr(6, 2)) };
            }

            const dateParts = value.trim().split('/');
            if (dateParts.length === 1 && isNumber(dateParts[0])) {
                return { year: toInteger(dateParts[0]), month: null, day: null };
            } else if (dateParts.length === 2 && isNumber(dateParts[0]) && isNumber(dateParts[1])) {
                return { year: toInteger(dateParts[0]), month: toInteger(dateParts[1]), day: null };
            } else if (dateParts.length === 3 && isNumber(dateParts[0]) && isNumber(dateParts[1]) && isNumber(dateParts[2])) {
                return { year: toInteger(dateParts[0]), month: toInteger(dateParts[1]), day: toInteger(dateParts[2]) };
            }
        }
        return null;
    }

    format(date: NgbDateStruct): string {
        return date ?
            `${date.year}/${isNumber(date.month) ? padNumber(date.month) : ''}/${isNumber(date.day) ? padNumber(date.day) : ''}` :
            '';
    }
}