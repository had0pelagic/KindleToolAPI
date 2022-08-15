using Notion.Client;

namespace KindleToolAPI.Util.Notion
{
    public static class Blocks
    {
        /// <summary>
        /// Returns paragraph text block
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static ParagraphBlock GetParagraphBlock(string text)
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
        /// Returns heading one block
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static HeadingOneBlock GetHeadingOneBlock(string text)
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
    }
}
