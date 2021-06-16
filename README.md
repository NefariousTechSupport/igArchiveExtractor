# igArchiveExtractor
 A command line utility for extracting files from a .arc file from Skylander's Spyro's Adventure for Wii U

## Usage

`igArchiveExtractor [input .arc file] [output folder]`

*The output folder must already exist

### Example

The following will extract all files from `Title.arc` and put its contents into the folder called `title`, do not include a `/` after the folder name

`igArchiveExtractor Title.arc title`

## Building
### Prerequisites
* Mac OS: make and g++ installed (can be done with brew)
* Linux: make and g++ installed (can be done with sudo apt-get)
* Windows: mingw installed (mingw32 for 32bit building, mingw64 for 64bit building)

### How to Build
* `cd` to the repo's root folder
* run `make osx` on mac os, run `make` and linux, and run `mingw32-make` or `mingw64-make` on windows depending on the bitness you desire

## To Do

* Add support for Wii files of SSA as well as SG, and STT
* Add in nametable support
* Compile versions for other versions to be added to the release (shouldn't be hard)
* Comment the code
* Stop using bubble sort for sorting the headers
