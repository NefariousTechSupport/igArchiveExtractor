# igArchiveExtractor
 A command line utility for extracting files from a .arc file from Skylander's Spyro's Adventure for Wii U

## Usage

`igArchiveExtractor -i [input .arc file] -o [output folder]`

### Example

The following will extract all files from `Title.arc` and put its contents into the folder called `title`, do not include a `/` after the folder name

`igArchiveExtractor -i Title.arc -o title`

## Building
### Prerequisites
* Mac OS: make and g++ installed (can be done with brew)
* Linux: make and g++ installed (can be done with sudo apt-get)
* Windows: mingw installed (mingw32 for 32bit building, mingw64 for 64bit building)

### How to Build
* `cd` to the repo's root folder
* run `make osx` on mac os, run `make` and linux, and run `mingw32-make` or `mingw64-make` on windows depending on the bitness you desire

## To Do

* Add support for SG
* Figure out bld files (known as pak files on ssf and ssc)
