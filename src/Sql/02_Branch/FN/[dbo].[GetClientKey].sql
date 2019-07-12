
IF EXISTS (SELECT * FROM sys.objects
            WHERE object_id = object_id(N'GetClientKey')
              AND type in (N'FN'))
BEGIN
    DROP FUNCTION [dbo].[GetClientKey];
END;
GO

SET ANSI_NULLS ON
GO
CREATE FUNCTION [dbo].[GetClientKey]
(@ProgramId             NVARCHAR(6)
,@ClientName            NVARCHAR(30)
,@CompanyCode           NVARCHAR(20)
,@LoginUserCode         NVARCHAR(20))
RETURNS VARBINARY(20)
AS
BEGIN
 RETURN CAST(
    HASHBYTES
        (N'SHA1'
        ,@ProgramId + @ClientName + @CompanyCode + @LoginUserCode
        ) AS VARBINARY(20));
END

GO
