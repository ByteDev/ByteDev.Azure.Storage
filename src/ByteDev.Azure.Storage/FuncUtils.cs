using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ByteDev.Azure.Storage
{
    internal static class FuncUtils
    {
        public static async Task WhenAllAsync(Func<string, Task> func, IEnumerable<string> items, int maxRunningTasks = 100)
        {
            if (items == null)
                throw new ArgumentNullException(nameof(items));

            var tasks = new List<Task>();

            foreach (var item in items)
            {
                tasks.Add(func(item));

                if (tasks.Count == maxRunningTasks)
                {
                    await Task.WhenAll(tasks);
                    tasks.Clear();
                }
            }

            await Task.WhenAll(tasks);
        }
    }
}