using System;
using System.Drawing;

namespace TerrainGeneration
{

    public enum TerrainTypes
    {

        WATER,
        DEEP_WATER,
        GRASS,
        SAND,
        MOUNTAIN,
        SNOW,
        FOREST

    }

    public class Terrain
    {

        //Size of the map
        int width;
        int height;

        //Allows you to zoom in and out of the map
        float scale = 192;

        //The noise maps
        FastNoiseLite elevationNoise;
        FastNoiseLite moistureNosie;

        //Everything bellow this level is water
        public float waterLevel = 0.3f;


        //Everything above this level is mountain
        public float mountainLevel = 0.85f;

        //Adds to water level to create beaches.
        public float beachRange = 0.05f;
        //public to mountain level to create snow.
        private float snowRange = 0.15f;

        private TerrainTypes[,] terrain;
        private double[,] heights;

        public Terrain(int width, int height, float scale) {

            this.width = width;
            this.height = height;
            this.scale = scale;

        }

        //Generate the terrain and save to an image.
        public void generateTerrain()
        {

            terrain = new TerrainTypes[width, height];
            heights = new double[width, height];

            elevationNoise = new FastNoiseLite();
            moistureNosie = new FastNoiseLite();

            Random rnd = new Random();

            elevationNoise.SetSeed(rnd.Next(1,2048));
            moistureNosie.SetSeed(rnd.Next(1,2048));

            for (int y = 0; y < height; y++)
            {

                for (int x = 0;x < width; x++)
                {

                    double e = (1 * noise1(1 * x, 1 * y)) +
                        (.5f * noise1(2 * x, 2 * y)) + 
                        (0.25f * noise1(4 * x, 4 * y));

                    double m = (1 * noise2(1 * x, 1 * y)) +
                        (.5f * noise2(2 * x, 2 * y)) +
                        (0.25f * noise2(4 * x, 4 * y));

                    heights[x, y] = e;

                    e = (e + 1) / 2;
                    e = Math.Pow(e,2f);


                    if (e > waterLevel)
                    {

                        if (e > waterLevel + beachRange)
                        {

                            if(e > mountainLevel)
                            {

                                if(e > mountainLevel + snowRange && m > 0.5f)
                                {

                                    terrain[x, y] = TerrainTypes.SNOW;

                                }
                                else
                                {

                                    terrain[x, y] = TerrainTypes.MOUNTAIN;


                                }

                            }
                            else
                            {


                                if(m * e > 0.1f)
                                {


                                    terrain[x, y] = TerrainTypes.FOREST;

                                }
                                else
                                {

                                    terrain[x, y] = TerrainTypes.GRASS;

                                }


                            }


                        }
                        else
                        {

                            if (m > -.5f)
                            {

                                terrain[x, y] = TerrainTypes.SAND;

                            }
                            else
                            {

                                terrain[x, y] = TerrainTypes.GRASS;


                            }
                        }

                    }
                    else
                    {

                        if(e < waterLevel / 2)
                        {

                            terrain[x, y] = TerrainTypes.DEEP_WATER;

                        }
                        else
                        {

                            terrain[x, y] = TerrainTypes.WATER;

                        }


                    }

                }

            }


        }

        private double noise1(float x, float y)
        {

            return elevationNoise.GetNoise(x * (width / scale),y  * (height/ scale));

        }
        private double noise2(float x, float y)
        {

            return moistureNosie.GetNoise(x * (width / scale), y * (height / scale));

        }

        public void generateBitmap(String fileName)
        {

            Bitmap bitmap = new Bitmap(width, height);

            for (int x = 0; x < width; x++)
            {

                for (int y = 0; y < height; y++)
                {

                    if (terrain[x, y].Equals(TerrainTypes.WATER)) { bitmap.SetPixel(x, y, Color.Blue); }
                    if (terrain[x, y].Equals(TerrainTypes.GRASS)) { bitmap.SetPixel(x, y, Color.Green); }
                    if (terrain[x, y].Equals(TerrainTypes.SAND)) { bitmap.SetPixel(x, y, Color.Yellow); }
                    if (terrain[x, y].Equals(TerrainTypes.MOUNTAIN)) { bitmap.SetPixel(x, y, Color.Gray); }
                    if (terrain[x, y].Equals(TerrainTypes.SNOW)) { bitmap.SetPixel(x, y, Color.White); }
                    if (terrain[x, y].Equals(TerrainTypes.FOREST)) { bitmap.SetPixel(x, y, Color.DarkGreen); }
                    if (terrain[x, y].Equals(TerrainTypes.DEEP_WATER)) { bitmap.SetPixel(x, y, Color.DarkBlue); }

                }

            }

            bitmap.Save("output/" + fileName + ".png");

        }

        public void generateHeightMap(String fileName)
        {

            Bitmap heightMap = buildHeightMap();

        }


        public double[,] getHeightData()
        {

            return this.heights;

        }

        public Bitmap getHeightmap()
        {

            return buildHeightMap();

        }

        private Bitmap buildHeightMap()
        {

            Bitmap bitmap = new Bitmap(width, height);

            for (int x = 0; x < width; x++)
            {

                for (int y = 0; y < height; y++)
                {

                    double normalisedValue = (heights[x, y] + 1) / 2;
                    if (normalisedValue > 1) { normalisedValue = 1; }
                    if (normalisedValue < 0) { normalisedValue = 0; }

                    int colourValue = (int)(normalisedValue * 255);

                    Color color = Color.FromArgb(colourValue, colourValue, colourValue);
                    bitmap.SetPixel(x, y, color);
                }

            }

            return bitmap;

        }

    }
}
