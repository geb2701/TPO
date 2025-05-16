using System;

namespace SharedKernel.Domain
{
    public class BusinessRuleException : Exception
    {
        private IBusinessRule _brokenRule;
        public string BusinessRuleName { get; }
        public BusinessRuleException(IBusinessRule brokenRule) :
            base(brokenRule.Message)
        {
            _brokenRule = brokenRule;
            BusinessRuleName = brokenRule.GetType().Name;
        }
    }
}
