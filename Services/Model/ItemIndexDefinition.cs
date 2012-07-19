namespace DotNetGroup.Services.Model
{
    using Lucene.Net.Documents;
    using Lucene.Net.Index;

    using SimpleLucene;

    public class ItemIndexDefinition : IIndexDefinition<Item>
    {
        public Document Convert(Item entity)
        {
            var document = new Document();
            document.Add(new Field("id", entity.Id, Field.Store.YES, Field.Index.NOT_ANALYZED));
            document.Add(new Field("title", entity.Title, Field.Store.NO, Field.Index.ANALYZED));
            var tagsField = new Field("tags", string.Join(", ", entity.Tags), Field.Store.NO, Field.Index.ANALYZED);
            tagsField.SetBoost(4);
            document.Add(tagsField);
            document.Add(new Field("author", entity.AuthorName, Field.Store.NO, Field.Index.NOT_ANALYZED));            
            return document;
        }

        public Term GetIndex(Item entity)
        {
            return new Term("id", entity.Id);
        }
    }
}