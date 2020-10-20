# Azure Application Insights Hackfest

In this 1 day Hackfest, you will be able to deploy a fully working ASP .NET Core web application for a ficticious e-commerce shop, including Application Insights features to provide telemetry and monitoring metrics. The topics covered in this hackfests are the following:

1. Introduction to Azure Application Insights
2. Configuration and Setup of Application Insights for .NET Applications
3. Working with the application map and adding new components for autodiscovery
4. Custom Telemetry logging
5. Querying raw data and custom queries from App Insights data
6. Checking live Metrics under stress load and exceptions
7. Using PowerBI to generate custom dashboards
8. Using Snapshot debugging and Visual Studio tooling for App Insights

## Pre-requisites

1. Install Visual Studio Enterprise 2018 or above. If you dont have a VS Studio Enterprise licence you can use the free trial from https://visualstudio.microsoft.com/thank-you-downloading-visual-studio/?sku=Enterprise&rel=15 . 

2. Make sure that during the VS Studio Enterprise installation you check the Web Development tools and the Azure Development tools options. This will install all the required SDKs and tools we will be using during this hackfest.

![alt text](https://github.com/kincho-guerrero/hack-appinsights/blob/master/Images/app-insights-hack-1.png "SDKS and Tooling")

3. To use Application Insights you will be needing an Azure Subscription. In case you don't have one, you can activate your free Azure Trial for 30 days or 200$ USD https://azure.microsoft.com/en-us/free/

![alt text](https://github.com/kincho-guerrero/hack-appinsights/blob/master/Images/app-insights-hack-2.png "Azure Free Subscription")

## Seting up your E-Shop Solution

For this hack we will be using an existing pre-built app to ensure we focus in the app monitoring and telemetry rather than coding the whole app. The app will provide us with a fully functional ficticious environment to analyze and gather telemetry and insights. For this you will be needing to configure additional services in Azure, those services are detailed in the following steps:

## 1. Setting up you storage account for the assets

1. Login to your Azure account through the portal or through CLI

2. In case you logged in using the Azure Portal, initialize an Azure CLI clicking on the console icon at the top navigation bar.

3. Once you have an Azure CLI logged in with your account, proceed to create a resource group. We will be using this resource group to encapsulate all the resources needed in this hackfest.

```shell
az group create --name appinsights-hack --location southcentralus
```

4. Once you have the resource group you will need to create an storage account, make sure that you use an unique name by adding your alias to the storage account name (for example appinsightslruval )

```shell
az storage account create --name appinsights{alias} --resource-group appinsights-hack -l southcentralus --sku Standard_LRS
```

5. For storing the application data we will be using an Azure SQL DB. First you will need to create a SQL DB Server. Remember to choose an unique server name  for your instance as they are reserved names (use your alias)

```shell
az sql server create --name appinsights{alias} --resource-group appinsights-hack -l southcentralus -u mydbadmin -p Hackfest2020!
```

6. Then you will need to create the Database.

```shell
az sql db create --name appinsightshackdb --resource-group appinsights-hack -s appinsights{alias} --service-objective Basic
```

7. To access the database from a local SQL Management Studio or for running your app locally before deploying to Azure, you will need to enable the firewall rules for your IP. In this case we will open the server to all the IPs so we don't face any connectivity issues. WARNING: This is not a good case practice for secure environments, be sure to protect your SQL DB with the adequate IPs for your trusted connections. This configuration shouldn't be run in production environments.

```shell
az sql server firewall-rule create --resource-group appinsights-hack -s appinsights{alias} --name myfirewallrule --start-ip-address 0.0.0.0 --end-ip-address 255.255.255.255
```

8. Obtain the SQL DB connection string to replace it in the solution file named AppSettings.json.

```shell
az sql db show-connection-string -s appinsights{alias} -n appinsightshackdb -c ado.net
```

Include your username and password since it will be needed on the following step


9. Clone the solution locally from this git repo located at the Start Folder

9. Open the "eShopOnWeb" solution, search on the "src" folder and within the "Web" project look for the appsettings.json file and replace the connection string for the DB. Both CatalogConnection and IdentityConnection will use the same connection string.

10. Select project "Web" as start up project and run it. Since it is the first time that you run the app after having configured the connection string, the following error will appear:

>     A database operation failed while processing the request.
>     
>     SqlException: Invalid object name 'Catalog'.

*To fix it, click on "Apply Migrations". And wait for the message "Try refreshing the page" before hitting F5*

11. Within the Web App, click on the Login Button and select the option to register a new user, fill in the form for the 
 - Email
 - Password
 - Confirm password

Since we will be trying to register a new user to the database and we don't have the tables already registered, the folowing error will ocurr:

>    A database operation failed while processing the request.
>   SqlException: Invalid object name 'AspNetUsers'.

To fix the problem, just click on "Apply Migrations" and wait for the confirmation message to appear before hitting F5 (and confirm the operation to complete the initial registration of your user)

12. Test the application by adding some items to your basked and proceed to check out.

13. Publish your application to an Azure App Service Web App, just click on the Web project and select "Publish"

 - Select App Service and choose "Select new"
 - Click on "Create profile"
 - Provide a name to your App (must be unique), for example appinsightsweb{alias}
 - Select your subscription
 - Select the working resource group (appinsights-hack)
 - Create a new hosting plan, change the location to SouthCentralUS (or the closest one to you), optionally you can change the size to Free
 - For now select "None" on Application Insights
 - Click on create
 - Finally click on publish

Browse your web application and validate that it is working properly (log in, add something to your cart and checkout)

## Configuration and Setup of Application Insights for .NET Applications

1. Create an Application Insights workspace. From the Azure portal, click on create a new resource, then within the Management Tools category select "Application Insights" and complete the following:
 - Name: Provide a name to your Application Insights (for example appinsights-hack)
 - Application Type: Select "ASP.NET web application"
 - Subscription: Select your subscription
 - Resource Group: Select your working resource group (appinsights-hack)

	Click on create

2. Within the eShopOnWeb solution on VisualStudio, select the Web project, rigth clic and selecc Add > Application Insigths Telemetry

3. On the wizard, click on "Get Started" at the bottom

4. Select your subscription and on the resouce list, search for the application insights that you have just created and click on Register

5. Wait for the wizard to complete the registration

6. Update the App Insights nuget packet. Rigth click on the Web Solution and select "Manage Nuget  Packages" and go to the installed ones, search for "Microsoft.ApplicationInsights.AspNetCore and click on update to version 2.6.1

7. Republish your web app to reflect the changes on your App Service web app

8. Run your app locally and within Visual Studio click on "Application Insights" button on the upper menu.


## Getting session and user telemetry

Next step is to gather the details of the user and ther sessions as they user your web application. You can get the details about this feature on the following article: https://docs.microsoft.com/en-us/azure/azure-monitor/app/usage-send-user-context

**Explore the Analytics part of Application Insights by going to the request talbe and checking that there is currently no info about the users**

To accelerate the execution of this hackfest and since we are using an ASP .NET Core App, there is already the code that handles the sending of users IDs. You have to complete the following:

1. Uncomment the class WebSessionTelemetryInitializer within the Web project
2. Go to the Startup class within the Web project and uncomment line 102, this will register the class that you have previously uncomment. You should also fix the compilation error by adding the proper USING (using Microsoft.ApplicationInsights.Extensibility)
3. Republish your app and run it locally, login using an existing account and browse some web pages.
4. Explore the requests table within Analytics on your Application Insigths and check that the user ids are now being added to the records 

Now that we are able to see the users Ids, we are going to check the Usage section within Application Insights.

 1. Explore the Users reports within the Usage section
 2. Check the documentation about Funnels: https://docs.microsoft.com/en-us/azure/azure-monitor/app/usage-funnels 
 3. Create a funnel that shows users that have login and Checked their Profile
 4. Create a funnel that shows users that have login and place an order
 5. Explore the user flows to understand how your users navigate within your web app, see what happens before "Checkout Complete - Microsoft.eShopOnweb"

## Custom Telemetry Logging

We are going to track the event of the placement of an order, to do that, please check the following article:  https://docs.microsoft.com/en-us/azure/azure-monitor/app/api-custom-events-metrics#trackevent

The followin steps will guide you on the process to record the events of checkout

1. Within the Web Project, open the file Pages\Basket\Checkout.cshtml.cs and locate the method OnPost, uncomment the section that start on line 51 that will record the event "Orders", to get this working you have to get your instrumentation key of your Application Insights, this info is on the overview tab, put a breakpoint on telemetry.TrackEvent("Orders")
2. Execute your web app, login and execute a checkout, make sure that you hit the breakpoint and then validate that this info was registered within .
3. Add more customs events, for example when checking the profile of an user within the webpage

## Application Map 

Go to Applicaton Map and see the components that have been mapped to your application. You can see requests both from your local PC and from the web app on Azure App Service. Next task is to upload the /images folder to an azure storage blob and make a request within your web app to see how this storage component is automatically placed on your application Map

1. Go to the storage account that you have created on the prerequisites and upload the images located on the folder /Start/Assets/images/products to a new container named products

2. On the Web project, go to the file Services\CatalogService and uncomment lines 60 to 70 and comment lines 55 to 58.

3. Provide your storage name and connnection string on line 64, put a breakpoint on line 68 and 69 and validate that the request to the storage account is working properly. Then navigate to your app and use it normally, (add items and ckeckout)

4. Go back to the Application map and validate how this storage account dependency was automatically added.

## Querying raw data and custom queries from App Insights data

Now that we have users id registered and we already registered the custom event of placing an order, we are going to explore the data on tables using queries, the first example will show how many Orders have been placed on the last 2 days and we will select only the first 10 results.

    customEvents
    | where timestamp > ago(1d)
    | where name == "Orders"
    | take  10

Take this as an example and write more queries on other tables to include the filtering by user ID

## Live Metrics under stress load and exceptions

We are going to see how the live metrics behave under load, create a new powershell script and paste the following content and replace your actual web app endpoint

for ($i = 0 ; $i -lt 250; $i++)
{
 Invoke-WebRequest -uri https://monitoringhack.azurewebsites.net/
}

1. Execute the PowerShell script and go to Live Metrics.

2. We are going to cause some execptions by logging into SQL Server Managment Studio and changing the names of the tables, see what is being reported on Live Metrics and on the Analytics section of your Application Insights.
