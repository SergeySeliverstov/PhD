@echo off
rem %f - input file           | *.tif
rem %m - method               | [0,1] 0 - tree, 1 - list
rem %t - template             | [0,1,2,3]
rem %c - crop pixels          | [0,1]
rem %a - accuracy             | [0-100]
rem %l - transaction lenght   | [3,4,5]
rem %p - pollution percent    | [0-100]
rem %u - use mask             | [0,1]
rem %w - WSM                  | [0,1]
rem %q - Limit                | [0,1]
rem %s - SP                   | [0,1]

del statistics.csv
for /f "delims=" %%f in ('dir D:\Apps.net\_Phd\BmpImage\DataMining\bin\x64\Debug\*.tif /b /a:-d') do (
 for /L %%m in (0,1,0) do (
  for /L %%t in (0,1,0) do (
   for /L %%s in (0,1,1) do (
    for /L %%q in (0,1,1) do ( 
     for /L %%u in (0,1,1) do ( 
      for /L %%w in (0,1,1) do ( 
       for /L %%c in (0,1,1) do ( 
         for /L %%a in (15,15,15) do (
          for /L %%l in (3,1,3) do (
           for /L %%p in (10,10,10) do (
            rem echo %%f %%m %%t %%c %%a %%l %%p %%u %%w %%q %%s
            rem "File" "Method" "Template" "Crop pixels" "Accuracy" "Transaction length" "Pollution percent" "Use mask", "WSM", "Limit", "SP"
            call DataMining.exe %%f %%m %%t %%c %%a %%l %%p %%u %%w %%q %%s
            echo Completed: %%f %%m %%t %%c %%a %%l %%p %%u %%w %%q %%s
			copy %%f Completed\%%f
			rem move /Y %%f_polluted.png Completed\%%f_polluted.png      
			rem move /Y %%f_restored.png Completed\%%f_restored.png
			move /Y %%f_maskStatistics.png Completed\%%f_mask_polluted_P_%%p_UseMask_%%u_Crop_%%c_Acc_%%a_S_%%s_Q_%%q_W_%%w_L_%%l.png
			move /Y %%f_maskOriginal.png Completed\%%f_mask_find_P_%%p_UseMask_%%u_Crop_%%c_Acc_%%a_S_%%s_Q_%%q_W_%%w_L_%%l.png
			move /Y %%f_maskMiss.png Completed\%%f_mask_miss_P_%%p_UseMask_%%u_Crop_%%c_Acc_%%a_%%_S_%%s_Q_%%q_W_%%w_L_%%l.png
			move /Y %%f_maskFalse.png Completed\%%f_mask_false_P_%%p_UseMask_%%u_Crop_%%c_Acc_%%a_S_%%s_Q_%%q_W_%%w_L_%%l.png
)))))))))))
echo finish
copy statistics.csv statistics_finished.csv