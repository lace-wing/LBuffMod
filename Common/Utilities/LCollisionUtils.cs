using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LBuffMod.Common.Utilities
{
    public class LCollisionUtils
    {
        public static int ContactTileNum(Vector2 position, int width, int height, int tileType, bool countActuated = false)
        {
            int contactNum = 0;
            int startTileX = (int)(position.X / 16f);
            int startTileY = (int)(position.Y / 16f);
            int widthTile = (int)(width / 16f);
            int heightTile = (int)(height / 16f);
            int boundaryX0 = startTileX - 1 >= 0 ? startTileX - 1 : 0;
            int boundaryXMax = startTileX + widthTile <= Main.maxTilesX ? startTileX + widthTile : Main.maxTilesX;
            int boundaryY0 = startTileY - 1 >= 0 ? startTileY - 1 : 0;
            int boundaryYMax = startTileY + heightTile <= Main.maxTilesY ? startTileY + heightTile : Main.maxTilesY;

            Vector2 tilePosition = default(Vector2);//TODO Bug!!!!!!!!
            for (int i = boundaryX0; i < boundaryXMax; i++)
            {
                for (int j = boundaryY0; j < boundaryYMax; j++)
                {
                    Tile tile = Main.tile[i, j];
                    if (tile == null || tile.TileType != tileType)
                    {
                        continue;
                    }
                    tilePosition.X = i * 16;
                    tilePosition.Y = j * 16;
                    int hBOffset = 0;
                    if (tile.IsHalfBlock)
                    {
                        hBOffset = 8;
                    }
                    if (tile.Slope == SlopeType.Solid)
                    {
                        if (((tilePosition.X <= position.X && tilePosition.X + 16 >= position.X) || (tilePosition.X >= position.X && tilePosition.X <= position.X + width)) && ((tilePosition.Y + hBOffset <= position.Y && tilePosition.Y + 16 >= position.Y) || (tilePosition.Y + hBOffset >= position.Y && tilePosition.Y + hBOffset <= position.Y + height)))
                        {
                            contactNum++;
                        }
                    }
                }
            }

            return contactNum;
        }
    }
}
