using System.Threading;
using PostSharp.Aspects;

namespace MonoStockPortfolio.Framework
{
    public class OnWorkerThreadAttribute : MethodInterceptionAspect
    {
        public static IThreadingService ThreadingService;

        public override void OnInvoke(MethodInterceptionArgs args)
        {
            if(ThreadingService == null) ThreadingService = new ThreadingService();
            ThreadingService.QueueUserWorkItem(d => args.Invoke(args.Arguments));
        }
    }

    // this is kinda fail, but it helps with testing to inject in a "non threaded" service
    // and I suppose it might make refactoring easier maybe...? just go with it
    public interface IThreadingService
    {
        void QueueUserWorkItem(WaitCallback func);
    }

    public class ThreadingService : IThreadingService
    {
        public void QueueUserWorkItem(WaitCallback waitCallback)
        {
            ThreadPool.QueueUserWorkItem(waitCallback);
        }
    }
}