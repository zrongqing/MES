using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace MES.Infrastructure.Context;

public class MESDbContextFactory: IDesignTimeDbContextFactory<MESDbContext>
{
    public MESDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<MESDbContext>();
        optionsBuilder.UseSqlServer("Server=localhost;Database=MES;Trusted_Connection=True;TrustServerCertificate=True;");
        return new MESDbContext(optionsBuilder.Options);
    }
}

