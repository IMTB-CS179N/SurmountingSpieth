using Project.Game;
using Project.Input;
using Project.Items;

using System;
using System.Collections.Generic;

using UnityEngine;

namespace Project
{
    public static class ResourceManager
    {
        public static readonly string DefaultTexturePath = "Sprites/Shared/DEFAULT_TEXTURE";

        public static readonly string DefaultSpritePath = "Sprites/Shared/DEFAULT_SPRITE";

        public static readonly string ArmorDataPath = "Data/Armor.csv";

        public static readonly string WeaponDataPath = "Data/Weapons.csv";

        public static readonly string PotionDataPath = "Data/Potions.csv";

        public static readonly string TrinketDataPath = "Data/Trinkets.csv";

        public static readonly string AbilityDataPath = "Data/Abilities.csv";

        public static readonly string RaceDataPath = "Data/Races.csv";

        public static readonly string ClassDataPath = "Data/Classes.csv";

        public static readonly string CharacterDataPath = "Data/Characters.csv";

        public static readonly string DisallowPath = "UI/Shared/DisallowIcon";

        public static readonly string SelectedItemPath = "UI/Shared/SelectedItemBackground";

        private static readonly Dictionary<string, Texture2D> ms_loadedTextures = new();
        private static readonly Dictionary<string, Sprite> ms_loadedSprites = new();

        private static TrinketData[] ms_trinkets;
        private static PotionData[] ms_potions;
        private static WeaponData[] ms_weapons;
        private static ArmorData[] ms_armors;

        private static AbilityData[] ms_abilities;
        private static ClassInfo[] ms_classes;
        private static RaceInfo[] ms_races;
        private static BaseStats[] ms_stats;

        private static Texture2D ms_defaultTexture;
        private static Sprite ms_defaultSprite;

        public static Texture2D DefaultTexture => ms_defaultTexture == null ? (ms_defaultTexture = Resources.Load<Texture2D>(DefaultTexturePath)) : ms_defaultTexture;

        public static Sprite DefaultSprite => ms_defaultSprite == null ? (ms_defaultSprite = Resources.Load<Sprite>(DefaultSpritePath)) : ms_defaultSprite;

        public static IReadOnlyList<BaseStats> Stats => ms_stats ??= AssetParser.ParseFromCSV<BaseStats>(CharacterDataPath, true);

        public static IReadOnlyList<RaceInfo> Races => ms_races ??= AssetParser.ParseFromCSV<RaceInfo>(RaceDataPath, true);

        public static IReadOnlyList<ClassInfo> Classes => ms_classes ??= AssetParser.ParseFromCSV<ClassInfo>(ClassDataPath, true);

        public static IReadOnlyList<AbilityData> Abilities => ms_abilities ??= AssetParser.ParseFromCSV<AbilityData>(AbilityDataPath, true);

        public static IReadOnlyList<ArmorData> Armors => ms_armors ??= AssetParser.ParseFromCSV<ArmorData>(ArmorDataPath, true).Sorted();

        public static IReadOnlyList<WeaponData> Weapons => ms_weapons ??= AssetParser.ParseFromCSV<WeaponData>(WeaponDataPath, true).Sorted();

        public static IReadOnlyList<PotionData> Potions => ms_potions ??= AssetParser.ParseFromCSV<PotionData>(PotionDataPath, true).Sorted();

        public static IReadOnlyList<TrinketData> Trinkets => ms_trinkets ??= AssetParser.ParseFromCSV<TrinketData>(TrinketDataPath, true).Sorted();

        public static Texture2D LoadTexture2D(string path)
        {
            if (String.IsNullOrEmpty(path))
            {
                throw new ArgumentNullException(nameof(path));
            }

            if (ms_loadedTextures.TryGetValue(path, out var texture))
            {
                if (texture != null)
                {
                    return texture;
                }
            }

            var resource = Resources.Load<Texture2D>(path);

            if (resource != null)
            {
                ms_loadedTextures[path] = resource;
            }
            else
            {
                resource = DefaultTexture;
            }

            return resource;
        }

        public static Sprite LoadSprite(string path)
        {
            if (String.IsNullOrEmpty(path))
            {
                throw new ArgumentNullException(nameof(path));
            }

            if (ms_loadedSprites.TryGetValue(path, out var sprite))
            {
                if (sprite != null)
                {
                    return sprite;
                }
            }

            var resource = Resources.Load<Sprite>(path);

            if (resource != null)
            {
                ms_loadedSprites[path] = resource;
            }
            else
            {
                resource = DefaultSprite;
            }

            return resource;
        }

        public static T Find<T>(this IReadOnlyList<T> items, Func<T, bool> match)
        {
            if (items is null)
            {
                throw new ArgumentNullException(nameof(items));
            }

            int count = items.Count;

            for (int i = 0; i < count; ++i)
            {
                var item = items[i];

                if (match(item))
                {
                    return item;
                }
            }

            return default;
        }

        public static T[] Sorted<T>(this T[] items) where T : IItem
        {
            if (items is not null && items.Length > 0)
            {
                //Array.Sort(items, (x, y) => x.Price.CompareTo(y.Price));
            }

            return items;
        }
    }
}
