﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using System.Windows.Shapes;

namespace RelhaxModpack.Windows
{
    public enum AskCloseWoTResult
    {
        Retry,
        CancelInstallation,
        ForceClosed
    }
    /// <summary>
    /// Interaction logic for AskCloseWoT.xaml
    /// </summary>
    public partial class AskCloseWoT : RelhaxWindow
    {
        public AskCloseWoTResult AskCloseWoTResult { get; set; } = AskCloseWoTResult.CancelInstallation;

        /// <summary>
        /// Create an instance of the AskCloseWoT window
        /// </summary>
        public AskCloseWoT()
        {
            InitializeComponent();
        }

        private void WoTRunningCancelInstallButton_Click(object sender, RoutedEventArgs e)
        {
            AskCloseWoTResult = AskCloseWoTResult.CancelInstallation;
            DialogResult = false;
            Close();
        }

        private void WoTRunningForceCloseButton_Click(object sender, RoutedEventArgs e)
        {
            AskCloseWoTResult = AskCloseWoTResult.Retry;
            Logging.Info("Getting WoT process(es)");
            Process WoTProcess = Utils.GetProcess(Settings.WoTProcessName, Settings.WoTDirectory);
            if(WoTProcess == null)
            {
                Logging.Error("Failed to get process (null result)");
                MessageBox.Show(Translations.GetTranslatedString("failedCloseProcess"));
                return;
            }
            try
            {
                WoTProcess.Kill();
                System.Threading.Thread.Sleep(100);
                if (WoTProcess.HasExited)
                {
                    Logging.Info("success in ending process!");
                    AskCloseWoTResult = AskCloseWoTResult.ForceClosed;
                    DialogResult = true;
                    Close();
                }
                else
                {
                    Logging.Error("Failed to get process (timeout)");
                    MessageBox.Show(Translations.GetTranslatedString("failedCloseProcess"));
                    return;
                }
            }
            catch (Exception ex)
            {
                Logging.Error("Failed to kill process");
                Logging.Info(ex.ToString());
                MessageBox.Show(Translations.GetTranslatedString("failedCloseProcess"));
                return;
            }
        }

        private void WoTRunningRetryButton_Click(object sender, RoutedEventArgs e)
        {
            AskCloseWoTResult = AskCloseWoTResult.Retry;
            DialogResult = true;
            Close();
        }
    }
}
