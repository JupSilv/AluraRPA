@echo off
:inicio
@echo off
cls
echo. ---------------------------
echo   Menu Principal
echo. ---------------------------
echo  1 - Install servico
echo. 3 - Stop servico
echo  4 - Delete servico
echo  0 - SAIR
echo. ---------------------------
set /p Comando= Digite uma Opcao :
if "%Comando%" equ "1" (goto:op1)
if "%Comando%" equ "3" (goto:op3)
if "%Comando%" equ "4" (goto:op4)
if "%Comando%" equ "0" (goto:exit)
:op1
echo Install servico
start sc create Nome_Do_Servico binpath= "D:\Publish\Nome_Do_Servico\Nome_Do_Servico.exe" DisplayName= "Nome_Do_Servico" type= own start= auto 
pause
goto:inicio
:op3
echo Stop servico
start sc stop Nome_Do_Servico
pause
goto:inicio
:op4
echo Delete servico
start sc delete Nome_Do_Servico
pause
goto:inicio
:exit
exit
