﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using EveSettingsSaviour.Models;
using EveSettingsSaviour.Enumerations;

namespace EveSettingsSaviour.Helpers
{
    public static class SettingsManager
    {
        public static IEnumerable<SettingsFolder> ScanSettings(Servers server)
        {

            var isWindows = true;
            var selectedServer = GetFolderName(server);
            var localAppData = Environment.GetEnvironmentVariable("LocalAppData");
            var baseCacheDirectory = isWindows ? $"{localAppData}\\CCP\\EVE\\{selectedServer}" : null;

            Console.WriteLine($"Base cache directory: {baseCacheDirectory}");

            Regex regex_userfile_win = new Regex(@"^.*\\core_user_(?<id>\d+)\.dat$", RegexOptions.Compiled | RegexOptions.IgnoreCase);
            Regex regex_charfile_win = new Regex(@"^.*\\core_char_(?<id>\d+).dat$", RegexOptions.Compiled | RegexOptions.IgnoreCase);

            List<SettingsFolder> folders = new List<SettingsFolder>();

            if (baseCacheDirectory == null)
            {
                Console.WriteLine("Not windows");
                Console.ReadLine();
                return null;
            }

            foreach (var directoryName in Directory.GetDirectories(baseCacheDirectory, "settings_*"))
            {
                //Console.WriteLine(directoryName);
                var folderName = new DirectoryInfo(directoryName).Name;

                var charFiles = Directory.GetFiles(directoryName, "core_char_*.dat").Where(c => regex_charfile_win.IsMatch(c));
                foreach (var file in charFiles)
                {
                    //Console.WriteLine(file);
                    var m = isWindows ? regex_charfile_win.Match(file) : null;
                    if (m.Success)
                    {
                        var folder = folders.FirstOrDefault(x => x.FilePath == directoryName);
                        if (folder == null)
                        {
                            folder = new SettingsFolder(directoryName);
                            folders.Add(folder);
                        }

                        FileInfo fileInfo = new FileInfo(file);
                        var id = int.Parse(m?.Groups["id"].Value);
                        var character = ESI.GetCharacter(id).Result;
                        //Console.WriteLine($"Id: {character.Id} Name: {character.Name} LastEdit: {fileInfo.LastWriteTime}");

                        folder.CharacterFiles.Add(new CharacterFile
                        {
                            Character = character,
                            Id = id,
                            FilePath = file,
                            LastEdited = fileInfo.LastWriteTime
                        });
                    }
                }

                var userFiles = Directory.GetFiles(directoryName, "core_user_*.dat").Where(c => regex_userfile_win.IsMatch(c));
                foreach (var file in userFiles)
                {
                    var m = isWindows ? regex_userfile_win.Match(file) : null;
                    if (m.Success)
                    {
                        var folder = folders.FirstOrDefault(x => x.FilePath == directoryName);
                        if (folder == null)
                        {
                            folder = new SettingsFolder(directoryName);
                            folders.Add(folder);
                        }

                        FileInfo fileInfo = new FileInfo(file);
                        var id = int.Parse(m?.Groups["id"].Value);

                        folder.UserFiles.Add(new UserFile { 
                            Id = id, 
                            LastEdited = fileInfo.LastWriteTime,
                            FilePath = file
                        });
                    }
                }
            }

            return folders;
        }

        public static async Task WriteCharacterSettings(CharacterFile source, IEnumerable<CharacterFile> targets)
        {
            var content = await File.ReadAllBytesAsync(source.FilePath);
            List<Task> tasks = new();
            foreach (var target in targets)
            {

                tasks.Add(File.WriteAllBytesAsync(target.FilePath, content));
            }

            await Task.WhenAll(tasks.ToArray());
            return;
        }
        public static async Task WriteUserSettings(UserFile source, IEnumerable<UserFile> targets)
        {
            var content = await File.ReadAllBytesAsync(source.FilePath);
            List<Task> tasks = new();
            foreach (var target in targets)
            {
                tasks.Add(File.WriteAllBytesAsync(target.FilePath, content));                
            }

            await Task.WhenAll(tasks.ToArray());
            return;
        }

        private static string GetFolderName(Enumerations.Servers server)
        {
            switch (server)
            {
                case Servers.Tranquility:
                    return "c_eve_sharedcache_tq_tranquility";
                case Servers.Serenity:
                    return "c_eve_sharedcache_sisi_singularity";
                default:
                    return null;
            }
        }
    }
}