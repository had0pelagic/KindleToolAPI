using Notion.Client;

namespace KindleToolAPI.Services
{
    public class NotionService : INotionService
    {
        public async Task<string> Test(string key, string databaseId)
        {
            var client = NotionClientFactory.Create(new ClientOptions
            {
                AuthToken = key
            });

            var pageId = await AddNewPage(client, databaseId, "Page!");
            await AppendBlockToPage(client, pageId, GetHeadingOneBlock("Heading one block!"));

            return "test";
        }

        /// <summary>
        /// Retrieves database data
        /// </summary>
        /// <param name="client"></param>
        /// <param name="databaseId"></param>
        /// <returns></returns>
        private async Task<Database> GetDatabase(NotionClient client, string databaseId)
        {
            return await client.Databases.RetrieveAsync(databaseId);
        }

        /// <summary>
        /// Adds new page to database with given name
        /// </summary>
        /// <param name="client"></param>
        /// <param name="databaseId"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        private async Task<string> AddNewPage(NotionClient client, string databaseId, string name)
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
                            Content = name
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
        private async Task AppendBlockToPage(NotionClient client, string pageId, IBlock block)
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
        /// Gets keys and values of database entries
        /// </summary>
        /// <param name="client"></param>
        /// <param name="databaseId"></param>
        /// <returns></returns>
        private async Task QueryWholeDatabase(NotionClient client, string databaseId)
        {
            var dbQueryParameters = new DatabasesQueryParameters();//empty database filter used to get all values without sorting or filtering
            var query = await client.Databases.QueryAsync(databaseId, dbQueryParameters);

            foreach (var result in query.Results)
            {
                foreach (var property in result.Properties)
                {
                    var propKey = property.Key;
                    var value = property.Value;

                    var titleName = GetValue(value);
                    Console.WriteLine(titleName);
                    Console.WriteLine($"key: {propKey} -- -- -- value: {value}");
                }
            }
        }

        /// <summary>
        /// Adds new page to all pages in the database
        /// </summary>
        /// <param name="client"></param>
        /// <param name="databaseId"></param>
        /// <returns></returns>
        private async Task AddNewPageToAllPages(NotionClient client, string databaseId)
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
        private async Task GetPageBlocks(NotionClient client, string databaseId)
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
        private async Task AppendToPages(NotionClient client, string databaseId)
        {
            var dbQueryParameters = new DatabasesQueryParameters();
            var query = await client.Databases.QueryAsync(databaseId, dbQueryParameters);

            foreach (var result in query.Results)
            {
                var blocks = new BlocksAppendChildrenParameters()
                {
                    Children = new List<IBlock>()
                    {
                       GetHeadingOneBlock("Test heading"),
                       GetParagraphBlock($"Paragraph for page: {result.Id}")
                    }
                };

                await client.Blocks.AppendChildrenAsync(result.Id, blocks);
            }
        }

        /// <summary>
        /// Returns paragraph text block
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        private ParagraphBlock GetParagraphBlock(string text)
        {
            return new ParagraphBlock
            {
                Paragraph = new ParagraphBlock.Info()
                {
                    RichText = new List<RichTextBase>()
                    {
                        new RichTextText()
                        {
                            Text = new Text()
                            {
                                Content = text
                            }
                        }
                    }
                }
            };
        }

        /// <summary>
        /// Returns full heading one block
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        private HeadingOneBlock GetHeadingOneBlock(string text)
        {
            return new HeadingOneBlock
            {
                Heading_1 = new HeadingOneBlock.Info()
                {
                    RichText = new List<RichTextBase>()
                    {
                        new RichTextText()
                        {
                            Text = new Text()
                            {
                                Content = text
                            }
                        }
                    }
                }
            };
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
