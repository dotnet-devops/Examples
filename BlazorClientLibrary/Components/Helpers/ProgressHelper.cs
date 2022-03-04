using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorClientLibrary.Components.Helpers;
public class ProgressHelper
{
    public ProgressHelper()
    {
        Maximum = 0;
        Current = 0;
        Text = string.Empty;
    }

    public bool Loading { get => Maximum != 0 && Current != Maximum; }
    public int Maximum { get; set; }
    public int Current { get; set; }
    public string Text { get; set; }

    public event EventHandler ProgressChanged;

    /// <summary>
    /// Automatically sets loading to true. Sets current += 1.
    /// </summary>
    /// <param name="text"></param>
    /// <param name="loading"></param>
    public void Next(string text = "")
    {
        Text = text;
        if (Current != Maximum)
            Current += 1;

        if (Current == Maximum)
        {
            Current = 0;
            Maximum = 0;
            Text = string.Empty;
        }
            
        ProgressChanged?.Invoke(this, EventArgs.Empty);
    }

    public void RecallProgress(int values)
    {
        if (Current >= values)
            Current -= values;

        if (Maximum >= values)
            Maximum -= values;

        ProgressChanged?.Invoke(this, EventArgs.Empty);
    }
}