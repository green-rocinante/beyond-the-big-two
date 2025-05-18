using System;

namespace BeyondtheBigTwo
{
    public interface IFilterable
    {
        bool MatchesFilter(UserFilter filter);
    }
}

