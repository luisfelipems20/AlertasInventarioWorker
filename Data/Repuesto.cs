namespace AlertasInventarioWorker.Data
{
    public class Repuesto
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public int CantidadStock { get; set; }
        public int NivelCritico { get; set; }
        public bool AlertaEnviada { get; set; }
    }
}