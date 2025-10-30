namespace LogFormatter.IO.Types
{
    public struct SrcLogHeaderFormat1
    {
        public string RecordDate;
        public string RecordType;
        public string Message;
        public string Version;
    }

    public struct SrcLogHeaderFormat2
    {
        public string RecordDate;
        public string RecordType;
        public string CallMethodName;
        public string DeviceID;
    }

    public struct DstLogHeaderFormat
    {
        public string RecordDate;
        public string RecordTime;
        public string RecordType;
        public string Version;
        public string CallMethodName;
    }
}