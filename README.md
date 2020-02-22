# DevOpsAcademyTasks
This is a demo application for DevOps Academy for task list creation with ability to share between user groups.


# Case Study information

## Repository content

### Static Progressive Web Application
- folder StaticProgressiveWebApp

This folder contains single page progressive Web Application build with vue.js, axios.js, bluebird.js, bootstrap, etc.

- required configuration
- - replace URLs in appConfig variable 
- - set the proper authentication provider in appConfig variable

- delivery
- - because the site is static you can deliver it in Static Website hosting in Azure Storage Account 
- - optional CDN availability

### Serverless Azure Function Application
- folder AzureFuncService

This folder contains .Net Core 3.0 solution for Azure Functions 

- required configuration
- - The application uses Azure Function, so it need to use Function service and related storage account.
- - The data is stored in Azure Table Storage (with two tables)
- - Sharing task between groups can be done by using Azure Service Bus Queue (to be provided by client)

- delivery
Upon delivery you shall configure:
- - connection strings for Azure Table Storage and Azure Service Bus
- - CQRS to allow processing requests from the Static Web Site above
- - Authentication with chosen provider with configured return urls.