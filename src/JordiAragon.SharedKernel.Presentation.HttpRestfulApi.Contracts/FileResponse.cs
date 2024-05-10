namespace JordiAragon.SharedKernel.Presentation.HttpRestfulApi.Contracts
{
    public record class FileResponse(byte[] FileContents, string ContentType, string FileDownloadName);
}