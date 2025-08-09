namespace AcademiaDoZe.Infraestrutura.Exeption
{
    public class InfraestruturaException : Exception
    {
        public InfraestruturaException(string message) : base(message)
        {
        }
        public InfraestruturaException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
