namespace Rac.VOne.Common.Security
{
    public interface IHashAlgorithm
    {
        string Compute(string value);
    }
}
