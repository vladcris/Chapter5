using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LoyaltyProgram.Users;

public record LoyaltyProgramUser(int Id,
    string Name,
    int LoyaltyPoints,
    LoyaltyProgramSettings Settings);

public record LoyaltyProgramSettings()
{
    public string[] Interests { get; init; } = Array.Empty<string>();

    public LoyaltyProgramSettings(string[] interests) : this()
    {
        Interests = interests;
    }
}

