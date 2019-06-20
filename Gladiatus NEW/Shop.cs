﻿using System.Collections.Generic;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace Gladiatus_NEW
{
    class Shop
    {
        private static readonly ChromeDriver driver = Program.driver;
        public static void Sell()
        {
            if (!User.Default.sell_items)
                return;

            int category = 1;
            int shop = 1;
            while(category <= 10)
            {
                Navigation.Packages();
                while (category <= 10)
                {
                    Navigation.Filter_packages(Get.Category_packages(category++), "");
                    Navigation.Backpack(User.Default.free_backpack);
                    Basic.Click_if("//a[@clas='paging_button paging_right_full']");
                    if (!Double_click_items(Get_items(driver.FindElementsByXPath("//div[@id='packages']//div[contains(@class,'draggable')]")), true))
                        break;
                }

                while (true)
                {
                    if (!Choose_shop(shop++))
                        return;
                    if (!Double_click_items(Get_items(driver.FindElementsByXPath("//div[@id='inv']//div[contains(@class,'draggable')]")), false))
                        break;
                }
            }
        }

        public static void Buy()
        {

        }

        private static bool Double_click_items(List<IWebElement> elements, bool packages)
        {
            bool result = false;
            if (elements.Count == 0 && !packages)
                result = false;

            string var = "inv";
            if (!packages)
                var = "shop";
            foreach(IWebElement element in elements)
            {
                string hash = element.GetAttribute("data-hash");
                Basic.Double_click(element);
                if (!Basic.Search_element("//div[@id='" + var + "']//div[@data-hash='" + hash + "']"))
                {
                    result = !result;
                    break;
                }
            }
            if (packages)
                return !result;
            return result;
        }

        private static bool Choose_shop(int shop)
        {
            switch(shop)
            {
                case 1:
                    Navigation.Main_menu("Broń");
                    break;
                case 2:
                    Navigation.Main_menu("Pancerz");
                    break;
                case 3:
                    Navigation.Main_menu("Handlarz");
                    break;
                case 4:
                    Navigation.Main_menu("Alchemik");
                    break;
                case 5:
                    Navigation.Main_menu("Żołnierz");
                    break;
                case 6:
                    Navigation.Main_menu("Malefica");
                    break;
                default:
                    Navigation.Main_menu("Broń");
                    if (!User.Default.sell_rubles)
                        return false;
                    Basic.Click_element("//input[@value='Nowe towary']");
                    break;
            }
            Basic.Click_element("//div[contains(@class,'shopTab')][contains(text(),'sprzedaj')]");
            Navigation.Backpack(User.Default.free_backpack);
            return true;
        }

        private static List<IWebElement> Get_items(IReadOnlyCollection<IWebElement> all)
        {
            List<IWebElement> elements = new List<IWebElement>();
            foreach(IWebElement element in all)
            {
                string quality = element.GetAttribute("data-quality");
                if (!User.Default.sell_purple && quality == "2" || quality == "3" || quality == "4")
                    continue;
                elements.Add(element);
            }
            return elements;
        }
    }
}
