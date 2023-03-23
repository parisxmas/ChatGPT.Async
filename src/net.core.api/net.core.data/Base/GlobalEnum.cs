using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace net.core.data.Base
{
    public enum ColumnType
    {
        Boolean,
        Byte,
        ByteArray,
        Int16,
        Int32,
        Int64,
        String,
        SByte,
        Single,
        TimeSpan,
        Decimal,
        Double,
        Guid,
        DateTime,
        UInt16,
        UInt32,
        UInt64
    }

    public enum MessageType
    {
        Success,
        Error,
        Warning,
        Info
    }

    public enum DateType
    {
        LongDateTime,
        LongDate,
        Time,
        DateTime,
        Date
    }

    public enum ExportType
    {
        Excel,
        PDF,
        Word,
        Html
    }

    public enum HTTPType
    {
        GET,
        POST
    }

    public enum Status
    {
        Success,
        Error
    }

    public enum OrderBy
    {
        ASC,
        DESC
    }

    public enum SubstringType
    {
        Space,
        Global
    }

    public enum RedirectType
    {
        R301,
        R302
    }

    public enum ImageSize
    {
        Small,
        Medium,
        Large
    }

    public enum StringDataType
    {
        Json,
        Xml
    }

    public enum InformationType
    {
        Id,
        BrowserType,
        BrowserName,
        MachineName,
        IsMobile,
        Language,
        LanguageCulture,
        UserAgent,
        IP
    }

    public enum RequestType
    {
        Integer,
        String,
        Double,
        DateTime,
        Boolean
    }

}