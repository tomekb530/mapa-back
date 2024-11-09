using Microsoft.EntityFrameworkCore;

namespace mapa_back
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options) { }

    }
}
