using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;

namespace Project
{
    public static class ResourceManager
    {
        private static readonly Dictionary<string, Texture2D> ms_loadedTextures;
        private static readonly Dictionary<string, Sprite> ms_loadedSprites;

        public static Texture2D LoadTexture2D(string path)
        {
            if (ms_loadedTextures.TryGetValue(path, out var texture))
            {
                if (texture != null)
                {
                    return texture;
                }
            }

            var resource = Resources.Load<Texture2D>(path);

            ms_loadedTextures[path] = resource;

            return resource;
        }

        public static Sprite LoadSprite(string path)
        {
            if (ms_loadedSprites.TryGetValue(path, out var sprite))
            {
                if (sprite != null)
                {
                    return sprite;
                }
            }

            var resource = Resources.Load<Sprite>(path);

            ms_loadedSprites[path] = resource;

            return resource;
        }
    }
}
