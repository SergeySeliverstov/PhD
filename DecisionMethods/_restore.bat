@echo off
rem %f - file name            | *.tif
rem %p - pollution percent    | [0-100]
rem %m - mask M               | [0.0001,1000]
rem %n - mask N               | [0.0001,1000]
rem %k - mask K               | [0.0001,1000]
rem %c - use color            | [0,1]
rem %u - use mask             | [0,1]
rem %z - restore M            | [0.0001,1000]
rem %x - restore N            | [0.0001,1000]

for /F "delims=" %%f in ('dir *.tif /b /a:-d') do (
 for /D %%p in (10) do (
  for /D %%m in (2,3,4,5,6,7,8,16,32,64,128) do (
   for /D %%n in (1) do (
    for /D %%k in (1) do (
     for /D %%c in (1) do (
      for /D %%u in (0,1) do (
       for /D %%z in (2,3,4,5,6,7,8,16,32,64,128) do (
        for /D %%x in (1) do (
         call DecisionMethods.exe /r %%f %%p %%m %%n %%k %%c %%u %%z %%x
         echo Restore complete. File: %%f Pollution: %%p M: %%m N: %%n K: %%k Use color: %%c Use mask: %%u MR: %%z NR: %%x
         copy %%f Completed\%%f      
         move /Y %%f_polluted.png Completed\%%f_polluted_%%p_M_%%m_N_%%n_K_%%k_UseColor_%%c_UseMask_%%u_MR_%%z_NR_%%x.png
         move /Y %%f_restored.png Completed\%%f_restored_%%p_M_%%m_N_%%n_K_%%k_UseColor_%%c_UseMask_%%u_MR_%%z_NR_%%x.png
		 move /Y %%f_restoredOld.png Completed\%%f_restoredOld_%%p_M_%%m_N_%%n_K_%%k_UseColor_%%c_UseMask_%%u_MR_%%z_NR_%%x.png
		 move /Y %%f_mask.png Completed\%%f_mask_%%p_M_%%m_N_%%n_K_%%k_UseColor_%%c_UseMask_%%u_MR_%%z_NR_%%x.png
)))))))))

echo Batch completed
pause