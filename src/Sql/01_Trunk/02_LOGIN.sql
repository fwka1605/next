/* ログイン作成 */
USE master
go
IF  EXISTS (SELECT * FROM sys.server_principals WHERE name = N'Scarlet')
DROP LOGIN Scarlet
go
CREATE LOGIN Scarlet WITH PASSWORD=N'Scarlet', DEFAULT_DATABASE=ScarletTrunk, CHECK_EXPIRATION=OFF, CHECK_POLICY=OFF
go

/* ユーザー作成 */
USE ScarletTrunk
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
