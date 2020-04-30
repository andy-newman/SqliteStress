using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using NetTopologySuite.Geometries;

namespace SqliteStress
{
    class Program
    {
        static void Main(string[] args)
        {
            for (var i = 0; i < 10000000; i++)
            {
                using (var connection = new SqliteConnection("DataSource=:memory:"))
                {
                    connection.Open();

                    var options = new DbContextOptionsBuilder<Context>()
                        .UseSqlite(connection, x => x.UseNetTopologySuite())
                        .Options;

                    var context = new Context(options);

                    context.Database.EnsureCreated();

                    context.Add(new Data { Name = "sdf", Geometry = new Point(10, 10) });

                    context.SaveChanges();
                }
            }
        }
    }

    public class Context : DbContext
    {
        public Context(DbContextOptions<Context> options) : base(options)
        {
        }

        public DbSet<Data> Datas { get; set; }
    }

    public class Data
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string Name { get; set; }

        public Geometry Geometry { get; set; }
    }
}
