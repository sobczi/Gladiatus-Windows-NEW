﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using Microsoft.SqlServer.Server;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;

namespace Gladiatus_NEW
{
    class Program
    {
        static public ChromeDriver driver;
        static public Actions ac;
        static public int wait = 250;
        static readonly NotifyIcon notify = new NotifyIcon();
        static readonly List<Thread> threads = new List<Thread>();

        static void Notify_doubleClick(object Sender, EventArgs e)
        {
            Sys.Kill_all(threads);
            Application.Exit();
        }

        static void Notify_click(object Sender, EventArgs e)
        {
            User.Default.headless = !User.Default.headless;
            User.Default.Save();
            if(driver != null)
                driver.Close();
            Run_bot();
        }

        static void Run_bot()
        {
            Sys.Kill_all(threads);
            Thread bot = new Thread(Bot);
            bot.Start();
            threads.Add(bot);
        }

        static void Bot()
        {
            User.Default.username = "sobczi";
            User.Default.password = "p9su9w64";
            User.Default.server = "35";
            User.Default.expedition = true;
            User.Default.expedition_option = "4";
            User.Default.dungeon = true;
            User.Default.dungeon_advenced = true;
            User.Default.pack = true;
            User.Default.pack_level = 50000;
            User.Default.heal_backpack = "512";
            User.Default.heal_level = 30;
            User.Default.free_backpack = "514";
            User.Default.extract_backpack = "515";
            User.Default.extract_orange = true;
            User.Default.extract_red = true;

            User.Default.sell_items = true;
            User.Default.sell_purple = true;
            User.Default.sell_rubles = true;
            User.Default.headless = false;
            User.Default.Save();

            Sys.Kill_chromes();
            var driverService = ChromeDriverService.CreateDefaultService();
            driverService.HideCommandPromptWindow = true;
            var driverOptions = new ChromeOptions();
            if (Screen.AllScreens.Length > 1)
                driverOptions.AddArgument("--window-position=2000,0");
            if (User.Default.headless)
                driverOptions.AddArgument("--headless");
            driverOptions.AddExtension("settings/GladiatusTools.crx");
            driver = new ChromeDriver(driverService, driverOptions);
            driver.Manage().Window.Maximize();
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromMilliseconds(wait);
            ac = new Actions(driver);

            Task.Login();
            Task.Disable_notifications();
            if (true)
            {
                Extract.Extract_items();
                bool exp = true;
                bool dung = true;
                int it = 0;
                while (exp || dung || Task.Hades_costume())
                {
                    exp = Task.Expedition();
                    dung = Task.Dungeon();
                    if(++it == 5)
                    {
                        Extract.Extract_items();
                        Pack.Search();
                        it = 0;
                    }
                    Pack.Buy();
                }
                Shop.Sell();
                Extract.Extract_items();
                Pack.Buy();
                Pack.Search();
                driver.Close();
            }
            Sys.Sleep();
        }

        static void Main()
        {
            Thread bot = new Thread(Bot);
            bot.Start();
            threads.Add(bot);

            Thread mouse_mov = new Thread(Sys.Catch_mouse);
            mouse_mov.Start();

            //notify.Visible = true;
            //notify.Text = "Gladiatus BOT";
            //notify.Icon = new Icon("resources/icon.ico");
            //notify.DoubleClick += new System.EventHandler(Notify_doubleClick);
            //notify.Click += new System.EventHandler(Notify_click);
            //Application.Run();
        }
    }
}
