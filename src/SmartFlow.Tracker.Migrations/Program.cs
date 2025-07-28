using System.Reflection;
using DbUp;

class Program
{
    static int Main(string[] args)
    {
        var connectionString = Environment.GetEnvironmentVariable("CONNECTION_STRING")
                               ?? throw new ArgumentNullException("CONNECTION_STRING not found");

        var upgrader = DeployChanges.To
            .PostgresqlDatabase(connectionString)
            .WithScriptsEmbeddedInAssembly(Assembly.GetExecutingAssembly())
            .LogToConsole()
            .Build();

        var result = upgrader.PerformUpgrade();

        if (!result.Successful)
        {
            Console.Error.WriteLine(result.Error);
        }

        return result.Successful ? 0 : -1;
    }
}