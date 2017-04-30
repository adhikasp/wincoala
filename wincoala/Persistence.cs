using SQLite;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wincoala
{
    /**
     * Persistence is responsible to reduce the load 
     * time of API access through caching of data.
     */
    class Persistence
    {
        private static Persistence instance;
        private SQLiteConnection dbConnection;

        public static Persistence Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new Persistence();
                }
                return instance;
            }
        }

        Persistence()
        {
            this.dbConnection = new SQLiteConnection("wincoala");

            // Setup DB
            dbConnection.CreateTable<BearMetadata>();
            dbConnection.DeleteAll<BearMetadata>();
        }

        public List<BearMetadata> getAllBear()
        {
            return dbConnection.Table<BearMetadata>().ToList();
        }

        public Boolean saveBear(List<BearMetadata> bears)
        {
            // Always drop existing cache.
            dbConnection.DeleteAll<BearMetadata>();
            // Make sure something is saved to db.
            return dbConnection.InsertAll(bears) != 0;
        }

    }
}
