using ContestPark.DataAccessLayer.Interfaces;
using System;

namespace ContestPark.DataAccessLayer.Tables
{
    public abstract class EntityBase : IEntity
    {
        public DateTime ModifiedDate { get; set; } = DateTime.Now;
        public DateTime CreatedDate { get; set; } = DateTime.Now;
    }
}