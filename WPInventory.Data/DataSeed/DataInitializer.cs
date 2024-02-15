using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace WPInventory.Data.DataSeed
{
   public abstract class DataInitializer
    {
        private readonly DbContext _dbContext;

        protected DataInitializer(DbContext context)
        {
            _dbContext = context;
        }

        public async Task Initialize(bool useMigration)
        {
            if (useMigration)
            {
                _dbContext.Database.Migrate();
            }

            await DataInit();
        }
        protected abstract Task DataInit();
    }

   public class DataInitializer<TDbContext> : DataInitializer where TDbContext : DbContext
   {
       public DataInitializer(TDbContext context):base(context)
       {
           
       }

       protected override Task DataInit()
       {
           return Task.CompletedTask;
       }
   }
}
