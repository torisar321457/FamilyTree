using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FamilyTree.Model
{
    public class Halfling(string name, string family, DateTime yearOfBirth, string pathFromPatriarch, int level)
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; private set; }
        public string PathFromPatriarch { get; set; } = pathFromPatriarch;
        public string Name { get; set; } = name;
        public string Family { get; set; } = family;
        public int Level { get; set; } = level;
        public DateTime YearOfBirth { get; set; } = yearOfBirth;

    }
}
