
EXTRA_DIST =  

# Warning: This is an automatically generated file, do not edit!

if ENABLE_DEBUG
ASSEMBLY_COMPILER_COMMAND = gmcs
ASSEMBLY_COMPILER_FLAGS =  -noconfig -codepage:utf8 -warn:4 -optimize- -debug "-define:DEBUG;TRACE"
ASSEMBLY = bin/Debug/GXDLMSDirector.exe
ASSEMBLY_MDB = $(ASSEMBLY).mdb
COMPILE_TARGET = winexe
PROJECT_REFERENCES = 
BUILD_DIR = bin/Debug/

GXDLMSDIRECTOR_EXE_MDB_SOURCE=bin/Debug/GXDLMSDirector.exe.mdb
GXDLMSDIRECTOR_EXE_MDB=$(BUILD_DIR)/GXDLMSDirector.exe.mdb
ZEDGRAPH_DLL_SOURCE=ZedGraph.dll

endif

if ENABLE_RELEASE
ASSEMBLY_COMPILER_COMMAND = gmcs
ASSEMBLY_COMPILER_FLAGS =  -noconfig -codepage:utf8 -warn:4 -optimize+ "-define:TRACE"
ASSEMBLY = bin/Release/GXDLMSDirector.exe
ASSEMBLY_MDB = 
COMPILE_TARGET = winexe
PROJECT_REFERENCES = 
BUILD_DIR = bin/Release/

GXDLMSDIRECTOR_EXE_MDB=
ZEDGRAPH_DLL_SOURCE=ZedGraph.dll

endif

AL=al2
SATELLITE_ASSEMBLY_NAME=$(notdir $(basename $(ASSEMBLY))).resources.dll

PROGRAMFILES = \
	$(GXDLMSDIRECTOR_EXE_MDB) \
	$(ZEDGRAPH_DLL)  

BINARIES = \
	$(GXDLMSDIRECTOR)  


RESGEN=resgen2
	
all: $(ASSEMBLY) $(PROGRAMFILES) $(BINARIES) 

FILES = \
	Extensions/Actaris.cs \
	GXAsyncWork.cs \
	CryptHelper.cs \
	DeviceState.cs \
	GXGraphItem.cs \
	GXGraphItemCollection.cs \
	GXGraphItemForm.cs \
	GXGraphItemForm.Designer.cs \
	GXGroupBy.cs \
	IGXManufacturerExtension.cs \
	ManufacturerSettings/GXHelpers.cs \
	GXObisCodeGraphItem.cs \
	GXObisCodeGraphItemCollection.cs \
	GXObisValueItemForm.cs \
	GXObisValueItemForm.Designer.cs \
	ManufacturerSettings/GXError.cs \
	MonoWrapper.cs \
	MRUManager.cs \
	Views/GXDLMSActionScheduleView.cs \
	Views/GXDLMSAssociationLogicalNameView.cs \
	Views/GXDLMSAssociationShortNameView.cs \
	Views/GXDLMSAutoAnswerView.cs \
	Views/GXDLMSAutoAnswerView.Designer.cs \
	Views/GXDLMSMacAddressSetupView.cs \
	Views/GXDLMSMacAddressSetupView.Designer.cs \
	Views/GXDLMSRegisterMonitorView.cs \
	Views/GXDLMSSapAssignmentView.cs \
	Views/GXDLMSScriptTableView.cs \
	Views/GXDLMSGprsSetupView.cs \
	Views/GXDLMSPppSetupView.cs \
	Views/GXDLMSIp4SetupView.cs \
	Views/GXDLMSTcpUdpSetupView.cs \
	Views/GXDLMSAutoConnectView.cs \
	Views/GXDLMSModemConfigurationView.cs \
	Views/GXDLMSViewAttribute.cs \
	Views/GXValueField.cs \
	Views/GXValueField.Designer.cs \
	IGXDLMSView.cs \
	ManufacturerSettings/GXLogWriter.cs \
	GXValueSerializer.cs \
	GXDLMSObjectSerializer.cs \
	GuruxWin32.cs \
	GXDLMSCommunicator.cs \
	GXDLMSDevice.cs \
	GXDLMSDeviceCollection.cs \
	MainForm.cs \
	MainForm.Designer.cs \
	ManufacturerForm.cs \
	ManufacturerForm.Designer.cs \
	ManufacturersForm.cs \
	ManufacturersForm.Designer.cs \
	OBISCodeForm.cs \
	OBISCodeForm.Designer.cs \
	OBISCodesForm.cs \
	OBISCodesForm.Designer.cs \
	Program.cs \
	DevicePropertiesForm.cs \
	DevicePropertiesForm.Designer.cs \
	Properties/AssemblyInfo.cs \
	Properties/Resources.Designer.cs \
	Properties/Settings.Designer.cs \
	Views/GXDLMSSpecialDaysTableView.cs \
	Views/GXDLMSScheduleView.cs \
	Views/GXDLMSIECOpticalPortSetupView.cs \
	Views/GXDLMSHdlcSetupView.cs \
	Views/GXDLMSActivityCalendarView.cs \
	Views/GXDLMSRegisterActivationView.cs \
	Views/GXDLMSClockView.cs \
	Views/GXDLMSDemandRegisterView.cs \
	Views/GXDLMSExtendedRegisterView.cs \
	Views/GXDLMSRegisterView.cs \
	Views/GXDLMSProfileGenericView.cs \
	Views/GXDLMSDataView.cs 

DATA_FILES = 

RESOURCES = \
	GXGraphItemForm.resx,GXDLMSDirector.GXGraphItemForm.resources \
	GXObisValueItemForm.resx,GXDLMSDirector.GXObisValueItemForm.resources \
	Views/GXDLMSActionScheduleView.resx,GXDLMSDirector.Views.GXDLMSActionScheduleView.resources \
	Views/GXDLMSAssociationLogicalNameView.resx,GXDLMSDirector.Views.GXDLMSAssociationLogicalNameView.resources \
	Views/GXDLMSAssociationShortNameView.resx,GXDLMSDirector.Views.GXDLMSAssociationShortNameView.resources \
	Views/GXDLMSAutoAnswerView.resx,GXDLMSDirector.Views.GXDLMSAutoAnswerView.resources \
	Views/GXDLMSMacAddressSetupView.resx,GXDLMSDirector.Views.GXDLMSMacAddressSetupView.resources \
	Views/GXDLMSRegisterMonitorView.resx,GXDLMSDirector.Views.GXDLMSRegisterMonitorView.resources \
	Views/GXDLMSSapAssignmentView.resx,GXDLMSDirector.Views.GXDLMSSapAssignmentView.resources \
	Views/GXDLMSScriptTableView.resx,GXDLMSDirector.Views.GXDLMSScriptTableView.resources \
	Views/GXDLMSGprsSetupView.resx,GXDLMSDirector.Views.GXDLMSGprsSetupView.resources \
	Views/GXDLMSPppSetupView.resx,GXDLMSDirector.Views.GXDLMSPppSetupView.resources \
	Views/GXDLMSIp4SetupView.resx,GXDLMSDirector.Views.GXDLMSIp4SetupView.resources \
	Views/GXDLMSTcpUdpSetupView.resx,GXDLMSDirector.Views.GXDLMSTcpUdpSetupView.resources \
	Views/GXDLMSAutoConnectView.resx,GXDLMSDirector.Views.GXDLMSAutoConnectView.resources \
	Views/GXDLMSModemConfigurationView.resx,GXDLMSDirector.Views.GXDLMSModemConfigurationView.resources \
	Views/GXValueField.resx,GXDLMSDirector.Views.GXButton.resources \
	MainForm.resx,GXDLMSDirector.MainForm.resources \
	DevicePropertiesForm.resx,GXDLMSDirector.DevicePropertiesForm.resources \
	ManufacturerForm.resx,GXDLMSDirector.ManufacturerForm.resources \
	ManufacturersForm.resx,GXDLMSDirector.ManufacturersForm.resources \
	OBISCodeForm.resx,GXDLMSDirector.OBISCodeForm.resources \
	OBISCodesForm.resx,GXDLMSDirector.OBISCodesForm.resources \
	Properties/Resources.resx,GXDLMSDirector.Properties.Resources.resources \
	Views/GXDLMSSpecialDaysTableView.resx,GXDLMSDirector.Views.GXDLMSSpecialDaysTableView.resources \
	Views/GXDLMSScheduleView.resx,GXDLMSDirector.Views.GXDLMSScheduleView.resources \
	Views/GXDLMSIECOpticalPortSetupView.resx,GXDLMSDirector.Views.GXDLMSIECOpticalPortSetupView.resources \
	Views/GXDLMSHdlcSetupView.resx,GXDLMSDirector.Views.GXDLMSHdlcSetupView.resources \
	Views/GXDLMSActivityCalendarView.resx,GXDLMSDirector.Views.GXDLMSActivityCalendarView.resources \
	Views/GXDLMSRegisterActivationView.resx,GXDLMSDirector.Views.GXDLMSRegisterActivationView.resources \
	Views/GXDLMSClockView.resx,GXDLMSDirector.Views.GXDLMSClockView.resources \
	Views/GXDLMSDemandRegisterView.resx,GXDLMSDirector.Views.GXDLMSDemandRegisterView.resources \
	Views/GXDLMSExtendedRegisterView.resx,GXDLMSDirector.Views.GXDLMSExtendedRegisterView.resources \
	Views/GXDLMSRegisterView.resx,GXDLMSDirector.Views.GXDLMSRegisterView.resources \
	Views/GXDLMSProfileGenericView.resx,GXDLMSDirector.Views.GXDLMSProfileGenericView.resources \
	Views/GXDLMSDataView.resx,GXDLMSDirector.Views.GXDLMSDataView.resources 

EXTRAS = \
	Properties/Settings.settings \
	Resources/NewMnu.bmp \
	Resources/DeleteMnu.bmp \
	Resources/OpenMnu.bmp \
	Resources/OptionsMnu.bmp \
	Resources/ReadMnu.bmp \
	Resources/SaveMnu.bmp \
	Resources/WriteMnu.bmp \
	Resources/ConnectMnu.bmp \
	GXDLMSDirector.ico \
	Resources/warning.ico \
	gxdlmsdirector.in 

REFERENCES =  \
	$(GURUXCOMMON_LIBS) \
	$(GURUXNET_LIBS) \
	$(GURUXSERIAL_LIBS) \
	System \
	System.Core \
	System.Data \
	System.Drawing \
	System.Messaging \
	System.Windows.Forms \
	System.Xml \
	$(GURUXDLMS_LIBS)

DLL_REFERENCES =  \
	ZedGraph.dll

CLEANFILES = $(PROGRAMFILES) $(BINARIES) 

include $(top_srcdir)/Makefile.include

ZEDGRAPH_DLL = $(BUILD_DIR)/ZedGraph.dll
GXDLMSDIRECTOR = $(BUILD_DIR)/gxdlmsdirector

$(eval $(call emit-deploy-target,ZEDGRAPH_DLL))
$(eval $(call emit-deploy-wrapper,GXDLMSDIRECTOR,gxdlmsdirector,x))


$(eval $(call emit_resgen_targets))
$(build_xamlg_list): %.xaml.g.cs: %.xaml
	xamlg '$<'

$(ASSEMBLY_MDB): $(ASSEMBLY)

$(ASSEMBLY): $(build_sources) $(build_resources) $(build_datafiles) $(DLL_REFERENCES) $(PROJECT_REFERENCES) $(build_xamlg_list) $(build_satellite_assembly_list)
	mkdir -p $(shell dirname $(ASSEMBLY))
	$(ASSEMBLY_COMPILER_COMMAND) $(ASSEMBLY_COMPILER_FLAGS) -out:$(ASSEMBLY) -target:$(COMPILE_TARGET) $(build_sources_embed) $(build_resources_embed) $(build_references_ref)
