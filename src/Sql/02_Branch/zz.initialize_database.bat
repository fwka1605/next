cd /d %~dp0
set cdir=%~dp0
sqlcmd -S (local)\V12 -i xx.initialize_database.sql -v cdir="%cdir%"
rem ��L -S �I�v�V�����̃T�[�o�[����ύX���Ă�������
pause
