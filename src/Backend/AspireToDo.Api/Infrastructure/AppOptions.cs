namespace AspireToDo.Api.Infrastructure;

public class StorageOptions
{
    public const string SectionName = "Storage";
    public string AzureWebJobsStorage { get; set; }
}

public class AzureADOptions
{
    public const string SectionName = "AzureAD";
}