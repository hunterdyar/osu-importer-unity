# osu! Importer for Unity
Unity Importer for osu! rhythm game files.

Released under MIT license.

## How it Works

For information about what the different elements are, see the osu! file format. Described
here: https://osu.ppy.sh/wiki/en/Client/File_formats/osu_%28file_format%29

This will parse the file into a readable OSUBeatmap with mostly 1-1 properties from the file format. 
Where appropriate, the importer parses ints as booleans (ie:, a 0 or 1 int) or enums instead.

## Install
The easiest way to install is through the package manager through git.
### Install via Package Manager (Preferred)
1. Make sure Git is installed on your system.
2. Go to the package manager and choose the "+" icon, and "Add package from GIT url"
3. Enter the following:
    - > https://github.com/hunterdyar/osu-importer-unity.git
4. The package manager should download and install the package.
### Install Manually
If you don't have Git, you can install the package by downloading the zip file (The green 'Code' button on this page, and Download ZIP).
1. Create a folder alled 'osu-importer' in your Packages folder (where manifest is)
2. Copy the repo contents into that folder, such that package.json is in the root folder.

Alternatively, you can place the package anywhere in your assets folder, and it should still work; just delete the .json files.

Installing via git is preferred, as you can update without redownloading and reinstalling.