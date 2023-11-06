namespace Api.Models.Enums
{
    public enum ErrorType
    {
        Unexcepted = 0,
        InvalidModel = 1,
        NotFound = 2,
        MissingIdClaim = 3,
        MissingUsernameClaim = 4,
        InvalidGuid = 5,
        InvalidSchema = 6,
        InvalidId = 7
    }
}
