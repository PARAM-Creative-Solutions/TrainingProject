using Microsoft.Owin;
using Owin;
using System.Threading;

[assembly: OwinStartupAttribute(typeof(TrainingProject.Startup))]
namespace TrainingProject
{
    //public partial class Startup
    //{
    //    public void Configuration(IAppBuilder app)
    //    {
    //        ConfigureAuth(app);
    //    }
    //}

    /// <summary>
    /// Maps the signal R
    /// </summary>
    public partial class Startup
    {
        #region Method

        /// <summary>
        /// Configures the app
        /// </summary>
        /// <param name="app">Interface of Appbuilder</param>
        public void Configuration(IAppBuilder app)
        {
            app.MapSignalR();
            ThreadPool.SetMinThreads(7, 7);
        }

        #endregion
    }
}
