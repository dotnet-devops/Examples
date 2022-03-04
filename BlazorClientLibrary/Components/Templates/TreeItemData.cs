using MudBlazor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorClientLibrary.Components.Templates
{
    public class TreeItemData
    {
        private string _icon;
        public TreeItemData(string title, string path, string icon)
        {
            Title = title;
            Icon = icon;
            TreeItems = new();
            Path = path;
        }

        public string Title { get; set; }
        public string Path { get; set; }
        public string Icon 
        { 
            get 
            {
                if (_icon == Icons.Filled.House)
                    return _icon;

                if (IsExpanded)
                    return Icons.Filled.Folder;
                else
                    return Icons.Filled.FolderOpen;
            }
            set { _icon = value; }
        }

        public bool IsExpanded { get; set; }

        public HashSet<TreeItemData> TreeItems { get; set; }
        public override string ToString() => Title;
    }
}
