using System.Reflection;
using ByteDev.Azure.Storage.IntTests.Blob;
using ByteDev.Testing;

namespace ByteDev.Azure.Storage.IntTests
{
    public abstract class IntTestBase
    {
        protected string GetTestConnectionString()
        {
            var assembly = Assembly.GetAssembly(typeof(BlobTestBase));

            var testConn = new TestConnectionString(assembly)
            {
                FilePaths = new[] {@"Z:\Dev\ByteDev.Azure.Storage.IntTests.connstring"}
            };

            return testConn.GetConnectionString();
        }
    }
}