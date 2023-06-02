namespace JordiAragon.SharedKernel.Application.Contracts
{
    public record class FileOutputDto(byte[] FileContents, string ContentType, string FileDownloadName);
}