using Bogus;
using Brainbay.Characters.Contracts;

namespace Brainbay.Characters.Tests.Fakers;

internal static class CharacterFaker
{
    public static Faker<Character> Create() => new Faker<Character>()
        .CustomInstantiator(f => new Character(
            f.Random.Int(1, 10000),
            f.Person.FullName,
            f.Lorem.Word(),
            CharacterStatus.Alive,
            f.Random.Enum<CharacterGender>(),
            f.Date.Past(),
            new Uri(f.Internet.Url())
        ));
}