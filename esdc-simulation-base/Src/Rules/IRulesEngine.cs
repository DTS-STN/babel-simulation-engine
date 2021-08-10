namespace esdc_simulation_base.Src.Rules
{
    public interface IRulesEngine
    {
        T Execute<T>(string endpoint, object rulesRequest);
    }
}