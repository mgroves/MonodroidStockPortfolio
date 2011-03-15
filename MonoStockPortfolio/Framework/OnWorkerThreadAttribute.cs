using System.Threading;
using PostSharp.Aspects;

namespace MonoStockPortfolio.Framework
{
    public class OnWorkerThreadAttribute : MethodInterceptionAspect
    {
        public override void OnInvoke(MethodInterceptionArgs args)
        {
            ThreadPool.QueueUserWorkItem(d => args.Proceed());
        }
    }
}