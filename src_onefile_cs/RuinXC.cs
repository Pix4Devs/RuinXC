using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium;
using System.Threading;
using Microsoft.Win32;
using System.Diagnostics;
using OpenQA.Selenium.Support.UI;
using System.IO;
using System.Net.Http;
using System.IO.Compression;
using System.Runtime;

namespace RuinXC
{
    /*
     * Programmed by Z3NTL3 (irl name Efdal)
     * Compiled version with an improved source will be on pix4.dev/discord soon
     */
    public partial class RuinXC : Form
    {
        public class Glob
        {
            public static int amountTh;
            public static string genName;
            public static List<string> usedNames = new List<string>();
            public static List<string> karakters = new List<string>();
            public static List<string> karaktersUpper = new List<string>();
            public static List<Int32> getallen = new List<Int32>();
            public static List<string> specials = new List<string>();
            public static bool SuccesvolGedownload;
            public static bool KanNiDownloaden;
            public static bool chromeErr;
            public static bool driverInstalled;
            public static bool gestart;

        }
        public RuinXC()
        {
            InitializeComponent();
        }
        private async void driverfixtaak(object callb)
        {
            Glob.driverInstalled = false;
            Glob.chromeErr = false;
            Process[] workers = Process.GetProcessesByName("chromedriver");
            foreach (Process worker in workers)
            {
                worker.Kill();
                worker.WaitForExit();
                worker.Dispose();
            }
               
              /*
                   https://chromedriver.storage.googleapis.com/102.0.5005.61/chromedriver_win32.zip
                   https://chromedriver.storage.googleapis.com/101.0.4951.41/chromedriver_win32.zip
                   https://chromedriver.storage.googleapis.com/100.0.4896.60/chromedriver_win32.zip 
              */

            Glob.SuccesvolGedownload = false;
            Glob.KanNiDownloaden = false;
            string regPathChrome = @"HKEY_LOCAL_MACHINE\Software\Microsoft\Windows\CurrentVersion\App Paths\chrome.exe";
            object Path = Registry.GetValue(regPathChrome, "", null);

            string currentDir = Directory.GetCurrentDirectory();

            if (Path != null)
            {
                string chromeVersion = FileVersionInfo.GetVersionInfo(Path.ToString()).FileVersion;
                List<string> GetDirectVersionBaseNum = new List<string>();

                string[] ver = chromeVersion.Split('.');

                for (int i = 0; i < 1; i++)
                {
                    GetDirectVersionBaseNum.Add(ver[i]);
                }

                string ChromeVersion = GetDirectVersionBaseNum[0];
                HttpClient client = new HttpClient();


                if (File.Exists("chromedriver.zip"))
                {
                    File.Delete("chromedriver.zip");
                }

                if (ChromeVersion.Contains("100"))
                {
                    try
                    {
                        HttpResponseMessage response = await client.GetAsync("https://chromedriver.storage.googleapis.com/100.0.4896.60/chromedriver_win32.zip");
                        response.EnsureSuccessStatusCode();
                        byte[] content = await response.Content.ReadAsByteArrayAsync();
                        FileStream fs = File.Create($"{currentDir}\\chromedriver.zip");
                        fs.Write(content, 0, content.Length);
                        fs.Close();
                        Glob.SuccesvolGedownload = true;
                        client.Dispose();
                    }

                    catch
                    {
                        Glob.KanNiDownloaden = true;
                    }
                    finally
                    {
                        client.Dispose();
                    }
                }
                else if (ChromeVersion.Contains("101"))
                {
                    try
                    {
                        HttpResponseMessage response = await client.GetAsync("https://chromedriver.storage.googleapis.com/101.0.4951.41/chromedriver_win32.zip");
                        response.EnsureSuccessStatusCode();
                        byte[] content = await response.Content.ReadAsByteArrayAsync();
                        FileStream fs = File.Create($"{currentDir}\\chromedriver.zip");
                        fs.Write(content, 0, content.Length);
                        fs.Close();
                        Glob.SuccesvolGedownload = true;
                        client.Dispose();
                    }
                    catch
                    {
                        Glob.KanNiDownloaden = true;
                    }
                    finally
                    {
                        client.Dispose();
                    }
                }
                else if (ChromeVersion.Contains("102"))
                {
                    try
                    {
                        HttpResponseMessage response = await client.GetAsync("https://chromedriver.storage.googleapis.com/102.0.5005.61/chromedriver_win32.zip");
                        response.EnsureSuccessStatusCode();
                        byte[] content = await response.Content.ReadAsByteArrayAsync();
                        FileStream fs = File.Create($"{currentDir}\\chromedriver.zip");
                        fs.Write(content, 0, content.Length);
                        fs.Close();
                        Glob.SuccesvolGedownload = true;
                    }
                    catch
                    {
                        Glob.KanNiDownloaden = true;
                    }
                    finally
                    {
                        client.Dispose();
                    }
                }
                else
                {
                    Glob.chromeErr = true;
                    MessageBox.Show("You are using a very old chrome version.\n\nPlease install chrome version 100,101 or 102", "Something Bad Happened", MessageBoxButtons.OK, MessageBoxIcon.Error);

                }
                if (Glob.SuccesvolGedownload == true)
                {
                    if (File.Exists($"{currentDir}\\chromedriver.exe"))
                    {
                        File.Delete($"{currentDir}\\chromedriver.exe");
                    }
                    ZipFile.ExtractToDirectory($"{currentDir}\\chromedriver.zip", currentDir);

                    if (File.Exists($"{currentDir}\\chromedriver.exe"))
                    {
                        Glob.driverInstalled = true;
                        MessageBox.Show("Successfully installed the correct ChromeDriver version compabitle with your Chrome version.", "Compabitility with Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("Cannot find 'chromedriver.exe' on local file directories.", "Something Bad Happened", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    }

                }
            }
            else
            {
                Glob.chromeErr = true;
                MessageBox.Show("You do not have Chrome installed on your PC. Please install chrome version 100, 101 or 102!", "Something Bad Happened", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
        }
        private void label5_Click(object sender, EventArgs e)
        {
            var optie = MessageBox.Show(
                "You are about to exit this application. Are you sure about this?",
                "Exit Software",
                MessageBoxButtons.OKCancel,
                MessageBoxIcon.Question);

            if (optie == DialogResult.OK)
            {
                this.Close();
            }
        }

        private IWebDriver ConfigBot()
        {
            var chromeOptions = new ChromeOptions();

            List<string> jobsToDo = new List<string>()
            {
                "headless",
                "--silent",
                "--window-size=1920,1080"
            };

            foreach (string job in jobsToDo)
            {
                chromeOptions.AddArgument(job);
            }
            ChromeDriverService service = ChromeDriverService.CreateDefaultService();
            service.HideCommandPromptWindow = true;
            IWebDriver driver = new ChromeDriver(service, chromeOptions);

            return driver;
        }
        private void LaadGrid(object a)
        {     
            grid.Invoke(new Action(() =>
            {
                grid.View = View.Details;
                grid.Columns.Add("Nickname", 200, HorizontalAlignment.Left);
                grid.Columns.Add("Status",200, HorizontalAlignment.Left);
                grid.Columns.Add("Platform", 200, HorizontalAlignment.Left);
                grid.Columns.Add("Stand", 200, HorizontalAlignment.Left);
            }));
        }

        public class XPATH
        {
            private string Xpath;
            private int Time;
            private IWebDriver Driver;

            public XPATH(IWebDriver driver, string path, int time)
            {
                var elem = new WebDriverWait(driver, TimeSpan.FromSeconds(time)).Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.XPath(path)));
                this.Xpath = path;
                this.Time = time;
                this.Driver = driver;
            }

            public void Click()
            {
                Driver.FindElement(By.XPath(Xpath)).Click();
            }

            public void SendKeys(string key)
            {
                Driver.FindElement(By.XPath(Xpath)).SendKeys(key);
            }
            public void Clear()
            {
                Driver.FindElement(By.XPath(Xpath)).Clear();
            }

            public void Quit()
            {
                Driver.Quit();
            }
        }

        private void CleanGrid()
        {
            grid.Clear();
            grid.Items.Clear();
        }

        private void BackBoneTH(object a)
        {
            Glob.usedNames.Add("a");
            string[] kars = { "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "o", "p", "q", "r", "s", "t", "v", "w", "x", "y", "z" };
            string[] specialkars = { "!", "@", "#", "$", "%", "^", "&", "*", "(", ")", "_", "+", "-", "_", ">", "<", "?"};
           
            Glob.karakters.AddRange(kars);
            Glob.specials.AddRange(specialkars);

            foreach (string k in Glob.karakters)
            {
                Glob.karaktersUpper.Add(k.ToUpper());
            }

            for (int i = 0; i < 11; i++)
            {
                Glob.getallen.Add(i);
            }
        }
        
        private void RuinXC_Load(object sender, EventArgs e)
        {
            string operatingSystem = Environment.OSVersion.ToString();

            if (operatingSystem.ToLower().Contains("windows"))
            {
                ThreadPool.QueueUserWorkItem(BackBoneTH);
                ThreadPool.QueueUserWorkItem(driverfixtaak);
                ThreadPool.QueueUserWorkItem(LaadGrid);
                botCnt.ReadOnly = true;
                logConsole.ReadOnly = true;
                this.ActiveControl = this.panel1;
            }
            else
            {
                MessageBox.Show("You can only run RUINXC on windows operating system.", "OS ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
            }
            
            

        }
        private string Tijd()
        {
            DateTime time = DateTime.Now;
            string timeString = time.ToString();

            return timeString;
        }
        private async void Bot(object A)
        {
            IWebDriver driver = ConfigBot();

            try
            {
                Glob.genName = "";
                int mstime = 15;
                Glob.amountTh++;
                DateTime time = DateTime.Now;
                string timeString = time.ToString();


                logConsole.Invoke(new Action(() =>
                {

                    logConsole.ForeColor = Color.Green;
                    logConsole.Text = $"{logConsole.Text}{timeString} - Thread {A} Setted Ready\n";
                }));

               
                driver.Navigate().GoToUrl("https://lessonup.app/code");
                XPATH EnterCode = new XPATH(driver, "//*[@id='pincodeInput']", mstime);
                EnterCode.Clear();

                string getCode = code.Text.Trim();
                EnterCode.SendKeys(getCode);
                XPATH NickName = new XPATH(driver, "//*[@id='player-name']", mstime);
                var random = new Random();
              
                bool genL = true;
                while (genL)
                {
                    Glob.genName = "";
                    int rand1 = random.Next(Glob.karakters.Count);
                    int rand2 = random.Next(Glob.karaktersUpper.Count);
                    int rand3 = random.Next(Glob.specials.Count);
                    int rand4 = random.Next(Glob.getallen.Count);

                    string generateName = $"{nick.Text}-{Glob.karakters[rand1]}{Glob.karaktersUpper[rand2]}{Glob.specials[rand3]}{Glob.getallen[rand4]}";
                    foreach (string usedName in Glob.usedNames)
                    {
                        if (generateName != usedName)
                        {
                            Glob.genName = generateName;
                            await Task.Delay(0);
                            genL = false;
                            break;
                        }

                    }
                }

                NickName.SendKeys(Glob.genName);
                XPATH JoinBtn = new XPATH(driver, "/html/body/div[3]/div/div[3]/div/div[1]/form/div", mstime);

                JoinBtn.Click();
                await Task.Delay(0);

                XPATH Gelukt = new XPATH(driver, "/html/body/div[3]/div/div[3]/div/div[1]/p[2]", mstime);
                if (!grid.Items.ContainsKey(Glob.genName))
                {
                    grid.Invoke(new Action(() =>

                    {
                        grid.View = View.Details;
                        ListViewItem item1 = new ListViewItem();
                        item1.Text = $"{Glob.genName}";
                        item1.SubItems.Add("Joined Game");
                        item1.SubItems.Add("LessonUp");
                        item1.SubItems.Add($"{A}");
                        grid.Items.Add(item1);
                    }));
                }
                else
                {
                    grid.Invoke(new Action( () =>

                    {
                        grid.View = View.Details;
                        ListViewItem item1 = new ListViewItem();
                        item1.Text = $"{Glob.genName}";
                        item1.SubItems.Add("Failed");
                        item1.SubItems.Add("LessonUp");
                        item1.SubItems.Add($"{A}");
                        grid.Items.Add(item1);
                    }));
                }

                botCnt.Invoke(new Action(() =>

                {
                    int aantalJoined = Convert.ToInt32(botCnt.Text) + 1;
                    string aantalJS = Convert.ToString(aantalJoined);
                    botCnt.Text = $"{aantalJS}";
                }));


                
            }
            catch
            {
                logConsole.Invoke(new Action(() =>
                {
                    DateTime time = DateTime.Now;
                    string timeString = time.ToString();
                    logConsole.ForeColor = Color.Red;
                    logConsole.Text = $"{logConsole.Text}{timeString} - LessonUp blocked request, try again later.\n";
                }));
                
                
            }
            finally
            {
                driver.Quit();
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                Convert.ToInt32(threads.Text);

                if (Glob.chromeErr == true)
                {
                    MessageBox.Show("Chrome was not found on your computer", "Chrome Err", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else if (Glob.SuccesvolGedownload == false)
                {
                    MessageBox.Show("There was an error while downloading chromedriver.", "ChromeDriver CDN", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else if (Convert.ToInt32(threads.Text) > 30)
                {
                    MessageBox.Show("Maximum threads are 30 , so you do not overload your sys.", "Thread Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    if (lessonup.Checked)
                    {
                        try
                        {
                            if (String.IsNullOrEmpty(threads.Text))
                            {
                                MessageBox.Show("Please add some non decimal numbers on threads.", "Thread Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                            }
                            else
                            {
                                Convert.ToInt32(threads.Text);

                                if (String.IsNullOrEmpty(code.Text))
                                {
                                    MessageBox.Show("Please provide a game code to spam.", "GameCode Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }
                                else
                                {
                                    if (String.IsNullOrEmpty(nick.Text))
                                    {
                                        MessageBox.Show("Please enter a nick to spam with", "NickName Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    }

                                    else
                                    {
                                        MessageBox.Show("Will send the bots after the threads were setup.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                        int ths = Convert.ToInt32(threads.Text) + 1;
                                        Glob.gestart = true;
                                        for (int i = 1; i < ths; i++)
                                        {
                                            ThreadPool.QueueUserWorkItem(Bot, i);
                                        }
                                        Glob.gestart = false;

                                    }
                                }
                            }

                        }
                        catch
                        {
                            MessageBox.Show("Only a number, insert no decimal numbers or letters on threads.", "Unrecognised chars", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    else { MessageBox.Show("No platforms are checked.", "Unrecognised platform", MessageBoxButtons.OK, MessageBoxIcon.Error); }
                }
            }
            catch
            {
                MessageBox.Show("Threads can only be full numbers with none decimals or letters included.", "Unrecognised chars", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            // top most application 
            if (checkBox1.Checked)
            {
                this.TopMost = true;
            }
            else
            {
                this.TopMost = false;
            }
        }
        private void TermBots(object Callb)
        {
            var drivers = Process.GetProcesses().Where(pr => pr.ProcessName == "chromedriver");
            foreach (var process in drivers)
            {
                process.Kill();
            }

            var chrome = Process.GetProcesses().Where(pr => pr.ProcessName == "chrome");
            foreach (var process in chrome)
            {
                process.Kill();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (Glob.gestart == true)
            {
                ThreadPool.QueueUserWorkItem(TermBots);
            }
            else
            {
                MessageBox.Show("You need to send the bots first before you try to terminate", "Termination Err", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            
        }
    }
}
