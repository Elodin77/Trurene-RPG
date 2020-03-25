## Download/Run

1. Download the zip file **[here](https://drive.google.com/file/d/1N83e_JdJyPF9xXsPHbBKWK85RdzZC32b/view?usp=sharing)**, cloning/downloading the repository may not work.

2. Open the `Trurene RPG` folder inside the outer folder (with the same name). Then right click the `Trurene RPG.csproj` file and open with Notepad or other text editing program (NOT visual studio). Scroll through the document and delete the following block of text from the file (if it is there): 
```
  <PropertyGroup>
    <ManifestCertificateThumbprint>.....</ManifestCertificateThumbprint>
  </PropertyGroup>
  <PropertyGroup>
    <ManifestKeyFile>Trurene RPG_TemporaryKey.pfx</ManifestKeyFile>
  </PropertyGroup>
  <PropertyGroup>
    <GenerateManifests>true</GenerateManifests>
  </PropertyGroup>
  <PropertyGroup>
    <SignManifests>true</SignManifests>
  </PropertyGroup>
```
This simply removes some security stuff which would otherwise prevent Visual Studio from running the code.

It may take some time to download:
* Part 1 Documentation: 10000 words
* Part 2 Documentation: 2000 words
* Program: 3000 lines

# Trurene - The RPG
This is a game which I, DJ, have made as part of a submission for a Computer Science project. Expect bugs, errors and some incorrect information in the files contained in this repository. 


## Feedback

Feedback is greatly appreciated and can be given through the form at https://forms.gle/rNMHBPLkZixPSEyX6.

I would be happy for you to contact me for feedback or any other matter. My personal email is djimondjayasundera@gmail.com

## Gamestate file template

Below shows the template which can be used to create custom worlds without boundaries.

    [rows] [cols] [turnNum] [numVillages] [numShrines]
    [hawkPosRows] [hawkPosCols] 
    [maejaPosRows] [maejaPosCols]
    [questPosRows] [questPosCols]
    [auroraPosRows] [auroraPosCols]
    [auroraHealth] 
    [auroraMaxHealth]
    [auroraAccuracy] [auroraPower] [auroraTime]
    [auroraSpell1?] [auroraSpell2?] [auroraSpell3?] [auroraSpell4?]
    [auroraGold]
    [goblinKingPosRows] [goblinKingPosCols]
    [goblinKingHealth] 
    [goblinKingMaxHealth]
    [goblinKingAccuracy] [goblinKingPower] [goblinKingTime]
    [wolvesPosRows] [wolvesPosCols]
    [wolvesHealth] 
    [wolvesMaxHealth]
    [wolvesAccuracy] [wolvesPower] [wolvesTime]
    [villagePositions]
    [destroyedVillagePositions]
    [ShrinePositions]
    [ShrineSolved]

