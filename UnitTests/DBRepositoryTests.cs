using Microsoft.VisualStudio.TestTools.UnitTesting;
using zulilySurvey;
using System.Collections.Generic;
using System.Linq;
using System;
using zulilySurvey.Entities;
using zulilySurvey.Data;
using MongoDB.Driver;

namespace UnitTests
{
    /// <summary>
    /// This test class creates a Test DB in Mongo and then inserts sample Surveys into the DB. Exercises and tests insertation, deletion and querying of survey data
    /// TODO: Create similar tests for Users in the DB and other items and respositories.  Not done in the interest of time.
    /// </summary>
    [TestClass]
    public class DBRepositoryTests
    {
        private List<Survey> TestSurveys = new List<Survey>();


        /// <summary>
        /// Creat a test DB in Mongo, then insert test survey data into the MongoDB
        /// </summary>
        [TestInitialize]
        public void StartUp()
        {
            //These should not be stored here but rather in some configuration file that is used when test runner is running
            RepositoryFactory._connectionString = "mongodb://54.241.188.114:27000/";
            RepositoryFactory._databaseName = "Staging";

            //ensure we have a clean DB to start with. The Cleanup Method below does this, but just being sure.
            var _client = new MongoClient(RepositoryFactory._connectionString);
            _client.DropDatabase(RepositoryFactory._databaseName);

            //Create sample test surveys with images
            TestSurveys = new List<Survey>()
            {
                { new Survey() {
                                AgeRange = AgeRange.Range0to4,
                                Gender = Gender.Male,
                                Type = SurveyType.Template,
                                Images = new List<Image>() {
                                    new Image("blue", "cute"){
                                        Url = new Uri("https://a248.e.akamai.net/media.zulily.com/images/cache/event/357x357/235228/58c1e852d2ae870001c4a3b0/202662_newbornnecessitiesboys_hp_2016_1015_md1_1476489650_109907.c9a7f8af-65e1-42c1-8152-76c6eb2edfc6.jpg"),
                                        Description = "Boys baby jumpers"
                                    },
                                    new Image("warm", "comfy", "sophiscated", "wool"){
                                        Url = new Uri("https://a248.e.akamai.net/media.zulily.com/images/cache/event/289x289/228748/58b9f4aad2ae870001c48fb7/228748_StillaLittleChilly_HP_2017_0311_MEB2_87344.50a9d8cd-8c53-47f3-b85f-f680a0f5bdfe.jpg"),
                                        Description = "Boys tan wool sweater with buttons"
                                    },
                                    new Image("blue", "daddy", "hunting", "outdoors"){
                                        Url = new Uri("https://a248.e.akamai.net/media.zulily.com/images/cache/event/289x289/229570/58b99c81d2ae870001c48bf5/229570_HuntingAndFishing_HP_2017_0312_CT2_236412.70dac3c3-9b6d-42ea-b1dd-4f25547efaa7.jpg"),
                                        Description = "Boys Jumpers with hunting logos"
                                    },
                                }
                            }       
                },
                { new Survey() {
                                AgeRange = AgeRange.Range0to4,
                                Gender = Gender.Female,
                                Type = SurveyType.Template,
                                Images = new List<Image>() {
                                    new Image("cool", "bright colors", "summery", "swim"){
                                        Url = new Uri("https://a248.e.akamai.net/media.zulily.com/images/cache/event/390x390/hp/2017_03/223696_prepspringbr_kids_hp3_1488916166.jpg"),
                                        Description = "Baby Girl on beach with swim gear and sunglasses"
                                    },
                                    new Image("oldtime", "frilly", "sophiscated", "eye-catching", "pastels"){
                                        Url = new Uri("https://a248.e.akamai.net/media.zulily.com/images/cache/event/390x390/221881/58c071b84fa8060001047a7d/221881_LemonLovesLimeBabytoBigKids_HP_2017_0310_KR1_181739.4e4d03c2-2120-4cd6-87aa-8d5c3ffee2c5.jpg"),
                                        Description = "baby girls with frilly cotton dresses"
                                    },
                                    new Image("red", "hoodie", "street style", "love"){
                                        Url = new Uri("https://a248.e.akamai.net/media.zulily.com/images/cache/event/289x289/228748/58b9f4f1d2ae870001c48fbb/228748_BurrrItsColdinHere_HP_2017_0311_MEB1_120922.5f6cf4df-289b-48cf-bae1-f2b654d9cda3.jpg"),
                                        Description = "Baby girl with red hoodie and love branding. Street smart"
                                    },
                                }
                            }
                },
                 { new Survey() {
                                AgeRange = AgeRange.Range30to45,
                                Gender = Gender.Female,
                                Type = SurveyType.Template,
                                Images = new List<Image>() {
                                    new Image("patriotic", "relaxed", "country music", "simple", "cuttoffs"){
                                        Url = new Uri("https://a248.e.akamai.net/media.zulily.com/images/cache/event/390x390/232173/58c2e7a64fa806000104822c/232173_YouMayHaveMissedIt_HP_2017_0312_CG1_106828.6044dde2-b5b0-484e-8803-f36758b2bc32.jpg"),
                                        Description = "Woman with cutoff jean shorts and simple americana shirt. "
                                    },
                                    new Image("sporty", "spandex", "running", "exercise"){
                                        Url = new Uri("https://a248.e.akamai.net/media.zulily.com/images/cache/event/390x390/231988/58c1d4cd4fa8060001047f8d/231988_ReadytoRun_HP_2017_0310_KM1_294058.65cb067d-42fc-437f-95da-bfe3a01e9a07.jpg"),
                                        Description = "Woman running with spandex and running shoes"
                                    },
                                    new Image("fresh", "dress", "bright", "above knee"){
                                        Url = new Uri("https://a248.e.akamai.net/media.zulily.com/images/cache/event/390x390/232175/58c19fd54fa8060001047d5e/232175_MixnMatchWithFloralsAndStripes_HP_2017_0310_MEB1_110232.ca1e0f37-5c3d-4342-bdce-8babe0d6d784.jpg"),
                                        Description = "Woman with floral print skirt and pastel top on dress. Young, bright, fresh"
                                    },
                                }
                            }
                },
                 { new Survey() {
                                AgeRange = AgeRange.Range30to45,
                                Gender = Gender.Male,
                                Type = SurveyType.Template,
                                Images = new List<Image>() {
                                    new Image("business", "professional", "dapper", "leather"){
                                        Url = new Uri("https://a248.e.akamai.net/media.zulily.com/images/cache/event/289x289/231091/58bf197e4fa80600010475a0/231091_WhatADapperDuo_HP_2017_0311_CT1_181872.79c8f90f-56a9-4273-bf5e-19ec237fdb86.jpg"),
                                        Description = "mens leather business shoes with leather briefcase"
                                    },
                                    new Image("khakis", "simple", "beard", "knit sweater", "pickup truck"){
                                        Url = new Uri("https://a248.e.akamai.net/media.zulily.com/images/cache/event/390x390/234260/58c2071fd2ae870001c4a486/234260_Browning_HP_2017_0312_AD2_171550.76477db3-a178-4453-9917-baeccdeb1418.jpg"),
                                        Description = "man with simple khakis and close knitted sweater on pickup truck"
                                    },
                                    new Image("sporty", "running", "determined", "exercise"){
                                        Url = new Uri("https://a248.e.akamai.net/media.zulily.com/images/cache/event/390x390/231213/58c195804fa8060001047cfb/231213_SODO_HP_2017_0310_BW2_154861.3f5906a3-c625-418b-9dcd-f4651e34ff9a.jpg"),
                                        Description = "Man running on dock with exercise clothing"
                                    },
                                }
                            }
                },

            };
            foreach (var Image in TestSurveys)
            {
                Survey.Repository.InsertOrReplace(Image);
            }

        }


        /// <summary>
        /// Delete the Test DB after tests are complete
        /// </summary>
        [TestCleanup]
        public void Cleanup()
        {
            var _client = new MongoClient(RepositoryFactory._connectionString);
            _client.DropDatabase(RepositoryFactory._databaseName);
        }

        /// <summary>
        /// Insert a new survey in the DB and make sure it is accessible. Then delete it.
        /// </summary>
        [TestMethod]
        public void InsertandDeleteSurvey()
        {

            var Survey = new Survey()
            {
                AgeRange = AgeRange.Range0to4,
                Gender = Gender.Male,
                Type = SurveyType.Template,
                Images = new List<Image>() {
                                    new Image("business", "professional", "dapper", "leather"){
                                        Url = new Uri("https://a248.e.akamai.net/media.zulily.com/images/cache/event/289x289/231091/58bf197e4fa80600010475a0/231091_WhatADapperDuo_HP_2017_0311_CT1_181872.79c8f90f-56a9-4273-bf5e-19ec237fdb86.jpg"),
                                        Description = "mens leather business shoes with leather briefcase"
                                    },
                                    new Image("khakis", "simple", "beard", "knit sweater", "pickup truck"){
                                        Url = new Uri("https://a248.e.akamai.net/media.zulily.com/images/cache/event/390x390/234260/58c2071fd2ae870001c4a486/234260_Browning_HP_2017_0312_AD2_171550.76477db3-a178-4453-9917-baeccdeb1418.jpg"),
                                        Description = "man with simple khakis and close knitted sweater on pickup truck"
                                    },
                                    new Image("sporty", "running", "determined", "exercise"){
                                        Url = new Uri("https://a248.e.akamai.net/media.zulily.com/images/cache/event/390x390/231213/58c195804fa8060001047cfb/231213_SODO_HP_2017_0310_BW2_154861.3f5906a3-c625-418b-9dcd-f4651e34ff9a.jpg"),
                                        Description = "Man running on dock with exercise clothing"
                                    },
                                }
            };

            Survey.Repository.InsertOrReplace(Survey);
            Assert.IsNotNull(Survey.Repository.GetById(Survey.Id));
            Survey.Repository.DeleteById(Survey.Id);
            Assert.IsNull(Survey.Repository.GetById(Survey.Id));
        }

        [TestMethod]
        public void QuerySurveys()
        {
            var surveys = Survey.Repository.GetByAge(AgeRange.Range0to4);
            Assert.IsTrue(surveys.Count == 2);
            Assert.IsTrue(surveys.Any(t => t.sId == TestSurveys[0].sId));
            Assert.IsTrue(surveys.SelectMany(t => t.Images).Count() == 6);

            surveys = Survey.Repository.GetByGender(Gender.Female);
            Assert.IsTrue(surveys.Count == 2);
            Assert.IsTrue(surveys.Any(t => t.sId == TestSurveys[1].sId));
            Assert.IsTrue(surveys.SelectMany(t => t.Images).Count() == 6);

            surveys = Survey.Repository.GetByAgeAndGender(AgeRange.Range0to4, Gender.Female);
            Assert.IsTrue(surveys.Count == 1);
            Assert.IsTrue(surveys.Any(t => t.sId == TestSurveys[1].sId));
            Assert.IsTrue(surveys.SelectMany(t => t.Images).Count() == 3);
        }

    }
}
