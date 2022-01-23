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

### To Extract Textures:

* Once a file is loaded, find a texture or level.bld and double click it, alternatively, open an igz/level.bld directly.
* You'll be presented with the igz viewer, find an object of type igImage2 and you'll see a preview and an option to extract.

### To Replace Textures:

* Once a file is loaded, find a texture or level.bld and double click it, alternatively, open an igz/level.bld directly.
* You'll be presented with the igz viewer, find an object of type igImage2 and you'll see a preview and an option to replace.
* If you opened an iga initially, save the igz and replace the original level.bld and rebuild.
* If you opened an igz directly, your changes save automatically.

If you're on mac or linux use wine but idk if that works

### Features & Games:
| Game | Platform | Extracting IGA Files | Rebuilding IGA Files | Texture Extraction | Texture Replacement |
|---|---|---|---|---|---|
| Skylanders Spyro's Adventure | 3DS | ✅ | ❓ | ❌ | ❌ |
| | Wii | ✅ | ❓ | ❌ | ❌ |
| | Wii U | ✅ | ✅ | ❌ | ❌ |
| Skylanders Giants | 3DS | ✅ | ✅ | ❌ | ❌ |
| | Wii | ✅ | ✅ | ❌ | ❌ |
| | Wii U | ✅ | ✅ | ❌ | ❌ |
| | PS3 | ✅ | ✅ | ✅ | ✅ |
| | Xbox 360 | ✅ | ✅ | ❓ | ❓ |
| Skylanders Swap Force | 3DS | ✅ | ❓ | ❌ | ❌ |
| | Wii | ✅ | ❓ | ❌ | ❌ |
| | Wii U | ✅ | ✅ | ❌ | ❌ |
| | PS3 | ✅ | ❓ | ✅ | ✅ |
| | PS4 | ✅ | ❓ | ❓ | ❓ |
| | Xbox 360 | ✅ | ✅ | ❓ | ❓ |
| Skylanders Trap Team | 3DS | ✅ | ❓ | ❌ | ❌ |
| | Wii | ✅ | ✅ | ❌ | ❌ |
| | Wii U | ✅ | ✅ | ❌ | ❌ |
| | PS3 | ✅ | ✅ | ✅ | ✅ |
| | PS4 | ✅ | ✅ | ❓ | ❓ |
| | Xbox 360 | ✅ | ✅ | ❓ | ❓ |
| Skylanders Superchargers | Wii U | ✅ | ✅ | ❌ | ❌ |
| | PS3 | ✅ | ✅ | ✅ | ✅ |
| | PS4 | ✅ | ✅ | ❓ | ❓ |
| | Xbox 360 | ✅ | ✅ | ❓ | ❌ |
| Skylanders Imaginators | PS4 | ❌ | ❌ | ❌ | ❌ |
| | PS3 | ✅ | ✅ | ✅ | ✅ |
| | Xbox 360 | ✅ | ❓ | ❌ | ❌ |
| | Wii U | ✅ | ✅ | ❌ | ❌ |
| | Switch | ❌ | ❌ | ❌ | ❌ |

## Building
### Prerequisites
* Mac OS: no
* Linux: no
* Windows: dotnet 5.0 + visual studio

### How to Build
* Open the sln file in visual studio, and press ctrl + b to build, alternatively press f5 to build (if neccessary) and run

## Credits
* DTZxPorter: Figured out the HashSearch and CalculateSlop functions
* AdventureT: Texture Extraction code adapted from [IGZModelConverter](https://github.com/AdventureT/IgzModelConverter). Figured out that the games use FNV1A32 on their hashes
* LG-RZ: Explained certain aspects of IGA + IGZ files. Provided final RVTB ReadPackedInt code
* Drawdler: Drew the logo :)
* KillzXGaming: Referenced [IGA_PAK.cs](https://github.com/KillzXGaming/Switch-Toolbox/blob/master/File_Format_Library/FileFormats/CrashBandicoot/IGA_PAK.cs) when making this
* SixLabours: ImageSharp library was used for texture previews and also texture importing
* Nominom: BCnEncoder was used for texture previews and also texture importing 

## To Do

* Improve performance by using multithreading
* Make the table green
* Bring back text viewing and maybe text editing