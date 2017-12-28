using IoTControl.Common.DAL;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using IoTControl.Models;

namespace IoTControl.Web.Services
{
    public class LookupService
    {
        private readonly IoTControlDbContext dbContext;

        public LookupService(IoTControlDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public List<SelectListItem> GetSelectListItems(string selectOption, string category)
        {
            var lookupItems = dbContext.LookupItems
                .Where(x => x.Category == category)
                .OrderBy(x => x.Text)
                .ToList();

            var selectListItems = new List<SelectListItem>
            {
                new SelectListItem
                {
                    Text = selectOption,
                    Value = string.Empty
                }
            };

            selectListItems.AddRange(lookupItems.Select(x =>
                new SelectListItem
                {
                    Text = x.Text,
                    Value = x.Value
                }));

            return selectListItems;
        }
    }
}