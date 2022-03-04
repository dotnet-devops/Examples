using Blazored.LocalStorage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorClientLibrary.Components.Helpers
{
    public class TableHelper
    {
        private readonly ILocalStorageService _storage;
        private string[] properties;

        public TableHelper(ILocalStorageService storage)
        {
            Visibility = new();
            Size = 0;
            _storage = storage;
            Filter = string.Empty;
        }

        public void SetVisibilityTokens(string[] props)
        {
            properties = props;
            foreach (var p in properties)
                Visibility.Add(p, true);
        }

        
        public string Filter { get; set; }
        public int Size { get; private set; }
        public Dictionary<string, bool> Visibility { get; }

        public async Task FontBigger()
        {
            if (Size < 18)
            {
                Size += 1;
                await _storage.SetItemAsStringAsync("fontsize", Size.ToString());
            }
        }

        public string FontSize() => $"font-size: { Size }px";

        public async Task FontSmaller()
        {
            if (Size > 5)
            {
                Size -= 1;
                await _storage.SetItemAsStringAsync("fontsize", Size.ToString());
            }
        }

        public string HeaderSize() => $"font-size: { Size + 2 }px; width: 1%;";

        public async Task RecallLocalVariables()
        {
            foreach (var ap in properties)
            {
                if (await _storage.ContainKeyAsync(ap))
                {
                    Visibility[ap] = Convert.ToBoolean(await _storage.GetItemAsStringAsync(ap));
                }
                else
                {
                    await _storage.SetItemAsStringAsync(ap, true.ToString());
                }
            }

            if (await _storage.ContainKeyAsync("fontsize"))
            {
                Size = Convert.ToInt32(await _storage.GetItemAsStringAsync("fontsize"));
            }
            else
            {
                Size = 11;
                await _storage.SetItemAsStringAsync("fontsize", Size.ToString());
            }
        }

        public async Task SetVisibility(string prop, bool visible)
        {
            Visibility[prop] = visible;
            await _storage.SetItemAsStringAsync(prop, visible.ToString());
        }
    }
}
