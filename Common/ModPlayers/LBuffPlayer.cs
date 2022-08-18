using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LBuffMod.Common.ModPlayers
{
    public class LBuffPlayer : ModPlayer
    {
        public override void UpdateBadLifeRegen()
        {
            if (Player.lifeRegen < 0)
            {
                //全局：根据持续时间增加伤害：所有伤害性原版debuff + 流血
                for (int i = 0; i < LBuffUtils.lDamagingDebuffs.Length; i++)
                {
                    int buffIndex = Player.FindBuffIndex(LBuffUtils.lDamagingDebuffs[i]);
                    if (buffIndex != -1)//TODO Balanced formula needed
                    {
                        int additionalDamage = (int)(LBuffUtils.BuffIDToLifeRegen(LBuffUtils.lDamagingDebuffs[i]) * MathHelper.Lerp(-0.1f, 3.1f, Player.buffTime[buffIndex] / 10800f));
                        Player.lifeRegen += additionalDamage;
                        Main.NewText("Player: buffTime: " + Player.buffTime[buffIndex] + " " + "Additional damage: " + additionalDamage);
                    }
                }
            }
            if (Player.HasBuff(BuffID.Bleeding))
            {
                Player.lifeRegen -= 6;
                if (Player.lifeRegen > 0)
                {
                    Player.lifeRegen = 0;
                }
            }
        }
        public override void PostUpdate()
        {
            if (LBuffUtils.PlayerHasBuffInBuffSet(Player, LBuffUtils.damagingDebuffsToBuff))
            {
                Player.velocity *= 0.95f;
            }
        }
        public override void ModifyHitByNPC(NPC npc, ref int damage, ref bool crit)
        {
            for (int i = 0; i < LBuffUtils.damagingDebuffsToBuff.Length; i++)
            {
                int buffIndex = Player.FindBuffIndex(LBuffUtils.damagingDebuffsToBuff[i]);
                if (buffIndex != -1)
                {
                    //弱debuff通用增伤
                    damage += (int)(-LBuffUtils.BuffIDToLifeRegen(LBuffUtils.damagingDebuffsToBuff[i]) * MathHelper.Lerp(0.2f, 2f, Player.buffTime[buffIndex] / 5400));
                }
            }
            for (int i = 0; i < LBuffUtils.thermalDebuffs.Length; i++)
            {
                int buffIndex = Player.FindBuffIndex(LBuffUtils.damagingDebuffsToBuff[i]);
                if (buffIndex != -1)
                {
                    //火系debuff产生额外被暴击率
                    int c = -LBuffUtils.BuffIDToLifeRegen(LBuffUtils.thermalDebuffs[i]) / 4;
                    crit = Main.rand.Next(1, 100) < c ? true : false;
                }

            }
        }
        public override void ModifyHitByProjectile(Projectile proj, ref int damage, ref bool crit)
        {
            for (int i = 0; i < LBuffUtils.damagingDebuffsToBuff.Length; i++)
            {
                int buffIndex = Player.FindBuffIndex(LBuffUtils.damagingDebuffsToBuff[i]);
                if (buffIndex != -1)
                {
                    //弱debuff通用增伤
                    damage += (int)(-LBuffUtils.BuffIDToLifeRegen(LBuffUtils.damagingDebuffsToBuff[i]) * MathHelper.Lerp(0.2f, 2f, Player.buffTime[buffIndex] / 5400));
                }
            }
            for (int i = 0; i < LBuffUtils.thermalDebuffs.Length; i++)
            {
                int buffIndex = Player.FindBuffIndex(LBuffUtils.damagingDebuffsToBuff[i]);
                if (buffIndex != -1)
                {
                    //火系debuff产生额外被暴击率
                    int c = -LBuffUtils.BuffIDToLifeRegen(LBuffUtils.thermalDebuffs[i]) / 4;
                    crit = Main.rand.Next(1, 100) < c ? true : false;
                }

            }
        }
    }
}
