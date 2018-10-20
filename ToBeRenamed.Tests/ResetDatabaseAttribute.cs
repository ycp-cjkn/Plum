using System.Reflection;
using Xunit.Sdk;

namespace ToBeRenamed.Tests
{
    public class ResetDatabaseAttribute : BeforeAfterTestAttribute
    {
        public override void Before(MethodInfo methodUnderTest)
        {
            // Must be called synchronously. Otherwise, the test will start then
            // ResetDatabase() may complete in the middle of the test.
            DatabaseFixture.ResetDatabase(DatabaseFixture.FullResetCheckpoint).GetAwaiter().GetResult();
        }
    }
}
