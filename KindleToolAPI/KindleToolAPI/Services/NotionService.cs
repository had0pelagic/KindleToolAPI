using KindleToolAPI.DTOs;
using KindleToolAPI.Util.Constants;
using KindleToolAPI.Util.Notion;
using Notion.Client;

namespace KindleToolAPI.Services
{
    public class NotionService : INotionService
    {
        private readonly INotionPageService _pageService;
        private readonly IClippingsService _clippingsService;

        public NotionService(INotionPageService pageService, IClippingsService clippingsService)
        {
            _pageService = pageService;
            _clippingsService = clippingsService;
        }

        /// <summary>
        /// Sends clippings to given notion database
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="databaseId"></param>
        /// <param name="secret"></param>
        /// <returns></returns>
        public async Task AddClippingsToNotion(ClippingsDto dto, string databaseId, string secret)
        {
            var clippings = await _clippingsService.GetClippings(dto);

            if (clippings.Count == 0)
            {
                return;
            }

            var client = NotionClientFactory.Create(new ClientOptions
            {
                AuthToken = secret
            });

            foreach (var clipping in clippings)
            {
                var parentPageName = $"{clipping.Title} ({clipping.Author})";
                var childPageName = $"{clipping.Date.ToShortDateString()} - {clipping.Title}";

                var parentPageId = await _pageService.PageExistsByName(client, parentPageName);
                if (parentPageId == null)
                {
                    parentPageId = await _pageService.AddPage(client, databaseId, parentPageName);
                }

                var childPageId = await _pageService.PageExistsByName(client, childPageName);
                if (childPageId == null)
                {
                    childPageId = await _pageService.AddChildPage(client, parentPageId, childPageName);
                }

                if (await _pageService.ContainsDuplicateParagraph(client, childPageId, clipping.Text))
                {
                    continue;
                }

                if (clipping.Text.Length > ClippingConstants.MaximumTextLength)
                {
                    var texts = SplitText(clipping.Text, ClippingConstants.MaximumTextLength);
                    foreach (var text in texts)
                    {
                        await _pageService.AppendBlockToPage(client, childPageId, Blocks.GetParagraphBlock(text));
                    }
                }
                else
                {
                    await _pageService.AppendBlockToPage(client, childPageId, Blocks.GetParagraphBlock(clipping.Text));
                }
            }
        }

        /// <summary>
        /// Splits given text into smaller pieces
        /// </summary>
        /// <param name="text"></param>
        /// <param name="maxLength"></param>
        /// <returns></returns>
        private static List<string> SplitText(string text, int maxLength)
        {
            var textList = new List<string>();
            var textLength = text.Length;

            while (true)
            {
                if (textList.Count == 0)
                {
                    textList.Add(text.Substring(0, maxLength));
                    textLength -= maxLength;
                }
                else if (textLength > maxLength)
                {
                    textList.Add(text.Substring(textList[^1].Length * textList.Count, maxLength));
                    textLength -= maxLength;
                }
                else if (textLength == maxLength || textLength < maxLength)
                {
                    textList.Add(text.Substring(textList[^1].Length * textList.Count, textLength));
                    break;
                }
            }

            return textList;
        }
    }
}
