using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using LogFormatter.IO.Extantions;
using LogFormatter.IO.Types;

using LogFormatter.Exceptions;

namespace LogFormatter.IO
{
    public class Disk: IDisposable
    {
        protected string logFolder = null;

        protected SrcLogHeaderFormat1? srcLogHeader1 = null;
        protected SrcLogHeaderFormat2? srcLogHeader2 = null;

        protected SrcLogFormat1[] srcLog1 = null;
        protected SrcLogFormat2[] srcLog2 = null;

        protected string srcLog1FileName = "srcLog1.txt";
        protected string srcLog2FileName = "srcLog2.txt";
        protected string dstLogFileName = "dstLog.txt";
        public Disk(string logFolderPath)
        {
            logFolder = logFolderPath;

            srcLog1 = new SrcLogFormat1[10];
            srcLog2 = new SrcLogFormat2[10];

            DateTime dateTimeNow = DateTime.Now;

            srcLogHeader1 = new SrcLogHeaderFormat1()
            {
                RecordDate = "Дата",
                RecordType = "Уровень Логирования",
                Message = "Сообщение",
                Version = "Версия программы"
            };

            srcLogHeader2 = new SrcLogHeaderFormat2()
            {
                RecordDate = "Дата",
                RecordType = "Уровень Логирования",
                CallMethodName = "ВызвавшийМетод",
                DeviceID = "Код устройства"
            };

            if(!Directory.Exists(logFolderPath))
            {
                Directory.CreateDirectory(logFolderPath);
            }


            for(int i = 0; i < 10; i++)
            {
                srcLog1[i].Version = "3.4.0.48729";
                srcLog1[i].RecordDate = dateTimeNow.AddDays(1);
                srcLog1[i].SrcRecordType = SrcRecordTypesFormat1.INFORMATION;
           
                srcLog2[i].DeviceID = "@MINDEO-M40-D-410244015546";
                srcLog2[i].RecordDate= dateTimeNow.AddDays(1);
                srcLog2[i].SrcRecordType = SrcRecordTypesFormat2.INFO;
                srcLog2[i].CallMethod = "MobileComputer.GetDeviceId";
            }

            string headerForLog1 = srcLogHeader1.Value.RecordDate + '\t' +
                                    srcLogHeader1.Value.RecordType + '\t' +
                                    srcLogHeader1.Value.Version + Environment.NewLine;

            string headerForLog2 = srcLogHeader2.Value.RecordDate + '\t' +
                                    srcLogHeader2.Value.RecordType + '\t' +
                                    srcLogHeader2.Value.CallMethodName + '\t' +
                                    srcLogHeader2.Value.DeviceID + Environment.NewLine;

            List<string> srcLog1Body = new List<string>();
            List<string> srcLog2Body = new List<string>();

            foreach(SrcLogFormat1 recordItem in srcLog1)
            {
                srcLog1Body.Add(recordItem.RecordDate.ToString() + '\t' +
                               recordItem.SrcRecordType.ToString() + '\t' +
                               recordItem.Version);
            }

            foreach(SrcLogFormat2 recordItem in srcLog2)
            {
                srcLog2Body.Add(recordItem.RecordDate.ToString() + '\t' +
                               recordItem.SrcRecordType.ToString() + '\t' +
                               recordItem.CallMethod + '\t' +
                               recordItem.DeviceID);
            }

            File.WriteAllText(logFolderPath + '\\' + srcLog1FileName, headerForLog1);
            File.AppendAllLines(logFolderPath + '\\' + srcLog1FileName, srcLog1Body);

            File.WriteAllText(logFolderPath + '\\' + srcLog2FileName, headerForLog2);
            File.AppendAllLines(logFolderPath + '\\' + srcLog2FileName, srcLog2Body);
        }

        public SrcLogHeaderFormat1 GetLog1Header()
        {
            string[] headerParts = GetHeaderParts(srcLog1FileName);

            return new SrcLogHeaderFormat1()
            {
                RecordDate = headerParts[0],
                RecordType = headerParts[1],
                Version = headerParts[2]
            };
        }

        public SrcLogHeaderFormat2 GetLog2Header()
        {
            string[] headerParts = GetHeaderParts(srcLog2FileName);

            return new SrcLogHeaderFormat2()
            {
                RecordDate = headerParts[0],
                RecordType = headerParts[1],
                CallMethodName = headerParts[2],
                DeviceID = headerParts[3]
            };
        }

        public List<SrcLogFormat1> GetLog1Body()
        {
            string[] srcLog1 = File.ReadAllLines(logFolder + '\\' + srcLog1FileName);
            List<string> srcLog1Body = new List<string>();

            for(int i = 1; i < srcLog1.Length - 1; i++)
            {
                srcLog1Body.Add (srcLog1[i]);
            }

            List<SrcLogFormat1> log1Body = new List<SrcLogFormat1>();

            try
            {
                foreach (string srcLog1BodyItem in srcLog1Body)
                {
                    string[] logParts = srcLog1BodyItem.Split('\t');

                    log1Body.Add(new SrcLogFormat1()
                    {
                        RecordDate = Convert.ToDateTime(logParts[0]),
                        SrcRecordType = logParts[1].ToEnumFromLog1().Value,
                        Version = logParts[2]
                    });
                }
            }
            catch (LogIOException e) 
            {
                new LogIOException(e.Message, e.Source);
            }

            return log1Body;
        }

        public List<SrcLogFormat2> GetLog2Body()
        {
            string[] srcLog2 = File.ReadAllLines(logFolder + '\\' + srcLog2FileName);
            List<string> srcLog2Body = new List<string>();

            for(int i = 1; i < srcLog2.Length; i++)
            {
                srcLog2Body.Add(srcLog2[i]);
            }

            List<SrcLogFormat2> log2Body = new List<SrcLogFormat2>();

            try
            {
                foreach(string srcLog2BodyItem in srcLog2Body)
                {
                    string[] logParts = srcLog2BodyItem.Split('\t');

                    log2Body.Add(new SrcLogFormat2()
                    {
                        RecordDate = Convert.ToDateTime(logParts[0]),
                        SrcRecordType = logParts[1].ToEnumFromLog2().Value,
                        CallMethod = logParts[2],
                        DeviceID = logParts[3]
                    });
                }
            }
            catch (LogIOException e)
            {
                new LogIOException(e.Message, e.Source);
            }

            return log2Body;
        }

        public void WriteDstLog(DstLogFormat[] dstLog, DstLogHeaderFormat dstHeader)
        {
            string dstHeaderLine = dstHeader.RecordDate + '\t' +
                                   dstHeader.RecordTime + '\t' +
                                   dstHeader.RecordType + '\t' +
                                   dstHeader.Version + '\t' +
                                   dstHeader.CallMethodName + Environment.NewLine;
            
            File.WriteAllText(logFolder + '\\' + dstLogFileName, dstHeaderLine);

            List<string> dstLogBody = new List<string>();

            foreach(DstLogFormat dstLogItem in dstLog)
            {
                string dstLogLineBuffer = dstLogItem.RecordDate.ToShortDateString() + '\t' +
                                          dstLogItem.RecordDate.ToShortTimeString() + "\t" +
                                          dstLogItem.RecordType + '\t' +
                                          dstLogItem.Version + '\t' +
                                          dstLogItem.CallMethod;
         
                dstLogBody.Add(dstLogLineBuffer);
            }

            File.AppendAllLines(logFolder + '\\' + dstLogFileName, dstLogBody.ToArray());
        }

        private string[] GetHeaderParts(string srcLogFileName)
        {
            string[] logBody = File.ReadAllLines(logFolder + '\\' + srcLogFileName);
            string logHeader = logBody[0];

            return logHeader.Split('\t');
        }

        public void Dispose() {
            logFolder = null;

            srcLogHeader1 = null;
            srcLogHeader2 = null;
      
            srcLog1 = null;
            srcLog2 = null;

            srcLog1FileName = null;
            srcLog2FileName = null;
            dstLogFileName = null;

            GC.SuppressFinalize(this);
        }
    }
}
