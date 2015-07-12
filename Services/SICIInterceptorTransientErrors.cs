using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Infrastructure.Interception;
using System.Data.Entity.SqlServer;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Reflection;
using SICIApp.Interfaces;

namespace SICIApp.Services
{
    public class SICIInterceptorTransientErrors : DbCommandInterceptor
    {
        private int vContador = 0;
        private ILogger _logger = new Logger();

        public override void ReaderExecuting(DbCommand command, DbCommandInterceptionContext<DbDataReader> interceptionContext)
        {

            bool vThrowTransientErrors = false;
            if(command.Parameters.Count > 0 && command.Parameters[0].Value.ToString() == "Throw")
            {
                vThrowTransientErrors = true;
                command.Parameters[0].Value = "an";
                command.Parameters[1].Value = "an";
            }

            if(vThrowTransientErrors && vContador < 4)
            {
                _logger.Informacion("Retornado el error trasendiente para el comando: {0}", command.CommandText);
                vContador++;
                interceptionContext.Exception = CreateDummySqlException();
            }
        }

        private Exception CreateDummySqlException()
        {
            // The instance of SQL Server you attempted to connect to does not support encryption
            var sqlErrorNumber = 20;

            var sqlErrorCtor = typeof(SqlError).GetConstructors(BindingFlags.Instance | BindingFlags.NonPublic).Where(c => c.GetParameters().Count() == 7).Single();
            var sqlError = sqlErrorCtor.Invoke(new object[] { sqlErrorNumber, (byte)0, (byte)0, "", "", "", 1 });

            var errorCollection = Activator.CreateInstance(typeof(SqlErrorCollection), true);
            var addMethod = typeof(SqlErrorCollection).GetMethod("Add", BindingFlags.Instance | BindingFlags.NonPublic);
            addMethod.Invoke(errorCollection, new[] { sqlError });

            var sqlExceptionCtor = typeof(SqlException).GetConstructors(BindingFlags.Instance | BindingFlags.NonPublic).Where(c => c.GetParameters().Count() == 4).Single();
            var sqlException = (SqlException)sqlExceptionCtor.Invoke(new object[] { "Dummy", errorCollection, null, Guid.NewGuid() });

            return sqlException;
        }
    }
}