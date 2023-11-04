using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace Budget.Model
{
    public class Wallet
    {
        int id;
        float amount;
        string name;
        bool visible;

        public Wallet() { }

        public Wallet(int id, float amount, string name, bool visible)
        {
            this.Id = id;
            this.Amount = amount;
            this.Name = name;
            this.Visible = visible;
        }

        [PrimaryKey, AutoIncrement]
        public int Id { get => id; set => id = value; }
        public float Amount { get => amount; set => amount = value; }
        public string Name { get => name; set => name = value; }
        public bool Visible { get => visible; set => visible = value; }

    }
}
