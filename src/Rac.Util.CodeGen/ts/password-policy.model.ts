export class PasswordPolicy {
    public companyId: number;
    public minLength: number;
    public maxLength: number;
    public useAlphabet: number;
    public minAlphabetUseCount: number;
    public useNumber: number;
    public minNumberUseCount: number;
    public useSymbol: number;
    public minSymbolUseCount: number;
    public symbolType: string | null;
    public caseSensitive: number;
    public minSameCharacterRepeat: number;
    public expirationDays: number;
    public historyCount: number;
}

