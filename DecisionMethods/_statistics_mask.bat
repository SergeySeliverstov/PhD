@echo off

for /F "delims=" %%f in ('dir *.tif /b /a:-d') do (
 for /D %%p in (10,20,30) do (
  for /D %%m in (1,2,3,4,5,6,7,8,16,32,64,128) do (
   for /D %%n in (1,2,3,4,5,0.1,0.2,0.3,0.4,0.5) do (
    for /D %%k in (1,2,3,4,5,0.1,0.2,0.3,0.4,0.5) do (
     for /D %%c in (0,1) do (
      call DecisionMethods.exe /s %%f %%p %%m %%n %%k %%c
      echo Statistics mask complete. File: %%f Pollution: %%p M: %%m N: %%n K: %%k Color:%%c
     )
    )
   )
  )
 )
)

echo Batch completed
pause