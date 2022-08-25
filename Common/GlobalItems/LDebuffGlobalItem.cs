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
        public override void UpdateInventory(Item item, Player player)
        {
            //毁灭刃
            if (item.type == ItemID.BreakerBlade)
            {
                if (item.scale < 1.6f)
                {
                    item.scale = 1.6f;
                } 
                if (item.scale > 1.6f)
                {
                    item.scale -= 0.002f;
                }
            }
        }
        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            //Accessories
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
            //Weapons
            if (item.type == ItemID.BreakerBlade)
            {
                tooltips.Add(new(Mod, "BreakerBladeBuffsTooltip", Language.GetTextValue("Mods.LBuffMod.TooltipLines.BreakerBladeBuffsTooltip")));
            }
            if (item.type == ItemID.VampireFrogStaff || item.type == ItemID.BloodRainBow || item.type == ItemID.SanguineStaff || item.type == ItemID.DripplerFlail)
            {
                tooltips.Add(new(Mod, "BloodWeaponsTooltip", Language.GetTextValue("Mods.LBuffMod.TooltipLines.BloodWeaponsTooltip")));
            }
        }
    }
}
