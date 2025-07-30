namespace AcademiaDoZe.Domain
{
    public record Arquivo
    {
        public byte[] Dados { get; set; }

        public Arquivo(byte[] dados)
        {
            Dados = dados;
        }
    }
}
