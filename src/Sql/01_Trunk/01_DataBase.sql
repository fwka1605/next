-- DB作成
USE master
GO
-- 存在しなければ作成
-- フォルダは環境に応じて書き換えて実行
IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = N'ScarletTrunk')
BEGIN
	CREATE DATABASE [ScarletTrunk] ON  PRIMARY 
	( NAME = N'ScarletTrunk', FILENAME = N'F:\RAC\MSSQL12.V12\MSSQL\DATA\ScarletTrunk.mdf' , SIZE = 5120KB , FILEGROWTH = 1024KB )
	 LOG ON 
	( NAME = N'ScarletTrunk_log', FILENAME = N'F:\RAC\MSSQL12.V12\MSSQL\DATA\ScarletTrunk_log.ldf' , SIZE = 1024KB , FILEGROWTH = 10%)
	 COLLATE Japanese_CI_AS_KS_WS
END
GO
