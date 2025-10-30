using LogFormatter.Exceptions;
using LogFormatter.Logs;

using (Rebuilder rebuilder = new Rebuilder())
{
    try
    {
        rebuilder.RebuidLogs();
    }
    catch (LogIOException e)
    {
        Console.WriteLine(e.Message);
    }
}

Console.WriteLine("Операция завершена...");