using ClientApplications.Domain.Model.Client.ValueObjects;
using ClientApplications.Domain.Tests.Unit.Builders;
using FluentAssertions;

namespace ClientApplications.Domain.Tests.Unit;

// minor client must have at least one guardian

public class AddMinorRelationTests
{
    [Fact]
    public void Client_with_minor_type_must_have_at_least_one_guardian()
    {
        var client = new TestClientBuilder()
            .BuildOfMinor();

        client.IsError.Should().BeTrue();

        client.FirstError.Description.Should().Be
            (Resources.Messages.Validations.MinorClientMustHaveAtLeastOneGuardian);
    }

    [Fact]
    public void When_a_relation_is_deleted_there_must_be_at_least_one_relation_left_for_the_minor_client()
    {
        var client = new TestClientBuilder()
            .WithMinorRelation(id: 100)
            .BuildOfMinor();

        var result = client.Value.CanRelationBeDeleted();

        result.IsError.Should().BeTrue();

        result.FirstError.Description.Should().Be
            (Resources.Messages.Validations.MinorClientMustHaveAtLeastOneGuardian);
    }

    [Fact]
    public void When_a_relation_is_added_it_should_be_added_to_the_client_contact_list()
    {
        var client = new TestClientBuilder()
            .WithMinorRelation(id: 100)
            .BuildOfMinor();

        client.Value.Contacts.Should().HaveCount(1);
        client.Value.Contacts.First().Id.Value.Should().Be(100);
    }

    [Fact]
    public void When_a_relation_is_deleted_it_should_be_deleted_from_client_contact_list()
    {
        var client = new TestClientBuilder()
            .WithMinorRelation(id: 100)
            .WithMinorRelation(id: 200)
            .BuildOfMinor();

        client.Value.RemoveMinorRelation(new ClientId(100));
        client.Value.Contacts.Count.Should().Be(1);
        client.Value.Contacts.First().Id.Value.Should().Be(200);
    }
}
