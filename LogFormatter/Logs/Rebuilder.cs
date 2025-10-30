using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogFormatter.Exceptions;
using LogFormatter.IO;
using LogFormatter.IO.Types;

namespace LogFormatter.Logs
{
    public class Rebuilder: IDisposable
    {
        public void RebuidLogs()
        {
            SrcLogHeaderFormat1 log1Header = new SrcLogHeaderFormat1();
            SrcLogHeaderFormat2 log2Header = new SrcLogHeaderFormat2();

            List<SrcLogFormat1> log1 = null;
            List<SrcLogFormat2> log2 = null;

            DstLogHeaderFormat resultHeader = new DstLogHeaderFormat();
            List<DstLogFormat> resultLog = new List<DstLogFormat>();

            using (Disk disk = new Disk(@"c:\Temp"))
            {
                log1Header = disk.GetLog1Header();
                log1 = disk.GetLog1Body();

                log2Header = disk.GetLog2Header();
                log2 = disk.GetLog2Body();

                resultHeader.RecordDate = log1Header.RecordDate;
                resultHeader.RecordTime = "Время";
                resultHeader.RecordType= log1Header.RecordType;
                resultHeader.Version = log1Header.Version;
                resultHeader.CallMethodName = log2Header.CallMethodName;
             
                foreach(SrcLogFormat1 log1Item in log1)
                {
                    resultLog.Add(new DstLogFormat()
                    {
                        RecordDate = log1Item.RecordDate,
                        Version = log1Item.Version,
                    });
                }

                List<DstLogFormat>resultLogItemBuffer = new List<DstLogFormat>();

                foreach(SrcLogFormat2 logItem in log2.ToArray())
                {
                    DstLogFormat resultLogItem = resultLog.FirstOrDefault(x => x.RecordDate == logItem.RecordDate);

                    resultLogItem.RecordType = logItem.SrcRecordType;
                    resultLogItem.CallMethod = logItem.CallMethod;

                    resultLogItemBuffer.Add(resultLogItem);
                }

                resultLog = resultLogItemBuffer;

                try
                {
                    disk.WriteDstLog(resultLog.ToArray(), resultHeader);
                }
                catch(LogIOException e)
                {
                    throw new Exception(e.Message);
                }
            }
        }
        public void Dispose() 
        {
            GC.SuppressFinalize(this);
        }
    }
}
