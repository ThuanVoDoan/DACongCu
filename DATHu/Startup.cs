using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(DATHu.Startup))]
namespace DATHu
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
