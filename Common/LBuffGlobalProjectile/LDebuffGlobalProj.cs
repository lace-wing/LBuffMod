using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.DataStructures;

namespace LBuffMod.Common.LBuffGlobalProjectile
{
    public class LDebuffGlobalProj : GlobalProjectile
    {
        public override bool InstancePerEntity => true;
        public NPC sourceNPC;
        public Player sourcePlayer; //Use it to check a proj's real owner
        public Item sourceItem;
        public Projectile sourceProjectile;

        public NPC npcStriking;
        public Player playerStriking;
        public Item itemStriking;
        public Projectile projectileStriking;
        public override void OnSpawn(Projectile projectile, IEntitySource source)
        {
            if (source != null)
            {
                if (source is EntitySource_Parent)
                {
                    EntitySource_Parent source_Parent = source as EntitySource_Parent;
                    if (source_Parent.Entity is NPC)
                    {
                        sourceNPC = (NPC)source_Parent.Entity;
                    }
                    if (source_Parent.Entity is Player)
                    {
                        sourcePlayer = (Player)source_Parent.Entity;
                    }
                }
                if (source is EntitySource_ItemUse)
                {
                    EntitySource_ItemUse source_ItemUse = source as EntitySource_ItemUse;
                    if (source_ItemUse.Item is Item)
                    {
                        sourceItem = (Item)source_ItemUse.Item;
                        sourcePlayer = Main.player[sourceItem.whoAmI];
                        //Main.NewText($"{sourcePlayer.name} just used {sourceItem.Name} to swpan {projectile.Name}");
                    }
                }
                if (source is EntitySource_ItemUse_WithAmmo)
                {
                    EntitySource_ItemUse_WithAmmo source_ItemUse_WithAmmo = source as EntitySource_ItemUse_WithAmmo;
                    if (source_ItemUse_WithAmmo.Item is Item)
                    {
                        sourceItem = (Item)source_ItemUse_WithAmmo.Item;
                        sourcePlayer = Main.player[sourceItem.whoAmI];
                        //Main.NewText($"{sourcePlayer.name} just used {sourceItem.Name} to swpan {projectile.Name}");
                    }
                }
                else if (source is EntitySource_OnHit) //TODO Not implement
                {
                    EntitySource_OnHit source_OnHit = source as EntitySource_OnHit;
                    if (source_OnHit.EntityStriking is Player)
                    {
                        playerStriking = (Player)source_OnHit.EntityStriking;
                    }
                    else if (source_OnHit.EntityStriking is Item)
                    {
                        itemStriking = (Item)source_OnHit.EntityStriking;
                        playerStriking = Main.player[sourceItem.whoAmI];
                    }
                    else if (source_OnHit.EntityStriking is Projectile)
                    {
                        projectileStriking = (Projectile)source_OnHit.EntityStriking;
                    }
                }
            }
        }
        public override void ModifyHitPlayer(Projectile projectile, Player target, ref int damage, ref bool crit)
        {
            if (sourceNPC != null)
            {
                #region Pre-hard Mode projs inflicting damaging debuffs
                //哥布林鲨、血鱿鱼、恐惧鹦鹉螺
                if (sourceNPC.type == NPCID.GoblinShark || sourceNPC.type == NPCID.BloodSquid || sourceNPC.type == NPCID.BloodNautilus)
                {
                    target.AddBuff(BuffID.Bleeding, 120);
                }
                //血肉墙、饿鬼、血蛭
                if (sourceNPC.type == NPCID.TheHungry || sourceNPC.type == NPCID.TheHungryII || sourceNPC.type == NPCID.WallofFlesh || sourceNPC.type == NPCID.WallofFleshEye || sourceNPC.type == NPCID.LeechHead || sourceNPC.type == NPCID.LeechBody || sourceNPC.type == NPCID.LeechTail)
                {
                    if (!Main.hardMode)
                    {
                        target.AddBuff(BuffID.OnFire3, 48);
                    }
                    if (Main.hardMode)
                    {
                        target.AddBuff(BuffID.CursedInferno, 48);
                    }
                }
                #endregion
                #region Hard mode projs inflicting damaging debuffs
                //机械骷髅王、激光眼、毁灭者身体&尾
                if (sourceNPC.type == NPCID.SkeletronPrime || sourceNPC.type == NPCID.PrimeCannon || sourceNPC.type == NPCID.PrimeLaser || sourceNPC.type == NPCID.PrimeSaw || sourceNPC.type == NPCID.PrimeVice || sourceNPC.type == NPCID.Retinazer || sourceNPC.type == NPCID.TheDestroyerBody || sourceNPC.type == NPCID.TheDestroyerTail)
                {
                    target.AddBuff(BuffID.OnFire, 48);
                }
                //魔焰眼、毁灭者头
                if (sourceNPC.type == NPCID.Spazmatism || sourceNPC.type == NPCID.TheDestroyer)
                {
                    target.AddBuff(BuffID.OnFire, 96);
                }
                #endregion
            }
            //TODO More projs to inflict debuffs!!!
        }
        public override void PostAI(Projectile projectile)
        {
            //挥发明胶射弹修改
            if (projectile.type == ProjectileID.VolatileGelatinBall && projectile.friendly && (sourceItem != null ? sourceItem.type == ItemID.VolatileGelatin : false))
            {
                if (projectile.scale > 1.5f)
                {
                    projectile.scale -= 0.003f;
                }
                projectile.position.X += projectile.velocity.X * 0.4f;
                projectile.position.Y -= projectile.velocity.Y * 0.1f;
                NPC targetNPC = projectile.FindTargetWithinRange(480);
                if (targetNPC != null)
                {
                    int hX = (int)(targetNPC.Center.X - projectile.Center.X);
                    int hY = (int)(targetNPC.Center.Y - projectile.Center.Y);
                    if (hX < 120 && hX > -120)
                    {
                        hX = hX > 0 ? 120 : -120;
                    }
                    if (hY < 120 && hY > -120)
                    {
                        hY = hY > 0 ? 120 : -120;
                    }
                    Vector2 homingVelocity = new Vector2(hX, hY) * MathHelper.Lerp(0.0003f, 0.0036f, new Vector2(hX, hY).Length() / 480f);
                    projectile.velocity += homingVelocity;
                    if (targetNPC.Center.Y < projectile.Center.Y)
                    {
                        projectile.velocity += homingVelocity * 0.0012f;
                    }
                }
            }
        }
    }
}
