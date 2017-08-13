using System;
using System.Web.Http;
using AzureContainerInstances.Logging.Logging;
using Microsoft.Practices.Unity;

namespace AzureContainerInstances.Logging
{
	public static class WebApiConfig
	{
		private static readonly Lazy<UnityContainer> SingletonContainer = new
			Lazy<UnityContainer>(() =>
			{
				var container = new UnityContainer();
				container.RegisterInstance<IMessageLogger>(new InMemoryMessageLogger());
				return container;
			});


		public static void Register(HttpConfiguration config)
		{
			// Web API configuration and services
			config.DependencyResolver = new UnityResolver(SingletonContainer.Value);
			
			// Web API routes
			config.MapHttpAttributeRoutes();
		}
	}
}
