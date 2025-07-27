using System.Reflection;
using DbUp;

class Program
{
    static int Main(string[] args)
    {
        var connectionString = Environment.GetEnvironmentVariable("CONNECTION_STRING")
                               ?? "Host=db;Database=smartflow;Username=postgres;Password=postgres";

        var upgrader = DeployChanges.To
            .PostgresqlDatabase(connectionString)
            .WithScriptsEmbeddedInAssembly(Assembly.GetExecutingAssembly())
            .LogToConsole()
            .Build();

        var result = upgrader.PerformUpgrade();
        return result.Successful ? 0 : -1;
    }
}