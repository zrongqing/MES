using Microsoft.EntityFrameworkCore;

namespace MES.Tests.UnitTests;

public class AppDbTest
{
    [SetUp]
    public void Setup()
    {
        // var contextOptions = new DbContextOptionsBuilder<MesDbContext>()
        //     .UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=MES;ConnectRetryCount=0")
        //     .Options;
        //
        // using var context = new MesDbContext(contextOptions);
    }

    [Test]
    public void Test1()
    {
        Assert.Pass();
    }
}