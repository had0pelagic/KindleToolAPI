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
        private readonly ILogger<NotionService> _logger;

        public NotionService(INotionPageService pageService, IClippingsService clippingsService, ILogger<NotionService> logger)
        {
            _pageService = pageService;
            _clippingsService = clippingsService;
            _logger = logger;
        }

        /// <summary>
        /// Sends clippings to given notion database
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="databaseId"></param>
        /// <param name="secret"></param>
        /// <returns></returns>
        public async Task<string> AddClippingsToNotion(ClippingsNotionDto dto)
        {
            var clippings = await _clippingsService.GetClippings(dto);

            _logger.LogInformation($"{GetType().Name} received {clippings.Count} clippings");

            var client = NotionClientFactory.Create(new ClientOptions
            {
                AuthToken = dto.Secret
            });

            foreach (var clipping in clippings)
            {
                var parentPageName = $"{clipping.Title} ({clipping.Author})";
                var childPageName = $"{clipping.Date.ToShortDateString()} - {clipping.Title}";

                var parentPageId = await _pageService.PageExistsByName(client, parentPageName);
                if (parentPageId == null)
                {
                    _logger.LogInformation($"Adding new parent page with [{parentPageName}] name");
                    parentPageId = await _pageService.AddPage(client, dto.DatabaseId, parentPageName);
                }

                var childPageId = await _pageService.PageExistsByName(client, childPageName);
                if (childPageId == null)
                {
                    _logger.LogInformation($"Adding new child page with [{childPageName}] name");
                    childPageId = await _pageService.AddChildPage(client, parentPageId, childPageName);
                }

                if (await _pageService.ContainsDuplicateParagraph(client, childPageId, clipping.Text))
                {
                    _logger.LogInformation($"Child page with [{childPageId}] id contains duplicate text");
                    continue;
                }

                if (clipping.Text.Length > ClippingConstants.MaximumTextLength)
                {
                    var texts = SplitText(clipping.Text, ClippingConstants.MaximumTextLength);
                    foreach (var text in texts)
                    {
                        _logger.LogInformation($"Appending block to page with [{childPageId}] id");
                        await _pageService.AppendBlockToPage(client, childPageId, Blocks.GetParagraphBlock(text));
                    }
                }
                else
                {
                    _logger.LogInformation($"Appending block to page with [{childPageId}] id");
                    await _pageService.AppendBlockToPage(client, childPageId, Blocks.GetParagraphBlock(clipping.Text));
                }
            }

            var message = $"Successfully uploaded [{clippings.Count}] clippings";
            _logger.LogInformation(message);

            return message;
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
