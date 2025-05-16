using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace SharedKernel.Domain
{
    public static class Permissions
    {
        public const string CanDeleteExampleWithStringId = nameof(CanDeleteExampleWithStringId);
        public const string CanUpdateExampleWithStringId = nameof(CanUpdateExampleWithStringId);
        public const string CanAddExampleWithStringId = nameof(CanAddExampleWithStringId);
        public const string CanReadExampleWithStringId = nameof(CanReadExampleWithStringId);

        public static List<string> List()
        {
            return typeof(Permissions)
                .GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)
                .Where(fi => fi.IsLiteral && !fi.IsInitOnly && fi.FieldType == typeof(string))
                .Select(x => (string)x.GetRawConstantValue())
                .ToList();
        }
    }
}