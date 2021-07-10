# -----------------------------------------------------------------------------
# The path to your g++ install, leaving it as g++ is normally good enough
# -----------------------------------------------------------------------------
GPP			=	g++

# -----------------------------------------------------------------------------
# IGAE     : The final output file for igae
# BUILD    : The folder where .o files go (not implemented)
# SOURCES  : The folder(s) containing the c++ sources
# INCLUDES : The folder containing the header files
# CPPFLAGS : The optional compiler flags to use
# -----------------------------------------------------------------------------
IGAE		=	igArchiveExtractor
BUILD		=	build
SOURCES		=	source
INCLUDES	=	include
CPPFLAGS	=	-Wall -g														# g is used as using O0, O1, or O2 results in false incorrect magic number errors, this'll be fixed in a later release
LDFLAGS		=	-static-libgcc -static-libstdc++ -static -LC:\Users\jaska\Documents\source\VScode\igArchiveExtractor -llibIGAE

# -----------------------------------------------------------------------------
# Builds lists of the files to be put into the compiler
# -----------------------------------------------------------------------------
export INCLUDE	:=	$(foreach dir,$(INCLUDES),-I"$(CURDIR)/$(dir)")
export SOURCE	:=	$(foreach dir,$(SOURCES),$(wildcard $(dir)/*.cpp))
#export OFILES	:=	$(foreach dir,$(BUILD),$(notdir $(wildcard $(dir)/*.o)))
export OFILES	:=	$(foreach dir,$(SOURCE),./$(basename $(notdir $(dir))).o)
export OBJECTS	:=	$(foreach dir,$(OFILES),-o $(dir))
export OUTPUT	:=	$(CURDIR)/$(IGAE)

all:
	"$(GPP)" -c $(SOURCE) $(INCLUDE) -D BUILD_LIB -I"lib" $(CPPFLAGS)
	"$(GPP)" -o $(IGAE) $(OFILES) $(LDFLAGS)
osx:
	"$(GPP)" -c $(SOURCE) $(INCLUDE) $(CPPFLAGS)
	"$(GPP)" -o $(IGAE) $(OFILES)
debug:
	@echo $(SOURCE)
winlib:
	"$(GPP)" -c lib/IGAE.cpp source/IGAE_helpers.cpp lib/entry.cpp -D BUILD_LIB -I"include" -I"lib" $(CPPFLAGS)
	"$(GPP)" -o libIGAE.dll IGAE.o helpers.o entry.o -static -static-libgcc -static-libstdc++
#copy "./libIGAE.dll" "./GUI/IGAE_GUI/IGAE_GUI/libIGAE.dll"
#copy "./lib/libIGAE.h" "./GUI/IGAE_GUI/IGAE_GUI/libIGAE.h"
	copy "./libIGAE.dll" "./GUI/IGAE_GUI/IGAE_GUI/bin/Debug/libIGAE.dll"