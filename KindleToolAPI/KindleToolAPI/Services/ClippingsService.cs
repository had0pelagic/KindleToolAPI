using KindleToolAPI.DTOs;
using KindleToolAPI.Models;
using KindleToolAPI.Util.Constants;
using KindleToolAPI.Util.Enums;
using System.Globalization;
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
        public async Task<List<Clipping>> GetClippings(ClippingsFileDto dto)
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
                    GetFullDate(line, clipping);
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

            return clippings;
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
                clipping.Title = match.Groups[1].Value.Trim();
                clipping.Author = match.Groups[2].Value.Trim();
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

                clipping.Location = line.Substring(locationFrom, locationTo).Trim();
            }
            else
            {
                clipping.Location = "None";
            }
        }

        /// <summary>
        /// Extracts full date from given text
        /// </summary>
        /// <param name="line"></param>
        /// <param name="clipping"></param>
        private static void GetFullDate(string line, Clipping clipping)
        {
            if (line.Contains("Added on"))
            {
                var dateFrom = line.IndexOf("|") + "| ".Length;
                var dateTo = line.Length - dateFrom;

                var addedOn = line.Substring(dateFrom, dateTo);
                clipping.AddedOn = addedOn;

                GetDate(addedOn, clipping);
            }
            else
            {
                clipping.AddedOn = "None";
            }
        }

        /// <summary>
        /// Extracts date from given text
        /// </summary>
        /// <param name="line"></param>
        /// <param name="clipping"></param>
        private static void GetDate(string line, Clipping clipping)
        {
            var matchAddedOn = new Regex(ClippingConstants.AddedOnRegex).Match(line);
            var matchTime = new Regex(ClippingConstants.DateRegex).Match(line);

            if (matchAddedOn.Success && matchTime.Success)
            {
                line = line.Replace(matchAddedOn.ToString(), string.Empty);
                line = line.Replace(matchTime.ToString(), string.Empty);

                var dateSplit = line.Trim().Split(' ');
                if (!(dateSplit.Length == 3 && dateSplit[0].Length > 0 && dateSplit[1].Length > 0 && dateSplit[2].Length > 0))
                {
                    clipping.Date = new DateOnly();
                }

                var month = DateOnly.ParseExact(dateSplit[1], "MMMM", CultureInfo.CurrentCulture).Month;
                var date = new DateOnly(int.Parse(dateSplit[2]), month, int.Parse(dateSplit[0]));

                clipping.Date = date;
            }
            else
            {
                clipping.Date = new DateOnly();
            }
        }
    }
}
