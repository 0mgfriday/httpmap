namespace _0mg.HttpMap.Scraper.Model
{
    public class PageData
    {
        public PageData()
        {
            Paths = new SortedSet<string>();
            ExternalUrls = new SortedSet<string>();
            WebSockets = new SortedSet<string>();
            FormActions = new SortedSet<string>();
            JavaScriptFiles = new SortedSet<string>();
            GraphQL = new SortedSet<string>();
            Secrets = new SortedSet<string>();
        }

        public SortedSet<string> Paths { get; }
        public SortedSet<string> ExternalUrls { get; }
        public SortedSet<string> WebSockets { get; }
        public SortedSet<string> FormActions { get; }
        public SortedSet<string> JavaScriptFiles { get; }
        public SortedSet<string> GraphQL { get; }
        public SortedSet<string> Secrets { get; }


        public void AddPaths(IEnumerable<string> paths)
        {
            Paths.UnionWith(paths);
        }

        public void AddPath(string path)
        {
            Paths.Add(path);
        }

        public void AddExternalPaths(IEnumerable<string> externalPaths)
        {
            ExternalUrls.UnionWith(externalPaths);
        }

        public void AddExternalPath(string externalPath)
        {
            ExternalUrls.Add(externalPath);
        }

        public void AddWebSockets(IEnumerable<string> websockets)
        {
            WebSockets.UnionWith(websockets);
        }

        public void AddFormActions(IEnumerable<string> formActions)
        {
            FormActions.UnionWith(formActions);
        }

        public void AddJavaScriptFiles(IEnumerable<string> javascriptFiles)
        {
            JavaScriptFiles.UnionWith(javascriptFiles);
        }

        public void AddGraphQL(IEnumerable<string> graphql)
        {
            GraphQL.UnionWith(graphql);
        }

        public void AddSecrets(IEnumerable<string> secrets)
        {
            Secrets.UnionWith(secrets);
        }
    }
}
