using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using LBuffMod.Common.ModPlayers;
using static LBuffMod.Common.Utilities.LBuffUtils;
using static LBuffMod.Common.Utilities.LCollisionUtils;

namespace LBuffMod.Common.GlobalNPCs
{
    public class LDebuffGlobalNPC : GlobalNPC
    {
        public override bool InstancePerEntity => true;

        public float fireSlowDownMultiplier = 0.04f;

        public bool royalGelNearby = false;
        public int totalRoyalGelFireDamage = 0;
        public float totalRoyalGelFireDamageMultiplier = 1;

        public bool volatilegeltinNearby = false;
        public int totalVolatileGelatinFireDamage = 1;
        public float totalVolatileGelatinFireDamageMultiplier = 0;

        public int npcOnSpikesTimer = 0;
        public override void ResetEffects(NPC npc)
        {
            royalGelNearby = false;
            totalRoyalGelFireDamage = 0;
            totalRoyalGelFireDamageMultiplier = 1;
            volatilegeltinNearby = false;
            totalVolatileGelatinFireDamage = 0;
            totalVolatileGelatinFireDamageMultiplier = 1;
        }
        public override void UpdateLifeRegen(NPC npc, ref int damage)
        {
            //检测皇家凝胶常规火焰增伤
            for (int i = 0; i < Main.player.Length; i++)
            {
                if (Main.player[i].active && Main.player[i].GetModPlayer<LDebuffPlayer>().royalGelOnFire && Vector2.Distance(npc.Center, Main.player[i].Center) < 640)
                {
                    royalGelNearby = true;
                    totalRoyalGelFireDamage += Main.player[i].GetModPlayer<LDebuffPlayer>().royalGelFireDamage;
                    totalRoyalGelFireDamageMultiplier += Main.player[i].GetModPlayer<LDebuffPlayer>().royalGelFireDamageMultiplier;
                }
            }
            //检测挥发明胶火焰增伤
            for (int i = 0; i < Main.player.Length; i++)
            {
                if (Main.player[i].active && Main.player[i].GetModPlayer<LDebuffPlayer>().volatileGelatinFire && Vector2.Distance(npc.Center, Main.player[i].Center) < 640)
                {
                    volatilegeltinNearby = true;
                    totalVolatileGelatinFireDamage += Main.player[i].GetModPlayer<LDebuffPlayer>().volatileGelatinFireDamage;
                    totalVolatileGelatinFireDamageMultiplier += Main.player[i].GetModPlayer<LDebuffPlayer>().volatileGelatinFireDamageMultiplier;
                }
            }
            //全局：根据持续时间增加伤害：所有伤害性原版debuff + 流血
            if (npc.lifeRegen < 0)
            {
                for (int i = 0; i < lDamagingDebuffs.Length; i++)
                {
                    int buffIndex = npc.FindBuffIndex(lDamagingDebuffs[i]);
                    if (buffIndex != -1)//TODO Balanced formula needed
                    {
                        int additionalDamage = (int)(BuffIDToLifeRegen(lDamagingDebuffs[i]) * MathHelper.Lerp(-0.3f, 2.1f, npc.buffTime[buffIndex] / 43200f));

                        if (NPCHasTheBuffInBuffSet(npc, lDamagingDebuffs[i], normalFireDebuffs))//是常规火焰
                        {
                            if (royalGelNearby)//皇家凝胶常规火焰增伤
                            {
                                additionalDamage += (int)(totalRoyalGelFireDamage * MathHelper.Lerp(0.3f, 3f, npc.buffTime[buffIndex] / 43200f));
                                additionalDamage = (int)(additionalDamage * totalRoyalGelFireDamageMultiplier);
                            }
                        }
                        if (NPCHasTheBuffInBuffSet(npc, lDamagingDebuffs[i], normalFireDebuffs) || NPCHasTheBuffInBuffSet(npc, lDamagingDebuffs[i], frostFireDebuffs))//是常规火、霜火
                        {
                            if (volatilegeltinNearby)//挥发明胶火焰增伤
                            {
                                additionalDamage += (int)(totalVolatileGelatinFireDamage * MathHelper.Lerp(0.3f, 3f, npc.buffTime[buffIndex] / 43200f));
                                additionalDamage = (int)(additionalDamage * totalVolatileGelatinFireDamageMultiplier);
                            }
                        }
                        npc.lifeRegen += additionalDamage;
                        damage -= additionalDamage / 2;
                    }
                }
            }
            //流血真的流血了
            if (npc.HasBuff(BuffID.Bleeding))
            {
                npc.lifeRegen += BuffIDToLifeRegen(BuffID.Bleeding);
                if (npc.lifeRegen > 0)
                {
                    npc.lifeRegen = 0;
                }
                if (npc.lifeRegen < 0)
                {
                    damage -= BuffIDToLifeRegen(BuffID.Bleeding);
                }
                if (Main.rand.NextBool(2))
                {
                    Dust.NewDustDirect(npc.TopLeft, npc.width, npc.height, DustID.Blood, npc.velocity.X * 0.1f, npc.velocity.Y * 0.1f, 32);
                }
            }
            //灼烧也掉血了
            if (npc.HasBuff(BuffID.Burning))
            {
                if (npc.lifeRegen > 0)
                {
                    npc.lifeRegen = 0;
                }
                npc.lifeRegen += BuffIDToLifeRegen(BuffID.Burning);
                damage -= BuffIDToLifeRegen(BuffID.Burning);
                if (Main.rand.NextBool(3))
                {
                    Dust dust = Dust.NewDustDirect(npc.TopLeft, npc.width, npc.height, DustID.SolarFlare, npc.velocity.X * 0.1f, npc.velocity.Y * 0.1f, 128);
                }
            }
            //带电真的根据速度掉血了
            if (npc.HasBuff(BuffID.Electrified))
            {
                int f = Math.Clamp((int)(Vector2.Distance(npc.position, npc.oldPosition) * 12f), 8, 1024);
                npc.lifeRegen -= f;
                damage += f;
                if (Main.rand.NextBool(3))
                {
                    Dust dust = Dust.NewDustDirect(npc.TopLeft, npc.width, npc.height, DustID.Electric, npc.velocity.X * 0.1f, npc.velocity.Y * 0.1f);
                }
            }
        }
        public override void ModifyHitByItem(NPC npc, Player player, Item item, ref int damage, ref float knockback, ref bool crit)
        {
            for (int i = 0; i < thermalDebuffs.Length; i++)
            {
                int buffIndex = npc.FindBuffIndex(thermalDebuffs[i]);
                if (buffIndex != -1)//有火系debuff则获得额外的被暴击率
                {
                    if (!crit && item.DamageType != DamageClass.Summon)
                    {
                        int c = (int)(-BuffIDToLifeRegen(thermalDebuffs[i]) * 0.6f);
                        crit = Main.rand.Next(100) < c ? true : false;
                    }
                }
            }
            for (int i = 0; i < poisonousDebuffs.Length; i++)
            {
                int buffIndex = npc.FindBuffIndex(poisonousDebuffs[i]);
                if (buffIndex != -1)
                {
                    if (crit && npc.buffTime[buffIndex] >= 1800)//超过30秒时暴击则按系数*时长增伤，时长减少至1/5
                    {
                        damage += (int)(-BuffIDToLifeRegen(poisonousDebuffs[i]) * MathHelper.Lerp(0.6f, 6f, npc.buffTime[buffIndex] / 43200f));
                        damage = (int)Math.Pow(damage, 9d / 8d);
                        npc.buffTime[buffIndex] = (int)(npc.buffTime[buffIndex] * 0.2f);
                    }
                }
            }
            //流血增伤
            if (npc.HasBuff(BuffID.Bleeding))
            {
                int buffTime = npc.FindBuffIndex(BuffID.Bleeding);
                damage += (int)(damage * MathHelper.Lerp(0.05f, 0.5f, buffTime / 43200f));
            }
        }
        public override void ModifyHitByProjectile(NPC npc, Projectile projectile, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            for (int i = 0; i < thermalDebuffs.Length; i++)
            {
                int buffIndex = npc.FindBuffIndex(thermalDebuffs[i]);
                if (buffIndex != -1)
                {
                    if (!crit && !projectile.minion && projectile.DamageType != DamageClass.Summon && projectile.DamageType != DamageClass.SummonMeleeSpeed)
                    {
                        int c = (int)(-BuffIDToLifeRegen(thermalDebuffs[i]) * 0.4f);//Lower add-crit chance
                        crit = Main.rand.Next(100) < c ? true : false;
                    }
                }
            }
            for (int i = 0; i < poisonousDebuffs.Length; i++)
            {
                int buffIndex = npc.FindBuffIndex(poisonousDebuffs[i]);
                if (buffIndex != -1)
                {
                    if (crit && npc.buffTime[buffIndex] >= 1800)//超过30秒时暴击则按系数*时长增伤，时长减少至1/5
                    {
                        damage += (int)(-BuffIDToLifeRegen(poisonousDebuffs[i]) * MathHelper.Lerp(0.6f, 6f, npc.buffTime[buffIndex] / 43200f));
                        damage = (int)Math.Pow(damage, 9d / 8d);
                        npc.buffTime[buffIndex] = (int)(npc.buffTime[buffIndex] * 0.2f);
                    }
                }
            }
            //流血增伤
            if (npc.HasBuff(BuffID.Bleeding))
            {
                int buffTime = npc.buffTime[npc.FindBuffIndex(BuffID.Bleeding)];
                damage = (int)(damage * MathHelper.Lerp(1.05f, 1.5f, buffTime / 43200f));
                //Main.NewText(damage);
            }
        }
        public override void PostAI(NPC npc)
        {
            //Test
            int x = ContactTileNum(npc.position, npc.width, npc.height, new int[] { TileID.EbonstoneBrick });
            if (x > 0 && Main.GameUpdateCount % 90 == 0)
            {
                Main.NewText($"{npc.FullName} is touching {x} brick(s)");
            }
            //检测火焰减速
            for (int i = 0; i < Main.player.Length; i++)
            {
                if (Main.player[i].active && Main.player[i].GetModPlayer<LDebuffPlayer>().fireSlowDown && Vector2.Distance(npc.Center, Main.player[i].Center) < 640)
                {
                    fireSlowDownMultiplier = 0.08f;
                }
            }
            //尖刺上流血
            int bleedingM = ContactTileNum(npc.position, npc.width, npc.height, new int[] { TileID.EbonstoneBrick });
            if (!npc.friendly)
                npc.AddBuff(BuffID.Bleeding, 4 * bleedingM);//尖刺上流血
            //站在陨石、狱石、狱石砖上时施加灼烧
            for (int i = 0; i < (int)(npc.width / 16f); ++i)
            {
                for (int j = 0; j < (int)(npc.height / 16f); j++)
                {
                    int bX = (int)npc.BottomLeft.X / 16 + i;
                    int bY = (int)npc.BottomLeft.Y / 16 + j;
                    bX = Math.Clamp(bX, 0, Main.maxTilesX);
                    bY = Math.Clamp(bY, 0, Main.maxTilesY);
                    Tile contactTile = Main.tile[bX, bY];
                    if (contactTile.HasUnactuatedTile && Main.tileSolid[contactTile.TileType] && (contactTile.TileType == TileID.Meteorite || contactTile.TileType == TileID.Hellstone || contactTile.TileType == TileID.HellstoneBrick))
                    {
                        npc.AddBuff(BuffID.Burning, 60);//为什么每次update只+5？
                    }
                }
            }
            int k = NPCBuffNumInBuffSet(npc, thermalDebuffs);
            npc.position -= npc.velocity * fireSlowDownMultiplier * k;//火焰减速
        }
        public override void OnHitPlayer(NPC npc, Player target, int damage, bool crit)
        {
            #region Electrify on hit
            //接触时触电，两者平分持续时间
            int npcElectrifiedIndex = npc.FindBuffIndex(BuffID.Electrified);
            int targetElectrifiedIndex = target.FindBuffIndex(BuffID.Electrified);
            if (npcElectrifiedIndex != -1 && targetElectrifiedIndex != -1)
            {
                npc.buffTime[npcElectrifiedIndex] = target.buffTime[targetElectrifiedIndex] = (int)((npc.buffTime[npcElectrifiedIndex] + target.buffTime[targetElectrifiedIndex]) * 0.5f);
            }
            if (targetElectrifiedIndex == -1 && npcElectrifiedIndex != -1)
            {
                npc.buffTime[npcElectrifiedIndex] = (int)((npc.buffTime[npcElectrifiedIndex]) * 0.5f);
                target.AddBuff(BuffID.Electrified, (int)((npc.buffTime[npcElectrifiedIndex]) * 0.5f));
            }
            if (npcElectrifiedIndex == -1 && targetElectrifiedIndex != -1)
            {
                target.buffTime[targetElectrifiedIndex] = (int)((target.buffTime[targetElectrifiedIndex]) * 0.5f);
                npc.AddBuff(BuffID.Electrified, (int)(target.buffTime[targetElectrifiedIndex] * 0.5f));
            }
            #endregion
            #region Pre-hard mode NPCs inflicting damaging debuffs
            //世吞、大中小噬魂怪、腐化者、世吞口水、腐化者口水近战
            if (npc.type == NPCID.EaterofWorldsBody || npc.type == NPCID.EaterofWorldsHead || npc.type == NPCID.EaterofWorldsTail || npc.type == NPCID.EaterofSouls || npc.type == NPCID.BigEater || npc.type == NPCID.LittleEater || npc.type == NPCID.Corruptor || npc.type == NPCID.VileSpitEaterOfWorlds || npc.type == NPCID.VileSpit)
            {
                if (!Main.hardMode)
                {
                    target.AddBuff(BuffID.OnFire, 24);//48 in expert world, 60 in master world
                }
                if (Main.hardMode)
                {
                    target.AddBuff(BuffID.CursedInferno, 48);
                }
            }
            //克脑、血腥僵尸、滴滴怪、僵尸人鱼、哥布林鲨、血鳗鱼头、血鱿鱼、恐惧鹦鹉螺
            if (npc.type == NPCID.BrainofCthulhu || npc.type == NPCID.BloodZombie || npc.type == NPCID.Drippler || npc.type == NPCID.ZombieMerman || npc.type == NPCID.GoblinShark || npc.type == NPCID.BloodEelHead || npc.type == NPCID.BloodSquid || npc.type == NPCID.BloodNautilus)
            {
                target.AddBuff(BuffID.Bleeding, 120);
            }
            //血肉墙、饿鬼、血蛭的近战
            if (npc.type == NPCID.TheHungry || npc.type == NPCID.TheHungryII || npc.type == NPCID.WallofFlesh || npc.type == NPCID.WallofFleshEye || npc.type == NPCID.LeechHead || npc.type == NPCID.LeechBody || npc.type == NPCID.LeechTail)
            {
                if (!Main.hardMode)
                {
                    target.AddBuff(BuffID.OnFire3, 120);
                }
                if (Main.hardMode)
                {
                    target.AddBuff(BuffID.CursedInferno, 120);
                }
            }
            #endregion
            #region Hard mode NPCs inflicting damaging debuffs
            //机械骷髅王、激光眼、毁灭者身体&尾近战
            if (npc.type == NPCID.SkeletronPrime || npc.type == NPCID.PrimeCannon || npc.type == NPCID.PrimeLaser || npc.type == NPCID.PrimeSaw || npc.type == NPCID.PrimeVice || npc.type == NPCID.Retinazer || npc.type == NPCID.TheDestroyerBody || npc.type == NPCID.TheDestroyerTail)
            {
                target.AddBuff(BuffID.CursedInferno, 120);
            }
            //魔焰眼、毁灭者头近战
            if (npc.type == NPCID.Spazmatism || npc.type == NPCID.TheDestroyer)
            {
                target.AddBuff(BuffID.CursedInferno, 240);
            }
            #endregion
            //TODO More NPCs to inflict debuffs!!!
        }
    }
}
