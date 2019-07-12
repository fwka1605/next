cd /d %~dp0
set cdir=%~dp0
sqlcmd -S (local)\V12 -i xx.initialize_database.sql -v cdir="%cdir%"
rem 上記 -S オプションのサーバー名を変更してください
pause
