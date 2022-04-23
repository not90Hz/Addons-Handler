using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SDKAddons
{
    public class ButtonAPI
    {
        private string name;
        private string link;
        private string description;
        private string category;
        private bool installed;

        public ButtonAPI(string name, string link, string description, string category, bool installed)
        {
            this.name = name;
            this.link = link;
            this.description = description;
            this.category = category;
            this.installed = installed;
        }

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public string Link
        {
            get { return link; }
            set { link = value; }
        }

        public string Description
        {
            get { return description; }
            set { description = value; }
        }

        public string Category
        {
            get { return category; }
            set { category = value; }
        }

        public bool Installed
        {
            get { return installed; }
            set { installed = value; }
        }
    }

}