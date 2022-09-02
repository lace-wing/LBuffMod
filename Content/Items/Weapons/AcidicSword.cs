using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using LBuffMod.Content.Buffs;

namespace LBuffMod.Content.Items.Weapons
{
    public class AcidicSword : ModItem
    {
        public override void SetStaticDefaults()
        {
            SacrificeTotal = 1;
        }

        public override void SetDefaults()
        {
            Item.damage = 10;
            Item.DamageType = DamageClass.Melee;
            Item.crit = -100;
            Item.width = 40;
            Item.height = 40;
            Item.useTime = 20;
            Item.useAnimation = 20;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.knockBack = 6;
            Item.value = 10000;
            Item.rare = ItemRarityID.Green;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
        }
        public override void OnHitNPC(Player player, NPC target, int damage, float knockBack, bool crit)
        {
            //target.AddBuff(BuffID.Electrified, 1800);
            target.AddBuff(BuffID.Burning, 1800);
            /*for (int i = 0; i < LBuffUtils.thermalDebuffs.Length; i++)
            {
                target.AddBuff(LBuffUtils.thermalDebuffs[i], 1200);
            }*/
        }
        public override bool AltFunctionUse(Player player)
        {
            return true;
        }
        public override bool? UseItem(Player player)
        {
            /*player.AddBuff(BuffID.Electrified, 600);
            player.AddBuff(ModContent.BuffType<Madness>(), 60000);
            Main.NewText(player.lifeRegen);*/
            return base.UseItem(player);
        }
    }
}