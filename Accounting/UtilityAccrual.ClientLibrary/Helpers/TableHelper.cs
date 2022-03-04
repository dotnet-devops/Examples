using Blazored.LocalStorage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UtilityAccrual.ClientLibrary.Helpers
{
    public class TableHelper
    {
        private readonly ILocalStorageService _storage;
        private readonly string[] _adjustmentProps = 
        { 
            // Redacted
        };
       
        public TableHelper(ILocalStorageService storage)
        {
            AdjustmentVisibility = new();
            Size = 0;
            _storage = storage;

            foreach (var p in _adjustmentProps)
                AdjustmentVisibility.Add(p, true);
        }

        public Dictionary<string, bool> AdjustmentVisibility { get; }
        public int Size { get; private set; }
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

        public string HeaderSize() => $"font-size: { Size + 2 }px; width: 0.1%;";

        public async Task RecallLocalVariables()
        {
            foreach (var ap in _adjustmentProps)
            {
                if (await _storage.ContainKeyAsync(ap))
                {
                    AdjustmentVisibility[ap] = Convert.ToBoolean(await _storage.GetItemAsStringAsync(ap));
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

        public async Task SetAdjustmentVisibility(string prop, bool visible)
        {
            AdjustmentVisibility[prop] = visible;
            await _storage.SetItemAsStringAsync(prop, visible.ToString());
        }
    }
}
