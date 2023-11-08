using Api.Models.Enums;

namespace Api.Models.Dtos
{
    public class ErrorDto
    {
        public string Message { get; private set; } = null!;
        public int Code { get; set; }
        public ErrorDto(ErrorType type) 
        {
            Code = (int)type;
            SetMessage(type);
        }

        private void SetMessage(ErrorType type)
        {
            switch(type)
            {
                case ErrorType.Unexcepted:
                    Message = "Internal servers error";
                    break;
                case ErrorType.InvalidModel:
                    Message = "Missing properties in request body";
                    break;
                case ErrorType.NotFound:
                    Message = "Requested data not found";
                    break;
                case ErrorType.MissingIdClaim:
                    Message = "Missing id from jwt";
                    break;
                case ErrorType.MissingUsernameClaim:
                    Message = "Missing username from jwt";
                    break;
                case ErrorType.InvalidGuid:
                    Message = "Invalid guid";
                    break;
                case ErrorType.InvalidSchema:
                    Message = "Invalid schema values";
                    break;
                case ErrorType.InvalidId:
                    Message = "Invalid id";
                    break;
                default:
                    Message = string.Empty;
                    break;
            }
        }
    }
}
