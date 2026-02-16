using Microsoft.Win32;
using System;
using System.IO;
using System.Windows;

namespace Installer
{
    public partial class MainWindow : Window
    {
        private string selectedPath = string.Empty;

        public MainWindow()
        {
            InitializeComponent();
            InstallButton.IsEnabled = false;
        }

        private void BrowseButton_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFolderDialog
            {
                Title = "Select Assetto Corsa Competizione folder"
            };

            if (dialog.ShowDialog() == true)
            {
                selectedPath = dialog.FolderName;
                PathTextBox.Text = selectedPath;
                InstallButton.IsEnabled = true;
            }
        }

        private void InstallButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(selectedPath))
                {
                    MessageBox.Show("No folder selected.", "Error",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                // ELVÁRT ACCC ÚTVONAL
                string targetDir = Path.Combine(
                    selectedPath,
                    "AC2",
                    "Binaries",
                    "Win64"
                );

                if (!Directory.Exists(targetDir))
                {
                    MessageBox.Show(
                        "Invalid ACCC folder.\nExpected:\n...\\Assetto Corsa Competizione\n Open steam and copy root game folder!",
                        "Wrong path",
                        MessageBoxButton.OK,
                        MessageBoxImage.Error);
                    return;
                }

                string contentDir = Path.Combine(
                    AppDomain.CurrentDomain.BaseDirectory,
                    "content"
                );

                if (!Directory.Exists(contentDir))
                {
                    MessageBox.Show(
                        "Missing 'content' folder next to installer exe.",
                        "Installer broken",
                        MessageBoxButton.OK,
                        MessageBoxImage.Error);
                    return;
                }

                CopyDirectory(contentDir, targetDir);

                MessageBox.Show(
                    "SUCCESSFULLY INSTALLED\nCRACK MADE BY: knocker_lali\n ENJOY YOUR FREE DLC CONTENT!",
                    "Done",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Installation failed:\n" + ex.Message,
                    "Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        private void CopyDirectory(string sourceDir, string targetDir)
        {
            foreach (string file in Directory.GetFiles(sourceDir))
            {
                string destFile = Path.Combine(targetDir, Path.GetFileName(file));
                File.Copy(file, destFile, true); // override, leszarjuk mi volt ott
            }

            foreach (string directory in Directory.GetDirectories(sourceDir))
            {
                string destSubDir = Path.Combine(targetDir, Path.GetFileName(directory));
                Directory.CreateDirectory(destSubDir);
                CopyDirectory(directory, destSubDir);
            }
        }
    }
}
