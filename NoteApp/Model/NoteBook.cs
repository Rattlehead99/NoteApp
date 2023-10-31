using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using NoteApp.Model.Interfaces;
using SQLite;

namespace NoteApp.Model
{
    public class NoteBook : IIdentifiable
    {
       // [PrimaryKey, AutoIncrement]
        public string Id { get; set; }
       // [Indexed]
        public string UserId { get; set; }
        public string Name { get; set; }
    }
}
