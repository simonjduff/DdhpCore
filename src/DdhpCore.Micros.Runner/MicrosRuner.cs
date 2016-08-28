using System.Threading;
using System.Threading.Tasks;

namespace DdhpCore.Micros.Runner
{
    public class MicrosRuner
    {
        private readonly CancellationTokenSource _cancellationTokenSource;

        public MicrosRuner()
        {
            _cancellationTokenSource = new CancellationTokenSource();
        }

        public void Run()
        {
            Task.Run(RunJobs, _cancellationTokenSource.Token);
        }

        private async Task RunJobs()
        {
            var cancellationToken = _cancellationTokenSource.Token;

            while (!cancellationToken.IsCancellationRequested)
            {
                // Read from queues here
                await Task.Delay(1000, cancellationToken);
            }
        }

        public void StopApplication()
        {
            _cancellationTokenSource.Cancel();
        }
    }
}
