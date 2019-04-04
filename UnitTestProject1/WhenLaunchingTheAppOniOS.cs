﻿using NUnit.Framework;
using System.Threading.Tasks;
using BrowserStackIntegration;
using System.Linq;
using System;
using System.Diagnostics;
using OpenQA.Selenium;

namespace MobileAppTests
{
    //[TestFixture("parallel", "iphone-8")]
    //[TestFixture("parallel", "iphone-8-plus")]
    //[TestFixture("parallel", "iphone-se")]
    [TestFixture("parallel", "iphone-xs")]
    //[TestFixture("parallel", "ipad-pro")]
    //[TestFixture("parallel", "ipad-5th")]
    [Parallelizable(ParallelScope.Fixtures)]
    public class WhenLaunchingTheAppOniOS : DayPartingScreen
    {
        public WhenLaunchingTheAppOniOS(string profile, string device) : base(profile, device) { }

        //[Test]
        public async Task TheUserCanAccessTheDayPartingScreen()
        {
            await IOSUserCanAccessTheDayPartingScreen();
            Assert.IsTrue(iosDriver.FindElementByName
                ("Good Morning Sponsored By non-module|-4|ad|advertisementModule|0|manually placed in splash-screen|").Displayed);

        }

        //[Test]
        public async Task TheDayPartingBannerIsGenerated()
        {
            await IOSDayPartingBannerIsGenerated();

            string sponsoredByElement = iosDriver.FindElementByName("Sponsored By non-module|-4|ad|advertisementModule|0|manually placed in splash-screen|").Text;
            string dayPartingBannerElement = iosDriver.FindElementByName("Good {*} Sponsored By non-module|-4|ad|advertisementModule|0|manually placed in splash-screen|").Text;

            Assert.IsTrue(sponsoredByElement.Any());
            Assert.IsTrue(dayPartingBannerElement.Any());
            Assert.IsTrue(dayPartingBannerElement.Contains("Good "), dayPartingBannerElement + "Day Parting Banner does not contain 'Good ***'.");
            Assert.IsTrue(sponsoredByElement.Contains("Sponsored By"), sponsoredByElement + "'Sponsored by' message does not contain 'Sponsored By'.");
        }

        [Test]
        public async Task TheDayPartingScreenAdIsPresent()
        {
            for (int i = 0; ; i++)
            {
                await ApproveiOSAlerts();
                if (i >= 15) Assert.Fail("The Day Parting Screen Ad is not present.");
                try
                {
                    var dayPartingAdElement = iosDriver.FindElementByName("non-module|-4|ad|advertisementModule|0|manually placed in splash-screen|");
                    //if (IsiOSElementPresent(By.Name("non-module|-4|ad|advertisementModule|0|manually placed in splash-screen|")))
                    break;
                }
                catch (Exception ex)
                {
                    string message = $"The Day Parting Screen Ad is not present. {ex}";
                    Debug.WriteLine(message);
                    //Debug.ReadLine();
                    Console.WriteLine(message);
                }
                Wait(1);
            }
        }


    }
}
