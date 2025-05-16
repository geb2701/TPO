using System;

namespace SharedKernel
{
    public static class Consts
    {
        public static readonly DateTime FechaMax = new DateTime(2900, 01, 01);

        public static class Testing
        {
            public const string IntegrationTestingEnvName = "LocalIntegrationTesting";
            public const string FunctionalTestingEnvName = "LocalFunctionalTesting";
        }

    }
}