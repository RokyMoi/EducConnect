using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HtmlAgilityPack;
using System.Text.RegularExpressions;

namespace EduConnect.Helpers
{
    public class HtmlSentenceExtractor
    {
        public static List<string> ExtractSentencesWithKeywords(string html, string keyword)
        {
            var sentencesWithKeyword = new List<string>();

            var doc = new HtmlDocument();

            doc.LoadHtml(html);


            var nodes = doc.DocumentNode.SelectNodes("//p|//li|//blockquote|//h1|//h2|//h3|//h4");

            if (nodes == null)
            {
                return sentencesWithKeyword;
            }

            foreach (var node in nodes)
            {
                string plainText = HtmlEntity.DeEntitize(node.InnerText);
                var sentences = Regex.Split(plainText, @"(?<=[\.!\?])\s+");

                foreach (var sentence in sentences)
                {
                    if (sentence.Contains(keyword, StringComparison.OrdinalIgnoreCase))
                    {
                        sentencesWithKeyword.Add(sentence.Trim());
                    }
                }

            }

            return sentencesWithKeyword;
        }

        public static string HighlightKeywordInHtml(string html, string keyword)
        {
            var doc = new HtmlDocument();
            doc.LoadHtml(html);

            foreach (var node in doc.DocumentNode.DescendantsAndSelf().Where(x => x.NodeType == HtmlNodeType.Text && !string.IsNullOrWhiteSpace(x.InnerText)))
            {
                node.InnerHtml = Regex.Replace(
                    node.InnerHtml,
                    $@"\b({Regex.Escape(keyword)})\b",
                "<mark>$1</mark>",
                RegexOptions.IgnoreCase
                );
            }

            return doc.DocumentNode.OuterHtml;
        }
    }
}