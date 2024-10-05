namespace DBI.DataCenter.Exceptions;

public enum DataCenterError
{
    RawDataFileNotFound
}

public static class DataCenterErrorToStringExtensions
{
    public static string ToErrorMessage(this DataCenterError error) =>
        error switch
        {
            DataCenterError.RawDataFileNotFound => "The requested raw data file could not be found.",
            _ => "An unexpected error occured."
        };
}
