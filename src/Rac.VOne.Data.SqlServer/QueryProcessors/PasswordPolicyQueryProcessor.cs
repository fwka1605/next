using Rac.VOne.Web.Models;
using Rac.VOne.Data.QueryProcessors;
using System.Threading;
using System.Threading.Tasks;

namespace Rac.VOne.Data.SqlServer.QueryProcessors
{
    public class PasswordPolicyQueryProcessor :
        IAddPasswordPolicyQueryProcessor
    {

        private readonly IDbHelper dbHelper;

        public PasswordPolicyQueryProcessor(IDbHelper dbHelper)
        {
            this.dbHelper = dbHelper;
        }

        public Task<PasswordPolicy> SaveAsync(PasswordPolicy PasswordPolicy, CancellationToken token = default(CancellationToken))
        {
            var query = @"
MERGE INTO PasswordPolicy target
USING (SELECT @CompanyId        CompanyId
      ) source
   ON (    target.CompanyId         = source.CompanyId
      )
 WHEN MATCHED THEN
      UPDATE SET
             MinLength              = @MinLength
           , MaxLength              = @MaxLength
           , UseAlphabet            = @UseAlphabet
           , MinAlphabetUseCount    = @MinAlphabetUseCount
           , UseNumber              = @UseNumber
           , MinNumberUseCount      = @MinNumberUseCount
           , UseSymbol              = @UseSymbol
           , MinSymbolUseCount      = @MinSymbolUseCount
           , SymbolType             = @SymbolType
           , CaseSensitive          = @CaseSensitive
           , MinSameCharacterRepeat = @MinSameCharacterRepeat
           , ExpirationDays         = @ExpirationDays
           , HistoryCount           = @HistoryCount
 WHEN NOT MATCHED THEN
      INSERT ( CompanyId,  MinLength,  MaxLength
           ,  UseAlphabet, MinAlphabetUseCount
           ,  UseNumber  , MinNumberUseCount
           ,  UseSymbol  , MinSymbolUseCount   ,  SymbolType
           ,  CaseSensitive  ,  MinSameCharacterRepeat
           ,  ExpirationDays ,  HistoryCount
            )
      VALUES (@CompanyId, @MinLength, @MaxLength
           , @UseAlphabet,@MinAlphabetUseCount
           , @UseNumber  ,@MinNumberUseCount
           , @UseSymbol  ,@MinSymbolUseCount   , @SymbolType
           , @CaseSensitive  , @MinSameCharacterRepeat
           , @ExpirationDays , @HistoryCount
            )
      OUTPUT inserted.*; ";
            return dbHelper.ExecuteAsync<PasswordPolicy>(query, PasswordPolicy, token);
        }

    }
}
