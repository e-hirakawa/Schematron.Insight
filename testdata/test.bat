chcp 65001
@echo off

REM 検証対象XMLファイル名（または、複数ファイルの場合には、フォルダパス）
set xml="mimetype.xml"
REM Schematronファイル名
set sch="mimetype.sch"
REM 検証結果出力先ファイル名
set out="mimetype-result.html"
REM 検証結果出力フォーマット（html|log|tab|xml|json）
set frm=html
set phs=#ALL

REM ################# !編集禁止! #################
schematron.tester.exe -s=%sch% -o=%out% -f=%frm% -p=%phs% -x=%xml%
REM ##############################################
pause