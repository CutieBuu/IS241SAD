//C# and Razor Code Written by Zaid Abuisba https://github.com/vgc12
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using StudentAttendanceTracker.Models.DatabaseModels;
using StudentAttendanceTracker.Models.Identity;

namespace StudentAttendanceTracker.Models.Initialization
{
    /// <summary>
    /// This class is primarily for creating users of specific types while starting the program.
    /// Users can either be Admin, Students, Instructors, or Qualified Staff.
    /// This class may be removed on production build.
    /// </summary>
    public class ConfigureIdentity
    {

        /// <summary>
        /// Creates a single admin user
        /// </summary>
        /// <param name="provider">IServiceProvider generated from a ScopeFactory Object in Program.cs</param>

        public static async Task CreateAdminUserAsync(IServiceProvider provider)
        {
            var roleManager = provider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = provider.GetRequiredService<UserManager<User>>();
            string email = "adminjosh@stlcc.edu";
            string firstName = "josh";
            string lastName = "cobus";
            string password = "Sesame";
            string roleName = "Admin";


            // if role doesn't exist, create it
            if (await roleManager.FindByNameAsync(roleName) == null)
            {
                await roleManager.CreateAsync(new IdentityRole(roleName));
            }
            // if username doesn't exist, create it and add to role
            if (await userManager.FindByNameAsync(email) == null)
            {
                User user = new() { UserName = email, FirstName = "Admin", LastName = "Admin" };
                var result = await userManager.CreateAsync(user, password);
                if (result.Succeeded)
                {

                    await userManager.AddToRoleAsync(user, roleName);
                    var context = new AttendanceTrackerContext(new DbContextOptions<AttendanceTrackerContext>());

                    await context.Admins.AddAsync(new() { AdministratorEmail = email, FirstName = firstName, LastName = lastName, UserId = user.Id });

                    await context.SaveChangesAsync();

                }
            }
        }

        /// <summary>
        /// Creates many student users.
        /// </summary>
        /// <param name="provider">IServiceProvider generated from a ScopeFactory Object in Program.cs</param>
        public static async Task CreateStudentUserAsync(IServiceProvider provider)
        {
            var roleManager = provider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = provider.GetRequiredService<UserManager<User>>();

            string roleName = "Student";

            List<User> users = new()
            {
                new User { UserName = "rmiller@my.stlcc.edu", FirstName = "robert", LastName = "miller" },
                new User { UserName = "rlogan@my.stlcc.edu", FirstName = "randy", LastName = "logan" },
                new User { UserName = "eeoude@my.stlcc.edu", FirstName = "elise", LastName = "houde" },
                new User { UserName = "cpeterson@my.stlcc.edu", FirstName = "christine", LastName = "peterson" },
                new User { UserName = "tsharp@my.stlcc.edu", FirstName = "tricia", LastName = "sharp" },
                new User { UserName = "lflynn@my.stlcc.edu", FirstName = "lena", LastName = "flynn" },
                new User { UserName = "jlau@my.stlcc.edu", FirstName = "jas", LastName = "lau" },
                new User { UserName = "flaflamme@my.stlcc.edu", FirstName = "fatima", LastName = "laflamme" },
                new User { UserName = "vlapierre@my.stlcc.edu", FirstName = "vladimir", LastName = "lapierre" },
            };
            List<string> passwords = new() { "RM#2023", "RL#2023", "EH#2023", "CP#2023", "TS#2023", "LF#2023", "JL#2023", "FL#2023", "VL#2023" };


            if (await roleManager.FindByNameAsync(roleName) == null)
            {
                await roleManager.CreateAsync(new IdentityRole(roleName));
            }

            var context = new AttendanceTrackerContext(new DbContextOptions<AttendanceTrackerContext>());
            Random rand = new(10);
            for (int i = 0; i < users.Count; i++)
            {

                // if username doesn't exist, create it and add to role
                if (await userManager.FindByNameAsync(users[i].UserName!) is null)
                {
                    var result = await userManager.CreateAsync(users[i], passwords[i]);
                    if (result.Succeeded)
                    {
                        await userManager.AddToRoleAsync(users[i], "Student");

                        List<Course> c = new();

                        foreach (var course in context.Courses)
                        {
                            if (c.Count >= 3)
                                break;
                            int r = rand.Next(context.Courses.Count());
                            if (context.Courses.Find(r) is null || c.Contains(context.Courses.Find(r)!))
                                continue;

                            c.Add(context.Courses.Find(r)!);

                        }


                        Student s = new()
                        {
                            FirstName = users[i].FirstName,
                            LastName = users[i].LastName,
                            StudentEmail = users[i].UserName!,
                            UserId = users[i].Id,
                            Courses = c
                        };


                        if (s.UserId != null)
                        {
                            await context.Students.AddAsync(s);
                            await context.SaveChangesAsync();
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Creates many Instructor users.
        /// </summary>
        /// <param name="provider">IServiceProvider generated from a ScopeFactory Object in Program.cs</param>
        public static async Task CreateInstructorUsersAsync(IServiceProvider provider)
        {
            var roleManager = provider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = provider.GetRequiredService<UserManager<User>>();



            // if role doesn't exist, create it
            if (await roleManager.FindByNameAsync("Instructor") == null)
            {
                await roleManager.CreateAsync(new IdentityRole("Instructor"));
            }


            List<User> users = new()
            {
                new User { UserName = "rjohnson@stlcc.edu", FirstName = "robert", LastName = "johnson" },
                new User { UserName = "jsmith@stlcc.edu", FirstName = "john", LastName = "smith" },
                new User { UserName = "aparker@stlcc.edu", FirstName = "amy", LastName = "parker" },
                new User { UserName = "jphillips@stlcc.edu", FirstName = "jennifer", LastName = "phillips" },
                new User { UserName = "bcollins@stlcc.edu", FirstName = "benjamin", LastName = "collins" },
                new User { UserName = "redwards@stlcc.edu", FirstName = "rachel", LastName = "edwards" },
                new User { UserName = "tevans@stlcc.edu", FirstName = "thomas", LastName = "evans" },
                new User { UserName = "jcampbell@stlcc.edu", FirstName = "joeseph", LastName = "campbell" },
                new User { UserName = "sphillips@stlcc.edu", FirstName = "samuel", LastName = "phillips" },
            };
            List<string> passwords = new() { "RJ#2023", "JS#2023", "AP#2023", "JP#2023", "BC#2023", "RE#2023", "TE#2023", "JC#2023", "SP#2023" };

            var context = new AttendanceTrackerContext(new DbContextOptions<AttendanceTrackerContext>());
            for (int i = 0; i < users.Count; i++)
            {
                if (await userManager.FindByNameAsync(users[i].UserName!) == null)
                {


                    var result = await userManager.CreateAsync(users[i], passwords[i]);
                    if (result.Succeeded)
                    {
                        await userManager.AddToRoleAsync(users[i], "Instructor");
                        await context.Instructors.AddAsync(
                            new Instructor { FirstName = users[i].FirstName, LastName = users[i].LastName, InstructorEmail = users[i].UserName!, UserId = users[i].Id, Courses = { context.Courses.First(x => x.CourseId == i + 1) } }
                            ); ;

                        await context.SaveChangesAsync();
                    }
                }
            }

        }

        /// <summary>
        /// Creates many Qualified Staff users.
        /// </summary>
        /// <param name="provider">IServiceProvider generated from a ScopeFactory Object in Program.cs</param>
        public static async Task CreateQStaffUserAsync(IServiceProvider provider)
        {
            var roleManager = provider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = provider.GetRequiredService<UserManager<User>>();
            string username = "mturner@stlcc.edu";
            string password = "MT#2023";
            string roleName = "QualifiedStaff";

            // if role doesn't exist, create it
            if (await roleManager.FindByNameAsync(roleName) == null)
            {
                await roleManager.CreateAsync(new IdentityRole(roleName));
            }
            if (await userManager.FindByNameAsync(username) == null)
            {
                User user = new() { UserName = username, FirstName = "michael", LastName = "turner" };
                var result = await userManager.CreateAsync(user, password);
                if (result.Succeeded)
                {
                    var context = new AttendanceTrackerContext(new DbContextOptions<AttendanceTrackerContext>());
                    await context.QualifiedStaff.AddAsync(new QualifiedStaff { FirstName = user.FirstName, LastName = user.LastName, QualifiedStaffEmail = user.UserName, UserId = user.Id });
                    await context.SaveChangesAsync();
                    await userManager.AddToRoleAsync(user, roleName);
                }
            }
        }

    }
}
