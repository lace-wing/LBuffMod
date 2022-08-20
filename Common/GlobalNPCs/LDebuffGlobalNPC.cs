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
        public override void UpdateLifeRegen(NPC npc, ref int damage)
        {
            //全局：根据持续时间增加伤害：所有伤害性原版debuff + 流血
            for (int i = 0; i < LBuffUtils.lDamagingDebuffs.Length; i++)
            {
                int buffIndex = npc.FindBuffIndex(LBuffUtils.lDamagingDebuffs[i]);
                if (buffIndex != -1)//TODO Balanced formula needed
                {
                    int additionalDamage = (int)(LBuffUtils.BuffIDToLifeRegen(LBuffUtils.lDamagingDebuffs[i]) * MathHelper.Lerp(-0.9f, 4f, npc.buffTime[buffIndex] / 43200f));
                    npc.lifeRegen += additionalDamage;
                    damage -= additionalDamage / 2;
                    //Main.NewText("buffTime: " + npc.buffTime[buffIndex] + " " + "Additional damage: " + additionalDamage + " lifeRegen: " + npc.lifeRegen);
                }
            }
            //流血真的流血了
            if (npc.HasBuff(BuffID.Bleeding))
            {
                npc.lifeRegen -= 6;
                if (npc.lifeRegen > 0)
                {
                    npc.lifeRegen = 0;
                }
            }
            //带电真的根据速度掉血了
            if (npc.HasBuff(BuffID.Electrified))
            {
                int f = Math.Clamp((int)(Vector2.Distance(npc.position, npc.oldPosition) * 128f), 8, 1024);
                npc.lifeRegen -= f;
            }
            //皇家凝胶常规火焰增伤
            for (int i = 0; i < Main.player.Length; i++)
            {
                if (Main.player[i].active && Main.player[i].GetModPlayer<LDebuffPlayer>().royalGelOnFire && Vector2.Distance(npc.Center, Main.player[i].Center) < 640)
                {
                    for (int j = 0; j < LBuffUtils.normalFireDebuffs.Length; j++)
                    {
                        if (npc.HasBuff(LBuffUtils.normalFireDebuffs[j]))
                        {
                            npc.lifeRegen += LDebuffPlayer.royalGelFireDamage;
                            damage -= (int)(LDebuffPlayer.royalGelFireDamage / 2f);
                        }
                    }
                }
            }
        }
        public override void ModifyHitByItem(NPC npc, Player player, Item item, ref int damage, ref float knockback, ref bool crit)
        {
            for (int i = 0; i < LBuffUtils.thermalDebuffs.Length; i++)
            {
                int buffIndex = npc.FindBuffIndex(LBuffUtils.thermalDebuffs[i]);
                if (buffIndex != -1)//有火系debuff则获得额外的被暴击率+增伤
                {
                    damage = (int)(damage * 1.05);
                    if (!crit && item.DamageType != DamageClass.Summon)
                    {
                        int c = -LBuffUtils.BuffIDToLifeRegen(LBuffUtils.thermalDebuffs[i]);
                        crit = Main.rand.Next(1, 100) < c ? true : false;
                    }
                }
            }
            for (int i = 0; i < LBuffUtils.poisonousDebuffs.Length; i++)
            {
                int buffIndex = npc.FindBuffIndex(LBuffUtils.poisonousDebuffs[i]);
                if (buffIndex != -1)
                {
                    if (crit && npc.buffTime[buffIndex] >= 1200)//超过20秒时暴击则按系数*时长增伤，时长减少至1/4
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
                    damage = (int)(damage * 1.05);
                    if (!crit && !projectile.minion && projectile.DamageType != DamageClass.Summon)
                    {
                        int c = -LBuffUtils.BuffIDToLifeRegen(LBuffUtils.thermalDebuffs[i]) - 6;//Lower add-crit chance
                        crit = Main.rand.Next(1, 100) < c ? true : false;
                    }
                }
            }
            for (int i = 0; i < LBuffUtils.poisonousDebuffs.Length; i++)
            {
                int buffIndex = npc.FindBuffIndex(LBuffUtils.poisonousDebuffs[i]);
                if (buffIndex != -1)
                {
                    if (crit && npc.buffTime[buffIndex] >= 1200)//超过20秒时暴击则按系数*时长增伤，时长减少至1/4
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
            }
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
