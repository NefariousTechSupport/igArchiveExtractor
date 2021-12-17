# igArchiveExtractor

![igArchiveExtractor Logo](https://media.discordapp.net/attachments/852283501738065951/884608132016394240/image0.png?width=300&height=300)

A GUI tool for extracting files from .arc/.bld/.pak files from various games made with the Vicarious Visions Alchemy game engine

Join The [Skylander Reverse Engineering Discord Server](https://discord.gg/evFbgBpmMf) for help and updates!

### Features & Games:
| Game | Extracting IGA Files | Text Viewing
|---|---|---|
| Skylanders Spyro's Adventure (3DS/Wii/Wii U) | ✅ | ❌ |
| Skylanders Giants | ✅ | ✅ |
| Skylanders Swap Force | ✅ | ❌ |
| Skylanders Trap Team | ✅ | ❌ |
| Skylanders Superchargers | ✅ | ❌ |
| Skylanders Imaginators | ✅ | ❌ |

## Usage

* Start the program
* Load a file using the "File > Load File", then select the game and platform you wish to open
* Navigate to the file you want to extract and click "Extract File" or just click "Extract All"
* Select an output folder
* Wait
* Profit

If you're on mac or linux use wine but idk if that works

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
