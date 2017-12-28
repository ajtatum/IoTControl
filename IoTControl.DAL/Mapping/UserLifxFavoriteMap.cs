using System.Data.Entity.ModelConfiguration;
using IoTControl.Models;

namespace IoTControl.Common.Mapping
{
    public class UserLifxFavoriteMap : EntityTypeConfiguration<UserLifxFavorite>
    {
        public UserLifxFavoriteMap()
        {
            this.ToTable("UserLifxFavorites", "dbo");

            this.HasKey(a => a.Id);
        }
    }
}
