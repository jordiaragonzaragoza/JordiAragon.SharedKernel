namespace JordiAragon.SharedKernel.Application.Contracts
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1819:Properties should not return arrays", Justification = "It is part of a Data Transfer Object (DTO) class.")]
    public record class FileOutputDto(byte[] FileContents, string ContentType, string FileDownloadName);
}