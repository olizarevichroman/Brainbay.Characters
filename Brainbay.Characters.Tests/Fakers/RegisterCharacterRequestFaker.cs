using Bogus;
using Brainbay.Characters.Contracts;

namespace Brainbay.Characters.Tests.Fakers;

internal static class RegisterCharacterRequestFaker
{
    public static Faker<RegisterCharacterRequest> Create() => new Faker<RegisterCharacterRequest>()
        .CustomInstantiator(f => new RegisterCharacterRequest(
            f.Person.FullName,
            f.Lorem.Word(),
            CharacterStatus.Alive,
            f.Random.Enum<CharacterGender>(),
            new Uri(f.Internet.Url())
        ));
}