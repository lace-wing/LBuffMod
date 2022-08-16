using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LBuffMod.Common.GlobalNPCs
{
    public class LDebuffGlobalNPC : GlobalNPC
    {
        public float lNegativeLifeRegenGlobalMultiplier = 1.2f;
        public override void UpdateLifeRegen(NPC npc, ref int damage)
        {
            if (npc.lifeRegen < 0)
            {
                npc.lifeRegen += (int)(npc.lifeRegen * (lNegativeLifeRegenGlobalMultiplier - 1));
                damage += (int)(damage * (lNegativeLifeRegenGlobalMultiplier - 1));
            }
        }
        public override void ModifyHitByItem(NPC npc, Player player, Item item, ref int damage, ref float knockback, ref bool crit)
        {
            if (npc.HasBuff(BuffID.OnFire))
            {
                if (!crit)
                {
                    crit = Main.rand.Next(1, 100) > 80 ? true : false;
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
    }
}
