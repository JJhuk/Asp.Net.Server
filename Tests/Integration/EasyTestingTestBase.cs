using System;
using Server;
using Wd3w.AspNetCore.EasyTesting;

namespace Tests.Integration
{
    public abstract class EasyTestingTestBase : IDisposable
    {
        // ReSharper disable once InconsistentNaming
        protected readonly SystemUnderTest<Startup> SUT;

        protected EasyTestingTestBase()
        {
            SUT = new SystemUnderTest<Startup>();
        }

        public void Dispose()
        {
            SUT?.Dispose();
        }
    }
}