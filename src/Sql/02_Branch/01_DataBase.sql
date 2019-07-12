-- DB作成
USE master
GO

-- 存在しなければ作成
-- フォルダは環境に応じて書き換えて実行
declare @DBName as sysname = N'ScarletBranch01'
      , @DataPath as sysname = N'F:\RAC\MSSQL12.V12\MSSQL\DATA\'
      , @cmd as nvarchar(max)
set @cmd = N'
IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = ''' + @DBName + ''')
BEGIN
	CREATE DATABASE [' + @DBName +'] ON  PRIMARY 
	( NAME = N''' + @DBName + ''', FILENAME = N''' + @DataPath + @DBName + '.mdf'' , SIZE = 5120KB , FILEGROWTH = 1024KB )
	 LOG ON 
	( NAME = N''' + @DBName + '_log'', FILENAME = N''' + @DataPath + @DBName + '_log.ldf'' , SIZE = 1024KB , FILEGROWTH = 10%)
	 COLLATE Japanese_CI_AS_KS_WS
END
'
exec sp_executesql @cmd
GO
