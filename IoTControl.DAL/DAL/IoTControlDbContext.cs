using IoTControl.Models;
using System;
using System.Data.Entity;
using IoTControl.Common.Mapping;

namespace IoTControl.Common.DAL
{
    public class IoTControlDbContext : DbContext, IDisposable
    {
        static IoTControlDbContext()
        {
            Database.SetInitializer<IoTControlDbContext>(null);
        }

        public IoTControlDbContext() : base("Name=DefaultConnection")
        {
            Configuration.LazyLoadingEnabled = false;
            Database.SetInitializer<IoTControlDbContext>(null);
        }
        
        public IDbSet<AspNetRole> AspNetRoles { get; set; }
        public IDbSet<AspNetUserClaim> AspNetUserClaims { get; set; }
        public IDbSet<AspNetUserLogin> AspNetUserLogins { get; set; }
        public IDbSet<AspNetUserRole> AspNetUserRoles { get; set; }
        public IDbSet<AspNetUser> AspNetUsers { get; set; }
        public IDbSet<UserLifxAccessToken> UserLifxAccessTokens { get; set; }
        public IDbSet<UserLifxFavorite> UserLifxFavorites { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new AspNetRoleMap());
            modelBuilder.Configurations.Add(new AspNetUserClaimMap());
            modelBuilder.Configurations.Add(new AspNetUserLoginMap());
            modelBuilder.Configurations.Add(new AspNetUserRoleMap());
            modelBuilder.Configurations.Add(new AspNetUsersMap());

            modelBuilder.Configurations.Add(new UserLifxAccessTokenMap());
            modelBuilder.Configurations.Add(new UserLifxFavoriteMap());
        }
    }
}
