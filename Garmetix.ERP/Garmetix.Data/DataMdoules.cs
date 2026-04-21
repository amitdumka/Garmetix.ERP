using Garmetix.Core.Interfaces;
using Garmetix.Data.Context;
using Garmetix.Data.Repositories;

namespace Garmetix.Data
{
    // All the code in this file is included in all platforms.
    public static class DataMdoules
    {
        public const string DatabaseFileName = "garmetix_erp_v1.db3";

        public static MauiAppBuilder UseGarmetixDataModules(this MauiAppBuilder builder)
        {
            // 1. Setup DB Context
            string dbPath = Path.Combine(FileSystem.AppDataDirectory, DatabaseFileName);
            builder.Services.AddSingleton(s => new AppDbContext(dbPath));

            // 2. THIS IS THE MAGIC LINE YOU MIGHT BE MISSING
            // It registers the open generic type so any IRepository<T> automatically gets a GenericRepository<T>
            builder.Services.AddTransient(typeof(IRepository<>), typeof(GenericRepository<>));
            return builder;
        }   
    }
}
