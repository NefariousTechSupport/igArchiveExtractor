# Intrinsic Games Archive File Structure

## Version 0x08 - Skylanders Spyro's Adventure Wii U, Skylanders Giants

#### Header

|    Address   |     Size     |     Description
|--------------|--------------|----------------------
|   00000000   |   00000004   | Magic Number (0x1A414749)
|   00000004   |   00000004   | Version Number (0x08)
|   0000000C   |   00000004   | Number of Files \[N]
|   00000010   |   00000004   | Block Size
|   0000001C   |   00000004   | Nametable Location
|   00000020   |   00000004   | Nametable Size
|   00000034   |    N * 4     | Checksums for something, each is 4 bytes long and there is one for each file
|   34 + N*4   |    N * C     | File Descriptors (Called Local File Headers in IGAE)

#### File Descriptors

|    Address   |     Size     |     Description
|--------------|--------------|-------------------------------
|   00000000   |   00000004   | Absolute offset of the file's starting location
|   00000004   |   00000004   | Size of the file when uncompressed
|   00000008   |   00000004   | The compression mode of the file

#### Compression Modes

|     Mode     |     Type
|--------------|-------------
|   20000000   | Lzma
|   FFFFFFFF   | Uncompressed

## Version 0x0A - Skylanders Swap Force

#### Header

|    Address   |     Size     |     Description
|--------------|--------------|----------------------
|   00000000   |   00000004   | Magic Number (0x1A414749)
|   00000004   |   00000004   | Version Number (0x08)
|   0000000C   |   00000004   | Number of Files \[N]
|   00000010   |   00000004   | Block Size
|   00000014   |   00000004   | Memory Pool Index
|   0000002C   |   00000004   | Nametable Location
|   00000030   |   00000004   | Nametable Size
|   00000038   |    N * 4     | Checksums for something, each is 4 bytes long and there is one for each file
|   38 + N*4   |    N * 10    | File Descriptors (Called Local File Headers in IGAE)

#### File Descriptors

|    Address   |     Size     |     Description
|--------------|--------------|-------------------------------
|   00000000   |   00000000   | Ordinal
|   00000004   |   00000004   | Absolute offset of the file's starting location
|   00000008   |   00000004   | Size of the file when uncompressed
|   0000000C   |   00000004   | The compression mode of the file

#### Compression Modes

|     Mode     |     Type
|--------------|-------------
|   10000000   | Zlib
|   FFFFFFFF   | Uncompressed

## Version 0x0A - Skylanders Lost Islands

#### Header

|    Address   |     Size     |     Description
|--------------|--------------|----------------------
|   00000000   |   00000004   | Magic Number (0x1A414749)
|   00000004   |   00000004   | Version Number (0x08)
|   0000000C   |   00000004   | Number of Files \[N]
|   00000018   |   00000004   | Block Size (Unconfirmed)
|   00000014   |   00000004   | Memory Pool Index
|   00000028   |   00000004   | Nametable Location
|   00000030   |   00000004   | Nametable Size
|   00000038   |    N * 4     | Checksums for something, each is 4 bytes long and there is one for each file
|   38 + N*4   |    N * 10    | File Descriptors (Called Local File Headers in IGAE)

#### File Descriptors

|    Address   |     Size     |     Description
|--------------|--------------|-------------------------------
|   00000000   |   00000000   | Ordinal
|   00000004   |   00000004   | Absolute offset of the file's starting location
|   00000008   |   00000004   | Size of the file when uncompressed
|   0000000C   |   00000004   | The compression mode of the file

#### Compression Modes

|     Mode     |     Type
|--------------|-------------
|   30000000   | Unknown, 16 bit value contains size, next one is unknown, then uncompressed data
|   FFFFFFFF   | Uncompressed

## Version 0x0A - Skylanders Trap Team

#### Header

|    Address   |     Size     |     Description
|--------------|--------------|----------------------
|   00000000   |   00000004   | Magic Number (0x1A414749)
|   00000004   |   00000004   | Version Number (0x08)
|   0000000C   |   00000004   | Number of Files \[N]
|   00000010   |   00000004   | Block Size
|   00000014   |   00000004   | Memory Pool Index
|   0000002C   |   00000004   | Nametable Location
|   00000030   |   00000004   | Nametable Size
|   00000038   |    N * 4     | Checksums for something, each is 4 bytes long and there is one for each file
|   38 + N*4   |    N * 10    | File Descriptors (Called Local File Headers in IGAE)

#### File Descriptors

|    Address   |     Size     |     Description
|--------------|--------------|-------------------------------
|   00000000   |   00000000   | Ordinal
|   00000004   |   00000004   | Absolute offset of the file's starting location
|   00000008   |   00000004   | Size of the file when uncompressed
|   0000000C   |   00000004   | The compression mode of the file

#### Compression Modes

|     Mode     |     Type
|--------------|-------------
|   10000000   | Zlib
|   FFFFFFFF   | Uncompressed