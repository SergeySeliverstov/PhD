@echo off
rem %i - input file         | *.tif
rem %m - method             | [0,1] 0 - tree, 1 - list
rem %t - template           | [0,1,2,3]
rem %c - crop pixels        | [0,1]
rem %a - accuracy           | [0-100]
rem %l - transaction lenght | [3,4,5]
rem %p - pollution percent  | [0-100]
rem %u - use mask           | [0,1]

del statistics.csv
for /f "delims=" %%i in ('dir D:\Apps.net\_Phd\BmpImage\DataMining\bin\x64\Debug\*.tif /b /a:-d') do (
 for /L %%m in (0,1,0) do (
  for /L %%t in (0,1,3) do (
   for /L %%c in (0,1,0) do (
    for /L %%a in (0,15,30) do (
     for /L %%l in (3,1,5) do (
      for /L %%p in (0,10,20) do (
       for /L %%u in (0,1,1) do ( 
        rem echo %%i %%m %%t %%c %%a %%l %%p %%u
		rem "File" "Method" "Template" "Crop pixels" "Accuracy" "Transaction length" "Pollution percent" "Use mask"
        call DataMining.exe %%i %%m %%t %%c %%a %%l %%p %%u
		echo Completed: %%i %%m %%t %%c %%a %%l %%p %%u
))))))))
echo finish
copy statistics.csv statistics_finished.csv