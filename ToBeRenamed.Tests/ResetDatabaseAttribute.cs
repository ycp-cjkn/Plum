using System.Reflection;
using Xunit.Sdk;

namespace ToBeRenamed.Tests
{
    public class ResetDatabaseAttribute : BeforeAfterTestAttribute
    {
        public override async void Before(MethodInfo methodUnderTest)
        {
            await DatabaseFixture.ResetDatabase(DatabaseFixture.FullResetCheckpoint);
        }
    }
}
