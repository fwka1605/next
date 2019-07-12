namespace Rac.VOne.EbData
{
    public enum ImportResult
    {
        None = 0,
        FileNotFound,
        FileReadError,
        FileFormatError,
        BankAccountMasterError,
        ImportDataNotFound,
        DBError,
        BankAccountFormatError,
        PayerCodeFormatError,
        Success,
    }

}