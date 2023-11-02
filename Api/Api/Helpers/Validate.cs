namespace Api.Helpers
{
    public class Validate
    {
        private const int MinUsernameLength = 5;
        private const int MaxUsernameLength = 16;

        private const string Alphabet = "abcdefghijklmnopqrstuvwxyz";
        private const string AlphabetUpper = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        private const string Numbers = "0123456789";
        private const string Special = "~!@#$%^&*()-_+={}][|\\`,./?;:'\"<>";

        public static bool Password(string password)
        {
            if (password == null) return false;

            if (password.Length <= 7) return false;

            if (password.Length >= 50) return false;

            if (password.All(x => (Alphabet + AlphabetUpper + Numbers + Special).Contains(x)) == false) return false;

            if (password.Any(x => Alphabet.Contains(x)) == false ||
                password.Any(x => AlphabetUpper.Contains(x)) == false ||
                password.Any(x => Numbers.Contains(x)) == false ||
                password.Any(x => Special.Contains(x)) == false)
                return false;

            return true;
        }

        public static bool Username(string username)
        {
            if (username == null) return false;

            if (username.Length <= 5) return false;

            if (username.Length >= 16) return false;

            if (username.All(x => (Alphabet + AlphabetUpper + Numbers).Contains(x)) == false) return false;

            return true;
        }
    }
}
