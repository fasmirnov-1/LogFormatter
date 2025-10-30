namespace LogFormatter.IO.Types
{
    public struct SrcLogFormat1
    {
        public DateTime RecordDate;
        public SrcRecordTypesFormat1 SrcRecordType;
        public string Message;
        public string Version;
    }

    public struct SrcLogFormat2
    {
        public DateTime RecordDate;
        public SrcRecordTypesFormat2 SrcRecordType;
        public string CallMethod;
        public string DeviceID;
    }

    public struct DstLogFormat
    {
        public DateTime RecordDate;
        public SrcRecordTypesFormat2 RecordType;
        public string Version;
        public string CallMethod;
    }
}