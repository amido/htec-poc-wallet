using System;

namespace Htec.Poc.API.Models.Responses;

/// <summary>
/// Response model representing a search result item in the SearchWallet api endpoint
/// </summary>
public class SearchWalletResultItem
{
    /// <example>d290f1ee-6c54-4b01-90e6-d701748f0851</example>
    public Guid Id { get; set; }

    /// <example>Wallet name</example>
    public string Name { get; set; }

    /// <example>Wallet description</example>
    public string Description { get; set; }

    /// <summary>
    /// Represents the status of the wallet. False if disabled
    /// </summary>
    public bool Enabled { get; set; }
}
