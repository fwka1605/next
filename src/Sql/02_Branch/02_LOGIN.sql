/* ユーザー作成 */
USE ScarletBranch01
go
IF  EXISTS (SELECT * FROM sys.database_principals WHERE name = N'Scarlet')
DROP USER Scarlet
go
CREATE USER Scarlet FOR LOGIN Scarlet
go
EXEC sp_addrolemember N'db_ddladmin', N'Scarlet'
go
EXEC sp_addrolemember N'db_datareader', N'Scarlet'
go
EXEC sp_addrolemember N'db_datawriter', N'Scarlet'
go
EXEC sp_addrolemember N'db_owner', N'Scarlet'
go
