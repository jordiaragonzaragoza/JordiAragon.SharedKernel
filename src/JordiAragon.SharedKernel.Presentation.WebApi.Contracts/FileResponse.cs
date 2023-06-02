namespace JordiAragon.SharedKernel.Presentation.WebApi.Contracts
{
    public record class FileResponse(byte[] FileContents, string ContentType, string FileDownloadName);
}