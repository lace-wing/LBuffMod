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
                //根据持续时间增加伤害
                for (int i = 0; i < LBuffUtilities.damagingDebuffsToBuff.Length; i++)
                {
                    int buffIndex = npc.FindBuffIndex(LBuffUtilities.damagingDebuffsToBuff[i]);
                    if (buffIndex != -1)//TODO Balanced formula needed
                    {
                        int additionalDamage = (int)(LBuffUtilities.BuffIDToLifeRegen(LBuffUtilities.damagingDebuffsToBuff[i]) * (Math.Clamp(npc.buffTime[buffIndex] / 3600, 1, 7) - 1.6f));
                        npc.lifeRegen += additionalDamage;
                        damage -= additionalDamage / 2;
                        Main.NewText("Debuff dps: " + damage + " " + "Additional damage: " + additionalDamage);
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
            for (int i = 0; i < LBuffUtilities.thermalDebuffs.Length; i++)
            {
                int buffIndex = npc.FindBuffIndex(LBuffUtilities.thermalDebuffs[i]);
                if (buffIndex != -1)
                {
                    int c = (-LBuffUtilities.BuffIDToLifeRegen(LBuffUtilities.thermalDebuffs[i]));
                    crit = Main.rand.Next(1, 100) < c ? true : false;
                    Main.NewText("Additional crit chance: " + c);
                }
            }
        }
        public override void ModifyHitByProjectile(NPC npc, Projectile projectile, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            for (int i = 0; i < LBuffUtilities.thermalDebuffs.Length; i++)
            {
                int buffIndex = npc.FindBuffIndex(LBuffUtilities.thermalDebuffs[i]);
                if (buffIndex != -1)
                {
                    int c = (-LBuffUtilities.BuffIDToLifeRegen(LBuffUtilities.thermalDebuffs[i]));
                    crit = Main.rand.Next(1, 100) < c ? true : false;
                    Main.NewText("Additional Crit: " + c);
                }
            }
        }
        public override void PostAI(NPC npc)
        {
            if (LBuffUtilities.NPCHasBuffInBuffSet(npc, LBuffUtilities.thermalDebuffs))
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
