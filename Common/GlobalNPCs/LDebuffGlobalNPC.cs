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
                        int additionalDamage = (int)(LBuffUtils.BuffIDToLifeRegen(LBuffUtils.lDamagingDebuffs[i]) * MathHelper.Lerp(-0.9f, 6.9f, npc.buffTime[buffIndex] / 43200));
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
                if (buffIndex != -1)//有火系debuff则获得额外的被暴击率
                {
                    int c = -LBuffUtils.BuffIDToLifeRegen(LBuffUtils.thermalDebuffs[i]);
                    crit = Main.rand.Next(1, 100) < c ? true : false;
                }
            }
            for (int i = 0; i < LBuffUtils.poisonousDebuffs.Length; i++)
            {
                int buffIndex = npc.FindBuffIndex(LBuffUtils.poisonousDebuffs[i]);
                if (buffIndex != -1)
                {
                    if (crit && npc.buffTime[buffIndex] >= 1200)//超过20秒时暴击则按系数*时长增伤，时长减少至1/15
                    {
                        damage += (int)(-LBuffUtils.BuffIDToLifeRegen(LBuffUtils.poisonousDebuffs[i]) * MathHelper.Lerp(0.6f, 19.4f, npc.buffTime[buffIndex] / 43200));
                        damage = (int)Math.Pow(damage, 5 / 3);
                        npc.buffTime[buffIndex] /= 15;
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
                    int c = -LBuffUtils.BuffIDToLifeRegen(LBuffUtils.thermalDebuffs[i]) - 6;//Lower add-crit chance
                    crit = Main.rand.Next(1, 100) < c ? true : false;
                }
            }
            for (int i = 0; i < LBuffUtils.poisonousDebuffs.Length; i++)
            {
                int buffIndex = npc.FindBuffIndex(LBuffUtils.poisonousDebuffs[i]);
                if (buffIndex != -1)
                {
                    if (crit && npc.buffTime[buffIndex] >= 1200)//超过20秒时暴击则按系数*时长增伤，时长减少至1/15
                    {
                        damage += (int)(-LBuffUtils.BuffIDToLifeRegen(LBuffUtils.poisonousDebuffs[i]) * MathHelper.Lerp(0.6f, 19.4f, npc.buffTime[buffIndex] / 43200));
                        damage = (int)Math.Pow(damage, 5 / 3);
                        npc.buffTime[buffIndex] /= 15;
                    }
                }
            }
        }
        public override void PostAI(NPC npc)
        {
            if (LBuffUtils.NPCHasBuffInBuffSet(npc, LBuffUtils.thermalDebuffs))
            {
                npc.velocity *= 0.95f;
            }
            if (npc.type == NPCID.BrainofCthulhu)
            {
                npc.AddBuff(BuffID.Bleeding, 6);
            }
        }
    }
}
