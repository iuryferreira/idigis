using DotNetEnv;

namespace Tests
{
    public abstract class UnitTest
    {
        protected UnitTest ()
        {
            Env.TraversePath().Load();
        }
    }
}
