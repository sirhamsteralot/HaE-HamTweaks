using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using HaEHamTweaks;
using HaEPluginCore;
using HaEPluginCore.Console;
using VRage;
using VRage.Filesystem;
using VRage.FileSystem;

namespace HaEHamTweaks.Managers
{
    public class TexturePackManager
    {
        public static TexturePackManager Instance;

        public TexturePackManager()
        {
            Instance = this;

            if (!Directory.Exists(HaEConstants.pluginFolder + "\\" + HamTweakConstants.TexturePackFolder))
                Directory.CreateDirectory(HaEConstants.pluginFolder + "\\" + HamTweakConstants.TexturePackFolder);

            TexturePackCommands.RegisterCommands();
        }

        public void LoadTexturesFrom(string texturePackName)
        {
            string source = HaEConstants.pluginFolder + "\\" + HamTweakConstants.TexturePackFolder + "\\" + texturePackName;

            List<string> directories = new List<string>();
            List<string> nextDirs = new List<string>();

            directories.Add(source);
            while (directories.Count != 0)
            {
                nextDirs.Clear();

                foreach (var directory in directories)
                {
                    HaEConsole.WriteLine($"In: {directory}");

                    foreach (var file in Directory.EnumerateFiles(directory))
                    {
                        var substr = file.Substring(source.Length + 1);
                        SwapCustomTexture(substr, texturePackName);
                    }
                    nextDirs.AddRange(Directory.GetDirectories(directory));
                }

                directories.Clear();
                directories.AddRange(nextDirs);
            }

        }

        public void SwapCustomTexture(string texture, string texturePackName)
        {
            string toReplacePath = MyFileSystem.ContentPath + "\\Textures\\" + texture;
            string backupVanillaPath = HaEConstants.pluginFolder + "\\" + HamTweakConstants.VanillaTextureFolder + "\\" + texture;
            string fullTexturePath = HaEConstants.pluginFolder + "\\" + HamTweakConstants.TexturePackFolder + "\\" + texturePackName + "\\" + texture;

            if (!File.Exists(backupVanillaPath)) {
                if (!Directory.Exists(Path.GetDirectoryName(backupVanillaPath)))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(backupVanillaPath));
                    HaEConsole.WriteLine($"Backup directory: {Path.GetDirectoryName(backupVanillaPath)} Created!");
                }

                HaEConsole.WriteLine($"Vanilla Backup: {backupVanillaPath} Created!");
                File.Copy(toReplacePath, backupVanillaPath);
            }

            if (File.Exists(fullTexturePath))
            {
                HaEConsole.WriteLine($"Swapping in Texture: {fullTexturePath}");
                File.Delete(toReplacePath);
                File.Copy(fullTexturePath, toReplacePath, true);
            }
        }
    }
}
