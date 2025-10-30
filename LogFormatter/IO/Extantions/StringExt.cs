using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using LogFormatter.IO.Types;
using LogFormatter.Exceptions;

namespace LogFormatter.IO.Extantions
{
    public static class StringExt
    {
        public static SrcRecordTypesFormat1? ToEnumFromLog1(this string recType)
        {
            switch(recType)
            {
                case "INFORMATION":
                    {
                        return SrcRecordTypesFormat1.INFORMATION;
                    }
                case "WARNING":
                    {
                        return SrcRecordTypesFormat1.WARNING;
                    }
                case "ERROR":
                    {
                        return SrcRecordTypesFormat1.ERROR;
                    }
                case "DEBUG":
                    {
                        return SrcRecordTypesFormat1.DEBUG;
                    }
            }

            throw new LogIOException("Ошибка интерпритации типа записи...", "StringExtModule...");
        }

        public static SrcRecordTypesFormat2? ToEnumFromLog2(this string recType)
        {
            switch(recType)
            {
                case "INFO":
                    {
                        return SrcRecordTypesFormat2.INFO;
                    }
                case "WARN":
                    {
                        return SrcRecordTypesFormat2.WARN;
                    }
                case "ERROR":
                    {
                        return SrcRecordTypesFormat2.ERROR;
                    }
                case "DEBUG":
                    {
                        return SrcRecordTypesFormat2.DEBUG;
                    }
            }

            throw new LogIOException("Ошибка интерпритации типа записи...", "StringExtModule...");
        }
    }
}
