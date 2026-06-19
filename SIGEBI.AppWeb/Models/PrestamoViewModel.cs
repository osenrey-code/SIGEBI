namespace SIGEBI.AppWeb.Models
{
    public class PrestamoViewModel
    {
        public string? RequestId { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}
