namespace PhotoContest.Data.Migrations
{
    #region

    using System;
    using System.Data.Entity.Migrations;

    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;

    using PhotoContest.Models.Models;

    #endregion

    public sealed class Configuration : DbMigrationsConfiguration<PhotoContestDbContext>
    {
        public Configuration()
        {
            this.AutomaticMigrationsEnabled = true;
            this.AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(PhotoContestDbContext context)
        {

            var roleManager = new RoleManager<Microsoft.AspNet.Identity.EntityFramework.IdentityRole>(new RoleStore<IdentityRole>(context));
            if (!roleManager.RoleExists("Admin"))
            {
                var role = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole();
                role.Name = "Admin";
                roleManager.Create(role);
            }

            var userManager = new UserManager<User>(new UserStore<User>(context));
            if (userManager.FindByName("mar0der") == null)
            {
                //creating mar0der admin with password 123
                var user = new User()
                               {
                                   UserName = "mar0der",
                                   Email = "mar0der@gmail.com",
                                   PasswordHash = "ADDqeu799LPu2MFv/G9l9Dc3W5aM60JfeYUQx8JzZIkXL+IJ0SVahuH+m6/3efWFqw==" //pass is 123
                               };
                if (userManager.Create(user).Succeeded)
                {
                    userManager.AddToRole(user.Id, "Admin");
                }

                //creating pesho regular user with password 123
                user = new User()
                {
                    UserName = "pesho",
                    Email = "pesho@gmail.com",
                    PasswordHash = "ADDqeu799LPu2MFv/G9l9Dc3W5aM60JfeYUQx8JzZIkXL+IJ0SVahuH+m6/3efWFqw==" //pass is 123
                };

                userManager.Create(user);
            }
        }

    }
}