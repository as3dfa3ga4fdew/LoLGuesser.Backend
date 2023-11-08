using Api.Models.Enums;

namespace Api.Models.Dtos
{
    public class StatusDto
    {
        public string Message { get; set; }
        public int Code { get; set; }

        public StatusDto() { }

        public StatusDto(ErrorType type)
        {
            Code = (int)type;
            SetErrorMessage(type);
        }

        private void SetErrorMessage(ErrorType type)
        {
            switch (type)
            {
                case ErrorType.Unexcepted:
                    Message = "Internal server error";
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
                case ErrorType.InvalidCredentials:
                    Message = "Invalid username or password";
                    break;
                case ErrorType.UsernameAlreadyTaken:
                    Message = "Username already taken";
                    break;
                default:
                    Message = string.Empty;
                    break;
            }
        }
    }
}
