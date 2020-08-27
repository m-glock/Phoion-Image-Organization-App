namespace DLuOvBamG.Services
{
    public interface IPathService
    {
        string InternalFolder { get; }
        string PublicExternalFolder { get; }
        string PrivateExternalFolder { get; }
        string DcimFolder { get; }
        string PictureFolder { get; }
    }
}
