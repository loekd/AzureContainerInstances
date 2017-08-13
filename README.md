# AzureContainerInstances

Demo for Azure Container Instances with multiple containers cooperating

## Web Server `JobGenerator`
ASP.NET MVC Web API project that enqueues work. 

## Job Processing `JobProcessing`
.NET Console project that dequeues work and 'processes' it. 

## Web Server `Logging`
ASP.NET MVC Web API project collects logging information. 
