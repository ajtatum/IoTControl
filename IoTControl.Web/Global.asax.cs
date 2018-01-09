using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Autofac;
using Autofac.Integration.Mvc;
using IoTControl.Common.DAL;
using IoTControl.Web.Classes.Lighting;
using IoTControl.Web.Classes.LIFX;
using IoTControl.Web.ViewModels;

namespace IoTControl.Web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            var builder = new ContainerBuilder();
            builder.RegisterControllers(typeof(MvcApplication).Assembly);
            builder.RegisterModule<AutofacWebTypesModule>();
            builder.RegisterType<IoTControlDbContext>().InstancePerRequest();

            var container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));

            AutoMapper.Mapper.Initialize(config =>
            {
                config.CreateMap<IoTControl.Models.UserLifxFavorite, LifxViewModel.FavoriteEditor>();

                config.CreateMap<LifxViewModel.LifxFavoriteJson, LifxColor>()
                    .ForMember(x => x.Hue, y => y.MapFrom(z => z.Hue))
                    .ForMember(x => x.Saturation, y => y.MapFrom(z => z.Saturation))
                    .ForMember(x => x.Brightness, y => y.MapFrom(z => z.Brightness))
                    .ForMember(x => x.Kelvin, y => y.MapFrom(z => z.Kelvin))
                    .ForMember(x => x.Name, y => y.Ignore())
                    .ForMember(x => x.Hex, y => y.Ignore())
                    .ForMember(x => x.RGB, y => y.Ignore());
            });

            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
    }
}
