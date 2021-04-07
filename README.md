#Terrain Generation

This is a simple library for creating heightmaps for random terrain.

## How to

1) Create a new `Terrain` object, give it a size and a scale `Terrain terrain = new Terrain(width, height,scale);`
2) Generate the terrain with `terrain.generateTerrain();`
3) Generate heightmap and colour map using `terrain.generateHeightmap(filename);` & `terrain.generateBitmap(filename);`
4) Use the maps in your project.

Settings you can alter:

- Water Level - `terrain.waterLevel` : At what height should water appear (0-1).
- Mountain Level - `terrain.mountainLevel` : At what height should mountains appear (0-1)
- Beach Range - `terrain.beachHange` : How much the terrain can elevate from the water before it no longer has the chance to be beach.