using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.Localization;

namespace LBuffMod.Common.Utilities
{
    public static class LLocalizationUtils
    {
        public static string GetTranslationAsString(string key)
        {
            if (key.Contains(" "))
            {
                throw new Exception("ModTranslation keys can't contain spaces.");
            }
            string nullValueException = "Sadly the value is null";
            string gameCulture = Language.ActiveCulture.Name;
            string value = LocalizationLoader.GetOrCreateTranslation(key).GetTranslation(gameCulture);
            return value == null ? nullValueException : value;
        }
    }
}
