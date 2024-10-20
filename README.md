# Tada Todo!

## Local Development

### Install Dependencies

To run Tada Todo! locally you'll need `Node.js`, `Visual Studio 2022`, and an instance of `SQL Server`.

`Node.js` can be downloaded at [https://nodejs.org/](https://nodejs.org/).

`Visual Studio` can be downloaded at [https://visualstudio.microsoft.com/vs/community/](https://visualstudio.microsoft.com/vs/community/).
Check the `ASP.NET and web development` workload during installation. This will ensure that SQL Server Express and
other dependencies are available.

### Run Locally
1. Open `TadaTodo.sln` in `Visual Studio`.
2. Confirm the database connection string in `appsettings.Development.json` points to the `SQL Server` instance you want
   to use. The default value points to the default instance created by SQL Server Express and uses a database named
   `tadatodo`. This only needs to be changed if you want to use a different `SQL Server` instance.
3. Run the `http` run configuration for the `TadaTodo.Server` project in Debug mode. Running in Debug mode will make the
   application apply all database migrations automatically at runtime. The first time you run the application it will
   install the dependencies for the `TadaTodo.Client` project. This can take a some time so please be patient.