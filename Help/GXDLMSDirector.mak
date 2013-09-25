#change the value of this macro to where you keep the html help compiler
HHC= "C:\Program Files\HTML Help Workshop\hhc.exe"


!IF !EXIST ($(HHC))
!ERROR Could not find the html help compiler
!ENDIF

# hhc returns 1 on success, which NMAKE interprets as an error
# ignore non-zero exit codes
.IGNORE :
GXDLMSDirector.chm: GXDLMSDirector.hhp 
# make the CHM	
	$(HHC) GXDLMSDirector.chm
!IF EXISTS(..\Install\Files)
	copy GXDLMSDirector.chm ..\Install\Files /y	
!ENDIF