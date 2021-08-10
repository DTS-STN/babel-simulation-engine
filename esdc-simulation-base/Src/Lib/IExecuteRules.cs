using esdc_simulation_base.Src.Classes;

namespace esdc_simulation_base.Src.Lib
{
    public interface IExecuteRules<T, U> 
        where T : ISimulationCase
        where U : IPerson
    {
        decimal Execute(T rule, U person);
    }
}