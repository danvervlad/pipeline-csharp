using System.Collections.Generic;
using System.Threading.Tasks;

namespace pipeline
{
    public class AggregateStep<TIn, TOut> : IPipelineStep<TIn, IList<TOut>>
    {
        private readonly IPipelineStep<TIn, TOut>[] _steps;

        public AggregateStep(params IPipelineStep<TIn, TOut>[] steps)
        {
            _steps = steps;
        }

        public async Task<IList<TOut>> Process(TIn input)
        {
            var results = new List<TOut>();
            foreach (var step in _steps)
            {
                results.Add(await step.Process(input));
            }

            return results;
        }
    }
}