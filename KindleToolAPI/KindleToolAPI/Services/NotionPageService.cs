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
        public async Task<string> AddPage(NotionClient client, string databaseId, string titleName)
        {
            //check if one exists with the same name
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
        /// Adds child page to given page
        /// </summary>
        /// <param name="client"></param>
        /// <param name="databaseId"></param>
        /// <param name="titleName"></param>
        /// <param name="text"></param>
        /// <returns></returns>
        public async Task<string> AddChildPage(NotionClient client, string parentPageId, string titleName)
        {
            var pageCreateParameters = PagesCreateParametersBuilder.Create(new ParentPageInput
            {
                PageId = parentPageId
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
        /// Checks if page exists with given id
        /// </summary>
        /// <param name="client"></param>
        /// <param name="pageId"></param>
        /// <returns></returns>
        public async Task<bool> PageExistsById(NotionClient client, string pageId)
        {
            var search = await client.Search.SearchAsync(new SearchParameters());

            foreach (var item in search.Results)
            {
                if (item is not Page page)
                {
                    continue;
                }

                var id = page.Id.Replace("-", "");
                if (id == pageId || page.Id == pageId)
                {
                    return true;
                }
                continue;
            }

            return false;
        }

        /// <summary>
        /// Check if page exists with given name and returns it's name
        /// </summary>
        /// <param name="client"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public async Task<string?> PageExistsByName(NotionClient client, string name)
        {
            var search = await client.Search.SearchAsync(new SearchParameters());

            foreach (var item in search.Results)
            {
                if (item is not Page page)
                {
                    continue;
                }

                foreach (var property in page.Properties)
                {
                    if (property.Value is TitlePropertyValue titlePropertyValue && titlePropertyValue.Title.FirstOrDefault()?.PlainText == name)
                    {
                        return item.Id;
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="client"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public async Task<bool> ContainsDuplicateParagraph(NotionClient client, string pageId, string text)
        {
            var blocks = await client.Blocks.RetrieveChildrenAsync(pageId, new BlocksRetrieveChildrenParameters());

            foreach (var block in blocks.Results)
            {
                if (block is not ParagraphBlock paragraph)
                {
                    continue;
                }
                if (text == paragraph.Paragraph.RichText.FirstOrDefault()?.PlainText)
                {
                    return true;
                }
            }

            return false;
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

        /// <summary>
        /// Returns property value based on type
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private object GetValue(PropertyValue value)
        {
            switch (value)
            {
                case RichTextPropertyValue richTextPropertyValue://text type
                    return richTextPropertyValue.RichText.FirstOrDefault()?.PlainText;
                case TitlePropertyValue titlePropertyValue://title type
                    return titlePropertyValue.Title.FirstOrDefault()?.PlainText;
                default:
                    return null;
            }
        }
    }
}
