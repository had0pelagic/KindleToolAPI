using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;

namespace KindleToolAPI.Extensions
{
    public static class FileExtension
    {
        public static FileContentResult ToTextFile<T>(this List<T> list, string fileName = "file", string contentType = "text/plain")
        {
            var data = JsonConvert.SerializeObject(list);
            var file = new FileContentResult(Encoding.Unicode.GetBytes(data), contentType);
            file.FileDownloadName = $"{fileName}.txt";

            return file;
        }
    }
}
