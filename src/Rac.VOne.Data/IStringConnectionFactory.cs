namespace Rac.VOne.Data
{
    public interface IStringConnectionFactory
    {
        IConnectionFactory Create(string connectionString);
    }
}
