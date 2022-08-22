using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using LBuffMod.Common.ModPlayers;

namespace LBuffMod.Content.Items.Accessories
{
    public class HairDressersWhiteSilkStockings : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;
            Item.rare = ItemRarityID.LightPurple;
            Item.value = 1010100;
            Item.accessory = true;
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.statDefense -= 4;
            player.GetModPlayer<LDebuffPlayer>().hairdressersWhiteSilkStockings = true;
        }
    }
}
