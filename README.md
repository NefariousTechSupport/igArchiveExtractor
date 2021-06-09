# igArchiveExtractor
 A command line utility for extracting files from a .arc file from Skylander's Spyro's Adventure for Wii and Wii U 

## Usage

`igArchiveExtractor [input .arc file] [output folder]`

*The output folder must already exist

## Example

The following will extract all files from `Title.arc` and put its contents into the folder called `title`, do not include a `/` after the folder name

`igArchiveExtractor Title.arc title`

## Building
* Ensure `make` is installed on mac and linux, on windows ensure mingw is installed
* `cd` to the folder containing the repo
* run `make` on mac os and linux, on windows run `mingw32-make`

## To Do

* Ensure Wii versions work
* Add in nametable support
* Compile versions for other versions to be added to the release (shouldn't be hard)
* Comment the code
* Stop using bubble sort for sorting the headers