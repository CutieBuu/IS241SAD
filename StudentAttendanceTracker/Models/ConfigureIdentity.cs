//C# and Razor code written by Zaid Abuisba
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace StudentAttendanceTracker.Models
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
            string firstName = "Josh";
            string lastName = "Cobus";
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
                new User { UserName = "RMiller@my.stlcc.edu", FirstName = "Robert", LastName = "Miller" },
                new User { UserName = "RLogan@my.stlcc.edu", FirstName = "Randy", LastName = "Logan" },
                new User { UserName = "EHoude@my.stlcc.edu", FirstName = "Elise", LastName = "Houde" },
                new User { UserName = "CPeterson@my.stlcc.edu", FirstName = "Christine", LastName = "Peterson" },
                new User { UserName = "TSharp@my.stlcc.edu", FirstName = "Tricia", LastName = "Sharp" },
                new User { UserName = "LFlynn@my.stlcc.edu", FirstName = "Lena", LastName = "Flynn" },
                new User { UserName = "JLau@my.stlcc.edu", FirstName = "Jas", LastName = "Lau" },
                new User { UserName = "FLaflamme@my.stlcc.edu", FirstName = "Fatima", LastName = "Laflamme" },
                new User { UserName = "VLapierre@my.stlcc.edu", FirstName = "Vladimir", LastName = "Lapierre" },
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
                new User { UserName = "RJohnson@stlcc.edu", FirstName = "Robert", LastName = "Johnson" },
                new User { UserName = "JSmith@stlcc.edu", FirstName = "John", LastName = "Smith" },
                new User { UserName = "AParker@stlcc.edu", FirstName = "Amy", LastName = "Parker" },
                new User { UserName = "JPhillips@stlcc.edu", FirstName = "Jennifer", LastName = "Phillips" },
                new User { UserName = "BCollins@stlcc.edu", FirstName = "Benjamin", LastName = "Collins" },
                new User { UserName = "REdwards@stlcc.edu", FirstName = "Rachel", LastName = "Edwards" },
                new User { UserName = "TEvans@stlcc.edu", FirstName = "Thomas", LastName = "Evans" },
                new User { UserName = "JCampbell@stlcc.edu", FirstName = "Joeseph", LastName = "Campbell" },
                new User { UserName = "SPhillips@stlcc.edu", FirstName = "Samuel", LastName = "Phillips" },
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
            string username = "MTurner@stlcc.edu";
            string password = "MT#2023";
            string roleName = "QualifiedStaff";

            // if role doesn't exist, create it
            if (await roleManager.FindByNameAsync(roleName) == null)
            {
                await roleManager.CreateAsync(new IdentityRole(roleName));
            }
            if (await userManager.FindByNameAsync(username) == null)
            {
                User user = new() { UserName = username, FirstName = "Michael", LastName = "Turner" };
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
