import * as _moment from 'moment';
import { default as _rollupMoment, Moment } from 'moment';

export const YMPICKER_FORMAT = {
    parse: {
        dateInput: 'YYYY/MM',
    },
    display: {
        dateInput: 'YYYY/MM',
        monthYearLabel: 'MMM YYYY',
        dateA11yLabel: 'LL',
        monthYearA11yLabel: 'MMMM YYYY',
    },
};

export const moment = _rollupMoment || _moment;