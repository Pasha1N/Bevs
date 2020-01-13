using System;

namespace Bevs.Model
{
    public class Track
    {
        private DateTime dateCreation;
        private string name;
        private string path;


        public Track(string name)
        {
            this.path = name;
            TrimName(name);
            dateCreation = DateTime.Parse(DateTime.Now.ToShortDateString());
        }

        public DateTime DateCreation => dateCreation;
        public string Name => name;
        public string Path => path;

        private void TrimName(string fullName)
        {
            int ind = fullName.LastIndexOf("\\");

            name = fullName.Substring(ind + 1);
        }
    }
}