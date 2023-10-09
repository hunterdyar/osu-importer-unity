# osu! Importer for Unity
Unity Importer for osu! rhythm game files.

Released under MIT license.

## How it Works

For information about what the different elements are, see the osu! file format. Described
here: https://osu.ppy.sh/wiki/en/Client/File_formats/osu_%28file_format%29

This will parse the file into a readable OSUBeatmap with mostly 1-1 properties from the file format. 
Where appropriate, the importer parses ints as booleans (ie:, a 0 or 1 int) or enums instead.
