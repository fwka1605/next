using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
using SimpleInjector;
using SimpleInjector.Lifestyles;
using SimpleInjector.Integration.Wcf;

namespace Rac.VOne.Web.Service
{
    public class Global : System.Web.HttpApplication
    {
        protected void Application_Start(object sender, EventArgs e)
        {
            InitializeSimpleInjectorSettings();
        }

        private void InitializeSimpleInjectorSettings()
        {
            var container = new SimpleInjector.Container();
            //container.Options.DefaultScopedLifestyle = new SimpleInjector.Integration.Wcf.WcfOperationLifestyle();
            container.Options.DefaultScopedLifestyle = new AsyncScopedLifestyle();

            container.RegisterWcfServices(System.Reflection.Assembly.GetExecutingAssembly());

            new SimpleInjectorConfigurator(container).Configure();
            new SimpleInjectorWebCommonProcessorConfigurator(container).Configure();
            new SimpleInjectorQueryProcessorConfigurator(container).Configure();
            SimpleInjectorServiceHostFactory.SetContainer(container);
        }

        protected void Session_Start(object sender, EventArgs e)
        {
            // https://stackoverflow.com/a/1966562
            // avoid : Session state has created a session id, but cannot save it
            //         because the response was already flushed by the applicatoin.
            var sessionId = Session.SessionID;
            System.Diagnostics.Debug.Assert(!string.IsNullOrEmpty(sessionId));
        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {

        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected void Application_Error(object sender, EventArgs e)
        {

        }

        protected void Session_End(object sender, EventArgs e)
        {

        }

        protected void Application_End(object sender, EventArgs e)
        {

        }
    }
}