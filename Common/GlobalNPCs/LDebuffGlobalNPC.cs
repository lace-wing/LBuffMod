using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LBuffMod.Common.GlobalNPCs
{
    public class LDebuffGlobalNPC : GlobalNPC
    {
        public override void UpdateLifeRegen(NPC npc, ref int damage)
        {
            int buffType = LBuffUtilities.GetAllElements(npc.buffType);
            if (npc.lifeRegen < 0)
            {
                //根据持续时间增加伤害
                if (buffType == LBuffUtilities.GetAllElements(LBuffUtilities.damagingDebuffsToBuff))
                {
                    int additionalDamage = (int)(LBuffUtilities.BuffIDToLifeRegen(LBuffUtilities.GetAllElements(LBuffUtilities.damagingDebuffsToBuff)) * Math.Clamp(npc.buffTime[buffType] / 3600, 1, 7) * 0.6f - 1);
                    npc.lifeRegen += additionalDamage;
                    damage += additionalDamage;
                }
            }
            if (buffType == BuffID.Bleeding)
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
            if (npc.HasBuff(BuffID.OnFire))
            {
                if (!crit)
                {
                    crit = Main.rand.Next(1, 100) > 90 ? true : false;
                }
            }
        }
        public override void ModifyHitByProjectile(NPC npc, Projectile projectile, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            if (npc.HasBuff(LBuffUtilities.GetAllElements(LBuffUtilities.thermalDebuffs)))
            {
                if (!crit)
                {
                    int c = 100 - Math.Max(LBuffUtilities.BuffIDToLifeRegen(LBuffUtilities.GetAllElements(LBuffUtilities.thermalDebuffs)), 40);
                    crit = Main.rand.Next(1, 100) > c ? true : false;
                    Main.NewText(c);
                }
            }
        }
        public override void PostAI(NPC npc)
        {
            if (npc.HasBuff(LBuffUtilities.GetAllElements(LBuffUtilities.damagingDebuffsToBuff)))
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
