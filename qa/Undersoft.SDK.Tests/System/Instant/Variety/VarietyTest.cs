using System.Collections.Generic;
using System.Uniques;
using RadicalR;
using Xunit;

namespace System.Instant.Tests
{
    public class VarietyTest
    {
        [Fact]
        public void Variety_Integrated_Test()
        {
            var profile = new Agreement()
            {
                Comments = "fdfdsgg",
                Created = DateTime.Now,
                Kind = AgreementKind.Agree,
                Creator = "sfssd",
                VersionId = 222,
                Formats = new EntitySet<AgreementFormat>()
                {
                    new AgreementFormat() { Id = 10, Name = "telefon" },
                    new AgreementFormat() { Id = 15, Name = "skan" }
                },
                Versions = new EntitySet<AgreementVersion>()
                {
                    new AgreementVersion()
                    {
                        Id = 20,
                        VersionNumber = 2,
                        Texts = new EntitySet<AgreementText>()
                        {
                            new AgreementText()
                            {
                                VersionId = 2,
                                Language = "en",
                                Content = "dfsdgdsdfsgfd"
                            },
                            new AgreementText()
                            {
                                VersionId = 2,
                                Language = "pl",
                                Content = "telefon"
                            }
                        }
                    },
                    new AgreementVersion()
                    {
                        Id = 25,
                        VersionNumber = 5,
                        Texts = new EntitySet<AgreementText>()
                        {
                            new AgreementText()
                            {
                                VersionId = 5,
                                Language = "en",
                                Content = "dfsdgdsdfsgfd"
                            },
                            new AgreementText()
                            {
                                VersionId = 5,
                                Language = "pl",
                                Content = "telefon"
                            }
                        }
                    }
                }
            };

            var variety6 = new Variety<Agreement>(profile);

            variety6.MapDevisor();

            var sleeve = new Sleeve<Agreement>();
            var variety0 = new Variety<UserProfile>();
            var variety1 = new Variety<User>();

            var user = new User()
            {
                Email = "jflskdfjlkdj",
                FirstName = "hfdjkfsjdkh",
                LastName = "fdlfhjdsk",
                OperationDate = DateTime.Now
            };

            user.Sign();

            var variety3 = new Variety<User>(user);

            var userprofile = new UserProfile()
            {
                Email = "jflskdfjlkdj",
                Name = "hfdjkfsjdkh",
                Surname = "fdlfhjdsk",
                Created = DateTime.Now
            };

            userprofile.Sign();

            var variety5 = new Variety<UserProfile>(userprofile);

            var _sleeve0 = sleeve.Combine(profile);
            var _sleeve1 = sleeve.Combine(_sleeve0);

            var mock = new Agreement()
            {
                Comments = "fdsfsf",
                Created = DateTime.Now,
                Kind = AgreementKind.Cancellation,
                Creator = "fsds",
                VersionId = 992
            };

            var prop = sleeve.Rubrics;

            List<IVariety> list = new();
            for (int i = 0; i < 300000; i++)
            {
                var variety2 = new Variety<Agreement>();

                variety2.ValueArray = _sleeve0.ValueArray;

                list.Add(variety2);
            }

            variety0.Entry.ChipNumber = 1005L;
            variety0.Entry.Name = "nnnnnnn";
            variety0.Entry.SSOID = new Guid();
            variety0.UniqueKey = 93939393UL;

            variety0.Patch(variety1);
            variety0.Patch(profile);
            variety1.Put(_sleeve0);

            variety3.Patch(variety6);

            var c = variety5.Preset;
            c.City = "dfdfhdsdh";
            c.FacebookId = "dfklsdfk";

            variety5.MapDevisor();

            ((Agreement)_sleeve0).TypeId = 1005L;
            var uk = _sleeve0.UniqueKey;
            var k2 = _sleeve0["UniqueKey"];
            _sleeve0.UniqueKey = 2500UL;

            _sleeve0["Id"] = 1005L;
            _sleeve0["TypeId"] = 1005L;
            object o = _sleeve1[5];

            profile.Time = DateTime.Now;

            var serial = new Uscn(profile.UniqueCode);
        }
    }
}
