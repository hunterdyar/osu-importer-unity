# osu! Importer for Unity
Unity Importer for osu! rhythm game files.

Released under MIT license.

## How it Works

For information about what the different elements are, see the osu! file format. Described
here: https://osu.ppy.sh/wiki/en/Client/File_formats/osu_%28file_format%29

This will parse the file into a readable OSUBeatmap with mostly 1-1 properties from the file format. 
Where appropriate, the importer parses ints as booleans (ie:, a 0 or 1 int) or enums instead.

## What are .osu files? Why not .osz?
When you download a beatmap from [osu!](https://osu.ppy.sh/beatmapsets?sort=plays_desc), you download an "[osz](https://osu.ppy.sh/wiki/en/Client/File_formats/osz_%28file_format%29)" file, which is just a .zip file. You can rename the extension to ".zip", and open it up ("extract" it) like any zip folder, to be a normal folder on your computer. This folder has the various audio files, and one or more .osu files, usually different difficulties.

I wrote this to support developers who are interested in using osu beatmap [authoring tools](https://osu.ppy.sh/wiki/en/Client/Beatmap_editor) for making their own rhythm game. This does not support .osz, because this is an editor-time plugin and not focused on opening osz files during runtime.

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
