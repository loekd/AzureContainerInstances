﻿using System;
using AzureContainerInstances.Logging.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using AzureContainerInstances.Logging.Models;


namespace AzureContainerInstances.Logging.Controllers
{
	[RoutePrefix("api/logs")]
	public class LogController : ApiController
	{
		private readonly IMessageLogger _messageLogger;

		public LogController(IMessageLogger messageLogger = null)
		{
			_messageLogger = messageLogger ?? new InMemoryMessageLogger();
		}

		// GET api/log
		[HttpGet]
		[Route("")]
		public IEnumerable<string> Get()
		{
			var allMessages = _messageLogger.GetAllMessages().ToList();
			if (allMessages.Count == 0)
				allMessages = new List<string> { "There are no messages in the system yet." };

			return  allMessages;
		}

		// POST api/log
		[HttpPost]
		[Route("")]
		public void Post([FromBody]LogMessage message)
		{
			if (message == null || string.IsNullOrWhiteSpace(message.Message))
				throw new Exception($"Value for {nameof(message)} is missing.");

			_messageLogger.Log(message.Message);
		}
	}
}
