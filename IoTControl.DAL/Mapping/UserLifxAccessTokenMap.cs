using System.Data.Entity.ModelConfiguration;
using IoTControl.Models;

namespace IoTControl.Common.Mapping
{
    public class UserLifxAccessTokenMap : EntityTypeConfiguration<UserLifxAccessToken>
    {
        public UserLifxAccessTokenMap()
        {
            this.ToTable("UserLifxAccessTokens", "dbo");

            this.HasKey(a => a.Id);
        }
    }
}
