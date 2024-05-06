using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using WebDating.Entities.ProfileEntities;
using WebDating.Entities.UserEntities;

namespace WebDating.Data
{
    public class Seed
    {
        public static async Task SeedUsers(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager)
        {
            if (await userManager.Users.AnyAsync()) return;

            var userData = await File.ReadAllTextAsync("Data/UserSeedData.json");
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            var users = JsonSerializer.Deserialize<List<AppUser>>(userData, options);

            var roles = new List<AppRole>
            {
                new AppRole{Name = "Member"},
                new AppRole{Name = "Admin"},
                new AppRole{Name = "Moderator"},
            };

            foreach (var role in roles)
            {
                await roleManager.CreateAsync(role);
            }

            foreach (var user in users)
            {
                user.UserName = user.UserName.ToLower();
                await userManager.CreateAsync(user, "Pa$$w0rd");
                await userManager.AddToRoleAsync(user, "Member");

            }

            var admin = new AppUser()
            {
                UserName = "admin",
            };
            await userManager.CreateAsync(admin, "Pa$$w0rd");
            await userManager.AddToRolesAsync(admin, new[] { "Admin", "Moderator" });

        }

        static Random _rd = new Random();
        public static void SeedData(DataContext context)
        {
            int numOfUsers = 100;

            Gender[] gendersUser = new Gender[] { Gender.Male, Gender.Female };
            Gender[] gendersToDate = new Gender[] { Gender.EveryOne, Gender.Male, Gender.Female };
            int[] height = Enum.GetValues(typeof(Height)).Cast<int>().ToArray();
            int[] provinces = Enum.GetValues(typeof(Provice)).Cast<int>().ToArray();
            int[] interestes = Enum.GetValues(typeof(Interest)).Cast<int>().ToArray();

            for (int i = 12; i < numOfUsers; i++)
            {
                Gender gender = gendersUser[_rd.Next(0, gendersUser.Length) % gendersUser.Length];
                Gender lookingFor = gender == Gender.Male ? Gender.Female : Gender.Female;

                int year = _rd.Next(1980, 2007);
                int month = _rd.Next(1, 13);
                int day = _rd.Next(1, 29);
                AppUser user = new AppUser()
                {
                    Created = DateTime.Now,
                    DateOfBirth = new DateOnly(year, month, day),
                    UserName = string.Format("user{0}", i),
                    Gender = gender.ToString(),
                    //Id = i,
                };

                DatingProfile datingProfile = new DatingProfile()
                {
                    //Id = i,
                    UserId = i,
                    //User = user,
                    Height = (Height)height[_rd.Next(0, height.Length) % height.Length],
                    WhereToDate = (Provice)provinces[_rd.Next(0, provinces.Length) % provinces.Length],
                    DatingObject = lookingFor,
                };

                List<UserInterest> userInterests = new List<UserInterest>();

                int interestCount = _rd.Next(3, 6);
                for (int j = 0; j < interestCount; j++)
                {
                    UserInterest ui = new UserInterest
                    {
                        DatingProfile = datingProfile,
                        DatingProfileId = i,
                        InterestName = (Interest)interestes[_rd.Next(0, interestes.Length)]
                    };
                    userInterests.Add(ui);
                }
                //datingProfile.UserInterests = userInterests;
                //user.DatingProfile = datingProfile;

                context.Users.Add(user);
                context.DatingProfiles.Add(datingProfile);
                context.UserInterests.AddRange(userInterests);

            }

            context.SaveChanges();
        }
    }
}
