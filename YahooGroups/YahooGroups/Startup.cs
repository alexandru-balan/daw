using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using Owin;
using YahooGroups.Models;

[assembly: OwinStartupAttribute(typeof(YahooGroups.Startup))]
namespace YahooGroups
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);

            createAdminUsersAndAppRoles();
        }

        private void createAdminUsersAndAppRoles ()
        {
            ApplicationDbContext context = new ApplicationDbContext();

            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));

            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));

            // Adaugam rolurile utilizatorilor in aplicatie

            // Rolul de Administrator
            if (!roleManager.RoleExists("admin"))
            {
                // Se adauga rolul de admin
                var role = new IdentityRole();
                role.Name = "admin";
                roleManager.Create(role);

                // Se adauga administratorii aplicatiei
                var admin1 = new ApplicationUser();
                var admin2 = new ApplicationUser();

                admin1.UserName = "alexandru.balan@fictiv.com";
                admin1.Email = "alexandru.balan@fictiv.com";

                admin2.UserName = "horatiu.razvan@fictiv.com";
                admin2.Email = "horatiu.razvan@fictiv.com";

                var created = userManager.Create(admin1, "Admin1!"); // Parola super sigura
                if (created.Succeeded)
                {
                    userManager.AddToRole(admin1.Id, "admin");
                }

                created = userManager.Create(admin2, "Admin2!"); // Parola super sigura
                if (created.Succeeded)
                {
                    userManager.AddToRole(admin2.Id, "admin");
                }
            }

            // Rolul de utilizator inregistrat
            if (!roleManager.RoleExists("user"))
            {
                var role = new IdentityRole();
                role.Name = "user";
                roleManager.Create(role);

                var user = new ApplicationUser();

                user.Email = "user@user.com";
                user.UserName = "user@user.com";

                var created = userManager.Create(user, "User1!");
                if (created.Succeeded)
                {
                    userManager.AddToRole(user.Id, "user");
                }
            }

            // Rolul de moderator
            if (!roleManager.RoleExists("moderator"))
            {
                var role = new IdentityRole();
                role.Name = "moderator";
                roleManager.Create(role);

                var moderator = new ApplicationUser();

                moderator.Email = "moderator@moderator.com";
                moderator.UserName = "moderator@moderator.com";

                var created = userManager.Create(moderator, "Moderator1!");

                if (created.Succeeded)
                {
                    userManager.AddToRole(moderator.Id, "moderator");
                }
            }
        }
    }
}
