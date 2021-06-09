TARGET		=	igArcExtractor.exe
BUILD		=	build
SOURCES		=	source
INCLUDES	=	include

CPPFLAGS	=	-Wall -O2 $(INCLUDE)

export INCLUDE	:=	$(foreach dir,$(INCLUDES),-I"$(CURDIR)/$(dir)")
export SOURCE	:=	$(foreach dir,$(SOURCES),$(wildcard $(dir)/*.cpp))
#export OFILES	:=	$(foreach dir,$(BUILD),$(notdir $(wildcard $(dir)/*.o)))
export OFILES	:=	$(foreach dir,$(SOURCE),./$(basename $(notdir $(dir))).o)
export OBJECTS	:=	$(foreach dir,$(OFILES),-o $(dir))

export OUTPUT	:=	$(CURDIR)/$(TARGET)

all:
	g++ -c $(SOURCE) $(INCLUDE)
	g++ -o $(TARGET) $(OFILES)