using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(StoryBoard.Startup))]
[assembly: log4net.Config.XmlConfigurator(ConfigFile = "Web.config", Watch = true)]

namespace StoryBoard
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
