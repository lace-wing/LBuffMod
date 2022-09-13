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
        //TODO Test this
        public static int ContactTileNum(Vector2 position, int width, int height, int tileType = -1, int countActuated = 0, int[] tileShape = default)
        {
            int contactNum = 0;
            int startTileX = (int)(position.X / 16f);
            int startTileY = (int)(position.Y / 16f);
            int widthTile = (int)(width / 16f);
            int heightTile = (int)(height / 16f);
            int boundaryX0 = startTileX - 1 >= 0 ? startTileX - 1 : 0;
            int boundaryXMax = startTileX + widthTile + 2 <= Main.maxTilesX ? startTileX + widthTile + 2 : Main.maxTilesX;
            int boundaryY0 = startTileY - 1 >= 0 ? startTileY - 1 : 0;
            int boundaryYMax = startTileY + heightTile + 2 <= Main.maxTilesY ? startTileY + heightTile + 2 : Main.maxTilesY;

            Vector2 tilePosition = default(Vector2);
            for (int i = boundaryX0; i < boundaryXMax; i++)
            {
                for (int j = boundaryY0; j < boundaryYMax; j++)
                {
                    Tile tile = Main.tile[i, j];
                    if (tile == null || (tileType != -1 && tile.TileType != tileType) || (countActuated == 0 && tile.IsActuated) || (countActuated == 2 && !tile.IsActuated))
                    {
                        continue;
                    }
                    if (tileShape.Length > 0)
                    {
                        int p = 0;
                        for (int s = 0; s < tileShape.Length; s++)
                        {
                            int sV = tileShape[s];
                            if (sV == 0 && tile.Slope == SlopeType.Solid)
                            {
                                p++;
                                continue;
                            }
                            else if (sV == 1 && tile.Slope == SlopeType.SlopeDownLeft)
                            {
                                p++;
                                continue;
                            }
                            else if (sV == 2 && tile.Slope == SlopeType.SlopeDownRight)
                            {
                                p++;
                                continue;
                            }
                            else if (sV == 3 && tile.Slope == SlopeType.SlopeUpLeft)
                            {
                                p++;
                                continue;
                            }
                            else if (sV == 4 && tile.Slope == SlopeType.SlopeUpRight)
                            {
                                p++;
                                continue;
                            }
                            else if (sV == 5 && tile.IsHalfBlock)
                            {
                                p++;
                                continue;
                            }
                        }
                        if (p <= 0)
                        {
                            continue;
                        }
                    }
                    tilePosition.X = i * 16;
                    tilePosition.Y = j * 16;
                    int hBOffset = 0;
                    if (tile.IsHalfBlock)//半砖
                    {
                        hBOffset = 8;
                    }
                    if (tile.Slope == SlopeType.Solid)//正常的
                    {
                        if ((tilePosition.X < position.X + width && tilePosition.X + 16 >= position.X) && (tilePosition.Y + hBOffset <= position.Y + height && tilePosition.Y + 16 >= position.Y))
                        {
                            contactNum++;
                        }
                    }
                    else if (tile.Slope == SlopeType.SlopeDownLeft)
                    {
                        float OffsetX = Math.Clamp(position.Y - tilePosition.Y, 0, 16);
                        float OffsetY = Math.Clamp(position.X - tilePosition.X, 0, 16);
                        if ((tilePosition.X + OffsetX <= position.X + width && tilePosition.X + 16 >= position.X) && (tilePosition.Y <= position.Y + height && tilePosition.Y + OffsetY >= position.Y))
                        {
                            contactNum++;
                        }
                    }
                    else if (tile.Slope == SlopeType.SlopeDownRight)
                    {
                        float OffsetX = 16 - Math.Clamp(position.Y - tilePosition.Y, 0, 16);
                        float OffsetY = 16 - Math.Clamp(position.X - tilePosition.X, 0, 16);
                        if ((tilePosition.X <= position.X + width && tilePosition.X + OffsetX >= position.X) && (tilePosition.Y <= position.Y + height && tilePosition.Y + OffsetY >= position.Y))
                        {
                            contactNum++;
                        }
                    }
                    else if (tile.Slope == SlopeType.SlopeUpLeft)
                    {
                        float OffsetX = 16 - Math.Clamp(position.Y - tilePosition.Y, 0, 16);
                        float OffsetY = 16 - Math.Clamp(position.X - tilePosition.X, 0, 16);
                        if ((tilePosition.X + OffsetX <= position.X + width && tilePosition.X + 16 >= position.X) && (tilePosition.Y + OffsetY <= position.Y + height && tilePosition.Y + 16 >= position.Y))
                        {
                            contactNum++;
                        }
                    }
                    if (tile.Slope == SlopeType.SlopeUpRight)
                    {
                        float OffsetX = Math.Clamp(position.Y - tilePosition.Y, 0, 16);
                        float OffsetY = Math.Clamp(position.X - tilePosition.X, 0, 16);
                        if ((tilePosition.X <= position.X + width && tilePosition.X + OffsetX >= position.X) && (tilePosition.Y + OffsetY <= position.Y + height && tilePosition.Y + 16 >= position.Y))
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
