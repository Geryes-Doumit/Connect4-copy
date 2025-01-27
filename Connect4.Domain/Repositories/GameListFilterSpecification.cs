using Connect4.Common.Contracts;
using Connect4.Common.Model;
using System;
using System.Linq.Expressions;

namespace Connect4.Domain.Queries
{
    /// <summary>
    /// Specification to filter Connect4Game entities based on a category string
    /// (e.g. "waiting" or "finished-<username>").
    /// </summary>
    public class GameListFilterSpecification : Specification<Connect4Game>
    {
        private readonly string? _categoryFilter;

        /// <summary>
        /// Initializes a new instance of the <see cref="GameListFilterSpecification"/> class.
        /// </summary>
        /// <param name="categoryFilter">
        /// A string indicating the filter type, for example:
        /// "waiting" or "finished-bob" (meaning Bob wants to see finished games).
        /// </param>
        public GameListFilterSpecification(string? categoryFilter)
        {
            _categoryFilter = categoryFilter?.Trim().ToLower();
        }

        /// <inheritdoc />
        public override Expression<Func<Connect4Game, bool>> ToExpression()
        {
            // Si aucune catégorie => renvoie tout
            if (string.IsNullOrEmpty(_categoryFilter))
            {
                return game => true;
            }

            // Cas "waiting" => game.Status == "WaitingForPlayers"
            if (_categoryFilter == "waiting")
            {
                return game => game.Status.Equals("WaitingForPlayers", StringComparison.OrdinalIgnoreCase);
            }

            // Cas "finished-<username>"
            // => game.Status == "Finished" && (game.Host == username || game.Guest == username)
            if (_categoryFilter.StartsWith("finished-"))
            {
                var parts = _categoryFilter.Split('-', 2);
                if (parts.Length == 2)
                {
                    var userName = parts[1]; // ex: "bob"

                    return game =>
                        game.Status.Equals("Finished", StringComparison.OrdinalIgnoreCase)
                        && (
                             game.Host.Equals(userName, StringComparison.OrdinalIgnoreCase)
                             || (game.Guest != null && game.Guest.Equals(userName, StringComparison.OrdinalIgnoreCase))
                           );
                }
                // format invalide => aucun résultat
                return game => false;
            }

            // Cas "playing-<username>"
            if (_categoryFilter.StartsWith("playing-"))
            {
                var parts = _categoryFilter.Split('-', 2);
                if (parts.Length == 2)
                {
                    var userName = parts[1]; // ex: "bob"
                    return game =>
                        game.Status.Equals("InProgress", StringComparison.OrdinalIgnoreCase)
                        && (
                             game.Host.Equals(userName, StringComparison.OrdinalIgnoreCase)
                             || (game.Guest != null && game.Guest.Equals(userName, StringComparison.OrdinalIgnoreCase))
                           );
                }
                // format invalide => aucun résultat
                return game => false;
            }

            // Si la catégorie n'est pas reconnue => aucun résultat
            return game => false;
        }
    }
}
