using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using LBuffMod.Common.ModPlayers;

namespace LBuffMod.Common.GlobalNPCs
{
    public class LDebuffGlobalNPC : GlobalNPC
    {
        public override bool InstancePerEntity => true;

        public bool royalGelNearby = false;
        public int totalRoyalGelFireDamage = 0;
        public float totalRoyalGelFireDamageMultiplier = 1;
        public bool volatilegeltinNearby = false;
        public int totalVolatileGelatinFireDamage = 1;
        public float totalVolatileGelatinFireDamageMultiplier = 0;
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
                if (Main.player[i].active && Main.player[i].GetModPlayer<LDebuffPlayer>().royalGelOnFire && Vector2.Distance(npc.Center, Main.player[i].Center) < 810)
                {
                    royalGelNearby = true;
                    totalRoyalGelFireDamage += Main.player[i].GetModPlayer<LDebuffPlayer>().royalGelFireDamage;
                    totalRoyalGelFireDamageMultiplier += Main.player[i].GetModPlayer<LDebuffPlayer>().royalGelFireDamageMultiplier;
                }
            }
            //检测挥发明胶火焰增伤
            for (int i = 0; i < Main.player.Length; i++)
            {
                if (Main.player[i].active && Main.player[i].GetModPlayer<LDebuffPlayer>().volatileGelatinFireNOil && Vector2.Distance(npc.Center, Main.player[i].Center) < 810)
                {
                    volatilegeltinNearby = true;
                    totalVolatileGelatinFireDamage += Main.player[i].GetModPlayer<LDebuffPlayer>().volatileGelatinFireDamage;
                    totalVolatileGelatinFireDamageMultiplier += Main.player[i].GetModPlayer<LDebuffPlayer>().volatileGelatinFireDamageMultiplier;
                }
            }
            //全局：根据持续时间增加伤害：所有伤害性原版debuff + 流血
            for (int i = 0; i < LBuffUtils.lDamagingDebuffs.Length; i++)
            {
                int buffIndex = npc.FindBuffIndex(LBuffUtils.lDamagingDebuffs[i]);
                if (buffIndex != -1)//TODO Balanced formula needed
                {
                    int additionalDamage = (int)(LBuffUtils.BuffIDToLifeRegen(LBuffUtils.lDamagingDebuffs[i]) * MathHelper.Lerp(-0.3f, 3f, npc.buffTime[buffIndex] / 43200f));
                    if (LBuffUtils.NPCHasTheBuffInBuffSet(npc, LBuffUtils.lDamagingDebuffs[i], LBuffUtils.normalFireDebuffs))//是常规火焰
                    {
                        if (royalGelNearby)//皇家凝胶常规火焰增伤
                        {
                            additionalDamage += (int)(totalRoyalGelFireDamage * MathHelper.Lerp(1f, 6f, npc.buffTime[buffIndex] / 43200f));
                            additionalDamage = (int)(additionalDamage * totalRoyalGelFireDamageMultiplier);
                        }
                    }
                    if (LBuffUtils.NPCHasTheBuffInBuffSet(npc, LBuffUtils.lDamagingDebuffs[i], LBuffUtils.thermalDebuffs))//是火
                    {
                        if (royalGelNearby)//挥发明胶火焰增伤
                        {
                            additionalDamage += (int)(totalVolatileGelatinFireDamage * MathHelper.Lerp(1f, 6f, npc.buffTime[buffIndex] / 43200f));
                            additionalDamage = (int)(additionalDamage * totalVolatileGelatinFireDamageMultiplier);
                        }
                    }
                    npc.lifeRegen += additionalDamage;
                    damage -= additionalDamage / 2;
                    //Main.NewText("buffTime: " + npc.buffTime[buffIndex] + " " + "Additional damage: " + additionalDamage + " lifeRegen: " + npc.lifeRegen + " totalRGD: " + totalVolatileGelatinFireDamage);
                }
            }
            //流血真的流血了
            if (npc.HasBuff(BuffID.Bleeding))
            {
                npc.lifeRegen += LBuffUtils.BuffIDToLifeRegen(BuffID.Bleeding);
                if (npc.lifeRegen > 0)
                {
                    npc.lifeRegen = 0;
                }
                if (npc.lifeRegen < 0)
                {
                    damage -= LBuffUtils.BuffIDToLifeRegen(BuffID.Bleeding);
                }
            }
            //灼烧也掉血了
            if (npc.HasBuff(BuffID.Burning))
            {
                if (npc.lifeRegen > 0)
                {
                    npc.lifeRegen = 0;
                }
                npc.lifeRegen += LBuffUtils.BuffIDToLifeRegen(BuffID.Burning);
                damage -= LBuffUtils.BuffIDToLifeRegen(BuffID.Burning);
                if (Main.rand.NextBool(3))
                {
                    Dust dust = Dust.NewDustDirect(npc.TopLeft, npc.width, npc.height, DustID.SolarFlare, npc.velocity.X * 0.1f, npc.velocity.Y * 0.1f, 128);
                    //dust.
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
            for (int i = 0; i < LBuffUtils.thermalDebuffs.Length; i++)
            {
                int buffIndex = npc.FindBuffIndex(LBuffUtils.thermalDebuffs[i]);
                if (buffIndex != -1)//有火系debuff则获得额外的被暴击率
                {
                    if (!crit && item.DamageType != DamageClass.Summon)
                    {
                        int c = (int)(-LBuffUtils.BuffIDToLifeRegen(LBuffUtils.thermalDebuffs[i]) * 0.6f);
                        crit = Main.rand.Next(100) < c ? true : false;
                    }
                }
            }
            for (int i = 0; i < LBuffUtils.poisonousDebuffs.Length; i++)
            {
                int buffIndex = npc.FindBuffIndex(LBuffUtils.poisonousDebuffs[i]);
                if (buffIndex != -1)
                {
                    if (crit && npc.buffTime[buffIndex] >= 1800)//超过30秒时暴击则按系数*时长增伤，时长减少至1/4
                    {
                        damage += (int)(-LBuffUtils.BuffIDToLifeRegen(LBuffUtils.poisonousDebuffs[i]) * MathHelper.Lerp(0.6f, 60f, npc.buffTime[buffIndex] / 43200));
                        damage = (int)Math.Pow(damage, 7 / 5);
                        npc.buffTime[buffIndex] /= 4;
                    }
                }
            }
            //流血增伤
            if (npc.HasBuff(BuffID.Bleeding))
            {
                int buffTime = npc.FindBuffIndex(BuffID.Bleeding);
                damage += (int)(damage * MathHelper.Lerp(0.05f, 0.5f, buffTime / 43200));
            }
        }
        public override void ModifyHitByProjectile(NPC npc, Projectile projectile, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            for (int i = 0; i < LBuffUtils.thermalDebuffs.Length; i++)
            {
                int buffIndex = npc.FindBuffIndex(LBuffUtils.thermalDebuffs[i]);
                if (buffIndex != -1)
                {
                    if (!crit && !projectile.minion && projectile.DamageType != DamageClass.Summon)
                    {
                        int c = (int)(-LBuffUtils.BuffIDToLifeRegen(LBuffUtils.thermalDebuffs[i]) * 0.4f);//Lower add-crit chance
                        crit = Main.rand.Next(100) < c ? true : false;
                    }
                }
            }
            for (int i = 0; i < LBuffUtils.poisonousDebuffs.Length; i++)
            {
                int buffIndex = npc.FindBuffIndex(LBuffUtils.poisonousDebuffs[i]);
                if (buffIndex != -1)
                {
                    if (crit && npc.buffTime[buffIndex] >= 1800)//超过30秒时暴击则按系数*时长增伤，时长减少至1/4
                    {
                        damage += (int)(-LBuffUtils.BuffIDToLifeRegen(LBuffUtils.poisonousDebuffs[i]) * MathHelper.Lerp(0.6f, 60f, npc.buffTime[buffIndex] / 43200));
                        damage = (int)Math.Pow(damage, 7 / 5);
                        npc.buffTime[buffIndex] /= 4;
                    }
                }
            }
            //流血增伤
            if (npc.HasBuff(BuffID.Bleeding))
            {
                int buffTime = npc.FindBuffIndex(BuffID.Bleeding);
                damage += (int)(damage * MathHelper.Lerp(0.05f, 0.5f, buffTime / 43200));
            }
        }
        public override void PostAI(NPC npc)
        {
            if (npc.type == NPCID.BrainofCthulhu)
            {
                npc.AddBuff(BuffID.Bleeding, 60);
                //Main.NewText("GameUpdate: " + Main.GameUpdateCount % 60 + " buffTime: " + npc.buffTime[npc.FindBuffIndex(BuffID.Bleeding)] + " lifeRegen: " + npc.lifeRegen);
            }
            //站在陨石、狱石、狱石砖上时施加灼烧
            for (int i = 0; i < (int)(npc.width / 16f); i++)
            {
                int bX = (int)npc.BottomLeft.X / 16 + i;
                int bY = (int)npc.BottomLeft.Y / 16;
                bX = Math.Clamp(bX, 0, Main.maxTilesX);
                bY = Math.Clamp(bY, 0, Main.maxTilesY);
                Tile tileSteppingOn = Main.tile[bX, bY];
                if (tileSteppingOn.HasUnactuatedTile && Main.tileSolid[tileSteppingOn.TileType] && (tileSteppingOn.TileType == TileID.Meteorite || tileSteppingOn.TileType == TileID.Hellstone || tileSteppingOn.TileType == TileID.HellstoneBrick))
                {
                    npc.AddBuff(BuffID.Burning, 60);//为什么每次update只+5？
                }
            }
            int j = LBuffUtils.NPCBuffNumInBuffSet(npc, LBuffUtils.thermalDebuffs);
            npc.position -= npc.velocity * 0.05f * j;
            /*if (npc.type == NPCID.DD2EterniaCrystal)
            {
                for (int i = 0; i < LBuffUtils.lDamagingDebuffs.Length; i++)
                {
                    npc.buffImmune[LBuffUtils.lDamagingDebuffs[i]] = true;
                }
            }*/
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
        public override void OnHitByProjectile(NPC npc, Projectile projectile, int damage, float knockback, bool crit)
        {
            if (npc.type == NPCID.DD2EterniaCrystal)
            {
                //Main.NewText("HP: " + npc.life + "  lifeRegen: " + npc.lifeRegen + "\ntype: " + projectile.type + " damage: " + damage + " owner: " + projectile.owner);
            }
        }
    }
}
