using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LBuffMod.Content.Buffs;
using LBuffMod.Common.LBuffGlobalProjectile;
using static LBuffMod.Common.Utilities.LBuffUtils;
using static LBuffMod.Common.Utilities.LCollisionUtils;

namespace LBuffMod.Common.ModPlayers
{
    public class LDebuffPlayer : ModPlayer
    {
        public bool royalGelOnFire;
        public int royalGelFireDamage = -16;
        public float royalGelFireDamageMultiplier = 0.7f;

        public bool volatileGelatinFire;
        public int volatileGelatinFireDamage = -32;
        public float volatileGelatinFireDamageMultiplier = 2.1f;

        public bool fireSlowDown;

        public bool sharkToothNecklaceBleeding;
        public bool stingerNecklaceBleedingAndPoison;
        public bool madnessDebuff;
        public bool hairdressersWhiteSilkStockings;

        public bool woodArmorSet;
        public bool corruptionSetBonus;
        public bool crimsonSetBonus;

        public bool dryadsWardOnHit;

        int spikeNum = 0;
        public int plrOnSpikesTimer = 0;
        public int initialBleedingTime = 0;
        int additionalBleedingTime = 0;
        public override void ResetEffects()
        {
            royalGelOnFire = false;
            volatileGelatinFire = false;
            sharkToothNecklaceBleeding = false;
            stingerNecklaceBleedingAndPoison = false;
            madnessDebuff = false;
            hairdressersWhiteSilkStockings = false;
            woodArmorSet = false;
            corruptionSetBonus = false;
            crimsonSetBonus = false;
            dryadsWardOnHit = false;
            fireSlowDown = false;
            spikeNum = 0;
        }
        public override void PostUpdate()
        {
            #region 尖刺流血
            spikeNum = ContactTileNum(Player.position, Player.width, Player.height, new int[] { TileID.Spikes, TileID.WoodenSpikes });
            int bleedingI = Player.FindBuffIndex(BuffID.Bleeding);
            additionalBleedingTime = Math.Clamp(additionalBleedingTime, -600, 1800);
            if (spikeNum > 0)
            {
                plrOnSpikesTimer += spikeNum;
                additionalBleedingTime += (int)(Player.velocity.Length() * spikeNum);
                if (bleedingI != -1)
                {
                    Player.buffTime[Player.FindBuffIndex(BuffID.Bleeding)] = 60 + initialBleedingTime + additionalBleedingTime;
                }
            }
            if (spikeNum == 0)
            {
                additionalBleedingTime -= 1;
                if (plrOnSpikesTimer > 0)
                {
                    Player.AddBuff(BuffID.Bleeding, (int)(plrOnSpikesTimer * 0.0f));
                    plrOnSpikesTimer = 0;
                }
                if (bleedingI == -1)
                {
                    initialBleedingTime = 0;
                }
                if (bleedingI != -1)
                {
                    initialBleedingTime = Player.buffTime[Player.FindBuffIndex(BuffID.Bleeding)];
                }
            }
            if (Player.dead)
            {
                additionalBleedingTime = 0;
            }
            #endregion
        }
        public override void UpdateEquips()
        {
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
            //TODO Balance needed
            #region 木套一般效果
            if (Player.armor[0].type == ItemID.WoodHelmet && Player.armor[1].type == ItemID.WoodBreastplate && Player.armor[2].type == ItemID.WoodGreaves)
            {
                woodArmorSet = true;
                dryadsWardOnHit = true;
                if (Main.rand.NextBool(180))
                {
                    Player.AddBuff(BuffID.DryadsWard, 90);
                }
                Player.moveSpeed += 0.25f;
                Player.lifeMagnet = true;
                Player.treasureMagnet = true;
                if (Player.ZoneForest)
                {
                    Player.moveSpeed += 0.15f;
                }
            }
            //红木套效果
            if (Player.armor[0].type == ItemID.RichMahoganyHelmet && Player.armor[1].type == ItemID.RichMahoganyBreastplate && Player.armor[2].type == ItemID.RichMahoganyGreaves)
            {
                woodArmorSet = true;
                dryadsWardOnHit = true;
                if (Main.rand.NextBool(120))
                {
                    Player.AddBuff(BuffID.DryadsWard, 90);
                }
                Player.buffImmune[BuffID.Poisoned] = true;
                Player.statLifeMax2 += 40;
                if (Player.ZoneJungle)
                {
                    Player.GetAttackSpeed(DamageClass.Generic) += 0.10f;
                    Player.jumpBoost = true;
                    Player.jumpSpeedBoost += 0.2f;
                }
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
                if (Player.ZoneSnow)
                {
                    Player.moveSpeed += 0.25f;
                }
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
                Player.fishingSkill += 33;
                Player.canFloatInWater = true;
                if (Player.ZoneBeach || Player.ZoneDesert)
                {
                    Player.hasJumpOption_Sail = true;
                }
            }
            //乌木套效果
            if (Player.armor[0].type == ItemID.EbonwoodHelmet && Player.armor[1].type == ItemID.EbonwoodBreastplate && Player.armor[2].type == ItemID.EbonwoodGreaves)
            {
                woodArmorSet = true;
                corruptionSetBonus = true;
                Player.buffImmune[BuffID.OnFire] = true;
                Player.buffImmune[BuffID.CursedInferno] = true;
                Player.wingTimeMax += 30;
                if (Player.equippedWings == null)
                {
                    Player.wingsLogic = 1;
                }
                if (Player.ZoneCorrupt)
                {
                    Player.statDefense += 2;
                }
            }
            //暗影木套效果
            if (Player.armor[0].type == ItemID.ShadewoodHelmet && Player.armor[1].type == ItemID.ShadewoodBreastplate && Player.armor[2].type == ItemID.ShadewoodGreaves)
            {
                woodArmorSet = true;
                crimsonSetBonus = true;
                Player.buffImmune[BuffID.OnFire] = true;
                Player.GetDamage(DamageClass.Generic) += 0.1f;
                if (Player.lifeRegen >= 2)
                {
                    Player.lifeRegen -= 2;
                }
                if (Player.lifeRegen < 2)
                {
                    Player.lifeRegen -= 1;
                }
                if (Player.HasBuff(BuffID.Bleeding))
                {
                    Player.GetAttackSpeed(DamageClass.Melee) += 0.15f;
                    Player.GetAttackSpeed(DamageClass.SummonMeleeSpeed) += 0.15f;
                }
                if (Player.ZoneCrimson)
                {
                    Player.GetDamage(DamageClass.Generic) += 0.15f;
                    if (!Player.HasBuff(BuffID.Bleeding) || (Player.HasBuff(BuffID.Bleeding) && Player.buffTime[Player.FindBuffIndex(BuffID.Bleeding)] < 45))
                    {
                        Player.AddBuff(BuffID.Bleeding, 15);
                    }
                }
            }
            //珍珠木套效果
            if (Player.armor[0].type == ItemID.PearlwoodHelmet && Player.armor[1].type == ItemID.PearlwoodBreastplate && Player.armor[2].type == ItemID.PearlwoodGreaves)
            {
                woodArmorSet = true;
                Player.buffImmune[BuffID.Frostburn] = true;
                Player.AddBuff(BuffID.DryadsWard, 90);
                Player.statDefense += 8;
                Player.statManaMax2 += 60;
                Player.GetDamage(DamageClass.Generic) += 0.25f;
                Player.GetAttackSpeed(DamageClass.Generic) += 0.15f;
                Player.maxMinions += 2;
                Player.moveSpeed += 0.25f;
                Player.wingTimeMax += 60;
                if (Player.equippedWings == null)
                {
                    Player.wingsLogic = 1;
                }
                if (Player.ZoneHallow)
                {
                    Player.endurance += 0.24f;
                }
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
                Player.moveSpeed *= 1.1f;
            }
            #endregion，
        }
        public override void PostUpdateEquips()
        {
            base.PostUpdateEquips();
        }
        public override void UpdateBadLifeRegen()
        {
            //全局：根据持续时间增加伤害：所有伤害性原版debuff + 流血
            if (Player.lifeRegen < 0)
            {
                for (int i = 0; i < lDamagingDebuffs.Length; i++)
                {
                    int buffIndex = Player.FindBuffIndex(lDamagingDebuffs[i]);
                    if (buffIndex != -1)//TODO Balanced formula needed
                    {
                        int additionalDamage = (int)(BuffIDToLifeRegen(lDamagingDebuffs[i]) * MathHelper.Lerp(-0.3f, 2.1f, Player.buffTime[buffIndex] / 6300f));
                        Player.lifeRegen += additionalDamage;
                        if (lDamagingDebuffs[i] == BuffID.Electrified && madnessDebuff)
                        {
                            Player.lifeRegen -= additionalDamage;
                        }
                        if (lDamagingDebuffs[i] == BuffID.Burning)//灼烧额外伤害-80%
                        {
                            Player.lifeRegen -= (int)(additionalDamage * 0.8f);
                        }
                        //Main.NewText("Player: buffTime: " + Player.buffTime[buffIndex] + " " + "Additional damage: " + additionalDamage);
                    }
                }
            }
            //流血真的流血了
            if (Player.HasBuff(BuffID.Bleeding))
            {
                Player.lifeRegen += BuffIDToLifeRegen(BuffID.Bleeding);
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
                        Player.lifeRegen += (int)(f / 2f);
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
            for (int i = 0; i < damagingDebuffsToBuff.Length; i++)
            {
                int buffIndex = Player.FindBuffIndex(damagingDebuffsToBuff[i]);
                if (buffIndex != -1)
                {
                    //弱debuff通用增伤
                    damage += (int)(-BuffIDToLifeRegen(damagingDebuffsToBuff[i]) * MathHelper.Lerp(0.3f, 3f, Player.buffTime[buffIndex] / 21600f));
                }
            }
            for (int i = 0; i < thermalDebuffs.Length; i++)
            {
                int buffIndex = Player.FindBuffIndex(damagingDebuffsToBuff[i]);
                if (buffIndex != -1)
                {
                    //火系debuff产生额外被暴击率
                    if (!crit)
                    {
                        int c = (int)(-BuffIDToLifeRegen(thermalDebuffs[i]) / 4f);
                        crit = Main.rand.Next(1, 100) < c ? true : false;
                    }
                }
            }
            //流血增伤
            if (Player.HasBuff(BuffID.Bleeding))
            {
                int buffTime = Player.FindBuffIndex(BuffID.Bleeding);
                damage += (int)(damage * MathHelper.Lerp(0.1f, 0.5f, buffTime / 6300f));
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
            //腐化套装效果
            if (corruptionSetBonus)
            {
                if (Main.myPlayer == Player.whoAmI)
                {
                    for (int i = 1; i < 3; i++)
                    {
                        Projectile.NewProjectileDirect(Player.GetSource_OnHurt(npc), Player.Center, (npc.Center - Player.Center) * i * 0.2f, ProjectileID.BallofFire, (int)(damage * 0.6f), 4f, Player.whoAmI);
                    }
                }
                npc.AddBuff(BuffID.CursedInferno, 30);
            }
            //受击时获得树妖庇护
            if (dryadsWardOnHit)
            {
                Player.AddBuff(BuffID.DryadsWard, 180);
            }
            //皇家凝胶受击惩罚
            if (royalGelOnFire)
            {
                if (damage < 1 || crit)
                {
                    Player.AddBuff(BuffID.OnFire, 90);
                }
                if (damage >= 1 && !crit)
                {
                    Player.AddBuff(BuffID.OnFire, Main.rand.Next(0, 60));
                }
            }
            //挥发明胶受击惩罚
            if (volatileGelatinFire)
            {
                if (damage < 1 || crit)
                {
                    Player.AddBuff(BuffID.Frostburn, 120);
                }
                if (damage >= 1 && !crit)
                {
                    Player.AddBuff(BuffID.Frostburn, Main.rand.Next(0, 90));
                }
            }
        }
        public override void ModifyHitByProjectile(Projectile proj, ref int damage, ref bool crit)
        {
            for (int i = 0; i < damagingDebuffsToBuff.Length; i++)
            {
                int buffIndex = Player.FindBuffIndex(damagingDebuffsToBuff[i]);
                if (buffIndex != -1)
                {
                    //弱debuff通用增伤
                    damage += (int)(-BuffIDToLifeRegen(damagingDebuffsToBuff[i]) * MathHelper.Lerp(0.2f, 2f, Player.buffTime[buffIndex] / 21600));
                }
            }
            for (int i = 0; i < thermalDebuffs.Length; i++)
            {
                int buffIndex = Player.FindBuffIndex(damagingDebuffsToBuff[i]);
                if (buffIndex != -1)
                {
                    //火系debuff产生额外被暴击率
                    if (!crit)
                    {
                        int c = -BuffIDToLifeRegen(thermalDebuffs[i]) / 4;
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
            //腐化套装效果
            if (corruptionSetBonus)
            {
                for (int i = 1; i < 3; i++)
                {
                    if (Main.myPlayer == Player.whoAmI)
                    {
                        Projectile.NewProjectileDirect(Player.GetSource_OnHurt(proj), Player.Center, (proj.Center - Player.Center) * i * 0.2f, ProjectileID.BallofFire, (int)(damage * 0.4f), 4f, Player.whoAmI);
                    }
                }
            }
            //受击时获得树妖庇护
            if (dryadsWardOnHit)
            {
                Player.AddBuff(BuffID.DryadsWard, 180);
            }
            //皇家凝胶受击惩罚
            if (royalGelOnFire)
            {
                if (damage < 1 || crit)
                {
                    Player.AddBuff(BuffID.OnFire, 90);
                }
                if (damage >= 1 && !crit)
                {
                    Player.AddBuff(BuffID.OnFire, Main.rand.Next(0, 60));
                }
            }
            //挥发明胶受击惩罚
            if (volatileGelatinFire)
            {
                if (damage < 1 || crit)
                {
                    Player.AddBuff(BuffID.Frostburn, 120);
                }
                if (damage >= 1 && !crit)
                {
                    Player.AddBuff(BuffID.Frostburn, Main.rand.Next(0, 90));
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
                    if (Main.myPlayer == Player.whoAmI)
                    {
                        Vector2 position = target.Center + new Vector2((opp ? sW * 0.5f : -sW * 0.5f) + (opp ? Main.rand.Next(-sW, 0) : Main.rand.Next(0, sW)), (opp ? sH * 0.5f : -sH * 0.5f) + (opp ? Main.rand.Next(-sH, 0) : Main.rand.Next(0, sH)));
                        Projectile breakerBladeFireBall = Projectile.NewProjectileDirect(Player.GetSource_OnHit(item), position, Vector2.Normalize((Player.Center + target.Center) / 2 - position) * 9, ProjectileID.CultistBossFireBall, (int)(damage * 0.8f), item.knockBack * 0.8f, Player.whoAmI);
                        breakerBladeFireBall.friendly = true;
                        breakerBladeFireBall.hostile = false;
                        breakerBladeFireBall.tileCollide = false;
                    }
                }
            }
            //皇家凝胶施加着火
            if (royalGelOnFire && !target.friendly)
            {
                target.AddBuff(BuffID.OnFire, 300);
            }
            //挥发明胶施加霜火
            if (volatileGelatinFire && !target.friendly)
            {
                //target.AddBuff(BuffID.Oiled, 180);
                target.AddBuff(BuffID.Frostburn, 300);
            }
            //鲨牙项链施加流血
            if (sharkToothNecklaceBleeding && !target.friendly)
            {
                target.AddBuff(BuffID.Bleeding, 300);
            }
            //甜心项链施加流血和中毒
            if (stingerNecklaceBleedingAndPoison && !target.friendly)
            {
                target.AddBuff(BuffID.Bleeding, 300);
                target.AddBuff(BuffID.Poisoned,240);
            }
            //发电命中敌人对自己施加带电
            if (madnessDebuff && !target.friendly)
            {
                Player.AddBuff(BuffID.Electrified, 180);
            }
            //木套效果
            if (woodArmorSet && !target.friendly)
            {
                target.AddBuff(BuffID.DryadsWardDebuff, 240);
            }
            //血腥套效果
            if (crimsonSetBonus && !target.friendly)
            {
                target.AddBuff(BuffID.Bleeding, 45);
                Player.AddBuff(BuffID.Bleeding, 10);
                if (target.HasBuff(BuffID.Bleeding) && Player.HasBuff(BuffID.Bleeding))
                {
                    int lS = 1;
                    if (target.buffTime[target.FindBuffIndex(BuffID.Bleeding)] >= 900)
                    {
                        lS += 1;
                    }
                    if (Player.buffTime[Player.FindBuffIndex(BuffID.Bleeding)] >= 1200)
                    {
                        lS += 1;
                    }
                    Player.statLife += lS;
                    Player.HealEffect(lS);
                }
            }
        }
        public override void ModifyHitNPCWithProj(Projectile proj, NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            #region 武器
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
            #endregion
            //拜月邪教徒火球
            if (proj.type == ProjectileID.CultistBossFireBall && !target.friendly)
            {
                target.AddBuff(BuffID.Burning, 30);
                if (target.life >= target.lifeMax * 0.7f)
                {
                    damage = (int)(damage * 2.4f);
                }
            }
            #region 饰品
            //挥发明胶射弹
            if (proj.type == ProjectileID.VolatileGelatinBall && !target.friendly)
            {
                damage = (int)(damage * 0.3f);
                damage += Math.Min(damage * 3, (int)(proj.damage * proj.scale * 0.5f));
                proj.velocity *= 0.66f;
                target.AddBuff(BuffID.Frostburn, (int)(60 * proj.scale));
                if (proj.scale < 6)
                {
                    proj.scale += 0.5f;
                }
                if (Main.rand.Next(7) >= 2)
                {
                    proj.penetrate += 2;
                }
                if (Main.rand.NextBool(12 - (int)proj.scale) && proj.scale >= 3f && Main.myPlayer == Player.whoAmI && Main.myPlayer == proj.owner)
                {
                    NPC targetNPC = proj.FindTargetWithinRange(320);
                    for (int i = 0; i < 2; i++)
                    {
                        Vector2 velocity = (targetNPC.Center - proj.Center) * 0.12f * (i + 1);
                        Projectile extraVGProj = Projectile.NewProjectileDirect(proj.GetSource_FromThis(), proj.Center, velocity, ProjectileID.VolatileGelatinBall, (int)(damage * 0.75f), knockback, Player.whoAmI);
                        extraVGProj.penetrate = (int)(proj.penetrate * 0.5f);
                        extraVGProj.scale = proj.scale * 0.5f;
                    }
                    proj.Kill();
                }
            }
            //皇家凝胶施加着火
            if (royalGelOnFire && !target.friendly)
            {
                target.AddBuff(BuffID.OnFire, 90);
                if (proj.DamageType == DamageClass.Melee || proj.DamageType == DamageClass.SummonMeleeSpeed)
                {
                    target.AddBuff(BuffID.OnFire, 90);
                }
            }
            //挥发明胶施加霜火
            if (volatileGelatinFire && !target.friendly && proj.owner == Main.myPlayer)
            {
                target.AddBuff(BuffID.Frostburn, 90);
                if (proj.DamageType == DamageClass.Melee || proj.DamageType == DamageClass.SummonMeleeSpeed)
                {
                    target.AddBuff(BuffID.Frostburn, 180);
                }
            }
            //鲨牙项链施加流血
            if (sharkToothNecklaceBleeding && !target.friendly)
            {
                target.AddBuff(BuffID.Bleeding, 90);
                if (proj.DamageType == DamageClass.Melee || proj.DamageType == DamageClass.SummonMeleeSpeed)
                {
                    target.AddBuff(BuffID.Bleeding, 90);
                }
            }
            //毒刺项链施加流血和中毒
            if (stingerNecklaceBleedingAndPoison && !target.friendly)
            {
                target.AddBuff(BuffID.Bleeding, 90);
                target.AddBuff(BuffID.Poisoned, 90);
                if (proj.DamageType == DamageClass.Melee || proj.DamageType == DamageClass.SummonMeleeSpeed)
                {
                    target.AddBuff(BuffID.Bleeding, 90);
                    target.AddBuff(BuffID.Poisoned, 60);
                }
            }
            #endregion
            //发电命中敌人对自己施加带电
            if (madnessDebuff && !target.friendly)
            {
                Player.AddBuff(BuffID.Electrified, 60);
            }
            //木套效果
            if (woodArmorSet && !target.friendly)
            {
                target.AddBuff(BuffID.DryadsWardDebuff, 90);
            }
            //血腥套效果
            if (crimsonSetBonus && !target.friendly)
            {
                if (proj.DamageType == DamageClass.Melee || proj.DamageType == DamageClass.MeleeNoSpeed || proj.DamageType == DamageClass.SummonMeleeSpeed)
                {
                    target.AddBuff(BuffID.Bleeding, 45);
                    Player.AddBuff(BuffID.Bleeding, 10);
                    if (target.HasBuff(BuffID.Bleeding) && Player.HasBuff(BuffID.Bleeding))
                    {
                        int lS = 1;
                        if (target.buffTime[target.FindBuffIndex(BuffID.Bleeding)] >= 900)
                        {
                            lS += 1;
                        }
                        if (Player.buffTime[Player.FindBuffIndex(BuffID.Bleeding)] >= 1200)
                        {
                            lS += 1;
                        }
                        Player.statLife += lS;
                        Player.HealEffect(lS);
                    }
                }
            }
        }
    }
}
