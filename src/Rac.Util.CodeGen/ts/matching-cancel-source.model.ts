import {MatchingHeader} from './matching-header.model';
export class MatchingCancelSource {
    public loginUserId: number;
    public connectionId: string | null;
    public headers: MatchingHeader[];
}

