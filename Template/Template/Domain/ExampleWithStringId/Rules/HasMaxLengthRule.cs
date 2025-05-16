using SharedKernel.Domain;

namespace Template.Domain.ExampleWithStringId.Rules
{
    public class HasMaxLengthRule : IBusinessRule
    {
        private readonly bool _isBroken;
        private readonly string _message;


        public HasMaxLengthRule(string stringValue, int maxLength)
        {
            _isBroken = IsRuleBroken(stringValue, maxLength);

        }
        private static bool IsRuleBroken(string value, int maxLength)
        {
            return value.Length > maxLength;
        }
        public bool IsBroken => _isBroken;
        public string Message => _message;
    }
}
