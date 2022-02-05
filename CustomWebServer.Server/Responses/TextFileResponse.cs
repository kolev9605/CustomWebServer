using CustomWebServer.Server.Common;
using CustomWebServer.Server.HTTP;

namespace CustomWebServer.Server.Responses;

public class TextFileResponse : Response
{
    public TextFileResponse(string fileName) 
        : base(StatusCode.OK)
    {
        FileName = fileName;
    }

    public string FileName { get; set; }

    public override string ToString()
    {
        if (File.Exists(FileName))
        {
            Body = File.ReadAllTextAsync(FileName).Result;
        }

        var fileBytesCount = new FileInfo(FileName).Length;
        Headers.Add(Constants.HeaderNames.ContentLength, fileBytesCount.ToString());
        Headers.Add(Constants.HeaderNames.ContentDisposition, $"attachment; filename=\"{FileName}\"");

        return base.ToString();
    }
}

