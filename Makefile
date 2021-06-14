# -----------------------------------------------------------------------------
# The path to your g++ install, leaving it as g++ is normally good enough
# -----------------------------------------------------------------------------
GPP			=	g++

# -----------------------------------------------------------------------------
# TARGET   : The final output file
# BUILD    : The folder where .o files go (not implemented)
# SOURCES  : The folder(s) containing the c++ sources
# INCLUDES : The folder containing the header files
# CPPFLAGS : The optional compiler flags to use
# -----------------------------------------------------------------------------
TARGET		=	igArcExtractor
BUILD		=	build
SOURCES		=	source
INCLUDES	=	include
CPPFLAGS	=	-Wall -g
LDFLAGS		=	-static-libgcc -static-libstdc++ -static

# -----------------------------------------------------------------------------
# Builds lists of the files to be put into the compiler
# -----------------------------------------------------------------------------
export INCLUDE	:=	$(foreach dir,$(INCLUDES),-I"$(CURDIR)/$(dir)")
export SOURCE	:=	$(foreach dir,$(SOURCES),$(wildcard $(dir)/*.cpp))
#export OFILES	:=	$(foreach dir,$(BUILD),$(notdir $(wildcard $(dir)/*.o)))
export OFILES	:=	$(foreach dir,$(SOURCE),./$(basename $(notdir $(dir))).o)
export OBJECTS	:=	$(foreach dir,$(OFILES),-o $(dir))
export OUTPUT	:=	$(CURDIR)/$(TARGET)

all:
	"$(GPP)" -c $(SOURCE) $(INCLUDE) $(CPPFLAGS)
	"$(GPP)" -o $(TARGET) $(OFILES) $(LDFLAGS)
