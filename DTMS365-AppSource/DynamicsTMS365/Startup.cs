using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(DynamicsTMS365.Startup))]
namespace DynamicsTMS365
{
    public partial class Startup {
        public void Configuration(IAppBuilder app) {
            //ConfigureAuth(app);
        }
    }
}
