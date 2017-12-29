using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Autofac;
using Autofac.Integration.Mvc;
using IoTControl.Common.DAL;
using IoTControl.Web.Classes.Lighting;
using IoTControl.Web.Classes.LIFX;

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
                config.CreateMap<IoTControl.Models.UserLifxFavorite, IoTControl.Web.ViewModels.LifxViewModel.FavoriteEditor>()
                    .ForMember(x => x.SelectorTypeList, y => y.Ignore());

                config.CreateMap<HsvColor, LifxColor>()
                    .ForMember(x => x.Hue, y => y.MapFrom(z => z.H))
                    .ForMember(x => x.Saturation, y => y.MapFrom(z => z.S))
                    .ForMember(x => x.Brightness, y => y.MapFrom(z => z.V))
                    .ForMember(x => x.Name, y => y.Ignore())
                    .ForMember(x => x.Kelvin, y => y.Ignore())
                    .ForMember(x => x.Hex, y => y.Ignore())
                    .ForMember(x => x.RGB, y => y.Ignore())
                    .ForSourceMember(x => x.A, y => y.Ignore());

            });

            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
    }
}
