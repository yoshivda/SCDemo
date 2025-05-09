namespace SCDemo;

using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System.Data.Common;

public class QueryInterceptor : DbCommandInterceptor
{
    public override InterceptionResult<DbDataReader> ReaderExecuting(DbCommand command, CommandEventData eventData,
        InterceptionResult<DbDataReader> result)
    {
        Thread.Sleep(10);
        return base.ReaderExecuting(command, eventData, result);
    }

    public override DbDataReader ReaderExecuted(DbCommand command, CommandExecutedEventData eventData,
        DbDataReader result)
    {
        var stats = eventData.Context.GetService<QueryStats>();
        stats.Track(eventData);
        return new RowCountingDbReader(result, stats);
        // return result;
    }
}
