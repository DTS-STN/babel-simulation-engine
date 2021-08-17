# ESDC Simulation Engine

This project is the Simulation Engine component of the Maternity Benefit Policy Difference Engine prototype. 

A Policy Difference Engine (PDE) is a set of tool that measures the impact of a proposed change to a rule (e.g. policy, regulation, legislation, etc.) The idea is to have both sets of rules encoded (the existing rules and proposed changes). A sample population of individuals is run against both sets of rules, generating results that show the impact of the change. The Simulation Engine is linked to a database that stores the sample population, and connects to a Rules Engine, which stores the encoded rules. When a new simulation is requested, the population is fetched from the database. Each member of the population is sent to the Rules Engine along with the existing rule (base case). This calculates their base amount, i.e. their maternity benefits entitlement under the current system. Then the member is sent to the Rules Engine with the proposed change (variant case). The result is their variant amount, i.e. their maternity benefits entitlement under the proposed change. These results are stored in the database, and can be  subsequently retrieved for analysis.


## Overview of projects

This repo is a template for the simulation engine component of the PDE, and it can be run as a web API. The goal is to make this repo act as a generic template for *any* PDE, since PDE's will all have some components and functionality in common. We want to capture those commonalities through interfaces and generics, so that it is very easy to create PDE's for a given rule in the future. In some cases, this will involve a developer implementing pre-defined interfaces, such as those for interacting with the rules and interacting with a database. The repo is composed of several projects:

### esdc-simulation-base
This project is a class library that contains the core code for running a simulation for a PDE. This includes generic handlers, simulation case runners, request/result classes, interfaces for a rules engine, and interfaces for storage systems. It is up to the developer to implement these interfaces to suit their purpose. It doesn't contain code specific to any rule. There is also an associated test project.

### esdc-simulation-api
This is the executable web-api project that exposes the functionality of the other class libraries as a REST API.

### esdc-simulation-classes
This project is a class library containing plain C# objects which are used as request/response objects in the esdc-simulation-api project. These are separated as their own class library so that they can be published as a Nuget Package and used by consuming applications, such as the Data Primer and the Front-end Web Application. 

### maternity-benefits
This is a concrete example of a class library that implements the interfaces specified in the esdc-simulation-base library. This includes code specific to the maternity benefits simulation, as well as implementations for the storage layer, which interacts with the database. There is also an associated test project which includes a Postman collection for the API calls.

## Spec

The OpenAPI Spec for the project can be viewed at `/swagger/v1/swagger.json`, and is the most reliable source for the spec. But the general functionality includes the following:
- Create Persons: This request allows you to send a list of simulated persons in to be stored in the system for use by subsequent simulations. This call is used by the Data Primer to load the simulated persons into the system. The request body is a list of the simulated persons containing the required properties. Note that this call requires a password to be passed in the header.
- Get Persons: This request allows you to view all the persons that will be used in simulations. Note that this call requires a password to be passed in the header.
- Create Simulation: This request kicks off a new simulation and returns the unique GUID of the resulting simulation. The request body must contain the specifications for both the base case and the variant case. This request is used by the Front-end Web App.
- Get Simulation Results: Using a simulations unique GUID, this request fetches the results of running a simulation, including the base/variant cases, as well as the simulated persons, their demographic info, and their base/variant amounts. This request is used by the Front-end Web App.

## Development

### Storage Layer

The Simulation Engine requires the implementation of a storage interface. The storage layer is responsible for storing the simulation persons (data) as well as the simulation results. There is code to support both a cache implementation and a DB implementation. These can be switched around on the dependency injection in the Startup.cs file. The cache implementation uses the built-in caching, so no other database setup is required. If you want persistent data, then you will need to provision and link to a database. This project uses Entity Framework (EF) Core as an ORM for interacting with a relational SQL Database. If deploying to a new environment, you will likely want to set up a new database for that environment.

The EF functionality is stored in the maternity-benefits project under the Storage/EF folder. The ApplicationDbContext is the main link between the database and the C# classes. This class is injected into the EFStore classes so that the C# code can interact with the database values. 

If new properties are being added to the database, then an EF migration is required (https://docs.microsoft.com/en-us/ef/core/managing-schemas/migrations/?tabs=dotnet-core-cli). The changes can be made to the C# class and then a migration must be created, which will generate a script inside the Migrations folder in the esdc-simulation-api project. The database must then be updated with the migration. This is actually handled in the code - in the Startup.cs file there is functionality to automatically run migrations.

If you are setting up a new Azure SQL database, then take the following steps:
- Create a new SQL server  in Azure. Create a username and password and store it securely
- In the firewall settings of your new server, ensure that you "Allow Azure Services" to access it, and you can also add your client IP to the whitelist
- Create a new database on that server
- Get the connection string for the database, and add it into the config setting (see "Config" section for where to add it)
- Format of connection string: `Server=tcp:your-server-name.net,1433;Database=your-db-name;Initial Catalog=your-db-name;Persist Security Info=False;User ID=your-user-id;Password=XXXXXXXXX;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;`
- Run the project, so that the code in Startup.cs can run the migration against the database
- Verify the database has the proper tables

### Data

Another consideration is the data that will be used for the simulations. We have explored using both real data as well as simply using mock data. For working with real data, you will need to be familiar with the [Data Primer project](https://github.com/DTS-STN/babel-data-primer). If using mock data, then you can use the functionality that is built-in to the simulation engine for generating and storing the mocks. The code to generate mocks is run by default on startup and can be edited in the Startup.cs file.

If you just want to get started on development, the easiest way to get things up and running is to use the cache storage implementation and use the mock data. Ensure this is all configured in the dependency injection in the Startup.cs file. When you run the project, the data will automatically get populated into the cache and will be ready to use for simulations.


### Config

When developing locally, ensure the config settings are properly set in the proper appsettings.Development.json file in the esdc-simulation-api project. The config settings of interest are:
- ConnectionStrings.DefaultDB: this is the connection string for the database that stores the simulation data and results.
- RulesOptions.Url: This is the host URL for the Rules Engine
- PasswordOptions.Password: This is a password that must be sent in the header of all requests related to storing the Persons (Getting, Deleting, Generating Mocks)

For deployments, the configs are handled in the Azure App Services (Configuration settings), and are injected as environment variables. The names of the required variables can be found in the Startup.cs file:
- DEFAULT_DB
- RULES_URL
- PASSWORD_FILTER


### Running Locally

- `cd` into the main solution folder
- `dotnet run --project esdc-simulation-api`

Note: If running this project locally alongside related web APIs (such as the Rules Engine), ensure you are specifying the projects to run on separate ports in the launchSettings.json file

### Running in Docker

- `docker build -t babel-simulation-engine .`
- `docker run -it --rm -p 7000:80 babel-simulation-engine`

### Testing

Tests are set up for the base library and the maternity-benefits class library. Running `dotnet test` will run the tests for all test projects. There is also a postman collection included to manually test the API requests.

### Deploying

Currently, the different deployments of the Simulation Engine are all hosted in  Microsoft Azure (Azure App Service). Each deployment is connected to its own server/db. Deployments are set up using github actions, based on manually clicking a button. Go to the github actions, choose the workflow, and then run it.

### Nuget package

The esdc-simulation-classes library must be re-published when making a change to the API request/response objects. The Front-end web app and the Data Primer rely on this Nuget Package, so they will need to be updated accordingly.

Reference: https://docs.microsoft.com/en-us/nuget/quickstart/create-and-publish-a-package-using-the-dotnet-cli

To update the Nuget package.
- Open the esdc-simulation-classes.csproj file
- Update the number inside the <Version> tag appropriately, depending on the type of change (semantic versioning)
- Run `dotnet build`. The project is configured to generate the Nuget package on build
- cd into the bin/debug folder of the esdc-simulation-classs project, where the generated .nupkg file is stored
- Replace the relevant values (X.Y.Z and the Api Key) in the following command and run it: `dotnet nuget push BabelSimulationEngineSample.X.Y.Z.nupkg --api-key [API_KEY_GOES_HERE] --source https://api.nuget.org/v3/index.json`. The X.Y.Z should correspond to the version you set in the csproj file.
- You can now go into the consuming applications (e.g. Data Primer) and update the package reference
 
