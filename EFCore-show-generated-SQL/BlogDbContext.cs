using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Options;
using System.Data.Common;
using System.Diagnostics;

namespace EFCore_show_generated_SQL
{
    public class BlogDbContext : DbContext
    {
        public BlogDbContext()
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            //optionsBuilder.UseInMemoryDatabase("MyDatabase");
            if (!options.IsConfigured)
            {
                //options
                //    .LogTo(message => Debug.WriteLine(message))
                //    .EnableSensitiveDataLogging();

                options
                    .UseSqlite($"Data Source=blogging.db")
                    .LogTo(message => Debug.WriteLine(message))
                    //.LogTo(Console.WriteLine, LogLevel.Information)
                    .EnableSensitiveDataLogging()
                ;

                options.AddInterceptors(new SqlInterceptor());

            }
        }

        public DbSet<Blog> Blogs { get; set; }

    }

    public class Blog
    {
        public int BlogId { get; set; }
        public string Url { get; set; }
    }

    public class SqlInterceptor : DbCommandInterceptor
    {
        public override InterceptionResult<DbDataReader> ReaderExecuting(
            DbCommand command,
            CommandEventData eventData,
            InterceptionResult<DbDataReader> result)
        {
            var sql = command.CommandText;
            return result;
        }
    }
}
