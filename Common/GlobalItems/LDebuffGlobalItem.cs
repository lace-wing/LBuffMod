using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LBuffMod.Common.ModPlayers;
using Terraria.Localization;

namespace LBuffMod.Common.GlobalItems
{
    public class LDebuffGlobalItem : GlobalItem
    {
        public override void UpdateAccessory(Item item, Player player, bool hideVisual)
        {
            if (item.type == ItemID.RoyalGel)
            {
                player.GetModPlayer<LDebuffPlayer>().royalGelOnFire = true;
            }
            if (item.type == ItemID.SharkToothNecklace)
            {
                player.GetModPlayer<LDebuffPlayer>().sharkToothNecklaceBleeding = true;
            }
            if (item.type == ItemID.SweetheartNecklace)
            {
                player.GetModPlayer<LDebuffPlayer>().stingerNecklaceBleedingAndPoison = true;
            }
        }
        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            if (item.type == ItemID.RoyalGel)
            {
                tooltips.Add(new(Mod, "RoyalGelFireTooltip", Language.GetTextValue("Mods.LBuffMod.TooltipLines.RoyalGelFireTooltip")));
            }
            if (item.type == ItemID.SharkToothNecklace)
            {
                tooltips.Add(new(Mod, "SharkToothNecklaceBleedingTooltip", Language.GetTextValue("Mods.LBuffMod.TooltipLines.SharkToothNecklaceBleedingTooltip")));
            }
            if (item.type == ItemID.StingerNecklace)
            {
                tooltips.Add(new(Mod, "StingerNecklaceBleedingAndPoisonTooltip", Language.GetTextValue("Mods.LBuffMod.TooltipLines.StingerNecklaceBleedingAndPoisonTooltip")));
            }
        }
    }
}
