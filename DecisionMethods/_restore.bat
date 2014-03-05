@echo off

for /F "delims=" %%f in ('dir *.tif /b /a:-d') do (
 for /D %%p in (10,20,30) do (
  for /D %%m in (0,1) do (
   call DecisionMethods.exe /r %%f %%p %%m
   echo Restore complete. File: %%f Pollution: %%p Use mask: %%m
   copy %%f Completed\%%f      
   move /Y %%f_polluted.png Completed\%%f_polluted_%%p_mask_%%m.png
   move /Y %%f_restored.png Completed\%%f_restored_%%p_mask_%%m.png
  )
 )
)

echo Batch completed
pause