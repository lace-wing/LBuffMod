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
            int startTileX = Math.Clamp((int)(position.X / 16f - 1), 0, Main.maxTilesX);
            int startTileY = Math.Clamp((int)(position.Y / 16f - 1), 0, Main.maxTilesY);
            int widthTile = Math.Clamp((int)(width / 16f + 3), 0, Main.maxTilesX);
            int heightTile = Math.Clamp((int)(height / 16f + 3), 0, Main.maxTilesY);

            Vector2 tilePosition = default(Vector2);
            for (int i = startTileX; i < startTileX + widthTile; i++)
            {
                for (int j = startTileY; j < startTileY + heightTile; j++)
                {
                    /*Dust dust = Dust.NewDustDirect(new Vector2((i) * 16, (j) * 16), 16, 16, DustID.AncientLight, 0, 0);
                    dust.velocity = Vector2.Zero;
                    dust.noGravity = true;*/
                    Tile tile = Main.tile[i, j];
                    if (tile == null || (tileType != -1 && tile.TileType != tileType) || (countActuated == 0 && tile.IsActuated) || (countActuated == 2 && !tile.IsActuated))
                    {
                        continue;
                    }
                    if (tileShape != null)
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
                        if (tilePosition.X <= position.X + width && tilePosition.X + 16 >= position.X
                            && tilePosition.Y + hBOffset <= position.Y + height && tilePosition.Y + 16 >= position.Y - 0.01f)
                        {
                            contactNum++;
                        }
                    }
                    else if (tile.Slope == SlopeType.SlopeDownLeft)
                    {
                        float OffsetX = Math.Clamp(position.Y - tilePosition.Y, 0, 16);
                        float OffsetY = Math.Clamp(position.X + width - tilePosition.X, 0, 16);
                        if (tilePosition.X + OffsetX <= position.X + width && tilePosition.X + 16 >= position.X
                            && tilePosition.Y <= position.Y + height && tilePosition.Y + OffsetY >= position.Y - 0.01f)
                        {
                            contactNum++;
                        }
                    }
                    else if (tile.Slope == SlopeType.SlopeDownRight)
                    {
                        float OffsetX = 16 - Math.Clamp(position.Y - tilePosition.Y, 0, 16);
                        float OffsetY = 16 - Math.Clamp(position.X - tilePosition.X, 0, 16);
                        if (tilePosition.X <= position.X + width && tilePosition.X + OffsetX >= position.X
                            && tilePosition.Y <= position.Y + height && tilePosition.Y + OffsetY >= position.Y - 0.01f)
                        {
                            contactNum++;
                        }
                    }
                    else if (tile.Slope == SlopeType.SlopeUpLeft)
                    {
                        float OffsetX = 16 - Math.Clamp(position.Y + height - tilePosition.Y, 0, 16);
                        float OffsetY = 16 - Math.Clamp(position.X + width - tilePosition.X, 0, 16);
                        if (tilePosition.X + OffsetX <= position.X + width && tilePosition.X + 16 >= position.X
                            && tilePosition.Y + OffsetY <= position.Y + height && tilePosition.Y + 16 >= position.Y - 0.01f)
                        {
                            contactNum++;
                        }
                    }
                    if (tile.Slope == SlopeType.SlopeUpRight)
                    {
                        float OffsetX = 16 - Math.Clamp(position.Y + height - tilePosition.Y, 0, 16);
                        float OffsetY = Math.Clamp(position.X - tilePosition.X, 0, 16);
                        if (tilePosition.X <= position.X + width && tilePosition.X + OffsetX >= position.X
                            && tilePosition.Y + OffsetY <= position.Y + height && tilePosition.Y + 16 >= position.Y - 0.01f)
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
