using Terraria.ID;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LBuffMod.Common.GlobalItems;
using LBuffMod.Common.GlobalNPCs;
using LBuffMod.Common.LBuffGlobalProjectile;
using LBuffMod.Common.ModPlayers;

namespace LBuffMod.Common.Utilities
{
    public class LPhysicsUtil
    {
        public static Vector2 ForceToVelocity(Entity entity, Vector2 force, Vector2 targetPosition = default, Vector2 targetRectangle = default)
        {
            if (force == Vector2.Zero || !entity.active)
            {
                return Vector2.Zero;
            }

            float mass = 0;
            Vector2[] histVelocity = new Vector2[] { };
            Vector2[] histAcceleration = new Vector2[] { };
            Vector2 velocity = Vector2.Zero;
            bool shouldStop = false;

            if (entity is Player)
            {
                Player player = (Player)entity;
                LPhysicsPlayer lbPlr = player.GetModPlayer<LPhysicsPlayer>();
                mass = lbPlr.plrMass;
            }
            else if (entity is Projectile)
            {
                Projectile projectile = (Projectile)entity;
                LPhysicsGlobalProj lbProj = projectile.GetGlobalProjectile<LPhysicsGlobalProj>();
                mass = lbProj.projMass;
            }
            else if (entity is Item)
            {
                Item item = (Item)entity;
                LPhysicsGlobalItem lbItem = item.GetGlobalItem<LPhysicsGlobalItem>();
                mass = lbItem.itemMass;
            }
            else if (entity is NPC)
            {
                NPC npc = (NPC)entity;
                LPhysicsGlobalNPC lbNPC = npc.GetGlobalNPC<LPhysicsGlobalNPC>();
                mass = lbNPC.npcMass;
            }

            if (Collision.CheckAABBvAABBCollision(entity.position, entity.Size, targetPosition, targetRectangle))
            {
                shouldStop = true;
            }

            velocity = force / mass;

            if (shouldStop)
            {
                velocity *= 0;
            }

            return velocity;
        }
        public static Vector2 ElectromagneticForce2(Entity entity1, Entity entity2)
        {
            if (!entity1.active || !entity2.active)
            {
                return Vector2.Zero;
            }

            float rangeFactor = 1800;
            float rangeSqd = 0;
            bool canBeAttractedByMagnet1 = false;
            bool canBeAttractedByMagnet2 = false;
            bool hasMagnetism1 = false;
            bool hasMagnetism2 = false;
            int polarity1 = 0;
            int polarity2 = 0;
            float mfs1 = 0;
            float mfs2 = 0;
            int forceDir = 0;
            float distanceSqd = Vector2.DistanceSquared(entity1.Center, entity2.Center);
            float force = 0;
            float forceMult = 0;

            if (entity1 is Player)
            {
                Player player1 = (Player)entity1;
                LPhysicsPlayer lbPlr1 = player1.GetModPlayer<LPhysicsPlayer>();
                canBeAttractedByMagnet1 = lbPlr1.canBeAttractedByMagnet;
                hasMagnetism1 = lbPlr1.equippedMagnet;
                polarity1 = lbPlr1.polarity;
                mfs1 = lbPlr1.plrMFS;
            }
            else if (entity1 is Projectile)
            {
                Projectile projectile1 = (Projectile)entity1;
                LPhysicsGlobalProj lbProj1 = projectile1.GetGlobalProjectile<LPhysicsGlobalProj>();
                canBeAttractedByMagnet1 = lbProj1.canBeAttractedByMagnet;
                hasMagnetism1 = lbProj1.hasMagnetism;
                polarity1 = lbProj1.polarity;
                mfs1 = lbProj1.projMFS;
            }
            else if (entity1 is Item)
            {
                Item item1 = (Item)entity1;
                LPhysicsGlobalItem lbItem1 = item1.GetGlobalItem<LPhysicsGlobalItem>();
                canBeAttractedByMagnet1 = lbItem1.canBeAttractedByMagnet;
                hasMagnetism1 = lbItem1.hasMagnetism;
                polarity1 = lbItem1.polarity;
                mfs1 = lbItem1.itemMFS;
            }
            else if (entity1 is NPC)
            {
                NPC npc1 = (NPC)entity1;
                LPhysicsGlobalNPC lbNPC1 = npc1.GetGlobalNPC<LPhysicsGlobalNPC>();
                canBeAttractedByMagnet1 = lbNPC1.canBeAttractedByMagnet;
                hasMagnetism1 = lbNPC1.hasMagnetism;
                polarity1 = lbNPC1.polarity;
                mfs1 = lbNPC1.npcMFS;
            }
            else return Vector2.Zero;

            if (entity2 is Player)
            {
                Player player2 = (Player)entity2;
                LPhysicsPlayer lbPlr2 = player2.GetModPlayer<LPhysicsPlayer>();
                canBeAttractedByMagnet2 = lbPlr2.canBeAttractedByMagnet;
                hasMagnetism2 = lbPlr2.equippedMagnet;
                polarity2 = lbPlr2.polarity;
                mfs2 = lbPlr2.plrMFS;
            }
            else if (entity2 is Projectile)
            {
                Projectile projectile2 = (Projectile)entity2;
                LPhysicsGlobalProj lbProj2 = projectile2.GetGlobalProjectile<LPhysicsGlobalProj>();
                canBeAttractedByMagnet2 = lbProj2.canBeAttractedByMagnet;
                hasMagnetism2 = lbProj2.hasMagnetism;
                polarity2 = lbProj2.polarity;
                mfs2 = lbProj2.projMFS;
            }
            else if (entity2 is Item)
            {
                Item item2 = (Item)entity2;
                LPhysicsGlobalItem lbItem2 = item2.GetGlobalItem<LPhysicsGlobalItem>();
                canBeAttractedByMagnet2 = lbItem2.canBeAttractedByMagnet;
                hasMagnetism2 = lbItem2.hasMagnetism;
                polarity2 = lbItem2.polarity;
                mfs2 = lbItem2.itemMFS;
            }
            else if (entity2 is NPC)
            {
                NPC npc2 = (NPC)entity2;
                LPhysicsGlobalNPC lbNPC2 = npc2.GetGlobalNPC<LPhysicsGlobalNPC>();
                canBeAttractedByMagnet2 = lbNPC2.canBeAttractedByMagnet;
                hasMagnetism2 = lbNPC2.hasMagnetism;
                polarity2 = lbNPC2.polarity;
                mfs2 = lbNPC2.npcMFS;
            }
            else return Vector2.Zero;

            if (!hasMagnetism1 && !hasMagnetism2)
            {
                return Vector2.Zero;
            }

            rangeSqd = MathF.Max((MathF.Pow(mfs1 < 0 ? 0 : mfs1, 2) + MathF.Pow(mfs2 < 0 ? 0 : mfs2, 2)) * rangeFactor, 0.01f);

            forceDir = polarity1 * polarity2;
            forceDir = forceDir == 0 ? -1 : forceDir;

            if (distanceSqd <= rangeSqd)
            {

                if (hasMagnetism1)
                {
                    force += mfs1;
                    forceMult += 0.4f;
                }
                if (canBeAttractedByMagnet1)
                {
                    force += mfs1 * 0.5f;
                    forceMult += 0.2f;
                }
                if (hasMagnetism2)
                {
                    force += mfs2;
                    forceMult += 0.4f;
                }
                if (canBeAttractedByMagnet2)
                {
                    force += mfs2 * 0.5f;
                    forceMult += 0.2f;
                }

                force *= (rangeSqd - distanceSqd) / rangeSqd;
            }

            return (entity1.Center - entity2.Center).SafeNormalize(Vector2.Zero) * forceDir * force * forceMult;
        }

        public static void MagnetSphereTransformProj(Projectile msProj, Projectile targetProj)
        {
            if (msProj.type == ProjectileID.MagnetSphereBall && !targetProj.friendly && Collision.CheckAABBvAABBCollision(msProj.position, msProj.Size, targetProj.position, targetProj.Size))
            {
                targetProj.hostile = false;
                targetProj.GetGlobalProjectile<LPhysicsGlobalProj>().hasMagnetism = true;
                targetProj.GetGlobalProjectile<LPhysicsGlobalProj>().projMFS += 1;
                msProj.damage += (int)(targetProj.damage * 0.02f);
                msProj.CritChance += 1;
                msProj.timeLeft += msProj.damage > 600 ? -3 : msProj.damage > 300 ? 0 : msProj.damage > 150 ? 1 : 2;
            }
        }

        //TODO Implementation
        /// <summary>
        /// 接触物块的数量
        /// </summary>
        /// <param name="position"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="tileType">TileID.theTile, null for all the tiles</param>
        /// <param name="countActuated">0 for count unactuated only, 1 for count both, 2 for count actuated only</param>
        /// <param name="relativePosition">Only count tiles centered at listed relative postions to the given rectangle, 0 for left and above, 1 for above, 2 for right and above, 3 for left, 4 for exactly at the center, etc., null for all relative positions</param>
        /// <param name="tileShape"> Only count tiles in listed shapes, 1~4 for respective SlopeTypes, 5 for IsHalfBlock, null for all the shapes</param>
        /// <param name="overlapMode">Currently no use</param>
        /// <param name="countCorner">Only listed corners of tiles will be counted, 0 for left top, 1 for right top, 2 for left bottom rtc., null for none of them</param>
        /// <returns>An int represents the number of given tiles which contact with the given rectangle</returns>
        public static int ContactTileNum(Vector2 position, int width, int height, int[] tileType = default, int countActuated = 0, int[] relativePosition = default, int[] tileShape = default, int overlapMode = 2, int[] countCorner = default)
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
                    if (tile == null || (countActuated == 0 && tile.IsActuated) || (countActuated == 2 && !tile.IsActuated))
                    {
                        continue;
                    }
                    if (tileType != null)
                    {
                        int pT = 0;
                        for (int t = 0; t < tileType.Length; t++)
                        {
                            if (tile.TileType == tileType[t])
                            {
                                pT++;
                                continue;
                            }
                        }
                        if (pT <= 0)
                        {
                            continue;
                        }
                    }
                    if (relativePosition != null)
                    {
                        int pD = 0;
                        Vector2 tileCenter = new Vector2(i * 16 + 8, j * 16 + 8);
                        Vector2 targetCenter = new Vector2(position.X + width * 0.5f, position.Y + height * 0.5f);
                        for (int d = 0; d < relativePosition.Length; d++)
                        {
                            int dV = relativePosition[d];
                            if (dV == 0 && tileCenter.X < targetCenter.X && tileCenter.Y < targetCenter.Y)
                            {
                                pD++;
                                continue;
                            }
                            else if (dV == 1 && tileCenter.Y < position.Y)
                            {
                                pD++;
                                continue;
                            }
                            else if (dV == 2 && tileCenter.X > targetCenter.X && tileCenter.Y < targetCenter.Y)
                            {
                                pD++;
                                continue;
                            }
                            else if (dV == 3 && tileCenter.X < position.X)
                            {
                                pD++;
                                continue;
                            }
                            else if (dV == 4 && tileCenter == targetCenter)
                            {
                                pD++;
                                continue;
                            }
                            else if (dV == 5 && tileCenter.X > position.X + width)
                            {
                                pD++;
                                continue;
                            }
                            else if (dV == 6 && tileCenter.X < targetCenter.X && tileCenter.Y > targetCenter.Y)
                            {
                                pD++;
                                continue;
                            }
                            else if (dV == 7 && tileCenter.Y > position.Y + height)
                            {
                                pD++;
                                continue;
                            }
                            else if (dV == 8 && tileCenter.X > targetCenter.X && tileCenter.Y > targetCenter.Y)
                            {
                                pD++;
                                continue;
                            }
                        }
                        if (pD <= 0)
                        {
                            continue;
                        }
                    }
                    if (tileShape != null)
                    {
                        int pS = 0;
                        for (int s = 0; s < tileShape.Length; s++)
                        {
                            int sV = tileShape[s];
                            if (sV == 0 && tile.Slope == SlopeType.Solid)
                            {
                                pS++;
                                continue;
                            }
                            else if (sV == 1 && tile.Slope == SlopeType.SlopeDownLeft)
                            {
                                pS++;
                                continue;
                            }
                            else if (sV == 2 && tile.Slope == SlopeType.SlopeDownRight)
                            {
                                pS++;
                                continue;
                            }
                            else if (sV == 3 && tile.Slope == SlopeType.SlopeUpLeft)
                            {
                                pS++;
                                continue;
                            }
                            else if (sV == 4 && tile.Slope == SlopeType.SlopeUpRight)
                            {
                                pS++;
                                continue;
                            }
                            else if (sV == 5 && tile.IsHalfBlock)
                            {
                                pS++;
                                continue;
                            }
                        }
                        if (pS <= 0)
                        {
                            continue;
                        }
                    }
                    bool canCount = false;
                    tilePosition.X = i * 16;
                    tilePosition.Y = j * 16;
                    int hBOffset = 0;
                    bool cCorner0 = false;
                    bool cCorner1 = false;
                    bool cCorner2 = false;
                    bool cCorner3 = false;
                    if (countCorner != null)
                    {
                        for (int c = 0; c < countCorner.Length; c++)
                        {
                            int cV = countCorner[c];
                            if (cV == 0)
                            {
                                cCorner0 = true;
                            }
                            if (cV == 1)
                            {
                                cCorner1 = true;
                            }
                            if (cV == 2)
                            {
                                cCorner2 = true;
                            }
                            if (cV == 3)
                            {
                                cCorner3 = true;
                            }
                        }
                    }
                    if (tile.IsHalfBlock)//半砖
                    {
                        hBOffset = 8;
                    }
                    if (new Vector2(tilePosition.X, tilePosition.Y + hBOffset) == position + new Vector2(width, height) && !cCorner0 //左上角
                        || new Vector2(tilePosition.X + 16, tilePosition.Y + hBOffset) == new Vector2(position.X, position.Y + height) && !cCorner1 //右上角
                        || new Vector2(tilePosition.X, tilePosition.Y + 16) == new Vector2(position.X + width, position.Y - 0.01f) && !cCorner2 //左下角
                        || new Vector2(tilePosition.X + 16, tilePosition.Y + 16) == new Vector2(position.X, position.Y - 0.01f) && !cCorner3)
                    {
                        continue;
                    }
                    if (tile.Slope == SlopeType.Solid)//正常的
                    {
                        if (tilePosition.X <= position.X + width && tilePosition.X + 16 >= position.X
                            && tilePosition.Y + hBOffset <= position.Y + height && tilePosition.Y + 16 >= position.Y - 0.01f)
                        {
                            contactNum++;
                        }
                    }
                    else if (tile.Slope == SlopeType.SlopeUpRight)
                    {
                        float OffsetX = Math.Clamp(position.Y - tilePosition.Y, 0, 16);
                        float OffsetY = Math.Clamp(position.X + width - tilePosition.X, 0, 16);
                        if (tilePosition.X + OffsetX <= position.X + width && tilePosition.X + 16 >= position.X
                            && tilePosition.Y <= position.Y + height && tilePosition.Y + OffsetY >= position.Y - 0.01f)
                        {
                            contactNum++;
                        }
                    }
                    else if (tile.Slope == SlopeType.SlopeUpLeft)
                    {
                        float OffsetX = 16 - Math.Clamp(position.Y - tilePosition.Y, 0, 16);
                        float OffsetY = 16 - Math.Clamp(position.X - tilePosition.X, 0, 16);
                        if (tilePosition.X <= position.X + width && tilePosition.X + OffsetX >= position.X
                            && tilePosition.Y <= position.Y + height && tilePosition.Y + OffsetY >= position.Y - 0.01f)
                        {
                            contactNum++;
                        }
                    }
                    else if (tile.Slope == SlopeType.SlopeDownRight)
                    {
                        float OffsetX = 16 - Math.Clamp(position.Y + height - tilePosition.Y, 0, 16);
                        float OffsetY = 16 - Math.Clamp(position.X + width - tilePosition.X, 0, 16);
                        if (tilePosition.X + OffsetX <= position.X + width && tilePosition.X + 16 >= position.X
                            && tilePosition.Y + OffsetY <= position.Y + height && tilePosition.Y + 16 >= position.Y - 0.01f)
                        {
                            contactNum++;
                        }
                    }
                    else if (tile.Slope == SlopeType.SlopeDownLeft)
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
