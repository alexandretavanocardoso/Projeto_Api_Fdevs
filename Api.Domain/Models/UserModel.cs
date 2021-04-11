using System;

namespace Api.Domain.Models
{
    public class UserModel
    {
        private Guid _id;
        private string _name;
        private string _email;
        private DateTime? _createAt;
        private DateTime? _updateAt;

        public Guid Id
        {
            get { return _id; }
            set { _id = value; }
        }

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public string Email
        {
            get { return _email; }
            set { _email = value; }
        }

        public DateTime? CreateAt
        {
            get { return _createAt; }
            set
            {
                _createAt = value.HasValue ? value : DateTime.UtcNow;
            }
        }

        public DateTime? UpdateAt
        {
            get { return _updateAt; }
            set { _updateAt = value; }
        }

    }
}
