using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NoteApp.Model.Interfaces;
using SQLite;

namespace NoteApp.Model
{
    public class Note : IIdentifiable

    {
    //[PrimaryKey, AutoIncrement]
    public string Id { get; set; }

    //[Indexed]
    public string NotebookId { get; set; }
    public string Title { get; set; }
    public DateTime CreatedOn { get; set; }
    public DateTime UpdatedOn { get; set; }
    public string FileLocation { get; set; }
    }
}
