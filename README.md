# NetCoreRedisDemo
Redis implementation on .Net Core 7

**Demo setup:**

Database .bak download:

https://learn.microsoft.com/en-us/sql/samples/adventureworks-install-configure?view=sql-server-ver15&tabs=ssms

**Importing database:**

If you are working with Visual Studio, you can use the [Package Manager Console commands](https://www.learnentityframeworkcore.com/migrations/commands/pmc-commands#scaffold-dbcontext) to generate the code files for the model. The equivalent command to the last CLI command just above is:

```vhdl
 Install-Package Microsoft.EntityFrameworkCore.Tools 
```

Scaffold-DbContext "Server=localhost\SQLEXPRESS;database=AdventureWorks2022;Trusted_Connection=True;MultipleActiveResultSets=true" Microsoft.EntityFrameworkCore.SqlServer -OutputDir Model -Context "AdventureModelContext"

Redis Windows: https://redis.io/docs/install/install-redis/install-redis-on-windows/

Redis Linux: https://redis.io/docs/install/install-redis/install-redis-on-linux/

StackExchange.Redis: https://stackexchange.github.io/StackExchange.Redis/

Distributed Redis Cache: https://learn.microsoft.com/en-us/aspnet/core/performance/caching/distributed?view=aspnetcore-7.0

Redis benchmarks: https://redis.io/docs/management/optimization/benchmarks/#:~:text=Redis%20is%2C%20mostly%2C%20a%20single,on%20several%20cores%20if%20needed.

Debugging slow requests: https://redis.io/commands/slowlog-get/