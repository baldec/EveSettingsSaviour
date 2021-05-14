using EveSettingsSaviour.Common;
using EveSettingsSaviour.Controls;
using EveSettingsSaviour.Helpers;
using EveSettingsSaviour.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace EveSettingsSaviour
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<SettingsFolder> _settingsFolders;
        Enumerations.Servers _server;

        public MainWindow()
        {
            InitializeComponent();

            _settingsFolders = new List<SettingsFolder>();
            _server = Enumerations.Servers.Tranquility;
        }

        private async void ScanSettingsFolder(Enumerations.Servers server)
        {
            _settingsFolders = await Task.Run(() => SettingsManager.ScanSettings(server).ToList());

            sp_targets.Dispatcher.Invoke(() =>
            {
                sp_targets.Children.Clear();
            });

            source_userFile.Dispatcher.Invoke(() =>
            {
                source_userFile.Items.Clear();
            });

            source_characterFile.Dispatcher.Invoke(() =>
            {
                source_characterFile.Items.Clear();
            });
            

            foreach (SettingsFolder s in _settingsFolders)
            {
                foreach (UserFile u in s.UserFiles)
                {
                    //ComboBoxItem item = new ComboBoxItem();

                    var displayValue = $"{s.FolderName} - {u.Id}";

                    var item = new KeyValuePair<UserFile, string>(u, displayValue);

                    source_userFile.Dispatcher.Invoke(() => 
                    {
                        source_userFile.Items.Add(item);
                    });
                    
                }

                foreach (CharacterFile c in s.CharacterFiles)
                {
                    //ComboBoxItem item = new ComboBoxItem();
                    var displayValue = $"{s.FolderName} - {c.Character.Name} - {c.LastEdited}";

                    var item = new KeyValuePair<CharacterFile, string>(c, displayValue);

                    source_characterFile.Dispatcher.Invoke(() =>
                    {
                        source_characterFile.Items.Add(item);
                    });
                }

                Dispatcher.Invoke(() =>
                {
                    SettingsFolderControl sfc = new SettingsFolderControl(s);

                    sp_targets.Dispatcher.Invoke(() => { sp_targets.Children.Add(sfc); });
                });

                
            }
        }

        private async void btn_ScanSettings_Click(object sender, RoutedEventArgs e)
        {
            btn_ReadSettings.IsEnabled = false;

            await Task.Run(() => ScanSettingsFolder(_server));

            btn_ReadSettings.IsEnabled = true;
        }

        private async void btn_CopySettings_Click(object sender, RoutedEventArgs e)
        {
            // TODO: Logging is something that we need to fix.
            // TODO: Add so that there isn't a race conditon on dropdowns etc if used alot.
            btn_CopySettings.IsEnabled = false;

            //await Task.Run(() =>
            //{

            var targetUserFiles = new List<UserFile>();
            var targetCharacterFiles = new List<CharacterFile>();

            foreach (SettingsFolderControl x in sp_targets.Children)
            {
                foreach (CharacterSettingsControl cControl in x.sp_characterFiles.Children)
                {
                    if (cControl.cb_WillOverwrite.IsChecked == true)
                    {
                        targetCharacterFiles.Add(cControl.CharacterFile);
                    }

                }
                foreach (UserSettingsControl cControl in x.sp_userFiles.Children)
                {
                    if (cControl.cb_WillOverwrite.IsChecked == true)
                    {
                        targetUserFiles.Add(cControl.UserFile);
                    }

                }
            }

            if (cb_copy_userAccounts.IsChecked == true)
            {
                var selectedUserAccount = (UserFile)source_userFile.SelectedValue;

                // TODO: Logging and visual feeback
                if (selectedUserAccount == null) return;

                var fromPath = new FileInfo(selectedUserAccount.FilePath);
                var from_coreYamlPath = new FileInfo($"{fromPath.DirectoryName}/{Constants.CORE_PUBLIC}");

                foreach (var target in targetUserFiles)
                {
                    var result = fromPath.CopyTo(target.FilePath, true);
                    var target_coreYamlPath = $"{result.DirectoryName}/{Constants.CORE_PUBLIC}";
                    from_coreYamlPath.CopyTo(target_coreYamlPath, true);
                }
            }

            if (cb_copy_characters.IsChecked == true)
            {
                var selectedCharacter = (CharacterFile)source_characterFile.SelectedValue;

                // TODO: Logging and visual feeback
                if (selectedCharacter == null) return;

                var fromPath = new FileInfo(selectedCharacter.FilePath);

                foreach (var target in targetCharacterFiles)
                {
                    fromPath.CopyTo(target.FilePath, true);
                }
            }
            //});

            // Reload settings so that the timestamp is accurate
            await Task.Run(() => ScanSettingsFolder(_server));

            btn_CopySettings.IsEnabled = true;
        }

        private void btn_chooseFrom_Click(object sender, RoutedEventArgs e)
        {
            btn_chooseFrom.IsEnabled = false;
            System.Windows.Forms.FolderBrowserDialog openFileDlg = new System.Windows.Forms.FolderBrowserDialog();

            if (txt_fromPath.Text == "" || txt_fromPath.Text == null)
            {
                openFileDlg.SelectedPath = SettingsManager.GetFolderPath(Enumerations.Servers.Tranquility);
            }
            else
            {
                openFileDlg.SelectedPath = txt_fromPath.Text;
            }

            var result = openFileDlg.ShowDialog();
            if (result.ToString() != string.Empty)
            {
                txt_fromPath.Text = openFileDlg.SelectedPath;
            }
            else
            {
                txt_fromPath.Text = "";
            }
            btn_chooseFrom.IsEnabled = true;
        }

        private void btn_chooseTo_Click(object sender, RoutedEventArgs e)
        {
            btn_chooseTo.IsEnabled = false;
            System.Windows.Forms.FolderBrowserDialog openFileDlg = new System.Windows.Forms.FolderBrowserDialog();

            if (txt_toPath.Text == "" || txt_toPath.Text == null)
            {
                //openFileDlg.SelectedPath = SettingsManager.GetFolderPath(Enumerations.Servers.Tranquility);
            }
            else
            {
                openFileDlg.SelectedPath = txt_toPath.Text;
            }

            var result = openFileDlg.ShowDialog();
            if (result.ToString() != string.Empty)
            {
                txt_toPath.Text = openFileDlg.SelectedPath;
            }
            else
            {
                txt_toPath.Text = "";
            }
            btn_chooseTo.IsEnabled = true;
        }

        private void txt_pathValidation(object sender, TextChangedEventArgs e)
        {
            dynamic fromGotPath = txt_fromPath.Text != "" && txt_fromPath.Text != null;
            dynamic toGotPath = txt_toPath.Text != "" && txt_toPath.Text != null;
            dynamic bothGotPath = fromGotPath && toGotPath;

            if (fromGotPath)
            {
                btn_loadBackup.IsEnabled = true;
            }
            else
            {
                btn_loadBackup.IsEnabled = false;
            }

            if (bothGotPath)
            {
                btn_backupSelected.IsEnabled = true;
            }
            else
            {
                btn_backupSelected.IsEnabled = false;
            }

        }

        private async void btn_loadBackup_Click(object sender, RoutedEventArgs e)
        {
            btn_ReadSettings.IsEnabled = false;

            var path = txt_fromPath.Text;

            var settingsFolder = await Task.Run(() => SettingsManager.ScanFolder(path));

            _settingsFolders?.Clear();
            _settingsFolders.Add(settingsFolder);

            //sp_targets.Children.Clear();
            source_userFile.Items.Clear();
            source_characterFile.Items.Clear();

            foreach (SettingsFolder s in _settingsFolders)
            {
                foreach (UserFile u in s.UserFiles)
                {
                    //ComboBoxItem item = new ComboBoxItem();

                    var displayValue = $"{s.FolderName} - {u.Id}";

                    var item = new KeyValuePair<UserFile, string>(u, displayValue);
                    source_userFile.Items.Add(item);
                }

                foreach (CharacterFile c in s.CharacterFiles)
                {
                    //ComboBoxItem item = new ComboBoxItem();
                    var displayValue = $"{s.FolderName} - {c.Character.Name} - {c.LastEdited}";

                    var item = new KeyValuePair<CharacterFile, string>(c, displayValue);

                    source_characterFile.Items.Add(item);
                }

                SettingsFolderControl sfc = new SettingsFolderControl(s);

                sp_targets.Children.Add(sfc);
            }
            btn_ReadSettings.IsEnabled = true;
        }

        private async void btn_backupSelected_Click(object sender, RoutedEventArgs e)
        {
            btn_backupSelected.IsEnabled = false;

            var from = txt_fromPath.Text;
            var to = txt_toPath.Text;

            var fromDirInfo = new DirectoryInfo(from);
            var toDirInfo = new DirectoryInfo(to);

            dynamic fromIsValid = fromDirInfo?.Exists;
            dynamic toIsValid = toDirInfo?.Exists;

            var folderAndDate = $"{fromDirInfo.Name}-{DateTime.Now.ToString("yyyy-MM-dd_HHmmss")}";

            var backupDirectory = Directory.CreateDirectory($"{to}/{folderAndDate}");

            if (fromIsValid && toIsValid)
            {
                await Task.Run(() => CopyDirectory.CopyAll(fromDirInfo, backupDirectory));
            }
            btn_backupSelected.IsEnabled = true;
        }

        private void cb_configuration_allianceOnly_Checked(object sender, RoutedEventArgs e)
        {
            if (source_characterFile.SelectedItem == null)
            {
                cb_configuration_allianceOnly.IsChecked = false;
                return;
            }

            if (cb_configuration_corporationOnly.IsChecked == true)
            {
                cb_configuration_corporationOnly.IsChecked = false;
            }

            var charFileOfSelected = (CharacterFile)source_characterFile.SelectedValue;

            var allianceIdOfSelectedCharacter = charFileOfSelected.Character.AllianceId;
            var filePathOfSelectedCharacter = charFileOfSelected.FilePath;

            foreach (SettingsFolderControl x in sp_targets.Children)
            {
                foreach (CharacterSettingsControl cControl in x.sp_characterFiles.Children)
                {
                    var cAllianceId = cControl.CharacterFile.Character.AllianceId;
                    if (allianceIdOfSelectedCharacter == cAllianceId && filePathOfSelectedCharacter != cControl.CharacterFile.FilePath)
                    {
                        cControl.IsEnabled = true;
                    }
                    else
                    {
                        cControl.IsEnabled = false;
                    }
                }
            }
        }

        private void cb_configuration_allianceOnly_Unchecked(object sender, RoutedEventArgs e)
        {
            var charFileOfSelected = (CharacterFile)source_characterFile.SelectedValue;
            var filePathOfSelectedCharacter = charFileOfSelected.FilePath;

            foreach (SettingsFolderControl x in sp_targets.Children)
            {
                foreach (CharacterSettingsControl cControl in x.sp_characterFiles.Children)
                {
                    if (filePathOfSelectedCharacter != cControl.CharacterFile.FilePath)
                    {
                        cControl.IsEnabled = true;
                    }
                    else
                    {
                        cControl.IsEnabled = false;
                    }

                }
            }
        }

        private void cb_configuration_corporationOnly_Checked(object sender, RoutedEventArgs e)
        {
            // TODO: Make sure that the selected values in the dropdown isn't removed. 

            if (source_characterFile.SelectedItem == null)
            {
                cb_configuration_corporationOnly.IsChecked = false;
                return;
            }

            if (cb_configuration_allianceOnly.IsChecked == true)
            {
                cb_configuration_allianceOnly.IsChecked = false;
            }

            var charFileOfSelected = (CharacterFile)source_characterFile.SelectedValue;

            var corporationIdOfSelectedCharacter = charFileOfSelected.Character.CorporationId;
            var filePathOfSelectedCharacter = charFileOfSelected.FilePath;

            foreach (SettingsFolderControl x in sp_targets.Children)
            {
                foreach (CharacterSettingsControl cControl in x.sp_characterFiles.Children)
                {
                    var cCorporationId = cControl.CharacterFile.Character.CorporationId;
                    if (corporationIdOfSelectedCharacter == cCorporationId && filePathOfSelectedCharacter != cControl.CharacterFile.FilePath)
                    {
                        cControl.IsEnabled = true;
                    }
                    else
                    {
                        cControl.IsEnabled = false;
                    }
                }
            }
        }

        private void cb_configuration_corporationOnly_Unchecked(object sender, RoutedEventArgs e)
        {
            var charFileOfSelected = (CharacterFile)source_characterFile.SelectedValue;
            var filePathOfSelectedCharacter = charFileOfSelected.FilePath;

            foreach (SettingsFolderControl x in sp_targets.Children)
            {
                foreach (CharacterSettingsControl cControl in x.sp_characterFiles.Children)
                {
                    if (filePathOfSelectedCharacter != cControl.CharacterFile.FilePath)
                    {
                        cControl.IsEnabled = true;
                    }
                    else
                    {
                        cControl.IsEnabled = false;
                    }
                }
            }
        }



        private void source_userFile_DropDownClosed(object sender, EventArgs e)
        {
            var selectedFilePath = ((UserFile)source_userFile.SelectedValue)?.FilePath;

            if (selectedFilePath == null) return;

            foreach (SettingsFolderControl x in sp_targets.Children)
            {
                foreach (UserSettingsControl cControl in x.sp_userFiles.Children)
                {
                    var cFilePath = cControl.UserFile.FilePath;
                    if (selectedFilePath == cFilePath)
                    {
                        cControl.IsEnabled = false;
                    }
                    else
                    {
                        cControl.IsEnabled = true;
                    }
                }
            }
        }

        private void source_characterFile_DropDownClosed(object sender, EventArgs e)
        {
            var selectedFilePath = ((CharacterFile)source_characterFile.SelectedValue)?.FilePath;

            if (selectedFilePath == null) return;

            foreach (SettingsFolderControl x in sp_targets.Children)
            {
                foreach (CharacterSettingsControl cControl in x.sp_characterFiles.Children)
                {
                    var cFilePath = cControl.CharacterFile.FilePath;
                    if (selectedFilePath == cFilePath)
                    {
                        cControl.IsEnabled = false;
                    }
                    else
                    {
                        cControl.IsEnabled = true;
                    }
                }
            }
        }


    }
}
