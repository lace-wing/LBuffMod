using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LBuffMod.Common.ModPlayers
{
    public class LDebuffPlayer : ModPlayer
    {
        public bool royalGelOnFire = false;
        public static int royalGelFireDamage = -5;
        public bool sharkToothNecklaceBleeding = false;
        public bool stingerNecklaceBleedingAndPoison = false;
        public override void ResetEffects()
        {
            royalGelOnFire = false;
            sharkToothNecklaceBleeding = false;
            stingerNecklaceBleedingAndPoison = false;
        }
        public override void UpdateEquips()
        {
            if (royalGelOnFire)
            {
                Player.GetDamage(DamageClass.Generic) -= 0.2f;
            }
        }
        public override void UpdateBadLifeRegen()
        {
            //全局：根据持续时间增加伤害：所有伤害性原版debuff + 流血
            for (int i = 0; i < LBuffUtils.lDamagingDebuffs.Length; i++)
            {
                int buffIndex = Player.FindBuffIndex(LBuffUtils.lDamagingDebuffs[i]);
                if (buffIndex != -1)//TODO Balanced formula needed
                {
                    int additionalDamage = (int)(LBuffUtils.BuffIDToLifeRegen(LBuffUtils.lDamagingDebuffs[i]) * MathHelper.Lerp(-0.1f, 3f, Player.buffTime[buffIndex] / 6300f));
                    Player.lifeRegen += additionalDamage;
                    //Main.NewText("Player: buffTime: " + Player.buffTime[buffIndex] + " " + "Additional damage: " + additionalDamage);
                }
            }
            //流血真的流血了
            if (Player.HasBuff(BuffID.Bleeding))
            {
                Player.lifeRegen -= 6;
                if (Player.lifeRegen > 0)
                {
                    Player.lifeRegen = 0;
                }
            }
            //带电真的根据移动速度掉血了
            if (Player.HasBuff(BuffID.Electrified))
            {
                if (Player.velocity.X == 0 && (Player.controlLeft || Player.controlRight))
                {
                    Player.lifeRegen += 32;
                }
                if (Player.velocity != Vector2.Zero && (Player.controlLeft || Player.controlRight || Player.controlJump))
                {
                    float f = Vector2.Distance(Player.velocity, Vector2.Zero) / 144;
                    Player.lifeRegen -= f > 4 ? 4 : (int)f;
                }
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
                    damage += (int)(-LBuffUtils.BuffIDToLifeRegen(LBuffUtils.damagingDebuffsToBuff[i]) * MathHelper.Lerp(0.2f, 2f, Player.buffTime[buffIndex] / 21600));
                }
            }
            for (int i = 0; i < LBuffUtils.thermalDebuffs.Length; i++)
            {
                int buffIndex = Player.FindBuffIndex(LBuffUtils.damagingDebuffsToBuff[i]);
                if (buffIndex != -1)
                {
                    //火系debuff产生额外被暴击率
                    if (!crit)
                    {
                        int c = -LBuffUtils.BuffIDToLifeRegen(LBuffUtils.thermalDebuffs[i]) / 4;
                        crit = Main.rand.Next(1, 100) < c ? true : false;
                    }
                }
            }
            //流血增伤
            if (Player.HasBuff(BuffID.Bleeding))
            {
                int buffTime = Player.FindBuffIndex(BuffID.Bleeding);
                damage += (int)(damage * MathHelper.Lerp(0.1f, 0.5f, buffTime / 6300));
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
                    damage += (int)(-LBuffUtils.BuffIDToLifeRegen(LBuffUtils.damagingDebuffsToBuff[i]) * MathHelper.Lerp(0.2f, 2f, Player.buffTime[buffIndex] / 21600));
                }
            }
            for (int i = 0; i < LBuffUtils.thermalDebuffs.Length; i++)
            {
                int buffIndex = Player.FindBuffIndex(LBuffUtils.damagingDebuffsToBuff[i]);
                if (buffIndex != -1)
                {
                    //火系debuff产生额外被暴击率
                    if (!crit)
                    {
                        int c = -LBuffUtils.BuffIDToLifeRegen(LBuffUtils.thermalDebuffs[i]) / 4;
                        crit = Main.rand.Next(1, 100) < c ? true : false;
                    }
                }
            }
            //流血增伤
            if (Player.HasBuff(BuffID.Bleeding))
            {
                int buffTime = Player.FindBuffIndex(BuffID.Bleeding);
                damage += (int)(damage * MathHelper.Lerp(0.1f, 0.5f, buffTime / 6300));
            }
        }
        public override void ModifyHitNPC(Item item, NPC target, ref int damage, ref float knockback, ref bool crit)
        {
            //皇家凝胶施加着火
            if (royalGelOnFire)
            {
                target.AddBuff(BuffID.OnFire, 120);
            }
            //鲨牙项链施加流血
            if (sharkToothNecklaceBleeding)
            {
                target.AddBuff(BuffID.Bleeding, 120);
            }
            //甜心项链施加流血和中毒
            if (stingerNecklaceBleedingAndPoison)
            {
                target.AddBuff(BuffID.Bleeding, 120);
                target.AddBuff(BuffID.Poisoned, 60);
            }

        }
        public override void ModifyHitNPCWithProj(Projectile proj, NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            Main.NewText("NPC lifeRegen: " + target.lifeRegen);
            //血箭、血蛙、血蝠、血雨
            if (proj.type == ProjectileID.BloodArrow || proj.type == ProjectileID.VampireFrog || proj.type == ProjectileID.BatOfLight || proj.type == ProjectileID.BloodRain)
            {
                target.AddBuff(BuffID.Bleeding, 180);
            }
            //血荆棘、滴滴链球
            if (proj.type == ProjectileID.SharpTears || proj.type == ProjectileID.DripplerFlail)
            {
                target.AddBuff(BuffID.Bleeding, 720);
            }
            //皇家凝胶施加着火
            if (royalGelOnFire)
            {
                target.AddBuff(BuffID.OnFire, 60);
            }
            //鲨牙项链施加流血
            if (sharkToothNecklaceBleeding)
            {
                target.AddBuff(BuffID.Bleeding, 60);
            }
            //甜心项链施加流血和中毒
            if (stingerNecklaceBleedingAndPoison)
            {
                target.AddBuff(BuffID.Bleeding, 60);
                target.AddBuff(BuffID.Poisoned, 60);
            }
        }
    }
}
