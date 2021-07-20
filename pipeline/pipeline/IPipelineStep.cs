using System.Threading.Tasks;

namespace pipeline
{
    public interface IPipelineStep<in TIn, TOut>
    {
        Task<TOut> Process(TIn input);
    }
}