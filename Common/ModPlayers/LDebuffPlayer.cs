using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LBuffMod.Content.Buffs;
using Terraria.DataStructures;

namespace LBuffMod.Common.ModPlayers
{
    public class LDebuffPlayer : ModPlayer
    {
        public bool royalGelOnFire = false;
        public static int royalGelFireDamage = -6;
        public bool sharkToothNecklaceBleeding = false;
        public bool stingerNecklaceBleedingAndPoison = false;
        public bool madnessDebuff = false;
        public bool hairdressersWhiteSilkStockings = false;
        public bool woodArmorSet = false;
        public override void ResetEffects()
        {
            royalGelOnFire = false;
            sharkToothNecklaceBleeding = false;
            stingerNecklaceBleedingAndPoison = false;
            madnessDebuff = false;
            hairdressersWhiteSilkStockings = false;
            woodArmorSet = false;
        }
        public override void UpdateEquips()
        {
            if (royalGelOnFire)//皇家凝胶降伤
            {
                Player.GetDamage(DamageClass.Generic) -= 0.2f;
            }
            if (madnessDebuff)//发电提升属性
            {
                Player.statLifeMax2 += 30;
                Player.statManaMax2 += 30;
                Player.statDefense -= 3;
                if (Player.HasBuff(BuffID.Electrified))//同时发电和带电
                {
                    Player.moveSpeed += 0.1f;
                    Player.noKnockback = true;
                    Player.GetDamage(DamageClass.Magic) += 0.1f;
                    Player.GetDamage(DamageClass.Summon) += 0.1f;
                    Player.GetDamage(DamageClass.Melee) -= 0.1f;
                    Player.GetDamage(DamageClass.Ranged) -= 0.1f;
                }
            }
            //木套效果
            if (Player.armor[0].type == ItemID.WoodHelmet && Player.armor[1].type == ItemID.WoodBreastplate && Player.armor[2].type == ItemID.WoodGreaves)
            {
                woodArmorSet = true;
                if (Main.rand.NextBool(180))
                {
                    Player.AddBuff(BuffID.DryadsWard, 90);
                }
                Player.moveSpeed += 0.2f;
                Player.noFallDmg = true;
            }
            //红木套效果
            if (Player.armor[0].type == ItemID.RichMahoganyHelmet && Player.armor[1].type == ItemID.RichMahoganyBreastplate && Player.armor[2].type == ItemID.RichMahoganyGreaves)
            {
                woodArmorSet = true;
                if (Main.rand.NextBool(120))
                {
                    Player.AddBuff(BuffID.DryadsWard, 90);
                }
                Player.buffImmune[BuffID.Poisoned] = true;
                Player.lifeForce = true;
            }
            //针叶木效果
            if (Player.armor[0].type == ItemID.BorealWoodHelmet && Player.armor[1].type == ItemID.BorealWoodBreastplate && Player.armor[2].type == ItemID.BorealWoodGreaves)
            {
                woodArmorSet = true;
                if (Main.rand.NextBool(180))
                {
                    Player.AddBuff(BuffID.DryadsWard, 90);
                }
                Player.buffImmune[BuffID.Chilled] = true;
                Player.statDefense += 4;
                Player.noKnockback = true;
            }
            //棕榈木套效果
            if (Player.armor[0].type == ItemID.PalmWoodHelmet && Player.armor[1].type == ItemID.PalmWoodBreastplate && Player.armor[2].type == ItemID.PalmWoodGreaves)
            {
                woodArmorSet = true;
                if (Main.rand.NextBool(180))
                {
                    Player.AddBuff(BuffID.DryadsWard, 90);
                }
                Player.buffImmune[BuffID.Wet] = true;
                Player.fishingSkill += 25;
                Player.hasFloatingTube = true;
            }
            //黑檀木套效果
            if (Player.armor[0].type == ItemID.EbonwoodHelmet && Player.armor[1].type == ItemID.EbonwoodBreastplate && Player.armor[2].type == ItemID.EbonwoodGreaves)
            {
                woodArmorSet = true;
                Player.buffImmune[BuffID.OnFire] = true;
                Player.buffImmune[BuffID.CursedInferno] = true;
                Player.wingTimeMax += 30;
            }
            //暗影木套效果
            if (Player.armor[0].type == ItemID.ShadewoodHelmet && Player.armor[1].type == ItemID.ShadewoodBreastplate && Player.armor[2].type == ItemID.ShadewoodGreaves)
            {
                woodArmorSet = true;
                Player.buffImmune[BuffID.OnFire] = true;
                Player.GetDamage(DamageClass.Generic) += 0.15f;
                if (Player.lifeRegen >= 2)
                {
                    Player.lifeRegen -= 2;
                }
            }
            //珍珠木套效果
            if (Player.armor[0].type == ItemID.PearlwoodHelmet && Player.armor[1].type == ItemID.PearlwoodBreastplate && Player.armor[2].type == ItemID.PearlwoodGreaves)
            {
                woodArmorSet = true;
                Player.buffImmune[BuffID.Frostburn] = true;
                Player.AddBuff(BuffID.DryadsWard, 90);
                Player.statDefense += 8;
                Player.statManaMax2 += 30;
                Player.wingTimeMax += 60;
            }
            //阴森木套效果
            if (Player.armor[0].type == ItemID.SpookyHelmet && Player.armor[1].type == ItemID.SpookyBreastplate && Player.armor[2].type == ItemID.SpookyLeggings)
            {
                woodArmorSet = true;
                Player.buffImmune[BuffID.ShadowFlame] = true;
                Player.whipRangeMultiplier += 0.15f;
            }
            //所有木套效果
            if (woodArmorSet)
            {
                Player.statDefense += 2;
                Player.moveSpeed *= 1.05f;
            }
        }
        public override void PostUpdateEquips()
        {
            base.PostUpdateEquips();
        }
        public override void UpdateBadLifeRegen()
        {
            //全局：根据持续时间增加伤害：所有伤害性原版debuff + 流血
            for (int i = 0; i < LBuffUtils.lDamagingDebuffs.Length; i++)
            {
                int buffIndex = Player.FindBuffIndex(LBuffUtils.lDamagingDebuffs[i]);
                if (buffIndex != -1)//TODO Balanced formula needed
                {
                    int additionalDamage = (int)(LBuffUtils.BuffIDToLifeRegen(LBuffUtils.lDamagingDebuffs[i]) * MathHelper.Lerp(-0.3f, 3f, Player.buffTime[buffIndex] / 6300f));
                    Player.lifeRegen += additionalDamage;
                    if (LBuffUtils.lDamagingDebuffs[i] == BuffID.Electrified && madnessDebuff)
                    {
                        Player.lifeRegen -= additionalDamage;
                    }
                    if (LBuffUtils.lDamagingDebuffs[i] == BuffID.Burning)//灼烧额外伤害-80%
                    {
                        Player.lifeRegen -= (int)(additionalDamage * 0.8f);
                    }
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
                if (Player.controlLeft || Player.controlRight)
                {
                    Player.lifeRegen += 32;
                }
                if (Player.velocity != Vector2.Zero && (Player.controlLeft || Player.controlRight || Player.controlJump) && !madnessDebuff)
                {
                    int f = Math.Clamp((int)(Vector2.Distance(Player.position, Player.oldPosition) / 1f), 0, 96);
                    Player.lifeRegen -= f;
                    if (madnessDebuff && f > 0)
                    {
                        Player.lifeRegen += f / 2;
                    }
                }
                if (madnessDebuff)
                {
                    Player.lifeRegen += 6;
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
            //白丝发电
            if (hairdressersWhiteSilkStockings)
            {
                if (damage >= 4)
                {
                    Player.AddBuff(BuffID.Electrified, 600);
                    Player.AddBuff(ModContent.BuffType<Madness>(), 360);
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
            //白丝发电
            if (hairdressersWhiteSilkStockings)
            {
                if (damage >= 4)
                {
                    Player.AddBuff(BuffID.Electrified, 900);
                    Player.AddBuff(ModContent.BuffType<Madness>(), 360);
                }
            }
        }
        public override void ModifyHitNPC(Item item, NPC target, ref int damage, ref float knockback, ref bool crit)
        {
            //毁灭刃
            if (item.type == ItemID.BreakerBlade)
            {
                if (item.scale < 3.6f)
                {
                    item.scale += 0.15f;
                    damage = (int)(damage / item.scale * 1.6f);
                }
                if (target.life >= target.lifeMax * 0.9f)
                {
                    target.AddBuff(BuffID.OnFire3, 600);
                    target.AddBuff(BuffID.Bleeding, 600);
                    damage = (int)(damage * 3.6f);
                    if (crit)
                    {
                        target.AddBuff(BuffID.Burning, 600);
                        target.AddBuff(BuffID.Bleeding, 600);
                    }
                }
                if (Main.myPlayer == Player.whoAmI)
                {
                    bool opp = Main.rand.NextBool();
                    int sW = Main.screenWidth;
                    int sH = Main.screenHeight;
                    Vector2 position = target.Center + new Vector2((opp ? sW * 0.5f : -sW * 0.5f) + (opp ? Main.rand.Next(-sW, 0) : Main.rand.Next(0, sW)), (opp ? sH * 0.5f : -sH * 0.5f) + (opp ? Main.rand.Next(-sH, 0) : Main.rand.Next(0, sH)));
                    Projectile breakerBladeFireBall = Projectile.NewProjectileDirect(Player.GetSource_OnHit(item), position, Vector2.Normalize((Player.Center + target.Center) / 2 - position) * 9, ProjectileID.CultistBossFireBall, (int)(damage * 0.8f), item.knockBack * 0.8f, Player.whoAmI);
                    breakerBladeFireBall.friendly = true;
                    breakerBladeFireBall.hostile = false;
                    breakerBladeFireBall.tileCollide = false;
                }
            }
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
            //发电命中敌人对自己施加带电
            if (madnessDebuff)
            {
                Player.AddBuff(BuffID.Electrified, 180);
            }
            //木套效果
            if (woodArmorSet)
            {
                target.AddBuff(BuffID.DryadsWardDebuff, 120);
            }
        }
        public override void ModifyHitNPCWithProj(Projectile proj, NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
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
            //拜月邪教徒火球
            if (proj.type == ProjectileID.CultistBossFireBall)
            {
                target.AddBuff(BuffID.Burning, 45);
                if (target.life >= target.lifeMax * 0.7f)
                {
                    damage = (int)(damage * 2.4f);
                }
            }
            //皇家凝胶施加着火
            if (royalGelOnFire)
            {
                target.AddBuff(BuffID.OnFire, 60);
                //target.AddBuff(BuffID.Electrified, 180);
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
            //发电命中敌人对自己施加带电
            if (madnessDebuff)
            {
                Player.AddBuff(BuffID.Electrified, 60);
            }
            //木套效果
            if (woodArmorSet)
            {
                target.AddBuff(BuffID.DryadsWardDebuff, 60);
            }
        }
    }
}
