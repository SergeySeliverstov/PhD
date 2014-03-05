@echo off

for /F "delims=" %%f in ('dir *.tif /b /a:-d') do (
 for /D %%p in (10,20,30) do (
  call DecisionMethods.exe /m %%f %%p
  echo Mine complete. File: %%f Pollution: %%p
 )
)

echo Batch completed
pause