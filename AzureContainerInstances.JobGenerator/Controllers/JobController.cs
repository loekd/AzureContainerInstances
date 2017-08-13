using System;
using System.Configuration;
using System.Threading.Tasks;
using System.Web.Http;
using AzureContainerInstances.JobGenerator.Models;
using Microsoft.ServiceBus.Messaging;

namespace AzureContainerInstances.JobGenerator.Controllers
{
	[RoutePrefix("api/jobs")]
	public class JobController : ApiController
	{
		private const string MicrosoftServicebusConnectionStringSettingName = "Microsoft.ServiceBus.ConnectionString";
		private const string QueueName = "TestQueue";
		private readonly string _connectionStringForWriting;

		public JobController()
		{
			_connectionStringForWriting = Environment.GetEnvironmentVariable(MicrosoftServicebusConnectionStringSettingName);
			if (string.IsNullOrWhiteSpace(_connectionStringForWriting))
			{
				_connectionStringForWriting = ConfigurationManager.AppSettings[MicrosoftServicebusConnectionStringSettingName];
			}
		}

		// GET api/job/?jobDescription={some value}
		[HttpGet]
		[Route("{jobDescription}")]
		public async Task<IHttpActionResult> Get(string jobDescription)
		{
			if (string.IsNullOrWhiteSpace(_connectionStringForWriting))
			{
				throw new ConfigurationErrorsException($"Provide an AppSetting or Environment Variable named '{MicrosoftServicebusConnectionStringSettingName}' that holds a connection string that has write access to your Azure Service Bus.");
			}

			if (string.IsNullOrWhiteSpace(jobDescription))
			{
				return BadRequest($"A value for '{jobDescription}' is required.");
			}

			await SendMessageAsync(jobDescription);

			return Ok(jobDescription);
		}

		// POST api/job
		[HttpPost]
		[Route("")]
		public async Task Post([FromBody]JobMessage job)
		{
			if (job == null || string.IsNullOrWhiteSpace(job.JobDescription))
				throw new Exception($"Value for {nameof(job)} is missing.");

			if (string.IsNullOrWhiteSpace(_connectionStringForWriting))
			{
				throw new ConfigurationErrorsException($"Provide an AppSetting or Environment Variable named '{MicrosoftServicebusConnectionStringSettingName}' that holds a connection string that has write access to your Azure Service Bus.");
			}

			await SendMessageAsync(job.JobDescription);
		}

		private async Task SendMessageAsync(string jobDescription)
		{
			var client = QueueClient.CreateFromConnectionString(_connectionStringForWriting, QueueName);
			var message = new BrokeredMessage(jobDescription);
			await client.SendAsync(message).ConfigureAwait(true);
		}
	}
}
