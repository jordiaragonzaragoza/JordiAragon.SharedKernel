namespace JordiAragon.SharedKernel.Presentation.HttpRestfulApi.Contracts
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1819:Properties should not return arrays", Justification = "It is part of a Data Transfer Object (DTO) class.")]
    public record class FileResponse(byte[] FileContents, string ContentType, string FileDownloadName);
}