using KindleToolAPI.DTOs;
using KindleToolAPI.Extensions;
using KindleToolAPI.Models;
using KindleToolAPI.Util.Constants;
using KindleToolAPI.Util.Enums;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Text.RegularExpressions;

namespace KindleToolAPI.Services
{
    public class ClippingsService : IClippingsService
    {
        public ClippingsService() { }

        /// <summary>
        /// Convert clippings file text to JSON
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task<FileContentResult> ClippingsToJson(ClippingsFileDto dto)
        {
            var clippings = new List<Clipping>();
            var clipping = new Clipping();
            var lineNumber = 1;
            var text = new StringBuilder();

            using var reader = new StreamReader(dto.File.OpenReadStream());
            while (reader.Peek() >= 0)
            {
                var line = await reader.ReadLineAsync();

                if (line == null)
                {
                    line = "None";
                }

                if (lineNumber == 1)
                {
                    GetTitleAuthor(line, clipping);
                }
                else if (lineNumber == 2)
                {
                    GetType(line, clipping);
                    GetLocation(line, clipping);
                    GetDate(line, clipping);
                }
                else if (line == ClippingConstants.TextSeparator)
                {
                    clipping.Text = text.ToString();
                    clippings.Add(clipping);
                    clipping = new();
                    text.Clear();
                    lineNumber = 1;
                    continue;
                }
                else
                {
                    text.Append(line);
                }

                lineNumber++;
            }

            return clippings.ToTextFile("ClippingsJson");
        }

        /// <summary>
        /// Extracts title and author from given text
        /// </summary>
        /// <param name="line"></param>
        /// <param name="clipping"></param>
        private static void GetTitleAuthor(string line, Clipping clipping)
        {
            var match = new Regex(ClippingConstants.TitleAuthorRegex).Match(line);

            if (match.Success)
            {
                clipping.Title = match.Groups[1].Value;
                clipping.Author = match.Groups[2].Value;
            }
            else
            {
                clipping.Title = "None";
                clipping.Author = "None";
            }
        }

        /// <summary>
        /// Extracts type from given text
        /// </summary>
        /// <param name="line"></param>
        /// <param name="clipping"></param>
        private static void GetType(string line, Clipping clipping)
        {
            if (line.Contains("Highlight"))
            {
                clipping.Type = ClippingTypeEnum.Highlight.ToString();
            }
            else if (line.Contains("Note"))
            {
                clipping.Type = ClippingTypeEnum.Note.ToString();
            }
            else if (line.Contains("Bookmark"))
            {
                clipping.Type = ClippingTypeEnum.Bookmark.ToString();
            }
            else
            {
                clipping.Type = ClippingTypeEnum.None.ToString();
            }
        }

        /// <summary>
        /// Extracts location from given text
        /// </summary>
        /// <param name="line"></param>
        /// <param name="clipping"></param>
        private static void GetLocation(string line, Clipping clipping)
        {
            if (line.Contains("location"))
            {
                var locationFrom = line.IndexOf("location") + "location ".Length;
                var locationTo = line.IndexOf("|") - locationFrom - 1;

                clipping.Location = line.Substring(locationFrom, locationTo);
            }
            else
            {
                clipping.Location = "None";
            }
        }

        /// <summary>
        /// Extracts date from given text
        /// </summary>
        /// <param name="line"></param>
        /// <param name="clipping"></param>
        private static void GetDate(string line, Clipping clipping)
        {
            if (line.Contains("Added on"))
            {
                var dateFrom = line.IndexOf("|") + "| ".Length;
                var dateTo = line.Length - dateFrom;

                clipping.AddedOn = line.Substring(dateFrom, dateTo);
            }
            else
            {
                clipping.Location = "None";
            }
        }
    }
}
