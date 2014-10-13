using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using System.IO;
using Microsoft.Xna.Framework.Content;

namespace ISurvived
{
    static class ContentLoader
    {

        public static Dictionary<String, Texture2D> LoadContent(this ContentManager contentManager, string contentFolder)
        {
            //Load directory info, abort if none
            DirectoryInfo dir = new DirectoryInfo(contentManager.RootDirectory + "\\" + contentFolder);
            if (!dir.Exists)
                throw new DirectoryNotFoundException();

            //Init the resulting list
            Dictionary<String, Texture2D> result = new Dictionary<String, Texture2D>();

            //Load all files that matches the file filter
            FileInfo[] files = dir.GetFiles("*.*");
            foreach (FileInfo file in files)
            {
                string key = Path.GetFileNameWithoutExtension(file.Name);

                result[key] = contentManager.Load<Texture2D>(contentFolder + "/" + key);
            }

            //Return the result
            return result;
        }

    }
}
