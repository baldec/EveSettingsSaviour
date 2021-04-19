using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using EveSettingsSaviour.Controls;
using EveSettingsSaviour.Helpers;
using EveSettingsSaviour.Models;

namespace EveSettingsSaviour
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<SettingsFolder> _settingsFolders;

        public MainWindow()
        {
            InitializeComponent();

            
        }


        private async void btn_ScanSettings_Click(object sender, RoutedEventArgs e)
        {
            btn_ReadSettings.IsEnabled = false;

            _settingsFolders = await Task.Run (() => SettingsManager.ScanSettings(Enumerations.Servers.Tranquility).ToList());

            sp_targets.Children.Clear();
            source_userFile.Items.Clear();
            source_characterFile.Items.Clear();

            foreach (SettingsFolder s in _settingsFolders)
            {
                foreach (UserFile u in s.UserFiles)
                {
                    ComboBoxItem item = new ComboBoxItem();


                    item.Content = $"{u.Id} - {u.FilePath}";

                    source_userFile.Items.Add(item);
                }

                foreach (CharacterFile c in s.CharacterFiles)
                {
                    ComboBoxItem item = new ComboBoxItem();


                    item.Content = $"{s.FolderName} - {c.Character.Name}";

                    source_characterFile.Items.Add(item);
                }

                SettingsFolderControl sfc = new SettingsFolderControl(s);
                
                sp_targets.Children.Add(sfc);
            }
            btn_ReadSettings.IsEnabled = true;
        }

        private async void btn_CopySettings_Click(object sender, RoutedEventArgs e)
        {

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

            if(bothGotPath)
            {
                btn_backupSelected.IsEnabled = true;
            }
            else
            {
                btn_backupSelected.IsEnabled = false;
            }

        }

        private void btn_loadBackup_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
