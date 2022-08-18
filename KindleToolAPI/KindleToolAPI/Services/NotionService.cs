using KindleToolAPI.DTOs;
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
                var authorTitle = $"{clipping.Title} ({clipping.Author})";
                var childPageName = $"{clipping.Date.ToShortDateString()} - {clipping.Title}";

                var parentPageId = await _pageService.PageExistsByName(client, authorTitle);
                if (parentPageId == null)
                {
                    parentPageId = await _pageService.AddPage(client, databaseId, authorTitle);
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

                await _pageService.AppendBlockToPage(client, childPageId, Blocks.GetParagraphBlock(clipping.Text));
            }
        }
    }
}
