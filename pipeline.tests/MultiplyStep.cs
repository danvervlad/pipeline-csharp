using System.Threading.Tasks;

namespace pipeline.tests
{
    public class MultiplyStep : IPipelineStep<int, int>
    {
        private readonly int _multiplier;

        public MultiplyStep(int multiplier)
        {
            _multiplier = multiplier;
        }

        public Task<int> Process(int input)
        {
            return Task.FromResult(input * _multiplier);
        }
    }
}