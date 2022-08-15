using KindleToolAPI.Util.Notion;
using Notion.Client;

namespace KindleToolAPI.Services
{
    public class NotionPageService : INotionPageService
    {

        /// <summary>
        /// Adds new page to database with given name
        /// </summary>
        /// <param name="client"></param>
        /// <param name="databaseId"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public async Task<string> AddNewPage(NotionClient client, string databaseId, string titleName)
        {
            var pageCreateParameters = PagesCreateParametersBuilder.Create(new DatabaseParentInput
            {
                DatabaseId = databaseId
            }).AddProperty("title", new TitlePropertyValue()
            {
                Title = new List<RichTextBase>()
                {
                    new RichTextText()
                    {
                        Text = new Text()
                        {
                            Content = titleName
                        }
                    }
                }
            }).Build();

            var page = await client.Pages.CreateAsync(pageCreateParameters);

            return page.Id;
        }

        /// <summary>
        /// Appends given block to page
        /// </summary>
        /// <param name="client"></param>
        /// <param name="pageId"></param>
        /// <param name="block"></param>
        /// <returns></returns>
        public async Task AppendBlockToPage(NotionClient client, string pageId, IBlock block)
        {
            var blocks = new BlocksAppendChildrenParameters()
            {
                Children = new List<IBlock>()
                {
                    block
                }
            };

            await client.Blocks.AppendChildrenAsync(pageId, blocks);
        }

        /// <summary>
        /// Adds new page to all pages in the database
        /// </summary>
        /// <param name="client"></param>
        /// <param name="databaseId"></param>
        /// <returns></returns>
        public async Task AddNewPageToAllPages(NotionClient client, string databaseId)
        {
            var dbQueryParameters = new DatabasesQueryParameters();
            var query = await client.Databases.QueryAsync(databaseId, dbQueryParameters);

            foreach (var result in query.Results)
            {
                Console.WriteLine($"Page ID: {result.Id}");

                var pageParams = PagesCreateParametersBuilder.Create(new ParentPageInput
                {
                    PageId = result.Id
                }).Build();

                var blocks = new List<IBlock>()
                {
                    new HeadingOneBlock
                    {
                        Heading_1 = new HeadingOneBlock.Info()
                        {
                            RichText = new List<RichTextBase>()
                            {
                                new RichTextText()
                                {
                                    Text = new Text
                                    {
                                        Content="text"
                                    }
                                }
                            }
                        }
                    }
                };

                pageParams.Children = blocks;
                await client.Pages.CreateAsync(pageParams);
            }
        }

        /// <summary>
        /// Returns all blocks in database pages
        /// </summary>
        /// <param name="client"></param>
        /// <param name="databaseId"></param>
        /// <returns></returns>
        public async Task GetPageBlocks(NotionClient client, string databaseId)
        {
            var dbQueryParameters = new DatabasesQueryParameters();
            var query = await client.Databases.QueryAsync(databaseId, dbQueryParameters);

            foreach (var result in query.Results)
            {
                var blocks = await client.Blocks.RetrieveChildrenAsync(result.Id);
            }
        }

        /// <summary>
        /// Appends to database pages
        /// </summary>
        /// <param name="client"></param>
        /// <param name="databaseId"></param>
        /// <returns></returns>
        public async Task AppendToPages(NotionClient client, string databaseId)
        {
            var dbQueryParameters = new DatabasesQueryParameters();
            var query = await client.Databases.QueryAsync(databaseId, dbQueryParameters);

            foreach (var result in query.Results)
            {
                var blocks = new BlocksAppendChildrenParameters()
                {
                    Children = new List<IBlock>()
                    {
                       Blocks.GetHeadingOneBlock("Test heading"),
                       Blocks.GetParagraphBlock($"Paragraph for page: {result.Id}")
                    }
                };

                await client.Blocks.AppendChildrenAsync(result.Id, blocks);
            }
        }
    }
}
