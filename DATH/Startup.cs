using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(DATH.Startup))]
namespace DATH
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            //ConfigureAuth(app);
        }
    }
}
