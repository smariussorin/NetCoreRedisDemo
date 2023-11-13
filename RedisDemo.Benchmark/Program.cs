using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Engines;
using BenchmarkDotNet.Running;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RedisDemo.CompositionRoot;
using RedisDemo.Services.Employees;
using IHost = Microsoft.Extensions.Hosting.IHost;

var summary = BenchmarkRunner.Run(typeof(Program).Assembly);
Console.ReadLine();

[MemoryDiagnoser]
[SimpleJob(RunStrategy.ColdStart, iterationCount: 15, launchCount: 5)]
[MinColumn, MaxColumn, MeanColumn, MedianColumn]
public class EmployeesServiceBenchmark
{
    private EmployeesService _employeesService;
    private IHost _container;

    [Params("adventure-works\\james1", "adventure-works\\denise0")]
    public string LoginId { get; set; }

    [GlobalSetup]
    public void Setup()
    {
        IConfiguration configuration = new ConfigurationBuilder()
           .SetBasePath(Directory.GetCurrentDirectory())
           .AddJsonFile("appSettings.json", false)
           .Build();

        _container = Host.CreateDefaultBuilder()
            .ConfigureServices(services =>
            {
                services.ConfigureDependencyInjection(configuration);
            })
            .Build();

        _employeesService = _container.Services.GetService<EmployeesService>();
    }

    [Benchmark]
    public async Task GetByLoginId()
    {
        await _employeesService.GetByLoginIdAsync(LoginId);
    }

    [Benchmark]
    public async Task GetByLoginIdFromLocalCache()
    {
        await _employeesService.GetByLoginIdFromLocalCacheAsync(LoginId);
    }

    [Benchmark]
    public async Task GetByLoginIdCache()
    {
        await _employeesService.GetByLoginIdFromCacheAsync(LoginId);
    }

    [GlobalCleanup]
    public void Cleanup()
    {
        _container.Dispose();
    }
}