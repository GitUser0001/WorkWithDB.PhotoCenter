using Microsoft.Owin;
using Owin;
using WorkWithDB.DAL.Abstract;
using WorkWithDB.DAL.PostgreSQL;

[assembly: OwinStartupAttribute(typeof(WebApp.Startup))]
namespace WebApp
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            UnitOfWorkFactory.__Initialize(() => new UnitOfWork());
            ConfigureAuth(app);
        }
    }
}
