using Npgsql;

namespace Discount.Grpc.Extensions;

public static class HostExtension
{
    public static IHost MigrateDatabase<TContext>(this IHost host, int? retry = 0)
    {
        int retryForAvailability = retry.Value;
        using (IServiceScope scope = host.Services.CreateScope())
        {
            IServiceProvider service = scope.ServiceProvider;
            IConfiguration configuration = service.GetRequiredService<IConfiguration>();
            ILogger<TContext> logger = service.GetRequiredService<ILogger<TContext>>();

            try
            {
                logger.LogInformation("Migration PostgreSQL");

                using NpgsqlConnection connection =
                    new NpgsqlConnection(configuration.GetValue<string>("DatabaseSettings:ConnectionString"));
                connection.Open();

                using NpgsqlCommand command = new NpgsqlCommand
                {
                    Connection = connection
                };

                command.CommandText = "DROP TABLE IF EXISTS Coupon";
                command.ExecuteNonQuery();

                command.CommandText = @"CREATE TABLE Coupon(Id SERIAL PRIMARY KEY,
                                                            ProductName VARCHAR(24) NOT NULL,
                                                            Description TEXT,
                                                            Amount INT)";
                command.ExecuteNonQuery();
                
                command.CommandText = @"INSERT INTO Coupon (ProductName, Description, Amount) VALUES ('IPhone X', 'IPhone Discount', 150);";
                command.ExecuteNonQuery();

                command.CommandText = @"INSERT INTO Coupon (ProductName, Description, Amount) VALUES ('Samsung 10', 'Samsung Discount', 100);";
                command.ExecuteNonQuery();
                
                logger.LogInformation("Migrated PostgreSQL database");
            }
            catch (NpgsqlException e)
            {
                logger.LogError(e, "An error occurred while migrating the PostgreSQL database");

                if (retryForAvailability < 50)
                {
                    retryForAvailability++;
                    Thread.Sleep(2000);
                    MigrateDatabase<TContext>(host, retryForAvailability);
                }
            }
        }

        return host;
    }
}