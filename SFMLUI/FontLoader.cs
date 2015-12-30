using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using SFML.Graphics;

namespace SFMLUI
{
    /// <summary>
    /// A class that provides various helper methods to load Font objects.
    /// </summary>
    public static class FontLoader
    {
        private const String DATA_EXTENSION = ".ttf";

        /// <summary>
        /// Loads all fonts at the given directory
        /// </summary>
        public static Dictionary<String, Font> LoadAll(String path)
        {
            var dir = new DirectoryInfo(path);
            var ttfFiles = dir.GetFiles("*" + DATA_EXTENSION);

            return Load(ttfFiles.Select(file => path + file.Name));
        }

        /// <summary>
        /// Loads all fonts pointed to by the given Strings
        /// </summary>
        public static Dictionary<String, Font> Load(IEnumerable<String> paths)
        {
            var fonts = new Dictionary<String, Font>();

            foreach (var path in paths)
            {
                Debug.Assert(path.EndsWith(DATA_EXTENSION));

                //Get the fileName, without the extension
                var fontName = path.Split('/').Last().Replace(DATA_EXTENSION, "");

                fonts.Add(fontName, new Font(path));
            }

            return fonts;
        }
    }
}