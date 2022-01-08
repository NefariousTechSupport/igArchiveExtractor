# igArchiveExtractor

![igArchiveExtractor Logo](https://media.discordapp.net/attachments/852283501738065951/884608132016394240/image0.png?width=300&height=300)

A GUI tool for extracting files from .arc/.bld/.pak files from various games made with the Vicarious Visions Alchemy game engine

Join The [Skylander Reverse Engineering Discord Server](https://discord.gg/evFbgBpmMf) for help and updates!


## Usage

### General Use:

* Start the program
* Load a file using the "File > Load File", then select the game and platform you wish to open
* Navigate to the file you want to extract and click "Extract File" or just click "Extract All"
* Select an output folder
* Wait

### To Rebuild IGA Files:

* Once a file is laoded, extract all to the same folder the IGA is in.
* Navigate to File > Build.
* From the build window you can save the settings as a CSV, once you save you can close IGAE.
* Edit the extracted files with your desired changes.
* Once happy with your edits, reopen IGAE, navigate to File > Build, and load the CSV.
* Click Build, select your destination, and you'll have an IGA file.

### To View Text:

* Once a file is loaded, find a text asset and double click it.

### To Extract Textures:

* Once a file is loaded, find a texture and double click it.
* You''ll be presented with the option to save as, select a location, and save.

If you're on mac or linux use wine but idk if that works

### Features & Games:
| Game | Platform | Extracting IGA Files | Rebuilding IGA Files | Text Viewing | Texture Extraction |
|---|---|---|---|---|---|
| Skylanders Spyro's Adventure | 3DS | ✅ | ❓ | ❌ | ❌ |
| | Wii | ✅ | ❓ | ❌ | ❌ |
| | Wii U | ✅ | ✅ | ❌ | ❌ |
| Skylanders Giants | 3DS | ✅ | ✅ | ✅ | ✅ |
| | Wii | ✅ | ✅ | ✅ | ❓ |
| | Wii U | ✅ | ✅ | ✅ | ❌ |
| | PS3 | ✅ | ✅ | ✅ | ✅ |
| | Xbox 360 | ✅ | ✅ | ✅ | ❓ |
| Skylanders Swap Force | 3DS | ✅ | ❓ | ❌ | ❌ |
| | Wii | ✅ | ❓ | ❌ | ❌ |
| | Wii U | ✅ | ❓ | ❌ | ❌ |
| | PS3 | ✅ | ❓ | ❌ | ❌ |
| | PS4 | ✅ | ❓ | ❌ | ❌ |
| | Xbox 360 | ✅ | ❓ | ❌ | ❌ |
| Skylanders Trap Team | 3DS | ✅ | ❓ | ❌ | ❌ |
| | Wii | ✅ | ✅ | ✅ | ❓ |
| | Wii U | ✅ | ✅ | ✅ | ❓ |
| | PS3 | ✅ | ✅ | ✅ | ✅ |
| | PS4 | ✅ | ✅ | ✅ | ❓ |
| | Xbox 360 | ✅ | ✅ | ✅ | ❓ |
| Skylanders Superchargers | Wii U | ✅ | ✅ | ✅ | ❌ |
| | PS3 | ✅ | ✅ | ✅ | ❌ |
| | PS4 | ✅ | ✅ | ✅ | ❌ |
| | Xbox 360 | ✅ | ✅ | ✅ | ❌ |
| Skylanders Imaginators | PS4 | ✅ | ❌ | ❌ | ❌ |
| | PS3 | ✅ | ✅ | ❌ | ❌ |
| | PS4 | ✅ | ❌ | ❌ | ❌ |
| | Xbox 360 | ✅ | ❓ | ❌ | ❌ |

## Building
### Prerequisites
* Mac OS: no
* Linux: no
* Windows: dotnet 5.0 + visual studio

### How to Build
* Open the sln file in visual studio, and press ctrl + b to build, alternatively press f5 to build (if neccessary) and run

## To Do

* Improve performance by using multithreading
* Add support for rebuilding these files
* Add support for editing igz files
* Make the table green
