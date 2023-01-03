using System;
using Xunit;
using HOTS.DataObjects;
using System.Diagnostics;
using System.Collections.Generic;
using System.Threading.Tasks;
using HOTS.Logging;
using System.Text.RegularExpressions;

namespace HOTS.Tests
{
    public class HeroTest
    {
        [Fact]
        public void InvalidNameShouldThrowError()
        {
            WCFHost.HotsService hotsService = new WCFHost.HotsService();
            string result;
            var exception = Assert.Throws<AggregateException>(() =>
                {
                    var task = hotsService.InsertHero(new HeroModel
                    {
                        Name = "Brian123",
                        DateCreated = DateTime.Now,
                        IsActive = true,
                        Price = 5,
                        Type = HeroType.Assassin,
                        IsFreePromo = false
                    });

                    task.Wait();
                    result = task.Result;
                }
                );

            // Make sure the correct exception gets thrown
            Assert.Equal("The field Name must match the regular expression '^[A-Za-z ]+$'.", exception.InnerException.Message);
        }

        [Fact]
        public void InvalidPriceShouldThrowError()
        {
            WCFHost.HotsService hotsService = new WCFHost.HotsService();
            string result;
            var exception = Assert.Throws<AggregateException>(() =>
                {
                    var task = hotsService.InsertHero(new HeroModel
                    {
                        Name = CreateRandomName(),
                        DateCreated = DateTime.Now,
                        IsActive = true,
                        Price = 30,
                        Type = HeroType.Support,
                        IsFreePromo = true
                    });
                    task.Wait();
                    result = task.Result;
                }
                );

            // Make sure the correct exception gets thrown
            Assert.Equal("The field Price must be between 5 and 25.", exception.InnerException.Message);
        }

        [Fact]
        public void Insert10AsyncShouldSucceed()
        {
            WCFHost.HotsService hotsService = new WCFHost.HotsService();

            List<Task<string>> tasks = new List<Task<string>>();
            for (int i = 0; i < 10; i++)
            {
                tasks.Add(hotsService.InsertHero(new HeroModel
                {
                    Name = CreateRandomName(),
                    DateCreated = DateTime.Now,
                    IsActive = true,
                    Price = 12,
                    Type = HeroType.Support,
                    IsFreePromo = false
                }));
            }
            Logger.Instance.LogInfo("All Insert Tasks sent, waiting for completion of them all");
            // Wait until all 10 complete successfully
            // before exiting the test

            for (int j = 0; j < 10; j++)
            {
                var task = tasks[j];
                Logger.Instance.LogInfo("Waiting for task # " + (j + 1) + " to complete");
                task.Wait();
                Logger.Instance.LogInfo("Hero number " + (j + 1) + " in batch inserted.");
            }
            Logger.Instance.LogInfo("All Insert Tasks complete");
        }

        [Fact]
        public void UpdateHeroValidDataShouldSucceed()
        {
            // This test needs to run synchronously

            WCFHost.HotsService hotsService = new WCFHost.HotsService();
            string name = CreateRandomName();
            string result;
            var hero = new HeroModel
            {
                Name = name,
                DateCreated = DateTime.Now,
                IsActive = true,
                Price = 13,
                Type = HeroType.Support,
                IsFreePromo = true
            };
            var task = hotsService.InsertHero(hero);
            task.Wait();

            // Set free promo to true, and update it
            hero.IsFreePromo = true;

            task = hotsService.UpdateHero(hero);
            task.Wait();

            var updatedHero = hotsService.GetHero(hero.Name);
            updatedHero.Wait();

            Assert.Equal(true, updatedHero.Result.IsFreePromo);
        }

        [Fact]
        public void DeleteHeroValidDataShouldSucceed()
        {
            // This test needs to run synchronously

            WCFHost.HotsService hotsService = new WCFHost.HotsService();
            string name = CreateRandomName();
            var hero = new HeroModel
            {
                Name = name,
                DateCreated = DateTime.Now,
                IsActive = true,
                Price = 13,
                Type = HeroType.Support,
                IsFreePromo = false
            };
            var task = hotsService.InsertHero(hero);
            task.Wait();

            // Set free promo to true, and update it
            hero.IsFreePromo = true;

            var task2 = hotsService.DeleteHero(name);
            task2.Wait();

            var deletedHero = hotsService.GetHero(hero.Name);
            deletedHero.Wait();

            Assert.Equal(true, deletedHero.Result.IsDeleted);
        }

        [Fact]
        public void GetHeroesShouldSucceed()
        {
            WCFHost.HotsService hotsService = new WCFHost.HotsService();
            var task = hotsService.GetHeroes();
            task.Wait();
            var result = task.Result;

            Assert.NotNull(result);
        }

        private string CreateRandomName()
        {
            var name = Guid.NewGuid().ToString().Replace("-", string.Empty);
            name = Regex.Replace(name, "[^a-zA-Z]+", "");
            return name;
        }
    }
}
