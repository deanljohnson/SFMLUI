﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using SFML.Graphics;

namespace SFMLUI
{
    public class FontLoader
    {
        private const String DATA_EXTENSION = ".ttf";
        private String m_DataDir { get; }

        public FontLoader(String path)
        {
            m_DataDir = path;
        }

        public Dictionary<String, Font> LoadAll()
        {
            var dir = new DirectoryInfo(m_DataDir);
            var ttfFiles = dir.GetFiles("*" + DATA_EXTENSION);

            return Load(ttfFiles.Select(file => m_DataDir + file.Name));
        }

        private static Dictionary<String, Font> Load(IEnumerable<String> ttfPaths)
        {
            var fonts = new Dictionary<String, Font>();

            foreach (var path in ttfPaths)
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