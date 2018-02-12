﻿using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.Contracts;
using System.Web;

namespace Shsict.Peccancy.Service.Model
{
    public abstract class Entity<TKey> : IEntity where TKey : struct
    {
        protected virtual string GenerateKey()
        {
            return KeyGenerator.Generate();
        }

        public override string ToString()
        {
            return Key;
        }

        private static class KeyGenerator
        {
            public static string Generate()
            {
                return Generate(Guid.NewGuid().ToString("D").Substring(24));
            }

            private static string Generate(string input)
            {
                Contract.Requires(!string.IsNullOrWhiteSpace(input));
                return HttpUtility.UrlEncode(input.Replace(" ", "_").Replace("-", "_").Replace("&", "and"));
            }
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof(Entity<TKey>)) return false;
            return Equals((Entity<TKey>)obj);
        }

        public bool Equals(Entity<TKey> other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            if (other.GetType() != GetType()) return false;

            if (default(TKey).Equals(ID) || default(TKey).Equals(other.ID))
                return Equals(other.Key, Key);

            return other.ID.Equals(ID);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                if (default(TKey).Equals(ID))
                    return Key.GetHashCode() * 397;

                return ID.GetHashCode();
            }
        }

        public static bool operator ==(Entity<TKey> left, Entity<TKey> right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(Entity<TKey> left, Entity<TKey> right)
        {
            return !Equals(left, right);
        }

        #region Members and Properties

        [Key]
        public virtual TKey ID
        {
            get
            {
                if (typeof(TKey) == typeof(Guid))
                {
                    if (_id == null || (_id != null && _id.Equals(Guid.Empty)))
                    {
                        _id = Guid.NewGuid();
                    }
                }

                return (TKey?)_id ?? default(TKey);
            }
            set { _id = value; }
        }

        private object _id;


        [Required, StringLength(50), NotMapped]
        public virtual string Key
        {
            get { return _key = _key ?? GenerateKey(); }
            protected set { _key = value; }
        }

        private string _key;

        #endregion
    }

    public interface IEntity
    {
        /// <summary>
        ///     The entity's unique (and URL-safe) public identifier
        /// </summary>
        /// <remarks>
        ///     This is the identifier that should be exposed via the web, etc.
        /// </remarks>
        string Key { get; }
    }
}