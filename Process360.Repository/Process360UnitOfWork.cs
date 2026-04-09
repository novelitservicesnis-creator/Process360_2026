using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Process360.Core;
using Process360.Repository.Repository.Base;
using System.Data.Common;
using System.Xml.Linq;

namespace Process360.Repository
{
    public class Process360UnitOfWork : UnitOfWorkBase<ProcessDbContext>
    {
        private readonly IConfiguration _configuration;

        public readonly ProcessDbContext Entity;

        public Process360UnitOfWork(int businessId, IConfiguration configuration)
        {
            this._configuration = configuration;


            var entiry = DbEntities(businessId);
            this.Entity = entiry;

        }
        ProcessDbContext DbEntities(int businessId)
        {
            var conn = SetCustomerDatabaseInfo(businessId);
            return new ProcessDbContext(conn);
        }

        string SetCustomerDatabaseInfo(int businessId)
        {
            var dbPrefix = _configuration["BDPrefix"];
            var databaseServer = _configuration["DbServer"];
            var databaseUserName = _configuration["DbUser"];
            var databasePassword = _configuration["DbPassword"];

            var databaseName = string.Format("{0}_{1}", dbPrefix, businessId);
            var databaseConnectionString = CreateString(databaseServer, databaseName, databaseUserName, databasePassword); 

            return databaseConnectionString;


        }
        string CreateString(string databaseServer, string databaseName, string databaseUserName, string databasePassword)
        {
            //"Server=JSSAGAR-LAPTOP\\SQLEXPRESS2022; Database=BridalConfig; User Id=sa; Password=Pl,mnb@1234; TrustServerCertificate=true;"
            var conn = string.Format(@"Server={0}; Database={1}; User Id={2}; Password={3}; TrustServerCertificate=true;"
                    , databaseServer
                    , databaseName
                    , databaseUserName
                    , databasePassword); 

            return conn; 
        }

        

    }
}
