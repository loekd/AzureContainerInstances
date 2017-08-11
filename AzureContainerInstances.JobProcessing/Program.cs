using System;
using System.Configuration;
using Microsoft.ServiceBus;
using Microsoft.ServiceBus.Messaging;

namespace AzureContainerInstances.JobProcessing
{
	internal static class Program
	{
		private const string MicrosoftServicebusConnectionStringSettingName = "Microsoft.ServiceBus.ConnectionString";
		private const string QueueName = "TestQueue";
		private static readonly int ProcessId = System.Diagnostics.Process.GetCurrentProcess().Id;
		private static string _connectionStringForReading;

		private static void Main(string[] args)
		{
			if (args != null && args.Length > 0)
			{
				_connectionStringForReading = args[0];
			}
			if (string.IsNullOrWhiteSpace(_connectionStringForReading))
			{
				_connectionStringForReading = Environment.GetEnvironmentVariable(MicrosoftServicebusConnectionStringSettingName);
			}
			if (string.IsNullOrWhiteSpace(_connectionStringForReading))
			{
				_connectionStringForReading = ConfigurationManager.AppSettings[MicrosoftServicebusConnectionStringSettingName];
			}
			try
			{
				if (string.IsNullOrWhiteSpace(_connectionStringForReading))
				{
					throw new ConfigurationErrorsException($"Provide an Commandline argument, AppSetting or Environment Variable named '{MicrosoftServicebusConnectionStringSettingName}' that holds a connection string that has read access to your Azure Service Bus.");
				}
				LogMessage("Job Processing is starting.");
				EnsureQueueExists();
				ProcessQueueMessages();
				LogMessage("Job Processing is started.");
				LogMessage("Press <enter> to exit.");
				Console.ReadLine();
			}
			catch (Exception ex)
			{
				LogMessage(ex.ToString());
				Console.Error.WriteLine(ex);
			}
			finally
			{
				LogMessage("Job Processing is stopped.");
			}
		}

		private static void EnsureQueueExists()
		{
			var namespaceManager = NamespaceManager.CreateFromConnectionString(_connectionStringForReading);

			if (!namespaceManager.QueueExists(QueueName))
			{
				LogMessage($"Service bus queue {QueueName} does not exist. Attempting to create it using the provided connection.");
				try
				{
					namespaceManager.CreateQueue(QueueName);
				}
				catch
				{
					LogMessage("Failed to create queue.");
					throw;
				}
			}
			else
			{
				LogMessage($"Service bus queue {QueueName} exists!");
			}
		}

		private static void ProcessQueueMessages()
		{
			var client = QueueClient.CreateFromConnectionString(_connectionStringForReading, QueueName, ReceiveMode.ReceiveAndDelete);

			client.OnMessage(message =>
			{
				LogMessage($"Message processed: {message.GetBody<string>()}");
			});
		}

		private static void LogMessage(string messsage)
		{
			Console.WriteLine($"{ProcessId:D5} - {DateTime.UtcNow:O} - {messsage}");
		}
	}
}
