namespace SCDemo;

using Microsoft.EntityFrameworkCore.Diagnostics;
using System.Runtime.CompilerServices;

public class QueryStats
{
    public int Queries { get; private set; }
    public int Rows { get; private set; }
    public TimeSpan Duration { get; private set; } = TimeSpan.Zero;

    [MethodImpl(MethodImplOptions.Synchronized)]
    public void Track(CommandExecutedEventData eventData)
    {
        Queries++;
        Duration += eventData.Duration;
    }

    public void AddRow()
    {
        Rows++;
    }
}
