using Microsoft.Azure.Documents.Client;
using System;
using System.IO.Pipes;

namespace solution3
{
    public class CosmosDBClient
    {
        private static CosmosDBClient _instance = null;
        private static DocumentClient client = null;
        private static Object _mutex = new Object();

        private CosmosDBClient(string url, string key)
        {
            client = new DocumentClient(new Uri(url), key);
        }

        public DocumentClient GetClient()
        {
            return client;
        }

        public static CosmosDBClient GetInstance(string arg1, string arg2)
        {
            if (_instance == null)
            {
                lock (_mutex) // now I can claim some form of thread safety...
                {
                    if (_instance == null)
                    {
                        _instance = new CosmosDBClient(arg1, arg2);
                    }
                }
            }

            return _instance;
        }
    }
}