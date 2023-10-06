# osu-importer-unity
Unity Importer for osu! rhythm game files.

Currently a sample project as I hack it together. Will eventually make it a unity package you can install via git.

The osu! file format is described here: https://osu.ppy.sh/wiki/en/Client/File_formats/osu_%28file_format%29

This will parse the file into a readable OSUBeatmap with mostly 1-1 properties from the file format. 
Where appropriate, the importer parses ints as booleans (ie:, a 0 or 1 int) or enums instead.