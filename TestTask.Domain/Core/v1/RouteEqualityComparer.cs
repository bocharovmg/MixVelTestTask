using System.Diagnostics.CodeAnalysis;
using TestTask.Domain.Contracts.v1.Dtos;


namespace TestTask.Domain.Core.v1
{
    public class RouteEqualityComparer : EqualityComparer<Route>
    {
        public override bool Equals(Route? x, Route? y)
        {
            if (x != null && y != null)
            {
                return GetHashCode(x) == GetHashCode(y);
            }
            else if (x == null && y == null)
            {
                return true;
            }

            return false;
        }

        public override int GetHashCode([DisallowNull] Route obj)
        {
            return obj.GetHashCode();
        }
    }
}
