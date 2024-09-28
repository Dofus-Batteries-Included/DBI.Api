using NSwag.Generation.Processors;
using NSwag.Generation.Processors.Contexts;

namespace Server.Common.OpenApi;

class SortTagsInDocument : IDocumentProcessor
{
    public void Process(DocumentProcessorContext context) => context.Document.Tags = context.Document.Tags.OrderBy(t => t.Name).ToList();
}
