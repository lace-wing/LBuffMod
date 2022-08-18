using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace LBuffMod.Common.GlobalNPCs
{
    public class LDebuffGlobalNPC : GlobalNPC
    {
        public override void UpdateLifeRegen(NPC npc, ref int damage)
        {
            if (npc.lifeRegen < 0)
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
                        Main.NewText("buffTime: " + npc.buffTime[buffIndex] + " " + "Additional damage: " + additionalDamage);
                    }
                }
            }
            if (npc.HasBuff(BuffID.Bleeding))
            {
                npc.lifeRegen -= 6;
                if (npc.lifeRegen > 0)
                {
                    npc.lifeRegen = 0;
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
            //世吞、大中小噬魂怪、腐化者近战
            if (npc.type == NPCID.EaterofWorldsBody || npc.type == NPCID.EaterofWorldsHead || npc.type == NPCID.EaterofWorldsTail || npc.type == NPCID.EaterofSouls || npc.type == NPCID.BigEater || npc.type == NPCID.LittleEater || npc.type == NPCID.Corruptor)
            {
                if (!Main.hardMode)
                {
                    target.AddBuff(BuffID.OnFire, 48);//96 in expert world, 120 in master world
                }
                if (Main.hardMode)
                {
                    target.AddBuff(BuffID.CursedInferno, 96);
                }
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
            //机械骷髅王、激光眼近战
            if (npc.type == NPCID.SkeletronPrime || npc.type == NPCID.PrimeCannon || npc.type == NPCID.PrimeLaser || npc.type == NPCID.PrimeSaw || npc.type == NPCID.PrimeVice || npc.type == NPCID.Retinazer)
            {
                target.AddBuff(BuffID.CursedInferno, 120);
            }
            //魔焰眼近战
            if (npc.type == NPCID.Spazmatism)
            {
                target.AddBuff(BuffID.CursedInferno, 240);
            }
        }
    }
}
