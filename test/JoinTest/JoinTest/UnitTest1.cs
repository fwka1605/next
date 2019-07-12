using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace JoinTest
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {

            IWebDriver chrome = new ChromeDriver(); // Chromeブラウザの立ち上げ

            chrome.Url = "https://www.google.co.jp/"; // ブラウザのURLに入力
            IWebElement element = chrome.FindElement(By.Name("q"));
            element.SendKeys("Cheese!"); // ブラウザの検索条件に入力
            element.Submit(); // ボタンを押す

            Assert.AreEqual(chrome.Title, "Cheese! - Google 検索"); // 実行結果の確認

            chrome.Quit();
        }
    }
}
