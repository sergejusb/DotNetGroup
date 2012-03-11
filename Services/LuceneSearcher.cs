using System.Collections.Generic;
using System.IO;
using System.Linq;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.Documents;
using Lucene.Net.QueryParsers;
using Lucene.Net.Search;
using Lucene.Net.Util;
using Services.Model;
using SimpleLucene;
using SimpleLucene.Impl;

namespace Services
{
    public class LuceneSearcher
    {
        private readonly ISearchService searchService;

        public LuceneSearcher(ISearchService searchService)
        {
            this.searchService = searchService;
        }

        public LuceneSearcher(string indexPath)
        {
            var searcher = new DirectoryIndexSearcher(new DirectoryInfo(indexPath));
            searchService = new SearchService(searcher);           
        }

        public virtual IList<string> Search(string keywords)
        {
            var query = new ItemQuery(keywords);
            IList<string> results = searchService.SearchIndex(query.Query, new ItemResultDefinition()).Results.ToList();
            return results;
        }
    }

    public class ItemResultDefinition : IResultDefinition<string>
    {
        public string Convert(Document document)
        {
            return document.GetValues("id")[0];
        }
    }

    public class ItemQuery : QueryBase
    {
        public ItemQuery(Query query) : base(query)
        {            
        }

        public ItemQuery(string keywords)
        {
            if (string.IsNullOrEmpty(keywords))
            {
                return;
            }

            string[] fields = { "name", "title", "author", "tags" };                            
            var parser = new MultiFieldQueryParser(Version.LUCENE_29, fields, new StandardAnalyzer(Version.LUCENE_29));
            Query multiQuery;

            // we try to allow complex queries and in case they fail,
            // we remove complex stuff and still return result
            try
            {
                multiQuery = parser.Parse(keywords);
            }
            catch(ParseException ex)
            {
                multiQuery = parser.Parse(QueryParser.Escape(keywords));
            }

            this.AddQuery(multiQuery);
        }       
    }
}