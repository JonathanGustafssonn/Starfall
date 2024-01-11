using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Starfall.GameManagment;
using Starfall.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TiledSharp;

namespace Starfall.Map
{
    public class MapLoader
    {
        public TmxMap Map;
        Texture2D Tileset;
        public int TilesetTilesWide;
        public int TileWidth;
        public int TileHeight;
        
        public MapLoader(TmxMap map, Texture2D tileset, int tilesetTilesWide, int tileWidth, int tileHeight) 
        {
            this.Map = map;
            this.Tileset = tileset;
            this.TilesetTilesWide = tilesetTilesWide;
            this.TileWidth = tileWidth;
            this.TileHeight = tileHeight;
        }
        public void Draw()
        {
            for (var i = 0; i < Map.Layers.Count; i++)
            {
                for (var j = 0; j < Map.Layers[i].Tiles.Count; j++)
                {
                    int gid = Map.Layers[i].Tiles[j].Gid;
                    if (gid == 0)
                    {
                        
                    }
                    else
                    {
                        int tileFrame = gid - 1;
                        int column = tileFrame % (int)TilesetTilesWide;
                        int row = (int)Math.Floor((double)tileFrame / (double)TilesetTilesWide);
                        float X = (j % Map.Width) * Map.TileWidth;
                        float Y = (float)Math.Floor(j / (double)Map.Width) * Map.TileHeight;
                        Objects.BoundingBox tileSetBox = new Objects.BoundingBox(TileWidth * column, TileHeight * row, TileWidth, TileHeight);


                        //Global.SpriteBatch.Draw(Tileset, new Rectangle((int)X, (int)Y, (int)TileWidth, (int)TileHeight ),new Rectangle((int)tileSetBox.X, (int)tileSetBox.Y, (int)tileSetBox.Width, (int)tileSetBox.Height), Color.White);
                        Global.SpriteBatch.Draw(Tileset, new Vector2(X,Y), new Rectangle((int)tileSetBox.X, (int)tileSetBox.Y, (int)tileSetBox.Width, (int)tileSetBox.Height), Color.White);
                        // Global.SpriteBatch.Draw(Background, new Vector2(player.Hitbox.X, player.Hitbox.Y) ,null, Color.Green,0f, Vector2.Zero, new Vector2(player.Size.X / Background.Width,player.Size.Y / Background.Height), SpriteEffects.None, 0f);
                    }
                }
            }
        }
    }
}
