namespace card_index_DAL.Entities
{
    /// <summary>
    /// Base entity for all objects stored in DB,
    /// contains only identifier
    /// </summary>
    public class BaseEntity
    {
        /// <summary>
        /// Entity identifier, unique
        /// </summary>
        public int Id { get; set; }
    }
}
