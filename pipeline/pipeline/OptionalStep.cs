using System;
using System.Threading.Tasks;

namespace pipeline
{
    public class OptionalStep<TIn, TOut> : IPipelineStep<TIn, TOut> where TIn : TOut
    {
        private readonly IPipelineStep<TIn, TOut> _step;
        private readonly Func<TIn, bool> _predicate;

        public OptionalStep(Func<TIn, bool> predicate, IPipelineStep<TIn, TOut> step)
        {
            _predicate = predicate;
            _step = step;
        }

        public Task<TOut> Process(TIn input)
        {
            return _predicate(input)
                ? _step.Process(input)
                : Task.FromResult<TOut>(input);
        }
    }
}