using System.Threading.Tasks;

namespace pipeline
{
    public class Pipeline<TIn, TOut>
    {
        public static Pipeline<TIn, TIn> Create()
        {
            return new Pipeline<TIn, TIn>(new InputPipelineStep<TIn>());
        }

        private readonly IPipelineStep<TIn, TOut> _currentHandler;

        private Pipeline(IPipelineStep<TIn, TOut> currentHandler)
        {
            _currentHandler = currentHandler;
        }

        public Pipeline<TIn, TOut2> Step<TOut2>(IPipelineStep<TOut, TOut2> newHandler)
        {
            return new Pipeline<TIn, TOut2>(new ConnectionPipelineStep<TIn, TOut, TOut2>(_currentHandler, newHandler));
        }

        public Task<TOut> Execute(TIn input)
        {
            return _currentHandler.Process(input);
        }

        private class InputPipelineStep<TIIn> : IPipelineStep<TIIn, TIIn>
        {
            public Task<TIIn> Process(TIIn input)
            {
                return Task.FromResult(input);
            }
        }

        private class ConnectionPipelineStep<TCIn, TCOut, TCOut2> : IPipelineStep<TCIn, TCOut2>
        {
            private readonly IPipelineStep<TCIn, TCOut> _firstPipelineStep;
            private readonly IPipelineStep<TCOut, TCOut2> _secondPipelineStep;

            public ConnectionPipelineStep(
                IPipelineStep<TCIn, TCOut> firstPipelineStep,
                IPipelineStep<TCOut, TCOut2> secondPipelineStep)
            {
                _firstPipelineStep = firstPipelineStep;
                _secondPipelineStep = secondPipelineStep;
            }

            public async Task<TCOut2> Process(TCIn input)
            {
                var output = await _firstPipelineStep.Process(input);
                return await _secondPipelineStep.Process(output);
            }
        }
    }
}