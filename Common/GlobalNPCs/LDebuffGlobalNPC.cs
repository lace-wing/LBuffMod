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
                    int additionalDamage = (int)(LBuffUtilities.BuffIDToLifeRegen(LBuffUtilities.GetAllElements(LBuffUtilities.damagingDebuffsToBuff)) * Math.Clamp(npc.buffTime[buffType] / 7200, 1, 5) * 0.6f);
                    npc.lifeRegen += additionalDamage;
                    damage += additionalDamage;
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
            if (npc.HasBuff(BuffID.OnFire))
            {
                if (!crit)
                {
                    crit = Main.rand.Next(1, 100) > 80 ? true : false;
                }
            }
        }
        public override void PostAI(NPC npc)
        {
            if (npc.HasBuff(LBuffUtilities.GetAllElements(LBuffUtilities.damagingDebuffsToBuff)))
            {
                npc.velocity *= 0.95f;
            }
        }
    }
}
