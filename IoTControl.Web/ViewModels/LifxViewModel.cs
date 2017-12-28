using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;

namespace IoTControl.Web.ViewModels
{
    public class LifxViewModel
    {
        public class FavoriteEditor : IoTControl.Models.UserLifxFavorite
        {
            [NotMapped]
            public List<SelectListItem> SelectorTypeList { get; set; }
        }
    }
}