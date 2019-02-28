# Azure Application Insights Hackfest

In this 1 day Hackfest, you will be able to deploy a fully working ASP .NET Core web application for a ficticious e-commerce shop, including Application Insights features to provide telemetry and monitoring metrics. The topics covered in this hackfests are the following:

1. Introduction to Azure Application Insights
2. Configuration and Setup of Application Insights for .NET Applications
3. Working with the application map and adding new components for autodiscovery
4. Custom Telemetry logging
5. Querying raw data and custom queries from App Insights data
6. Using PowerBI to generate custom dashboards
7. Using Snapshot debugging and Visual Studio tooling for App Insights

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
az sql server create --name appinsights{alias} --resource-group appinsights-hack -l southcentralus -u mydbadmin -p Hackfest2019!
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

A database operation failed while processing the request.
SqlException: Invalid object name 'Catalog'.

To fix it, click on "Apply Migrations". And wait for the message "Try refreshing the page" before hitting F5


## Configuration and Setup of Application Insights for .NET Applications



## Getting session and user telemetry



## Custom Telemetry Logging



## Querying raw data and custom queries from App Insights data



## Using PowerBI to generate custom dashboards



## Using Snapshot debugging and Visual Studio tooling for App Insights
